using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;
using System;
using System.Collections.Generic;

namespace LMS.BusinessLogic.Services.FetchMembers
{
    public class FetchMemberService : IFetchMemberService
    {
        private readonly IMemberRepository _memberRepo;

        public FetchMemberService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
        }

        public List<DTOFetchAllMembers> GetAllMembers()
        {
            return _memberRepo.GetAllMembers();
        }
    }
}
