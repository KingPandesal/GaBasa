using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.Catalog
{
    /// <summary>
    /// DTO for displaying book information in the catalog UI.
    /// </summary>
    public class DTOCatalogBook
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string CoverImagePath { get; set; }
        public DateTime DateAdded { get; set; }
        public int BorrowCount { get; set; }
    }
}
