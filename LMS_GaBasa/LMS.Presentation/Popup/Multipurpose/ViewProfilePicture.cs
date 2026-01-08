using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Multipurpose
{
    public partial class ViewProfilePicture : Form
    {
        public ViewProfilePicture()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the profile picture into the dialog. Accepts absolute or relative paths.
        /// Disposes previous image to avoid file locks.
        /// </summary>
        public void LoadProfilePicture(string imagePath)
        {
            if (PicBxProfilePicture.Image != null)
            {
                PicBxProfilePicture.Image.Dispose();
                PicBxProfilePicture.Image = null;
            }

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                MessageBox.Show("Profile picture not found.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    PicBxProfilePicture.Image = Image.FromStream(fs);
                }

                PicBxProfilePicture.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PicBxProfilePicture_Click(object sender, EventArgs e)
        {

        }
    }
}
