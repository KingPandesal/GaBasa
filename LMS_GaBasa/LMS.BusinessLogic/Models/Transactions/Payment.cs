using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Models.Transactions
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int FineID { get; set; }
        public Fine Fine { get; set; } // navigation: FK
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMode { get; set; } 
        // Enum: Cash, Online
    }

}
