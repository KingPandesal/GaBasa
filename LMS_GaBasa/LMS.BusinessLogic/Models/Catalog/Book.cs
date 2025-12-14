using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string CallNumber { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int PublisherID { get; set; }
        public Publisher Publisher { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public string Language { get; set; }
        public int Pages { get; set; }
        public string Edition { get; set; }
        public int PublicationYear { get; set; }
        public string PhysicalDescription { get; set; }
        public string ResourceType { get; set; } // Enum: Book, Periodical, Thesis, AV, E-Book
        public string CoverImage { get; set; }

        public List<Author> Authors { get; set; } = new List<Author>();
        public List<BookCopy> Copies { get; set; } = new List<BookCopy>();
    }

}
