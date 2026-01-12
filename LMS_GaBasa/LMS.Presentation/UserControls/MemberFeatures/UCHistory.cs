using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Services.MemberFeatures;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.MemberFeatures.History;

namespace LMS.Presentation.UserControls.MemberFeatures
{
    public partial class UCHistory : UserControl
    {
        private readonly MemberHistoryService _historyService;
        private readonly ReservationRepository _reservationRepository;

        private int _memberId;
        private List<DTOHistoryItem> _allHistory;
        private List<DTOHistoryItem> _filteredHistory;

        // pagination
        private int _pageSize = 10;
        private int _currentPage = 1;

        public UCHistory()
        {
            InitializeComponent();

            _historyService = new MemberHistoryService();
            _reservationRepository = new ReservationRepository();

            // wire events
            this.Load += UCHistory_Load;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;
            CmbBxPagination.SelectedIndexChanged += CmbBxPagination_SelectedIndexChanged;
            LblPaginationPrevious.Click += LblPaginationPrevious_Click;
            LblPaginationNext.Click += LblPaginationNext_Click;

            DgvHistory.ReadOnly = true;
            DgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvHistory.AllowUserToAddRows = false;
        }

        private void UCHistory_Load(object sender, EventArgs e)
        {
            SetupPaginationDefaults();
            InitializeMemberId();
            LoadHistory();
        }

        private void SetupPaginationDefaults()
        {
            if (CmbBxPagination.Items.Count == 0)
            {
                CmbBxPagination.Items.AddRange(new object[] { "10", "20", "30" });
            }

            if (!int.TryParse(CmbBxPagination.Text, out _pageSize))
                _pageSize = 10;

            CmbBxPagination.SelectedIndex = 0;
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

        private void LoadHistory()
        {
            if (_memberId <= 0)
            {
                ShowNoHistory();
                return;
            }

            _allHistory = _historyService.GetMemberHistory(_memberId) ?? new List<DTOHistoryItem>();
            _filteredHistory = _allHistory;

            _currentPage = 1;
            UpdatePaginationLabel();
            PopulateGridPage();
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            string q = TxtSearchBar.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(q))
            {
                _filteredHistory = _allHistory;
            }
            else
            {
                _filteredHistory = _allHistory.Where(h =>
                    !string.IsNullOrWhiteSpace(h.Title) &&
                    h.Title.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
            }

            _currentPage = 1;
            UpdatePaginationLabel();
            PopulateGridPage();
        }

        private void PopulateGridPage()
        {
            DgvHistory.Rows.Clear();

            if (_filteredHistory == null || _filteredHistory.Count == 0)
            {
                ShowNoHistory();
                return;
            }

            int total = _filteredHistory.Count;
            int start = (_currentPage - 1) * _pageSize;
            var pageItems = _filteredHistory.Skip(start).Take(_pageSize).ToList();

            int idx = start + 1;
            foreach (var item in pageItems)
            {
                int r = DgvHistory.Rows.Add();
                var row = DgvHistory.Rows[r];

                row.Cells["ColumnNumbering"].Value = idx.ToString();
                row.Cells["ColumnTitle"].Value = item.Title;
                row.Cells["ColumnDateofTransaction"].Value = item.TransactionDate.ToString("MMM dd, yyyy hh:mm tt");
                row.Cells["ColumnStatus"].Value = item.Status;
                if (!string.IsNullOrWhiteSpace(item.Details))
                    row.Cells["ColumnTitle"].ToolTipText = item.Details;

                ColorStatusCell(row.Cells["ColumnStatus"], item.Status);

                row.Tag = item;
                idx++;
            }

            UpdatePaginationLabel();
        }

        private void ShowNoHistory()
        {
            DgvHistory.Rows.Clear();
            int r = DgvHistory.Rows.Add();
            var row = DgvHistory.Rows[r];
            row.Cells["ColumnNumbering"].Value = "";
            row.Cells["ColumnTitle"].Value = "No history found.";
            row.Cells["ColumnDateofTransaction"].Value = "";
            row.Cells["ColumnStatus"].Value = "";
            LblPaginationShowEntries.Text = "Showing 0 entries";
        }

        private void ColorStatusCell(DataGridViewCell cell, string status)
        {
            if (cell == null) return;
            if (string.IsNullOrWhiteSpace(status)) { cell.Style.ForeColor = Color.Black; return; }

            var s = status.ToLowerInvariant();
            if (s.Contains("borrow")) cell.Style.ForeColor = Color.FromArgb(0, 100, 200);
            else if (s.Contains("return")) cell.Style.ForeColor = Color.FromArgb(0, 150, 0);
            else if (s.Contains("reserved") && !s.Contains("cancel")) cell.Style.ForeColor = Color.FromArgb(255, 165, 0);
            else if (s.Contains("cancel") || s.Contains("expired")) cell.Style.ForeColor = Color.FromArgb(192, 0, 0);
            else if (s.Contains("fine")) cell.Style.ForeColor = Color.FromArgb(0, 150, 0);
            else cell.Style.ForeColor = Color.Black;
        }

        private void UpdatePaginationLabel()
        {
            if (_filteredHistory == null || _filteredHistory.Count == 0)
            {
                LblPaginationShowEntries.Text = "Showing 0 entries";
                return;
            }

            int total = _filteredHistory.Count;
            int start = ((_currentPage - 1) * _pageSize) + 1;
            int end = Math.Min(_currentPage * _pageSize, total);

            LblPaginationShowEntries.Text = $"Showing {start} to {end} of {total} entries";
        }

        private void CmbBxPagination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(CmbBxPagination.Text, out int newSize) && newSize > 0)
            {
                _pageSize = newSize;
                _currentPage = 1;
                PopulateGridPage();
            }
        }

        private void LblPaginationPrevious_Click(object sender, EventArgs e)
        {
            if (_currentPage <= 1) return;
            _currentPage--;
            PopulateGridPage();
        }

        private void LblPaginationNext_Click(object sender, EventArgs e)
        {
            if (_filteredHistory == null) return;
            int maxPage = Math.Max(1, (int)Math.Ceiling(_filteredHistory.Count / (double)_pageSize));
            if (_currentPage >= maxPage) return;
            _currentPage++;
            PopulateGridPage();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // designer leftover - no action
        }
    }
}
