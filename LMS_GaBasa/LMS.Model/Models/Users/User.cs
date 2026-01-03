using LMS.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Users

{
    public abstract class User
    {
        public int UserID { get; internal set; }
        public string Username { get; internal set; }
        protected string PasswordHash { get; private set; } // hashed
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Email { get; internal set; }
        public string ContactNumber { get; internal set; }
        public string PhotoPath { get; internal set; }
        public UserStatus Status { get; internal set; }
        public DateTime? LastLogin { get; internal set; }
        // Enum (Database): Active, Inactive

        public abstract Role Role { get; }
        // Enum (Db): Admin, Staff, Member
        // abstract

        // ========== FORMATTED ID =========
        /// <summary>
        /// Gets the role-based prefix for the formatted ID (e.g., "LIB", "STF").
        /// Override in subclasses to provide role-specific prefixes.
        /// </summary>
        protected abstract string RolePrefix { get; }

        /// <summary>
        /// Gets the formatted display ID (e.g., "LIB-0001", "STF-0002").
        /// Format: {RolePrefix}-{UserID padded to at least 4 digits}
        /// </summary>
        public string FormattedID
        {
            get
            {
                int minDigits = 4;
                int actualDigits = UserID.ToString().Length;
                int padLength = Math.Max(minDigits, actualDigits);
                return $"{RolePrefix}-{UserID.ToString().PadLeft(padLength, '0')}";
            }
        }

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

        // INTERNAL ACCESS for auth only
        internal string GetPasswordHash()
        {
            return PasswordHash;
        }

        // ========== COMMON BEHAVIOR ==========
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }

}
