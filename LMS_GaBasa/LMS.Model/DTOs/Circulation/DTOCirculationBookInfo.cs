using System;

namespace LMS.Model.DTOs.Circulation
{
    /// <summary>
    /// DTO containing book/copy information for circulation checkout.
    /// </summary>
    public class DTOCirculationBookInfo
    {
        public int CopyID { get; set; }
        public int BookID { get; set; }
        public string AccessionNumber { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }          // comma-separated authors (Role = "Author" only)
        public string Category { get; set; }
        public string LoanType { get; set; }         // "Circulation" | "Reference" | null
        public string CopyStatus { get; set; }       // "Available" etc.
        public string Location { get; set; }

        // Additional fields for decision logic
        public string ResourceType { get; set; }     // e.g. "PhysicalBook", "EBook", "Thesis", etc.
        public string DownloadURL { get; set; }      // non-empty => digital resource

        // Derived properties
        public bool IsDigital => 
            string.Equals(ResourceType, "EBook", StringComparison.OrdinalIgnoreCase)
            || !string.IsNullOrWhiteSpace(DownloadURL);

        public bool IsCirculation =>
            !IsDigital && string.Equals(LoanType, "Circulation", StringComparison.OrdinalIgnoreCase);

        public bool IsAvailable => string.Equals(CopyStatus, "Available", StringComparison.OrdinalIgnoreCase);

        public bool CanBorrow => IsCirculation && IsAvailable;
    }
}
