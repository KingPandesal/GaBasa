using LMS.Presentation.UI.MainForm.ModuleIcon;
using LMS.Presentation.UI.MainForm.Navigation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LMS.Presentation.UI.MainForm.Sidebar
{
    internal class SidebarBuilder : ISidebarBuilder
    {
        private readonly IModuleNavigator _navigator;
        private readonly Dictionary<string, Image> _moduleIcons = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        // cache for scaled (zoomed) variants: key format "module|useRed|size"
        private readonly Dictionary<string, Image> _scaledIconCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        // Fixed icon edge size (px). Change this value to make icons larger/smaller.
        private const int IconEdge = 26;

        public SidebarBuilder(IModuleNavigator navigator)
        {
            _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
        }

        public void BuildSidebar(Panel container, IReadOnlyDictionary<string, string[]> sidebarLayout, Action<string> onModuleSelected, Action onLogout, string initialSelectedModule)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (sidebarLayout == null) throw new ArgumentNullException(nameof(sidebarLayout));
            if (onModuleSelected == null) throw new ArgumentNullException(nameof(onModuleSelected));
            if (onLogout == null) throw new ArgumentNullException(nameof(onLogout));

            container.Controls.Clear();

            // color definitions for selected / default / hover states
            var selectedBack = Color.White;
            var selectedFore = Color.FromArgb(161, 0, 11);
            var defaultBack = Color.Transparent;
            var defaultFore = Color.White;
            var hoverBack = Color.FromArgb(134, 33, 42); // hover background requested
            var hoverFore = Color.White;                // ensure contrast on hover

            // store module buttons so we can update selection styling
            var moduleButtons = new Dictionary<string, Button>(StringComparer.OrdinalIgnoreCase);
            string currentSelected = null;

            // Logout button at bottom
            var logoutBtn = new Button
            {
                Text = "  Logout",
                Height = 40,
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 11),
                ForeColor = defaultFore,
                BackColor = defaultBack,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += (s, e) => onLogout();

            // use fixed icon size
            var logoutIconSize = IconEdge;
            logoutBtn.Image = GetScaledIconImage("Logout", useRed: false, logoutIconSize);
            logoutBtn.ImageAlign = ContentAlignment.MiddleLeft;
            logoutBtn.TextImageRelation = TextImageRelation.ImageBeforeText;

            // hover behavior for logout
            logoutBtn.MouseEnter += (s, e) =>
            {
                logoutBtn.BackColor = hoverBack;
                logoutBtn.ForeColor = hoverFore;
            };
            logoutBtn.MouseLeave += (s, e) =>
            {
                logoutBtn.BackColor = defaultBack;
                logoutBtn.ForeColor = defaultFore;
            };

            container.Controls.Add(logoutBtn);

            // Modules panel (fill)
            var modulesPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            foreach (var category in sidebarLayout.Reverse())
            {
                var allowedModules = category.Value.Reverse()
                    .Where(m => _navigator.IsModuleAllowed(m))
                    .ToList();

                if (!allowedModules.Any())
                    continue;

                var categoryPanel = new Panel { Dock = DockStyle.Top, AutoSize = true };

                foreach (var module in allowedModules)
                {
                    var mod = module;
                    var btn = new Button
                    {
                        Text = "  " + mod,
                        Height = 40,
                        Font = new Font("Microsoft Sans Serif", 11),
                        ForeColor = defaultFore,
                        BackColor = defaultBack,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Tag = mod,
                        Padding = new Padding(15, 0, 0, 0)
                    };
                    btn.FlatAppearance.BorderSize = 0;

                    // add to dictionary for later state updates
                    moduleButtons[mod] = btn;

                    // click handler: update selection visual then invoke callback
                    btn.Click += (s, e) =>
                    {
                        SetSelected(mod);
                        onModuleSelected(mod);
                    };

                    // hover handlers: apply hover style only when the button is not the currently selected one
                    btn.MouseEnter += (s, e) =>
                    {
                        if (!string.Equals(currentSelected, mod, StringComparison.OrdinalIgnoreCase))
                        {
                            btn.BackColor = hoverBack;
                            btn.ForeColor = hoverFore;
                        }
                    };
                    btn.MouseLeave += (s, e) =>
                    {
                        if (!string.Equals(currentSelected, mod, StringComparison.OrdinalIgnoreCase))
                        {
                            btn.BackColor = defaultBack;
                            btn.ForeColor = defaultFore;
                        }
                    };

                    // use fixed icon size
                    var iconSize = IconEdge;
                    btn.Image = GetScaledIconImage(mod, useRed: false, iconSize);
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;

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
                modulesPanel.Controls.Add(categoryPanel);
            }

            container.Controls.Add(modulesPanel);

            // Helper to set selected visual state
            void SetSelected(string moduleName)
            {
                if (string.IsNullOrWhiteSpace(moduleName))
                    moduleName = null;

                currentSelected = moduleName;

                foreach (var kv in moduleButtons)
                {
                    var key = kv.Key;
                    var b = kv.Value;

                    if (string.Equals(key, moduleName, StringComparison.OrdinalIgnoreCase))
                    {
                        b.BackColor = selectedBack;
                        b.ForeColor = selectedFore;
                        // selected button sits on white -> use red icon (scaled)
                        var selectedIconSize = IconEdge;
                        b.Image = GetScaledIconImage(key, useRed: true, selectedIconSize);
                    }
                    else
                    {
                        b.BackColor = defaultBack;
                        b.ForeColor = defaultFore;
                        // non-selected button sits on colored sidebar -> use white icon (scaled)
                        var normalIconSize = IconEdge;
                        b.Image = GetScaledIconImage(key, useRed: false, normalIconSize);
                    }
                }
            }

            // Apply initial selection if provided
            if (!string.IsNullOrWhiteSpace(initialSelectedModule))
            {
                SetSelected(initialSelectedModule);
            }
        }

        private Image GetModuleIconImage(string key, bool useRed)
        {
            if (string.IsNullOrEmpty(key)) return null;

            if (ModuleIcons.Icons.TryGetValue(key, out var set))
            {
                if (useRed)
                    return set.Red ?? set.White;
                return set.White ?? set.Red;
            }

            // fallback: create a placeholder (single variant used for both states)
            return CreatePlaceholderIcon(key);
        }

        // Returns a scaled copy of the icon preserving aspect ratio (zoom). Results are cached.
        private Image GetScaledIconImage(string key, bool useRed, int targetMaxEdge)
        {
            if (string.IsNullOrEmpty(key)) return null;
            var cacheKey = $"{key}|{useRed}|{targetMaxEdge}";
            if (_scaledIconCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var src = GetModuleIconImage(key, useRed);
            if (src == null)
                return null;

            var scaled = ScaleImagePreserveAspect(src, targetMaxEdge, targetMaxEdge);
            _scaledIconCache[cacheKey] = scaled;
            return scaled;
        }

        // High-quality scaling while preserving aspect ratio (acts like PictureBoxSizeMode.Zoom)
        private static Image ScaleImagePreserveAspect(Image src, int maxWidth, int maxHeight)
        {
            if (src == null) return null;
            // if source already fits, return a clone to avoid accidental shared-dispose issues
            if (src.Width <= maxWidth && src.Height <= maxHeight)
                return new Bitmap(src);

            double ratioX = (double)maxWidth / src.Width;
            double ratioY = (double)maxHeight / src.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)Math.Round(src.Width * ratio);
            int newHeight = (int)Math.Round(src.Height * ratio);

            var bmp = new Bitmap(newWidth, newHeight);
            bmp.SetResolution(src.HorizontalResolution, src.VerticalResolution);

            using (var g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(src, 0, 0, newWidth, newHeight);
            }

            return bmp;
        }

        private Image CreatePlaceholderIcon(string key)
        {
            if (string.IsNullOrEmpty(key)) key = "?";

            if (_moduleIcons.TryGetValue(key, out var cached))
                return cached;

            const int size = IconEdge;
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
                using (var fnt = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(letter, fnt, brush, new RectangleF(0, 0, size, size), sf);
                }
            }

            _moduleIcons[key] = bmp;
            return bmp;
        }
    }
}
