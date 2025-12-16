using LMS.BusinessLogic.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

using LMS.DataAccess.Database;              

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCMembers : UserControl
    {
        private readonly DbConnection _db = new DbConnection();
        private readonly Sha256PasswordHasher _hasher = new Sha256PasswordHasher();

        public UCMembers()
        {
            InitializeComponent();

            // wire Add button (button2) to open registration dialog
            this.button2.Click += button2_Click;

            // Load members into grid on control initialization
            try
            {
                LoadMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load members: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var form = new RegistrationForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        RegisterUser(
                            form.Username,
                            form.Password,
                            form.FirstName,
                            form.LastName,
                            form.Role,         // database role string: "Admin"/"Staff"/"Member"
                            form.Email,
                            form.ContactNumber);
                        MessageBox.Show("Member registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // refresh grid after insertion
                        LoadMembers();
                    }
                    catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                    {
                        // Unique constraint violation (duplicate username).
                        MessageBox.Show("Username is already taken. Choose another username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to register member: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RegisterUser(string username, string password, string firstName, string lastName, string role, string email, string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required", nameof(password));
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required", nameof(lastName));

            // Hash with SHA256 (existing hasher)
            string passwordHash = _hasher.Hash(password);

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"
INSERT INTO [User] (Username, [Password], FirstName, LastName, [Role], Email, ContactNumber, [Status])
VALUES (@Username, @Password, @FirstName, @LastName, @Role, @Email, @ContactNumber, @Status);
";
                var p = cmd.CreateParameter();
                p.ParameterName = "@Username";
                p.DbType = DbType.String;
                p.Size = 50;
                p.Value = username.Trim();
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@Password";
                p.DbType = DbType.String;
                p.Size = 255;
                p.Value = passwordHash;
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@FirstName";
                p.DbType = DbType.String;
                p.Size = 50;
                p.Value = firstName.Trim();
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@LastName";
                p.DbType = DbType.String;
                p.Size = 50;
                p.Value = lastName.Trim();
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@Role";
                p.DbType = DbType.String;
                p.Size = 20;
                p.Value = string.IsNullOrWhiteSpace(role) ? "Member" : role;
                cmd.Parameters.Add(p);

                // Properly set DBNull when empty
                p = cmd.CreateParameter();
                p.ParameterName = "@Email";
                p.DbType = DbType.String;
                p.Size = 255;
                p.Value = string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email.Trim();
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@ContactNumber";
                p.DbType = DbType.String;
                p.Size = 20;
                p.Value = string.IsNullOrWhiteSpace(contactNumber) ? (object)DBNull.Value : contactNumber.Trim();
                cmd.Parameters.Add(p);

                p = cmd.CreateParameter();
                p.ParameterName = "@Status";
                p.DbType = DbType.String;
                p.Size = 20;
                p.Value = "Active";
                cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Load all users with Role = 'Member' and display them in the DataGridView.
        /// The designer grid has 8 columns (Full Name, Email, Contact Number, Address, Registration Date,
        /// Expiration Date, Membership Type, Status). We populate the fields we have from [User]
        /// and leave the rest empty.
        /// </summary>
        private void LoadMembers()
        {
            // Clear existing rows first
            dataGridView1.Rows.Clear();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
SELECT FirstName, LastName, Email, ContactNumber, [Status]
FROM [User]
WHERE [Role] = @Role
ORDER BY LastName, FirstName;
";
                var p = cmd.CreateParameter();
                p.ParameterName = "@Role";
                p.DbType = DbType.String;
                p.Size = 20;
                p.Value = "Member";
                cmd.Parameters.Add(p);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        string lastName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        string email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        string contact = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        string status = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);

                        string fullName = $"{firstName} {lastName}".Trim();

                        // The designer grid has 8 columns; fill known columns and leave others empty.
                        // Column mapping (from Designer): 
                        // 0 Full Name, 1 Email, 2 Contact Number, 3 Address, 4 Registration Date,
                        // 5 Expiration Date, 6 Membership Type, 7 Status
                        object[] row = new object[dataGridView1.Columns.Count];
                        row[0] = fullName;
                        row[1] = email;
                        row[2] = contact;
                        row[3] = string.Empty; // Address not in User table
                        row[4] = string.Empty; // Registration Date (could come from Members table)
                        row[5] = string.Empty; // Expiration Date
                        row[6] = string.Empty; // Membership Type
                        row[7] = status;

                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }

        // Lightweight registration form created at runtime so no designer changes are required.
        private class RegistrationForm : Form
        {
            public string Username => txtUsername.Text.Trim();
            public string Password => txtPassword.Text;
            public string FirstName => txtFirstName.Text.Trim();
            public string LastName => txtLastName.Text.Trim();
            public string Role => cmbRole.SelectedItem != null ? cmbRole.SelectedItem.ToString() : "Member";
            public string Email => txtEmail.Text.Trim();
            public string ContactNumber => txtContact.Text.Trim();

            private TextBox txtUsername;
            private TextBox txtPassword;
            private TextBox txtFirstName;
            private TextBox txtLastName;
            private ComboBox cmbRole;
            private TextBox txtEmail;
            private TextBox txtContact;
            private Button btnOk;
            private Button btnCancel;

            public RegistrationForm()
            {
                Text = "Register Member";
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;
                MaximizeBox = false;
                MinimizeBox = false;
                ClientSize = new Size(420, 360);

                var lblX = 15;
                var ctrlX = 130;
                var y = 20;
                var gap = 36;
                var labelWidth = 110;
                var controlWidth = 260;

                // Username
                Controls.Add(new Label { Text = "Username:", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtUsername = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24) };
                Controls.Add(txtUsername);
                y += gap;

                // Password
                Controls.Add(new Label { Text = "Password:", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtPassword = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24), UseSystemPasswordChar = true };
                Controls.Add(txtPassword);
                y += gap;

                // FirstName
                Controls.Add(new Label { Text = "First name:", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtFirstName = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24) };
                Controls.Add(txtFirstName);
                y += gap;

                // LastName
                Controls.Add(new Label { Text = "Last name:", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtLastName = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24) };
                Controls.Add(txtLastName);
                y += gap;

                // Role (default Member)
                Controls.Add(new Label { Text = "Role:", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                cmbRole = new ComboBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24), DropDownStyle = ComboBoxStyle.DropDownList };
                cmbRole.Items.AddRange(new[] { "Admin", "Staff", "Member" });
                cmbRole.SelectedItem = "Member";
                Controls.Add(cmbRole);
                y += gap;

                // Email
                Controls.Add(new Label { Text = "Email (optional):", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtEmail = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24) };
                Controls.Add(txtEmail);
                y += gap;

                // Contact
                Controls.Add(new Label { Text = "Contact No. (optional):", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtContact = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24) };
                Controls.Add(txtContact);
                y += gap + 6;

                btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(220, y), Size = new Size(80, 30) };
                btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(310, y), Size = new Size(80, 30) };

                btnOk.Click += BtnOk_Click;

                Controls.Add(btnOk);
                Controls.Add(btnCancel);

                AcceptButton = btnOk;
                CancelButton = btnCancel;
            }

            private void BtnOk_Click(object sender, EventArgs e)
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(Username))
                {
                    MessageBox.Show("Username is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    MessageBox.Show("Password is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    MessageBox.Show("First name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    MessageBox.Show("Last name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                // Role already constrained by combo box selection.
                // If all good, form will close with OK.
            }
        }
    }
}