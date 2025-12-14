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

        private void Login_Load(object sender, EventArgs e)
        {
            // aron dli focused ang TxtUsername sa sugod
            this.ActiveControl = lblH1;
            lblH1.Focus();
        }

        public Login(IUserManager userManager)
        {
            InitializeComponent();
            _userManager = userManager;

            // for placeholders
            SetPlaceholder(TxtUsername, "Enter your username");
            SetPlaceholder(TxtPassword, "Enter your password", isPassword: true);

        }

        private void SetPlaceholder(TextBox textBox, string placeholder, bool isPassword = false)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;

                    if (isPassword)
                        textBox.UseSystemPasswordChar = true;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;

                    if (isPassword)
                        textBox.UseSystemPasswordChar = false;
                }
            };
        }

        // ========== EVENT HANDLERS UI ==========

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Text;
            string selectedRole = GetSelectedRole();

            if (string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            var user = _userManager.Authenticate(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid credentials or inactive account.");
                return;
            }

            // 🔒 Role validation
            if (!user.Role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Selected role does not match your account role.");
                return;
            }

            // ✅ Role-based redirect
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

        private void ChkbxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // If placeholder is showing, do nothing
            if (TxtPassword.ForeColor == Color.Gray)
                return;

            TxtPassword.UseSystemPasswordChar = !ChkbxShowPassword.Checked;
        }

        // ========== NOT RELATED TO UI ==========
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

        private string GetSelectedRole()
        {
            switch (CmbbxSelectUserType.SelectedItem?.ToString())
            {
                case "Librarian / Admin":
                    return "Admin";
                case "Library Staff":
                    return "Staff";
                case "Library Member":
                    return "Member";
                default:
                    return null;
            }
        }

        // end code
    }
}
