using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Enums;

namespace LMS.Model.DTOs.Member
{
    public class DTOEditMember
    {
        public int MemberID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string MemberTypeName { get; set; }
        public string PhotoPath { get; set; }
        public string ValidIdPath { get; set; }
        public MemberStatus Status { get; set; }
    }
}
