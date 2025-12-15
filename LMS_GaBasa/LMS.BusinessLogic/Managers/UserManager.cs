using LMS.DataAccess.Repositories;
using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        // Inject both repository and hasher
        public UserManager(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepo.GetByUsername(username);

            if (user == null)
                return null;

            if (user.Status != "Active")
                return null;

            // Hash input with injected hasher then verify against stored hash
            string hashedInput = _passwordHasher.Hash(password);

            if (!user.VerifyPasswordHash(hashedInput))
                return null;

            return user;
        }

        // end code
    }
}
