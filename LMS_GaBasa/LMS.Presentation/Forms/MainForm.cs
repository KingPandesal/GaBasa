using LMS.BusinessLogic.Security;
using LMS.Model.Models.Users;
using LMS.Presentation.UI.MainForm.Navigation;
using LMS.Presentation.UI.MainForm.Sidebar;
using LMS.Presentation.UI.MainForm.TopBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.Presentation.Forms
{
    public partial class MainForm : Form
    {
        // ===== 1: Fields =====
        private readonly User _currentUser;
        private readonly IModuleNavigator _moduleNavigator;
        private readonly ISidebarBuilder _sidebarBuilder;
        private readonly ITopBarController _topBarController;

        // Static sidebar layout: Category -> Modules
        private readonly Dictionary<string, string[]> _sidebarLayout = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "MAIN", new string[] { "Dashboard", "Catalog" } },
            { "INSIGHTS", new string[] { "Reports" } },
            { "MANAGEMENT", new string[] { "Users", "Members", "Inventory", "Reservations", "Circulation", "Fines" } },
            { "CONFIGURATION", new string[] { "Settings" } },
            { "MEMBERS", new string[] { "My Wishlist", "My Borrowed", "My Overdue", "My Reserve", "My Fines", "My History" } }
        };

        // ===== 3: Constructors =====
        public MainForm(User currentUser)
            : this(currentUser, new RolePermissionService(), new ModuleNavigator(), null, null)
        {
        }

        public MainForm(User currentUser, IPermissionService permissionService)
            : this(currentUser, permissionService, new ModuleNavigator(), null, null)
        {
        }

        // internal extended ctor (allows injecting navigator, sidebar builder and topbar controller for testing)
        internal MainForm(User currentUser, IPermissionService permissionService, IModuleNavigator moduleNavigator, ISidebarBuilder sidebarBuilder, ITopBarController topBarController)
        {
            InitializeComponent();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _moduleNavigator = moduleNavigator ?? throw new ArgumentNullException(nameof(moduleNavigator));

            // Initialize the navigator first so permission checks / factories are available
            _moduleNavigator.Initialize(_currentUser, permissionService);

            // sidebar builder either injected or use default implementation
            _sidebarBuilder = sidebarBuilder ?? new SidebarBuilder(_moduleNavigator);

            // topbar controller either injected or default
            _topBarController = topBarController ?? new TopBarController();

            // Build sidebar (delegated to SidebarBuilder)
            _sidebarBuilder.BuildSidebar(
                PnlSidebar,
                _sidebarLayout,
                moduleName => LoadContentByName(moduleName),
                () => LogoutButton_Click(this, EventArgs.Empty),
                "Dashboard"); // default selected module

            // initialize topbar/profile controls via controller
            _topBarController.InitializeProfile(
                _currentUser,
                permissionService,
                PicBxProfilePic,
                LblProfileName,
                this.Controls.Find("LblProfileRole", true).FirstOrDefault() as Label,
                PnlProfileHeader,
                () => LoadContentByName("Profile"));

            LoadContentByName("Dashboard"); // default
        }

        // ===== 5: Event handlers =====
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            // restart app so startup/login flow runs again.
            Application.Restart();
        }

        // ===== 8: Module navigation system =====
        // Load content now delegates creation to the navigator.
        private void LoadContentByName(string name)
        {
            PnlContent.Controls.Clear();

            if (string.IsNullOrWhiteSpace(name))
                return;

            // update the topbar/title label via controller
            _topBarController.UpdateModuleTitle(name, this);

            var uc = _moduleNavigator.CreateModule(name);
            if (uc != null)
            {
                uc.Dock = DockStyle.Fill;
                PnlContent.Controls.Add(uc);
            }
        }

        // end code
    }
}
