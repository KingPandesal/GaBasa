using System;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Catalog
{
    public partial class ViewBookDownloadLink : Form
    {
        public ViewBookDownloadLink()
        {
            InitializeComponent();

            // Wire copy button handler in case designer did not
            try { BtnCopyDownloadURL.Click -= BtnCopyDownloadURL_Click; } catch { }
            BtnCopyDownloadURL.Click += BtnCopyDownloadURL_Click;
        }

        // New ctor: allow caller to pass the URL directly
        public ViewBookDownloadLink(string url) : this()
        {
            try
            {
                TxtDownloadURL.Text = url ?? string.Empty;
                // Select text so user can quickly copy/edit
                TxtDownloadURL.SelectAll();
                TxtDownloadURL.Focus();
            }
            catch { }
        }

        private void BtnCopyDownloadURL_Click(object sender, EventArgs e)
        {
            try
            {
                var txt = TxtDownloadURL.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(txt))
                {
                    MessageBox.Show("No download link available to copy.", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Clipboard.SetText(txt);
                MessageBox.Show("Download link copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to copy link: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // no-op (designer hook)
        }

        private void BtnCopyDownloadURL_Click_1(object sender, EventArgs e)
        {

        }
    }
}
