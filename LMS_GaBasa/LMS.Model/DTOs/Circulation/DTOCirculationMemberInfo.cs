using System;

namespace LMS.Model.DTOs.Circulation
{
    /// <summary>
    /// DTO containing member information for circulation verification.
    /// </summary>
    public class DTOCirculationMemberInfo
    {
        // Basic member info
        public int MemberID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string MemberType { get; set; }
        public string Status { get; set; }
        public string PhotoPath { get; set; }
        public string ValidIdPath { get; set; }

        // Member type privileges
        public int MaxBooksAllowed { get; set; }
        public decimal FineRate { get; set; }

        // Borrowing statistics
        public int CurrentBorrowedCount { get; set; }
        public int OverdueCount { get; set; }
        public decimal TotalUnpaidFines { get; set; }

        // Eligibility checks
        public bool IsActive => string.Equals(Status, "Active", StringComparison.OrdinalIgnoreCase);
        public bool HasNoOverdue => OverdueCount == 0;
        public bool IsBorrowLimitOk => CurrentBorrowedCount < MaxBooksAllowed;
        public bool IsFineWithinLimit => TotalUnpaidFines < 100m; // Configurable threshold

        /// <summary>
        /// Returns true if the member can borrow books (all eligibility checks pass).
        /// </summary>
        public bool CanBorrow => IsActive && HasNoOverdue && IsBorrowLimitOk && IsFineWithinLimit;
    }
}
