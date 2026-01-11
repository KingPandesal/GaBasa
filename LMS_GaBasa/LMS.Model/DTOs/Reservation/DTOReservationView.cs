using System;

namespace LMS.Model.DTOs.Reservation
{
    /// <summary>
    /// DTO for displaying reservation data in the management view.
    /// </summary>
    public class DTOReservationView
    {
        public int ReservationID { get; set; }
        public int CopyID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string BookTitle { get; set; }
        public string AccessionNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
    }
}
