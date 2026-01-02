using LMS.DataAccess.Repositories;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
using System;

namespace LMS.BusinessLogic.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public UserManager(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public AuthenticationResult Authenticate(string username, string password)
        {
            var user = _userRepo.GetByUsername(username);

            if (user == null)
                return AuthenticationResult.Fail(AuthFailureReason.InvalidCredentials);

            if (!_passwordHasher.Verify(user.GetPasswordHash(), password))
                return AuthenticationResult.Fail(AuthFailureReason.InvalidCredentials);

            if (user.Status != UserStatus.Active)
                return AuthenticationResult.Fail(AuthFailureReason.AccountInactive);

            return AuthenticationResult.Ok(user);
        }
    }
}
