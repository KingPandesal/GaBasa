using LMS.BusinessLogic.Security;
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
        private readonly IPermissionService _permissionService;

        private readonly Dictionary<string, Image> _moduleIcons = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        // ===== 2: Data mappings =====
        private readonly Dictionary<string, Func<UserControl>> _moduleFactories = new Dictionary<string, Func<UserControl>>(StringComparer.OrdinalIgnoreCase);

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
            : this(currentUser, new RolePermissionService())
        {
        }

        public MainForm(User currentUser, IPermissionService permissionService)
        {
            InitializeComponent();

            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

            InitializeSidebar();
            BuildModuleFactories();

            // initialize topbar/profile controls (labels, picture, click)
            InitializeTopBarProfile();

            LoadContentByName("Dashboard"); // default
        }

        // ===== 4: Initialization =====
        private void InitializeSidebar()
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
            BuildModules(modulesPanel);
            PnlSidebar.Controls.Add(modulesPanel);

        }

        // ===== TopBar / Profile / Role initialization =====
        private void InitializeTopBarProfile()
        {
            try
            {
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

                if (this.Controls.Find("LblProfileRole", true).FirstOrDefault() is Label roleLbl)
                    roleLbl.Text = _currentUser.Role.ToString();

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

                PicBxProfilePic.Cursor = Cursors.Hand;
                PicBxProfilePic.Click -= PicBxProfilePic_Click;
                PicBxProfilePic.Click += PicBxProfilePic_Click;

                PnlProfileHeader.Cursor = Cursors.Hand;
                PnlProfileHeader.Click -= PicBxProfilePic_Click;
                PnlProfileHeader.Click += PicBxProfilePic_Click;
                LblProfileName.Click -= PicBxProfilePic_Click;
                LblProfileName.Click += PicBxProfilePic_Click;
                if (this.Controls.Find("LblProfileRole", true).FirstOrDefault() is Label lblRole)
                {
                    lblRole.Click -= PicBxProfilePic_Click;
                    lblRole.Click += PicBxProfilePic_Click;
                }

                // small tooltip
                var tt = new ToolTip();
                tt.SetToolTip(PicBxProfilePic, "Open profile");
                tt.SetToolTip(PnlProfileHeader, "Open profile");
            }
            catch
            {
                // fail silently
            }
        }

        private Image TryGetUserImage()
        {
            // add Image or byte[] to User:
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

        private UserControl GetDashboardByRole()
        {
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

        // ===== helper to check permissions for modules (UI-driven) =====
        private bool IsModuleAllowed(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                return false;

            // normalize once
            var mod = moduleName.Trim();

            // map module names to permission checks
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

            // Dashboard and Profile are always available (UI chooses content based on permissions)
            if (string.Equals(mod, "Dashboard", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(mod, "Profile", StringComparison.OrdinalIgnoreCase))
                return true;

            // Default: allow module (UI will show fallback content if not implemented)
            return true;
        }

        // ===== 7: Sidebar construction logic =====
        private void BuildModules(Panel container)
        {
            foreach (var category in _sidebarLayout.Reverse())
            {
                var allowedModules = category.Value.Reverse()
                    .Where(module => IsModuleAllowed(module))
                    .ToList();

                if (!allowedModules.Any())
                    continue;

                var categoryPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true
                };

                foreach (var module in allowedModules)
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
            _moduleFactories["Profile"] = () => new UserControls.UCProfile(_currentUser, _permissionService);
            _moduleFactories["Dashboard"] = () => GetDashboardByRole();
            _moduleFactories["Catalog"] = () => new UserControls.UCCatalog();

            // Librarian only / staff-managed modules (UI will hide if not allowed)
            _moduleFactories["Users"] = () => new UserControls.Management.UCUsers();
            _moduleFactories["Reports"] = () => new UserControls.Insights.UCReports();
            _moduleFactories["Settings"] = () => new UserControls.Configurations.UCSettings();

            // LIBRARIAN AND STAFF
            _moduleFactories["Members"] = () => new UserControls.Management.UCMembers();
            _moduleFactories["Fines"] = () => new UserControls.Management.UCFines();
            _moduleFactories["Reservations"] = () => new UserControls.Management.UCReservation();
            _moduleFactories["Inventory"] = () => new UserControls.Management.UCInventory();
            _moduleFactories["Circulation"] = () => new UserControls.Management.UCCirculation();

            // members only
            _moduleFactories["My Borrowed"] = () => new UserControls.MemberFeatures.UCBorrowed();
            _moduleFactories["My Overdue"] = () => new UserControls.MemberFeatures.UCOverdue();
            _moduleFactories["My Reserve"] = () => new UserControls.MemberFeatures.UCReserve();
            _moduleFactories["My History"] = () => new UserControls.MemberFeatures.UCHistory();
            _moduleFactories["My Fines"] = () => new UserControls.MemberFeatures.UCMyFines();

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

            Label Lbl = null;

            Lbl = this.Controls.Find("LblModuleTitle", true).FirstOrDefault() as Label
               ?? this.Controls.Find("LblModuleName", true).FirstOrDefault() as Label
               ?? this.Controls.Find("lblModule", true).FirstOrDefault() as Label;

            if (Lbl != null)
            {
                Lbl.Text = display;
                return;
            }

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
