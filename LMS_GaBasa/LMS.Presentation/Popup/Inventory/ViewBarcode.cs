using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class ViewBarcode : Form
    {
        public ViewBarcode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads and displays a barcode image into the designer PictureBox (`PicBxBarcodeImage`).
        /// The designer Label (`LblBarcode`) will show only the accession number (if provided).
        /// - If barcodeOrImagePath is an existing file path, the image is loaded.
        /// - Otherwise the method will generate a simple visual from the barcode text and display it.
        /// </summary>
        public void LoadBarcode(string barcodeOrImagePath, string accessionNumber = null)
        {
            // Ensure designer controls exist
            if (this.PicBxBarcodeImage == null || this.LblBarcode == null)
            {
                MessageBox.Show("Barcode viewer controls are missing (designer mismatch).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Label should only show the accession number (or be empty)
            LblBarcode.Text = string.IsNullOrWhiteSpace(accessionNumber) ? string.Empty : accessionNumber.Trim();

            // Clear any existing image safely
            try
            {
                if (PicBxBarcodeImage.Image != null)
                {
                    PicBxBarcodeImage.Image.Dispose();
                    PicBxBarcodeImage.Image = null;
                }
            }
            catch
            {
                // ignore failures disposing image
            }

            // If no barcode text/path provided, nothing more to do
            if (string.IsNullOrWhiteSpace(barcodeOrImagePath))
                return;

            try
            {
                // If it's an existing file path, load the image
                if (File.Exists(barcodeOrImagePath))
                {
                    using (var img = Image.FromFile(barcodeOrImagePath))
                    {
                        PicBxBarcodeImage.Image = new Bitmap(img);
                    }
                    return;
                }

                // Otherwise treat value as barcode text and render a simple visualization.
                PicBxBarcodeImage.Image = GenerateSimpleBarcodeImage(barcodeOrImagePath, Math.Max(300, PicBxBarcodeImage.Width), Math.Max(80, PicBxBarcodeImage.Height));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to render barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Simple placeholder barcode renderer (keeps presentation-layer dependency only).
        // Replace with ZXing.Net or another library for production-quality barcodes.
        private Bitmap GenerateSimpleBarcodeImage(string text, int width, int height)
        {
            var bmp = new Bitmap(Math.Max(200, width), Math.Max(80, height));
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                // Pseudo-bar pattern derived from text hash -> repeatable visual
                var rnd = new Random(text?.GetHashCode() ?? 0);
                int x = 8;
                while (x < bmp.Width - 8)
                {
                    int w = rnd.Next(2, 8);
                    int h = bmp.Height - rnd.Next(18, 36);
                    g.FillRectangle(Brushes.Black, new Rectangle(x, 8, w, h));
                    x += w + rnd.Next(1, 4);
                }

                // Draw the barcode text underneath the bars only as part of the image
                var font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
                var textRect = new Rectangle(0, bmp.Height - 30, bmp.Width, 24);
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(text ?? string.Empty, font, Brushes.Black, textRect, sf);
                }
            }
            return bmp;
        }
    }
}
