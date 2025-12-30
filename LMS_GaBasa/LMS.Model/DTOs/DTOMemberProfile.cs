using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs
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
    }

}
