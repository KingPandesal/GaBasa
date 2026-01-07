using System;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class AddBookCopy : Form
    {
        // Exposed so caller (ViewBookCopy / EditBook) can build the staged BookCopy
        public string SelectedStatus { get; private set; }
        public string SelectedLocation { get; private set; }
        public int SelectedCopies { get; private set; } = 1;

        // Designer ctor kept for compatibility
        public AddBookCopy() : this(0) { }

        // We accept bookId for future use; kept to match previous ctor surface
        public AddBookCopy(int bookId)
        {
            InitializeComponent();

            // Wire events
            BtnSave.Click -= BtnSave_Click;
            BtnSave.Click += BtnSave_Click;

            LblCancel.Click -= LblCancel_Click;
            LblCancel.Click += LblCancel_Click;

            // sensible defaults
            if (CmbBxStatus.Items.Count > 0)
                CmbBxStatus.SelectedIndex = 0;

            // Ensure numeric control enforces minimum at runtime (designer also sets Min = 1)
            try
            {
                if (NumPckNoOfCopies != null)
                {
                    if (NumPckNoOfCopies.Min < 1) NumPckNoOfCopies.Min = 1;
                    if (NumPckNoOfCopies.Value < 1) NumPckNoOfCopies.Value = 1;
                }
            }
            catch
            {
                // ignore if control not present / custom control issues
            }
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // NOTE: This collects number of copies, status and location and returns OK.
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var status = CmbBxStatus.Text?.Trim();
                if (string.IsNullOrWhiteSpace(status))
                {
                    MessageBox.Show("Please select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbBxStatus.Focus();
                    return;
                }

                int copies = 1;
                try
                {
                    if (NumPckNoOfCopies != null)
                        copies = (int)NumPckNoOfCopies.Value;
                    else
                        copies = 1;
                }
                catch
                {
                    copies = 1;
                }

                // Enforce required minimum of 1 and prevent negative values
                if (copies < 1)
                {
                    MessageBox.Show("Please specify at least 1 copy.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    try { if (NumPckNoOfCopies != null) NumPckNoOfCopies.Focus(); } catch { }
                    return;
                }

                SelectedStatus = status;
                SelectedLocation = TxtLocation.Text?.Trim();
                SelectedCopies = copies;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click_1(object sender, EventArgs e)
        {
            // kept for designer compatibility
        }
    }
}
