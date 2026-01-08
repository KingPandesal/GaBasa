using System;

namespace LMS.Model.DTOs.User
{
    public class DTOUpdateUserProfile
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }

        // Added for EditLibrarianStaffProfile usage
        public string Username { get; set; }

        // Optional password change fields (null when not changing)
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
