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
            // ===== ALL ROLES =====
            AddAllRole("Dashboard", "dashboard");
            AddAllRole("Catalog", "catalog");
            AddAllRole("Logout", "logout");

            // ===== LIBRARIAN & STAFF =====
            AddLibrarianStaff("Reports", "reports");
            AddLibrarianStaff("Users", "users");
            AddLibrarianStaff("Settings", "settings");
            AddLibrarianStaff("Members", "members");
            AddLibrarianStaff("Inventory", "inventory");
            AddLibrarianStaff("Reservations", "reservations");
            AddLibrarianStaff("Circulation", "circulation");
            AddLibrarianStaff("Fines", "fines");

            // ===== MEMBER ONLY =====
            AddMember("My Wishlist", "wishlist");
            AddMember("My Borrowed", "borrowed");
            AddMember("My Overdue", "overdue");
            AddMember("My Reserve", "reserve");
            AddMember("My Fines", "myfines");
            AddMember("My History", "history");
        }

        // ---------- Helpers ----------
        private static void AddAllRole(string moduleName, string fileName)
        {
            Add(
                moduleName,
                $"Resources/icons/ModuleIcons/AllRoleIcons",
                fileName
            );
        }

        private static void AddLibrarianStaff(string moduleName, string fileName)
        {
            Add(
                moduleName,
                $"Resources/icons/ModuleIcons/LibrarianStaffIcons",
                fileName
            );
        }

        private static void AddMember(string moduleName, string fileName)
        {
            Add(
                moduleName,
                $"Resources/icons/ModuleIcons/MemberIcons",
                fileName
            );
        }

        private static void Add(string moduleName, string basePath, string fileName)
        {
            Icons[moduleName] = new ModuleIconSet(
                LoadImage($"{basePath}/White/{fileName}.png"),
                LoadImage($"{basePath}/Red/{fileName}.png")
            );
        }

        private static Image LoadImage(string path)
        {
            return File.Exists(path) ? Image.FromFile(path) : null;
        }

        // end code
    }
}
