using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace LMS.Presentation.UI.MainForm.ModuleIcon
{
    internal static class ModuleIcons
    {
        // Key: module name (must match sidebar button names exactly)
        public static readonly Dictionary<string, ModuleIconSet> Icons =
                    new Dictionary<string, ModuleIconSet>(System.StringComparer.OrdinalIgnoreCase);

        public static void LoadIcons()
        {
            // All roles
            Add("Dashboard", "dashboard");
            Add("Catalog", "catalog");
            Add("Logout", "logout");

            // Librarian only
            Add("Reports", "reports");
            Add("Users", "users");
            Add("Settings", "settings");

            // Librarian and Staff
            Add("Members", "members");
            Add("Inventory", "inventory");
            Add("Reservations", "reservations");
            Add("Circulation", "circulation");
            Add("Fines", "fines");

            // Member only
            Add("My Wishlist", "wishlist");
            Add("My Borrowed", "borrowed");
            Add("My Overdue", "overdue");
            Add("My Reserve", "reserve");
            Add("My Fines", "myfines");
            Add("My History", "history");

        }

        private static void Add(string moduleName, string fileName)
        {
            Icons[moduleName] = new ModuleIconSet(
                LoadImage($"Resources/icons/ModuleIcons/White/{fileName}.png"),
                LoadImage($"Resources/icons/ModuleIcons/Red/{fileName}.png")
            );
        }

        private static Image LoadImage(string path)
        {
            return File.Exists(path) ? Image.FromFile(path) : null;
        }

        // end code
    }
}
