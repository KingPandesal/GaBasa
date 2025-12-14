using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Models
{
    public class BookCopy
    {
        public int CopyID { get; set; }
        public int BookID { get; set; }
        public Book Book { get; set; } // navigation
        public string AccessionNumber { get; set; }
        public string Status { get; set; } 
        // Enum: Available, Borrowed, Reserved, Lost, Damaged, Repair
        public string Location { get; set; }
    }

}
