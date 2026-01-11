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

        /// <summary>
        /// Adds a borrowing transaction and marks the copy as borrowed.
        /// </summary>
        /// <param name="copyId">The Copy ID.</param>
        /// <param name="memberId">The Member ID.</param>
        /// <param name="borrowDate">The borrow date.</param>
        /// <param name="dueDate">The due date.</param>
        /// <returns>The new TransactionID if successful; otherwise, 0.</returns>
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

        /// <summary>
        /// Waives the specified fines by updating their Status to 'Waived' and setting the Reason.
        /// </summary>
        /// <param name="fineIds">List of FineIDs to waive.</param>
        /// <param name="reason">The reason for waiving.</param>
        /// <returns>True if all updated successfully; otherwise false.</returns>
        bool WaiveFines(List<int> fineIds, string reason);

        /// <summary>
        /// Processes payment for the specified fines.
        /// Inserts Payment records and updates Fine.Status to 'Paid'.
        /// Returns list of created PaymentIDs (empty on failure).
        /// </summary>
        List<int> ProcessPayment(List<int> fineIds, string paymentMode, DateTime paymentDate);

        /// <summary>
        /// Gets renewal information for an active borrowing by accession number.
        /// Includes reservation check and renewal limit validation.
        /// </summary>
        DTORenewalInfo GetRenewalInfoByAccession(string accessionNumber);

        /// <summary>
        /// Checks if there are active reservations for the specified BookID (not CopyID).
        /// Active = Status in ('Pending', 'Ready')
        /// </summary>
        bool HasActiveReservationForBook(int bookId);

        /// <summary>
        /// Gets the BookID for a given CopyID.
        /// </summary>
        int? GetBookIdByCopyId(int copyId);

        /// <summary>
        /// Attempts to renew a borrowing transaction.
        /// Returns true on success and outputs the new due date.
        /// </summary>
        bool RenewBorrowingTransaction(int transactionId, out DateTime newDueDate);

        /// <summary>
        /// Updates the Member table Status field for the specified member.
        /// </summary>
        /// <param name="memberId">The Member ID.</param>
        /// <param name="status">The new status (e.g., "Active", "Suspended").</param>
        /// <returns>True if updated; otherwise false.</returns>
        bool UpdateMemberStatus(int memberId, string status);

        /// <summary>
        /// Gets the MaxFineCap for a member from their MemberType.
        /// </summary>
        /// <param name="memberId">The Member ID.</param>
        /// <returns>The MaxFineCap value, or 0 if not found.</returns>
        decimal GetMemberMaxFineCap(int memberId);

        /// <summary>
        /// Checks if member's total unpaid fines have reached or exceeded MaxFineCap and suspends them if so.
        /// </summary>
        /// <param name="memberId">The Member ID.</param>
        void CheckAndSuspendMemberIfNeeded(int memberId);

        /// <summary>
        /// Checks if member's total unpaid fines have dropped below MaxFineCap and reactivates them if so.
        /// Only reactivates if current status is "Suspended".
        /// </summary>
        /// <param name="memberId">The Member ID.</param>
        void CheckAndReactivateMemberIfNeeded(int memberId);
    }
}
