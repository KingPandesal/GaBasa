using System;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class AddBookCopy : Form
    {
        // Exposed so caller (ViewBookCopy) can build the staged BookCopy
        public string SelectedStatus { get; private set; }
        public string SelectedLocation { get; private set; }

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
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // NOTE: This no longer writes to DB. It only collects status/location and returns OK.
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var status = CmbBxStatus.Text?.Trim();
                if (string.IsNullOrWhiteSpace(status))
                {
                    MessageBox.Show("Please select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SelectedStatus = status;
                SelectedLocation = TxtLocation.Text?.Trim();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
