using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;

namespace LMS.Model.DTOs.User
{
    public class DTOEditUser
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string PhotoPath { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
    }
}
