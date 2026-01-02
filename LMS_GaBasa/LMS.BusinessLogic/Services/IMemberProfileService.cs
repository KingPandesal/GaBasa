using LMS.Model.DTOs.Member;

namespace LMS.BusinessLogic.Services
{
    public interface IMemberProfileService
    {
        MemberProfileDto GetMemberProfile(int userId);
        MemberProfileUpdateResult UpdateMemberProfile(DTOUpdateMemberProfile profile);
    }
}
