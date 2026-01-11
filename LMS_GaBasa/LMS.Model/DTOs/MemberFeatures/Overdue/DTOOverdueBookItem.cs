using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.MemberFeatures.Overdue
{
    /// <summary>
    /// DTO representing an overdue book item for member display.
    /// </summary>
    public class DTOOverdueBookItem
    {
        public int TransactionID { get; set; }
        public int CopyID { get; set; }
        public int BookID { get; set; }
        public string AccessionNumber { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Category { get; set; }
        public string CoverImage { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public decimal FineRate { get; set; }
        public decimal CurrentFine { get; set; }
    }
}
