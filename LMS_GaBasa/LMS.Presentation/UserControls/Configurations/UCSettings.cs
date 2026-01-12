using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.Presentation.Popup.Settings;

namespace LMS.Presentation.UserControls.Configurations
{
    public partial class UCSettings : UserControl
    {
        public UCSettings()
        {
            InitializeComponent();

            // Ensure BtnOpenHelp uses a dedicated handler instead of the designer's shared one.
            // Designer originally wires BtnOpenHelp.Click to `button3_Click` — replace that subscription.
            try
            {
                BtnOpenHelp.Click -= button3_Click;
            }
            catch
            {
                // ignore if not previously wired
            }

            BtnOpenHelp.Click -= BtnOpenHelp_Click;
            BtnOpenHelp.Click += BtnOpenHelp_Click;

            // Wire the Terms & Services button to open the ViewTermsAndServices form.
            try
            {
                BtnViewTermsAndServices.Click -= BtnViewTermsAndServices_Click;
            }
            catch
            {
                // ignore if not previously wired
            }
            BtnViewTermsAndServices.Click += BtnViewTermsAndServices_Click;
        }

        // Designer in this project wires BtnCheckForUpdates.Click to 'button3_Click'.
        // Event handlers must be 'async void' (not Task) for WinForms events.
        private async void button3_Click(object sender, EventArgs e)
        {
            // Store original button state
            string originalText = BtnCheckForUpdates.Text;
            bool originalEnabled = BtnCheckForUpdates.Enabled;

            try
            {
                // Show loading state
                BtnCheckForUpdates.Text = "Checking...";
                BtnCheckForUpdates.Enabled = false;

                // Perform the async update check
                bool updateAvailable = await CheckForUpdatesAsync();

                if (!updateAvailable)
                {
                    MessageBox.Show(
                        "You are using the latest version. No updates available.",
                        "Check for Updates",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "A new update is available!",
                        "Check for Updates",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to check for updates: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // Restore button state
                BtnCheckForUpdates.Text = originalText;
                BtnCheckForUpdates.Enabled = originalEnabled;
            }
        }

        // Keep this wrapper in case other code or the designer uses BtnCheckForUpdates_Click name.
        // It simply forwards to the actual handler above.
        private void BtnCheckForUpdates_Click(object sender, EventArgs e)
        {
            // Forward to the async handler expected by the designer wiring.
            button3_Click(sender, e);
        }

        private async Task<bool> CheckForUpdatesAsync()
        {
            // Simulate network delay for checking updates
            await Task.Delay(1500);

            // TODO: Replace with actual update check logic (call an API, compare assembly version, etc.)
            return false; // No update available by default
        }

        // New: dedicated handler for the Help button that shows a professional message with the GitHub link.
        private void BtnOpenHelp_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(
                    "For assistance, documentation, and further support resources, please visit our project page:\n\nhttps://github.com/SixxCodes/GaBasa",
                    "Help & Support",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                // Swallow any unexpected UI exceptions to avoid crashing the settings control.
            }
        }

        // New: open the Terms & Services dialog when the button is pressed.
        private void BtnViewTermsAndServices_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new ViewTermsAndServices())
                {
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to open Terms and Services: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // end code
    }
}
