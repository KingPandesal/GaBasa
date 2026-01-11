using LMS.BusinessLogic.Helpers;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
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

            // Get base member list from repository
            var members = _memberRepo.GetAllMembers() ?? new List<DTOFetchAllMembers>();

            // Use circulation repo to fetch per-member dynamic values (overdues, unpaid fines)
            var circulationRepo = new CirculationRepository();

            // Apply formatted ID and populate dynamic fields
            foreach (var member in members)
            {
                try
                {
                    member.FormattedID = UserIdFormatter.FormatMemberId(member.MemberID);

                    // Populate dynamic values used for penalty/effective calculations in the UI
                    member.OverdueCount = circulationRepo.GetOverdueCount(member.MemberID);
                    member.TotalUnpaidFines = circulationRepo.GetTotalUnpaidFines(member.MemberID);
                }
                catch
                {
                    // Ignore errors for individual members to keep list loading robust
                    member.OverdueCount = member.OverdueCount; // no-op; explicit to show intent
                    member.TotalUnpaidFines = member.TotalUnpaidFines;
                }
            }

            return members;
        }
    }
}
