using System;
using System.Collections.Generic;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;

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

        /// <summary>
        /// Completes a return transaction for condition "Good".
        /// Updates BorrowingTransaction (Status=Returned, ReturnDate), BookCopy (Status=Available),
        /// and optionally creates a Fine record if there's an overdue amount.
        /// </summary>
        /// <param name="transactionId">The BorrowingTransaction ID.</param>
        /// <param name="copyId">The BookCopy ID.</param>
        /// <param name="memberId">The Member ID.</param>
        /// <param name="returnDate">The return date/time.</param>
        /// <param name="fineAmount">The overdue fine amount (0 if no fine).</param>
        /// <returns>True if successful; otherwise false.</returns>
        bool CompleteReturnGood(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount);

        /// <summary>
        /// Completes a return transaction for non-good conditions (e.g., "Lost", "Damaged").
        /// Updates BorrowingTransaction (Status=Returned, ReturnDate), BookCopy (Status = condition),
        /// and if fineAmount &gt; 0 inserts a Fine record with FineType set to the condition.
        /// </summary>
        /// <param name="transactionId">The BorrowingTransaction ID.</param>
        /// <param name="copyId">The BookCopy ID.</param>
        /// <param name="memberId">The Member ID.</param>
        /// <param name="returnDate">The return date/time.</param>
        /// <param name="fineAmount">Total fine/penalty to record (0 if none).</param>
        /// <param name="condition">Condition string: "Lost" or "Damaged" (case-insensitive).</param>
        /// <returns>True if successful; otherwise false.</returns>
        bool CompleteReturnWithCondition(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount, string condition);

        /// <summary>
        /// Gets all fine records for a member.
        /// </summary>
        /// <param name="memberId">The Member ID.</param>
        /// <returns>List of fine records.</returns>
        List<DTOFineRecord> GetFinesByMemberId(int memberId);

        /// <summary>
        /// Inserts a fine (charge) for a member. If transactionId &lt;= 0 the DB value for TransactionID will be NULL.
        /// </summary>
        /// <returns>True if inserted; otherwise false.</returns>
        bool AddFineCharge(int memberId, int transactionId, decimal amount, string fineType, DateTime dateIssued, string status);
    }
}
