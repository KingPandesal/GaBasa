using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int CopyID { get; set; }
        public BookCopy BookCopy { get; set; } // navigation: FK
        public int MemberID { get; set; }
        public Member Member { get; set; } // navigation: FK
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; } 
        // Enum: Active, Cancelled, Completed
    }

}
