using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LMS.BusinessLogic.Models.Users;

namespace LMS.BusinessLogic.Models.Members
{
    public class Member
    {
        public int MemberID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; } // navigation: FK
        public int MemberTypeID { get; set; }
        public MemberType MemberType { get; set; } // navigation: FK
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Photo { get; set; }
        public string ValidID { get; set; }
        public string Status { get; set; }
        // Enum: Active, Inactive, Suspended, Expired
    }

}
