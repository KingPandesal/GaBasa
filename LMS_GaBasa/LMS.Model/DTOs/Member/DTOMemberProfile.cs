using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs.User;

namespace LMS.Model.DTOs.Member
{
    public class MemberProfileDto : DTOUserProfile
    {
        // Member table
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ValidIdPath { get; set; }
        public string MemberStatus { get; set; }

        // MemberType → PRIVILEGES
        public string MemberTypeName { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowingPeriod { get; set; }
        public int RenewalLimit { get; set; }
        public bool ReservationPrivilege { get; set; }
        public decimal FineRate { get; set; }
        public decimal MaxFineCap { get; set; }

        // Account Standing data
        public decimal TotalUnpaidFines { get; set; }
        public int OverdueCount { get; set; }
    }

}
