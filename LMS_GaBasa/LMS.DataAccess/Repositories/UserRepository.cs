using LMS.DataAccess.Database;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
using System;
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

        // end code
    }
}
