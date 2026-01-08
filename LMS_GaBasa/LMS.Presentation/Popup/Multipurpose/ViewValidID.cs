using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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
        /// Loads a valid ID image into the picture box. Accepts absolute path or relative.
        /// Disposes previous image safely to avoid file locks.
        /// </summary>
        /// <param name="imagePath">Path to the image file.</param>
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
