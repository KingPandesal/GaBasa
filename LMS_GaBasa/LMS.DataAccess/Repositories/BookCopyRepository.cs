using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Enums;
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

        // Implement Add to satisfy IBookCopyRepository
        public int Add(BookCopy copy)
        {
            if (copy == null) throw new ArgumentNullException(nameof(copy));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    INSERT INTO [BookCopy] (BookID, AccessionNumber, Status, Location, barcode, dateAdded, addedByID)
                    VALUES (@BookID, @AccessionNumber, @Status, @Location, @Barcode, @DateAdded, @AddedByID);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@BookID", DbType.Int32, copy.BookID, 0);
                AddParameter(cmd, "@AccessionNumber", DbType.String, copy.AccessionNumber, 200);
                AddParameter(cmd, "@Status", DbType.String, copy.Status, 50);
                AddParameter(cmd, "@Location", DbType.String, copy.Location, 200);
                AddParameter(cmd, "@Barcode", DbType.String, copy.Barcode, 500);
                AddParameter(cmd, "@DateAdded", DbType.DateTime, copy.DateAdded == DateTime.MinValue ? (object)DateTime.UtcNow : copy.DateAdded, 0);
                AddParameter(cmd, "@AddedByID", DbType.Int32, copy.AddedByID, 0);

                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        // New: update an existing copy's mutable fields (Status, Location, Barcode)
        public bool Update(BookCopy copy)
        {
            if (copy == null) throw new ArgumentNullException(nameof(copy));
            if (copy.CopyID <= 0) throw new ArgumentException(nameof(copy.CopyID));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE [BookCopy]
                    SET Status = @Status,
                        Location = @Location,
                        barcode = @Barcode
                    WHERE CopyID = @CopyID";

                AddParameter(cmd, "@Status", DbType.String, copy.Status, 50);
                AddParameter(cmd, "@Location", DbType.String, copy.Location, 200);
                AddParameter(cmd, "@Barcode", DbType.String, copy.Barcode, 500);
                AddParameter(cmd, "@CopyID", DbType.Int32, copy.CopyID, 0);

                return cmd.ExecuteNonQuery() > 0;
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

        // New: update accession number and barcode path for a single copy
        public bool UpdateAccessionAndBarcodeByCopyId(int copyId, string accessionNumber, string barcodePath)
        {
            if (copyId <= 0) throw new ArgumentException(nameof(copyId));
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"UPDATE [BookCopy]
                                    SET AccessionNumber = @AccessionNumber,
                                        Barcode = @Barcode
                                    WHERE CopyID = @CopyID";
                AddParameter(cmd, "@AccessionNumber", DbType.String, (object)accessionNumber ?? DBNull.Value, 200);
                AddParameter(cmd, "@Barcode", DbType.String, (object)barcodePath ?? DBNull.Value, 500);
                AddParameter(cmd, "@CopyID", DbType.Int32, copyId, 0);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // New: delete all copies for a given BookID (used when converting to E-Book)
        public bool DeleteByBookId(int bookId)
        {
            if (bookId <= 0) return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"DELETE FROM [BookCopy] WHERE BookID = @BookID";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId, 0);

                // returns true even if 0 rows affected (no copies to delete)
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        // New: delete a single copy by its CopyID
        public bool DeleteByCopyId(int copyId)
        {
            if (copyId <= 0) return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"DELETE FROM [BookCopy] WHERE CopyID = @CopyID";
                AddParameter(cmd, "@CopyID", DbType.Int32, copyId, 0);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        // Expose a ResourceType -> prefix mapping for consumers (EditBook etc.)
        public string GetPrefixForResourceType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.EBook:
                    return "EB";
                case ResourceType.Thesis:
                    return "TH";
                case ResourceType.AV:
                    return "AV";
                case ResourceType.Periodical:
                    return "PR";
                case ResourceType.PhysicalBook:
                default:
                    return "BK";
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
