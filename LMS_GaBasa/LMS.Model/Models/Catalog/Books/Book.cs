using System;
using LMS.Model.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Catalog;

namespace LMS.Model.Models.Catalog.Books
{
    public abstract class Book
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
        public ResourceType ResourceType { get; set; }
        // Enum: Book, Periodical, Thesis, AV, E-Book
        public string CoverImage { get; set; }
        public string LoanType { get; set; }
        public string DownloadURL { get; set; }

        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public List<BookCopy> Copies { get; set; } = new List<BookCopy>();
    }
}
