using LMS.BusinessLogic.Hashing;
using LMS.BusinessLogic.Services.AddMember;
using LMS.BusinessLogic.Services.FetchMembers;
using LMS.BusinessLogic.Services.RenewMember;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Member;
using LMS.Model.Models.Enums;
using LMS.Presentation.Popup.Members;
using LMS.Presentation.Popup.Multipurpose;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCMembers : UserControl
    {
        private readonly IAddMemberService _addMemberService;
        private readonly IFetchMemberService _fetchMemberService;
        private readonly IRenewMemberService _renewMemberService;
        private readonly MemberRepository _memberRepository;

        private List<DTOFetchAllMembers> _allMembers;
        private List<DTOFetchAllMembers> _filteredMembers;

        // Pagination state
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        // Column indices (match designer column order)
        private const int ColMemberId = 1;
        private const int ColFullName = 2;
        private const int ColLastLogin = 15;
        private const int ColStatus = 16;
        private const int ColProfilePicture = 17;
        private const int ColValidID = 18;
        private const int ColEdit = 19;
        private const int ColArchive = 20;
        private const int ColRenew = 21;

        public UCMembers()
        {
            InitializeComponent();

            // Setup dependencies
            var memberRepo = new MemberRepository();
            var passwordHasher = new BcryptPasswordHasher(12);
            _addMemberService = new AddMemberService(memberRepo, passwordHasher);
            _fetchMemberService = new FetchMemberService(memberRepo);
            _renewMemberService = new RenewMemberService(memberRepo);
            _memberRepository = memberRepo;

            this.Load += UCMembers_Load;
        }

        private void UCMembers_Load(object sender, EventArgs e)
        {
            SetupFilters();
            SetupPagination();
            LoadMembers();

            // Wire up DataGridView events
            DgwMembers.CellContentClick += DgwMembers_CellContentClick;
            DgwMembers.CellFormatting += DgwMembers_CellFormatting;
        }

        private void DgwMembers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // Get the status value
            string status = DgwMembers.Rows[e.RowIndex].Cells[ColStatus].Value?.ToString() ?? "";

            // Format Renew button - only visible/enabled for Expired status
            if (e.ColumnIndex == ColRenew)
            {
                var cell = DgwMembers.Rows[e.RowIndex].Cells[ColRenew];

                if (!status.Equals("Expired", StringComparison.OrdinalIgnoreCase))
                {
                    // Hide or gray out the renew button for non-expired members
                    cell.Style.BackColor = Color.LightGray;
                    cell.ToolTipText = "Only expired memberships can be renewed";
                }
                else
                {
                    cell.Style.BackColor = Color.White;
                    cell.ToolTipText = "Click to renew membership";
                }
            }
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
            CmbBxPaginationNumbers.SelectedIndex = 0;
            _pageSize = 10;

            CmbBxPaginationNumbers.SelectedIndexChanged += CmbBxPaginationNumbers_SelectedIndexChanged;
            LblPaginationPrevious.Click += LblPaginationPrevious_Click;
            LblPaginationNext.Click += LblPaginationNext_Click;
        }

        private void LoadMembers()
        {
            try
            {
                _allMembers = _fetchMemberService.GetAllMembers();
                _currentPage = 1;
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

            string selectedType = CmbBxMemberTypeFilter.SelectedItem?.ToString() ?? "All Types";
            if (selectedType != "All Types")
            {
                filteredMembers = filteredMembers.Where(m =>
                    m.MemberType.Equals(selectedType, StringComparison.OrdinalIgnoreCase)
                );
            }

            string selectedStatus = CmbBxStatusFilter.SelectedItem?.ToString() ?? "All Status";
            if (selectedStatus != "All Status")
            {
                filteredMembers = filteredMembers.Where(m =>
                    m.Status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase)
                );
            }

            _filteredMembers = filteredMembers.ToList();
            CalculatePagination();
            DisplayCurrentPage();
        }

        private void CalculatePagination()
        {
            int totalRecords = _filteredMembers?.Count ?? 0;
            _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);

            if (_totalPages == 0)
                _totalPages = 1;

            if (_currentPage > _totalPages)
                _currentPage = _totalPages;

            if (_currentPage < 1)
                _currentPage = 1;

            UpdatePaginationButtons();
        }

        private void DisplayCurrentPage()
        {
            if (_filteredMembers == null)
                return;

            var pagedMembers = _filteredMembers
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            DisplayMembers(pagedMembers);
            UpdatePaginationLabel();
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
                    rowNumber++,
                    member.FormattedID,  // Changed from member.MemberID
                    member.FullName,
                    member.MemberType,
                    member.Username,
                    member.Email,
                    member.Address,
                    member.ContactNumber,
                    member.MaxBooksAllowed,
                    $"{member.BorrowingPeriod} days",
                    member.RenewalLimit,
                    member.ReservationPrivilege ? "Yes" : "No",
                    $"₱{member.FineRate:F2}",
                    member.RegistrationDate.ToString("MMM dd, yyyy"),
                    member.ExpirationDate.ToString("MMM dd, yyyy"),
                    member.LastLogin,
                    member.Status
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
            string selectedValue = CmbBxPaginationNumbers.SelectedItem?.ToString() ?? "10";
            if (int.TryParse(selectedValue, out int pageSize))
            {
                _pageSize = pageSize;
                _currentPage = 1;
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
            _currentPage = 1;
            ApplyFilters();
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            ApplyFilters();
        }

        private void BtnAddMember_Click(object sender, EventArgs e)
        {
            var addMemberForm = new AddMember(_addMemberService);
            if (addMemberForm.ShowDialog() == DialogResult.OK)
            {
                LoadMembers();
            }
        }

        private void DgwMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // Get the row number from the first column and find the member from filtered list
            int displayIndex = (_currentPage - 1) * _pageSize + e.RowIndex;
            if (_filteredMembers == null || displayIndex >= _filteredMembers.Count)
                return;

            var member = _filteredMembers[displayIndex];
            int memberId = member.MemberID;
            string memberName = member.FullName ?? "this member";
            string currentStatus = member.Status ?? "";

            // Profile picture button clicked
            if (e.ColumnIndex == ColProfilePicture)
            {
                string resolved = ResolveMemberImagePath(member.PhotoPath, "Assets\\dataimages\\Members");
                if (string.IsNullOrEmpty(resolved) || !File.Exists(resolved))
                {
                    MessageBox.Show("Profile picture not found for this member.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var view = new ViewProfilePicture())
                {
                    view.LoadProfilePicture(resolved);
                    view.ShowDialog();
                }
                return;
            }

            // Valid ID button clicked
            if (e.ColumnIndex == ColValidID)
            {
                string resolved = ResolveMemberImagePath(member.ValidIdPath, "Assets\\dataimages\\ValidIDs");
                if (string.IsNullOrEmpty(resolved) || !File.Exists(resolved))
                {
                    MessageBox.Show("Valid ID not found for this member.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var view = new ViewValidID())
                {
                    view.LoadValidID(resolved);
                    view.ShowDialog();
                }
                return;
            }

            // Edit button clicked
            if (e.ColumnIndex == ColEdit)
            {
                var editMemberForm = new EditMember();
                editMemberForm.LoadMember(memberId);

                if (editMemberForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload member list so image / valid ID changes reflect immediately
                    LoadMembers();
                }
            }
            // Archive button clicked
            else if (e.ColumnIndex == ColArchive)
            {
                HandleArchive(memberId, memberName, currentStatus);
            }
            // Renew button clicked
            else if (e.ColumnIndex == ColRenew)
            {
                HandleRenew(memberId, memberName, currentStatus);
            }
        }

        /// <summary>
        /// Resolves a member image path. Accepts absolute path, app-relative path, or filename stored inside the specified assetsFolder.
        /// Returns null if resolution failed.
        /// </summary>
        private string ResolveMemberImagePath(string storedPath, string assetsFolder)
        {
            if (string.IsNullOrEmpty(storedPath))
                return null;

            try
            {
                // If path already points to an existing file, use it
                if (File.Exists(storedPath))
                    return storedPath;

                // Try interpreting as relative to the app base directory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string relativeCandidate = Path.Combine(baseDir, storedPath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                if (File.Exists(relativeCandidate))
                    return relativeCandidate;

                // Try assets folder: <base>\Assets\dataimages\<assetsFolder>\<filename>
                string filename = Path.GetFileName(storedPath);
                string assetsCandidate = Path.Combine(baseDir, assetsFolder, filename);
                if (File.Exists(assetsCandidate))
                    return assetsCandidate;

                // Last resort: try combining storedPath directly with baseDir (if storedPath was like Assets\dataimages\...)
                string combined = Path.Combine(baseDir, storedPath);
                if (File.Exists(combined))
                    return combined;
            }
            catch
            {
                // ignore resolution errors
            }

            return null;
        }

        private void HandleArchive(int memberId, string memberName, string currentStatus)
        {
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
                bool success = _memberRepository.UpdateMemberStatusByMemberId(memberId, MemberStatus.Inactive);

                if (success)
                {
                    MessageBox.Show(
                        $"{memberName} has been archived successfully.",
                        "Archive Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadMembers();
                }
                else
                {
                    MessageBox.Show(
                        "Failed to archive member.",
                        "Archive Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void HandleRenew(int memberId, string memberName, string currentStatus)
        {
            // Only allow renew for Expired members
            if (!currentStatus.Equals("Expired", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Only expired memberships can be renewed.\n\nThis member's status is: " + currentStatus,
                    "Cannot Renew",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to renew {memberName}'s membership?\n\n" +
                "• Expiration date will be extended by 1 year\n" +
                "• Status will be changed to Active",
                "Confirm Renewal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var result = _renewMemberService.RenewMembership(memberId);

                if (result.Success)
                {
                    MessageBox.Show(
                        $"{memberName}'s membership has been renewed successfully!",
                        "Renewal Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadMembers();
                }
                else
                {
                    MessageBox.Show(
                        result.ErrorMessage,
                        "Renewal Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // end code
    }
}