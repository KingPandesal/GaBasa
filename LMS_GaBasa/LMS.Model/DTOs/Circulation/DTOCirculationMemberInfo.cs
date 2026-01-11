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
        public decimal MaxFineCap { get; set; }
        public int BorrowingPeriod { get; set; } // in days
        public int RenewalLimit { get; set; }
        public bool ReservationPrivilege { get; set; }

        // Borrowing statistics
        public int CurrentBorrowedCount { get; set; }
        public int OverdueCount { get; set; }
        public decimal TotalUnpaidFines { get; set; }

        /// <summary>
        /// Calculates the penalty level based on fines and overdues.
        /// 0 = Good standing, 1 = has fine OR overdue, 2 = has BOTH fine AND overdue
        /// </summary>
        public int PenaltyLevel
        {
            get
            {
                bool hasFines = TotalUnpaidFines > 0;
                bool hasOverdues = OverdueCount > 0;

                if (hasFines && hasOverdues)
                    return 2; // Both fine and overdue
                if (hasFines || hasOverdues)
                    return 1; // Either fine or overdue
                return 0; // Good standing
            }
        }
        /// <summary>
        /// Effective max books allowed after applying penalty reduction.
        /// </summary>
        public int EffectiveMaxBooksAllowed => Math.Max(0, MaxBooksAllowed - PenaltyLevel);

        /// <summary>
        /// Effective borrowing period after applying penalty reduction.
        /// </summary>
        public int EffectiveBorrowingPeriod => Math.Max(1, BorrowingPeriod - PenaltyLevel);

        /// <summary>
        /// Effective renewal limit after applying penalty reduction.
        /// </summary>
        public int EffectiveRenewalLimit => Math.Max(0, RenewalLimit - PenaltyLevel);
        
        /// <summary>
        /// Effective reservation privilege - disabled if any penalty exists.
        /// </summary>
        public bool EffectiveReservationPrivilege => ReservationPrivilege && PenaltyLevel == 0;


        // Eligibility checks
        public bool IsActive => string.Equals(Status, "Active", StringComparison.OrdinalIgnoreCase);
        public bool HasNoOverdue => OverdueCount == 0;
        public bool IsBorrowLimitOk => CurrentBorrowedCount < MaxBooksAllowed;
        public bool IsFineWithinLimit => TotalUnpaidFines <= MaxFineCap;

        /// <summary>
        /// Returns true if the member can borrow books (all eligibility checks pass).
        /// </summary>
        public bool CanBorrow => IsActive && HasNoOverdue && IsBorrowLimitOk && IsFineWithinLimit;

        /// <summary>
        /// Calculates the due date based on the member's borrowing period.
        /// </summary>
        public DateTime CalculateDueDate() => DateTime.Today.AddDays(BorrowingPeriod > 0 ? BorrowingPeriod : 14);
    }
}
