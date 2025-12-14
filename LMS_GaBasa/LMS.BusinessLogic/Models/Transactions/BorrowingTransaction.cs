using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Models.Transactions
{
    public class BorrowingTransaction
    {
        public int TransactionID { get; set; }
        public int CopyID { get; set; }
        public BookCopy BookCopy { get; set; } // navigation: FK
        public int MemberID { get; set; }
        public Member Member { get; set; } // navigation: FK
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } // Enum: Borrowed, Returned, Overdue
    }

}
