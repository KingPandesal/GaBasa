using LMS.Model.DTOs.Member;
using System.Collections.Generic;

namespace LMS.BusinessLogic.Services.FetchMembers
{
    public interface IFetchMemberService
    {
        List<DTOFetchAllMembers> GetAllMembers();
    }
}
