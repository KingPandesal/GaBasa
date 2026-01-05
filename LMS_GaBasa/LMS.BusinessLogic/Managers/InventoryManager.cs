using LMS.BusinessLogic.Managers.Interfaces;
using LMS.BusinessLogic.Services.Book.AddBook;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Book;
using LMS.Model.Models.Enums;
using System;
using System.Linq;

namespace LMS.BusinessLogic.Managers
{
    public class InventoryManager : IInventoryManager
    {
        private readonly IAddBookService _addBookService;
        private readonly IBookRepository _bookRepo;
        private readonly ICatalogManager _catalogManager;

        public InventoryManager(
            IAddBookService addBookService,
            IBookRepository bookRepo,
            ICatalogManager catalogManager)
        {
            _addBookService = addBookService ?? throw new ArgumentNullException(nameof(addBookService));
            _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
            _catalogManager = catalogManager ?? throw new ArgumentNullException(nameof(catalogManager));
        }

        public BookCreationResultService AddBook(DTOCreateBook dto)
        {
            // Pre-process: Handle category creation if typed
            if (dto.CategoryID <= 0 && !string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                var category = _catalogManager.GetOrCreateCategory(dto.CategoryName);
                if (category != null)
                    dto.CategoryID = category.CategoryID;
            }

            // Do NOT try to persist language to a separate table here.
            // Language is stored as a string on the Book model (Book.Language).
            // Removed call: _catalogManager.AddLanguageIfNotExists(dto.Language);

            return _addBookService.CreateBook(dto);
        }

        public bool ValidateBookData(DTOCreateBook dto, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(dto.ISBN))
            {
                errorMessage = "ISBN is required.";
                return false;
            }

            // ISBN must contain digits only
            if (!dto.ISBN.All(char.IsDigit))
            {
                errorMessage = "ISBN must contain digits only.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.CallNumber))
            {
                errorMessage = "Call number is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                errorMessage = "Title is required.";
                return false;
            }

            if (dto.Authors == null || !dto.Authors.Any())
            {
                errorMessage = "At least one author is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.Publisher))
            {
                errorMessage = "Publisher is required.";
                return false;
            }

            // Publication year must not be in the future
            int currentYear = DateTime.Now.Year;
            if (dto.PublicationYear > currentYear)
            {
                errorMessage = $"Publication year cannot be greater than {currentYear}.";
                return false;
            }

            if (dto.PublicationYear <= 0)
            {
                errorMessage = "Publication year is required.";
                return false;
            }

            if (dto.CategoryID <= 0 && string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                errorMessage = "Category is required.";
                return false;
            }

            if (dto.Pages <= 0)
            {
                errorMessage = "Number of pages is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.PhysicalDescription))
            {
                errorMessage = "Physical description is required.";
                return false;
            }


            // Resource type specific validation
            if (dto.ResourceType == ResourceType.PhysicalBook)
            {
                if (string.IsNullOrWhiteSpace(dto.LoanType))
                {
                    errorMessage = "Please select Reference or Circulation for physical book.";
                    return false;
                }
            }
            else if (dto.ResourceType == ResourceType.EBook)
            {
                if (string.IsNullOrWhiteSpace(dto.DownloadURL))
                {
                    errorMessage = "Download link is required for e-books.";
                    return false;
                }
            }

            // Copy information
            if (dto.InitialCopyCount <= 0)
            {
                errorMessage = "Number of copies is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.CopyStatus))
            {
                errorMessage = "Copy status is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dto.CopyLocation))
            {
                errorMessage = "Location is required.";
                return false;
            }

            return true;
        }
    }
}
