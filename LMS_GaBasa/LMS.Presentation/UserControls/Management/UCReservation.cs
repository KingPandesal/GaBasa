using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.Model.DTOs.Reservation;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCReservation : UserControl
    {
        private readonly IReservationManager _reservationManager;

        // In-memory lists and paging state
        private List<DTOReservationView> _allReservations = new List<DTOReservationView>();
        private List<DTOReservationView> _filteredReservations = new List<DTOReservationView>();
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        public UCReservation()
        {
            InitializeComponent();

            _reservationManager = new ReservationManager();

            // Wire events
            this.Load += UCReservation_Load;
            BtnApply.Click += BtnApply_Click;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;
            // Added: support Enter-key to run search immediately
            TxtSearchBar.KeyDown += TxtSearchBar_KeyDown;
            CmbBxPaginationNumbers.SelectedIndexChanged += CmbBxPaginationNumbers_SelectedIndexChanged;
            LblPaginationPrevious.Click += LblPaginationPrevious_Click;
            LblPaginationNext.Click += LblPaginationNext_Click;

            // Ensure DataGridView is read-only and selection mode is full row
            DgvReservation.ReadOnly = true;
            DgvReservation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvReservation.MultiSelect = false;

            // Do not show the current cell focus rectangle on load
            DgvReservation.CurrentCell = null;
        }

        private void UCReservation_Load(object sender, EventArgs e)
        {
            try
            {
                SetupStatusFilter();
                SetupPaginationDefaults();

                // Expire any overdue reservations
                try
                {
                    int expired = _reservationManager.ExpireOverdueReservations();
                    if (expired > 0)
                        Debug.WriteLine($"Expired {expired} reservations.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ExpireOverdueReservations failed: " + ex.Message);
                }

                LoadReservations();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UCReservation_Load failed: " + ex.Message);
            }
        }

        private void SetupStatusFilter()
        {
            try
            {
                CmbBxStatusFilter.Items.Clear();

                // Use DropDownList so the value is always one of the provided options
                CmbBxStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;

                CmbBxStatusFilter.Items.Add("All Status");
                CmbBxStatusFilter.Items.Add("Active");
                CmbBxStatusFilter.Items.Add("Completed");
                CmbBxStatusFilter.Items.Add("Cancelled");
                //CmbBxStatusFilter.Items.Add("Expired");
                CmbBxStatusFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SetupStatusFilter failed: " + ex.Message);
            }
        }

        private void SetupPaginationDefaults()
        {
            try
            {
                if (CmbBxPaginationNumbers.Items.Count == 0)
                {
                    CmbBxPaginationNumbers.Items.AddRange(new object[] { "10", "20", "30" });
                }

                if (!int.TryParse(CmbBxPaginationNumbers.Text, out _pageSize))
                    _pageSize = 10;
            }
            catch { _pageSize = 10; }
        }

        private void LoadReservations()
        {
            try
            {
                var list = _reservation_manager_getall(); // helper to avoid long line in ctor contexts
                _allReservations = list;
                _currentPage = 1;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadReservations failed: " + ex.Message);
                MessageBox.Show("Failed to load reservations. See logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // small wrapper to call manager and avoid inline null conditional clutter
        private List<DTOReservationView> _reservation_manager_getall()
        {
            return _reservationManager.GetAllReservationsForDisplay() ?? new List<DTOReservationView>();
        }

        private void ApplyFilters()
        {
            var query = TxtSearchBar.Text?.Trim() ?? string.Empty;
            var selectedStatus = (CmbBxStatusFilter.SelectedItem?.ToString() ?? CmbBxStatusFilter.Text ?? string.Empty).Trim();

            IEnumerable<DTOReservationView> q = _allReservations.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                q = q.Where(r =>
                    (!string.IsNullOrWhiteSpace(r.MemberName) && r.MemberName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(r.BookTitle) && r.BookTitle.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (!string.IsNullOrWhiteSpace(r.AccessionNumber) && r.AccessionNumber.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            if (!string.IsNullOrWhiteSpace(selectedStatus) && !selectedStatus.Equals("All Status", StringComparison.OrdinalIgnoreCase))
            {
                q = q.Where(r => string.Equals(r.Status ?? string.Empty, selectedStatus, StringComparison.OrdinalIgnoreCase));
            }

            _filteredReservations = q.ToList();
            CalculatePagination();
            DisplayCurrentPage();
        }

        private void CalculatePagination()
        {
            if (_pageSize <= 0) _pageSize = 10;
            int total = _filteredReservations?.Count ?? 0;
            _totalPages = (int)Math.Ceiling((double)total / _pageSize);
            if (_totalPages < 1) _totalPages = 1;
            if (_currentPage < 1) _currentPage = 1;
            if (_currentPage > _totalPages) _currentPage = _totalPages;
            UpdatePaginationButtons();
        }

        private void DisplayCurrentPage()
        {
            var pageItems = (_filteredReservations ?? new List<DTOReservationView>())
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            PopulateGrid(pageItems);
            UpdatePaginationLabel();
            UpdatePaginationButtons();

            // Ensure no row is selected/highlighted after display
            ClearGridSelection();
        }

        private void PopulateGrid(List<DTOReservationView> items)
        {
            try
            {
                DgvReservation.Rows.Clear();

                int startIndex = (_currentPage - 1) * _pageSize;
                int rowNum = startIndex + 1;

                foreach (var r in items)
                {
                    int idx = DgvReservation.Rows.Add();
                    var row = DgvReservation.Rows[idx];

                    if (DgvReservation.Columns.Contains("ColumnNumbering"))
                        row.Cells["ColumnNumbering"].Value = rowNum.ToString();

                    if (DgvReservation.Columns.Contains("ColumnMembername"))
                        row.Cells["ColumnMembername"].Value = string.IsNullOrWhiteSpace(r.MemberName) ? "N/A" : r.MemberName;

                    if (DgvReservation.Columns.Contains("ColumnBookTitle"))
                        row.Cells["ColumnBookTitle"].Value = string.IsNullOrWhiteSpace(r.BookTitle) ? "N/A" : r.BookTitle;

                    if (DgvReservation.Columns.Contains("ColumnAccessionNumber"))
                        row.Cells["ColumnAccessionNumber"].Value = string.IsNullOrWhiteSpace(r.AccessionNumber) ? "N/A" : r.AccessionNumber;

                    if (DgvReservation.Columns.Contains("ColumnReservationDate"))
                        row.Cells["ColumnReservationDate"].Value = r.ReservationDate != DateTime.MinValue
                            ? r.ReservationDate.ToString("MMMM dd, yyyy")
                            : string.Empty;

                    if (DgvReservation.Columns.Contains("ColumnExpirationDate"))
                        row.Cells["ColumnExpirationDate"].Value = r.ExpirationDate != DateTime.MinValue
                            ? r.ExpirationDate.ToString("MMMM dd, yyyy")
                            : string.Empty;

                    if (DgvReservation.Columns.Contains("ColumnStatus"))
                    {
                        // Do not apply color styling — show plain text only
                        row.Cells["ColumnStatus"].Value = string.IsNullOrWhiteSpace(r.Status) ? "N/A" : r.Status;
                        // ensure default appearance
                        row.Cells["ColumnStatus"].Style.ForeColor = Color.Black;
                    }

                    row.Tag = r;
                    rowNum++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateGrid failed: " + ex.Message);
            }
        }

        // Clear selection and current cell to remove blue highlighted first row
        private void ClearGridSelection()
        {
            try
            {
                DgvReservation.ClearSelection();
                // avoid focus rectangle by clearing CurrentCell if possible
                if (DgvReservation.CurrentCell != null)
                {
                    DgvReservation.CurrentCell = null;
                }
            }
            catch
            {
                // swallow — non-critical
            }
        }

        private void UpdatePaginationLabel()
        {
            int total = _filteredReservations?.Count ?? 0;
            int start = total == 0 ? 0 : ((_currentPage - 1) * _pageSize) + 1;
            int end = Math.Min(_currentPage * _pageSize, total);
            LblPaginationShowEntries.Text = $"Showing {start} to {end} of {total} entries";
        }

        private void UpdatePaginationButtons()
        {
            LblPaginationPrevious.Enabled = _currentPage > 1;
            LblPaginationNext.Enabled = _currentPage < _totalPages;
        }

        // Event handlers
        private void BtnApply_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            ApplyFilters();
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            // keep live filter while typing but Enter will also trigger search
            _currentPage = 1;
            ApplyFilters();
        }

        // Added: support pressing Enter in the search box to trigger search
        private void TxtSearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _currentPage = 1;
                ApplyFilters();
            }
        }

        private void CmbBxPaginationNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string txt = CmbBxPaginationNumbers.SelectedItem?.ToString() ?? CmbBxPaginationNumbers.Text;
            if (int.TryParse(txt, out int ps) && ps > 0)
            {
                _pageSize = ps;
                _currentPage = 1;
                CalculatePagination();
                DisplayCurrentPage();
            }
        }

        private void LblPaginationPrevious_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                DisplayCurrentPage();
            }
        }

        private void LblPaginationNext_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                DisplayCurrentPage();
            }
        }

        // end code
    }
}
