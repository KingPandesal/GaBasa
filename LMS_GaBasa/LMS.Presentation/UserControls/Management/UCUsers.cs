using LMS.BusinessLogic.Services.ArchiveUser;
using LMS.BusinessLogic.Services.FetchUsers;
using LMS.DataAccess.Repositories;
using LMS.Presentation.Popup.Users;
using System;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCUsers : UserControl
    {
        private readonly IFetchUserService _userListService;
        private readonly IArchiveUserService _archiveUserService;

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
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                DgwUsers.Rows.Clear();

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

                var users = _userListService.GetAllStaffUsers();

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
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
