using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs;

namespace LMS.BusinessLogic.Services
{
    public interface IUserProfileService
    {
        DTOUserProfile GetUserProfile(int userId);
        //DTOMemberProfile GetMemberProfile(int userId);
    }
}
