using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Managers
{
    public interface IUserManager
    {
        User Authenticate(string username, string password);
    }

}
