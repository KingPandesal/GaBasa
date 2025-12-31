using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.User
{
    public class DTOUpdateUserProfile
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
    }
}
