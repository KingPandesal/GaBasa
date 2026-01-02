using System;
using System.Data;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;

namespace LMS.DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DbConnection _db;

        public MemberRepository() : this(new DbConnection()) { }

        public MemberRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public MemberProfileDto GetMemberProfileByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        u.UserID, u.Username, u.FirstName, u.LastName, u.Email, 
                        u.ContactNumber, u.Photo, u.[Role], u.[Status],
                        m.MemberID, m.[Address], m.RegistrationDate, m.ExpirationDate, 
                        m.ValidID, m.[Status] AS MemberStatus,
                        mt.TypeName, mt.MaxBooksAllowed, mt.BorrowingPeriod, 
                        mt.RenewalLimit, mt.ReservationPrivilege, mt.FineRate
                    FROM [User] u
                    INNER JOIN [Member] m ON u.UserID = m.UserID
                    INNER JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    WHERE u.UserID = @UserID";

                var p = cmd.CreateParameter();
                p.ParameterName = "@UserID";
                p.DbType = DbType.Int32;
                p.Value = userId;
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new MemberProfileDto
                    {
                        UserID = reader.IsDBNull(reader.GetOrdinal("UserID")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserID")),
                        Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString(reader.GetOrdinal("Username")),
                        FullName = $"{(reader.IsDBNull(reader.GetOrdinal("FirstName")) ? "" : reader.GetString(reader.GetOrdinal("FirstName")))} {(reader.IsDBNull(reader.GetOrdinal("LastName")) ? "" : reader.GetString(reader.GetOrdinal("LastName")))}".Trim(),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? null : reader.GetString(reader.GetOrdinal("ContactNumber")),
                        PhotoPath = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : reader.GetString(reader.GetOrdinal("Photo")),
                        Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? null : reader.GetString(reader.GetOrdinal("Role")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                        Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                        RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                        ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                        ValidIdPath = reader.IsDBNull(reader.GetOrdinal("ValidID")) ? null : reader.GetString(reader.GetOrdinal("ValidID")),
                        MemberStatus = reader.IsDBNull(reader.GetOrdinal("MemberStatus")) ? null : reader.GetString(reader.GetOrdinal("MemberStatus")),
                        MemberTypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : reader.GetString(reader.GetOrdinal("TypeName")),
                        MaxBooksAllowed = reader.IsDBNull(reader.GetOrdinal("MaxBooksAllowed")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaxBooksAllowed")),
                        BorrowingPeriod = reader.IsDBNull(reader.GetOrdinal("BorrowingPeriod")) ? 0 : reader.GetInt32(reader.GetOrdinal("BorrowingPeriod")),
                        RenewalLimit = reader.IsDBNull(reader.GetOrdinal("RenewalLimit")) ? 0 : reader.GetInt32(reader.GetOrdinal("RenewalLimit")),
                        ReservationPrivilege = !reader.IsDBNull(reader.GetOrdinal("ReservationPrivilege")) && reader.GetBoolean(reader.GetOrdinal("ReservationPrivilege")),
                        FineRate = reader.IsDBNull(reader.GetOrdinal("FineRate")) ? 0m : reader.GetDecimal(reader.GetOrdinal("FineRate"))
                    };
                }
            }
        }

        public bool UpdateMemberProfile(int userId, string firstName, string lastName, string email,
            string contactNumber, string photoPath, string address, string validIdPath, string username)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Update User table (including username)
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = @"UPDATE [User] 
                                SET FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    Email = @Email, 
                                    ContactNumber = @ContactNumber, 
                                    Photo = @Photo,
                                    Username = @Username
                                WHERE UserID = @UserID";

                            AddParameter(cmd, "@UserID", DbType.Int32, userId);
                            AddParameter(cmd, "@FirstName", DbType.String, firstName, 100);
                            AddParameter(cmd, "@LastName", DbType.String, lastName, 100);
                            AddParameter(cmd, "@Email", DbType.String, email, 256);
                            AddParameter(cmd, "@ContactNumber", DbType.String, contactNumber, 20);
                            AddParameter(cmd, "@Photo", DbType.String, photoPath, 500);
                            AddParameter(cmd, "@Username", DbType.String, username, 256);

                            cmd.ExecuteNonQuery();
                        }

                        // Update Member table (Address and ValidID)
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = @"UPDATE [Member] 
                                SET [Address] = @Address,
                                    ValidID = @ValidID
                                WHERE UserID = @UserID";

                            AddParameter(cmd, "@UserID", DbType.Int32, userId);
                            AddParameter(cmd, "@Address", DbType.String, address, 500);
                            AddParameter(cmd, "@ValidID", DbType.String, validIdPath, 500);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool UpdateMemberPassword(int userId, string newPasswordHash)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"UPDATE [User] SET [Password] = @Password WHERE UserID = @UserID";

                AddParameter(cmd, "@UserID", DbType.Int32, userId);
                AddParameter(cmd, "@Password", DbType.String, newPasswordHash, 256);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public string GetPasswordHash(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"SELECT [Password] FROM [User] WHERE UserID = @UserID";

                AddParameter(cmd, "@UserID", DbType.Int32, userId);

                var result = cmd.ExecuteScalar();
                return result as string;
            }
        }

        public bool UsernameExistsForOtherUser(int userId, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(1) FROM [User] WHERE Username = @Username AND UserID != @UserID";
                AddParameter(cmd, "@Username", DbType.String, username, 256);
                AddParameter(cmd, "@UserID", DbType.Int32, userId);
                return (int)cmd.ExecuteScalar() > 0;
            }
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
