using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;

namespace LMS.BusinessLogic.Managers
{
    /// <summary>
    /// Manages report-related operations. 
    /// </summary>
    public class ReportManager : IReportManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly DbConnection _db;

        public ReportManager() : this(new BookRepository(), new DbConnection())
        {
        }

        public ReportManager(IBookRepository bookRepository, DbConnection db)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int GetTotalBooks()
        {
            try
            {
                var books = _bookRepository.GetAll();
                return books?.Count ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public int GetTotalBorrowedBooks()
        {
            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"
                        SELECT COUNT(*) 
                        FROM [BorrowingTransaction] 
                        WHERE ReturnDate IS NULL 
                          AND [Status] IN ('Borrowed', 'Overdue')";
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int GetTotalReservedBooks()
        {
            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT COUNT(*) FROM [Reservation] WHERE [Status] = 'Active'";
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int GetTotalOverdueBooks()
        {
            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"
                        SELECT COUNT(*) 
                        FROM [BorrowingTransaction] 
                        WHERE ReturnDate IS NULL 
                          AND (Status = 'Overdue' OR DueDate < @Today)";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@Today";
                    p.DbType = DbType.DateTime;
                    p.Value = DateTime.Today;
                    cmd.Parameters.Add(p);

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public decimal GetTotalUnpaidFines()
        {
            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT ISNULL(SUM(FineAmount), 0) FROM [Fine] WHERE [Status] = 'Unpaid'";
                    var result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                }
            }
            catch
            {
                return 0m;
            }
        }   

        /// <summary>
        /// Returns top N users ordered by LastLogin descending (most recent first).
        /// Key = display name (FirstName + ' ' + LastName or Username), Value = LastLogin DateTime.
        /// </summary>
        public Dictionary<string, DateTime> GetMostActiveUsers(int top = 8)
        {
            var result = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    // Get users with non-null LastLogin ordered by most recent
                    cmd.CommandText = @"
                        SELECT Username, FirstName, LastName, LastLogin
                        FROM [User]
                        WHERE LastLogin IS NOT NULL
                        ORDER BY LastLogin DESC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read() && count < top)
                        {
                            var lastLogin = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);
                            if (!lastLogin.HasValue) continue;

                            string username = reader.IsDBNull(0) ? null : reader.GetString(0);
                            string first = reader.IsDBNull(1) ? null : reader.GetString(1);
                            string last = reader.IsDBNull(2) ? null : reader.GetString(2);

                            string display = !string.IsNullOrWhiteSpace(first) || !string.IsNullOrWhiteSpace(last)
                                ? $"{(first ?? "").Trim()} {(last ?? "").Trim()}".Trim()
                                : (username ?? "Unknown");

                            if (string.IsNullOrWhiteSpace(display))
                                display = "Unknown";

                            if (!result.ContainsKey(display))
                            {
                                result[display] = lastLogin.Value;
                                count++;
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore and return partial/empty
            }

            return result;
        }

        /// <summary>
        /// Returns usage counts grouped by user Role. Key = Role name, Value = count of users with LastLogin (non-null).
        /// Implements IReportManager.GetUsageByRole.
        /// </summary>
        public Dictionary<string, int> GetUsageByRole()
        {
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = @"
                        SELECT ISNULL([Role], 'Unknown') AS RoleName, COUNT(*) AS UserCount
                        FROM [User]
                        WHERE LastLogin IS NOT NULL
                        GROUP BY [Role]
                        ORDER BY UserCount DESC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string role = reader.IsDBNull(0) ? "Unknown" : reader.GetString(0);
                            int count = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            if (string.IsNullOrWhiteSpace(role)) role = "Unknown";
                            result[role] = count;
                        }
                    }
                }
            }
            catch
            {
                // return partial/empty on error
            }

            return result;
        }

        public Dictionary<string, int> GetBorrowingTrendByWeek(int weeks = 8)
        {
            var result = new Dictionary<string, int>();
            try
            {
                var today = DateTime.Today;
                var weekStarts = new List<DateTime>();
                var weekLabels = new List<string>();
                for (int i = weeks - 1; i >= 0; i--)
                {
                    var weekStart = today.AddDays(-(int)today.DayOfWeek - (7 * i));
                    weekStarts.Add(weekStart);
                    weekLabels.Add(weekStart.ToString("MMM dd", CultureInfo.InvariantCulture));
                    result[weekStart.ToString("MMM dd", CultureInfo.InvariantCulture)] = 0;
                }

                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    var startDate = weekStarts.First();
                    var endDate = today.AddDays(1);

                    cmd.CommandText = @"
                        SELECT BorrowDate
                        FROM [BorrowingTransaction]
                        WHERE BorrowDate >= @StartDate AND BorrowDate < @EndDate";
                    AddParameter(cmd, "@StartDate", DbType.DateTime, startDate);
                    AddParameter(cmd, "@EndDate", DbType.DateTime, endDate);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0)) continue;
                            var borrowDate = reader.GetDateTime(0);
                            for (int i = 0; i < weekStarts.Count; i++)
                            {
                                var ws = weekStarts[i];
                                var we = ws.AddDays(7);
                                if (borrowDate >= ws && borrowDate < we)
                                {
                                    var label = ws.ToString("MMM dd", CultureInfo.InvariantCulture);
                                    result[label]++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public Dictionary<string, int> GetBorrowingByMemberType()
        {
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            try
            {
                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"
                        SELECT ISNULL(mt.TypeName, 'Unknown') AS MemberTypeName, COUNT(bt.TransactionID) AS BorrowCount
                        FROM [BorrowingTransaction] bt
                        INNER JOIN [Member] m ON bt.MemberID = m.MemberID
                        LEFT JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                        GROUP BY mt.TypeName
                        ORDER BY BorrowCount DESC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string typeName = reader.IsDBNull(0) ? "Unknown" : reader.GetString(0);
                            int count = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            if (string.IsNullOrWhiteSpace(typeName)) typeName = "Unknown";
                            result[typeName] = count;
                        }
                    }
                }
            }
            catch { }
            return result;
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
