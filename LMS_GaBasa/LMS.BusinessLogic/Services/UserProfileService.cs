using LMS.BusinessLogic.Helpers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;

namespace LMS.BusinessLogic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepo;

        public UserProfileService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public DTOUserProfile GetUserProfile(int userId)
        {
            var user = _userRepo.GetById(userId);
            if (user == null) return null;

            // Convert relative path to absolute for display
            string absolutePhotoPath = UserImageHelper.GetAbsolutePath(user.PhotoPath);

            return new DTOUserProfile
            {
                UserID = user.UserID,
                Username = user.Username,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                PhotoPath = absolutePhotoPath, // Return absolute path for UI
                Role = user.Role.ToString(),
                Status = user.Status.ToString()
            };
        }

        public bool UpdateUserProfile(DTOUpdateUserProfile profile)
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

            return _userRepo.UpdateProfile(
                profile.UserID,
                profile.FirstName,
                profile.LastName,
                profile.Email,
                profile.ContactNumber,
                photoPathToStore // Store relative path in DB
            );
        }
    }
}