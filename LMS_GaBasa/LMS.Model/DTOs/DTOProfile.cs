using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs
{
    public class DTOProfile
    {
        // Common user info
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string UserStatus { get; set; } //account status

        // Member-specific info
        public string MemberType { get; set; }
        // is a string instead of the full object to simplify UI binding.
        public string Address { get; set; } //nullable
        public DateTime? RegistrationDate { get; set; } 
        // is nullable because non-members don’t have these fields
        public DateTime? ExpirationDate { get; set; }
        // is nullable because non-members don’t have these fields
        public string Photo { get; set; } //nullable
        public string ValidID { get; set; } //nullable
        public string MemberStatus { get; set; } // membership status

        // note: ok ra string ang mga enums for UI display purposes
        // end code
    }
}
