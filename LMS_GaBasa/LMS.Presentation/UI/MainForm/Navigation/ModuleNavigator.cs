using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Security;
using LMS.Model.Models.Users;

namespace LMS.Presentation.UI.MainForm.Navigation
{
    public class ModuleNavigator : IModuleNavigator
    {
        private readonly Dictionary<string, Func<UserControl>> _factories = new Dictionary<string, Func<UserControl>>(StringComparer.OrdinalIgnoreCase);
        private User _currentUser;
        private IPermissionService _permissionService;

        // Implement interface signature exactly
        public void Initialize(User currentUser, IPermissionService permissionService)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            BuildDefaultFactories();
        }

        public void RegisterFactories(IDictionary<string, Func<UserControl>> factories)
        {
            if (factories == null) return;
            foreach (var kv in factories)
                _factories[kv.Key] = kv.Value;
        }

        public UserControl CreateModule(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                return CreateNotImplementedControl(moduleName);

            if (_factories.TryGetValue(moduleName, out var factory))
            {
                try
                {
                    var uc = factory?.Invoke();
                    if (uc != null)
                    {
                        uc.Dock = DockStyle.Fill;
                        return uc;
                    }
                }
                catch
                {
                    // fall through to fallback control
                }
            }

            return CreateNotImplementedControl(moduleName);
        }

        // New: permission mapping moved from MainForm
        public bool IsModuleAllowed(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                return false;

            var mod = moduleName.Trim();

            // Member-facing modules
            if (string.Equals(mod, "My Borrowed", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewBorrowed(_currentUser);

            if (string.Equals(mod, "My Overdue", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewOverdue(_currentUser);

            if (string.Equals(mod, "My Reserve", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewReserved(_currentUser);

            if (string.Equals(mod, "My Wishlist", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewWishlist(_currentUser);

            if (string.Equals(mod, "My History", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewHistory(_currentUser);

            if (string.Equals(mod, "My Fines", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewFines(_currentUser);

            // Management / staff modules
            if (string.Equals(mod, "Users", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanManageUsers(_currentUser);

            if (string.Equals(mod, "Members", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanManageMembers(_currentUser);

            if (string.Equals(mod, "Inventory", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanManageInventory(_currentUser);

            if (string.Equals(mod, "Reservations", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanManageReservations(_currentUser);

            if (string.Equals(mod, "Circulation", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanManageCirculation(_currentUser);

            if (string.Equals(mod, "Fines", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanManageFines(_currentUser);

            // Insights / reports
            if (string.Equals(mod, "Reports", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanGenerateReports(_currentUser);

            // Configuration
            if (string.Equals(mod, "Settings", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanAccessSettings(_currentUser);

            // Catalog
            if (string.Equals(mod, "Catalog", StringComparison.OrdinalIgnoreCase))
                return _permissionService.CanViewCatalog(_currentUser);

            // Dashboard and Profile always available
            if (string.Equals(mod, "Dashboard", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(mod, "Profile", StringComparison.OrdinalIgnoreCase))
                return true;

            // Default: allow (UI will show fallback content if needed)
            return true;
        }

        private void BuildDefaultFactories()
        {
            _factories.Clear();

            // ALL ROLES
            _factories["Profile"] = () => GetProfileByRole();
            _factories["Dashboard"] = () => GetDashboardByRole();
            _factories["Catalog"] = () => new UserControls.UCCatalog();

            // Librarian only / staff-managed modules
            _factories["Users"] = () => new UserControls.Management.UCUsers();
            _factories["Reports"] = () => new UserControls.Insights.UCReports();
            _factories["Settings"] = () => new UserControls.Configurations.UCSettings();

            // LIBRARIAN AND STAFF
            _factories["Members"] = () => new UserControls.Management.UCMembers();
            _factories["Fines"] = () => new UserControls.Management.UCFines();
            _factories["Reservations"] = () => new UserControls.Management.UCReservation();
            _factories["Inventory"] = () => new UserControls.Management.UCInventory();
            _factories["Circulation"] = () => new UserControls.Management.UCCirculation();

            // members only
            _factories["My Borrowed"] = () => new UserControls.MemberFeatures.UCBorrowed();
            _factories["My Overdue"] = () => new UserControls.MemberFeatures.UCOverdue();
            _factories["My Reserve"] = () => new UserControls.MemberFeatures.UCReserve();
            _factories["My History"] = () => new UserControls.MemberFeatures.UCHistory();
            _factories["My Fines"] = () => new UserControls.MemberFeatures.UCMyFines();

            var knownModules = new[]
            {
                "Members", "Catalog", "Transactions", "Fines", "Inventory",
                "Reports", "Settings", "Announcements", "Notifications",
                "Books", "Borrowed", "Overdue", "Reserve", "Wishlist", "History"
            };

            foreach (var mod in knownModules)
            {
                if (!_factories.ContainsKey(mod))
                    _factories[mod] = () => CreateNotImplementedControl(mod);
            }
        }

        private UserControl GetDashboardByRole()
        {
            if (_permissionService == null || _currentUser == null)
                return new UserControls.Dashboards.UCDashboard();

            if (_permissionService.CanGenerateReports(_currentUser) || _permissionService.CanManageUsers(_currentUser))
            {
                // staff / librarian view
                return new UserControls.Dashboards.UCDashboard();
            }

            if (_permissionService.CanBorrowBooks(_currentUser) || _permissionService.CanViewBorrowed(_currentUser))
            {
                // member view
                return new UserControls.Dashboards.UCDashboardMember();
            }

            // default fallback dashboard
            return new UserControls.Dashboards.UCDashboard();
        }

        // Updated: return profile control based on the user's Role (no dependency on deleted UCProfile)
        private UserControl GetProfileByRole()
        {
            // If we don't have a user, return a fallback placeholder.
            if (_currentUser == null)
                return CreateNotImplementedControl("Profile");

            // Use the explicit Role to choose the profile control.
            switch (_currentUser.Role)
            {
                case Role.Librarian:
                case Role.Staff:
                    // Librarian and Staff share the librarian/staff profile control
                    return new UserControls.Profile.UCLibrarianStaff();

                case Role.Member:
                    // Members get the member profile control
                    return new UserControls.Profile.UCMemberProfile();

                default:
                    // Unknown role — return placeholder
                    return CreateNotImplementedControl("Profile");
            }
        }

        private UserControl CreateNotImplementedControl(string name)
        {
            var uc = new UserControl();
            var lbl = new Label
            {
                Text = string.IsNullOrWhiteSpace(name) ? "Module is not implemented yet." : $"{name} module is not implemented yet.",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = System.Drawing.Color.DimGray,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular)
            };
            uc.Controls.Add(lbl);
            return uc;
        }
    }
}
