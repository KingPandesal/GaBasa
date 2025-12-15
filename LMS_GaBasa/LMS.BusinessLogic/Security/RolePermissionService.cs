using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Security
{
    public class RolePermissionService : IPermissionService
    {
        public bool CanManageUsers(User user)
            => user.Role == Role.Librarian;

        public bool CanBorrowBooks(User user)
            => user.Role == Role.Member;
    }

}
