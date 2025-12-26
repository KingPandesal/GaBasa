using LMS.BusinessLogic.Managers;
using LMS.Model.Models.Users;
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
            LoadRoles();
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

        // ========== ChkbxShowPassword ==========
        private void ChkbxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // If placeholder is showing, do nothing
            if (TxtPassword.ForeColor == Color.Gray)
                return;

            TxtPassword.UseSystemPasswordChar = !ChkbxShowPassword.Checked;
        }

        // para lang ni sa combobox
        // kay dli mn match ang enums ug ang options sa cmbbx
        // -ken:>
        private void LoadRoles()
        {
            CmbbxSelectUserType.Items.Clear();

            CmbbxSelectUserType.Items.Add(new KeyValuePair<string, Role>(
                "Librarian / Admin", Role.Librarian));

            CmbbxSelectUserType.Items.Add(new KeyValuePair<string, Role>(
                "Library Staff", Role.Staff));

            CmbbxSelectUserType.Items.Add(new KeyValuePair<string, Role>(
                "Library Member", Role.Member));

            CmbbxSelectUserType.DisplayMember = "Key";
            CmbbxSelectUserType.ValueMember = "Value";
            CmbbxSelectUserType.SelectedIndex = -1;
        }

        // ========== BtnLogin ==========
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Dungagi handling para sa wa na match na creds -ken:>
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Text;

            if (CmbbxSelectUserType.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            var user = _userManager.Authenticate(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            Role selectedRole =
                ((KeyValuePair<string, Role>)CmbbxSelectUserType.SelectedItem).Value;

            if (user.Role != selectedRole)
            {
                MessageBox.Show("Selected role does not match your account.");
                return;
            }

            OpenDashboard(user);
            this.Hide();
        }

        private void OpenDashboard(User user)
        {
            // Open main form (contains sidebar + content area)
            // wla na tong dashboard forms, usercontrols na sila
            var main = new MainForm(user);
            main.Show();

            // this.Close();
        }

        // ========== NOT UI ==========
        // delete or comment ni after testing
        // para lang this test if naka-konek sa datavis
        //private void BtnTestConnection_Click(object sender, EventArgs e)
        //{
        //note: wlay unod connString, butangi
        //    using (var conn = new SqlConnection(connString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            MessageBox.Show("Database Connected!");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Failed: " + ex.Message);
        //        }
        //    }
        //}

        // end code
    }
}
