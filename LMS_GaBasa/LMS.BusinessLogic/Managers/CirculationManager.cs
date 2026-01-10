using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LMS.DataAccess.Repositories;
using System.Linq;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;
using LMS.BusinessLogic.Managers.Interfaces;

namespace LMS.BusinessLogic.Managers
{
    /// <summary>
    /// Manager for handling circulation operations.
    /// (existing code remains; below we add a thin wrapper for adding fine charges)
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

        public bool CompleteReturnGood(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount)
        {
            if (transactionId <= 0 || copyId <= 0 || memberId <= 0)
                return false;

            return _circulationRepo.CompleteReturnGood(transactionId, copyId, memberId, returnDate, fineAmount);
        }

        public bool CompleteReturnWithCondition(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount, string condition)
        {
            if (transactionId <= 0 || copyId <= 0 || memberId <= 0) return false;
            if (string.IsNullOrWhiteSpace(condition)) condition = "Damaged";
            return _circulationRepo.CompleteReturnWithCondition(transactionId, copyId, memberId, returnDate, fineAmount, condition);
        }

        public List<DTOFineRecord> GetFinesByMemberId(int memberId)
        {
            if (memberId <= 0)
                return new List<DTOFineRecord>();

            return _circulationRepo.GetFinesByMemberId(memberId);
        }

        public bool AddFineCharge(int memberId, int transactionId, decimal amount, string fineType, DateTime dateIssued, string status)
        {
            if (memberId <= 0) return false;
            return _circulationRepo.AddFineCharge(memberId, transactionId, amount, fineType, dateIssued, status);
        }

        public bool WaiveFines(List<int> fineIds, string reason)
        {
            if (fineIds == null || fineIds.Count == 0)
                return false;

            return _circulationRepo.WaiveFines(fineIds, reason);
        }

        public List<int> ProcessPayment(List<int> fineIds, string paymentMode, DateTime paymentDate)
        {
            if (fineIds == null || fineIds.Count == 0)
                return new List<int>();

            return _circulationRepo.ProcessPayment(fineIds, paymentMode, paymentDate);
        }
    }
}