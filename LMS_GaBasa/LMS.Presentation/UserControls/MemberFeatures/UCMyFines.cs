using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;

namespace LMS.Presentation.UserControls.MemberFeatures
{
    public partial class UCMyFines : UserControl
    {
        private readonly ICirculationManager _circulationManager;
        private readonly IReservationRepository _reservationRepository;
        private int _memberId;
        private DTOCirculationMemberInfo _memberInfo;
        private List<DTOFineRecord> _allFines;
        private List<DTOFineRecord> _filteredFines;

        public UCMyFines()
        {
            InitializeComponent();

            // Setup dependencies
            var circulationRepo = new CirculationRepository();
            _circulationManager = new CirculationManager(circulationRepo);
            _reservationRepository = new ReservationRepository();

            // Wire up events
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;
        }

        private void UCMyFines_Load(object sender, EventArgs e)
        {
            InitializeMemberId();
            LoadMemberInfo();
            LoadFines();
        }

        private void InitializeMemberId()
        {
            try
            {
                // Get MainForm instance
                var mainForm = this.FindForm();
                if (mainForm == null)
                {
                    _memberId = 0;
                    return;
                }

                // Use reflection to get _currentUser from MainForm
                var currentUserField = mainForm.GetType().GetField("_currentUser",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (currentUserField != null)
                {
                    var currentUser = currentUserField.GetValue(mainForm);
                    if (currentUser != null)
                    {
                        var userIdProp = currentUser.GetType().GetProperty("UserID");
                        if (userIdProp != null)
                        {
                            int userId = (int)userIdProp.GetValue(currentUser);
                            _memberId = _reservationRepository.GetMemberIdByUserId(userId);
                        }
                    }
                }
            }
            catch
            {
                // Fallback: member ID not found
                _memberId = 0;
            }
        }

        private void LoadMemberInfo()
        {
            if (_memberId <= 0)
            {
                DisplayDefaultState();
                return;
            }

            try
            {
                _memberInfo = _circulationManager.GetMemberByFormattedId($"MEM-{_memberId:D4}");
                if (_memberInfo == null)
                {
                    DisplayDefaultState();
                    return;
                }

                // Update LblOverduePerDay with member's fine rate
                LblOverduePerDay.Text = $"• Overdue: ₱{_memberInfo.FineRate:N2} per day";

                // Update Outstanding Balance
                LblValueOustandingBalance.Text = $"₱{_memberInfo.TotalUnpaidFines:N2}";

                // Determine and display Account Standing
                UpdateAccountStanding();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading member info: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateAccountStanding()
        {
            if (_memberInfo == null)
            {
                LblValueAccountStanding.Text = "Unknown";
                LblValueAccountStanding.ForeColor = Color.Black;
                return;
            }

            bool hasFines = _memberInfo.TotalUnpaidFines > 0;
            bool hasOverdue = _memberInfo.OverdueCount > 0;
            bool reachedMaxFineCap = _memberInfo.TotalUnpaidFines >= _memberInfo.MaxFineCap;

            // Account Standing Logic:
            // Good = No fines, no overdue
            // Restricted = with fine or with overdue
            // Suspended = reaches MaxFineCap
            if (reachedMaxFineCap)
            {
                LblValueAccountStanding.Text = "Suspended";
                LblValueAccountStanding.ForeColor = Color.FromArgb(192, 0, 0); // Dark Red
            }
            else if (hasFines || hasOverdue)
            {
                LblValueAccountStanding.Text = "Restricted";
                LblValueAccountStanding.ForeColor = Color.FromArgb(255, 165, 0); // Orange
            }
            else
            {
                LblValueAccountStanding.Text = "Good";
                LblValueAccountStanding.ForeColor = Color.FromArgb(0, 150, 0); // Green
            }
        }

        private void LoadFines()
        {
            if (_memberId <= 0)
            {
                DisplayNoFinesMessage();
                return;
            }

            try
            {
                _allFines = _circulationManager.GetFinesByMemberId(_memberId) ?? new List<DTOFineRecord>();

                if (_allFines.Count == 0)
                {
                    DisplayNoFinesMessage();
                    return;
                }

                _filteredFines = _allFines;
                ApplySearchFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading fines: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            if (_allFines == null || _allFines.Count == 0)
            {
                DisplayNoFinesMessage();
                return;
            }

            string searchText = TxtSearchBar.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredFines = _allFines;
            }
            else
            {
                // Search by Status (ColumnStatus) or Fine Type (ColumnTitleDescription)
                _filteredFines = _allFines.Where(f =>
                    (f.Status != null && f.Status.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (f.FineType != null && f.FineType.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            DgvListOfFines.Rows.Clear();

            if (_filteredFines == null || _filteredFines.Count == 0)
            {
                DisplayNoFinesMessage();
                return;
            }

            int rowNum = 1;
            foreach (var fine in _filteredFines)
            {
                int rowIndex = DgvListOfFines.Rows.Add();
                var row = DgvListOfFines.Rows[rowIndex];

                // Column mappings:
                // ColumnNumbering = #
                // ColumnDateIncurred = Date Incurred
                // ColumnTitleDescription = Title / Description (Fine Type)
                // ColumnDaysLate = Days Late (N/A for non-overdue fines)
                // ColumnAmount = Amount
                // ColumnStatus = Status

                row.Cells["ColumnNumbering"].Value = rowNum.ToString();
                row.Cells["ColumnDateIncurred"].Value = fine.DateIssued == DateTime.MinValue 
                    ? "" 
                    : fine.DateIssued.ToString("MMM dd, yyyy");
                row.Cells["ColumnTitleDescription"].Value = GetFineTypeDescription(fine.FineType);
                row.Cells["ColumnDaysLate"].Value = CalculateDaysLate(fine);
                row.Cells["ColumnAmount"].Value = fine.FineAmount;
                row.Cells["ColumnStatus"].Value = fine.Status;

                // Color code status
                SetStatusCellColor(row.Cells["ColumnStatus"], fine.Status);

                row.Tag = fine;
                rowNum++;
            }
        }

        private string GetFineTypeDescription(string fineType)
        {
            if (string.IsNullOrWhiteSpace(fineType))
                return "Unknown";

            // Map fine types to friendly descriptions
            switch (fineType.ToLowerInvariant())
            {
                case "overdue":
                    return "Overdue Fine";
                case "lost":
                    return "Lost Book";
                case "damaged":
                    return "Damaged Book";
                case "cardreplacement":
                    return "ID Card Replacement";
                default:
                    return fineType;
            }
        }

        private string CalculateDaysLate(DTOFineRecord fine)
        {
            // Only calculate days late for overdue fines
            if (fine.FineType == null || 
                !fine.FineType.Equals("Overdue", StringComparison.OrdinalIgnoreCase))
            {
                return "N/A";
            }

            // Calculate days late based on fine amount and fine rate
            if (_memberInfo != null && _memberInfo.FineRate > 0)
            {
                int daysLate = (int)Math.Ceiling(fine.FineAmount / _memberInfo.FineRate);
                return daysLate.ToString();
            }

            return "N/A";
        }

        private void SetStatusCellColor(DataGridViewCell cell, string status)
        {
            if (cell == null || string.IsNullOrWhiteSpace(status))
                return;

            switch (status.ToLowerInvariant())
            {
                case "unpaid":
                    cell.Style.ForeColor = Color.FromArgb(192, 0, 0); // Dark Red
                    cell.Style.Font = new Font(DgvListOfFines.Font, FontStyle.Bold);
                    break;
                case "paid":
                    cell.Style.ForeColor = Color.FromArgb(0, 150, 0); // Green
                    break;
                case "waived":
                    cell.Style.ForeColor = Color.FromArgb(0, 100, 200); // Blue
                    break;
                default:
                    cell.Style.ForeColor = Color.Black;
                    break;
            }
        }

        private void DisplayNoFinesMessage()
        {
            DgvListOfFines.Rows.Clear();
            // Optionally add a message row
            int rowIndex = DgvListOfFines.Rows.Add();
            var row = DgvListOfFines.Rows[rowIndex];
            row.Cells["ColumnTitleDescription"].Value = "No fines found.";
        }

        private void DisplayDefaultState()
        {
            LblValueOustandingBalance.Text = "₱0.00";
            LblValueAccountStanding.Text = "Good";
            LblValueAccountStanding.ForeColor = Color.FromArgb(0, 150, 0);
            LblOverduePerDay.Text = "• Overdue: ₱0.00 per day";
            DisplayNoFinesMessage();
        }

        private void LblOverduePerDay_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void bigTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Legacy event handler - can be removed if not wired in designer
        }

        private void bigTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            // Legacy event handler - can be removed if not wired in designer
        }
    }
}
