using LMS.DataAccess.Repositories;
using LMS.Model.Models.Catalog;
using System;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class EditBookCopy : Form
    {
        private readonly BookCopyRepository _bookCopyRepo;
        private BookCopy _copy;

        // Number of copies the user requests (defaults to 1)
        public int CopiesRequested { get; private set; } = 1;

        public EditBookCopy() : this(null) { }

        public EditBookCopy(BookCopy copy)
        {
            InitializeComponent();

            _bookCopyRepo = new BookCopyRepository();
            _copy = copy;

            // Wire events
            BtnOk.Click -= BtnSave_Click;
            BtnOk.Click += BtnSave_Click;

            LblCancel.Click -= LblCancel_Click;
            LblCancel.Click += LblCancel_Click;

            // Defaults
            if (CmbBxStatus.Items.Count > 0)
                CmbBxStatus.SelectedIndex = 0;

            if (_copy != null)
                LoadCopyToForm(_copy);
        }

        private void LoadCopyToForm(BookCopy copy)
        {
            try
            {
                if (copy == null) return;
                if (!string.IsNullOrWhiteSpace(copy.Status) && CmbBxStatus.Items.Contains(copy.Status))
                    CmbBxStatus.Text = copy.Status;
                else if (CmbBxStatus.Items.Count > 0)
                    CmbBxStatus.SelectedIndex = 0;

                TxtLocation.Text = copy.Location ?? string.Empty;

                // If editing an existing copy leave CopiesRequested at 1 (no multi-create when editing)
                CopiesRequested = 1;
                try
                {
                    // Populate numeric control with 1 by default
                    NumPckNoOfCopies.Value = 1;
                }
                catch { }
            }
            catch
            {
                // ignore
            }
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // IMPORTANT: This no longer persists to database.
        // It updates the BookCopy instance in-memory and returns OK.
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_copy == null)
                {
                    MessageBox.Show("No copy loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate NumPckNoOfCopies (ensure minimum 1)
                int requested = 1;
                try
                {
                    requested = (int)NumPckNoOfCopies.Value;
                }
                catch
                {
                    requested = 1;
                }

                if (requested < 1)
                {
                    MessageBox.Show("Please specify at least 1 copy.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    NumPckNoOfCopies.Focus();
                    return;
                }

                // Save the requested count for the caller to consume
                CopiesRequested = requested;

                // Apply edits to the in-memory object only.
                _copy.Status = CmbBxStatus.Text?.Trim();
                _copy.Location = TxtLocation.Text?.Trim();

                // Return OK so the caller (EditBook) can decide when to persist and how many copies to create.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
