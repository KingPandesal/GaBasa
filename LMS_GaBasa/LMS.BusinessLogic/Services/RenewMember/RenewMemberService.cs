using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;
using System;

namespace LMS.BusinessLogic.Services.RenewMember
{
    public class RenewMemberService : IRenewMemberService
    {
        private readonly IMemberRepository _memberRepo;

        public RenewMemberService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
        }

        public DTORenewMemberResult RenewMembership(int memberId)
        {
            if (memberId <= 0)
                return DTORenewMemberResult.Failure("Invalid member ID.");

            try
            {
                bool success = _memberRepo.RenewMembership(memberId);

                if (success)
                    return DTORenewMemberResult.SuccessResult();
                else
                    return DTORenewMemberResult.Failure("Failed to renew membership.");
            }
            catch (Exception ex)
            {
                return DTORenewMemberResult.Failure($"Error renewing membership: {ex.Message}");
            }
        }
    }
}
