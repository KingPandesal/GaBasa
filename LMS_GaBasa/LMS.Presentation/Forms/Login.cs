using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // delete or comment ni after testing
        // para lang this test if naka-konek sa datavis
        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            string connString = @"Server=localhost\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Database Connected!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed: " + ex.Message);
                }
            }
        }
    }
}
