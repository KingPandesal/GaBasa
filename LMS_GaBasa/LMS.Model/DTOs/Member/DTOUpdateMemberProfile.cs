using LMS.Model.DTOs.User;

namespace LMS.Model.DTOs.Member
{
    public class DTOUpdateMemberProfile : DTOUpdateUserProfile
    {
        public string Address { get; set; }
        public string ValidIdPath { get; set; }
        public string Username { get; set; }
        
        // For password change (optional)
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
