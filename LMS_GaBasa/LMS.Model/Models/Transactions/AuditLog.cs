using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Transactions
{
    public class AuditLog
    {
        public int LogID { get; set; }           // Primary key
        public int UserID { get; set; }          // User who performed the action
        public User User { get; set; } // FK
        public string ModuleName { get; set; }   // Module where the action happened
        public string ActionPerformed { get; set; } // Description of the action
        public string Details { get; set; }      // Optional extra details
        public DateTime Timestamp { get; set; }
    }
}
