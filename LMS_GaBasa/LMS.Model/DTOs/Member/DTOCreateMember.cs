using System;

namespace LMS.Model.DTOs.Member
{
    public class DTOCreateMember
    {
        // Personal Details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string MemberTypeName { get; set; }

        // Login Details
        public string Username { get; set; }
        public string Password { get; set; }

        // Images (file paths)
        public string PhotoPath { get; set; }
        public string ValidIdPath { get; set; }
    }
}
