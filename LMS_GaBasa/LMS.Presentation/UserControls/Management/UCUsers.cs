using LMS.BusinessLogic.Services.ArchiveUser;
using LMS.BusinessLogic.Services.FetchUsers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Presentation.Popup.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCUsers : UserControl
    {
        private readonly IFetchUserService _userListService;
        private readonly IArchiveUserService _archiveUserService;
        private List<DTOFetchAllUsers> _allUsers; // Cache all users for filtering

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

            // Load users when control is loaded
            this.Load += UCUsers_Load;
        }

        private void UCUsers_Load(object sender, EventArgs e)
        {
            SetupFilters();
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

            // Clear search placeholder text on focus
            TxtSearchBar.Text = "";

            // Wire up Apply button
            BtnApply.Click += BtnApply_Click;

            // Optional: Real-time search as user types
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;
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

                // Apply current filters
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

            // Display filtered results
            DisplayUsers(filteredUsers.ToList());
        }

        private void DisplayUsers(List<DTOFetchAllUsers> users)
        {
            DgwUsers.Rows.Clear();

            int rowNumber = 1;
            foreach (var user in users)
            {
                DgwUsers.Rows.Add(
                    rowNumber++,
                    user.UserID,
                    user.FullName,
                    user.Role,
                    user.Username,
                    user.Email,
                    user.ContactNumber,
                    user.Status
                );
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            // Real-time search as user types
            ApplyFilters();
        }

        private void DgwUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header clicks
            if (e.RowIndex < 0)
                return;

            // Get the UserID from column index 1 (ID column, after #)
            int userId = Convert.ToInt32(DgwUsers.Rows[e.RowIndex].Cells[1].Value);

            // Edit button clicked (column index 8)
            if (e.ColumnIndex == 8)
            {
                EditUser editUserForm = new EditUser();
                editUserForm.LoadUser(userId);

                if (editUserForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
            // Archive button clicked (column index 9)
            else if (e.ColumnIndex == 9)
            {
                // Get the current status from the Status column (index 7, after #)
                string currentStatus = DgwUsers.Rows[e.RowIndex].Cells[7].Value?.ToString() ?? "";

                // Check if user is already inactive
                if (currentStatus.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "This user is already inactive.\n\nTo reactivate, use the Edit button and change the status to Active.",
                        "Already Archived",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                string userName = DgwUsers.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "this user";

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
            }
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUserForm = new AddUser();

            // Refresh the grid if user was added successfully
            if (addUserForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }
    }
}
