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

        // add this field near the existing private Role _currentRole;
        private User _currentUser;
        private Role _currentRole;

        public MainForm(User currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _currentRole = _currentUser.Role;

            InitializeSidebar(_currentRole);
            BuildModuleFactories();
            LoadContentByName("Dashboard"); // default
        }

        // Factory map for creating UserControls by module name
        private readonly Dictionary<string, Func<UserControl>> _moduleFactories = new Dictionary<string, Func<UserControl>>(StringComparer.OrdinalIgnoreCase);

        // Sidebar mapping: Role -> Categories -> Modules
        private readonly Dictionary<Role, Dictionary<string, string[]>> _sidebarItems
            = new Dictionary<Role, Dictionary<string, string[]>>()
        {
            { Role.Librarian, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard" } },
                    { "MANAGEMENT", new string[] { "Users", "Catalog", "Transactions", "Fines", "Inventory" } },
                    { "INSIGHTS", new string[] { "Reports" } },
                    { "CONFIGURATION", new string[] { "Settings", "Announcements", "Notifications" } }
                }
            },
            { Role.Staff, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard" } },
                    { "MANAGEMENT", new string[] { "Members", "Catalog", "Transactions", "Fines", "Inventory" } },
                    { "INSIGHTS", new string[] { "Reports" } }
                }
            },
            { Role.Member, new Dictionary<string, string[]>
                {
                    { "MAIN", new string[] { "Dashboard" } },
                    { "MANAGEMENT", new string[] { "Books", "Borrowed", "Overdue", "Reserve", "Wishlist", "Fines", "History" } }
                }
            }
        };


        /// <summary>
        /// Generates sidebar dynamically based on role and categories
        /// </summary>
        private void InitializeSidebar(Role role)
        {
            PnlSidebar.Controls.Clear();

            foreach (var category in _sidebarItems[role])
            {
                // Category label
                Label lbl = new Label
                {
                    Text = category.Key,
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    Height = 25,
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0)
                };
                PnlSidebar.Controls.Add(lbl);

                // Buttons
                foreach (var module in category.Value.Reverse()) // reverse so top button is first
                {
                    Button btn = new Button
                    {
                        Text = "  " + module, // extra space for icon alignment
                        Height = 40,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Tag = module // store module name for click handler
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += SidebarButton_Click;

                    PnlSidebar.Controls.Add(btn);
                }
            }
        }

        /// <summary>
        /// Handles sidebar button clicks
        /// </summary>
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
                    return new UserControls.Dashboards.UCDashboardLibrarian();

                case Role.Staff:
                    // there is an existing form DashboardStaff; prefer a UserControl. If you have a UC version use it.
                    return new UserControls.Dashboards.UCDashboardStaff();

                case Role.Member:
                    return new UserControls.Dashboards.UCDashboardMember();

                default:
                    throw new InvalidOperationException("Unsupported role");
            }
        }

        /// <summary>
        /// Build factory map for modules. Keeps module creation centralized and testable.
        /// </summary>
        private void BuildModuleFactories()
        {
            _moduleFactories.Clear();

            // Dashboard is role-specific
            _moduleFactories["Dashboard"] = () => GetDashboardByRole();

            // Wire modules that exist in the project (leave others as "not implemented" placeholders)
            // Keep names consistent with sidebar module strings.
            _moduleFactories["Users"] = () => new UserControl();

            // If you have concrete UserControls for these modules, replace the CreateNotImplementedControl entries with constructors.
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

        /// <summary>
        /// Loads the UserControl corresponding to the module name into the content panel
        /// Uses the factory map to create controls so creation is centralized (OCP, SRP).
        /// </summary>
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

        /// <summary>
        /// Small runtime placeholder so UI doesn't break when a module UC is not yet implemented.
        /// Keeps code safe while you incrementally convert forms to UserControls.
        /// </summary>
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
