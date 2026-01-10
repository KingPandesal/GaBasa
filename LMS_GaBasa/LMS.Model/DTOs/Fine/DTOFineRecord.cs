using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.Fine
{
    /// <summary>
    /// DTO for displaying fine records in the Fines management grid.
    /// </summary>
    public class DTOFineRecord
    {
        public int FineID { get; set; }
        public int TransactionID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public decimal FineAmount { get; set; }
        public string FineType { get; set; }
        public DateTime DateIssued { get; set; }
        public string Status { get; set; }
    }
}
