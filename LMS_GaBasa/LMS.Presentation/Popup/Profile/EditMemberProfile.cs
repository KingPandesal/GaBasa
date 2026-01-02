using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;
using LMS.Model.DTOs.Member;

namespace LMS.Presentation.Popup.Profile
{
    public partial class EditMemberProfile : Form
    {
        private readonly IMemberProfileService _memberProfileService;
        private int _userId;
        private string _photoPath;
        private string _validIdPath;
        private string _currentUsername;

        private const string UserPhotosFolder = @"Assets\dataimages\Members";
        private const string ValidIDsFolder = @"Assets\dataimages\ValidIDs";

        // Event to notify when profile is updated
        public event Action<DTOUpdateMemberProfile> ProfileUpdated;

        public EditMemberProfile(IMemberProfileService memberProfileService)
        {
            InitializeComponent();
            _memberProfileService = memberProfileService;
            SetupForm();
        }

        private void SetupForm()
        {
            // Wire up show/hide password checkboxes
            ChkBxShowOldPassword.CheckedChanged += (s, e) =>
                TxtEntOldPass.PasswordChar = ChkBxShowOldPassword.Checked ? '\0' : '•';

            ChkBxShowNewPassword.CheckedChanged += (s, e) =>
                TxtEntNewPass.PasswordChar = ChkBxShowNewPassword.Checked ? '\0' : '•';

            ChkBxShowConfirmNewPassword.CheckedChanged += (s, e) =>
                TxtConfirmNewPass.PasswordChar = ChkBxShowConfirmNewPassword.Checked ? '\0' : '•';

            // Wire up ValidID PictureBox click
            PicBxValidID.Click += PicBxValidID_Click;
        }

        public void LoadProfile(MemberProfileDto profile)
        {
            if (profile == null) return;

            _userId = profile.UserID;
            _photoPath = profile.PhotoPath;
            _validIdPath = profile.ValidIdPath;
            _currentUsername = profile.Username;

            // Split FullName into First and Last
            string[] nameParts = profile.FullName?.Split(new[] { ' ' }, 2) ?? new string[0];
            TxtFirstName.Text = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            TxtLastName.Text = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            TxtEmail.Text = profile.Email;
            TxtContactNumber.Text = profile.ContactNumber;
            TxtAddress.Text = profile.Address;
            TxtUsername.Text = profile.Username;

            // Load profile photo
            if (!string.IsNullOrEmpty(profile.PhotoPath))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, profile.PhotoPath);
                if (File.Exists(fullPath))
                {
                    PicBxProfilePic.ImageLocation = fullPath;
                }
            }

            // Load ValidID photo
            if (!string.IsNullOrEmpty(profile.ValidIdPath))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, profile.ValidIdPath);
                if (File.Exists(fullPath))
                {
                    PicBxValidID.ImageLocation = fullPath;
                }
            }
        }

        private void PicBxProfilePic_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select profile photo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _photoPath = ofd.FileName;
                    PicBxProfilePic.ImageLocation = ofd.FileName;
                }
            }
        }

        private void PicBxValidID_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Valid ID";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _validIdPath = ofd.FileName;
                    PicBxValidID.ImageLocation = ofd.FileName;
                }
            }
        }

        private void BtnSave_Click_1(object sender, EventArgs e)
        {
            // ===== REQUIRED FIELD VALIDATION =====
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                MessageBox.Show("First name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("Last name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== USERNAME VALIDATION =====
            if (string.IsNullOrWhiteSpace(TxtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TxtUsername.Text.Trim().Length < 8)
            {
                MessageBox.Show("Username must be at least 8 characters.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== EMAIL VALIDATION =====
            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(TxtEmail.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== CONTACT NUMBER VALIDATION =====
            if (string.IsNullOrWhiteSpace(TxtContactNumber.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var contactValidation = ValidateContactNumber(TxtContactNumber.Text.Trim());
            if (!contactValidation.IsValid)
            {
                MessageBox.Show(contactValidation.ErrorMessage, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== PASSWORD CHANGE VALIDATION =====
            bool isChangingPassword = !string.IsNullOrEmpty(TxtEntNewPass.Text) ||
                                       !string.IsNullOrEmpty(TxtConfirmNewPass.Text);

            if (isChangingPassword)
            {
                if (string.IsNullOrEmpty(TxtEntOldPass.Text))
                {
                    MessageBox.Show("Please enter your old password to change password.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (TxtEntNewPass.Text != TxtConfirmNewPass.Text)
                {
                    MessageBox.Show("New passwords do not match.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var passwordValidation = ValidatePassword(TxtEntNewPass.Text);
                if (!passwordValidation.IsValid)
                {
                    MessageBox.Show(passwordValidation.ErrorMessage, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // ===== PROCESS FILES =====
            string storedPhotoPath = _photoPath;
            string storedValidIdPath = _validIdPath;

            try
            {
                if (!string.IsNullOrEmpty(_photoPath) && !_photoPath.StartsWith(UserPhotosFolder))
                {
                    storedPhotoPath = CopyFileToAssets(_photoPath, UserPhotosFolder, $"member_{_userId}");
                }

                if (!string.IsNullOrEmpty(_validIdPath) && !_validIdPath.StartsWith(ValidIDsFolder))
                {
                    storedValidIdPath = CopyFileToAssets(_validIdPath, ValidIDsFolder, $"validid_{_userId}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ===== UPDATE PROFILE =====
            var updateProfile = new DTOUpdateMemberProfile
            {
                UserID = _userId,
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                ContactNumber = TxtContactNumber.Text.Trim(),
                PhotoPath = storedPhotoPath,
                Address = TxtAddress.Text.Trim(),
                ValidIdPath = storedValidIdPath,
                Username = TxtUsername.Text.Trim(),
                OldPassword = isChangingPassword ? TxtEntOldPass.Text : null,
                NewPassword = isChangingPassword ? TxtEntNewPass.Text : null
            };

            var result = _memberProfileService.UpdateMemberProfile(updateProfile);

            if (result.Success)
            {
                MessageBox.Show("Profile updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ProfileUpdated?.Invoke(updateProfile);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== VALIDATION METHODS =====

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private (bool IsValid, string ErrorMessage) ValidateContactNumber(string contactNumber)
        {
            string digitsOnly = new string(contactNumber.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length != contactNumber.Length)
                return (false, "Contact number must contain numbers only.");

            if (digitsOnly.Length != 11)
                return (false, "Contact number must be exactly 11 digits.");

            return (true, null);
        }

        private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (password.Length < 8)
                return (false, "Password must be at least 8 characters.");

            if (!password.Any(char.IsUpper))
                return (false, "Password must contain at least 1 uppercase letter.");

            if (!password.Any(char.IsDigit))
                return (false, "Password must contain at least 1 number.");

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return (false, "Password must contain at least 1 symbol (e.g., !@#$%^&*).");

            return (true, null);
        }

        private string CopyFileToAssets(string sourceFilePath, string destinationFolder, string filePrefix)
        {
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullDestinationFolder = Path.Combine(appBasePath, destinationFolder);

            // Create directory if it doesn't exist
            if (!Directory.Exists(fullDestinationFolder))
            {
                Directory.CreateDirectory(fullDestinationFolder);
            }

            // Generate unique filename
            string extension = Path.GetExtension(sourceFilePath);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName = $"{filePrefix}_{timestamp}{extension}";

            string destinationPath = Path.Combine(fullDestinationFolder, newFileName);

            // Copy the file
            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            // Return relative path for database storage
            return Path.Combine(destinationFolder, newFileName);
        }

        private void LblCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
