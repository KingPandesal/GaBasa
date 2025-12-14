using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LMS.DataAccess.Database
{
    public class DbConnection
    {
        private string connectionString = @"Server=LAPTOP-40FGBKCR\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;";
        // If using SQL Authentication, use:
        // private string connectionString = @"Server=localhost\SQLEXPRESS;Database=LibraryDB;User Id=sa;Password=yourPassword;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Temporary test method
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

    }
}
