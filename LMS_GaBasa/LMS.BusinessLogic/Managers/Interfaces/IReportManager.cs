using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Managers.Interfaces
{
    public interface IReportManager
    {
        int GetTotalBooks();
        int GetTotalBorrowedBooks();
        int GetTotalReservedBooks();
        int GetTotalOverdueBooks();
        decimal GetTotalUnpaidFines();

        /// <summary>
        /// Returns usage counts grouped by user Role. Key = Role name, Value = count of users with LastLogin (non-null).
        /// </summary>
        Dictionary<string, int> GetUsageByRole();

        Dictionary<string, int> GetBorrowingTrendByWeek(int weeks = 8);
        Dictionary<string, int> GetBorrowingByMemberType();
    }
}
