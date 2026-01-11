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
                        mt.BorrowingPeriod,
                        mt.RenewalLimit,
                        mt.ReservationPrivilege
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
                        BorrowingPeriod = reader.IsDBNull(reader.GetOrdinal("BorrowingPeriod")) ? 14 : reader.GetInt32(reader.GetOrdinal("BorrowingPeriod")),
                        RenewalLimit = reader.IsDBNull(reader.GetOrdinal("RenewalLimit")) ? 2 : reader.GetInt32(reader.GetOrdinal("RenewalLimit")),
                        ReservationPrivilege = !reader.IsDBNull(reader.GetOrdinal("ReservationPrivilege")) && reader.GetBoolean(reader.GetOrdinal("ReservationPrivilege"))
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

                        // After adding a fine, check if member should be suspended
                        if (fineAmount > 0)
                        {
                            CheckAndSuspendMemberIfNeeded(memberId);
                        }

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

                        // After adding a fine, check if member should be suspended
                        if (fineAmount > 0)
                        {
                            CheckAndSuspendMemberIfNeeded(memberId);
                        }

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
        /// Also checks if member should be suspended after the fine is added.
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
                
                if (rows > 0)
                {
                    // After adding a fine, check if member should be suspended
                    CheckAndSuspendMemberIfNeeded(memberId);
                    return true;
                }
                
                return false;
            }
        }

        public bool WaiveFines(List<int> fineIds, string reason)
        {
            if (fineIds == null || fineIds.Count == 0)
                return false;

            int? memberId = null;

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
                            // Get member ID before updating (for reactivation check)
                            if (!memberId.HasValue)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"SELECT MemberID FROM [Fine] WHERE FineID = @FineID";
                                AddParameter(cmd, "@FineID", DbType.Int32, fineId);
                                var result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                    memberId = Convert.ToInt32(result);
                            }

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

                        // After waiving fines, check if member should be reactivated
                        if (memberId.HasValue && memberId.Value > 0)
                        {
                            CheckAndReactivateMemberIfNeeded(memberId.Value);
                        }

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
        /// Processes payment for the specified fines.
        /// Inserts Payment records and updates Fine.Status to 'Paid'.
        /// Also checks if member should be reactivated after payment.
        /// Returns list of created PaymentIDs (empty on failure).
        /// </summary>
        public List<int> ProcessPayment(List<int> fineIds, string paymentMode, DateTime paymentDate)
        {
            var paymentIds = new List<int>();

            if (fineIds == null || fineIds.Count == 0)
                return paymentIds;

            if (string.IsNullOrWhiteSpace(paymentMode))
                paymentMode = "Cash";

            int? memberId = null;

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
                            // Get the fine amount and member ID for this fine and ensure it's still unpaid
                            cmd.Parameters.Clear();
                            cmd.CommandText = @"SELECT FineAmount, MemberID FROM [Fine] WHERE FineID = @FineID AND [Status] = 'Unpaid'";
                            AddParameter(cmd, "@FineID", DbType.Int32, fineId);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                    continue; // skip if not found or already paid/waived

                                decimal fineAmount = reader.GetDecimal(0);
                                if (!memberId.HasValue)
                                    memberId = reader.GetInt32(1);

                                reader.Close();

                                // Insert Payment record and get PaymentID
                                cmd.Parameters.Clear();
                                cmd.CommandText = @"
                                    INSERT INTO [Payment] (FineID, PaymentDate, AmountPaid, PaymentMode)
                                    VALUES (@FineID, @PaymentDate, @AmountPaid, @PaymentMode);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                                AddParameter(cmd, "@FineID", DbType.Int32, fineId);
                                AddParameter(cmd, "@PaymentDate", DbType.DateTime, paymentDate);
                                AddParameter(cmd, "@AmountPaid", DbType.Decimal, fineAmount);
                                AddParameter(cmd, "@PaymentMode", DbType.String, paymentMode);

                                var inserted = cmd.ExecuteScalar();
                                int paymentId = inserted == null || inserted == DBNull.Value ? 0 : Convert.ToInt32(inserted);
                                if (paymentId > 0)
                                {
                                    paymentIds.Add(paymentId);

                                    // Update Fine status to 'Paid'
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"
                                        UPDATE [Fine]
                                        SET [Status] = 'Paid'
                                        WHERE FineID = @FineID";

                                    AddParameter(cmd, "@FineID", DbType.Int32, fineId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        tran.Commit();

                        // After successful payment, check if member should be reactivated
                        if (memberId.HasValue && memberId.Value > 0)
                        {
                            CheckAndReactivateMemberIfNeeded(memberId.Value);
                        }

                        return paymentIds;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        return new List<int>();
                    }
                }
            }
        }

        public DTORenewalInfo GetRenewalInfoByAccession(string accessionNumber)
        {
            if (string.IsNullOrWhiteSpace(accessionNumber)) return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        bt.TransactionID,
                        bt.CopyID,
                        bt.MemberID,
                        ISNULL(u.FirstName,'') + ' ' + ISNULL(u.LastName,'') AS MemberName,
                        bt.BorrowDate,
                        bt.DueDate,
                        ISNULL(bt.RenewalCount, 0) AS RenewalCount,
                        b.Title,
                        bc.AccessionNumber,
                        bc.BookID,
                        ISNULL(mt.RenewalLimit, 2) AS MaxRenewals,
                        ISNULL(mt.BorrowingPeriod, 14) AS BorrowingPeriod
                    FROM [BorrowingTransaction] bt
                    INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    INNER JOIN [Member] m ON bt.MemberID = m.MemberID
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    LEFT JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    WHERE bc.AccessionNumber = @AccessionNumber
                      AND bt.ReturnDate IS NULL
                      AND bt.[Status] IN ('Borrowed', 'Overdue')";

                AddParameter(cmd, "@AccessionNumber", DbType.String, accessionNumber.Trim());

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    int bookId = reader.GetInt32(reader.GetOrdinal("BookID"));
                    int renewalCount = reader.GetInt32(reader.GetOrdinal("RenewalCount"));
                    int maxRenewals = reader.GetInt32(reader.GetOrdinal("MaxRenewals"));

                    var dto = new DTORenewalInfo
                    {
                        TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                        CopyID = reader.GetInt32(reader.GetOrdinal("CopyID")),
                        MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                        MemberName = reader.IsDBNull(reader.GetOrdinal("MemberName")) ? "" : reader.GetString(reader.GetOrdinal("MemberName")).Trim(),
                        BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                        DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                        RenewalCount = renewalCount,
                        MaxRenewals = maxRenewals,
                        BorrowingPeriod = reader.GetInt32(reader.GetOrdinal("BorrowingPeriod")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? "" : reader.GetString(reader.GetOrdinal("Title")).Trim(),
                        AccessionNumber = reader.IsDBNull(reader.GetOrdinal("AccessionNumber")) ? "" : reader.GetString(reader.GetOrdinal("AccessionNumber")).Trim(),
                        IsWithinRenewalLimit = renewalCount < maxRenewals
                    };

                    // Close reader before making another query
                    reader.Close();

                    // Check for active reservations on this book
                    dto.HasActiveReservation = HasActiveReservationForBook(bookId);

                    return dto;
                }
            }
        }

        public bool HasActiveReservationForBook(int bookId)
        {
            if (bookId <= 0) return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Check Reservation table for active reservations (Pending or Ready)
                // Reservation table typically has: ReservationID, MemberID, BookID/CopyID, ReservationDate, Status
                cmd.CommandText = @"
                    SELECT COUNT(*) 
                    FROM [Reservation] r
                    INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                    WHERE bc.BookID = @BookID
                      AND r.[Status] IN ('Pending', 'Ready', 'Active')";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                var result = cmd.ExecuteScalar();
                int count = result != null ? Convert.ToInt32(result) : 0;
                return count > 0;
            }
        }

        public int? GetBookIdByCopyId(int copyId)
        {
            if (copyId <= 0) return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"SELECT BookID FROM [BookCopy] WHERE CopyID = @CopyID";
                AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? (int?)Convert.ToInt32(result) : null;
            }
        }

        /// <summary>
        /// Renews the borrowing transaction by incrementing the renewal count and extending the due date.
        /// </summary>
        /// <param name="transactionId">The ID of the borrowing transaction to renew.</param>
        /// <param name="newDueDate">Output parameter that will receive the new due date if renewal is successful.</param>
        /// <returns>True if the renewal was successful, false otherwise.</returns>
        public bool RenewBorrowingTransaction(int transactionId, out DateTime newDueDate)
        {
            newDueDate = DateTime.MinValue;
            if (transactionId <= 0) return false;

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    try
                    {
                        // 1) Load transaction, current renewal count, member type renewal limit, borrowing period and book id
                        cmd.CommandText = @"
                            SELECT 
                                bc.BookID,
                                ISNULL(bt.RenewalCount, 0) AS RenewalCount,
                                ISNULL(mt.RenewalLimit, 2) AS RenewalLimit,
                                ISNULL(mt.BorrowingPeriod, 14) AS BorrowingPeriod
                            FROM [BorrowingTransaction] bt
                            INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                            INNER JOIN [Member] m ON bt.MemberID = m.MemberID
                            LEFT JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                            WHERE bt.TransactionID = @TransactionID
                              AND bt.ReturnDate IS NULL
                              AND bt.[Status] IN ('Borrowed', 'Overdue')";

                        AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);

                        int bookId;
                        int renewalCount;
                        int renewalLimit;
                        int borrowingPeriod;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                // transaction not found / not active
                                return false;
                            }

                            bookId = reader.GetInt32(reader.GetOrdinal("BookID"));
                            renewalCount = reader.GetInt32(reader.GetOrdinal("RenewalCount"));
                            renewalLimit = reader.GetInt32(reader.GetOrdinal("RenewalLimit"));
                            borrowingPeriod = reader.GetInt32(reader.GetOrdinal("BorrowingPeriod"));
                        }

                        // 2) Check renewal limit
                        if (renewalCount >= renewalLimit)
                        {
                            return false; // limit reached
                        }

                        // 3) Check for active reservations for the same book
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            SELECT COUNT(*) 
                            FROM [Reservation] r
                            INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                            WHERE bc.BookID = @BookID
                              AND r.[Status] IN ('Pending', 'Ready', 'Active')";
                        AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                        var resObj = cmd.ExecuteScalar();
                        int resCount = resObj != null && resObj != DBNull.Value ? Convert.ToInt32(resObj) : 0;
                        if (resCount > 0)
                            return false;

                        // 4) Perform update: increment renewal count and extend due date
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            UPDATE [BorrowingTransaction]
                            SET RenewalCount = ISNULL(RenewalCount,0) + 1,
                                DueDate = DATEADD(day, @BorrowingPeriod, DueDate)
                            WHERE TransactionID = @TransactionID
                              AND ReturnDate IS NULL
                              AND [Status] IN ('Borrowed', 'Overdue')";
                        AddParameter(cmd, "@BorrowingPeriod", DbType.Int32, borrowingPeriod);
                        AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);

                        int updated = cmd.ExecuteNonQuery();
                        if (updated == 0)
                        {
                            tran.Rollback();
                            return false;
                        }

                        // 5) Read new due date
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"SELECT DueDate FROM [BorrowingTransaction] WHERE TransactionID = @TransactionID";
                        AddParameter(cmd, "@TransactionID", DbType.Int32, transactionId);

                        var dueObj = cmd.ExecuteScalar();
                        if (dueObj != null && dueObj != DBNull.Value)
                        {
                            newDueDate = Convert.ToDateTime(dueObj);
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
        /// Updates the Member table Status field for the specified member.
        /// </summary>
        public bool UpdateMemberStatus(int memberId, string status)
        {
            if (memberId <= 0 || string.IsNullOrWhiteSpace(status))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    UPDATE [Member]
                    SET [Status] = @Status
                    WHERE MemberID = @MemberID";

                AddParameter(cmd, "@Status", DbType.String, status.Trim());
                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        /// <summary>
        /// Gets the MaxFineCap for a member from their MemberType.
        /// </summary>
        public decimal GetMemberMaxFineCap(int memberId)
        {
            if (memberId <= 0)
                return 0m;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT ISNULL(mt.MaxFineCap, 0)
                    FROM [Member] m
                    INNER JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    WHERE m.MemberID = @MemberID";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
            }
        }

        /// <summary>
        /// Checks if member's total unpaid fines have reached or exceeded MaxFineCap and suspends them if so.
        /// </summary>
        public void CheckAndSuspendMemberIfNeeded(int memberId)
        {
            if (memberId <= 0) return;

            decimal totalFines = GetTotalUnpaidFines(memberId);
            decimal maxCap = GetMemberMaxFineCap(memberId);

            // Only suspend if MaxFineCap > 0 and fines have reached it
            if (maxCap > 0 && totalFines >= maxCap)
            {
                UpdateMemberStatus(memberId, "Suspended");
            }
        }

        /// <summary>
        /// Checks if member's total unpaid fines have dropped below MaxFineCap and reactivates them if so.
        /// Only reactivates if current status is "Suspended".
        /// </summary>
        public void CheckAndReactivateMemberIfNeeded(int memberId)
        {
            if (memberId <= 0) return;

            // First check if member is currently suspended
            string currentStatus = GetMemberStatus(memberId);
            if (!string.Equals(currentStatus, "Suspended", StringComparison.OrdinalIgnoreCase))
                return; // Only reactivate suspended members

            decimal totalFines = GetTotalUnpaidFines(memberId);
            decimal maxCap = GetMemberMaxFineCap(memberId);

            // Reactivate if fines are now below the cap (or cap is 0 meaning no limit)
            if (maxCap <= 0 || totalFines < maxCap)
            {
                UpdateMemberStatus(memberId, "Active");
            }
        }

        /// <summary>
        /// Gets the current status of a member.
        /// </summary>
        private string GetMemberStatus(int memberId)
        {
            if (memberId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"SELECT [Status] FROM [Member] WHERE MemberID = @MemberID";
                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? result.ToString() : null;
            }
        }
    }
}
