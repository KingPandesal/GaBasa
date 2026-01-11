using System;

namespace LMS.Model.DTOs.MemberFeatures.Borrowed
{
    /// <summary>
    /// DTO representing a borrowed book item for member display.
    /// </summary>
    public class DTOBorrowedBookItem
    {
        public int TransactionID { get; set; }
        public int CopyID { get; set; }
        public int BookID { get; set; }
        public string AccessionNumber { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ResourceType { get; set; }
        public string CoverImage { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysUntilDue { get; set; }
    }
}
