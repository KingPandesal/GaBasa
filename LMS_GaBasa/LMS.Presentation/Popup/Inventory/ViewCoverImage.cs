using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class ViewCoverImage : Form
    {
        public ViewCoverImage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convenience constructor — load image and title immediately.
        /// The image is loaded into a copy (Image.FromStream -> new Bitmap) so the original file is not locked.
        /// </summary>
        /// <param name="imagePath">Full path to the cover image file (can be null/empty).</param>
        /// <param name="title">Title text to display centered at bottom.</param>
        public ViewCoverImage(string imagePath, string title) : this()
        {
            LoadCover(imagePath, title);
        }

        /// <summary>
        /// Loads the cover image and title text into the form controls.
        /// Safe: does not keep the source file locked.
        /// </summary>
        public void LoadCover(string imagePath, string title)
        {
            // Set title (null-safe)
            LblTitle.Text = string.IsNullOrWhiteSpace(title) ? string.Empty : title.Trim();

            // Clear any previous image (dispose to free memory)
            if (PicBxCoverImage.Image != null)
            {
                try { PicBxCoverImage.Image.Dispose(); } catch { }
                PicBxCoverImage.Image = null;
            }

            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            try
            {
                // Load without locking the file by copying the stream into a Bitmap
                using (var fs = File.OpenRead(imagePath))
                using (var img = Image.FromStream(fs))
                {
                    PicBxCoverImage.Image = new Bitmap(img);
                }
            }
            catch
            {
                // If loading fails, leave image null (optionally you can set a placeholder)
                PicBxCoverImage.Image = null;
            }
        }
    }
}
