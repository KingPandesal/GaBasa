using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs.User;

namespace LMS.BusinessLogic.Services.FetchUsers
{
    public interface IFetchUserService
    {
        List<DTOFetchAllUsers> GetAllStaffUsers();

    }
}
