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
        public static readonly Dictionary<string, Image> Icons = new Dictionary<string, Image>(System.StringComparer.OrdinalIgnoreCase);

        public static void LoadIcons()
        {
            // All roles
            Icons["Dashboard"] = LoadImage("Resources/icons/ModuleIcons/Red/Dashboard.png");
            Icons["Catalog"] = LoadImage("Resources/icons/ModuleIcons/Red/catalog.png");
            Icons["Logout"] = LoadImage("Resources/icons/ModuleIcons/Red/logout.png");

            // Librarian only
            Icons["Reports"] = LoadImage("Resources/icons/ModuleIcons/Red/settings.png");
            Icons["Users"] = LoadImage("Resources/icons/ModuleIcons/Red/users.png");
            Icons["Settings"] = LoadImage("Resources/icons/ModuleIcons/Red/settings.png");

            // Librarian and Staff
            Icons["Members"] = LoadImage("Resources/icons/ModuleIcons/Red/members.png");
            Icons["Inventory"] = LoadImage("Resources/icons/ModuleIcons/Red/inventory.png");
            Icons["Reservations"] = LoadImage("Resources/icons/ModuleIcons/Red/reservations.png");
            Icons["Circulation"] = LoadImage("Resources/icons/ModuleIcons/Red/circulation.png");
            Icons["Fines"] = LoadImage("Resources/icons/ModuleIcons/Red/fines.png");

            // Member only
            Icons["My Wishlist"] = LoadImage("Resources/icons/ModuleIcons/Red/wishlist.png");
            Icons["My Borrowed"] = LoadImage("Resources/icons/ModuleIcons/Red/borrowed.png");
            Icons["My Overdue"] = LoadImage("Resources/icons/ModuleIcons/Red/overdue.png");
            Icons["My Reserve"] = LoadImage("Resources/icons/ModuleIcons/Red/reserve.png");
            Icons["My Fines"] = LoadImage("Resources/icons/ModuleIcons/Red/myfines.png");
            Icons["My History"] = LoadImage("Resources/icons/ModuleIcons/Red/history.png");
        }

        private static Image LoadImage(string path)
        {
            return File.Exists(path) ? Image.FromFile(path) : null;
        }
    }

}
