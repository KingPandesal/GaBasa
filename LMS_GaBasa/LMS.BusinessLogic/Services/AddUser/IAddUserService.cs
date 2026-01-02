using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs.User;

namespace LMS.BusinessLogic.Services.AddUser
{
    public interface IAddUserService
    {
        UserCreationResult CreateUser(DTOCreateUser dto);

    }

    public class UserCreationResult
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string ErrorMessage { get; set; }

        public static UserCreationResult Ok(int userId) => new UserCreationResult { Success = true, UserId = userId };
        public static UserCreationResult Fail(string message) => new UserCreationResult { Success = false, ErrorMessage = message };
    }
}
