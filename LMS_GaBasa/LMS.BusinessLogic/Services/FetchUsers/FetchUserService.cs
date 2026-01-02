using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
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
                    FullName = user.GetFullName(),
                    Role = MapRoleToDisplayName(user.Role),
                    Username = user.Username,
                    Email = user.Email ?? "",
                    ContactNumber = user.ContactNumber ?? "",
                    Status = user.Status.ToString()
                });
            }

            return result;
        }

        private string MapRoleToDisplayName(Role role)
        {
            switch (role)
            {
                case Role.Librarian:
                    return "Librarian / Admin";
                case Role.Staff:
                    return "Library Staff";
                default:
                    return role.ToString();
            }
        }
    }
}
