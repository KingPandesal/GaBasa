using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Circulation;

namespace LMS.DataAccess.Repositories
{
    /// <summary>
    /// Repository for circulation data access operations.
    /// </summary>
    public class CirculationRepository : ICirculationRepository
    {
        private readonly DbConnection _db;

        public CirculationRepository() : this(new DbConnection()) { }

        public CirculationRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public DTOCirculationMemberInfo GetMemberInfoByMemberId(int memberId)
        {
            if (memberId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        m.MemberID,
                        u.UserID,
                        u.FirstName,
                        u.LastName,
                        mt.TypeName AS MemberType,
                        m.[Status],
                        u.Photo AS PhotoPath,
                        m.ValidID AS ValidIdPath,
                        mt.MaxBooksAllowed,
                        mt.FineRate,
                        mt.MaxFineCap,
                        mt.BorrowingPeriod
                    FROM [Member] m
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    INNER JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    WHERE m.MemberID = @MemberID";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new DTOCirculationMemberInfo
                    {
                        MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? "" : reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? "" : reader.GetString(reader.GetOrdinal("LastName")),
                        MemberType = reader.IsDBNull(reader.GetOrdinal("MemberType")) ? "" : reader.GetString(reader.GetOrdinal("MemberType")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : reader.GetString(reader.GetOrdinal("Status")),
                        PhotoPath = reader.IsDBNull(reader.GetOrdinal("PhotoPath")) ? null : reader.GetString(reader.GetOrdinal("PhotoPath")),
                        ValidIdPath = reader.IsDBNull(reader.GetOrdinal("ValidIdPath")) ? null : reader.GetString(reader.GetOrdinal("ValidIdPath")),
                        MaxBooksAllowed = reader.IsDBNull(reader.GetOrdinal("MaxBooksAllowed")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaxBooksAllowed")),
                        FineRate = reader.IsDBNull(reader.GetOrdinal("FineRate")) ? 0m : reader.GetDecimal(reader.GetOrdinal("FineRate")),
                        MaxFineCap = reader.IsDBNull(reader.GetOrdinal("MaxFineCap")) ? 0m : reader.GetDecimal(reader.GetOrdinal("MaxFineCap")),
                        BorrowingPeriod = reader.IsDBNull(reader.GetOrdinal("BorrowingPeriod")) ? 14 : reader.GetInt32(reader.GetOrdinal("BorrowingPeriod"))
                    };
                }
            }
        }

        public int GetCurrentBorrowedCount(int memberId)
        {
            if (memberId <= 0)
                return 0;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Count transactions where Status is 'Borrowed' or 'Overdue' (not returned)
                cmd.CommandText = @"
                    SELECT COUNT(*) 
                    FROM [BorrowingTransaction] 
                    WHERE MemberID = @MemberID 
                      AND [Status] IN ('Borrowed', 'Overdue')";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public int GetOverdueCount(int memberId)
        {
            if (memberId <= 0)
                return 0;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Count transactions where Status is 'Overdue' OR DueDate has passed and not returned
                cmd.CommandText = @"
                    SELECT COUNT(*) 
                    FROM [BorrowingTransaction] 
                    WHERE MemberID = @MemberID 
                      AND ReturnDate IS NULL
                      AND (Status = 'Overdue' OR DueDate < @Today)";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                AddParameter(cmd, "@Today", DbType.DateTime, DateTime.Today);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public decimal GetTotalUnpaidFines(int memberId)
        {
            if (memberId <= 0)
                return 0m;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Sum of unpaid fines for the member
                cmd.CommandText = @"
                    SELECT ISNULL(SUM(FineAmount), 0) 
                    FROM [Fine] 
                    WHERE MemberID = @MemberID 
                      AND [Status] = 'Unpaid'";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
            }
        }

        public DTOCirculationBookInfo GetBookInfoByAccession(string accessionNumber)
        {
            if (string.IsNullOrWhiteSpace(accessionNumber))
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        bc.CopyID,
                        bc.BookID,
                        bc.AccessionNumber,
                        bc.[Status] AS CopyStatus,
                        bc.Location,
                        b.Title,
                        b.LoanType,
                        b.ResourceType,
                        b.DownloadURL,
                        c.Name AS Category
                    FROM [BookCopy] bc
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    LEFT JOIN [Category] c ON b.CategoryID = c.CategoryID
                    WHERE bc.AccessionNumber = @AccessionNumber";

                AddParameter(cmd, "@AccessionNumber", DbType.String, accessionNumber.Trim());

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    int bookId = reader.GetInt32(reader.GetOrdinal("BookID"));

                    var dto = new DTOCirculationBookInfo
                    {
                        CopyID = reader.GetInt32(reader.GetOrdinal("CopyID")),
                        BookID = bookId,
                        AccessionNumber = reader.IsDBNull(reader.GetOrdinal("AccessionNumber")) ? "" : reader.GetString(reader.GetOrdinal("AccessionNumber")),
                        CopyStatus = reader.IsDBNull(reader.GetOrdinal("CopyStatus")) ? "" : reader.GetString(reader.GetOrdinal("CopyStatus")),
                        Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? "" : reader.GetString(reader.GetOrdinal("Location")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? "" : reader.GetString(reader.GetOrdinal("Title")),
                        LoanType = reader.IsDBNull(reader.GetOrdinal("LoanType")) ? "" : reader.GetString(reader.GetOrdinal("LoanType")),
                        Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? "" : reader.GetString(reader.GetOrdinal("Category")),
                        ResourceType = reader.IsDBNull(reader.GetOrdinal("ResourceType")) ? "" : reader.GetString(reader.GetOrdinal("ResourceType")),
                        DownloadURL = reader.IsDBNull(reader.GetOrdinal("DownloadURL")) ? "" : reader.GetString(reader.GetOrdinal("DownloadURL"))
                    };

                    // Authors (Role = 'Author' only)
                    dto.Authors = GetAuthorsForBook(bookId);

                    // Do not assign derived/read-only properties here. DTO exposes computed properties for those values.

                    return dto;
                }
            }
        }

        private string GetAuthorsForBook(int bookId)
        {
            try
            {
                var names = new List<string>();

                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    // 1) Primary attempt: fetch authors explicitly marked as 'Author' (case-insensitive)
                    cmd.CommandText = @"
                        SELECT a.FullName
                        FROM [BookAuthor] ba
                        INNER JOIN [Author] a ON ba.AuthorID = a.AuthorID
                        WHERE ba.BookID = @BookID
                          AND LOWER(RTRIM(LTRIM(ISNULL(ba.Role, '')))) = 'author'
                        ORDER BY ba.IsPrimaryAuthor DESC, a.FullName";

                    AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                names.Add(reader.GetString(0).Trim());
                        }
                    }

                    // 2) Fallback: if no explicit 'Author' roles found, include relations that are not Editor/Adviser (or with NULL role)
                    if (names.Count == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            SELECT a.FullName
                            FROM [BookAuthor] ba
                            INNER JOIN [Author] a ON ba.AuthorID = a.AuthorID
                            WHERE ba.BookID = @BookID
                              AND (ba.Role IS NULL OR LOWER(RTRIM(LTRIM(ba.Role))) NOT IN ('editor','adviser'))
                            ORDER BY ba.IsPrimaryAuthor DESC, a.FullName";

                        AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                        using (var reader2 = cmd.ExecuteReader())
                        {
                            while (reader2.Read())
                            {
                                if (!reader2.IsDBNull(0))
                                    names.Add(reader2.GetString(0).Trim());
                            }
                        }
                    }
                }

                // Deduplicate and return comma separated list
                var distinct = names.Where(n => !string.IsNullOrWhiteSpace(n))
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .ToList();

                return distinct.Count > 0 ? string.Join(", ", distinct) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void AddParameter(IDbCommand cmd, string name, DbType dbType, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.DbType = dbType;
            param.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }
    }
}
