using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace LMS.DataAccess.Database
{
    public class DbConnection
    {

        private readonly string connectionString;
        // ga-use ako ng Windows Auth, tingkyu -ken:>
        public DbConnection()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["LibraryDB"]
                .ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Temporary test method
        // wla mn pud nuon gigamit kay nagbuhat na lang kog button sa login form -ken:>
        public void TestConnection()
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection failed: " + ex.Message);
                }
            }
        }

        // end code
    }
}
