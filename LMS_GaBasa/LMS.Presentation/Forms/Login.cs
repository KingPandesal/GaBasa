using LMS.BusinessLogic.Managers;
using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LMS.Model.Models.Enums;

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
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Text;

            if (CmbbxSelectUserType.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _userManager.Authenticate(username, password);

            if (!result.Success)
            {
                ShowLoginError(result.FailureReason);
                return;
            }

            Role selectedRole = ((KeyValuePair<string, Role>)CmbbxSelectUserType.SelectedItem).Value;

            if (result.User.Role != selectedRole)
            {
                MessageBox.Show(
                    "Selected role does not match your account.",
                    "Role Mismatch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            OpenDashboard(result.User);
            this.Hide();
        }

        private void ShowLoginError(AuthFailureReason reason)
        {
            switch (reason)
            {
                case AuthFailureReason.AccountInactive:
                    MessageBox.Show(
                        "Your account is inactive. Please contact the library staff to activate your account.",
                        "Account Inactive",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    break;

                case AuthFailureReason.MemberSuspended:
                    MessageBox.Show(
                        "Your account has been suspended. Please contact the library staff for assistance.",
                        "Account Suspended",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    break;

                case AuthFailureReason.MemberExpired:
                    MessageBox.Show(
                        "Your account has expired. Please renew your membership to continue using the system.",
                        "Account Expired",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    break;

                case AuthFailureReason.InvalidCredentials:
                default:
                    MessageBox.Show(
                        "Invalid username or password.",
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    break;
            }
        }

        private void OpenDashboard(User user)
        {
            var main = new MainForm(user);
            main.Show();
        }

        // end code
    }
}
