using LMS.Model.Models.Users;

namespace LMS.BusinessLogic.Managers
{
    public interface IUserManager
    {
        AuthenticationResult Authenticate(string username, string password);
    }

    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public User User { get; set; }
        public AuthFailureReason FailureReason { get; set; }

        public static AuthenticationResult Ok(User user) => new AuthenticationResult
        {
            Success = true,
            User = user
        };

        public static AuthenticationResult Fail(AuthFailureReason reason) => new AuthenticationResult
        {
            Success = false,
            FailureReason = reason
        };
    }

    public enum AuthFailureReason
    {
        None,
        InvalidCredentials,
        AccountInactive,
        MemberSuspended,
        MemberExpired
    }
}
