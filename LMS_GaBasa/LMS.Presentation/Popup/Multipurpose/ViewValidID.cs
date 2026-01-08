using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Multipurpose
{
    public partial class ViewValidID : Form
    {
        public ViewValidID()
        {
            InitializeComponent();
        }

        private void PnlforPicBxBarcodeImageContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LblBarcode_Click(object sender, EventArgs e)
        {

        }

        private void PnlDesignOnly_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Load a valid ID image into the dialog. Accepts absolute or relative paths.
        /// Disposes previous image to avoid file locks.
        /// </summary>
        public void LoadValidID(string imagePath)
        {
            if (PicBxValidID.Image != null)
            {
                PicBxValidID.Image.Dispose();
                PicBxValidID.Image = null;
            }

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                MessageBox.Show("Valid ID image not found.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    PicBxValidID.Image = Image.FromStream(fs);
                }
                PicBxValidID.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
