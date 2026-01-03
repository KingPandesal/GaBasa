using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.User
{
    public class DTOFetchAllUsers
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; }
        public string LastLogin { get; set; }
    }
}
