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
        // Comma-separated authors (all authors) for display
        public string Authors { get; set; }

        // Location of the first copy (if any)
        public string FirstCopyLocation { get; set; }

        // Accession number of first copy (if any)
        public string FirstCopyAccession { get; set; }

        // Explicit ISBN property (Standard ID). Populate this in your repository/mapper.
        public string ISBN { get; set; }

        // Explicit CallNumber property. Populate this in your repository/mapper.
        public string CallNumber { get; set; }

        // NEW: Publisher name (from Book.Publisher.Name)
        public string Publisher { get; set; }

        // NEW: Publication year (from Book.PublicationYear)
        public int PublicationYear { get; set; }
    }
}
