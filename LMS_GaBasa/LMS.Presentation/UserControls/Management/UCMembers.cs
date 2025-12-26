using LMS.BusinessLogic.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
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

            // wire Edit button (button3) to open edit dialog (UI only)
            this.button3.Click += button3_Click;

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
            using (var form = new RegistrationForm(_db))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        RegisterUserWithMember(
                            form.Username,
                            form.Password,
                            form.FirstName,
                            form.LastName,
                            form.Role,         // "Admin"/"Staff"/"Member"
                            form.Email,
                            form.ContactNumber,
                            form.MemberTypeId,
                            form.Address,
                            form.PhotoPath,
                            form.ValidIdPath);
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

        // Edit button click - show a modal identical to Add (UI only, no functionality yet)
        private void button3_Click(object sender, EventArgs e)
        {
            // Show the same form UI but with the title "Edit Member".
            // No loading/saving of data is performed yet per request.
            using (var form = new RegistrationForm(_db, "Edit Member"))
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Inserts a user row and its corresponding member row inside a DB transaction.
        /// RegistrationDate is set to today; ExpirationDate defaults to 1 year from registration (adjust as needed).
        /// </summary>
        private void RegisterUserWithMember(
            string username,
            string password,
            string firstName,
            string lastName,
            string role,
            string email,
            string contactNumber,
            int memberTypeId,
            string address,
            string photoPath,
            string validIdPath)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required", nameof(password));
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required", nameof(lastName));
            //if (memberTypeId <= 0) throw new ArgumentException("MemberType is required", nameof(memberTypeId));

            string passwordHash = _hasher.Hash(password);

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert into [User] and get the generated UserID
                        int newUserId;
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"
INSERT INTO [User] (Username, [Password], FirstName, LastName, [Role], Email, ContactNumber, [Status])
VALUES (@Username, @Password, @FirstName, @LastName, @Role, @Email, @ContactNumber, @Status);
SELECT CAST(SCOPE_IDENTITY() AS INT);
";
                            cmd.Parameters.AddWithValue("@Username", username.Trim());
                            cmd.Parameters.AddWithValue("@Password", passwordHash);
                            cmd.Parameters.AddWithValue("@FirstName", firstName.Trim());
                            cmd.Parameters.AddWithValue("@LastName", lastName.Trim());
                            cmd.Parameters.AddWithValue("@Role", string.IsNullOrWhiteSpace(role) ? "Member" : role);
                            cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email.Trim());
                            cmd.Parameters.AddWithValue("@ContactNumber", string.IsNullOrWhiteSpace(contactNumber) ? (object)DBNull.Value : contactNumber.Trim());
                            cmd.Parameters.AddWithValue("@Status", "Active");

                            object scalar = cmd.ExecuteScalar();
                            newUserId = Convert.ToInt32(scalar);
                        }

                        // Insert into Member table
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = @"
INSERT INTO Member (UserID, MemberTypeID, Address, RegistrationDate, ExpirationDate, Photo, ValidID, [Status])
VALUES (@UserID, @MemberTypeID, @Address, @RegistrationDate, @ExpirationDate, @Photo, @ValidID, @Status);
";
                            cmd.Parameters.AddWithValue("@UserID", newUserId);
                            cmd.Parameters.AddWithValue("@MemberTypeID", memberTypeId);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address.Trim());

                            DateTime regDate = DateTime.Today;
                            DateTime expDate = regDate.AddYears(1); // default 1 year membership; adjust if you want member-type based expiry

                            cmd.Parameters.AddWithValue("@RegistrationDate", regDate.Date);
                            cmd.Parameters.AddWithValue("@ExpirationDate", expDate.Date);
                            cmd.Parameters.AddWithValue("@Photo", string.IsNullOrWhiteSpace(photoPath) ? (object)DBNull.Value : photoPath.Trim());
                            cmd.Parameters.AddWithValue("@ValidID", string.IsNullOrWhiteSpace(validIdPath) ? (object)DBNull.Value : validIdPath.Trim());
                            cmd.Parameters.AddWithValue("@Status", "Active");

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { /* ignore rollback errors */ }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Load all users with Role = 'Member' and display them in the DataGridView.
        /// This also joins the Member and MemberType tables when available to populate address, registration/expiration and membership type.
        /// </summary>
        private void LoadMembers()
        {
            dataGridView1.Rows.Clear();

            using (var conn = _db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                // Join with Member (left) and MemberType (left) to get more member-specific columns when present
                cmd.CommandText = @"
SELECT u.FirstName, u.LastName, u.Email, u.ContactNumber,
       m.Address, m.RegistrationDate, m.ExpirationDate,
       mt.TypeName, u.[Status]
FROM [User] u
LEFT JOIN Member m ON m.UserID = u.UserID
LEFT JOIN MemberType mt ON mt.MemberTypeID = m.MemberTypeID
WHERE u.[Role] = @Role
ORDER BY u.LastName, u.FirstName;
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
                        string address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        string registrationDate = reader.IsDBNull(5) ? string.Empty : Convert.ToDateTime(reader.GetValue(5)).ToString("yyyy-MM-dd");
                        string expirationDate = reader.IsDBNull(6) ? string.Empty : Convert.ToDateTime(reader.GetValue(6)).ToString("yyyy-MM-dd");
                        string memberType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        string status = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);

                        string fullName = $"{firstName} {lastName}".Trim();

                        object[] row = new object[dataGridView1.Columns.Count];

                        // Designer columns:
                        // 0 Full Name, 1 Email, 2 Contact Number, 3 Address, 4 Registration Date,
                        // 5 Expiration Date, 6 Membership Type, 7 Status
                        row[0] = fullName;
                        row[1] = email;
                        row[2] = contact;
                        row[3] = address;
                        row[4] = registrationDate;
                        row[5] = expirationDate;
                        row[6] = memberType;
                        row[7] = status;

                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }

        // Lightweight registration form created at runtime so no designer changes are required.
        // It now loads MemberType options from the database (MemberTypeID + TypeName).
        private class RegistrationForm : Form
        {
            // Exposed properties to caller
            public string Username => txtUsername.Text.Trim();
            public string Password => txtPassword.Text;
            public string FirstName => txtFirstName.Text.Trim();
            public string LastName => txtLastName.Text.Trim();
            public string Role => cmbRole.SelectedItem != null ? cmbRole.SelectedItem.ToString() : "Member";
            public string Email => txtEmail.Text.Trim();
            public string ContactNumber => txtContact.Text.Trim();
            // Use MemberTypeItem and safe cast
            public int MemberTypeId => (cmbMemberType.SelectedItem as MemberTypeItem)?.Id ?? 0;
            public string Address => txtAddress.Text.Trim();
            public string PhotoPath => txtPhoto.Text.Trim();
            public string ValidIdPath => txtValidId.Text.Trim();

            // Controls
            private TextBox txtUsername;
            private TextBox txtPassword;
            private TextBox txtFirstName;
            private TextBox txtLastName;
            private ComboBox cmbRole;
            private TextBox txtEmail;
            private TextBox txtContact;
            private ComboBox cmbMemberType;
            private TextBox txtAddress;
            private TextBox txtPhoto;
            private TextBox txtValidId;
            private Button btnOk;
            private Button btnCancel;
            private Button btnBrowsePhoto;
            private Button btnBrowseValidId;

            // simple helper type to avoid KeyValuePair boxing/unboxing issues
            private class MemberTypeItem
            {
                public int Id { get; }
                public string Name { get; }
                public MemberTypeItem(int id, string name) { Id = id; Name = name ?? string.Empty; }
                public override string ToString() => Name;
            }

            // Modified constructor: optional title so we can reuse the same UI for Edit mode
            public RegistrationForm(DbConnection db, string title = "Register Member")
            {
                if (db == null) throw new ArgumentNullException(nameof(db));

                Text = title;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;
                MaximizeBox = false;
                MinimizeBox = false;
                ClientSize = new Size(520, 520);

                var lblX = 15;
                var ctrlX = 160;
                var y = 20;
                var gap = 36;
                var labelWidth = 140;
                var controlWidth = 300;

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
                y += gap;

                // Member Type (loaded from DB)
                Controls.Add(new Label { Text = "Membership Type:", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                cmbMemberType = new ComboBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24), DropDownStyle = ComboBoxStyle.DropDownList };
                Controls.Add(cmbMemberType);
                y += gap;

                // Address
                Controls.Add(new Label { Text = "Address (optional):", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtAddress = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth, 24) };
                Controls.Add(txtAddress);
                y += gap;

                // Photo path (optional)
                Controls.Add(new Label { Text = "Photo (optional):", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtPhoto = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth - 90, 24) };
                btnBrowsePhoto = new Button { Text = "Browse...", Location = new Point(ctrlX + controlWidth - 80, y - 1), Size = new Size(80, 26) };
                btnBrowsePhoto.Click += (s, e) => BrowseFile(txtPhoto);
                Controls.Add(txtPhoto);
                Controls.Add(btnBrowsePhoto);
                y += gap + 6;

                // Valid ID path (optional)
                Controls.Add(new Label { Text = "Valid ID (optional):", Location = new Point(lblX, y), Size = new Size(labelWidth, 24) });
                txtValidId = new TextBox { Location = new Point(ctrlX, y), Size = new Size(controlWidth - 90, 24) };
                btnBrowseValidId = new Button { Text = "Browse...", Location = new Point(ctrlX + controlWidth - 80, y - 1), Size = new Size(80, 26) };
                btnBrowseValidId.Click += (s, e) => BrowseFile(txtValidId);
                Controls.Add(txtValidId);
                Controls.Add(btnBrowseValidId);
                y += gap + 10;

                btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(ctrlX + 80, y), Size = new Size(80, 30) };
                btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(ctrlX + 170, y), Size = new Size(80, 30) };

                btnOk.Click += BtnOk_Click;

                Controls.Add(btnOk);
                Controls.Add(btnCancel);

                AcceptButton = btnOk;
                CancelButton = btnCancel;

                // Load MemberType options from DB
                LoadMemberTypes(db);
            }

            private void BrowseFile(TextBox target)
            {
                using (var dlg = new OpenFileDialog())
                {
                    dlg.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp|All files|*.*";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        target.Text = dlg.FileName;
                    }
                }
            }

            private void LoadMemberTypes(DbConnection db)
            {
                try
                {
                    cmbMemberType.Items.Clear();

                    using (var conn = db.GetConnection())
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT MemberTypeID, TypeName FROM MemberType ORDER BY TypeName";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                // add MemberTypeItem which overrides ToString() so ComboBox displays name
                                cmbMemberType.Items.Add(new MemberTypeItem(id, name));
                            }
                        }
                    }

                    if (cmbMemberType.Items.Count == 0)
                    {
                        // helpful placeholder so the combobox isn't empty — also informs user to create member types
                        cmbMemberType.Items.Add(new MemberTypeItem(0, "-- No member types defined --"));
                        cmbMemberType.SelectedIndex = 0;
                    }
                    else
                    {
                        // insert prompt at top so user deliberately selects
                        cmbMemberType.Items.Insert(0, new MemberTypeItem(0, "-- Select membership type --"));
                        cmbMemberType.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    // Surface the error so you can diagnose DB/connectivity/schema problems
                    MessageBox.Show("Failed to load membership types: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbMemberType.Items.Clear();
                    cmbMemberType.Items.Add(new MemberTypeItem(0, "-- Error loading types --"));
                    cmbMemberType.SelectedIndex = 0;
                }
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

                // ensure valid membership selection (Id > 0)
                if ((cmbMemberType.SelectedItem as MemberTypeItem)?.Id <= 0)
                {
                    MessageBox.Show("Please select a valid membership type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                // All good -> form will close with OK
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void UCMembers_Load(object sender, EventArgs e)
        {

        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {

        }
    }
}