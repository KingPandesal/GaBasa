using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Enums;
using System;

namespace LMS.BusinessLogic.Managers
{
    public class MemberManager : IMemberManager
    {
        private readonly IMemberRepository _memberRepo;

        public MemberManager(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
        }

        public MemberStatus? GetMemberStatus(int userId)
        {
            if (userId <= 0)
                return null;

            return _memberRepo.GetMemberStatusByUserId(userId);
        }

        public AuthFailureReason? ValidateMemberLoginStatus(int userId)
        {
            var status = GetMemberStatus(userId);

            if (!status.HasValue)
                return null; // Not a member or member record not found

            switch (status.Value)
            {
                case MemberStatus.Active:
                    return null; // No issue, can login

                case MemberStatus.Inactive:
                    return AuthFailureReason.AccountInactive;

                case MemberStatus.Suspended:
                    return AuthFailureReason.MemberSuspended;

                case MemberStatus.Expired:
                    return AuthFailureReason.MemberExpired;

                default:
                    return AuthFailureReason.AccountInactive;
            }
        }
    }
}
