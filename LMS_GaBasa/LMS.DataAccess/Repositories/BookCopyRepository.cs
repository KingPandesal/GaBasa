using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class BookCopyRepository : IBookCopyRepository
    {
        private readonly DbConnection _db;

        public BookCopyRepository() : this(new DbConnection()) { }

        public BookCopyRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Adds a BookCopy. If copy.AccessionNumber is null/empty this method will
        /// generate a unique accession using the DB identity (CopyID) inside a transaction
        /// to avoid race conditions.
        /// Returns the new CopyID.
        /// </summary>
        public int Add(BookCopy copy)
        {
            if (copy == null)
                throw new ArgumentNullException(nameof(copy));

            // If caller already provided an accession number, do a simple insert.
            if (!string.IsNullOrWhiteSpace(copy.AccessionNumber))
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"INSERT INTO [BookCopy] 
                        (BookID, AccessionNumber, Status, Location, barcode, DateAdded, AddedByID) 
                        VALUES (@BookID, @AccessionNumber, @Status, @Location, @BarcodeImage, @DateAdded, @AddedByID);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    AddParameter(cmd, "@BookID", DbType.Int32, copy.BookID, 0);
                    AddParameter(cmd, "@AccessionNumber", DbType.String, copy.AccessionNumber, 50);
                    AddParameter(cmd, "@Status", DbType.String, copy.Status ?? "Available", 50);
                    AddParameter(cmd, "@Location", DbType.String, copy.Location, 100);
                    AddParameter(cmd, "@BarcodeImage", DbType.String, copy.Barcode, 500);
                    AddParameter(cmd, "@DateAdded", DbType.DateTime, copy.DateAdded == default(DateTime) ? (object)DateTime.Now : copy.DateAdded, 0);
                    AddParameter(cmd, "@AddedByID", DbType.Int32, copy.AddedByID, 0);

                    return (int)cmd.ExecuteScalar();
                }
            }

            // Otherwise generate accession atomically using identity (CopyID) to avoid concurrency/race.
            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Read resource type from Book (to determine prefix)
                        string resourceTypeStr = null;
                        using (var cmdGet = conn.CreateCommand())
                        {
                            cmdGet.Transaction = tran;
                            cmdGet.CommandText = "SELECT ResourceType FROM [Book] WHERE BookID = @BookID";
                            AddParameter(cmdGet, "@BookID", DbType.Int32, copy.BookID, 0);

                            using (var reader = cmdGet.ExecuteReader())
                            {
                                if (reader.Read() && !reader.IsDBNull(0))
                                    resourceTypeStr = reader.GetString(0);
                            }
                        }

                        // 2) Insert row WITHOUT AccessionNumber to obtain identity (CopyID)
                        int newId;
                        DateTime dateAdded = copy.DateAdded == default(DateTime) ? DateTime.Now : copy.DateAdded;
                        using (var cmdInsert = conn.CreateCommand())
                        {
                            cmdInsert.Transaction = tran;
                            cmdInsert.CommandText = @"INSERT INTO [BookCopy] 
                                (BookID, Status, Location, barcode, DateAdded, AddedByID) 
                                VALUES (@BookID, @Status, @Location, @BarcodeImage, @DateAdded, @AddedByID);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            AddParameter(cmdInsert, "@BookID", DbType.Int32, copy.BookID, 0);
                            AddParameter(cmdInsert, "@Status", DbType.String, copy.Status ?? "Available", 50);
                            AddParameter(cmdInsert, "@Location", DbType.String, copy.Location, 100);
                            AddParameter(cmdInsert, "@BarcodeImage", DbType.String, copy.Barcode, 500);
                            AddParameter(cmdInsert, "@DateAdded", DbType.DateTime, dateAdded, 0);
                            AddParameter(cmdInsert, "@AddedByID", DbType.Int32, copy.AddedByID, 0);

                            newId = (int)cmdInsert.ExecuteScalar();
                        }

                        // 3) Compute accession using prefix + year + sequence derived from newId
                        string prefix = MapResourceTypeToPrefix(resourceTypeStr);
                        int year = dateAdded.Year;

                        // Use identity (newId) as the unique per-insert sequence number.
                        // Format with at least 4 digits as before.
                        string accession = $"{prefix}-{year}-{newId:D4}";

                        // 4) Update the inserted row with the generated accession
                        using (var cmdUpdate = conn.CreateCommand())
                        {
                            cmdUpdate.Transaction = tran;
                            cmdUpdate.CommandText = @"UPDATE [BookCopy] 
                                                      SET AccessionNumber = @AccessionNumber
                                                      WHERE CopyID = @CopyID";

                            AddParameter(cmdUpdate, "@AccessionNumber", DbType.String, accession, 50);
                            AddParameter(cmdUpdate, "@CopyID", DbType.Int32, newId, 0);

                            cmdUpdate.ExecuteNonQuery();
                        }

                        tran.Commit();

                        // Populate the copy object with generated accession if caller expects it
                        copy.AccessionNumber = accession;
                        copy.DateAdded = dateAdded;
                        copy.CopyID = newId;

                        return newId;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        public List<BookCopy> GetByBookId(int bookId)
        {
            var copies = new List<BookCopy>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT CopyID, BookID, AccessionNumber, Status, Location, barcode, DateAdded, AddedByID 
                                    FROM [BookCopy] WHERE BookID = @BookID";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        copies.Add(new BookCopy
                        {
                            CopyID = reader.GetInt32(0),
                            BookID = reader.GetInt32(1),
                            AccessionNumber = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Status = reader.IsDBNull(3) ? "Available" : reader.GetString(3),
                            Location = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Barcode = reader.IsDBNull(5) ? null : reader.GetString(5),
                            DateAdded = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),
                            AddedByID = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                        });
                    }
                }
            }

            return copies;
        }

        /// <summary>
        /// Legacy method kept for compatibility; not used by new Add path.
        /// </summary>
        public string GenerateAccessionNumber(int bookId, DateTime dateAdded)
        {
            // Keep behavior for compatibility if some code still calls it:
            // Map resource type then use current count-based approach (may race).
            string resourceTypeStr = null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT ResourceType FROM [Book] WHERE BookID = @BookID";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                        resourceTypeStr = reader.GetString(0);
                }
            }

            string prefix = MapResourceTypeToPrefix(resourceTypeStr);

            int copyCount = 0;
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(*) FROM [BookCopy] WHERE BookID = @BookID AND YEAR(DateAdded) = @Year";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);
                AddParameter(cmd, "@Year", DbType.Int32, dateAdded.Year, 0);
                copyCount = (int)cmd.ExecuteScalar();
            }

            int nextNumber = copyCount + 1;
            int year = dateAdded.Year;

            return $"{prefix}-{year}-{nextNumber:D4}";
        }

        public bool UpdateBarcodeImage(string accessionNumber, string barcodeImagePath)
        {
            if (string.IsNullOrWhiteSpace(accessionNumber))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"UPDATE [BookCopy] 
                                    SET barcode = @BarcodeImage 
                                    WHERE AccessionNumber = @AccessionNumber";

                AddParameter(cmd, "@BarcodeImage", DbType.String, barcodeImagePath, 500);
                AddParameter(cmd, "@AccessionNumber", DbType.String, accessionNumber, 50);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private string MapResourceTypeToPrefix(string resourceTypeStr)
        {
            if (string.IsNullOrWhiteSpace(resourceTypeStr))
                return "BK";

            switch (resourceTypeStr.Trim())
            {
                case "EBook":
                case "E-Book":
                    return "EB";
                case "Thesis":
                case "Theses":
                    return "TH";
                case "AV":
                case "Audio-Visual":
                    return "AV";
                case "Periodical":
                case "Periodicals":
                case "Periodical / Magazine":
                    return "PR";
                case "PhysicalBook":
                case "Book":
                default:
                    return "BK";
            }
        }

        private void AddParameter(IDbCommand cmd, string name, DbType type, object value, int size)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.DbType = type;
            if (size > 0) p.Size = size;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}
