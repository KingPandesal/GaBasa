using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.Forms
{
    public partial class MainForm : Form
    {

        // ===== 1: Fields =====
        private readonly User _currentUser;
        private readonly Role _currentRole;

        // ===== 2: Data mappings =====
        private readonly Dictionary<string, Func<UserControl>> _moduleFactories = new Dictionary<string, Func<UserControl>>(StringComparer.OrdinalIgnoreCase);

        // Sidebar mapping: Role -> Categories -> Modules
        private readonly Dictionary<Role, Dictionary<string, string[]>> _sidebarItems
            = new Dictionary<Role, Dictionary<string, string[]>>()
        {
            { Role.Librarian, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard",
                                             "Catalog" } },
                    { "INSIGHTS", new string[] { "Reports" } },
                    { "MANAGEMENT", new string[] { "Users",
                                                    "Members",
                                                    "Inventory",
                                                    "Reservations",
                                                    "Circulation",
                                                    "Fines" } },
                    { "CONFIGURATION", new string[] { "Settings" } }
                }
            },
            { Role.Staff, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard",
                                             "Catalog" } },
                    { "MANAGEMENT", new string[] { "Members",
                                                    "Inventory",
                                                    "Reservations",
                                                    "Circulation",
                                                    "Fines" } }
                }
            },
            { Role.Member, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard", 
                                             "Catalog" } },
                    { "MANAGEMENT", new string[] { "Wishlist",
                                                    "Borrowed", 
                                                    "Overdue", 
                                                    "Reserve",
                                                    "Fines", 
                                                    "History" } }
                }
            }
        };





        // ===== 3: Constructor =====
        public MainForm(User currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _currentRole = _currentUser.Role;

            InitializeSidebar(_currentRole);
            BuildModuleFactories();
            LoadContentByName("Dashboard"); // default
        }





        // ===== 4: Initialization =====
        private void InitializeSidebar(Role role)
        {
            PnlSidebar.Controls.Clear();

            // pabaliktad kay ambot
            // 1️ Logout — BOTTOM
            var logoutBtn = new Button
            {
                Text = "               Logout", // 15 spaces, reserved for icons
                Height = 40,
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif, ", 11),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += LogoutButton_Click;
            PnlSidebar.Controls.Add(logoutBtn);

            // 2️ Modules panel — FILL (ADD THIS BEFORE PROFILE)
            var modulesPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            BuildModules(modulesPanel, role);
            PnlSidebar.Controls.Add(modulesPanel);

        }




        // ===== 5: Event handlers =====
        // Handles sidebar button clicks
        private void SidebarButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string moduleName)
            {
                LoadContentByName(moduleName);
            }
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            // restart app so startup/login flow runs again.
            Application.Restart();

            // this.Hide();
            // var login = new Login(yourUserManagerInstanceHere);
            // login.Show();
            // this.Close();
        }





        // ===== 6: Role-based logic =====
        // KAKINSA NGA DASHBOARD ANG IPAKITA
        private UserControl GetDashboardByRole()
        {
            switch (_currentRole)
            {
                case Role.Librarian:
                    return new UserControls.Dashboards.UCDashboard();

                // shared dashbaords na ang librarian and staff
                // obsolete: UCDashboardStaff
                case Role.Staff:
                    return new UserControls.Dashboards.UCDashboard();

                case Role.Member:
                    return new UserControls.Dashboards.UCDashboardMember();

                default:
                    throw new InvalidOperationException("Unsupported role");
            }
        }





        // ===== 7: Sidebar construction logic =====
        // ========== ALL MODULES OF CURRENT USER'S ROLE ==========
        private void BuildModules(Panel container, Role role)
        {
            foreach (var category in _sidebarItems[role].Reverse())
            {
                var categoryPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true
                };

                foreach (var module in category.Value.Reverse())
                {
                    var btn = new Button
                    {
                        Text = "              " + module, // 15 spaces, reserved for icons
                        Height = 40,
                        Font = new Font("Microsoft Sans Serif, ", 11),
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Tag = module
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += SidebarButton_Click;

                    categoryPanel.Controls.Add(btn);
                }

                var lbl = new Label
                {
                    Text = category.Key,
                    Height = 15,
                    Dock = DockStyle.Top,
                    Padding = new Padding(10, 0, 0, 0),
                    Font = new Font("Microsoft Sans Serif, ", 7),
                    ForeColor = Color.White
                };

                categoryPanel.Controls.Add(lbl);
                container.Controls.Add(categoryPanel);
            }
        }





        // ===== 8: Module navigation system =====
        private void BuildModuleFactories()
        {
            _moduleFactories.Clear();

            // ALL ROLES
            _moduleFactories["Profile"] = () => new UserControls.UCProfile(_currentUser);
            _moduleFactories["Dashboard"] = () => GetDashboardByRole();
            _moduleFactories["Catalog"] = () => new UserControls.UCCatalog();

            // Librarian only
            _moduleFactories["Users"] = () => new UserControls.Management.UCUsers();
            _moduleFactories["Reports"] = () => new UserControls.Insights.UCReports();
            _moduleFactories["Settings"] = () => new UserControls.Configurations.UCSettings();

            // LIBRARIAN AND STAFF
            _moduleFactories["Members"] = () => new UserControls.Management.UCMembers();
            _moduleFactories["Fines"] = () => new UserControls.Management.UCFines();
            _moduleFactories["Reservations"] = () => new UserControls.Management.UCReservation();
            _moduleFactories["Inventory"] = () => new UserControls.Management.UCInventory();
            _moduleFactories["Circulation"] = () => new UserControls.Management.UCCirculation();
            _moduleFactories["History"] = () => new UserControls.MemberFeatures.UCHistory();

            // members only
            _moduleFactories["Borrowed"] = () => new UserControls.MemberFeatures.UCBorrowed();
            _moduleFactories["Overdue"] = () => new UserControls.MemberFeatures.UCOverdue();
            _moduleFactories["Reserve"] = () => new UserControls.MemberFeatures.UCReserve();


            var knownModules = new[]
            {
                "Members", "Catalog", "Transactions", "Fines", "Inventory",
                "Reports", "Settings", "Announcements", "Notifications",
                "Books", "Borrowed", "Overdue", "Reserve", "Wishlist", "History"
            };

            foreach (var mod in knownModules)
            {
                if (!_moduleFactories.ContainsKey(mod))
                    _moduleFactories[mod] = () => CreateNotImplementedControl(mod);
            }
        }

        private void LoadContentByName(string name)
        {
            PnlContent.Controls.Clear();

            if (string.IsNullOrWhiteSpace(name))
                return;

            if (_moduleFactories.TryGetValue(name, out var factory))
            {
                var uc = factory?.Invoke();
                if (uc != null)
                {
                    uc.Dock = DockStyle.Fill;
                    PnlContent.Controls.Add(uc);
                    return;
                }
            }

            // Fallback placeholder
            var placeholder = CreateNotImplementedControl(name);
            placeholder.Dock = DockStyle.Fill;
            PnlContent.Controls.Add(placeholder);
        }





        // ===== 9: Fallback helpers =====
        private UserControl CreateNotImplementedControl(string name)
        {
            var uc = new UserControl();
            var lbl = new Label
            {
                Text = $"{name} module is not implemented yet.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            uc.Controls.Add(lbl);
            return uc;
        }

        // end code
    }
}
