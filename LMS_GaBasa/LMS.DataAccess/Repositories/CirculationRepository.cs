using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;

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

        public int CreateBorrowingTransaction(int copyId, int memberId, DateTime borrowDate, DateTime dueDate)
        {
            if (copyId <= 0 || memberId <= 0) return 0;

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        // Insert borrowing transaction
                        cmd.CommandText = @"
                            INSERT INTO [BorrowingTransaction] (CopyID, MemberID, BorrowDate, DueDate, Status)
                            VALUES (@CopyID, @MemberID, @BorrowDate, @DueDate, @Status);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        AddParameter(cmd, "@CopyID", DbType.Int32, copyId);
                        AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                        AddParameter(cmd, "@BorrowDate", DbType.DateTime, borrowDate);
                        AddParameter(cmd, "@DueDate", DbType.DateTime, dueDate);
                        AddParameter(cmd, "@Status", DbType.String, "Borrowed");

                        var result = cmd.ExecuteScalar();
                        int newId = result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);

                        if (newId <= 0)
                        {
                            tran.Rollback();
                            return 0;
                        }

                        // Update BookCopy status to Borrowed
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"UPDATE [BookCopy] SET [Status] = @Status WHERE CopyID = @CopyID";
                        AddParameter(cmd, "@Status", DbType.String, "Borrowed");
                        AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                        int updated = cmd.ExecuteNonQuery();
                        if (updated == 0)
                        {
                            // If update failed, rollback the transaction
                            tran.Rollback();
                            return 0;
                        }

                        tran.Commit();
                        return newId;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        return 0;
                    }
                }
            }
        }

        public DTOReturnInfo GetActiveBorrowingByAccession(string accessionNumber)
        {
            if (string.IsNullOrWhiteSpace(accessionNumber)) return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Query active borrowing transaction for given accession (ReturnDate IS NULL)
                // Join to member/user and membertype to compute fine rules
                cmd.CommandText = @"
                    SELECT 
                        bt.TransactionID,
                        bt.CopyID,
                        bt.MemberID,
                        ISNULL(u.FirstName,'') + ' ' + ISNULL(u.LastName,'') AS MemberName,
                        bt.BorrowDate,
                        bt.DueDate,
                        bt.ReturnDate,
                        b.Title,
                        bc.AccessionNumber,
                        ISNULL(mt.FineRate, 0) AS FineRate,
                        ISNULL(mt.MaxFineCap, 0) AS MaxFineCap
                    FROM [BorrowingTransaction] bt
                    INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    INNER JOIN [Member] m ON bt.MemberID = m.MemberID
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    LEFT JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    WHERE bc.AccessionNumber = @AccessionNumber
                      AND bt.ReturnDate IS NULL";

                AddParameter(cmd, "@AccessionNumber", DbType.String, accessionNumber.Trim());

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    var dto = new DTOReturnInfo
                    {
                        TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                        CopyID = reader.GetInt32(reader.GetOrdinal("CopyID")),
                        MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                        MemberName = reader.IsDBNull(reader.GetOrdinal("MemberName")) ? "" : reader.GetString(reader.GetOrdinal("MemberName")).Trim(),
                        BorrowDate = reader.IsDBNull(reader.GetOrdinal("BorrowDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                        DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                        ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? "" : reader.GetString(reader.GetOrdinal("Title")).Trim(),
                        AccessionNumber = reader.IsDBNull(reader.GetOrdinal("AccessionNumber")) ? "" : reader.GetString(reader.GetOrdinal("AccessionNumber")).Trim()
                    };

                    // Compute fine using member type rules if overdue
                    decimal fineRate = reader.IsDBNull(reader.GetOrdinal("FineRate")) ? 0m : reader.GetDecimal(reader.GetOrdinal("FineRate"));
                    decimal maxCap = reader.IsDBNull(reader.GetOrdinal("MaxFineCap")) ? 0m : reader.GetDecimal(reader.GetOrdinal("MaxFineCap"));

                    var today = DateTime.Today;
                    var due = dto.DueDate.Date;

                    if (today > due)
                    {
                        dto.DaysOverdue = (today - due).Days;
                        dto.FineAmount = dto.DaysOverdue * fineRate;
                        if (maxCap > 0 && dto.FineAmount > maxCap) dto.FineAmount = maxCap;
                    }
                    else
                    {
                        dto.DaysOverdue = 0;
                        dto.FineAmount = 0m;
                    }

                    return dto;
                }
            }
        }

        public bool CompleteReturnGood(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount)
        {
            if (transactionId <= 0 || copyId <= 0 || memberId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        // 1. Update BorrowingTransaction: Status = 'Returned', ReturnDate = returnDate
                        cmd.CommandText = @"
                            UPDATE [BorrowingTransaction]
                            SET [Status] = 'Returned', ReturnDate = @ReturnDate
                            WHERE TransactionID = @TransactionID";

                        AddParameter(cmd, "@ReturnDate", DbType.DateTime, returnDate);
                        AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            tran.Rollback();
                            return false;
                        }

                        // 2. Update BookCopy: Status = 'Available'
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            UPDATE [BookCopy]
                            SET [Status] = 'Available'
                            WHERE CopyID = @CopyID";

                        AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                        rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            tran.Rollback();
                            return false;
                        }

                        // 3. If there's a fine (overdue), insert into Fine table
                        if (fineAmount > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"
                                INSERT INTO [Fine] (MemberID, TransactionID, FineAmount, FineType, DateIssued, [Status])
                                VALUES (@MemberID, @TransactionID, @FineAmount, 'Overdue', @DateIssued, 'Unpaid')";

                            AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                            AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);
                            AddParameter(cmd, "@FineAmount", DbType.Decimal, fineAmount);
                            AddParameter(cmd, "@DateIssued", DbType.DateTime, returnDate);

                            rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                tran.Rollback();
                                return false;
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Handles returns where condition is Lost or Damaged.
        /// Sets BorrowingTransaction.Status = 'Returned', ReturnDate = returnDate.
        /// Sets BookCopy.Status = condition (normalized).
        /// Inserts Fine record when fineAmount &gt; 0 with FineType = condition.
        /// </summary>
        public bool CompleteReturnWithCondition(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount, string condition)
        {
            if (transactionId <= 0 || copyId <= 0 || memberId <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(condition))
                condition = "Damaged";

            // Normalize condition to allowed DB values
            string normalizedCondition;
            var c = condition.Trim().ToLowerInvariant();
            if (c.StartsWith("lost"))
                normalizedCondition = "Lost";
            else if (c.StartsWith("damag")) // matches "Damage" or "Damaged"
                normalizedCondition = "Damaged";
            else
                normalizedCondition = CultureInfoInvariantTitle(condition);

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        // 1) Update BorrowingTransaction
                        cmd.CommandText = @"
                            UPDATE [BorrowingTransaction]
                            SET [Status] = 'Returned', ReturnDate = @ReturnDate
                            WHERE TransactionID = @TransactionID";

                        AddParameter(cmd, "@ReturnDate", DbType.DateTime, returnDate);
                        AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            tran.Rollback();
                            return false;
                        }

                        // 2) Update BookCopy: set to Lost/Damaged
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            UPDATE [BookCopy]
                            SET [Status] = @Status
                            WHERE CopyID = @CopyID";

                        AddParameter(cmd, "@Status", DbType.String, normalizedCondition);
                        AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                        rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            tran.Rollback();
                            return false;
                        }

                        // 3) Insert Fine record if any
                        if (fineAmount > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"
                                INSERT INTO [Fine] (MemberID, TransactionID, FineAmount, FineType, DateIssued, [Status])
                                VALUES (@MemberID, @TransactionID, @FineAmount, @FineType, @DateIssued, 'Unpaid')";

                            AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                            AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);
                            AddParameter(cmd, "@FineAmount", DbType.Decimal, fineAmount);
                            AddParameter(cmd, "@FineType", DbType.String, normalizedCondition);
                            AddParameter(cmd, "@DateIssued", DbType.DateTime, returnDate);

                            rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                tran.Rollback();
                                return false;
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        return false;
                    }
                }
            }
        }

        // Helper to title-case unknown condition safely (no external globalization dependency here)
        private string CultureInfoInvariantTitle(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "Damaged";
            input = input.Trim().ToLowerInvariant();
            return char.ToUpperInvariant(input[0]) + input.Substring(1);
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

        // Add this method to CirculationRepository class

        /// <summary>
        /// Gets the most borrowed BookIDs with their borrow counts, ordered by most borrowed first.
        /// </summary>
        /// <param name="topCount">Maximum number of results to return.</param>
        /// <returns>Dictionary of BookID to borrow count.</returns>
        public Dictionary<int, int> GetMostBorrowedBookIds(int topCount = 8)
        {
            var result = new Dictionary<int, int>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT TOP (@TopCount) bc.BookID, COUNT(*) AS BorrowCount
                    FROM [BorrowingTransaction] bt
                    INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                    GROUP BY bc.BookID
                    ORDER BY BorrowCount DESC";

                AddParameter(cmd, "@TopCount", DbType.Int32, topCount);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int bookId = reader.GetInt32(0);
                        int count = reader.GetInt32(1);
                        result[bookId] = count;
                    }
                }
            }

            return result;
        }

        public List<DTOFineRecord> GetFinesByMemberId(int memberId)
        {
            var fines = new List<DTOFineRecord>();

            if (memberId <= 0)
                return fines;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        f.FineID,
                        f.TransactionID,
                        f.MemberID,
                        ISNULL(u.FirstName, '') + ' ' + ISNULL(u.LastName, '') AS MemberName,
                        f.FineAmount,
                        f.FineType,
                        f.DateIssued,
                        f.[Status]
                    FROM [Fine] f
                    INNER JOIN [Member] m ON f.MemberID = m.MemberID
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    WHERE f.MemberID = @MemberID
                    ORDER BY f.DateIssued DESC";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fines.Add(new DTOFineRecord
                        {
                            FineID = reader.GetInt32(reader.GetOrdinal("FineID")),
                            TransactionID = reader.IsDBNull(reader.GetOrdinal("TransactionID")) ? 0 : reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            MemberName = reader.IsDBNull(reader.GetOrdinal("MemberName")) ? "" : reader.GetString(reader.GetOrdinal("MemberName")).Trim(),
                            FineAmount = reader.IsDBNull(reader.GetOrdinal("FineAmount")) ? 0m : reader.GetDecimal(reader.GetOrdinal("FineAmount")),
                            FineType = reader.IsDBNull(reader.GetOrdinal("FineType")) ? "" : reader.GetString(reader.GetOrdinal("FineType")),
                            DateIssued = reader.IsDBNull(reader.GetOrdinal("DateIssued")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateIssued")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }
                }
            }

            return fines;
        }

        /// <summary>
        /// Inserts a fine charge for the member. If transactionId &lt;= 0, TransactionID will be inserted as NULL.
        /// </summary>
        public bool AddFineCharge(int memberId, int transactionId, decimal amount, string fineType, DateTime dateIssued, string status)
        {
            if (memberId <= 0) return false;
            if (amount <= 0m) return false;
            if (string.IsNullOrWhiteSpace(fineType)) fineType = "Charge";
            if (string.IsNullOrWhiteSpace(status)) status = "Unpaid";

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    INSERT INTO [Fine] (MemberID, TransactionID, FineAmount, FineType, DateIssued, [Status])
                    VALUES (@MemberID, @TransactionID, @FineAmount, @FineType, @DateIssued, @Status)";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                // Insert NULL for TransactionID when not provided
                AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId > 0 ? (object)transactionId : DBNull.Value);
                AddParameter(cmd, "@FineAmount", DbType.Decimal, amount);
                AddParameter(cmd, "@FineType", DbType.String, fineType);
                AddParameter(cmd, "@DateIssued", DbType.DateTime, dateIssued);
                AddParameter(cmd, "@Status", DbType.String, status);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool WaiveFines(List<int> fineIds, string reason)
        {
            if (fineIds == null || fineIds.Count == 0)
                return false;

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        foreach (var fineId in fineIds)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"
                                UPDATE [Fine]
                                SET [Status] = 'Waived', [Reason] = @Reason
                                WHERE FineID = @FineID AND [Status] = 'Unpaid'";

                            AddParameter(cmd, "@Reason", DbType.String, reason ?? (object)DBNull.Value);
                            AddParameter(cmd, "@FineID", DbType.Int32, fineId);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        return false;
                    }
                }
            }
        }

        public bool ProcessPayment(List<int> fineIds, string paymentMode, DateTime paymentDate)
        {
            if (fineIds == null || fineIds.Count == 0)
                return false;

            if (string.IsNullOrWhiteSpace(paymentMode))
                paymentMode = "Cash";

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        foreach (var fineId in fineIds)
                        {
                            // 1) Get the fine amount for this fine
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"SELECT FineAmount FROM [Fine] WHERE FineID = @FineID AND [Status] = 'Unpaid'";
                            AddParameter(cmd, "@FineID", DbType.Int32, fineId);

                            var result = cmd.ExecuteScalar();
                            if (result == null || result == DBNull.Value)
                                continue; // skip if not found or not unpaid

                            decimal fineAmount = Convert.ToDecimal(result);

                            // 2) Insert Payment record
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"
                                INSERT INTO [Payment] (FineID, PaymentDate, AmountPaid, PaymentMode)
                                VALUES (@FineID, @PaymentDate, @AmountPaid, @PaymentMode)";

                            AddParameter(cmd, "@FineID", DbType.Int32, fineId);
                            AddParameter(cmd, "@PaymentDate", DbType.DateTime, paymentDate);
                            AddParameter(cmd, "@AmountPaid", DbType.Decimal, fineAmount);
                            AddParameter(cmd, "@PaymentMode", DbType.String, paymentMode);

                            cmd.ExecuteNonQuery();

                            // 3) Update Fine status to 'Paid'
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"
                                UPDATE [Fine]
                                SET [Status] = 'Paid'
                                WHERE FineID = @FineID";

                            AddParameter(cmd, "@FineID", DbType.Int32, fineId);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        return false;
                    }
                }
            }
        }
    }
}
