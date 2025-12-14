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
        public string Password { get; set; } // hashed
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; }
        // Enum: Active, Inactive

        public abstract string Role { get; }
        // Enum: Admin, Staff, Member
        // abstract
        // dli ma-set na property -ken:>
    }

}
