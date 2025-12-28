using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LMS.Presentation.UI.MainForm.Navigation;

namespace LMS.Presentation.UI.MainForm.Sidebar
{
    internal class SidebarBuilder : ISidebarBuilder
    {
        private readonly IModuleNavigator _navigator;
        private readonly Dictionary<string, Image> _moduleIcons = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

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
                Text = "Logout",
                Height = 40,
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 11),
                ForeColor = defaultFore,
                BackColor = defaultBack,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += (s, e) => onLogout();
            logoutBtn.Image = CreatePlaceholderIcon("Logout");
            logoutBtn.ImageAlign = ContentAlignment.MiddleLeft;
            logoutBtn.TextImageRelation = TextImageRelation.ImageBeforeText;

            // hover behavior for logout as well
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
                    var mod = module; // capture for lambda
                    var btn = new Button
                    {
                        Text = mod,
                        Height = 40,
                        Font = new Font("Microsoft Sans Serif", 11),
                        ForeColor = defaultFore,
                        BackColor = defaultBack,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Tag = mod,
                        Padding = new Padding(10, 0, 0, 0)
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

                    btn.Image = CreatePlaceholderIcon(mod);
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
                    }
                    else
                    {
                        b.BackColor = defaultBack;
                        b.ForeColor = defaultFore;
                    }
                }
            }

            // Apply initial selection if provided
            if (!string.IsNullOrWhiteSpace(initialSelectedModule))
            {
                SetSelected(initialSelectedModule);
            }
        }

        private Image CreatePlaceholderIcon(string key)
        {
            if (string.IsNullOrEmpty(key)) key = "?";

            if (_moduleIcons.TryGetValue(key, out var cached))
                return cached;

            const int size = 24;
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
