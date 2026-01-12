using System;

namespace LMS.Model.DTOs.MemberFeatures.History
{
    /// <summary>
    /// Unified DTO for member history rows (borrows, returns, reservations, fine payments, cancellations).
    /// </summary>
    public class DTOHistoryItem
    {
        /// <summary>Friendly title (book title / fine description)</summary>
        public string Title { get; set; }

        /// <summary>Timestamp of the action</summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>Human readable status (Borrowed, Returned, Reserved, Reservation Cancelled, Fine Paid, etc.)</summary>
        public string Status { get; set; }

        /// <summary>Optional details (accession number, amount, notes)</summary>
        public string Details { get; set; }
    }
}
