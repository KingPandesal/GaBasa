using System;
using System.Collections.Generic;
using System.Data;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Transactions;

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
                AddParameter(cmd, "@ExpirationDate", DbType.DateTime, reservation.ExpirationDate);
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

        public bool HasActiveReservationForBook(int bookId, int memberId)
        {
            if (bookId <= 0 || memberId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                // Check if member has an active reservation for any copy of this book
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
                // Get a copy that is NOT Available (e.g., Borrowed, Reserved, etc.)
                // Prefer copies that are not already reserved by checking Reservation table
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

                // Fallback: get any copy (even if already reserved, for edge cases)
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
                ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
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
    }
}
