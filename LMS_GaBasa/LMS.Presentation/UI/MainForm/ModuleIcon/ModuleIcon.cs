using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.UI.MainForm.ModuleIcon
{
    internal static class ModuleIcons
    {
        // Key: module name (must match sidebar button names exactly)
        public static readonly Dictionary<string, ModuleIconSet> Icons =
                    new Dictionary<string, ModuleIconSet>(System.StringComparer.OrdinalIgnoreCase);

        public static void LoadIcons()
        {
            // modulename, filename (without extension)
            // ===== ALL ROLES =====
            AddAllRole("Profile", "Profile");
            AddAllRole("Dashboard", "Dashboard");
            AddAllRole("Catalog", "Catalog");
            AddAllRole("Logout", "LogOut");

            // Librarian Only
            AddLibrarianStaff("Users", "Users");
            AddLibrarianStaff("Settings", "Settings");

            // ===== LIBRARIAN & STAFF =====
            AddLibrarianStaff("Reports", "Reports");
            AddLibrarianStaff("Members", "Members");
            AddLibrarianStaff("Inventory", "Inventory");
            AddLibrarianStaff("Reservations", "Reservations");
            AddLibrarianStaff("Circulation", "Circulation");
            AddLibrarianStaff("Fines", "Fines");

            // ===== MEMBER ONLY =====
            AddMember("My Wishlist", "MyWishlist");
            AddMember("My Borrowed", "MyBorrowed");
            AddMember("My Overdue", "MyOverdue");
            AddMember("My Reserve", "MyReserve");
            AddMember("My Fines", "MyFines");
            AddMember("My History", "MyHistory");

            // Test icon loading (temporary and testing only)
            //MessageBox.Show(
            //    File.Exists("Assets/icons/ModuleIcons/AllRoleIcons/White/Dashboard.png")
            //        ? "ICON FOUND"
            //        : "ICON NOT FOUND"
            //);

        }

        // ---------- Helpers ----------
        private static void AddAllRole(string moduleName, string fileName)
        {
            Add(
                moduleName,
                $"Assets/icons/ModuleIcons/AllRoleIcons",
                fileName
            );
        }

        private static void AddLibrarianStaff(string moduleName, string fileName)
        {
            Add(
                moduleName,
                $"Assets/icons/ModuleIcons/LibrarianStaffIcons",
                fileName
            );
        }

        private static void AddMember(string moduleName, string fileName)
        {
            Add(
                moduleName,
                $"Assets/icons/ModuleIcons/MemberIcons",
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

        private static Image LoadImage(string relativePath)
        {
            var fullPath = Path.Combine(Application.StartupPath, relativePath.Replace("/", "\\"));
            if (File.Exists(fullPath))
                return Image.FromFile(fullPath);

            MessageBox.Show($"Icon not found: {fullPath}");
            return null;
        }

        // end code
    }
}
