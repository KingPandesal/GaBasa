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

        private readonly User _currentUser;
        private readonly Role _currentRole;

        public MainForm(User currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _currentRole = _currentUser.Role;

            InitializeSidebar(_currentRole);
            BuildModuleFactories();
            LoadContentByName("Dashboard"); // default
        }

        private readonly Dictionary<string, Func<UserControl>> _moduleFactories = new Dictionary<string, Func<UserControl>>(StringComparer.OrdinalIgnoreCase);

        // Sidebar mapping: Role -> Categories -> Modules
        private readonly Dictionary<Role, Dictionary<string, string[]>> _sidebarItems
            = new Dictionary<Role, Dictionary<string, string[]>>()
        {
            { Role.Librarian, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard",
                                             "Catalog" } },
                    { "MANAGEMENT", new string[] { "Users",
                                                    "Members",
                                                    "Circulation",
                                                    "Reservations",
                                                    "Inventory",
                                                    "Fines" } },
                    { "INSIGHTS", new string[] { "Reports" } },
                    { "CONFIGURATION", new string[] { "Settings" } }
                }
            },
            { Role.Staff, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard",
                                             "Catalog" } },
                    { "MANAGEMENT", new string[] { "Members",
                                                    "Circulation",
                                                    "Fines",
                                                    "Reservations",
                                                    "Inventory" } }
                }
            },
            { Role.Member, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard", 
                                             "Catalog" } },
                    { "MANAGEMENT", new string[] { "Borrowed", 
                                                    "Overdue", 
                                                    "Reserve", 
                                                    //"Wishlist", 
                                                    "Fines", 
                                                    "History" } }
                }
            }
        };

        // ========== SIDEBAR ==========
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

            // 3️ Profile header — TOP (ADD LAST)
            var profileHeader = CreateProfileHeader();
            profileHeader.Dock = DockStyle.Top;
            PnlSidebar.Controls.Add(profileHeader);
        }
        
        // Handles sidebar button clicks
        private void SidebarButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string moduleName)
            {
                LoadContentByName(moduleName);
            }
        }

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

        // ========== PROFILE MODULE ==========
        private Panel CreateProfileHeader()
        {
            // Profile panel
            var panel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                Padding = new Padding(10)
            };

            // Profile pic
            var pic = new PictureBox
            {
                Size = new Size(60, 60),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Full Name
            var lblName = new Label
            {
                Text = "Zy Manti",
                Location = new Point(80, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White
            };

            // Role
            var lblRole = new Label
            {
                Text = _currentRole.ToString(),
                Location = new Point(80, 45),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblRole);

            // If click: load profile module
            panel.Cursor = Cursors.Hand;
            panel.Click += (s, e) => LoadContentByName("Profile");
            lblName.Cursor = Cursors.Hand;
            lblName.Click += (s, e) => LoadContentByName("Profile");
            lblRole.Cursor = Cursors.Hand;
            lblRole.Click += (s, e) => LoadContentByName("Profile");

            return panel;
        }

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

        // ========== LOGOUT ==========
        // Logout click handler
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

        // ========== MORE ==========
        private void BuildModuleFactories()
        {
            _moduleFactories.Clear();
            _moduleFactories["Dashboard"] = () => GetDashboardByRole();
            _moduleFactories["Users"] = () => new UserControls.Management.UCUsers();
            _moduleFactories["Members"] = () => new UserControls.Management.UCMembers();
            _moduleFactories["Catalog"] = () => new UserControls.UCCatalog();
            _moduleFactories["Fines"] = () => new UserControls.Management.UCFines();
            _moduleFactories["Reservations"] = () => new UserControls.Management.UCReservation();
            _moduleFactories["Inventory"] = () => new UserControls.Management.UCInventory();
            _moduleFactories["Settings"] = () => new UserControls.Configurations.UCSettings();
            _moduleFactories["Circulation"] = () => new UserControls.Management.UCCirculation();
            _moduleFactories["Reports"] = () => new UserControls.Insights.UCReports();
            _moduleFactories["History"] = () => new UserControls.MemberFeatures.UCHistory();

            // members only
            _moduleFactories["Borrowed"] = () => new UserControls.MemberFeatures.UCBorrowed();
            _moduleFactories["Overdue"] = () => new UserControls.MemberFeatures.UCOverdue();
            _moduleFactories["Reserve"] = () => new UserControls.MemberFeatures.UCReserve();

            // Add Profile factory so clicking the profile header shows UCProfile for all roles
            _moduleFactories["Profile"] = () => new UserControls.UCProfile(_currentUser);

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

        private void LblProfileHeaderName_Click(object sender, EventArgs e)
        {

        }

        // end code
    }
}
