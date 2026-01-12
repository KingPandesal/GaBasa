using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DbConnection _db;

        public AuditLogRepository() : this(new DbConnection()) { }

        public AuditLogRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Add(AuditLog log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    INSERT INTO [AuditLog] (UserID, ModuleName, ActionPerformed, Details, Timestamp)
                    VALUES (@UserID, @ModuleName, @ActionPerformed, @Details, @Timestamp);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@UserID", DbType.Int32, log.UserID);
                AddParameter(cmd, "@ModuleName", DbType.String, log.ModuleName ?? string.Empty);
                AddParameter(cmd, "@ActionPerformed", DbType.String, log.ActionPerformed ?? string.Empty);
                AddParameter(cmd, "@Details", DbType.String, log.Details ?? string.Empty);
                AddParameter(cmd, "@Timestamp", DbType.DateTime, log.Timestamp == DateTime.MinValue ? DateTime.UtcNow : log.Timestamp);

                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        public AuditLog GetById(int logId)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT LogID, UserID, ModuleName, ActionPerformed, Details, Timestamp FROM [AuditLog] WHERE LogID = @LogID";
                AddParameter(cmd, "@LogID", DbType.Int32, logId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapFromReader(reader);
                }
            }
            return null;
        }

        public List<AuditLog> GetByUserId(int userId)
        {
            var logs = new List<AuditLog>();
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT LogID, UserID, ModuleName, ActionPerformed, Details, Timestamp FROM [AuditLog] WHERE UserID = @UserID ORDER BY Timestamp DESC";
                AddParameter(cmd, "@UserID", DbType.Int32, userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        logs.Add(MapFromReader(reader));
                }
            }
            return logs;
        }

        public List<AuditLog> GetAll()
        {
            var logs = new List<AuditLog>();
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT LogID, UserID, ModuleName, ActionPerformed, Details, Timestamp FROM [AuditLog] ORDER BY Timestamp DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        logs.Add(MapFromReader(reader));
                }
            }
            return logs;
        }

        private AuditLog MapFromReader(IDataReader reader)
        {
            return new AuditLog
            {
                LogID = reader.GetInt32(reader.GetOrdinal("LogID")),
                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                ModuleName = reader.IsDBNull(reader.GetOrdinal("ModuleName")) ? null : reader.GetString(reader.GetOrdinal("ModuleName")),
                ActionPerformed = reader.IsDBNull(reader.GetOrdinal("ActionPerformed")) ? null : reader.GetString(reader.GetOrdinal("ActionPerformed")),
                Details = reader.IsDBNull(reader.GetOrdinal("Details")) ? null : reader.GetString(reader.GetOrdinal("Details")),
                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
            };
        }

        private void AddParameter(IDbCommand cmd, string name, DbType type, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.DbType = type;
            param.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }
    }
}
