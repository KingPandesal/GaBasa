using LMS.BusinessLogic.Security;
using LMS.Model.Models.Users;
using LMS.Presentation.UI.MainForm.ModuleIcon;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LMS.Presentation.UI.MainForm.TopBar
{
    internal class TopBarController : ITopBarController
    {
        public void InitializeProfile(User currentUser, IPermissionService permissionService,
            PictureBox profilePictureBox, Label profileNameLabel, Label profileRoleLabel, Control profileHeader,
            Action onOpenProfile)
        {
            if (profileNameLabel == null || profilePictureBox == null || profileHeader == null)
                return;

            try
            {
                string displayName = null;
                try { displayName = currentUser?.GetFullName(); } catch { /* ignore */ }
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    var first = currentUser?.FirstName ?? string.Empty;
                    var last = currentUser?.LastName ?? string.Empty;
                    displayName = (first + " " + last).Trim();
                }
                if (string.IsNullOrWhiteSpace(displayName))
                    displayName = currentUser?.Username ?? "User";

                profileNameLabel.Text = displayName;

                if (profileRoleLabel != null)
                    profileRoleLabel.Text = currentUser?.Role.ToString();

                // picture
                profilePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                var userImage = TryGetUserImage(currentUser);
                if (userImage != null)
                {
                    profilePictureBox.Image = userImage;
                }
                else
                {
                    profilePictureBox.Image = CreateProfilePlaceholderImage(displayName, profilePictureBox.Width, profilePictureBox.Height);
                }

                profilePictureBox.Cursor = Cursors.Hand;
                profileHeader.Cursor = Cursors.Hand;

                // wire click handlers to provided callback
                if (onOpenProfile != null)
                {
                    profilePictureBox.Click += (s, e) => onOpenProfile();
                    profileHeader.Click += (s, e) => onOpenProfile();
                    profileNameLabel.Click += (s, e) => onOpenProfile();
                    if (profileRoleLabel != null)
                        profileRoleLabel.Click += (s, e) => onOpenProfile();
                }

                var tt = new ToolTip();
                tt.SetToolTip(profilePictureBox, "Open profile");
                tt.SetToolTip(profileHeader, "Open profile");
            }
            catch
            {
                // swallow UI errors - keep topbar tolerant
            }
        }

        public void UpdateModuleTitle(string title, Control topBarContainer)
        {
            var display = string.IsNullOrWhiteSpace(title) ? string.Empty : title;

            Label Lbl = null;

            // search the given container (which can be the Form)
            try
            {
                if (topBarContainer != null)
                {
                    Lbl = topBarContainer.Controls.Find("LblModuleTitle", true).FirstOrDefault() as Label
                       ?? topBarContainer.Controls.Find("LblModuleName", true).FirstOrDefault() as Label
                       ?? topBarContainer.Controls.Find("lblModule", true).FirstOrDefault() as Label;
                }
            }
            catch
            {
                // fallback to scanning all (if argument was null or unexpected)
                Lbl = null;
            }

            if (Lbl != null)
            {
                Lbl.Text = display;
                return;
            }

            // try to place label inside a PnlTopBar when present
            Control topBar = null;
            try
            {
                if (topBarContainer != null)
                    topBar = topBarContainer.Controls.Find("PnlTopBar", true).FirstOrDefault() as Control;
            }
            catch { topBar = null; }

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

            // last resort: if topBarContainer is a Form, set its Text
            var form = topBarContainer as Form;
            if (form != null)
            {
                form.Text = display;
            }
        }

        public void UpdateModuleIcon(string moduleName, PictureBox moduleIconPictureBox)
        {
            if (moduleIconPictureBox == null) return;

            if (string.IsNullOrWhiteSpace(moduleName))
            {
                moduleIconPictureBox.Image = null;
                return;
            }

            if (ModuleIcons.Icons.TryGetValue(moduleName, out var img))
            {
                moduleIconPictureBox.Image = img;
                moduleIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                moduleIconPictureBox.Image = null; // fallback
            }
        }

        // Try to obtain an image for the user; here we keep behavior simple and non-failing.
        // If User had bytes (e.g., PhotoBytes), this could be extended.
        private Image TryGetUserImage(User user)
        {
            // Placeholder: no persisted photo in User model in current codebase.
            // If you add a byte[] PhotoBytes to User, uncomment example below:
            /*
            if (user?.PhotoBytes != null)
            {
                using (var ms = new MemoryStream(user.PhotoBytes))
                    return Image.FromStream(ms);
            }
            */
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
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);

                var letter = char.ToUpperInvariant(key[0]).ToString();
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
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

        // end code
    }
}
