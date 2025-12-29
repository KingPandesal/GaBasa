using LMS.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Catalog.Books
{
    public class EBook : Book
    {
        public EBook()
        {
            ResourceType = ResourceType.EBook;
        }

    }
}
