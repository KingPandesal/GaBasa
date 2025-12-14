using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Models.Transactions
{
    public class Fine
    {
        public int FineID { get; set; }
        public int MemberID { get; set; }
        public Member Member { get; set; } // navigation: FK
        public int TransactionID { get; set; }
        public BorrowingTransaction Transaction { get; set; } // navigation: FK
        public decimal FineAmount { get; set; }
        public string FineType { get; set; } 
        // Enum: Overdue, Lost, Damaged, CardReplacement
        public DateTime DateIssued { get; set; }
        public string Status { get; set; } 
        // Enum: Paid, Unpaid, Waived
    }

}
