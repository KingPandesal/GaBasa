using LMS.DataAccess.Repositories;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
using System;

namespace LMS.BusinessLogic.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepo;
        private readonly IMemberManager _memberManager;
        private readonly IPasswordHasher _passwordHasher;

        public UserManager(IUserRepository userRepo, IPasswordHasher passwordHasher)
            : this(userRepo, null, passwordHasher)
        {
        }

        public UserManager(IUserRepository userRepo, IMemberManager memberManager, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _memberManager = memberManager;
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public AuthenticationResult Authenticate(string username, string password)
        {
            var user = _userRepo.GetByUsername(username);

            if (user == null)
                return AuthenticationResult.Fail(AuthFailureReason.InvalidCredentials);

            if (!_passwordHasher.Verify(user.GetPasswordHash(), password))
                return AuthenticationResult.Fail(AuthFailureReason.InvalidCredentials);

            // Check User table status (applies to all users)
            if (user.Status != UserStatus.Active)
                return AuthenticationResult.Fail(AuthFailureReason.AccountInactive);

            // Additional check for Members - check MemberStatus using MemberManager
            if (user.Role == Role.Member && _memberManager != null)
            {
                var memberFailure = _memberManager.ValidateMemberLoginStatus(user.UserID);
                if (memberFailure.HasValue)
                    return AuthenticationResult.Fail(memberFailure.Value);
            }

            // Update last login timestamp
            _userRepo.UpdateLastLogin(user.UserID);

            return AuthenticationResult.Ok(user);
        }
    }
}
