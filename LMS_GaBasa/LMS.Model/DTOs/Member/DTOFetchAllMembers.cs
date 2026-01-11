using System;

namespace LMS.Model.DTOs.Member
{
    public class DTOFetchAllMembers
    {
        public int MemberID { get; set; }
        public string FormattedID { get; set; }
        public string FullName { get; set; }
        public string MemberType { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        
        // Base values from MemberType
        public int MaxBooksAllowed { get; set; }
        public int BorrowingPeriod { get; set; }
        public int RenewalLimit { get; set; }
        public bool ReservationPrivilege { get; set; }
        public decimal FineRate { get; set; }
        
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LastLogin { get; set; }
        public string Status { get; set; }

        // Added so UCMembers can show the correct images
        public string PhotoPath { get; set; }
        public string ValidIdPath { get; set; }

        // Penalty calculation fields
        public int OverdueCount { get; set; }
        public decimal TotalUnpaidFines { get; set; }

        /// <summary>
        /// Penalty level: 0 = good, 1 = has fine OR overdue, 2 = has BOTH fine AND overdue
        /// </summary>
        public int PenaltyLevel
        {
            get
            {
                bool hasFines = TotalUnpaidFines > 0m;
                bool hasOverdues = OverdueCount > 0;

                if (hasFines && hasOverdues) return 2;
                if (hasFines || hasOverdues) return 1;
                return 0;
            }
        }

        /// <summary>
        /// Effective max books allowed after penalty reduction.
        /// </summary>
        public int EffectiveMaxBooksAllowed => Math.Max(0, MaxBooksAllowed - PenaltyLevel);

        /// <summary>
        /// Effective borrowing period after penalty reduction.
        /// </summary>
        public int EffectiveBorrowingPeriod => Math.Max(1, BorrowingPeriod - PenaltyLevel);

        /// <summary>
        /// Effective renewal limit after penalty reduction.
        /// </summary>
        public int EffectiveRenewalLimit => Math.Max(0, RenewalLimit - PenaltyLevel);

        /// <summary>
        /// Effective reservation privilege - disabled if any penalty exists.
        /// </summary>
        public bool EffectiveReservationPrivilege => ReservationPrivilege && PenaltyLevel == 0;
    }
}
