using System;

namespace LMS.Model.DTOs.Member
{
    public class DTOFetchAllMembers
    {
        public int MemberID { get; set; }
        public string FullName { get; set; }
        public string MemberType { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowingPeriod { get; set; }
        public int RenewalLimit { get; set; }
        public bool ReservationPrivilege { get; set; }
        public decimal FineRate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
    }
}
