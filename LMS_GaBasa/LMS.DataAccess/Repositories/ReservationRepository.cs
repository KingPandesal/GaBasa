using System;
using System.Collections.Generic;
using System.Data;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Transactions;
using LMS.Model.DTOs.Reservation;
using LMS.Model.DTOs.MemberFeatures.Reserve;

namespace LMS.DataAccess.Repositories
{
    /// <summary>
    /// Repository for Reservation data access.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        private readonly DbConnection _db;

        public ReservationRepository() : this(new DbConnection()) { }

        public ReservationRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Add(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    INSERT INTO [Reservation] (CopyID, MemberID, ReservationDate, ExpirationDate, [Status])
                    VALUES (@CopyID, @MemberID, @ReservationDate, @ExpirationDate, @Status);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@CopyID", DbType.Int32, reservation.CopyID);
                AddParameter(cmd, "@MemberID", DbType.Int32, reservation.MemberID);
                AddParameter(cmd, "@ReservationDate", DbType.DateTime, reservation.ReservationDate);
                AddParameter(cmd, "@ExpirationDate", DbType.DateTime, reservation.ExpirationDate.HasValue ? (object)reservation.ExpirationDate.Value : DBNull.Value);
                AddParameter(cmd, "@Status", DbType.String, reservation.Status ?? "Active", 50);

                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        public Reservation GetById(int reservationId)
        {
            if (reservationId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT ReservationID, CopyID, MemberID, ReservationDate, ExpirationDate, [Status]
                    FROM [Reservation]
                    WHERE ReservationID = @ReservationID";

                AddParameter(cmd, "@ReservationID", DbType.Int32, reservationId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapReservation(reader);
                    }
                }
            }

            return null;
        }

