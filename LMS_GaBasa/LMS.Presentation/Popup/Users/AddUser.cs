using LMS.BusinessLogic.Hashing;
using LMS.BusinessLogic.Services.AddUser;
using LMS.BusinessLogic.Services.Audit;
using LMS.DataAccess.Database;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Enums;
using System;
using System.IO;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Users
{
    public partial class AddUser : Form
    {
        private readonly IAddUserService _userService;
        private readonly IAuditLogService _auditLogService;
        private string _selectedPhotoPath;

        // Relative path for storing user photos
        private const string UserPhotosFolder = @"Assets\dataimages\Users";

        public AddUser() : this(new AddUserService(new UserRepository(), new BcryptPasswordHasher(12)))
        {
        }

        public AddUser(IAddUserService userService)
        {
            InitializeComponent();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

            // Initialize audit log service
            var dbConn = new DbConnection();
            var auditLogRepo = new AuditLogRepository(dbConn);
            _auditLogService = new AuditLogService(auditLogRepo);

            SetupForm();
        }

        private void SetupForm()
        {
            // Bind role combo with enum values (excluding Member for staff management)
            comboBox1.Items.Clear();
            comboBox1.Items.Add(new ComboItem("Librarian / Admin", Role.Librarian));
            comboBox1.Items.Add(new ComboItem("Library Staff", Role.Staff));
            comboBox1.SelectedIndex = 0;

            // Wire up events
            BtnSave.Click += BtnSave_Click;
            PicBxProfilePic.Click += PicBxProfilePic_Click;

            // Wire up show/hide password checkboxes
            ChkBxShowPassword1.CheckedChanged += ChkBxShowPassword1_CheckedChanged;
            ChkBxShowPassword2.CheckedChanged += ChkBxShowPassword2_CheckedChanged;
        }

        private void ChkBxShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility for Password field
            textBox2.PasswordChar = ChkBxShowPassword1.Checked ? '\0' : '•';
        }

        private void ChkBxShowPassword2_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility for Confirm Password field
            textBox3.PasswordChar = ChkBxShowPassword2.Checked ? '\0' : '•';
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate passwords match
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRoleItem = comboBox1.SelectedItem as ComboItem;
            var selectedRole = selectedRoleItem?.Value ?? Role.Staff;

            // Process photo - copy to Assets folder if selected
            string storedPhotoPath = null;
            if (!string.IsNullOrEmpty(_selectedPhotoPath))
            {
                try
                {
                    storedPhotoPath = CopyPhotoToAssets(_selectedPhotoPath, textBox1.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save photo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var dto = new DTOCreateUser
            {
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                ContactNumber = TxtContactNumber.Text.Trim(),
                Username = textBox1.Text.Trim(),
                Password = textBox2.Text,
                Role = selectedRole,
                PhotoPath = storedPhotoPath  // Store the relative path (or null)
            };

            var result = _userService.CreateUser(dto);

            if (result.Success)
            {
                // Log the add user action to audit log
                try
                {
                    string roleDisplayName = selectedRoleItem?.Text ?? (selectedRole == Role.Librarian ? "Librarian / Admin" : "Library Staff");
                    _auditLogService.LogAddUser(Program.CurrentUserId, roleDisplayName);
                }
                catch
                {
                    // Non-fatal: audit logging failed, but user was created.
                }

                MessageBox.Show("User created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // If user creation failed and we copied a photo, clean it up
                if (!string.IsNullOrEmpty(storedPhotoPath))
                {
                    TryDeletePhoto(storedPhotoPath);
                }
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CopyPhotoToAssets(string sourceFilePath, string username)
        {
            // Get the application base directory
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string destinationFolder = Path.Combine(appBasePath, UserPhotosFolder);

            // Create directory if it doesn't exist
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Generate unique filename: username_timestamp.extension
            string extension = Path.GetExtension(sourceFilePath);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string safeUsername = string.IsNullOrEmpty(username) ? "user" : SanitizeFilename(username);
            string newFileName = $"{safeUsername}_{timestamp}{extension}";

            string destinationPath = Path.Combine(destinationFolder, newFileName);

            // Copy the file
            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            // Return relative path for database storage
            return Path.Combine(UserPhotosFolder, newFileName);
        }

        private string SanitizeFilename(string filename)
        {
            // Remove invalid characters from filename
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                filename = filename.Replace(c, '_');
            }
            return filename;
        }

        private void TryDeletePhoto(string relativePath)
        {
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Silently fail - orphaned photo is not critical
            }
        }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedPhotoPath = ofd.FileName;
                    PicBxProfilePic.ImageLocation = _selectedPhotoPath;
                }
            }
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Helper class for ComboBox items
        private class ComboItem
        {
            public string Text { get; }
            public Role Value { get; }

            public ComboItem(string text, Role value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}
