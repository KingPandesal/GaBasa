using LMS.Model.DTOs.User;

namespace LMS.Model.DTOs.Member
{
    public class DTOUpdateMemberProfile : DTOUpdateUserProfile
    {
        public string Address { get; set; }
        public string ValidIdPath { get; set; }

        // Username, OldPassword and NewPassword are declared in DTOUpdateUserProfile.
        // Do not redeclare them here to avoid hiding the base members and compiler warnings.
    }
}
