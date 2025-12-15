using LMS.DataAccess.Database;
using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnection _db;

        public UserRepository() : this(new DbConnection())
        {
        }

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

                    // Map DB role to domain type
                    switch (role)
                    {
                        case "Admin":
                        case "Librarian":
                            user = new Librarian();
                            break;

                        case "Staff":
                            user = new LibraryStaff();
                            break;

                        case "Member":
                            user = new Member();
                            break;

                        default:
                            return null;
                    }

                    user.UserID = userId;
                    user.Username = dbUsername;
                    if (!string.IsNullOrEmpty(passwordHash))
                        user.SetPasswordHash(passwordHash);
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.Status = status;
                }
            }

            return user;
        }

        // end code
    }
}