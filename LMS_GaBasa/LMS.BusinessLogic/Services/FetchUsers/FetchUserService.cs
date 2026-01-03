using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
using LMS.BusinessLogic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.FetchUsers
{
    public class FetchUserService : IFetchUserService
    {
        private readonly IUserRepository _userRepo;

        public FetchUserService(IUserRepository userRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public List<DTOFetchAllUsers> GetAllStaffUsers()
        {
            var users = _userRepo.GetAllStaffUsers();
            var result = new List<DTOFetchAllUsers>();

            foreach (var user in users)
            {
                result.Add(new DTOFetchAllUsers
                {
                    UserID = user.UserID,
                    FormattedID = UserIdFormatter.Format(user.UserID, user.Role.ToString()),
                    FullName = user.GetFullName(),
                    Role = user.Role.ToString(),
                    Username = user.Username,
                    Email = user.Email ?? "",
                    ContactNumber = user.ContactNumber ?? "",
                    Status = user.Status.ToString(),
                    LastLogin = user.LastLogin.HasValue 
                        ? user.LastLogin.Value.ToString("MMM dd, yyyy hh:mm tt") 
                        : "Never"
                });
            }

            return result;
        }
    }
}
