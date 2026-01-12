using System.Collections.Generic;
using LMS.Model.DTOs.MemberFeatures.History;

namespace LMS.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for fetching unified member history (borrows, returns, reservations, payments).
    /// </summary>
    public interface IMemberHistoryRepository
    {
        List<DTOHistoryItem> GetBorrowingHistory(int memberId);
        List<DTOHistoryItem> GetReservationHistory(int memberId);
        List<DTOHistoryItem> GetFinePaymentHistory(int memberId);
    }
}
