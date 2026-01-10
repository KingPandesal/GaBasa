using LMS.Model.DTOs.Circulation;
using System;

namespace LMS.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for circulation data access operations.
    /// </summary>
    public interface ICirculationRepository
    {
        /// <summary>
        /// Gets member information by MemberID for circulation verification.
        /// </summary>
        DTOCirculationMemberInfo GetMemberInfoByMemberId(int memberId);

        /// <summary>
        /// Gets the count of currently borrowed books (not returned) for a member.
        /// </summary>
        int GetCurrentBorrowedCount(int memberId);

        /// <summary>
        /// Gets the count of overdue books for a member.
        /// </summary>
        int GetOverdueCount(int memberId);

        /// <summary>
        /// Gets the total unpaid fines for a member.
        /// </summary>
        decimal GetTotalUnpaidFines(int memberId);

        /// <summary>
        /// Gets book/copy information by accession number for checkout.
        /// </summary>
        DTOCirculationBookInfo GetBookInfoByAccession(string accessionNumber);

        // Add a borrowing transaction and mark the copy as borrowed. Returns new TransactionID (>0) on success, 0 on failure.
        int CreateBorrowingTransaction(int copyId, int memberId, DateTime borrowDate, DateTime dueDate);

        /// <summary>
        /// Gets an active (not-yet-returned) borrowing transaction by accession number.
        /// </summary>
        DTOReturnInfo GetActiveBorrowingByAccession(string accessionNumber);
    }
}
