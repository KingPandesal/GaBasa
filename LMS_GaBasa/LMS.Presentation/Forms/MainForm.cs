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
                    { "MAIN", new string[] { "Dashboard" } },
                    { "MANAGEMENT", new string[] { "Users",
                                                    "Members",
                                                    "Catalog",
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
                    { "MAIN", new string[] { "Dashboard" } },
                    { "MANAGEMENT", new string[] { "Members", 
                                                    "Catalog", 
                                                    "Transactions", 
                                                    "Fines", 
                                                    "Inventory" } },
                    { "INSIGHTS", new string[] { "Reports" } }
                }
            },
            { Role.Member, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard" } },
                    { "MANAGEMENT", new string[] { "Books", 
                                                    "Borrowed", 
                                                    "Overdue", 
                                                    "Reserve", 
                                                    "Wishlist", 
                                                    "Fines", 
                                                    "History" } }
                }
            }
        };

        // Generates sidebar based on role and categories
        private void InitializeSidebar(Role role)
        {
            PnlSidebar.Controls.Clear();
            PnlSidebar.Dock = DockStyle.None;
            PnlSidebar.AutoScroll = true;

            // Logout FIRST (Dock.Bottom rule)
            var logoutBtn = new Button
            {
                Text = "  Logout",
                Font = new Font("Microsoft Sans Serif", 13, FontStyle.Bold),
                ForeColor = Color.White,
                Height = 40,
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += LogoutButton_Click;
            PnlSidebar.Controls.Add(logoutBtn);

            // Build categories top-down
            foreach (var category in _sidebarItems[role].Reverse())
            {
                var categoryPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true
                };

                // Add buttons (reverse so visual order is correct)
                foreach (var module in category.Value.Reverse())
                {
                    var btn = new Button
                    {
                        Text = "  " + module,
                        Height = 40,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Microsoft Sans Serif", 13, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Tag = module,
                        ForeColor = Color.White
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += SidebarButton_Click;

                    categoryPanel.Controls.Add(btn);
                }

                // Add catgegory label LAST (so it appears on top)
                var lbl = new Label
                {
                    Text = category.Key,
                    Height = 25,
                    Dock = DockStyle.Top,
                    Font = new Font("Microsoft Sans Serif", 10),
                    Padding = new Padding(10, 0, 0, 0),
                    ForeColor = Color.White
                };

                categoryPanel.Controls.Add(lbl);
                PnlSidebar.Controls.Add(categoryPanel);
            }
        }


        // Handles sidebar button clicks
        private void SidebarButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string moduleName)
            {
                LoadContentByName(moduleName);
            }
        }

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

        private UserControl GetDashboardByRole()
        {
            switch (_currentRole)
            {
                case Role.Librarian:
                    return new UserControls.Dashboards.UCDashboardLibrarian();

                case Role.Staff:
                    return new UserControls.Dashboards.UCDashboardStaff();

                case Role.Member:
                    return new UserControls.Dashboards.UCDashboardMember();

                default:
                    throw new InvalidOperationException("Unsupported role");
            }
        }

        private void BuildModuleFactories()
        {
            _moduleFactories.Clear();
            _moduleFactories["Dashboard"] = () => GetDashboardByRole();
            _moduleFactories["Users"] = () => new UserControl();

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

    }
}
