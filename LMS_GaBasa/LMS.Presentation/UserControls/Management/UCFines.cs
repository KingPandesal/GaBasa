using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;
using LMS.Presentation.Popup.Fines;

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

            CmbBxChargeType.SelectedIndexChanged += CmbBxChargeType_SelectedIndexChanged;
            BtnAddToList.Click += BtnAddToList_Click;

            // Wire up waiver button
            BtnWaive.Click += BtnWaive_Click;

            // Wire up payment controls
            NumPckAmountReceived.ValueChanged += NumPckAmountReceived_ValueChanged;
            BtnSettlePayment.Click += BtnSettlePayment_Click;

            // Ensure numeric control rules
            try
            {
                NumPckAmount.Minimum = 1;
                NumPckAmount.DecimalPlaces = 2;
                NumPckAmount.Maximum = 999999999;
                NumPckAmount.ThousandsSeparator = true;

                NumPckAmountReceived.Minimum = 1;
                NumPckAmountReceived.DecimalPlaces = 2;
                NumPckAmountReceived.Maximum = 999999999;
                NumPckAmountReceived.ThousandsSeparator = true;
            }
            catch { }

            // Wire up grid checkbox events
            DgvFines.CellValueChanged += DgvFines_CellValueChanged;
            DgvFines.CurrentCellDirtyStateChanged += DgvFines_CurrentCellDirtyStateChanged;

            // Initialize UI state
            ClearMemberInfo();
            ClearFinesGrid();
            ResetPaymentTab();

            // Initial UI: hide accession panel until Damaged Book is chosen
            PnlforAccessionNumber.Visible = false;
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

            // 4. LblTotalFines - total fines from Fine table (unpaid)
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
            // Reload fines and exclude waived/paid entries so they disappear immediately
            DgvFines.Rows.Clear();
            var allFines = _circulationManager.GetFinesByMemberId(memberId) ?? new List<DTOFineRecord>();

            // Exclude waived and paid fines from the grid (only show unpaid)
            _currentFines = allFines
                .Where(f => string.Equals(f.Status, "Unpaid", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (_currentFines == null || _currentFines.Count == 0)
            {
                LblSelectedTotal.Text = "Selected Total: ₱0.00";

                // Also refresh member totals so LblTotalFines is accurate
                RefreshMemberTotals(memberId);
                ResetPaymentTab();
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
                row.Cells["ColumnDateIssued"].Value = fine.DateIssued == DateTime.MinValue ? "" : fine.DateIssued.ToString("MMM dd, yyyy");
                row.Cells["ColumnStatus"].Value = fine.Status;

                // Store the DTO for quick access
                row.Tag = fine;

                rowNum++;
            }

            // Reset selected total
            LblSelectedTotal.Text = "Selected Total: ₱0.00";

            // Refresh member totals so LblTotalFines updates real-time
            RefreshMemberTotals(memberId);

            // Reset payment tab
            ResetPaymentTab();
        }

        private void RefreshMemberTotals(int memberId)
        {
            try
            {
                // Get up-to-date member info (TotalUnpaidFines is computed from DB)
                var memberInfo = _circulationManager.GetMemberByFormattedId($"MEM-{memberId}");
                if (memberInfo != null)
                {
                    _currentMember = memberInfo;
                    // Update only totals part to avoid overwriting other displayed fields unnecessarily
                    LblTotalFines.Text = $"Total Fines: ₱{memberInfo.TotalUnpaidFines:N2}";
                    LblBooksCurrentlyBorrowed.Text = $"Books Currently Borrowed: {memberInfo.CurrentBorrowedCount} / {memberInfo.MaxBooksAllowed}";
                    LblOverdueCount.Text = $"Overdue Count: {memberInfo.OverdueCount}";
                }
            }
            catch
            {
                // ignore refresh errors; do not block UI
            }
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
                UpdatePaymentAmountToPay();
                RecalculateChange();
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

        private decimal GetSelectedTotal()
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

            return total;
        }

        #endregion

        #region Add Charges tab handlers

        private void CmbBxChargeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = CmbBxChargeType.SelectedItem?.ToString() ?? CmbBxChargeType.Text ?? string.Empty;
            bool isDamaged = selected.IndexOf("damag", StringComparison.OrdinalIgnoreCase) >= 0;

            PnlforAccessionNumber.Visible = isDamaged;

            // Always enforce minimum value for amount
            try
            {
                if (NumPckAmount.Minimum < 1)
                    NumPckAmount.Minimum = 1;
            }
            catch { }
        }

        private void BtnAddToList_Click(object sender, EventArgs e)
        {
            if (_currentMember == null)
            {
                MessageBox.Show("Please search and select a member first.", "No Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = CmbBxChargeType.SelectedItem?.ToString() ?? CmbBxChargeType.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(selected))
            {
                MessageBox.Show("Please select a charge type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal amount = 0m;
            try { amount = NumPckAmount.Value; } catch { amount = 0m; }

            if (amount < 1m)
            {
                MessageBox.Show("Amount must be at least 1.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int memberId = _currentMember.MemberID;
            int transactionId = 0;
            string fineType;
            string status;

            if (selected.IndexOf("id card", StringComparison.OrdinalIgnoreCase) >= 0
                || selected.IndexOf("card replacement", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // ID Card Replacement
                fineType = "CardReplacement"; // DB-allowed FineType
                status = "Unpaid";
                transactionId = 0;
            }
            else if (selected.IndexOf("damag", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // Damaged Book Fee - accession required
                var accession = TxtAccessionNumber.Text?.Trim();
                if (string.IsNullOrWhiteSpace(accession))
                {
                    MessageBox.Show("Please enter the Accession Number for the damaged copy.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtAccessionNumber.Focus();
                    return;
                }

                // Validate accession exists and check LoanType
                var bookInfo = _circulationManager.GetBookByAccession(accession);
                if (bookInfo == null)
                {
                    MessageBox.Show($"Accession not found: {accession}", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtAccessionNumber.Focus();
                    TxtAccessionNumber.SelectAll();
                    return;
                }

                var loanType = (bookInfo.LoanType ?? string.Empty).Trim();

                // Only allow when LoanType == "Reference" (this UI is for in-library Reference damage charges)
                if (!string.Equals(loanType, "Reference", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Invalid Action: This book is a borrowable copy.", "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtAccessionNumber.Focus();
                    TxtAccessionNumber.SelectAll();
                    return;
                }

                // For Reference copies, we allow adding a damage charge WITHOUT requiring an active borrowing.
                transactionId = 0;
                fineType = "Damaged"; // DB-allowed FineType
                status = "Unpaid";    // record as unpaid
            }
            else
            {
                // Generic fallback
                fineType = selected;
                status = "Unpaid";
                transactionId = 0;
            }

            bool ok = false;
            try
            {
                ok = _circulationManager.AddFineCharge(memberId, transactionId, amount, fineType, DateTime.Now, status);
            }
            catch (Exception ex)
            {
                ok = false;
                MessageBox.Show($"Error adding charge: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ok)
            {
                MessageBox.Show("Charge added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh fines grid (waived are excluded by LoadFinesGrid)
                LoadFinesGrid(memberId);

                // Reset inputs
                try { NumPckAmount.Value = NumPckAmount.Minimum; } catch { }
                TxtAccessionNumber.Text = "";
            }
            else
            {
                MessageBox.Show("Failed to add charge. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Waiver tab handlers

        private void BtnWaive_Click(object sender, EventArgs e)
        {
            if (_currentMember == null)
            {
                MessageBox.Show("Please search and select a member first.", "No Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get selected fines (only unpaid ones can be waived)
            var selectedFines = GetSelectedFines();
            if (selectedFines == null || selectedFines.Count == 0)
            {
                MessageBox.Show("Please select at least one fine to waive.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Filter only unpaid fines
            var unpaidFines = selectedFines.Where(f => string.Equals(f.Status, "Unpaid", StringComparison.OrdinalIgnoreCase)).ToList();
            if (unpaidFines.Count == 0)
            {
                MessageBox.Show("Only unpaid fines can be waived. Please select unpaid fines.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get reason from TxtReason
            var reason = TxtReason.Text?.Trim();
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Please enter a reason for waiving the fine(s).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtReason.Focus();
                return;
            }

            // Confirm action
            var fineIds = unpaidFines.Select(f => f.FineID).ToList();
            decimal totalAmount = unpaidFines.Sum(f => f.FineAmount);

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to waive {unpaidFines.Count} fine(s) totaling ₱{totalAmount:N2}?\n\nReason: {reason}",
                "Confirm Waiver",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            bool ok = false;
            try
            {
                ok = _circulationManager.WaiveFines(fineIds, reason);
            }
            catch (Exception ex)
            {
                ok = false;
                MessageBox.Show($"Error waiving fines: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ok)
            {
                MessageBox.Show($"Successfully waived {unpaidFines.Count} fine(s).", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh fines grid (waived entries are excluded)
                LoadFinesGrid(_currentMember.MemberID);

                // Clear reason textbox
                TxtReason.Text = "";

                // Refresh member info to update total fines (and update LblTotalFines)
                var memberInfo = _circulationManager.GetMemberByFormattedId($"MEM-{_currentMember.MemberID}");
                if (memberInfo != null)
                {
                    _currentMember = memberInfo;
                    DisplayMemberInfo(memberInfo);
                }
            }
            else
            {
                MessageBox.Show("Failed to waive fines. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Payment tab handlers

        private void ResetPaymentTab()
        {
            LblValueAmounttoPay.Text = "₱0.00";
            LblValueChange.Text = "₱0.00";
            try { NumPckAmountReceived.Value = NumPckAmountReceived.Minimum; } catch { }
            if (CmbBxMode.Items.Count > 0 && CmbBxMode.SelectedIndex < 0)
                CmbBxMode.SelectedIndex = 0;
        }

        private void UpdatePaymentAmountToPay()
        {
            decimal amountToPay = GetSelectedTotal();
            LblValueAmounttoPay.Text = $"₱{amountToPay:N2}";
        }

        private void NumPckAmountReceived_ValueChanged(object sender, EventArgs e)
        {
            RecalculateChange();
        }

        private void RecalculateChange()
        {
            decimal amountToPay = GetSelectedTotal();
            decimal amountReceived = 0m;
            try { amountReceived = NumPckAmountReceived.Value; } catch { amountReceived = 0m; }

            decimal change = amountReceived - amountToPay;
            LblValueChange.Text = $"₱{change:N2}";

            // Change color: red if negative, green if positive/zero
            if (change < 0)
                LblValueChange.ForeColor = Color.FromArgb(200, 0, 0);
            else
                LblValueChange.ForeColor = Color.FromArgb(0, 150, 0);
        }

        private void BtnSettlePayment_Click(object sender, EventArgs e)
        {
            if (_currentMember == null)
            {
                MessageBox.Show("Please search and select a member first.", "No Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get selected fines
            var selectedFines = GetSelectedFines();
            if (selectedFines == null || selectedFines.Count == 0)
            {
                MessageBox.Show("Please select at least one fine to pay.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Filter only unpaid fines
            var unpaidFines = selectedFines.Where(f => string.Equals(f.Status, "Unpaid", StringComparison.OrdinalIgnoreCase)).ToList();
            if (unpaidFines.Count == 0)
            {
                MessageBox.Show("Only unpaid fines can be paid. Please select unpaid fines.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get payment mode
            var paymentMode = CmbBxMode.SelectedItem?.ToString() ?? CmbBxMode.Text ?? "Cash";
            if (string.IsNullOrWhiteSpace(paymentMode))
                paymentMode = "Cash";

            // Get amount received
            decimal amountReceived = 0m;
            try { amountReceived = NumPckAmountReceived.Value; } catch { amountReceived = 0m; }

            decimal amountToPay = unpaidFines.Sum(f => f.FineAmount);

            // Validate amount received >= amount to pay
            if (amountReceived < amountToPay)
            {
                MessageBox.Show($"Insufficient amount received. Amount to pay is ₱{amountToPay:N2}.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NumPckAmountReceived.Focus();
                return;
            }

            decimal change = amountReceived - amountToPay;

            // Confirm action
            var fineIds = unpaidFines.Select(f => f.FineID).ToList();

            var confirmResult = MessageBox.Show(
                $"Settle payment for {unpaidFines.Count} fine(s)?\n\n" +
                $"Amount to Pay: ₱{amountToPay:N2}\n" +
                $"Amount Received: ₱{amountReceived:N2}\n" +
                $"Change: ₱{change:N2}\n" +
                $"Payment Mode: {paymentMode}",
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            DateTime paymentDate = DateTime.Now;

            bool ok = false;
            List<int> paymentIds = null;
            try
            {
                // ProcessPayment now returns the list of Payment IDs
                paymentIds = _circulationManager.ProcessPayment(fineIds, paymentMode, paymentDate);
                ok = (paymentIds != null && paymentIds.Count > 0);
            }
            catch (Exception ex)
            {
                ok = false;
                MessageBox.Show($"Error processing payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ok && paymentIds != null && paymentIds.Count > 0)
            {
                MessageBox.Show($"Payment successful!\n\nChange: ₱{change:N2}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Show receipt - pass payment IDs so receipt can display Payment ID(s)
                try
                {
                    using (var receipt = new FinePaymentReceipt(
                        _currentMember,
                        unpaidFines,
                        amountToPay,
                        paymentMode,
                        paymentDate,
                        paymentIds))
                    {
                        receipt.ShowDialog(this.FindForm());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Payment recorded but failed to show receipt: {ex.Message}", "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Refresh fines grid (paid entries are excluded)
                LoadFinesGrid(_currentMember.MemberID);

                // Reset payment tab
                ResetPaymentTab();

                // Refresh member info to update total fines
                var memberInfo = _circulationManager.GetMemberByFormattedId($"MEM-{_currentMember.MemberID}");
                if (memberInfo != null)
                {
                    _currentMember = memberInfo;
                    DisplayMemberInfo(memberInfo);
                }
            }
            else
            {
                MessageBox.Show("Failed to process payment. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void LblMemberID_Click(object sender, EventArgs e)
        {
        }

        private void BtnEnterMemberID_Click(object sender, EventArgs e)
        {
        }
    }
}
