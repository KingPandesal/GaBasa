    using System;

namespace LMS.Model.DTOs.Circulation
{
    /// <summary>
    /// DTO containing renewal information for an active borrowing transaction.
    /// </summary>
    public class DTORenewalInfo
    {
        public int TransactionID { get; set; }
        public int CopyID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string Title { get; set; }
        public string AccessionNumber { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        
        // Renewal tracking
        public int RenewalCount { get; set; }
        public int MaxRenewals { get; set; }
        public int BorrowingPeriod { get; set; }
        
        // Eligibility flags
        public bool HasActiveReservation { get; set; }
        public bool IsWithinRenewalLimit { get; set; }
        
        // Computed
        public bool CanRenew => !HasActiveReservation && IsWithinRenewalLimit;
        
        public DateTime CalculateNewDueDate()
        {
            // New due date is current due date + borrowing period
            return DueDate.AddDays(BorrowingPeriod);
        }
    }
}
