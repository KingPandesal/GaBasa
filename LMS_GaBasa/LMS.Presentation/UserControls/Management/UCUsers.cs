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

        public UCUsers() : this(new FetchUserService(new UserRepository()))
        {
        }

        public UCUsers(IFetchUserService userListService)  // Changed to interface
        {
            InitializeComponent();
            _userListService = userListService ?? throw new ArgumentNullException(nameof(userListService));
            
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

                var users = _userListService.GetAllStaffUsers();

                foreach (var user in users)
                {
                    DgwUsers.Rows.Add(
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

            // Get the UserID from the first column
            int userId = Convert.ToInt32(DgwUsers.Rows[e.RowIndex].Cells[0].Value);

            // Edit button clicked (column index 7)
            if (e.ColumnIndex == 7)
            {
                // TODO: Implement edit functionality
                MessageBox.Show($"Edit user ID: {userId}");
            }
            // Delete button clicked (column index 8)
            else if (e.ColumnIndex == 8)
            {
                // TODO: Implement delete functionality
                var result = MessageBox.Show("Are you sure you want to delete this user?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    // TODO: Call delete service
                    MessageBox.Show($"Delete user ID: {userId}");
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
