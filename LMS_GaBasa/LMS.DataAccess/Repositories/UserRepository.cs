using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LMS.DataAccess.Database;
using LMS.Model.Models.Users;

namespace LMS.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Holds the database connection object
        private readonly DbConnection _db;

        // Constructor initializes the DbConnection
        public UserRepository()
        {
            _db = new DbConnection();
        }

        // Retrieves a User object from the database based on the provided username
        public User GetByUsername(string username)
        {
            User user = null;
            using (var conn = _db.GetConnection())
            {
                conn.Open(); // Open the database connection

                // SQL command to select user info based on username
                using (var cmd = new SqlCommand(
                    "SELECT UserID, Username, [Password], [Role], [Status], FirstName, LastName FROM [User] WHERE Username=@Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Role = reader.GetString(3),
                                Status = reader.GetString(4),
                                FirstName = reader.GetString(5),
                                LastName = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            // Return the User object if found
            // kung wa, awh null, 8080 desuka? -ken:>
            return user;
        }
    }

}
