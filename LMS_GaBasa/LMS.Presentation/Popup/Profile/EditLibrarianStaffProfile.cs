using System;
using System.Drawing;
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
            // Basic validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) || string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var updateProfile = new DTOUpdateUserProfile
            {
                UserID = _userId,
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                ContactNumber = TxtContactNumber.Text.Trim(),
                PhotoPath = _photoPath
            };

            bool success = _userProfileService.UpdateUserProfile(updateProfile);

            if (success)
            {
                MessageBox.Show("Profile updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Raise event to notify listeners
                ProfileUpdated?.Invoke(updateProfile);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update profile.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
