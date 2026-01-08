using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;
using LMS.Model.DTOs.User;

namespace LMS.Presentation.Popup.Profile
{
    public partial class EditLibrarianStaffProfile : Form
    {
        private readonly IUserProfileService _userProfileService;
        private int _userId;
        private string _photoPath;

        // Event to notify when profile is updated
        public event Action<DTOUpdateUserProfile> ProfileUpdated;

        public EditLibrarianStaffProfile(IUserProfileService userProfileService)
        {
            InitializeComponent();
            _userProfileService = userProfileService;

            // Wire up form behaviors for login controls if they exist
            SetupForm();
        }

        private void SetupForm()
        {
            // Attach show/hide handlers only if the controls exist on the designer
            try
            {
                if (ChkBxShowOldPassword != null && TxtEntOldPass != null)
                {
                    ChkBxShowOldPassword.CheckedChanged += (s, e) =>
                        TxtEntOldPass.PasswordChar = ChkBxShowOldPassword.Checked ? '\0' : '•';
                }

                if (ChkBxShowNewPassword != null && TxtEntNewPass != null)
                {
                    ChkBxShowNewPassword.CheckedChanged += (s, e) =>
                        TxtEntNewPass.PasswordChar = ChkBxShowNewPassword.Checked ? '\0' : '•';
                }

                if (ChkBxShowConfirmNewPassword != null && TxtConfirmNewPass != null)
                {
                    ChkBxShowConfirmNewPassword.CheckedChanged += (s, e) =>
                        TxtConfirmNewPass.PasswordChar = ChkBxShowConfirmNewPassword.Checked ? '\0' : '•';
                }
            }
            catch
            {
                // If controls are not present for some builds, ignore wiring errors.
            }
        }

        public void LoadProfile(DTOUserProfile profile)
        {
            if (profile == null) return;

            _userId = profile.UserID;
            _photoPath = profile.PhotoPath;

            // Split FullName into First and Last (assuming "FirstName LastName" format)
            string[] nameParts = profile.FullName?.Split(new[] { ' ' }, 2) ?? new string[0];
            TxtFirstName.Text = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            TxtLastName.Text = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            TxtEmail.Text = profile.Email;
            TxtContactNumber.Text = profile.ContactNumber;

            // Populate username if control exists
            try
            {
                if (TxtUsername != null)
                    TxtUsername.Text = profile.Username ?? string.Empty;
            }
            catch { }

            if (!string.IsNullOrEmpty(profile.PhotoPath) && System.IO.File.Exists(profile.PhotoPath))
            {
                PicBxProfilePic.Image = Image.FromFile(profile.PhotoPath);
                PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select an image";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                PicBxProfilePic.Cursor = Cursors.Hand;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _photoPath = ofd.FileName;
                    PicBxProfilePic.Image = Image.FromFile(ofd.FileName);
                    PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // ===== PERSONAL FIELDS VALIDATION =====
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) || string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== USERNAME VALIDATION =====
            if (TxtUsername == null || string.IsNullOrWhiteSpace(TxtUsername.Text))
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

            // ===== OPTIONAL PASSWORD CHANGE VALIDATION =====
            bool isChangingPassword = (TxtEntNewPass != null && !string.IsNullOrEmpty(TxtEntNewPass.Text))
                                      || (TxtConfirmNewPass != null && !string.IsNullOrEmpty(TxtConfirmNewPass.Text));

            if (isChangingPassword)
            {
                // Old password required
                if (TxtEntOldPass == null || string.IsNullOrEmpty(TxtEntOldPass.Text))
                {
                    MessageBox.Show("Please enter your old password to change password.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // New and confirm must match
                if (TxtEntNewPass == null || TxtConfirmNewPass == null || TxtEntNewPass.Text != TxtConfirmNewPass.Text)
                {
                    MessageBox.Show("New passwords do not match.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate new password complexity
                var pwValidation = ValidatePassword(TxtEntNewPass.Text);
                if (!pwValidation.IsValid)
                {
                    MessageBox.Show(pwValidation.ErrorMessage, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // ===== BUILD DTO and CALL SERVICE =====
            var updateProfile = new DTOUpdateUserProfile
            {
                UserID = _userId,
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text?.Trim(),
                ContactNumber = TxtContactNumber.Text?.Trim(),
                PhotoPath = _photoPath,
                Username = TxtUsername.Text.Trim(),
                // Pass old/new passwords only if changing — service should verify old password
                OldPassword = isChangingPassword ? TxtEntOldPass.Text : null,
                NewPassword = isChangingPassword ? TxtEntNewPass.Text : null
            };

            try
            {
                bool success = _userProfileService.UpdateUserProfile(updateProfile);

                if (success)
                {
                    MessageBox.Show("Profile updated successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ProfileUpdated?.Invoke(updateProfile);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update profile. Please check the provided data.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // If the service throws an error (for example: old password mismatch), show it
                MessageBox.Show($"Failed to update profile: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== VALIDATION HELPERS =====

        private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                return (false, "Password must contain at least 1 uppercase letter.");

            if (!Regex.IsMatch(password, "[0-9]"))
                return (false, "Password must contain at least 1 number.");

            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                return (false, "Password must contain at least 1 symbol (e.g., !@#$%^&*).");

            return (true, null);
        }
    }
}
