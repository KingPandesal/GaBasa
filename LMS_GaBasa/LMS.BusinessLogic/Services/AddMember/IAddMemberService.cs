using LMS.Model.DTOs.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.AddMember
{
    public interface IAddMemberService
    {
        DTOCreateMemberResult CreateMember(DTOCreateMember dto);

    }
}
