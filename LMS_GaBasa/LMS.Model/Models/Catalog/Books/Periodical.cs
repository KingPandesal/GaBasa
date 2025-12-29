using LMS.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Catalog.Books
{
    public class Periodical : Book
    {
        public Periodical()
        {
            ResourceType = ResourceType.Periodical;
        }
    }
}
