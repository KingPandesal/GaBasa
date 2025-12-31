using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs;

namespace LMS.Presentation.UserControls.Profile
{
    public partial class UCLibrarianStaff : UserControl
    {
        private readonly IUserProfileService _userProfileService;

        public UCLibrarianStaff()
        {
            InitializeComponent();
            _userProfileService = new UserProfileService(new UserRepository());
        }

        public void LoadUserProfile(int userId)
        {
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
            TxtContact.Text = profile.ContactNumber;

            // Load profile photo if exists
            if (!string.IsNullOrEmpty(profile.PhotoPath) && File.Exists(profile.PhotoPath))
            {
                PicBxProfilePic.Image = Image.FromFile(profile.PhotoPath);
            }
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
                    PicBxProfilePic.Image = Image.FromFile(ofd.FileName);
                    PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }

        }
    }
}
