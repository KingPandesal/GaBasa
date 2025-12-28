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

        public void BuildSidebar(Panel container, IReadOnlyDictionary<string, string[]> sidebarLayout, Action<string> onModuleSelected, Action onLogout)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (sidebarLayout == null) throw new ArgumentNullException(nameof(sidebarLayout));
            if (onModuleSelected == null) throw new ArgumentNullException(nameof(onModuleSelected));
            if (onLogout == null) throw new ArgumentNullException(nameof(onLogout));

            container.Controls.Clear();

            // Logout button at bottom
            var logoutBtn = new Button
            {
                Text = "Logout",
                Height = 40,
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 11),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            logoutBtn.FlatAppearance.BorderSize = 0;
            logoutBtn.Click += (s, e) => onLogout();
            logoutBtn.Image = CreatePlaceholderIcon("Logout");
            logoutBtn.ImageAlign = ContentAlignment.MiddleLeft;
            logoutBtn.TextImageRelation = TextImageRelation.ImageBeforeText;

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
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Tag = mod,
                        Padding = new Padding(10, 0, 0, 0)
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += (s, e) => onModuleSelected(mod);

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
