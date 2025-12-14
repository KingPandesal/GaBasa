using LMS.BusinessLogic.Managers;
using LMS.Presentation.Forms.Librarian;
using LMS.Presentation.Forms.LibraryMember;
using LMS.Presentation.Forms.LibraryStaff;
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
        private readonly IUserManager _userManager;

        public Login(IUserManager userManager)
        {
            InitializeComponent();
            _userManager = userManager;
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

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = TxtUsername.Text;
            string password = TxtPassword.Text;

            var user = _userManager.Authenticate(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid credentials or inactive account.");
                return;
            }

            // Redirect based on role
            switch (user.Role)
            {
                case "Admin":
                    new DashboardLibrarian(user).Show();
                    break;
                case "Staff":
                    new DashboardStaff(user).Show();
                    break;
                case "Member":
                    new DashboardMember(user).Show();
                    break;
            }
            this.Hide();
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
