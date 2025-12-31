using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.BusinessLogic.Helpers;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;

namespace LMS.BusinessLogic.Services
{
    public class MemberProfileService : IMemberProfileService
    {
        private readonly IMemberRepository _memberRepo;

        public MemberProfileService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public MemberProfileDto GetMemberProfile(int userId)
        {
            var profile = _memberRepo.GetMemberProfileByUserId(userId);
            if (profile == null) return null;

            // Convert relative path to absolute for display
            profile.PhotoPath = UserImageHelper.GetAbsolutePath(profile.PhotoPath);
            profile.ValidIdPath = UserImageHelper.GetAbsolutePath(profile.ValidIdPath);

            return profile;
        }

        public bool UpdateMemberProfile(DTOUpdateMemberProfile profile)
        {
            if (profile == null || profile.UserID <= 0)
                return false;

            string photoPathToStore = profile.PhotoPath;

            // If a new image was selected (not already a relative path), copy it
            if (!string.IsNullOrEmpty(profile.PhotoPath) &&
                !UserImageHelper.IsRelativePath(profile.PhotoPath))
            {
                string relativePath = UserImageHelper.CopyImageToStorage(profile.PhotoPath, profile.UserID);
                if (relativePath != null)
                {
                    photoPathToStore = relativePath;
                }
            }

            return _memberRepo.UpdateMemberProfile(
                profile.UserID,
                profile.FirstName,
                profile.LastName,
                profile.Email,
                profile.ContactNumber,
                photoPathToStore,
                profile.Address
            );
        }
    }
}
