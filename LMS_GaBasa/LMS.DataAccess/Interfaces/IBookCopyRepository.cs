using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;

namespace LMS.DataAccess.Interfaces
{
    public interface IBookCopyRepository
    {
        int Add(BookCopy copy);
        List<BookCopy> GetByBookId(int bookId);

        // Generate accession number using the provided date (year component is used)
        string GenerateAccessionNumber(int bookId, DateTime dateAdded);

        // Persist generated barcode image path for a copy identified by accession number
        bool UpdateBarcodeImage(string accessionNumber, string barcodeImagePath);
    }
}
