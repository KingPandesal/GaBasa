using System;
using System.Text.RegularExpressions;
using System.Linq;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.Model.DTOs.Circulation;
using LMS.DataAccess.Interfaces;

namespace LMS.BusinessLogic.Managers.Circulation
{
    /// <summary>
    /// Manager for handling circulation operations.
    /// </summary>
    public class CirculationManager : ICirculationManager
    {
        private readonly ICirculationRepository _circulationRepo;
        private static readonly Regex MemberIdPattern = new Regex(@"^MEM-(\d+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public CirculationManager(ICirculationRepository circulationRepo)
        {
            _circulationRepo = circulationRepo ?? throw new ArgumentNullException(nameof(circulationRepo));
        }

        public DTOCirculationMemberInfo GetMemberByFormattedId(string formattedMemberId)
        {
            if (string.IsNullOrWhiteSpace(formattedMemberId))
                return null;

            int? memberId = ParseFormattedMemberId(formattedMemberId);
            if (!memberId.HasValue)
                return null;

            var memberInfo = _circulationRepo.GetMemberInfoByMemberId(memberId.Value);
            if (memberInfo == null)
                return null;

            // Populate borrowing statistics
            memberInfo.CurrentBorrowedCount = _circulationRepo.GetCurrentBorrowedCount(memberId.Value);
            memberInfo.OverdueCount = _circulationRepo.GetOverdueCount(memberId.Value);
            memberInfo.TotalUnpaidFines = _circulationRepo.GetTotalUnpaidFines(memberId.Value);

            return memberInfo;
        }

        public int? ParseFormattedMemberId(string formattedMemberId)
        {
            if (string.IsNullOrWhiteSpace(formattedMemberId))
                return null;

            var match = MemberIdPattern.Match(formattedMemberId.Trim());
            if (!match.Success)
                return null;

            if (int.TryParse(match.Groups[1].Value, out int memberId) && memberId > 0)
                return memberId;

            return null;
        }

        public DTOCirculationBookInfo GetBookByAccession(string accessionNumber)
        {
            if (string.IsNullOrWhiteSpace(accessionNumber))
                return null;

            var book = _circulationRepo.GetBookInfoByAccession(accessionNumber.Trim());
            if (book == null)
                return null;

            // Ensure Authors contains only a clean comma-separated list or "N/A"
            // Repository returns authors filtered by Role='Author' where possible,
            // but normalize here to be safe.
            if (string.IsNullOrWhiteSpace(book.Authors))
            {
                book.Authors = "N/A";
            }
            else
            {
                var parts = book.Authors
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                book.Authors = parts.Length > 0 ? string.Join(", ", parts) : "N/A";
            }

            return book;
        }

        public int CreateBorrowingTransaction(int copyId, int memberId, DateTime borrowDate, DateTime dueDate)
        {
            if (copyId <= 0 || memberId <= 0) return 0;
            return _circulationRepo.CreateBorrowingTransaction(copyId, memberId, borrowDate, dueDate);
        }

        public DTOReturnInfo GetActiveBorrowingByAccession(string accessionNumber)
        {
            if (string.IsNullOrWhiteSpace(accessionNumber)) return null;
            return _circulationRepo.GetActiveBorrowingByAccession(accessionNumber.Trim());
        }
    }
}