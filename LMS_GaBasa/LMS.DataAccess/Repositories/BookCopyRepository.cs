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
        /// to avoid race conditions and keep AccessionNumber NOT NULL.
        /// Returns the new CopyID.
        /// </summary>
        public int Add(BookCopy copy)
        {
            if (copy == null)
                throw new ArgumentNullException(nameof(copy));

            // Helper to map AddedByID -> DB null when not provided
            object addedByValue = copy.AddedByID > 0 ? (object)copy.AddedByID : null;

            // If caller already provided an accession number, do a simple insert.
            if (!string.IsNullOrWhiteSpace(copy.AccessionNumber))
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"INSERT INTO [BookCopy] 
                        (BookID, AccessionNumber, Status, Location, barcode, dateAdded, addedByID) 
                        VALUES (@BookID, @AccessionNumber, @Status, @Location, @Barcode, @DateAdded, @AddedByID);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    AddParameter(cmd, "@BookID", DbType.Int32, copy.BookID, 0);
                    AddParameter(cmd, "@AccessionNumber", DbType.String, copy.AccessionNumber, 30);
                    AddParameter(cmd, "@Status", DbType.String, copy.Status ?? "Available", 20);
                    AddParameter(cmd, "@Location", DbType.String, copy.Location, 100);
                    AddParameter(cmd, "@Barcode", DbType.String, copy.Barcode, 50);
                    AddParameter(cmd, "@DateAdded", DbType.DateTime, copy.DateAdded == default(DateTime) ? (object)DateTime.Now : copy.DateAdded, 0);
                    AddParameter(cmd, "@AddedByID", DbType.Int32, addedByValue, 0);

                    return (int)cmd.ExecuteScalar();
                }
            }

            // Otherwise generate accession atomically using identity (CopyID) to avoid concurrency/race.
            // Because AccessionNumber is NOT NULL and has a UNIQUE constraint, we:
            // 1) Insert a temporary unique placeholder AccessionNumber (GUID-based)
            // 2) Get the new identity (CopyID)
            // 3) Compute final accession using CopyID and update the row
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

                        // 2) Insert row WITH a temporary unique AccessionNumber (so NOT NULL + UNIQUE satisfied)
                        int newId;
                        DateTime dateAdded = copy.DateAdded == default(DateTime) ? DateTime.Now : copy.DateAdded;
                        string tempAccession = $"TMP-{Guid.NewGuid():N}";
                        using (var cmdInsert = conn.CreateCommand())
                        {
                            cmdInsert.Transaction = tran;
                            cmdInsert.CommandText = @"INSERT INTO [BookCopy] 
                                (BookID, AccessionNumber, Status, Location, barcode, dateAdded, addedByID) 
                                VALUES (@BookID, @AccessionNumber, @Status, @Location, @Barcode, @DateAdded, @AddedByID);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            AddParameter(cmdInsert, "@BookID", DbType.Int32, copy.BookID, 0);
                            AddParameter(cmdInsert, "@AccessionNumber", DbType.String, tempAccession, 30);
                            AddParameter(cmdInsert, "@Status", DbType.String, copy.Status ?? "Available", 20);
                            AddParameter(cmdInsert, "@Location", DbType.String, copy.Location, 100);
                            AddParameter(cmdInsert, "@Barcode", DbType.String, copy.Barcode, 50);
                            AddParameter(cmdInsert, "@DateAdded", DbType.DateTime, dateAdded, 0);
                            AddParameter(cmdInsert, "@AddedByID", DbType.Int32, addedByValue, 0);

                            newId = (int)cmdInsert.ExecuteScalar();
                        }

                        // 3) Compute accession using prefix + BookID + year + sequence derived from newId
                        string prefix = MapResourceTypeToPrefix(resourceTypeStr);
                        int year = dateAdded.Year;

                        // Format: PREFIX-BookID-Year-Sequence (sequence uses the unique identity to guarantee uniqueness)
                        string accession = $"{prefix}-{copy.BookID}-{year}-{newId:D4}";

                        // 4) Update the inserted row with the generated accession
                        using (var cmdUpdate = conn.CreateCommand())
                        {
                            cmdUpdate.Transaction = tran;
                            cmdUpdate.CommandText = @"UPDATE [BookCopy] 
                                                      SET AccessionNumber = @AccessionNumber
                                                      WHERE CopyID = @CopyID";

                            AddParameter(cmdUpdate, "@AccessionNumber", DbType.String, accession, 30);
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
                cmd.CommandText = @"SELECT CopyID, BookID, AccessionNumber, Status, Location, barcode, dateAdded, addedByID 
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
                cmd.CommandText = "SELECT COUNT(*) FROM [BookCopy] WHERE BookID = @BookID AND YEAR(dateAdded) = @Year";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);
                AddParameter(cmd, "@Year", DbType.Int32, dateAdded.Year, 0);
                copyCount = (int)cmd.ExecuteScalar();
            }

            int nextNumber = copyCount + 1;
            int year = dateAdded.Year;

            // Keep legacy format if someone still relies on it:
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
                                    SET barcode = @Barcode 
                                    WHERE AccessionNumber = @AccessionNumber";

                AddParameter(cmd, "@Barcode", DbType.String, barcodeImagePath, 50);
                AddParameter(cmd, "@AccessionNumber", DbType.String, accessionNumber, 30);

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
