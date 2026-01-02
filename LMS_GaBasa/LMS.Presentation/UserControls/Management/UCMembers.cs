using LMS.BusinessLogic.Hashing;
using LMS.BusinessLogic.Services.AddMember;
using LMS.BusinessLogic.Services.FetchMembers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Member;
using LMS.Presentation.Popup.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCMembers : UserControl
    {
        private readonly IAddMemberService _addMemberService;
        private readonly IFetchMemberService _fetchMemberService;

        private List<DTOFetchAllMembers> _allMembers;
        private List<DTOFetchAllMembers> _filteredMembers;

        // Pagination state
        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalPages = 1;

        public UCMembers()
        {
            InitializeComponent();

            // Setup dependencies
            var memberRepo = new MemberRepository();
            var passwordHasher = new BcryptPasswordHasher(12);
            _addMemberService = new AddMemberService(memberRepo, passwordHasher);
            _fetchMemberService = new FetchMemberService(memberRepo);

            this.Load += UCMembers_Load;
        }

        private void UCMembers_Load(object sender, EventArgs e)
        {
            SetupFilters();
            SetupPagination();
            LoadMembers();
        }

        private void SetupFilters()
        {
            // Setup Member Type filter
            CmbBxMemberTypeFilter.Items.Clear();
            CmbBxMemberTypeFilter.Items.Add("All Types");
            CmbBxMemberTypeFilter.Items.Add("Staff");
            CmbBxMemberTypeFilter.Items.Add("Faculty");
            CmbBxMemberTypeFilter.Items.Add("Student");
            CmbBxMemberTypeFilter.Items.Add("Guest");
            CmbBxMemberTypeFilter.SelectedIndex = 0;

            // Setup Status filter
            CmbBxStatusFilter.Items.Clear();
            CmbBxStatusFilter.Items.Add("All Status");
            CmbBxStatusFilter.Items.Add("Active");
            CmbBxStatusFilter.Items.Add("Inactive");
            CmbBxStatusFilter.Items.Add("Suspended");
            CmbBxStatusFilter.Items.Add("Expired");
            CmbBxStatusFilter.SelectedIndex = 0;

            TxtSearchBar.Text = "";

            // Wire up events
            BtnApply.Click += BtnApply_Click;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;
        }

        private void SetupPagination()
        {
            // Set default page size
            CmbBxPaginationNumbers.SelectedIndex = 0; // "5"
            _pageSize = 5;

            // Wire up pagination events
            CmbBxPaginationNumbers.SelectedIndexChanged += CmbBxPaginationNumbers_SelectedIndexChanged;
            LblPaginationPrevious.Click += LblPaginationPrevious_Click;
            LblPaginationNext.Click += LblPaginationNext_Click;
        }

        private void LoadMembers()
        {
            try
            {
                // Fetch and cache all members
                _allMembers = _fetchMemberService.GetAllMembers();

                // Reset to first page when loading
                _currentPage = 1;

                // Apply filters and pagination
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            if (_allMembers == null)
                return;

            var filteredMembers = _allMembers.AsEnumerable();

            // Apply search filter
            string searchText = TxtSearchBar.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredMembers = filteredMembers.Where(m =>
                    m.MemberID.ToString().Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                    (m.FullName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (m.Username?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (m.Email?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (m.ContactNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (m.Address?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            // Apply member type filter
            string selectedType = CmbBxMemberTypeFilter.SelectedItem?.ToString() ?? "All Types";
            if (selectedType != "All Types")
            {
                filteredMembers = filteredMembers.Where(m =>
                    m.MemberType.Equals(selectedType, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Apply status filter
            string selectedStatus = CmbBxStatusFilter.SelectedItem?.ToString() ?? "All Status";
            if (selectedStatus != "All Status")
            {
                filteredMembers = filteredMembers.Where(m =>
                    m.Status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Store filtered results
            _filteredMembers = filteredMembers.ToList();

            // Calculate pagination
            CalculatePagination();

            // Display current page
            DisplayCurrentPage();
        }

        private void CalculatePagination()
        {
            int totalRecords = _filteredMembers?.Count ?? 0;
            _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);

            if (_totalPages == 0)
                _totalPages = 1;

            // Ensure current page is within bounds
            if (_currentPage > _totalPages)
                _currentPage = _totalPages;

            if (_currentPage < 1)
                _currentPage = 1;

            // Update pagination buttons state
            UpdatePaginationButtons();
        }

        private void DisplayCurrentPage()
        {
            if (_filteredMembers == null)
                return;

            // Get current page data
            var pagedMembers = _filteredMembers
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            // Display in grid
            DisplayMembers(pagedMembers);

            // Update pagination label
            UpdatePaginationLabel();

            // Update pagination buttons state
            UpdatePaginationButtons();
        }

        private void DisplayMembers(List<DTOFetchAllMembers> members)
        {
            DgwMembers.Rows.Clear();

            int startIndex = (_currentPage - 1) * _pageSize;
            int rowNumber = startIndex + 1;

            foreach (var member in members)
            {
                DgwMembers.Rows.Add(
                    rowNumber++,                                    // #
                    member.MemberID,                                // ID
                    member.FullName,                                // Full Name
                    member.MemberType,                              // Member Type
                    member.Username,                                // Username
                    member.Email,                                   // Email
                    member.Address,                                 // Address
                    member.ContactNumber,                           // Contact Number
                    member.MaxBooksAllowed,                         // Max Books Allowed
                    $"{member.BorrowingPeriod} days",               // Borrowing Period
                    member.RenewalLimit,                            // Renewal Limit
                    member.ReservationPrivilege ? "Yes" : "No",     // Reservation Privilege
                    $"₱{member.FineRate:F2}",                       // Fine Rate
                    member.RegistrationDate.ToString("MMM dd, yyyy"), // Registration Date
                    member.ExpirationDate.ToString("MMM dd, yyyy"),   // Expiration Date
                    member.Status                                   // Status
                );
            }
        }

        private void UpdatePaginationLabel()
        {
            int totalRecords = _filteredMembers?.Count ?? 0;
            int startRecord = totalRecords == 0 ? 0 : ((_currentPage - 1) * _pageSize) + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);

            LblPaginationShowEntries.Text = $"Showing {startRecord} to {endRecord} of {totalRecords} entries";
        }

        private void UpdatePaginationButtons()
        {
            LblPaginationPrevious.Enabled = _currentPage > 1;
            LblPaginationNext.Enabled = _currentPage < _totalPages;
        }

        private void CmbBxPaginationNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = CmbBxPaginationNumbers.SelectedItem?.ToString() ?? "5";
            if (int.TryParse(selectedValue, out int pageSize))
            {
                _pageSize = pageSize;
                _currentPage = 1; // Reset to first page
                ApplyFilters();
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

        private void BtnApply_Click(object sender, EventArgs e)
        {
            _currentPage = 1; // Reset to first page when applying filters
            ApplyFilters();
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            _currentPage = 1; // Reset to first page when searching
            ApplyFilters();
        }

        private void BtnAddMember_Click(object sender, EventArgs e)
        {
            var addMemberForm = new AddMember(_addMemberService);
            if (addMemberForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh member list after successful add
                LoadMembers();
            }
        }

        private void DgwMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int memberId = Convert.ToInt32(DgwMembers.Rows[e.RowIndex].Cells[1].Value);

            // Edit button clicked (column index 16)
            if (e.ColumnIndex == 16)
            {
                // TODO: Implement EditMember form
                MessageBox.Show($"Edit member ID: {memberId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Archive button clicked (column index 17)
            else if (e.ColumnIndex == 17)
            {
                string currentStatus = DgwMembers.Rows[e.RowIndex].Cells[15].Value?.ToString() ?? "";
                string memberName = DgwMembers.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "this member";

                if (currentStatus.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "This member is already inactive.\n\nTo reactivate, use the Edit button and change the status to Active.",
                        "Already Archived",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Are you sure you want to archive {memberName}?\n\nThis member will no longer be able to login.",
                    "Confirm Archive",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    // TODO: Implement archive member service
                    MessageBox.Show("Archive functionality coming soon.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // end code
    }
}