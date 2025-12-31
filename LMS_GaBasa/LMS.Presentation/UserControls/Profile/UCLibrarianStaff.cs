using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Presentation.Popup.Profile;

namespace LMS.Presentation.UserControls.Profile
{
    public partial class UCLibrarianStaff : UserControl
    {
        private readonly IUserProfileService _userProfileService;
        private int _currentUserId;

        /// <summary>
        /// Raised when the user profile has been updated successfully.
        /// </summary>
        public event Action ProfileUpdated;

        public UCLibrarianStaff()
        {
            InitializeComponent();
            _userProfileService = new UserProfileService(new UserRepository());
        }

        public void LoadUserProfile(int userId)
        {
            _currentUserId = userId;
            DTOUserProfile profile = _userProfileService.GetUserProfile(userId);

            if (profile == null)
            {
                MessageBox.Show("User profile not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Populate UI controls
            LblFullname.Text = profile.FullName;
            LblIDNumber.Text = profile.UserID.ToString();
            LblEmail.Text = profile.Email;
            LblRole.Text = profile.Role;
            LblStatus.Text = profile.Status;
            LblActualContactNumber.Text = profile.ContactNumber;

            // Reposition Role and Status labels after the name
            RepositionRoleAndStatusLabels();

            // Load profile photo - PhotoPath is now absolute (converted by service)
            LoadProfileImage(profile.PhotoPath);
        }

        private void RepositionRoleAndStatusLabels()
        {
            const int spacing = 10; // Gap between labels

            // Position LblRole right after LblFullname
            int roleX = LblFullname.Right + spacing;
            LblRole.Location = new Point(roleX, LblRole.Location.Y);

            // Position LblStatus right after LblRole
            int statusX = LblRole.Right + spacing;
            LblStatus.Location = new Point(statusX, LblStatus.Location.Y);
        }

        private void LoadProfileImage(string photoPath)
        {
            // Dispose previous image to avoid file locking
            if (PicBxProfilePic.Image != null)
            {
                PicBxProfilePic.Image.Dispose();
                PicBxProfilePic.Image = null;
            }

            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
            {
                // Load image without locking the file
                using (var stream = new FileStream(photoPath, FileMode.Open, FileAccess.Read))
                {
                    PicBxProfilePic.Image = Image.FromStream(stream);
                }
                PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void BtnEditProfile_Click(object sender, EventArgs e)
        {
            DTOUserProfile currentProfile = _userProfileService.GetUserProfile(_currentUserId);

            if (currentProfile == null)
            {
                MessageBox.Show("Could not load profile for editing.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var editForm = new EditLibrarianStaffProfile(_userProfileService);
            editForm.LoadProfile(currentProfile);

            editForm.ProfileUpdated += (updatedProfile) =>
            {
                // Reload this UserControl's display
                LoadUserProfile(_currentUserId);

                // Notify MainForm to refresh the top bar
                ProfileUpdated?.Invoke();
            };

            editForm.ShowDialog();
        }
    }
}
