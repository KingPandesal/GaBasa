using LMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.ArchiveUser
{
    public class ArchiveUserService : IArchiveUserService
    {
        private readonly IUserRepository _userRepo;

        public ArchiveUserService(IUserRepository userRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public ArchiveUserResult Archive(int userId)
        {
            if (userId <= 0)
                return ArchiveUserResult.Fail("Invalid user ID.");

            // Check if user exists
            var user = _userRepo.GetById(userId);
            if (user == null)
                return ArchiveUserResult.Fail("User not found.");

            // Archive the user
            bool success = _userRepo.ArchiveUser(userId);

            return success
                ? ArchiveUserResult.Ok()
                : ArchiveUserResult.Fail("Failed to archive user.");
        }
    }
}
