using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Repositories
{
    public interface IUserRepository
    {
        // Fetch a user by username
        User GetByUsername(string username);
    }
}
