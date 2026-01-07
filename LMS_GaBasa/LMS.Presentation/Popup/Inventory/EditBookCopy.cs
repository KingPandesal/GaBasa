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

        // This updates the provided BookCopy instance in-memory (does not persist).
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_copy == null)
                {
                    MessageBox.Show("No copy loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Apply edits to the single in-memory copy.
                _copy.Status = CmbBxStatus.Text?.Trim();
                _copy.Location = TxtLocation.Text?.Trim();

                // Return OK so the caller can persist changes if desired.
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
