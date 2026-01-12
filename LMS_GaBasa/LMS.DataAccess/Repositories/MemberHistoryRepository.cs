using System;
using System.Collections.Generic;
using System.Data;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.MemberFeatures.History;

namespace LMS.DataAccess.Repositories
{
    /// <summary>
    /// Concrete implementation of IMemberHistoryRepository.
    /// Encapsulates SQL queries that produce unified history DTOs for UI consumption.
    /// </summary>
    public class MemberHistoryRepository : IMemberHistoryRepository
    {
        private readonly DbConnection _db;

        public MemberHistoryRepository() : this(new DbConnection()) { }

        public MemberHistoryRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public List<DTOHistoryItem> GetBorrowingHistory(int memberId)
        {
            var result = new List<DTOHistoryItem>();
            if (memberId <= 0) return result;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        bt.TransactionID,
                        b.Title,
                        bc.AccessionNumber,
                        bt.BorrowDate,
                        bt.ReturnDate,
                        bt.[Status]
                    FROM [BorrowingTransaction] bt
                    INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    WHERE bt.MemberID = @MemberID
                    ORDER BY bt.BorrowDate DESC, ISNULL(bt.ReturnDate, '1900-01-01') DESC";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string title = r.IsDBNull(r.GetOrdinal("Title")) ? "" : r.GetString(r.GetOrdinal("Title"));
                        string accession = r.IsDBNull(r.GetOrdinal("AccessionNumber")) ? "" : r.GetString(r.GetOrdinal("AccessionNumber"));
                        DateTime borrowDate = r.IsDBNull(r.GetOrdinal("BorrowDate")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("BorrowDate"));
                        DateTime? returnDate = r.IsDBNull(r.GetOrdinal("ReturnDate")) ? (DateTime?)null : r.GetDateTime(r.GetOrdinal("ReturnDate"));
                        string status = r.IsDBNull(r.GetOrdinal("Status")) ? "" : r.GetString(r.GetOrdinal("Status"));

                        // Borrow event
                        result.Add(new DTOHistoryItem
                        {
                            Title = title,
                            TransactionDate = borrowDate,
                            Status = "Borrowed",
                            Details = string.IsNullOrWhiteSpace(accession) ? null : $"Accession: {accession}"
                        });

                        // Return event (if present)
                        if (returnDate.HasValue)
                        {
                            result.Add(new DTOHistoryItem
                            {
                                Title = title,
                                TransactionDate = returnDate.Value,
                                Status = "Returned",
                                Details = string.IsNullOrWhiteSpace(accession) ? null : $"Accession: {accession}"
                            });
                        }
                    }
                }
            }

            return result;
        }

        public List<DTOHistoryItem> GetReservationHistory(int memberId)
        {
            var result = new List<DTOHistoryItem>();
            if (memberId <= 0) return result;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT r.ReservationID, b.Title, r.ReservationDate, r.[Status]
                    FROM [Reservation] r
                    INNER JOIN [BookCopy] bc ON r.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    WHERE r.MemberID = @MemberID
                    ORDER BY r.ReservationDate DESC";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string title = r.IsDBNull(r.GetOrdinal("Title")) ? "" : r.GetString(r.GetOrdinal("Title"));
                        DateTime resDate = r.IsDBNull(r.GetOrdinal("ReservationDate")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("ReservationDate"));
                        string status = r.IsDBNull(r.GetOrdinal("Status")) ? "" : r.GetString(r.GetOrdinal("Status"));

                        string displayStatus;
                        switch (status.Trim().ToLowerInvariant())
                        {
                            case "pending":
                            case "active":
                            case "ready":
                                displayStatus = "Reserved";
                                break;
                            case "cancelled":
                                displayStatus = "Reservation Cancelled";
                                break;
                            case "completed":
                                displayStatus = "Reservation Completed";
                                break;
                            case "expired":
                                displayStatus = "Reservation Expired";
                                break;
                            default:
                                displayStatus = $"Reserved ({status})";
                                break;
                        }

                        result.Add(new DTOHistoryItem
                        {
                            Title = title,
                            TransactionDate = resDate,
                            Status = displayStatus,
                            Details = null
                        });
                    }
                }
            }

            return result;
        }

        public List<DTOHistoryItem> GetFinePaymentHistory(int memberId)
        {
            var result = new List<DTOHistoryItem>();
            if (memberId <= 0) return result;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT p.PaymentID, f.FineType, p.PaymentDate, p.AmountPaid
                    FROM [Payment] p
                    INNER JOIN [Fine] f ON p.FineID = f.FineID
                    WHERE f.MemberID = @MemberID
                    ORDER BY p.PaymentDate DESC";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string fineType = r.IsDBNull(r.GetOrdinal("FineType")) ? "Fine" : r.GetString(r.GetOrdinal("FineType"));
                        DateTime paid = r.IsDBNull(r.GetOrdinal("PaymentDate")) ? DateTime.MinValue : r.GetDateTime(r.GetOrdinal("PaymentDate"));
                        decimal amt = r.IsDBNull(r.GetOrdinal("AmountPaid")) ? 0m : r.GetDecimal(r.GetOrdinal("AmountPaid"));

                        result.Add(new DTOHistoryItem
                        {
                            Title = $"{ToFriendlyFineType(fineType)} - ₱{amt:N2}",
                            TransactionDate = paid,
                            Status = "Fine Paid",
                            Details = null
                        });
                    }
                }
            }

            return result;
        }

        private string ToFriendlyFineType(string fineType)
        {
            if (string.IsNullOrWhiteSpace(fineType)) return "Fine";
            switch (fineType.Trim().ToLowerInvariant())
            {
                case "overdue": return "Overdue Fine";
                case "lost": return "Lost Book Fine";
                case "damaged": return "Damaged Book Fine";
                case "cardreplacement": return "ID Card Replacement";
                default: return fineType;
            }
        }

        private void AddParameter(IDbCommand cmd, string name, DbType dbType, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.DbType = dbType;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}
