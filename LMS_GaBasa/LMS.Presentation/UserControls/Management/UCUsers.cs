using LMS.BusinessLogic.Services.ArchiveUser;
using LMS.BusinessLogic.Services.FetchUsers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Presentation.Popup.Users;
using LMS.Presentation.Popup.Multipurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCUsers : UserControl
    {
        private readonly IFetchUserService _userListService;
        private readonly IArchiveUserService _archiveUserService;
        private List<DTOFetchAllUsers> _allUsers;
        private List<DTOFetchAllUsers> _filteredUsers;

        // Pagination state
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        // Column name constants (use names from designer to avoid index confusion)
        private const string ColName_ProfilePicture = "ColumnProfilePicture";
        private const string ColName_Edit = "Edit";
        private const string ColName_Archive = "Archive";

        public UCUsers() : this(
            new FetchUserService(new UserRepository()),
            new ArchiveUserService(new UserRepository()))
        {
        }

        public UCUsers(IFetchUserService userListService, IArchiveUserService archiveUserService)
        {
            InitializeComponent();
            _userListService = userListService ?? throw new ArgumentNullException(nameof(userListService));
            _archiveUserService = archiveUserService ?? throw new ArgumentNullException(nameof(archiveUserService));

            this.Load += UCUsers_Load;
        }

        private void UCUsers_Load(object sender, EventArgs e)
        {
            SetupFilters();
            SetupPagination();
            LoadUsers();
        }

        private void SetupFilters()
        {
            // Setup Role filter
            CmbBxRoleFilter.Items.Clear();
            CmbBxRoleFilter.Items.Add("All Roles");
            CmbBxRoleFilter.Items.Add("Librarian / Admin");
            CmbBxRoleFilter.Items.Add("Library Staff");
            CmbBxRoleFilter.SelectedIndex = 0;

            // Setup Status filter
            CmbBxStatusFilter.Items.Clear();
            CmbBxStatusFilter.Items.Add("All Status");
            CmbBxStatusFilter.Items.Add("Active");
            CmbBxStatusFilter.Items.Add("Inactive");
            CmbBxStatusFilter.SelectedIndex = 0;

            TxtSearchBar.Text = "";

            // Wire up events
            BtnApply.Click += BtnApply_Click;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;
        }

        private void SetupPagination()
        {
            // Set default page size to 10 (designer items are "10","20","30")
            CmbBxPaginationNumbers.SelectedIndex = 0; // selects "10"
            _pageSize = 10;

            // Do not re-wire SelectedIndexChanged here — designer wires the event to CmbBxPaginationNumbers_SelectedIndexChanged_1.
            LblPaginationPrevious.Click += LblPaginationPrevious_Click;
            LblPaginationNext.Click += LblPaginationNext_Click;
        }

        private void LoadUsers()
        {
            try
            {
                // Add # column at the beginning if it doesn't exist
                if (DgwUsers.Columns["ColumnRowNum"] == null)
                {
                    var rowNumColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "ColumnRowNum",
                        HeaderText = "#",
                        Width = 40
                    };
                    DgwUsers.Columns.Insert(0, rowNumColumn);
                }

                // Fetch and cache all users
                _allUsers = _userListService.GetAllStaffUsers();

                // Reset to first page when loading
                _currentPage = 1;

                // Apply filters and pagination
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            if (_allUsers == null)
                return;

            var filteredUsers = _allUsers.AsEnumerable();

            // Apply search filter
            string searchText = TxtSearchBar.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredUsers = filteredUsers.Where(u =>
                    u.UserID.ToString().Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                    (u.FullName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (u.Username?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (u.Email?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (u.ContactNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            // Apply role filter
            string selectedRole = CmbBxRoleFilter.SelectedItem?.ToString() ?? "All Roles";
            if (selectedRole != "All Roles")
            {
                filteredUsers = filteredUsers.Where(u =>
                    u.Role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Apply status filter
            string selectedStatus = CmbBxStatusFilter.SelectedItem?.ToString() ?? "All Status";
            if (selectedStatus != "All Status")
            {
                filteredUsers = filteredUsers.Where(u =>
                    u.Status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Store filtered results
            _filteredUsers = filteredUsers.ToList();

            // Calculate pagination
            CalculatePagination();

            // Display current page
            DisplayCurrentPage();
        }

        private void CalculatePagination()
        {
            int totalRecords = _filteredUsers?.Count ?? 0;
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
            if (_filteredUsers == null)
                return;

            // Get current page data
            var pagedUsers = _filteredUsers
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            // Display in grid
            DisplayUsers(pagedUsers);

            // Update pagination label
            UpdatePaginationLabel();

            // Update pagination buttons state
            UpdatePaginationButtons();
        }

        private void DisplayUsers(List<DTOFetchAllUsers> users)
        {
            DgwUsers.Rows.Clear();

            int startIndex = (_currentPage - 1) * _pageSize;
            int rowNumber = startIndex + 1;

            foreach (var user in users)
            {
                DgwUsers.Rows.Add(
                    rowNumber++,
                    user.FormattedID,  // Changed from user.UserID
                    user.FullName,
                    user.Role,
                    user.Username,
                    user.Email,
                    user.ContactNumber,
                    user.LastLogin,
                    user.Status
                );
            }
        }

        private void UpdatePaginationLabel()
        {
            int totalRecords = _filteredUsers?.Count ?? 0;
            int startRecord = totalRecords == 0 ? 0 : ((_currentPage - 1) * _pageSize) + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);

            LblPaginationShowEntries.Text = $"Showing {startRecord} to {endRecord} of {totalRecords} entries";
        }

        private void CmbBxPaginationNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = CmbBxPaginationNumbers.SelectedItem?.ToString() ?? "10";
            if (int.TryParse(selectedValue, out int pageSize))
            {
                _pageSize = pageSize;
                _currentPage = 1; // Reset to first page
                ApplyFilters();
            }
        }

        // Designer wires SelectedIndexChanged to this method; delegate to the implemented logic above.
        private void CmbBxPaginationNumbers_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CmbBxPaginationNumbers_SelectedIndexChanged(sender, e);
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

        private void DgwUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // Get the user from filtered list using the display index
            int displayIndex = (_currentPage - 1) * _pageSize + e.RowIndex;
            if (_filteredUsers == null || displayIndex < 0 || displayIndex >= _filteredUsers.Count)
                return;

            var user = _filteredUsers[displayIndex];
            int userId = user.UserID;
            string userName = user.FullName ?? "this user";
            string currentStatus = user.Status ?? "";

            // Use the column Name to decide action (avoids hard-coded indices)
            string colName = DgwUsers.Columns[e.ColumnIndex].Name;

            // View Profile Picture button clicked
            if (string.Equals(colName, ColName_ProfilePicture, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Use the profile service to get the absolute photo path
                    var profileService = new UserProfileService(new UserRepository());
                    var profile = profileService.GetUserProfile(userId);

                    string imagePath = profile?.PhotoPath;

                    if (string.IsNullOrEmpty(imagePath) || !System.IO.File.Exists(imagePath))
                    {
                        MessageBox.Show("Profile picture not found for this user.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    using (var view = new ViewProfilePicture())
                    {
                        view.LoadProfilePicture(imagePath);
                        view.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load profile picture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return;
            }

            // Edit button clicked
            if (string.Equals(colName, ColName_Edit, StringComparison.OrdinalIgnoreCase))
            {
                EditUser editUserForm = new EditUser();
                editUserForm.LoadUser(userId);

                if (editUserForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }

                return;
            }

            // Archive button clicked
            if (string.Equals(colName, ColName_Archive, StringComparison.OrdinalIgnoreCase))
            {
                if (currentStatus.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "This user is already inactive.\n\nTo reactivate, use the Edit button and change the status to Active.",
                        "Already Archived",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Are you sure you want to archive {userName}?\n\nThis user will no longer be able to login.",
                    "Confirm Archive",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    var result = _archiveUserService.Archive(userId);

                    if (result.Success)
                    {
                        MessageBox.Show("User archived successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                    else
                    {
                        MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                return;
            }
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUserForm = new AddUser();

            if (addUserForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void UpdatePaginationButtons()
        {
            LblPaginationPrevious.Enabled = _currentPage > 1;
            LblPaginationNext.Enabled = _currentPage < _totalPages;
        }

        private void LblEntries_Click(object sender, EventArgs e)
        {

        }

        private void LblShow_Click(object sender, EventArgs e)
        {

        }

        private void LblPaginationPrevious_Click_1(object sender, EventArgs e)
        {

        }

        private void LblPaginationNext_Click_1(object sender, EventArgs e)
        {

        }

        private void LblPaginationShowEntries_Click(object sender, EventArgs e)
        {

        }

        private void TxtSearchBar_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void CmbBxRoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LblSearch_Click(object sender, EventArgs e)
        {

        }

        private void BtnApply_Click_1(object sender, EventArgs e)
        {

        }

        private void CmbBxStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void UCUsers_Load_1(object sender, EventArgs e)
        {

        }
    }
}
