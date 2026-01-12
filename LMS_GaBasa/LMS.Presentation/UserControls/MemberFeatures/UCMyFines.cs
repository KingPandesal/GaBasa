using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Services.MemberFeatures;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Fine;

namespace LMS.Presentation.UserControls.MemberFeatures
{
    public partial class UCMyFines : UserControl
    {
        private readonly MemberFinesService _finesService;
        private readonly ReservationRepository _reservationRepository;

        private int _memberId;
        private DTOMemberFinesView _viewModel;
        private List<DTOFineRecord> _filteredFines;

        public UCMyFines()
        {
            InitializeComponent();

            _finesService = new MemberFinesService();
            _reservationRepository = new ReservationRepository();

            // Wire events
            this.Load += UCMyFines_Load;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;

            // Ensure grid read-only
            DgvListOfFines.ReadOnly = true;
            DgvListOfFines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void UCMyFines_Load(object sender, EventArgs e)
        {
            InitializeMemberId();
            LoadViewModelAndPopulate();
        }

        private void InitializeMemberId()
        {
            try
            {
                var mainForm = this.FindForm();
                if (mainForm == null) { _memberId = 0; return; }

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
                _memberId = 0;
            }
        }

        private void LoadViewModelAndPopulate()
        {
            if (_memberId <= 0)
            {
                DisplayDefaultState();
                return;
            }

            _viewModel = _finesService.GetMemberFinesView(_memberId) ?? new DTOMemberFinesView();
            _filteredFines = _viewModel.Fines ?? new List<DTOFineRecord>();

            // Labels
            LblOverduePerDay.Text = _viewModel.OverduePerDayText;
            LblValueOustandingBalance.Text = $"₱{_viewModel.TotalOutstanding:N2}";

            // Account standing
            LblValueAccountStanding.Text = _viewModel.AccountStanding ?? "Unknown";
            SetAccountStandingColor(LblValueAccountStanding, _viewModel.AccountStanding);

            PopulateGrid(_filteredFines);
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            if (_viewModel == null)
                return;

            string q = TxtSearchBar.Text?.Trim() ?? "";
            _filteredFines = _finesService.FilterFines(_viewModel.Fines, q);
            PopulateGrid(_filteredFines);
        }

        private void PopulateGrid(List<DTOFineRecord> fines)
        {
            DgvListOfFines.Rows.Clear();

            if (fines == null || fines.Count == 0)
            {
                // show empty row
                var idxEmpty = DgvListOfFines.Rows.Add();
                DgvListOfFines.Rows[idxEmpty].Cells["ColumnTitleDescription"].Value = "No fines found.";
                return;
            }

            int row = 1;
            foreach (var f in fines)
            {
                int r = DgvListOfFines.Rows.Add();
                var dgvRow = DgvListOfFines.Rows[r];

                dgvRow.Cells["ColumnNumbering"].Value = row.ToString();
                dgvRow.Cells["ColumnDateIncurred"].Value = f.DateIssued == DateTime.MinValue ? "" : f.DateIssued.ToString("MMM dd, yyyy");
                dgvRow.Cells["ColumnTitleDescription"].Value = GetFriendlyFineType(f.FineType);
                
                // Days Late - attempt to estimate for Overdue fines
                int daysLate = _finesService.EstimateDaysLate(f, _viewModel.MemberInfo);
                dgvRow.Cells["ColumnDaysLate"].Value = daysLate >= 0 ? daysLate.ToString() : "N/A";

                // Amount - designer ColumnAmount already set to C2; supply decimal
                dgvRow.Cells["ColumnAmount"].Value = f.FineAmount;

                dgvRow.Cells["ColumnStatus"].Value = f.Status;

                SetStatusCellColor(dgvRow.Cells["ColumnStatus"], f.Status);

                dgvRow.Tag = f;
                row++;
            }
        }

        private string GetFriendlyFineType(string fineType)
        {
            if (string.IsNullOrWhiteSpace(fineType))
                return "Unknown";

            switch (fineType.Trim().ToLowerInvariant())
            {
                case "overdue":
                    return "Overdue";
                case "lost":
                    return "Lost";
                case "damaged":
                case "damage":
                    return "Damaged";
                case "cardreplacement":
                    return "ID Card Replacement";
                default:
                    return fineType;
            }
        }

        private void SetStatusCellColor(DataGridViewCell cell, string status)
        {
            if (cell == null || string.IsNullOrWhiteSpace(status)) return;

            switch (status.Trim().ToLowerInvariant())
            {
                case "unpaid":
                    cell.Style.ForeColor = Color.FromArgb(192, 0, 0);
                    cell.Style.Font = new Font(DgvListOfFines.Font, FontStyle.Bold);
                    break;
                case "paid":
                    cell.Style.ForeColor = Color.FromArgb(0, 150, 0);
                    break;
                case "waived":
                    cell.Style.ForeColor = Color.FromArgb(0, 100, 200);
                    break;
                default:
                    cell.Style.ForeColor = Color.Black;
                    break;
            }
        }

        private void SetAccountStandingColor(Label lbl, string standing)
        {
            if (lbl == null) return;
            if (string.IsNullOrWhiteSpace(standing)) { lbl.ForeColor = Color.Black; return; }

            switch (standing.Trim().ToLowerInvariant())
            {
                case "good":
                    lbl.ForeColor = Color.FromArgb(0, 150, 0);
                    break;
                case "restricted":
                    lbl.ForeColor = Color.FromArgb(255, 165, 0);
                    break;
                case "suspended":
                    lbl.ForeColor = Color.FromArgb(192, 0, 0);
                    break;
                default:
                    lbl.ForeColor = Color.Black;
                    break;
            }
        }

        private void DisplayDefaultState()
        {
            LblOverduePerDay.Text = "• Overdue: ₱0.00 per day";
            LblValueOustandingBalance.Text = "₱0.00";
            LblValueAccountStanding.Text = "Good";
            LblValueAccountStanding.ForeColor = Color.FromArgb(0, 150, 0);
            DgvListOfFines.Rows.Clear();
        }
    }
}
