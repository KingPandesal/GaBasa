using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs
{
    public class DTOUserProfile
    {
        // Common user info
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string PhotoPath { get; set; }
        public string Status { get; set; } //account status

        // note: ok ra string ang mga enums for UI display purposes
        // end code
    }
}
