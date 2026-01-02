using LMS.Model.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.EditUser
{
    public interface IEditUserService
    {
        DTOEditUser GetUserForEdit(int userId);
        UserEditResult UpdateUser(DTOEditUser dto);
    }

    public class UserEditResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static UserEditResult Ok() => new UserEditResult { Success = true };
        public static UserEditResult Fail(string message) => new UserEditResult { Success = false, ErrorMessage = message };
    }
}
