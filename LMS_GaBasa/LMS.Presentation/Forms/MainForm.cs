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
        // MOSTLY PARA SA SET-UP NI SYA SA SIDEBAR AND TOPBAR KAY DRI IBUTANG ANG TANAN USERCONTROLS

        // ===== 1: Fields =====
        private readonly User _currentUser;
        private readonly Role _currentRole;

        // cached placeholder icons for modules
        private readonly Dictionary<string, Image> _moduleIcons = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

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

            // initialize topbar/profile controls (labels, picture, click)
            InitializeTopBarProfile();

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
                Text = "Logout",
                Height = 40,
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif, ", 11),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += LogoutButton_Click;

            // add placeholder icon to logout
            logoutBtn.Image = CreatePlaceholderIcon("Logout");
            logoutBtn.ImageAlign = ContentAlignment.MiddleLeft;
            logoutBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            logoutBtn.Padding = new Padding(10, 0, 0, 0);

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

        // ===== New: TopBar / Profile initialization =====
        // Makes LblProfileName, LblProfileRole and PicBxProfilePic functional.
        private void InitializeTopBarProfile()
        {
            try
            {
                // set name (prefer GetFullName if available)
                string displayName = null;
                try { displayName = _currentUser?.GetFullName(); } catch { /* ignore */ }
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    var first = _currentUser?.FirstName ?? string.Empty;
                    var last = _currentUser?.LastName ?? string.Empty;
                    displayName = (first + " " + last).Trim();
                }
                if (string.IsNullOrWhiteSpace(displayName))
                    displayName = _currentUser?.Username ?? "User";

                LblProfileName.Text = displayName;
                LblProfileRole.Text = _currentRole.ToString();

                // set profile picture (use user-provided image if available, otherwise placeholder)
                PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
                Image userImage = TryGetUserImage();
                if (userImage != null)
                {
                    PicBxProfilePic.Image = userImage;
                }
                else
                {
                    PicBxProfilePic.Image = CreateProfilePlaceholderImage(displayName, PicBxProfilePic.Width, PicBxProfilePic.Height);
                }

                // visual affordance + click
                PicBxProfilePic.Cursor = Cursors.Hand;
                // remove duplicated handler if designer already hooked it
                PicBxProfilePic.Click -= PicBxProfilePic_Click;
                PicBxProfilePic.Click += PicBxProfilePic_Click;

                // optionally allow clicking the header panel or name to open profile too
                PnlProfileHeader.Cursor = Cursors.Hand;
                PnlProfileHeader.Click -= PicBxProfilePic_Click;
                PnlProfileHeader.Click += PicBxProfilePic_Click;
                LblProfileName.Click -= PicBxProfilePic_Click;
                LblProfileName.Click += PicBxProfilePic_Click;
                LblProfileRole.Click -= PicBxProfilePic_Click;
                LblProfileRole.Click += PicBxProfilePic_Click;

                // small tooltip
                var tt = new ToolTip();
                tt.SetToolTip(PicBxProfilePic, "Open profile");
                tt.SetToolTip(PnlProfileHeader, "Open profile");
            }
            catch
            {
                // fail silently - profile is decorative if something goes wrong
            }
        }

        // If your User model had an image property, return it here.
        // This stub returns null because the current User signature doesn't include an image.
        private Image TryGetUserImage()
        {
            // Example if you later add Image or byte[] to User:
            // if (_currentUser?.PhotoBytes != null) { using (var ms = new MemoryStream(_currentUser.PhotoBytes)) return Image.FromStream(ms); }
            return null;
        }

        // Create a circular placeholder sized for the profile picture box.
        private Image CreateProfilePlaceholderImage(string key, int width, int height)
        {
            if (string.IsNullOrEmpty(key))
                key = "?";

            var size = Math.Max(32, Math.Min(Math.Max(width, height), 128)); // reasonable bounds
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var h = Math.Abs(key.GetHashCode());
                var r = (byte)(h % 200 + 20);
                var gr = (byte)((h / 31) % 200 + 20);
                var b = (byte)((h / 17) % 200 + 20);
                var bg = Color.FromArgb(r, gr, b);

                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(bg))
                {
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);
                }

                var letter = char.ToUpperInvariant(key[0]).ToString();
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    // scale font based on size
                    float fontSize = Math.Max(10f, size / 2.5f);
                    using (var fnt = new Font("Segoe UI", fontSize, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(letter, fnt, brush, new RectangleF(0, 0, size, size), sf);
                    }
                }
            }
            return bmp;
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

        // When the profile picture (or header) is clicked, open the Profile module
        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            // the module factory already provides UCProfile that expects the current user
            LoadContentByName("Profile");
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
                        Text = module,
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

                    // placeholder icon and layout adjustments
                    btn.Image = CreatePlaceholderIcon(module);
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.Padding = new Padding(10, 0, 0, 0);

                    categoryPanel.Controls.Add(btn);
                }

                var lbl = new Label
                {
                    Text = category.Key,
                    Height = 15,
                    Dock = DockStyle.Top,
                    Padding = new Padding(10, 0, 0, 0),
                    Font = new Font("Microsoft Sans Serif", 7),
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

            // update the topbar/title label
            UpdateModuleTitle(name);

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

        // ===== 10: Module title updater =====
        private void UpdateModuleTitle(string name)
        {
            var display = string.IsNullOrWhiteSpace(name) ? string.Empty : name;

            Label lbl = null;

            // Try common designer names (search recursively)
            lbl = this.Controls.Find("LblModuleTitle", true).FirstOrDefault() as Label
               ?? this.Controls.Find("LblModuleName", true).FirstOrDefault() as Label
               ?? this.Controls.Find("lblModule", true).FirstOrDefault() as Label;

            if (lbl != null)
            {
                lbl.Text = display;
                return;
            }

            // If you have a top panel named PnlTopBar, add a label there
            var topBar = this.Controls.Find("PnlTopBar", true).FirstOrDefault() as Control;
            if (topBar != null)
            {
                var newLbl = new Label
                {
                    Name = "LblModuleTitle",
                    Text = display,
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                topBar.Controls.Add(newLbl);
                return;
            }

            // As a last resort, set the form's Text (window title)
            this.Text = display;
        }

        // ===== 11: Placeholder icon generator =====
        // Creates a small circular colored icon with the module's first letter.
        // Icons are cached in _moduleIcons to avoid recreating bitmaps repeatedly.
        private Image CreatePlaceholderIcon(string key)
        {
            if (string.IsNullOrEmpty(key))
                key = "?";

            Image cached;
            if (_moduleIcons.TryGetValue(key, out cached))
                return cached;

            const int size = 24;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // determine color from stable hash
                var h = Math.Abs(key.GetHashCode());
                var r = (byte)(h % 200 + 20);
                var gr = (byte)((h / 31) % 200 + 20);
                var b = (byte)((h / 17) % 200 + 20);
                var bg = Color.FromArgb(r, gr, b);

                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(bg))
                {
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);
                }

                // draw first letter
                var letter = char.ToUpperInvariant(key[0]).ToString();
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var fnt = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(letter, fnt, brush, new RectangleF(0, 0, size, size), sf);
                }
            }

            _moduleIcons[key] = bmp;
            return bmp;
        }

        // end code
        
    }
}
