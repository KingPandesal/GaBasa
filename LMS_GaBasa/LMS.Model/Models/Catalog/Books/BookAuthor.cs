using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Catalog.Books
{
    public class BookAuthor
    {
        public int BookID { get; set; }
        public Book Book { get; set; }

        public int AuthorID { get; set; }
        public Author Author { get; set; }

        public string Role { get; set; }  // e.g., "Author","Editor", "Adviser"
        public bool IsPrimaryAuthor { get; set; }
    }
}