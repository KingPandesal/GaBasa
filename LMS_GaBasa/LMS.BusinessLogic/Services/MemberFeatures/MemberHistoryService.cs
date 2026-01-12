using System;
using System.Collections.Generic;
using System.Linq;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.MemberFeatures.History;

namespace LMS.BusinessLogic.Services.MemberFeatures
{
    /// <summary>
    /// Service composing member history from repositories.
    /// Keeps UI free of data access and supports unit testing via seam.
    /// </summary>
    public class MemberHistoryService
    {
        private readonly IMemberHistoryRepository _repo;

        public MemberHistoryService() : this(new MemberHistoryRepository()) { }

        // Prefer depending on the interface to follow SOLID (easy to mock/test)
        public MemberHistoryService(IMemberHistoryRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Returns unified, sorted (desc) history items for the given member.
        /// </summary>
        public List<DTOHistoryItem> GetMemberHistory(int memberId)
        {
            if (memberId <= 0) return new List<DTOHistoryItem>();

            var items = new List<DTOHistoryItem>();

            try { items.AddRange(_repo.GetBorrowingHistory(memberId)); } catch { /* swallow DB errors; UI will show empty */ }
            try { items.AddRange(_repo.GetReservationHistory(memberId)); } catch { }
            try { items.AddRange(_repo.GetFinePaymentHistory(memberId)); } catch { }

            // Normalize nulls
            items = items.Where(i => i != null).ToList();

            // Sort descending by date (most recent first)
            items.Sort((a, b) => b.TransactionDate.CompareTo(a.TransactionDate));

            return items;
        }
    }
}
