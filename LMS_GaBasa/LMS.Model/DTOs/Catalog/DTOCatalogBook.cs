using System;

namespace LMS.Model.DTOs.Catalog
{
    /// <summary>
    /// DTO used by the catalog UI and search results.
    /// Added extra read-only fields for UI use (Authors, FirstCopyLocation, FirstCopyAccession).
    /// </summary>
    public class DTOCatalogBook
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Author { get; set; } // primary author (keeps existing contract)
        public string Status { get; set; }
        public string CoverImagePath { get; set; }
        public DateTime DateAdded { get; set; }
        public int BorrowCount { get; set; }

        // New fields used by the search/result grid:
        public string Authors { get; set; }               // Comma-separated authors (all authors)
        public string FirstCopyLocation { get; set; }     // Location of the first copy (if any)
        public string FirstCopyAccession { get; set; }    // Accession number of first copy (if any)
        public string ISBN { get; set; }                  // Standard ID
        public string CallNumber { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }

        // NEW: Material format for filtering/display: "Physical" | "Digital"
        public string MaterialFormat { get; set; }

        // NEW: Resource type label for filtering/display:
        // e.g. "Book", "Periodical", "Thesis", "Audio-Visual", "E-Book"
        public string ResourceType { get; set; }

        // NEW: Loan type for filtering/display: "Circulation" | "Reference"
        public string LoanType { get; set; }

        // NEW: Language (mapped from Book.Language) used by language filter
        public string Language { get; set; }

        // NEW: Download URL for digital resources (E-Book, digital periodicals, etc.)
        // Used to determine "Available Online" status
        public string DownloadURL { get; set; }
    }
}
