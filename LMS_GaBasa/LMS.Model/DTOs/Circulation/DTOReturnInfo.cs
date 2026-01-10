using System;

namespace LMS.Model.DTOs.Circulation
{
    /// <summary>
    /// DTO returned when looking up an active borrowing transaction by accession number.
    /// </summary>
    public class DTOReturnInfo
    {
        public int TransactionID { get; set; }
        public int CopyID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Title { get; set; }
        public string AccessionNumber { get; set; }

        // Derived
        public int DaysOverdue { get; set; }
        public decimal FineAmount { get; set; }
    }
}
