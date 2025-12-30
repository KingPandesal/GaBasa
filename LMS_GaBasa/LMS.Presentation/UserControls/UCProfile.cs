using System;
using System.Windows.Forms;
using LMS.Model.Models.Users;
using LMS.BusinessLogic.Security;

namespace LMS.Presentation.UserControls
{
    public partial class UCProfile : UserControl
    {
        private readonly User _currentUser;
        private readonly IPermissionService _permissionService;

        public UCProfile()
        {
            InitializeComponent();
        }

        // Inject permission service (composition root / caller must provide it)
        public UCProfile(User currentUser, IPermissionService permissionService)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            //LblFullname.Text = _currentUser.GetFullName();
            LblRole.Text = _currentUser.Role.ToString();

            // Role/permission-specific UI
            ConfigureRoleSpecificUI();
        }

        private void ConfigureRoleSpecificUI()
        {
            // read capabilities from centralized permission service
            bool isMember = _permissionService.CanBorrowBooks(_currentUser);
            PnlMemberPrivilege.Visible = isMember;
            PnlRegExpDate.Visible = isMember;
        }

        private void UCProfile_Load(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label16_Click(object sender, EventArgs e) { }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {

        }
    }
}
