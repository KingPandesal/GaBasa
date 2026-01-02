using LMS.Model.DTOs.Member;

namespace LMS.BusinessLogic.Services.RenewMember
{
    public interface IRenewMemberService
    {
        DTORenewMemberResult RenewMembership(int memberId);
    }
}