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
            // Auto-expire members whose expiration date has passed
            _memberRepo.UpdateExpiredMembers();

            return _memberRepo.GetAllMembers();
        }
    }
}
