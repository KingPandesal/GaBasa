using System;
using System.Data;
using System.Collections.Generic;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;
using LMS.Model.Models.Enums;

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

        public bool UsernameExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT COUNT(1) FROM [User] WHERE Username = @Username";
                AddParameter(cmd, "@Username", DbType.String, username, 256);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public int? GetMemberTypeIdByName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT MemberTypeID FROM [MemberType] WHERE TypeName = @TypeName";
                AddParameter(cmd, "@TypeName", DbType.String, typeName, 100);

                var result = cmd.ExecuteScalar();
                return result != null ? (int?)Convert.ToInt32(result) : null;
            }
        }

        public int AddMember(string firstName, string lastName, string email, string contactNumber,
            string username, string passwordHash, string photoPath, string address,
            string validIdPath, int memberTypeId)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int userId;

                        // Insert into User table
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = @"
                                INSERT INTO [User] 
                                (Username, [Password], [Role], [Status], FirstName, LastName, Email, ContactNumber, Photo)
                                VALUES (@Username, @Password, 'Member', 'Active', @FirstName, @LastName, @Email, @ContactNumber, @Photo);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            AddParameter(cmd, "@Username", DbType.String, username, 256);
                            AddParameter(cmd, "@Password", DbType.String, passwordHash, 256);
                            AddParameter(cmd, "@FirstName", DbType.String, firstName, 100);
                            AddParameter(cmd, "@LastName", DbType.String, lastName, 100);
                            AddParameter(cmd, "@Email", DbType.String, email, 256);
                            AddParameter(cmd, "@ContactNumber", DbType.String, contactNumber, 20);
                            AddParameter(cmd, "@Photo", DbType.String, photoPath, 500);

                            userId = (int)cmd.ExecuteScalar();
                        }

                        // Insert into Member table
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = @"
                                INSERT INTO [Member] 
                                (UserID, MemberTypeID, [Address], RegistrationDate, ExpirationDate, ValidID, [Status])
                                VALUES (@UserID, @MemberTypeID, @Address, @RegistrationDate, @ExpirationDate, @ValidID, 'Active');
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            AddParameter(cmd, "@UserID", DbType.Int32, userId);
                            AddParameter(cmd, "@MemberTypeID", DbType.Int32, memberTypeId);
                            AddParameter(cmd, "@Address", DbType.String, address, 500);
                            AddParameter(cmd, "@RegistrationDate", DbType.DateTime, DateTime.Now);
                            AddParameter(cmd, "@ExpirationDate", DbType.DateTime, DateTime.Now.AddYears(1));
                            AddParameter(cmd, "@ValidID", DbType.String, validIdPath, 500);

                            cmd.ExecuteScalar();
                        }

                        transaction.Commit();
                        return userId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public MemberStatus? GetMemberStatusByUserId(int userId)
        {
            if (userId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT [Status] FROM [Member] WHERE UserID = @UserID";
                AddParameter(cmd, "@UserID", DbType.Int32, userId);

                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return null;

                string statusString = result.ToString();
                if (Enum.TryParse<MemberStatus>(statusString, true, out var status))
                    return status;

                return null;
            }
        }

        public List<DTOFetchAllMembers> GetAllMembers()
        {
            var members = new List<DTOFetchAllMembers>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
                    SELECT 
                        m.MemberID,
                        u.FirstName,
                        u.LastName,
                        mt.TypeName AS MemberType,
                        u.Username,
                        u.Email,
                        m.[Address],
                        u.ContactNumber,
                        mt.MaxBooksAllowed,
                        mt.BorrowingPeriod,
                        mt.RenewalLimit,
                        mt.ReservationPrivilege,
                        mt.FineRate,
                        m.RegistrationDate,
                        m.ExpirationDate,
                        u.LastLogin,
                        m.[Status]
                    FROM [Member] m
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    INNER JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    ORDER BY m.MemberID DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? "" : reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? "" : reader.GetString(reader.GetOrdinal("LastName"));
                        DateTime? lastLogin = reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastLogin"));

                        members.Add(new DTOFetchAllMembers
                        {
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            FullName = $"{firstName} {lastName}".Trim(),
                            MemberType = reader.IsDBNull(reader.GetOrdinal("MemberType")) ? "" : reader.GetString(reader.GetOrdinal("MemberType")),
                            Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "" : reader.GetString(reader.GetOrdinal("Username")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : reader.GetString(reader.GetOrdinal("Address")),
                            ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? "" : reader.GetString(reader.GetOrdinal("ContactNumber")),
                            MaxBooksAllowed = reader.IsDBNull(reader.GetOrdinal("MaxBooksAllowed")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaxBooksAllowed")),
                            BorrowingPeriod = reader.IsDBNull(reader.GetOrdinal("BorrowingPeriod")) ? 0 : reader.GetInt32(reader.GetOrdinal("BorrowingPeriod")),
                            RenewalLimit = reader.IsDBNull(reader.GetOrdinal("RenewalLimit")) ? 0 : reader.GetInt32(reader.GetOrdinal("RenewalLimit")),
                            ReservationPrivilege = !reader.IsDBNull(reader.GetOrdinal("ReservationPrivilege")) && reader.GetBoolean(reader.GetOrdinal("ReservationPrivilege")),
                            FineRate = reader.IsDBNull(reader.GetOrdinal("FineRate")) ? 0m : reader.GetDecimal(reader.GetOrdinal("FineRate")),
                            RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                            ExpirationDate = reader.IsDBNull(reader.GetOrdinal("ExpirationDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                            LastLogin = lastLogin.HasValue ? lastLogin.Value.ToString("MMM dd, yyyy hh:mm tt") : "Never",
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }
                }
            }

            return members;
        }

        public DTOEditMember GetMemberForEdit(int memberId)
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
                        m.UserID,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        m.[Address],
                        u.ContactNumber,
                        mt.TypeName AS MemberTypeName,
                        u.Photo,
                        m.ValidID,
                        m.[Status]
                    FROM [Member] m
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    INNER JOIN [MemberType] mt ON m.MemberTypeID = mt.MemberTypeID
                    WHERE m.MemberID = @MemberID";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    string statusString = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Active" : reader.GetString(reader.GetOrdinal("Status"));
                    MemberStatus status = MemberStatus.Active;
                    Enum.TryParse(statusString, true, out status);

                    return new DTOEditMember
                    {
                        MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? "" : reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? "" : reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString(reader.GetOrdinal("Email")),
                        Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : reader.GetString(reader.GetOrdinal("Address")),
                        ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? "" : reader.GetString(reader.GetOrdinal("ContactNumber")),
                        MemberTypeName = reader.IsDBNull(reader.GetOrdinal("MemberTypeName")) ? "" : reader.GetString(reader.GetOrdinal("MemberTypeName")),
                        PhotoPath = reader.IsDBNull(reader.GetOrdinal("Photo")) ? "" : reader.GetString(reader.GetOrdinal("Photo")),
                        ValidIdPath = reader.IsDBNull(reader.GetOrdinal("ValidID")) ? "" : reader.GetString(reader.GetOrdinal("ValidID")),
                        Status = status
                    };
                }
            }
        }

        public bool UpdateMember(DTOEditMember dto)
        {
            if (dto == null || dto.MemberID <= 0)
                return false;

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Get UserID from MemberID
                        int userId;
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = "SELECT UserID FROM [Member] WHERE MemberID = @MemberID";
                            AddParameter(cmd, "@MemberID", DbType.Int32, dto.MemberID);
                            var result = cmd.ExecuteScalar();
                            if (result == null)
                                return false;
                            userId = Convert.ToInt32(result);
                        }

                        // Get MemberTypeID from TypeName
                        int? memberTypeId = null;
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = "SELECT MemberTypeID FROM [MemberType] WHERE TypeName = @TypeName";
                            AddParameter(cmd, "@TypeName", DbType.String, dto.MemberTypeName, 100);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                                memberTypeId = Convert.ToInt32(result);
                        }

                        // Update User table
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = @"UPDATE [User] 
                                SET FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    Email = @Email, 
                                    ContactNumber = @ContactNumber, 
                                    Photo = @Photo
                                WHERE UserID = @UserID";

                            AddParameter(cmd, "@UserID", DbType.Int32, userId);
                            AddParameter(cmd, "@FirstName", DbType.String, dto.FirstName, 100);
                            AddParameter(cmd, "@LastName", DbType.String, dto.LastName, 100);
                            AddParameter(cmd, "@Email", DbType.String, dto.Email, 256);
                            AddParameter(cmd, "@ContactNumber", DbType.String, dto.ContactNumber, 20);
                            AddParameter(cmd, "@Photo", DbType.String, dto.PhotoPath, 500);

                            cmd.ExecuteNonQuery();
                        }

                        // Update Member table
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = @"UPDATE [Member] 
                                SET [Address] = @Address,
                                    ValidID = @ValidID,
                                    [Status] = @Status,
                                    MemberTypeID = @MemberTypeID
                                WHERE MemberID = @MemberID";

                            AddParameter(cmd, "@MemberID", DbType.Int32, dto.MemberID);
                            AddParameter(cmd, "@Address", DbType.String, dto.Address, 500);
                            AddParameter(cmd, "@ValidID", DbType.String, dto.ValidIdPath, 500);
                            AddParameter(cmd, "@Status", DbType.String, dto.Status.ToString(), 50);
                            AddParameter(cmd, "@MemberTypeID", DbType.Int32, memberTypeId);

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

        public void UpdateExpiredMembers()
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE [Member] 
                    SET [Status] = 'Expired' 
                    WHERE ExpirationDate < @Today 
                    AND [Status] = 'Active'";

                AddParameter(cmd, "@Today", DbType.DateTime, DateTime.Now.Date);
                cmd.ExecuteNonQuery();
            }
        }

        public DateTime? GetExpirationDateByUserId(int userId)
        {
            if (userId <= 0)
                return null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT ExpirationDate FROM [Member] WHERE UserID = @UserID";
                AddParameter(cmd, "@UserID", DbType.Int32, userId);

                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return null;

                return Convert.ToDateTime(result);
            }
        }

        public bool UpdateMemberStatusByUserId(int userId, MemberStatus status)
        {
            if (userId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "UPDATE [Member] SET [Status] = @Status WHERE UserID = @UserID";
                AddParameter(cmd, "@UserID", DbType.Int32, userId);
                AddParameter(cmd, "@Status", DbType.String, status.ToString(), 50);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateMemberStatusByMemberId(int memberId, MemberStatus status)
        {
            if (memberId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "UPDATE [Member] SET [Status] = @Status WHERE MemberID = @MemberID";
                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                AddParameter(cmd, "@Status", DbType.String, status.ToString(), 50);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public int GetUnresolvedViolationCount(int memberId)
        {
            if (memberId <= 0)
                return 0;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                // Assumes a Violation table exists with IsResolved column
                cmd.CommandText = @"
                    SELECT COUNT(*) 
                    FROM [Violation] 
                    WHERE MemberID = @MemberID 
                    AND IsResolved = 0";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);

                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public void SuspendMembersWithExcessiveViolations(int violationThreshold)
        {
            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                // Suspend members who have more than threshold unresolved violations
                cmd.CommandText = @"
                    UPDATE [Member] 
                    SET [Status] = 'Suspended' 
                    WHERE MemberID IN (
                        SELECT MemberID 
                        FROM [Violation] 
                        WHERE IsResolved = 0 
                        GROUP BY MemberID 
                        HAVING COUNT(*) >= @Threshold
                    )
                    AND [Status] = 'Active'";

                AddParameter(cmd, "@Threshold", DbType.Int32, violationThreshold);
                cmd.ExecuteNonQuery();
            }
        }

        public bool RenewMembership(int memberId)
        {
            if (memberId <= 0)
                return false;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE [Member] 
                    SET RegistrationDate = @RegistrationDate,
                        ExpirationDate = @ExpirationDate,
                        [Status] = 'Active'
                    WHERE MemberID = @MemberID";

                AddParameter(cmd, "@MemberID", DbType.Int32, memberId);
                AddParameter(cmd, "@RegistrationDate", DbType.DateTime, DateTime.Now.Date);
                AddParameter(cmd, "@ExpirationDate", DbType.DateTime, DateTime.Now.Date.AddYears(1));

                return cmd.ExecuteNonQuery() > 0;
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
