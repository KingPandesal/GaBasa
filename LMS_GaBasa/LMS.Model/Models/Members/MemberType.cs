using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Members
{
    public class MemberType
    {
        public int MemberTypeID { get; set; }
        public string TypeName { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowingPeriod { get; set; } // in days
        public int RenewalLimit { get; set; }
        public bool ReservationPrivilege { get; set; }
        public decimal FineRate { get; set; }
        public decimal MaxFineCap { get; set; }

    }

}
