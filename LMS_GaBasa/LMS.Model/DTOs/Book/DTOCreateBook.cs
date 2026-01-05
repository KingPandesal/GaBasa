using System.Collections.Generic;
using LMS.Model.Models.Enums;

namespace LMS.Model.DTOs.Book
{
    public class DTOCreateBook
    {
        public string ISBN { get; set; }
        public string CallNumber { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }

        // Publisher
        public int PublisherID { get; set; }
        public string Publisher { get; set; }

        // Category - can be ID (existing) or Name (new)
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string Language { get; set; }
        public int Pages { get; set; }
        public string Edition { get; set; }
        public int PublicationYear { get; set; }
        public string PhysicalDescription { get; set; }
        public ResourceType ResourceType { get; set; }
        public string CoverImage { get; set; }
        public string LoanType { get; set; } // "Reference" or "Circulation"
        public string DownloadURL { get; set; }

        // Authors with their roles
        public List<DTOBookAuthor> Authors { get; set; } = new List<DTOBookAuthor>();

        // Editors
        public List<DTOBookAuthor> Editors { get; set; } = new List<DTOBookAuthor>();

        // Copy information
        public int InitialCopyCount { get; set; } = 1;
        public string CopyStatus { get; set; } = "Available";
        public string CopyLocation { get; set; }

        // Who adds the copies (optional). Presentation layer should set this to current user id if available.
        public int AddedByID { get; set; } = 0;
    }

    public class DTOBookAuthor
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string Role { get; set; } = "Author";
        public bool IsPrimaryAuthor { get; set; }
    }
}
