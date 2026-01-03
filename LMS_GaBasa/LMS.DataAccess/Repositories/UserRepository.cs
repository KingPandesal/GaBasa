using LMS.DataAccess.Database;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnection _db;

        public UserRepository() : this(new DbConnection()) { }

        public UserRepository(DbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public User GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username is required", nameof(username));

            User user = null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "SELECT UserID, Username, [Password], [Role], [Status], FirstName, LastName " +
                                  "FROM [User] WHERE Username = @Username";

                var p = cmd.CreateParameter();
                p.ParameterName = "@Username";
                p.DbType = DbType.String;
                p.Size = 256;
                p.Value = username;
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    int userId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    string dbUsername = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string passwordHash = reader.IsDBNull(2) ? null : reader.GetString(2);
                    string role = reader.IsDBNull(3) ? null : reader.GetString(3);
                    string status = reader.IsDBNull(4) ? null : reader.GetString(4);
                    string firstName = reader.IsDBNull(5) ? null : reader.GetString(5);
                    string lastName = reader.IsDBNull(6) ? null : reader.GetString(6);

                    // Centralized role -> User mapping
                    user = CreateUserFromRoleString(role);
                    if (user == null)
                        return null; // unknown role

                    user.UserID = userId;
                    user.Username = dbUsername;
                    if (!string.IsNullOrEmpty(passwordHash))
                        user.SetPasswordHash(passwordHash);
                    user.FirstName = firstName;
                    user.LastName = lastName;

                    // Map status string to UserStatus enum
                    if (!string.IsNullOrEmpty(status) &&
                        Enum.TryParse<UserStatus>(status, true, out var parsedStatus))
                    {
                        user.Status = parsedStatus;
                    }
                    else
                    {
                        user.Status = UserStatus.Inactive;
                    }
                }
            }

            return user;
        }

        public User GetById(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            User user = null;

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "SELECT UserID, Username, [Password], [Role], [Status], FirstName, LastName, Email, ContactNumber, Photo " +
                                  "FROM [User] WHERE UserID = @UserID";

                var p = cmd.CreateParameter();
                p.ParameterName = "@UserID";
                p.DbType = DbType.Int32;
                p.Value = userId;
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    int dbUserId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    string dbUsername = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string passwordHash = reader.IsDBNull(2) ? null : reader.GetString(2);
                    string role = reader.IsDBNull(3) ? null : reader.GetString(3);
                    string status = reader.IsDBNull(4) ? null : reader.GetString(4);
                    string firstName = reader.IsDBNull(5) ? null : reader.GetString(5);
                    string lastName = reader.IsDBNull(6) ? null : reader.GetString(6);
                    string email = reader.IsDBNull(7) ? null : reader.GetString(7);
                    string contactNumber = reader.IsDBNull(8) ? null : reader.GetString(8);
                    string photo = reader.IsDBNull(9) ? null : reader.GetString(9);

                    user = CreateUserFromRoleString(role);
                    if (user == null)
                        return null;

                    user.UserID = dbUserId;
                    user.Username = dbUsername;
                    if (!string.IsNullOrEmpty(passwordHash))
                        user.SetPasswordHash(passwordHash);
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.Email = email;
                    user.ContactNumber = contactNumber;
                    user.PhotoPath = photo;

                    if (!string.IsNullOrEmpty(status) &&
                        Enum.TryParse<UserStatus>(status, true, out var parsedStatus))
                    {
                        user.Status = parsedStatus;
                    }
                    else
                    {
                        user.Status = UserStatus.Inactive;
                    }
                }
            }

            return user;
        }

        public bool UpdateProfile(int userId, string firstName, string lastName, string email, string contactNumber, string photoPath)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"UPDATE [User] 
                            SET FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                ContactNumber = @ContactNumber, 
                                Photo = @Photo 
                            WHERE UserID = @UserID";

                var pUserId = cmd.CreateParameter();
                pUserId.ParameterName = "@UserID";
                pUserId.DbType = DbType.Int32;
                pUserId.Value = userId;
                cmd.Parameters.Add(pUserId);

                var pFirstName = cmd.CreateParameter();
                pFirstName.ParameterName = "@FirstName";
                pFirstName.DbType = DbType.String;
                pFirstName.Size = 100;
                pFirstName.Value = (object)firstName ?? DBNull.Value;
                cmd.Parameters.Add(pFirstName);

                var pLastName = cmd.CreateParameter();
                pLastName.ParameterName = "@LastName";
                pLastName.DbType = DbType.String;
                pLastName.Size = 100;
                pLastName.Value = (object)lastName ?? DBNull.Value;
                cmd.Parameters.Add(pLastName);

                var pEmail = cmd.CreateParameter();
                pEmail.ParameterName = "@Email";
                pEmail.DbType = DbType.String;
                pEmail.Size = 256;
                pEmail.Value = (object)email ?? DBNull.Value;
                cmd.Parameters.Add(pEmail);

                var pContact = cmd.CreateParameter();
                pContact.ParameterName = "@ContactNumber";
                pContact.DbType = DbType.String;
                pContact.Size = 20;
                pContact.Value = (object)contactNumber ?? DBNull.Value;
                cmd.Parameters.Add(pContact);

                var pPhoto = cmd.CreateParameter();
                pPhoto.ParameterName = "@Photo";
                pPhoto.DbType = DbType.String;
                pPhoto.Size = 500;
                pPhoto.Value = (object)photoPath ?? DBNull.Value;
                cmd.Parameters.Add(pPhoto);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"INSERT INTO [User] 
            (Username, [Password], [Role], [Status], FirstName, LastName, Email, ContactNumber, Photo)
            VALUES (@Username, @Password, @Role, @Status, @FirstName, @LastName, @Email, @ContactNumber, @Photo);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                AddParameter(cmd, "@Username", DbType.String, user.Username, 256);
                AddParameter(cmd, "@Password", DbType.String, user.GetPasswordHash(), 256);
                AddParameter(cmd, "@Role", DbType.String, MapRoleToDbValue(user.Role), 50);  // Use mapper
                AddParameter(cmd, "@Status", DbType.String, user.Status.ToString(), 50);
                AddParameter(cmd, "@FirstName", DbType.String, user.FirstName, 100);
                AddParameter(cmd, "@LastName", DbType.String, user.LastName, 100);
                AddParameter(cmd, "@Email", DbType.String, user.Email, 256);
                AddParameter(cmd, "@ContactNumber", DbType.String, user.ContactNumber, 20);
                AddParameter(cmd, "@Photo", DbType.String, user.PhotoPath, 500);

                return (int)cmd.ExecuteScalar();
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

        public List<User> GetAllStaffUsers()
        {
            var users = new List<User>();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Get Admin and Staff users only (not Members)
                cmd.CommandText = @"SELECT UserID, Username, [Role], [Status], FirstName, LastName, Email, ContactNumber, Photo, LastLogin 
                                    FROM [User] 
                                    WHERE [Role] IN ('Admin', 'Staff')
                                    ORDER BY UserID DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int userId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? null : reader.GetString(1);
                        string role = reader.IsDBNull(2) ? null : reader.GetString(2);
                        string status = reader.IsDBNull(3) ? null : reader.GetString(3);
                        string firstName = reader.IsDBNull(4) ? null : reader.GetString(4);
                        string lastName = reader.IsDBNull(5) ? null : reader.GetString(5);
                        string email = reader.IsDBNull(6) ? null : reader.GetString(6);
                        string contactNumber = reader.IsDBNull(7) ? null : reader.GetString(7);
                        string photo = reader.IsDBNull(8) ? null : reader.GetString(8);
                        DateTime? lastLogin = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9);

                        User user = CreateUserFromRoleString(role);
                        if (user == null)
                            continue;

                        user.UserID = userId;
                        user.Username = username;
                        user.FirstName = firstName;
                        user.LastName = lastName;
                        user.Email = email;
                        user.ContactNumber = contactNumber;
                        user.PhotoPath = photo;
                        user.LastLogin = lastLogin;

                        if (!string.IsNullOrEmpty(status) &&
                            Enum.TryParse<UserStatus>(status, true, out var parsedStatus))
                        {
                            user.Status = parsedStatus;
                        }
                        else
                        {
                            user.Status = UserStatus.Inactive;
                        }

                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public bool UpdateUser(int userId, string firstName, string lastName, string email, string contactNumber, string photoPath, Role role, UserStatus status)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"UPDATE [User] 
                            SET FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                ContactNumber = @ContactNumber, 
                                Photo = @Photo,
                                [Role] = @Role,
                                [Status] = @Status
                            WHERE UserID = @UserID";

                AddParameter(cmd, "@UserID", DbType.Int32, userId, 0);
                AddParameter(cmd, "@FirstName", DbType.String, firstName, 100);
                AddParameter(cmd, "@LastName", DbType.String, lastName, 100);
                AddParameter(cmd, "@Email", DbType.String, email, 256);
                AddParameter(cmd, "@ContactNumber", DbType.String, contactNumber, 20);
                AddParameter(cmd, "@Photo", DbType.String, photoPath, 500);
                AddParameter(cmd, "@Role", DbType.String, MapRoleToDbValue(role), 50);
                AddParameter(cmd, "@Status", DbType.String, status.ToString(), 50);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool ArchiveUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "UPDATE [User] SET [Status] = @Status WHERE UserID = @UserID";

                AddParameter(cmd, "@UserID", DbType.Int32, userId, 0);
                AddParameter(cmd, "@Status", DbType.String, UserStatus.Inactive.ToString(), 50);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdateLastLogin(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("userId must be greater than 0", nameof(userId));

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "UPDATE [User] SET LastLogin = @LastLogin WHERE UserID = @UserID";

                AddParameter(cmd, "@UserID", DbType.Int32, userId, 0);
                AddParameter(cmd, "@LastLogin", DbType.DateTime, DateTime.Now, 0);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        // Helper to reduce repetition
        private void AddParameter(System.Data.IDbCommand cmd, string name, DbType type, object value, int size)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.DbType = type;
            p.Size = size;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        // ---------------------------
        // Private helper: centralized mapping of DB role string → concrete User subclass
        private User CreateUserFromRoleString(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return null;

            switch (role.Trim())
            {
                case "Admin":
                case "Librarian":
                    return new Librarian();
                case "Staff":
                    return new LibraryStaff();
                case "Member":
                    return new Member();
                default:
                    //return null;
                    throw new DataException($"Unknown role '{role}'.");
            }
        }

        // Add this helper method to map enum to DB value
        private string MapRoleToDbValue(Role role)
        {
            switch (role)
            {
                case Role.Librarian:
                    return "Admin";  // DB expects "Admin" for librarians
                case Role.Staff:
                    return "Staff";
                case Role.Member:
                    return "Member";
                default:
                    throw new ArgumentException($"Unknown role: {role}");
            }
        }

        // end code
    }
}
