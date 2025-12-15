using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Security
{
    public interface IPermissionService
    {
        bool CanManageUsers(User user);
        bool CanBorrowBooks(User user);
    }

}
