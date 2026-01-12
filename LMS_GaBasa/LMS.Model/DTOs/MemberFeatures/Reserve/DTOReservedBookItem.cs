using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.MemberFeatures.Reserve
{
    /// <summary>
    /// DTO representing a reserved book item for member display.
    /// </summary>
    public class DTOReservedBookItem
    {
        public int ReservationID { get; set; }
        public int CopyID { get; set; }
        public int BookID { get; set; }
        public string AccessionNumber { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ResourceType { get; set; }
        public string CoverImage { get; set; }
        public string Status { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DaysUntilExpiration { get; set; }
        public bool IsExpired { get; set; }
        public int QueuePosition { get; set; }
    }
}
