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

            // Check and update expiration before returning status
            CheckAndExpireMember(userId);

            return _memberRepo.GetMemberStatusByUserId(userId);
        }

        public AuthFailureReason? ValidateMemberLoginStatus(int userId)
        {
            var status = GetMemberStatus(userId);

            if (!status.HasValue)
                return null;

            switch (status.Value)
            {
                case MemberStatus.Active:
                    return null;

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

        private void CheckAndExpireMember(int userId)
        {
            var expirationDate = _memberRepo.GetExpirationDateByUserId(userId);
            var currentStatus = _memberRepo.GetMemberStatusByUserId(userId);

            // Only expire if currently Active and past expiration date
            if (currentStatus == MemberStatus.Active &&
                expirationDate.HasValue &&
                expirationDate.Value.Date < DateTime.Now.Date)
            {
                _memberRepo.UpdateMemberStatusByUserId(userId, MemberStatus.Expired);
            }
        }
    }
}
