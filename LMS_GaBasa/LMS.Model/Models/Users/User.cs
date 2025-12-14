using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Users

{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // hashed
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        // Enum: Admin, Staff, Member
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; } 
        // Enum: Active, Inactive
    }

}
