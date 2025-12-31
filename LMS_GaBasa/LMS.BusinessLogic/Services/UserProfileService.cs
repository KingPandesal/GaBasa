using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return new DTOUserProfile
            {
                UserID = user.UserID,
                Username = user.Username,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                PhotoPath = user.PhotoPath,
                Role = user.Role.ToString(),
                Status = user.Status.ToString()
            };
        }

        public bool UpdateUserProfile(DTOUpdateUserProfile profile)
        {
            if (profile == null || profile.UserID <= 0)
                return false;

            return _userRepo.UpdateProfile(
                profile.UserID,
                profile.FirstName,
                profile.LastName,
                profile.Email,
                profile.ContactNumber,
                profile.PhotoPath
            );
        }
    }
}
