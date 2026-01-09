using LMS.Model.DTOs.Circulation;

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
        /// <param name="memberId">The numeric member ID.</param>
        /// <returns>Member info if found; otherwise null.</returns>
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
    }
}
