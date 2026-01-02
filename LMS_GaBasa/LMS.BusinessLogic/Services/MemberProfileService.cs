using System;
using LMS.BusinessLogic.Helpers;
using LMS.BusinessLogic.Managers;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;

namespace LMS.BusinessLogic.Services
{
    public class MemberProfileService : IMemberProfileService
    {
        private readonly IMemberRepository _memberRepo;
        private readonly IPasswordHasher _passwordHasher;

        public MemberProfileService(IMemberRepository memberRepo) 
            : this(memberRepo, new LMS.BusinessLogic.Hashing.BcryptPasswordHasher(12))
        {
        }

        public MemberProfileService(IMemberRepository memberRepo, IPasswordHasher passwordHasher)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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

        public MemberProfileUpdateResult UpdateMemberProfile(DTOUpdateMemberProfile profile)
        {
            if (profile == null || profile.UserID <= 0)
                return MemberProfileUpdateResult.Fail("Invalid profile data.");

            // ===== USERNAME VALIDATION =====
            if (!string.IsNullOrEmpty(profile.Username))
            {
                if (profile.Username.Length < 8)
                    return MemberProfileUpdateResult.Fail("Username must be at least 8 characters.");

                // Check if username is taken by another user
                if (_memberRepo.UsernameExistsForOtherUser(profile.UserID, profile.Username))
                    return MemberProfileUpdateResult.Fail("Username already exists.");
            }

            // ===== HANDLE PASSWORD CHANGE =====
            if (!string.IsNullOrEmpty(profile.OldPassword) && !string.IsNullOrEmpty(profile.NewPassword))
            {
                // Verify old password
                string currentHash = _memberRepo.GetPasswordHash(profile.UserID);
                if (string.IsNullOrEmpty(currentHash))
                    return MemberProfileUpdateResult.Fail("Failed to verify password.");

                if (!_passwordHasher.Verify(currentHash, profile.OldPassword))
                    return MemberProfileUpdateResult.Fail("Old password is incorrect.");

                // Update password
                string newHash = _passwordHasher.Hash(profile.NewPassword);
                bool passwordUpdated = _memberRepo.UpdateMemberPassword(profile.UserID, newHash);
                if (!passwordUpdated)
                    return MemberProfileUpdateResult.Fail("Failed to update password.");
            }

            // ===== HANDLE PHOTO PATH =====
            string photoPathToStore = profile.PhotoPath;
            if (!string.IsNullOrEmpty(profile.PhotoPath) &&
                !UserImageHelper.IsRelativePath(profile.PhotoPath))
            {
                string relativePath = UserImageHelper.CopyImageToStorage(profile.PhotoPath, profile.UserID);
                if (relativePath != null)
                {
                    photoPathToStore = relativePath;
                }
            }

            // ===== HANDLE VALID ID PATH =====
            string validIdPathToStore = profile.ValidIdPath;
            if (!string.IsNullOrEmpty(profile.ValidIdPath) &&
                !UserImageHelper.IsRelativePath(profile.ValidIdPath))
            {
                string relativePath = CopyValidIdToStorage(profile.ValidIdPath, profile.UserID);
                if (relativePath != null)
                {
                    validIdPathToStore = relativePath;
                }
            }

            // ===== UPDATE PROFILE =====
            bool success = _memberRepo.UpdateMemberProfile(
                profile.UserID,
                profile.FirstName,
                profile.LastName,
                profile.Email,
                profile.ContactNumber,
                photoPathToStore,
                profile.Address,
                validIdPathToStore,
                profile.Username
            );

            return success 
                ? MemberProfileUpdateResult.Ok() 
                : MemberProfileUpdateResult.Fail("Failed to update profile.");
        }

        private string CopyValidIdToStorage(string sourcePath, int userId)
        {
            try
            {
                string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
                string destinationFolder = System.IO.Path.Combine(appBasePath, @"Assets\dataimages\ValidIDs");

                if (!System.IO.Directory.Exists(destinationFolder))
                {
                    System.IO.Directory.CreateDirectory(destinationFolder);
                }

                string extension = System.IO.Path.GetExtension(sourcePath);
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFileName = $"validid_{userId}_{timestamp}{extension}";

                string destinationPath = System.IO.Path.Combine(destinationFolder, newFileName);

                System.IO.File.Copy(sourcePath, destinationPath, overwrite: true);

                return System.IO.Path.Combine(@"Assets\dataimages\ValidIDs", newFileName);
            }
            catch
            {
                return null;
            }
        }
    }

    // Result class for better error handling
    public class MemberProfileUpdateResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static MemberProfileUpdateResult Ok() => new MemberProfileUpdateResult { Success = true };
        public static MemberProfileUpdateResult Fail(string message) => new MemberProfileUpdateResult { Success = false, ErrorMessage = message };
    }
}
