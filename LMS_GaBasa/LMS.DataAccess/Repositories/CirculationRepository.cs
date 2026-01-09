using System;
using System.Data;
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
                        mt.FineRate
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
                        MaxBooksAllowed = reader.GetInt32(reader.GetOrdinal("MaxBooksAllowed")),
                        FineRate = reader.GetDecimal(reader.GetOrdinal("FineRate"))
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
