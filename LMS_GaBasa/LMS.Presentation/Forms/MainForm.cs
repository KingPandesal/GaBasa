using LMS.BusinessLogic.Security;
using LMS.Model.Models.Users;
using LMS.Presentation.UI.MainForm.ModuleIcon;
using LMS.Presentation.UI.MainForm.Navigation;
using LMS.Presentation.UI.MainForm.Sidebar;
using LMS.Presentation.UI.MainForm.TopBar;
using LMS.Presentation.UserControls.Profile;
using LMS.BusinessLogic.Services;
using LMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IUserProfileService _userProfileService;
        private Label _lblProfileRole; // Added field for profile role label
        private readonly IPermissionService _permissionService;

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
            : this(currentUser, new RolePermissionService(), new ModuleNavigator(), null, null, null)
        {
        }

        public MainForm(User currentUser, IPermissionService permissionService)
            : this(currentUser, permissionService, new ModuleNavigator(), null, null, null)
        {
        }

        // internal extended ctor (allows injecting navigator, sidebar builder and topbar controller for testing)
        internal MainForm(User currentUser, 
            IPermissionService permissionService, 
            IModuleNavigator moduleNavigator, 
            ISidebarBuilder sidebarBuilder, 
            ITopBarController topBarController,
            IUserProfileService userProfileService)
        {
            InitializeComponent();

            // cache permission service so we can pass it to child UserControls later
            _permissionService = permissionService ?? new RolePermissionService();

            // Cache the profile role label reference
            _lblProfileRole = this.Controls.Find("LblProfileRole", true).FirstOrDefault() as Label;

            ModuleIcons.LoadIcons();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _moduleNavigator = moduleNavigator ?? throw new ArgumentNullException(nameof(moduleNavigator));

            // Initialize the navigator first so permission checks / factories are available
            _moduleNavigator.Initialize(_currentUser, _permissionService);

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

            _userProfileService = userProfileService
                ?? new UserProfileService(new UserRepository());

            // Fetch fresh profile from database to get PhotoPath
            var profile = _userProfileService.GetUserProfile(_currentUser.UserID);

            // Initialize topbar using profile data (includes photo) if available
            if (profile != null)
            {
                _topBarController.InitializeProfile(
                    _currentUser,
                    permissionService,
                    PicBxProfilePic,
                    LblProfileName,
                    _lblProfileRole, // Use the field instead of lookup
                    PnlProfileHeader,
                    () => LoadContentByName("Profile"));

                // Immediately refresh with database profile to load the photo
                _topBarController.RefreshProfile(profile, PicBxProfilePic, LblProfileName,
                    _lblProfileRole);
            }
            else
            {
                // Fallback to User object if profile fetch fails
                _topBarController.InitializeProfile(
                    _currentUser,
                    permissionService,
                    PicBxProfilePic,
                    LblProfileName,
                    _lblProfileRole,
                    PnlProfileHeader,
                    () => LoadContentByName("Profile"));
            }

            LoadContentByName("Dashboard"); // default
        }

        // ===== 5: Event handlers =====
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to log out? The application will exit.",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            // Exit the application to avoid restart/login flow issues.
            try
            {
                Application.Exit();
                // Ensure termination if Application.Exit doesn't immediately stop (defensive).
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }
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

            _topBarController.UpdateModuleIcon(name, PicBxModuleIcon);

            var uc = _moduleNavigator.CreateModule(name);
            if (uc != null)
            {
                uc.Dock = DockStyle.Fill;

                // If the created control is the catalog, give it the current user + permission service.
                // This ensures Reserve button visibility is decided from DB-backed role via RolePermissionService.
                if (uc is LMS.Presentation.UserControls.UCCatalog catalogUC)
                {
                    try
                    {
                        catalogUC.SetPermissionContext(_currentUser, _permissionService ?? new RolePermissionService());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to set permission context on UCCatalog: " + ex);
                    }
                }

                // Subscribe to profile update event if this is the Librarian/Staff profile module
                if (uc is UCLibrarianStaff librarianStaffUC)
                {
                    librarianStaffUC.ProfileUpdated += () =>
                    {
                        RefreshCurrentUserAndTopBar();
                    };
                }

                // Subscribe to profile update event if this is the Member profile module
                if (uc is UCMemberProfile memberProfileUC)
                {
                    memberProfileUC.ProfileUpdated += () =>
                    {
                        RefreshCurrentUserAndTopBar();
                    };
                }

                PnlContent.Controls.Add(uc);
            }
        }

        private void RefreshCurrentUserAndTopBar()
        {
            // Fetch fresh profile from database
            var profile = _userProfileService.GetUserProfile(_currentUser.UserID);

            if (profile == null) return;

            // Refresh top bar display using profile DTO directly
            _topBarController.RefreshProfile(profile, PicBxProfilePic, LblProfileName,
                _lblProfileRole); // Use the field instead of lookup
        }

        // end code
    }
}
