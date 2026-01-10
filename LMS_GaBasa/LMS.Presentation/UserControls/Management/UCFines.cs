using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers.Circulation;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCFines : UserControl
    {
        private readonly ICirculationManager _circulationManager;
        private DTOCirculationMemberInfo _currentMember;
        private List<DTOFineRecord> _currentFines;

        public UCFines()
        {
            InitializeComponent();

            // Setup dependencies
            var circulationRepo = new CirculationRepository();
            _circulationManager = new CirculationManager(circulationRepo);

            // Wire up events
            TxtSearchMember.KeyDown += TxtSearchMember_KeyDown;
            BtnSearchMember.Click += BtnSearchMember_Click;

            // Wire up grid checkbox events
            DgvFines.CellValueChanged += DgvFines_CellValueChanged;
            DgvFines.CurrentCellDirtyStateChanged += DgvFines_CurrentCellDirtyStateChanged;

            // Initialize UI state
            ClearMemberInfo();
            ClearFinesGrid();
        }

        private void TxtSearchMember_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchMember();
            }
        }

        private void BtnSearchMember_Click(object sender, EventArgs e)
        {
            SearchMember();
        }

        private void SearchMember()
        {
            string input = TxtSearchMember.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a Member ID.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtSearchMember.Focus();
                return;
            }

            var memberInfo = _circulationManager.GetMemberByFormattedId(input);

            if (memberInfo == null)
            {
                MessageBox.Show($"Member not found: {input}", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearMemberInfo();
                ClearFinesGrid();
                TxtSearchMember.Focus();
                TxtSearchMember.SelectAll();
                return;
            }

            _currentMember = memberInfo;
            DisplayMemberInfo(memberInfo);
            LoadFinesGrid(memberInfo.MemberID);
        }

        private void DisplayMemberInfo(DTOCirculationMemberInfo memberInfo)
        {
            // 1. LblName - member's full name
            LblName.Text = $"Name: {memberInfo.FullName}";

            // 2. LblType - member type
            LblType.Text = $"Type: {memberInfo.MemberType}";

            // 3. LblStatus - Member's current status (Active, Inactive, Suspended, Expired)
            LblStatus.Text = $"Status: {memberInfo.Status}";
            SetStatusColor(memberInfo.Status);

            // 4. LblTotalFines - total fines from Fine table
            LblTotalFines.Text = $"Total Fines: ₱{memberInfo.TotalUnpaidFines:N2}";

            // 5. LblBooksCurrentlyBorrowed - currently borrowed / limit from MemberType
            LblBooksCurrentlyBorrowed.Text = $"Books Currently Borrowed: {memberInfo.CurrentBorrowedCount} / {memberInfo.MaxBooksAllowed}";

            // 6. LblOverdueCount - how many overdues they have
            LblOverdueCount.Text = $"Overdue Count: {memberInfo.OverdueCount}";
        }

        private void SetStatusColor(string status)
        {
            switch (status?.ToLowerInvariant())
            {
                case "active":
                    LblStatus.ForeColor = Color.FromArgb(0, 200, 0);
                    break;
                case "inactive":
                    LblStatus.ForeColor = Color.FromArgb(200, 200, 0);
                    break;
                case "suspended":
                case "expired":
                    LblStatus.ForeColor = Color.FromArgb(200, 0, 0);
                    break;
                default:
                    LblStatus.ForeColor = Color.Black;
                    break;
            }
        }

        private void ClearMemberInfo()
        {
            _currentMember = null;

            LblName.Text = "Name:";
            LblType.Text = "Type:";
            LblStatus.Text = "Status:";
            LblStatus.ForeColor = Color.Black;
            LblTotalFines.Text = "Total Fines:";
            LblBooksCurrentlyBorrowed.Text = "Books Currently Borrowed:";
            LblOverdueCount.Text = "Overdue Count:";
        }

        #region Fines Grid

        private void LoadFinesGrid(int memberId)
        {
            DgvFines.Rows.Clear();
            _currentFines = _circulationManager.GetFinesByMemberId(memberId);

            if (_currentFines == null || _currentFines.Count == 0)
            {
                LblSelectedTotal.Text = "Selected Total: ₱0.00";
                return;
            }

            int rowNum = 1;
            foreach (var fine in _currentFines)
            {
                int rowIndex = DgvFines.Rows.Add();
                var row = DgvFines.Rows[rowIndex];

                row.Cells["ColumnNumbering"].Value = rowNum.ToString();
                row.Cells["ColumnCheckBox"].Value = false;
                row.Cells["ColumnTransactionID"].Value = fine.TransactionID > 0 ? fine.TransactionID.ToString() : "N/A";
                row.Cells["ColumnMemberID"].Value = $"MEM-{fine.MemberID:D4}";
                row.Cells["ColumnMemberName"].Value = fine.MemberName;
                row.Cells["ColumnFineAmount"].Value = $"₱{fine.FineAmount:N2}";
                row.Cells["ColumnFineType"].Value = fine.FineType;
                row.Cells["ColumnDateIssued"].Value = fine.DateIssued.ToString("MMM dd, yyyy");
                row.Cells["ColumnStatus"].Value = fine.Status;

                // Store the FineID and FineAmount in Tag for easy access
                row.Tag = fine;

                rowNum++;
            }

            // Reset selected total
            LblSelectedTotal.Text = "Selected Total: ₱0.00";
        }

        private void ClearFinesGrid()
        {
            DgvFines.Rows.Clear();
            _currentFines = null;
            LblSelectedTotal.Text = "Selected Total: ₱0.00";
        }

        // Commit checkbox changes immediately
        private void DgvFines_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DgvFines.IsCurrentCellDirty && DgvFines.CurrentCell is DataGridViewCheckBoxCell)
            {
                DgvFines.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // Recalculate selected total when checkbox changes
        private void DgvFines_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (DgvFines.Columns[e.ColumnIndex].Name == "ColumnCheckBox")
            {
                CalculateSelectedTotal();
            }
        }

        private void CalculateSelectedTotal()
        {
            decimal total = 0m;

            foreach (DataGridViewRow row in DgvFines.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var checkCell = row.Cells["ColumnCheckBox"];
                bool isChecked = checkCell.Value != null && (bool)checkCell.Value;

                if (isChecked)
                {
                    var fine = row.Tag as DTOFineRecord;
                    if (fine != null)
                    {
                        total += fine.FineAmount;
                    }
                }
            }

            LblSelectedTotal.Text = $"Selected Total: ₱{total:N2}";
        }

        /// <summary>
        /// Gets the list of selected fine records (where checkbox is checked).
        /// </summary>
        private List<DTOFineRecord> GetSelectedFines()
        {
            var selected = new List<DTOFineRecord>();

            foreach (DataGridViewRow row in DgvFines.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var checkCell = row.Cells["ColumnCheckBox"];
                bool isChecked = checkCell.Value != null && (bool)checkCell.Value;

                if (isChecked)
                {
                    var fine = row.Tag as DTOFineRecord;
                    if (fine != null)
                    {
                        selected.Add(fine);
                    }
                }
            }

            return selected;
        }

        #endregion

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void LblMemberID_Click(object sender, EventArgs e)
        {
        }

        private void BtnEnterMemberID_Click(object sender, EventArgs e)
        {
        }
    }
}
