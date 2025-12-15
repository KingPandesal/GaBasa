using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Users

{
    public abstract class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        protected string PasswordHash { get; private set; } // hashed
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; }
        // Enum (Database): Active, Inactive
        // Maybe daw magbuhat na pud ug enum jud for status

        public abstract Role Role { get; }
        // Enum (Db): Admin, Staff, Member
        // abstract

        // ========== PERMISSIONS ==========
        // obsolete -ken:>
        //public abstract bool CanManageUsers();
        //public abstract bool CanManageCatalog();
        //public abstract bool CanCirculateBooks();
        //public abstract bool CanViewReports();

        // ========== PASSWORD HANDLING ==========
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public bool VerifyPasswordHash(string hashedInput)
        {
            return PasswordHash == hashedInput;
        }

        // ========== COMMON BEHAVIOR ==========
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }

}