        public List<Reservation> GetActiveByMemberId(int memberId)
        {
            var reservations = new List<Reservation>();

            if (memberId <= 0)
                return reservations;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT ReservationID, CopyID, MemberID, ReservationDate, ExpirationDate, [Status]
                    FROM [Reservation]
                    WHERE MemberID = @MemberID AND [Status] = 'Active'
                    ORDER BY ReservationDate DESC";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(MapReservation(reader));
                    }
                }
            }

            return reservations;
        }

        public List<Reservation> GetAll()
        {
            var reservations = new List<Reservation>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT ReservationID, CopyID, MemberID, ReservationDate, ExpirationDate, [Status]
                    FROM [Reservation]
                    ORDER BY ReservationDate DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(MapReservation(reader));
                    }
                }
            }

            return reservations;
        }

        /// <summary>
        /// Gets all reservations with joined member and book info for display in the management view.
        /// </summary>
        public List<DTOReservationView> GetAllForDisplay()
        {
            var reservations = new List<DTOReservationView>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT 
                        r.ReservationID,
                        r.CopyID,
                        r.MemberID,
                        r.ReservationDate,
                        r.ExpirationDate,
                        r.[Status],
                        ISNULL(u.FirstName, '') + ' ' + ISNULL(u.LastName, '') AS MemberName,
                        b.Title AS BookTitle,
                        bc.AccessionNumber
                    FROM [Reservation] r
                    INNER JOIN [Member] m ON r.MemberID = m.MemberID
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    ORDER BY r.ReservationDate DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new DTOReservationView
                        {
                            ReservationID = reader.IsDBNull(reader.GetOrdinal("ReservationID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ReservationID")),
                            CopyID = reader.IsDBNull(reader.GetOrdinal("CopyID")) ? 0 : reader.GetInt32(reader.GetOrdinal("CopyID")),
                            MemberID = reader.IsDBNull(reader.GetOrdinal("MemberID")) ? 0 : reader.GetInt32(reader.GetOrdinal("MemberID")),
                            ReservationDate = reader.IsDBNull(reader.GetOrdinal("ReservationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                            ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Active" : reader.GetString(reader.GetOrdinal("Status")),
                            MemberName = reader.IsDBNull(reader.GetOrdinal("MemberName")) ? string.Empty : reader.GetString(reader.GetOrdinal("MemberName")).Trim(),
                            BookTitle = reader.IsDBNull(reader.GetOrdinal("BookTitle")) ? string.Empty : reader.GetString(reader.GetOrdinal("BookTitle")),
                            AccessionNumber = reader.IsDBNull(reader.GetOrdinal("AccessionNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("AccessionNumber"))
                        });
                    }
                }
            }

            return reservations;
        }

        public bool HasActiveReservationForBook(int bookId, int memberId)
        {
            if (bookId <= 0 || memberId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT COUNT(*)
                    FROM [Reservation] r
                    INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                    WHERE bc.BookID = @BookID 
                      AND r.MemberID = @MemberID 
                      AND r.[Status] = 'Active'";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId);
                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public bool HasActiveReservationForCopy(int copyId)
        {
            if (copyId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT COUNT(*)
                    FROM [Reservation]
                    WHERE CopyID = @CopyID 
                      AND [Status] = 'Active'";

                AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public Reservation GetActiveReservationByCopyId(int copyId)
        {
            if (copyId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT TOP 1 ReservationID, CopyID, MemberID, ReservationDate, ExpirationDate, [Status]
                    FROM [Reservation]
                    WHERE CopyID = @CopyID 
                      AND [Status] = 'Active'
                    ORDER BY ReservationDate ASC";

                AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapReservation(reader);
                    }
                }
            }

            return null;
        }

        public Reservation GetFirstInQueueByCopyId(int copyId)
        {
            if (copyId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT TOP 1 ReservationID, CopyID, MemberID, ReservationDate, ExpirationDate, [Status]
                    FROM [Reservation]
                    WHERE CopyID = @CopyID 
                      AND [Status] = 'Active'
                    ORDER BY ReservationDate ASC";

                AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapReservation(reader);
                    }
                }
            }

            return null;
        }

        public Reservation GetFirstInQueueByBookId(int bookId)
        {
            if (bookId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT TOP 1 r.ReservationID, r.CopyID, r.MemberID, r.ReservationDate, r.ExpirationDate, r.[Status]
                    FROM [Reservation] r
                    INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                    WHERE bc.BookID = @BookID 
                      AND r.[Status] = 'Active'
                    ORDER BY r.ReservationDate ASC";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapReservation(reader);
                    }
                }
            }

            return null;
        }

        public bool UpdateStatus(int reservationId, string status)
        {
            if (reservationId <= 0 || string.IsNullOrWhiteSpace(status))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE [Reservation]
                    SET [Status] = @Status
                    WHERE ReservationID = @ReservationID";

                AddParameter(cmd, "@ReservationID", DbType.Int32, reservationId);
                AddParameter(cmd, "@Status", DbType.String, status, 50);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public int ExpireOverdueReservations()
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE [Reservation]
                    SET [Status] = 'Expired'
                    WHERE [Status] = 'Active'
                      AND ExpirationDate IS NOT NULL
                      AND ExpirationDate < @Now";

                AddParameter(cmd, "@Now", DbType.DateTime, DateTime.Now);

                return cmd.ExecuteNonQuery();
            }
        }

        public int GetMemberIdByUserId(int userId)
        {
            if (userId <= 0)
                return 0;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT MemberID FROM [Member] WHERE UserID = @UserID";
                AddParameter(cmd, "@UserID", DbType.Int32, userId);

                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        public int GetUnavailableCopyIdForBook(int bookId)
        {
            if (bookId <= 0)
                return 0;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT TOP 1 bc.CopyID
                    FROM [BookCopy] bc
                    WHERE bc.BookID = @BookID 
                      AND bc.[Status] <> 'Available'
                      AND bc.CopyID NOT IN (
                          SELECT r.CopyID FROM [Reservation] r WHERE r.[Status] = 'Active'
                      )
                    ORDER BY bc.CopyID";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);

                cmd.Parameters.Clear();
                cmd.CommandText = @"
                    SELECT TOP 1 bc.CopyID
                    FROM [BookCopy] bc
                    WHERE bc.BookID = @BookID 
                      AND bc.[Status] <> 'Available'
                    ORDER BY bc.CopyID";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private Reservation MapReservation(IDataReader reader)
        {
            return new Reservation
            {
                ReservationID = reader.IsDBNull(reader.GetOrdinal("ReservationID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ReservationID")),
                CopyID = reader.IsDBNull(reader.GetOrdinal("CopyID")) ? 0 : reader.GetInt32(reader.GetOrdinal("CopyID")),
                MemberID = reader.IsDBNull(reader.GetOrdinal("MemberID")) ? 0 : reader.GetInt32(reader.GetOrdinal("MemberID")),
                ReservationDate = reader.IsDBNull(reader.GetOrdinal("ReservationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Active" : reader.GetString(reader.GetOrdinal("Status"))
            };
        }

        private void AddParameter(IDbCommand cmd, string name, DbType type, object value, int size = 0)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.DbType = type;
            if (size > 0) p.Size = size;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        public string GetBookCopyStatus(int copyId)
        {
            if (copyId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT [Status] FROM [BookCopy] WHERE CopyID = @CopyID";
                AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? null : result.ToString();
            }
        }

        public bool HasAnyCopies(int bookId)
        {
            if (bookId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(*) FROM [BookCopy] WHERE BookID = @BookID";
                AddParameter(cmd, "@BookID", DbType.Int32, bookId);

                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public bool SetExpirationDate(int reservationId, DateTime expirationDate)
        {
            if (reservationId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE [Reservation]
                    SET ExpirationDate = @ExpirationDate
                    WHERE ReservationID = @ReservationID";

                AddParameter(cmd, "@ReservationID", DbType.Int32, reservationId);
                AddParameter(cmd, "@ExpirationDate", DbType.DateTime, expirationDate);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public Reservation ActivateNextReservationInQueue(int copyId, int reservationPeriodDays)
        {
            if (copyId <= 0 || reservationPeriodDays <= 0)
                return null;

            // Get the BookID for this copy
            int bookId = GetBookIdByCopyId(copyId);
            if (bookId <= 0)
                return null;

            // Find the first reservation in queue for this book (any copy)
            var firstReservation = GetFirstInQueueByBookId(bookId);
            if (firstReservation == null)
                return null;

            // Only set expiration if not already set (prevents overwriting)
            if (!firstReservation.ExpirationDate.HasValue)
            {
                DateTime expirationDate = DateTime.Now.AddDays(reservationPeriodDays);
                if (SetExpirationDate(firstReservation.ReservationID, expirationDate))
                {
                    firstReservation.ExpirationDate = expirationDate;
                }
            }

            return firstReservation;
        }

        public int GetBookIdByCopyId(int copyId)
        {
            if (copyId <= 0)
                return 0;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT BookID FROM [BookCopy] WHERE CopyID = @CopyID";
                AddParameter(cmd, "@CopyID", DbType.Int32, copyId);

                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Gets all reserved books for a member with book info for display.
        /// </summary>
        public List<DTOReservedBookItem> GetReservedBooksForMember(int memberId)
        {
            var reservations = new List<DTOReservedBookItem>();

            if (memberId <= 0)
                return reservations;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT 
                        r.ReservationID,
                        r.CopyID,
                        b.BookID,
                        bc.AccessionNumber,
                        b.Title,
                        c.Name AS Category,
                        b.ResourceType,
                        b.CoverImage,
                        r.[Status],
                        r.ReservationDate,
                        r.ExpirationDate,
                        (SELECT COUNT(*) 
                         FROM [Reservation] r2 
                         INNER JOIN [BookCopy] bc2 ON r2.CopyID = bc2.CopyID
                         WHERE bc2.BookID = b.BookID 
                           AND r2.[Status] = 'Active' 
                           AND (r2.ReservationDate < r.ReservationDate 
                                OR (r2.ReservationDate = r.ReservationDate AND r2.ReservationID < r.ReservationID))) + 1 AS QueuePosition
                    FROM [Reservation] r
                    INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    LEFT JOIN [Category] c ON b.CategoryID = c.CategoryID
                    WHERE r.MemberID = @MemberID AND r.[Status] = 'Active'
                    ORDER BY r.ReservationDate DESC";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var expirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) 
                            ? (DateTime?)null 
                            : reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));

                        int daysUntilExpiration = 0;
                        bool isExpired = false;

                        if (expirationDate.HasValue)
                        {
                            daysUntilExpiration = (int)(expirationDate.Value.Date - DateTime.Now.Date).TotalDays;
                            isExpired = daysUntilExpiration < 0;
                        }

                        reservations.Add(new DTOReservedBookItem
                        {
                            ReservationID = reader.GetInt32(reader.GetOrdinal("ReservationID")),
                            CopyID = reader.GetInt32(reader.GetOrdinal("CopyID")),
                            BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                            AccessionNumber = reader.IsDBNull(reader.GetOrdinal("AccessionNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("AccessionNumber")),
                            Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? string.Empty : reader.GetString(reader.GetOrdinal("Title")),
                            Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? "N/A" : reader.GetString(reader.GetOrdinal("Category")),
                            ResourceType = reader.IsDBNull(reader.GetOrdinal("ResourceType")) ? string.Empty : reader.GetString(reader.GetOrdinal("ResourceType")),
                            CoverImage = reader.IsDBNull(reader.GetOrdinal("CoverImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("CoverImage")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Active" : reader.GetString(reader.GetOrdinal("Status")),
                            ReservationDate = reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                            ExpirationDate = expirationDate ?? DateTime.MinValue,
                            DaysUntilExpiration = daysUntilExpiration,
                            IsExpired = isExpired,
                            QueuePosition = reader.GetInt32(reader.GetOrdinal("QueuePosition"))
                        });
                    }
                }
            }

            return reservations;
        }

        /// <summary>
        /// Checks if a member currently has an active borrow for any copy of a book.
        /// </summary>
        public bool HasActiveBorrowForBook(int bookId, int memberId)
        {
            if (bookId <= 0 || memberId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Check BorrowingTransaction for any active (unreturned) borrow
                // where the copy belongs to the specified book
                cmd.CommandText = @"
                    SELECT COUNT(*)
                    FROM [BorrowingTransaction] bt
                    INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                    WHERE bc.BookID = @BookID
                      AND bt.MemberID = @MemberID
                      AND bt.ReturnDate IS NULL
                      AND bt.[Status] IN ('Borrowed', 'Overdue')";

                AddParameter(cmd, "@BookID", DbType.Int32, bookId);
                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var result = cmd.ExecuteScalar();
                int count = result != null ? Convert.ToInt32(result) : 0;
                return count > 0;
            }
        }
    }
}
