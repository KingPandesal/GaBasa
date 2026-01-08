using System;
using System.Collections.Generic;
using System.Linq;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Catalog;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Enums;

namespace LMS.BusinessLogic.Managers
{
    public class CatalogManager : ICatalogManager
    {
        private readonly IBookRepository _bookRepo;
        private readonly IBookCopyRepository _bookCopyRepo;
        private readonly BookAuthorRepository _bookAuthorRepo;
        private readonly AuthorRepository _authorRepo;
        private readonly CategoryRepository _categoryRepo;

        public CatalogManager()
            : this(new BookRepository(), new BookCopyRepository(), new BookAuthorRepository(), new AuthorRepository(), new CategoryRepository())
        {
        }

        public CatalogManager(
            IBookRepository bookRepo,
            IBookCopyRepository bookCopyRepo,
            BookAuthorRepository bookAuthorRepo,
            AuthorRepository authorRepo,
            CategoryRepository categoryRepo)
        {
            _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
            _bookCopyRepo = bookCopyRepo ?? throw new ArgumentNullException(nameof(bookCopyRepo));
            _bookAuthorRepo = bookAuthorRepo ?? throw new ArgumentNullException(nameof(bookAuthorRepo));
            _authorRepo = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepo.GetAll() ?? new List<Category>();
        }

        public Category GetOrCreateCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return null;

            var existing = GetAllCategories()
                .FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                return existing;

            var newCategory = new Category { Name = categoryName };
            int id = _categoryRepo.Add(newCategory);
            newCategory.CategoryID = id;
            return newCategory;
        }

        public List<string> GetAllLanguages()
        {
            // Placeholder - implement if you have a Language table
            return new List<string> { "English", "Spanish", "French", "German", "Italian", "Portuguese",
            "Chinese", "Japanese", "Korean", "Russian", "Arabic", "Hindi",
            "Tagalog", "Cebuano", "Ilocano", "Hiligaynon", "Waray", "Bikol",
            "Dutch", "Swedish", "Norwegian", "Danish", "Finnish", "Polish",
            "Greek", "Turkish", "Vietnamese", "Thai", "Indonesian", "Malay",
            "Hebrew", "Persian", "Bengali", "Urdu", "Tamil", "Telugu" };
            
        }

        public void AddLanguageIfNotExists(string language)
        {
            // Placeholder - implement if you have a Language table
        }

        /// <summary>
        /// Gets books where the earliest BookCopy.DateAdded is within the last 7 days.
        /// </summary>
        public List<DTOCatalogBook> GetNewArrivals()
        {
            var result = new List<DTOCatalogBook>();
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            var allBooks = _bookRepo.GetAll() ?? new List<Book>();

            foreach (var book in allBooks)
            {
                var copies = _bookCopyRepo.GetByBookId(book.BookID) ?? new List<BookCopy>();

                // For digital resources (no copies), we cannot determine DateAdded from copies.
                // Skip them or use a different logic if needed.
                if (copies.Count == 0)
                    continue;

                // Get the earliest DateAdded (first time book was added to library)
                DateTime firstAdded = copies.Min(c => c.DateAdded);

                if (firstAdded >= oneWeekAgo)
                {
                    result.Add(MapToDTO(book, copies, firstAdded));
                }
            }

            // Order by most recent first
            return result.OrderByDescending(b => b.DateAdded).ToList();
        }

        /// <summary>
        /// Gets the most borrowed books. Uses BorrowCount from BookCopy status tracking.
        /// Since there's no BorrowCount column yet, we'll count copies with Status = "Borrowed" historically.
        /// For now, this returns books ordered by total copies as a placeholder.
        /// TODO: Implement proper borrow count tracking when Borrow/Transaction table is available.
        /// </summary>
        public List<DTOCatalogBook> GetPopularBooks(int topCount = 10)
        {
            var result = new List<DTOCatalogBook>();
            var allBooks = _bookRepo.GetAll() ?? new List<Book>();

            foreach (var book in allBooks)
            {
                var copies = _bookCopyRepo.GetByBookId(book.BookID) ?? new List<BookCopy>();

                // Use total copies count as a proxy for popularity until borrow tracking exists
                int borrowCount = copies.Count;

                DateTime firstAdded = copies.Count > 0 ? copies.Min(c => c.DateAdded) : DateTime.MinValue;

                var dto = MapToDTO(book, copies, firstAdded);
                dto.BorrowCount = borrowCount;
                result.Add(dto);
            }

            // Order by borrow count descending, take top N
            return result.OrderByDescending(b => b.BorrowCount).Take(topCount).ToList();
        }

        private DTOCatalogBook MapToDTO(Book book, List<BookCopy> copies, DateTime dateAdded)
        {
            // Determine status
            bool isDigital = book.ResourceType == ResourceType.EBook || !string.IsNullOrWhiteSpace(book.DownloadURL);
            string status;

            if (isDigital)
            {
                status = !string.IsNullOrWhiteSpace(book.DownloadURL) ? "Available Online" : "Unavailable";
            }
            else
            {
                int availableCopies = copies.Count(c => 
                    string.Equals(c.Status, "Available", StringComparison.OrdinalIgnoreCase));
                status = availableCopies > 0 ? "Available" : "Unavailable";
            }

            // Get primary author
            string authorName = GetPrimaryAuthor(book.BookID);

            // Get category name
            string categoryName = GetCategoryName(book.CategoryID);

            return new DTOCatalogBook
            {
                BookID = book.BookID,
                Title = book.Title ?? "Untitled",
                Category = categoryName,
                Author = authorName,
                Status = status,
                CoverImagePath = book.CoverImage,
                DateAdded = dateAdded,
                BorrowCount = 0
            };
        }

        private string GetPrimaryAuthor(int bookId)
        {
            try
            {
                var bookAuthors = _bookAuthorRepo.GetByBookId(bookId);
                if (bookAuthors == null || bookAuthors.Count == 0)
                    return "Unknown Author";

                // Try to find primary author first
                var primary = bookAuthors.FirstOrDefault(ba => ba.IsPrimaryAuthor);
                if (primary == null)
                    primary = bookAuthors.FirstOrDefault(ba => 
                        string.Equals(ba.Role, "Author", StringComparison.OrdinalIgnoreCase));
                if (primary == null)
                    primary = bookAuthors.FirstOrDefault();

                if (primary != null)
                {
                    var author = _authorRepo.GetById(primary.AuthorID);
                    if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                        return author.FullName.Trim();
                }

                return "Unknown Author";
            }
            catch
            {
                return "Unknown Author";
            }
        }

        private string GetCategoryName(int categoryId)
        {
            if (categoryId <= 0)
                return "Uncategorized";

            try
            {
                var category = _categoryRepo.GetById(categoryId);
                return category?.Name ?? "Uncategorized";
            }
            catch
            {
                return "Uncategorized";
            }
        }
    }
}
