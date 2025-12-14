using LMS.DataAccess.Repositories;
using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepo;

        public UserManager()
        {
            _userRepo = new UserRepository();
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepo.GetByUsername(username);
            if (user == null || user.Status != "Active") return null;

            if (HashPassword(password) == user.Password)
                return user;

            return null;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }

    }

}
