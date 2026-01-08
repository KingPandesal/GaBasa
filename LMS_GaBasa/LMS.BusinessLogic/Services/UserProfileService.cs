using LMS.BusinessLogic.Helpers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.BusinessLogic.Hashing;

namespace LMS.BusinessLogic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        // Preserve existing usage by providing a convenience ctor that creates a default hasher.
        public UserProfileService(IUserRepository userRepo)
            : this(userRepo, new BcryptPasswordHasher(12))
        {
        }

        public UserProfileService(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
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
                Status = user.Status.ToString(),
                FormattedID = UserIdFormatter.Format(user.UserID, user.Role.ToString())
            };
        }

        public bool UpdateUserProfile(DTOUpdateUserProfile profile)
        {
            if (profile == null || profile.UserID <= 0)
                return false;

            // Fetch current user to compare username and for password hash
            var currentUser = _userRepo.GetById(profile.UserID);
            if (currentUser == null)
                return false;

            // If username changed, ensure it's not already taken
            if (!string.IsNullOrWhiteSpace(profile.Username))
            {
                var newUsername = profile.Username.Trim();
                if (!string.Equals(newUsername, currentUser.Username, System.StringComparison.OrdinalIgnoreCase)
                    && _userRepo.UsernameExists(newUsername))
                {
                    // username already taken
                    return false;
                }
            }

            // ===== HANDLE PASSWORD CHANGE =====
            if (!string.IsNullOrEmpty(profile.OldPassword) && !string.IsNullOrEmpty(profile.NewPassword))
            {
                // Verify old password
                string currentHash = _userRepo.GetPasswordHash(profile.UserID);
                if (string.IsNullOrEmpty(currentHash))
                    return false;

                if (!_passwordHasher.Verify(currentHash, profile.OldPassword))
                    return false;

                // Hash and update new password
                string newHash = _passwordHasher.Hash(profile.NewPassword);
                bool passwordUpdated = _userRepo.UpdatePassword(profile.UserID, newHash);
                if (!passwordUpdated)
                    return false;
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

            // ===== UPDATE PROFILE FIELDS (including username) =====
            return _userRepo.UpdateProfile(
                profile.UserID,
                profile.FirstName,
                profile.LastName,
                profile.Email,
                profile.ContactNumber,
                photoPathToStore,
                profile.Username?.Trim()
            );
        }
    }
}