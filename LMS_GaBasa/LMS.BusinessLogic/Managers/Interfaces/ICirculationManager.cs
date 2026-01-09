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
    }
}