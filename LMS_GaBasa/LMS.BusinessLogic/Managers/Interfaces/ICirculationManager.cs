using System;
using LMS.Model.DTOs.Circulation;

namespace LMS.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// Interface for managing circulation operations (borrow, return, renew).
    /// </summary>
    public interface ICirculationManager
    {
        /// <summary>
        /// Gets member information for circulation verification by formatted Member ID (e.g., "MEM-0023").
        /// Includes borrowing statistics and eligibility checks.
        /// </summary>
        /// <param name="formattedMemberId">The formatted member ID (e.g., "MEM-0023").</param>
        /// <returns>Member info with statistics if found; otherwise null.</returns>
        DTOCirculationMemberInfo GetMemberByFormattedId(string formattedMemberId);

        /// <summary>
        /// Parses a formatted member ID (e.g., "MEM-0023") to extract the numeric member ID.
        /// </summary>
        /// <param name="formattedMemberId">The formatted member ID.</param>
        /// <returns>The numeric member ID if valid; otherwise null.</returns>
        int? ParseFormattedMemberId(string formattedMemberId);

        /// <summary>
        /// Gets book/copy information by accession number for checkout.
        /// </summary>
        /// <param name="accessionNumber">Accession number string (exact match).</param>
        /// <returns>Book/copy DTO or null if not found.</returns>
        DTOCirculationBookInfo GetBookByAccession(string accessionNumber);

        /// <summary>
        /// Creates a borrowing transaction.
        /// </summary>
        /// <param name="copyId">The ID of the copy being borrowed.</param>
        /// <param name="memberId">The ID of the member borrowing the item.</param>
        /// <param name="borrowDate">The date the item is borrowed.</param>
        /// <param name="dueDate">The due date for returning the item.</param>
        /// <returns>The TransactionID if the transaction is successful; otherwise, 0.</returns>
        int CreateBorrowingTransaction(int copyId, int memberId, DateTime borrowDate, DateTime dueDate);

        /// <summary>
        /// Lookup active borrowing by accession (for returns).
        /// </summary>
        /// <param name="accessionNumber">Accession number of the book/copy.</param>
        /// <returns>Active borrowing information if found; otherwise null.</returns>
        DTOReturnInfo GetActiveBorrowingByAccession(string accessionNumber);

        /// <summary>
        /// Completes a return with condition "Good".
        /// Updates transaction status to Returned, sets return date, marks copy as Available,
        /// and creates a Fine record if there's an overdue amount.
        /// </summary>
        /// <param name="transactionId">The BorrowingTransaction ID.</param>
        /// <param name="copyId">The BookCopy ID.</param>
        /// <param name="memberId">The Member ID.</param>
        /// <param name="returnDate">The return date.</param>
        /// <param name="fineAmount">The overdue fine amount (0 if no fine).</param>
        /// <returns>True if successful; otherwise false.</returns>
        bool CompleteReturnGood(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount);

        /// <summary>
        /// Completes a return when condition is Lost/Damaged.
        /// </summary>
        /// <param name="transactionId">The BorrowingTransaction ID.</param>
        /// <param name="copyId">The BookCopy ID.</param>
        /// <param name="memberId">The Member ID.</param>
        /// <param name="returnDate">The return date.</param>
        /// <param name="fineAmount">The overdue fine amount (0 if no fine).</param>
        /// <param name="condition">The condition of the item being returned (Lost/Damaged).</param>
        /// <returns>True if successful; otherwise false.</returns>
        bool CompleteReturnWithCondition(int transactionId, int copyId, int memberId, DateTime returnDate, decimal fineAmount, string condition);
    }
}