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

        /// <summary>
        /// Full-text style search across common fields plus a few filters.
        /// Simple, in-memory approach using repository GetAll() results.
        /// </summary>
        public List<DTOCatalogBook> SearchCatalog(string query,
            string category = null,
            string publisher = null,
            int? year = null,
            string callNumber = null,
            string accessionNumber = null)
        {
            if (string.IsNullOrWhiteSpace(query) &&
                string.IsNullOrWhiteSpace(category) &&
                string.IsNullOrWhiteSpace(publisher) &&
                !year.HasValue &&
                string.IsNullOrWhiteSpace(callNumber) &&
                string.IsNullOrWhiteSpace(accessionNumber))
            {
                return new List<DTOCatalogBook>();
            }

            query = query?.Trim();
            var tokens = string.IsNullOrWhiteSpace(query)
                ? new string[0]
                : query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(t => t.Trim()).Where(t => t.Length > 0).ToArray();

            var allBooks = _bookRepo.GetAll() ?? new List<Book>();
            var result = new List<DTOCatalogBook>();

            foreach (var book in allBooks)
            {
                // fetch copies and authors once per book
                var copies = _bookCopyRepo.GetByBookId(book.BookID) ?? new List<BookCopy>();
                var bookAuthors = _bookAuthorRepo.GetByBookId(book.BookID) ?? new List<BookAuthor>();

                // Filter by explicit filters first
                if (!string.IsNullOrWhiteSpace(category))
                {
                    var catName = GetCategoryName(book.CategoryID) ?? string.Empty;
                    if (!catName.Equals(category, StringComparison.OrdinalIgnoreCase)) continue;
                }

                if (!string.IsNullOrWhiteSpace(publisher))
                {
                    var pubName = book.Publisher?.Name ?? string.Empty;
                    if (!pubName.Equals(publisher, StringComparison.OrdinalIgnoreCase)) continue;
                }

                if (year.HasValue && book.PublicationYear != year.Value) continue;

                if (!string.IsNullOrWhiteSpace(callNumber) && !string.Equals(book.CallNumber?.Trim(), callNumber.Trim(), StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!string.IsNullOrWhiteSpace(accessionNumber))
                {
                    if (!copies.Any(c => string.Equals(c.AccessionNumber, accessionNumber, StringComparison.OrdinalIgnoreCase)))
                        continue;
                }

                // Full-text token matching across title, subtitle, isbn, authors, category, publisher, callnumber
                bool matches = tokens.Length == 0;

                foreach (var t in tokens)
                {
                    var lower = t.ToLowerInvariant();

                    bool tokenMatches =
                        (!string.IsNullOrWhiteSpace(book.Title) && book.Title.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(book.Subtitle) && book.Subtitle.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(book.ISBN) && book.ISBN.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(book.CallNumber) && book.CallNumber.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(book.Publisher?.Name) && book.Publisher.Name.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(GetCategoryName(book.CategoryID)) && GetCategoryName(book.CategoryID).IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (copies.Any(c => !string.IsNullOrWhiteSpace(c.AccessionNumber) && c.AccessionNumber.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0)) ||
                        (bookAuthors.Any(ba =>
                        {
                            var author = _authorRepo.GetById(ba.AuthorID);
                            return author != null && !string.IsNullOrWhiteSpace(author.FullName) && author.FullName.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0;
                        }));

                    if (!tokenMatches)
                    {
                        matches = false;
                        break;
                    }
                    else
                    {
                        matches = true;
                    }
                }

                if (!matches) continue;

                // Map to DTO and add
                DateTime dateAdded = copies.Count > 0 ? copies.Min(c => c.DateAdded) : DateTime.MinValue;
                var dto = MapToDTO(book, copies, dateAdded);
                result.Add(dto);
            }

            // Order by title for deterministic results; you could also order by DateAdded or BorrowCount depending on UI needs.
            return result.OrderBy(b => b.Title).ToList();
        }

        private string MapResourceType(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.PhysicalBook:
                    return "Book";
                case ResourceType.Periodical:
                    return "Periodical";
                case ResourceType.Thesis:
                    return "Thesis";
                case ResourceType.AV:
                    return "Audio-Visual";
                case ResourceType.EBook:
                    return "E-Book";
                default:
                    return "Unknown";
            }
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

            // Get primary author (existing method)
            string authorName = GetPrimaryAuthor(book.BookID);

            // Get category name
            string categoryName = GetCategoryName(book.CategoryID);

            // Compose Authors (prefer only those with Role = "Author", fall back to any authors)
            string authorsAll = null;
            try
            {
                var bas = _bookAuthorRepo.GetByBookId(book.BookID) ?? new List<BookAuthor>();

                // Prefer relations with Role == "Author"
                var roleBas = bas
                    .Where(ba => !string.IsNullOrWhiteSpace(ba.Role) && string.Equals(ba.Role.Trim(), "Author", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var sourceBas = roleBas.Count > 0 ? roleBas : bas;

                var names = sourceBas
                    .Select(ba =>
                    {
                        var a = _authorRepo.GetById(ba.AuthorID);
                        return a?.FullName?.Trim();
                    })
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                authorsAll = names.Length > 0 ? string.Join(", ", names) : authorName;
            }
            catch
            {
                authorsAll = authorName;
            }

            string firstLocation = null;
            string firstAccession = null;
            if (copies != null && copies.Count > 0)
            {
                var first = copies.OrderBy(c => c.DateAdded).FirstOrDefault();
                if (first != null)
                {
                    firstLocation = first.Location;
                    firstAccession = first.AccessionNumber;
                }
            }

            // Resolve publisher name: prefer navigation property, otherwise load by PublisherID
            string publisherName = book.Publisher?.Name;
            if (string.IsNullOrWhiteSpace(publisherName) && book.PublisherID > 0)
            {
                try
                {
                    var pub = new PublisherRepository().GetById(book.PublisherID);
                    publisherName = pub?.Name;
                }
                catch
                {
                    publisherName = null;
                }
            }

            // Map a stable resource-type label (keep labels short / matching combobox when possible)
            string resourceLabel = null;
            try
            {
                switch (book.ResourceType)
                {
                    case ResourceType.PhysicalBook:
                        resourceLabel = "Book";
                        break;
                    case ResourceType.Periodical:
                        resourceLabel = "Periodical";
                        break;
                    case ResourceType.Thesis:
                        resourceLabel = "Thesis";
                        break;
                    case ResourceType.AV:
                        resourceLabel = "Audio-Visual";
                        break;
                    case ResourceType.EBook:
                        resourceLabel = "E-Book";
                        break;
                    default:
                        resourceLabel = "Unknown";
                        break;
                }
            }
            catch
            {
                resourceLabel = "Unknown";
            }

            return new DTOCatalogBook
            {
                BookID = book.BookID,
                Title = book.Title ?? "Untitled",
                Category = categoryName,
                Author = authorName,
                Status = status,
                CoverImagePath = book.CoverImage,
                DateAdded = dateAdded,
                BorrowCount = 0,
                Authors = authorsAll,
                FirstCopyLocation = firstLocation,
                FirstCopyAccession = firstAccession,

                // STRICT: map ISBN and CallNumber from Book columns only
                ISBN = book.ISBN,
                CallNumber = book.CallNumber,

                // Map Publisher name and PublicationYear explicitly
                Publisher = publisherName,
                PublicationYear = book.PublicationYear,

                // NEW: material format derived from ResourceType / DownloadURL
                MaterialFormat = isDigital ? "Digital" : "Physical",

                // NEW: resource type label
                ResourceType = resourceLabel,

                // NEW: loan type (string stored in Book.LoanType)
                LoanType = string.IsNullOrWhiteSpace(book.LoanType) ? null : book.LoanType.Trim(),

                // NEW: language mapped from book.Language
                Language = string.IsNullOrWhiteSpace(book.Language) ? null : book.Language.Trim()
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

        public List<Author> GetAllAuthors()
        {
            try
            {
                return _authorRepo.GetAll() ?? new List<Author>();
            }
            catch
            {
                return new List<Author>();
            }
        }

        public List<Publisher> GetAllPublishers()
        {
            try
            {
                // lightweight: use repository directly
                return new PublisherRepository().GetAll() ?? new List<Publisher>();
            }
            catch
            {
                return new List<Publisher>();
            }
        }

        public List<Author> GetAuthorsByRole(string role)
        {
            var result = new List<Author>();
            if (string.IsNullOrWhiteSpace(role))
                return result;

            try
            {
                var allBooks = _bookRepo.GetAll() ?? new List<Book>();
                var seen = new HashSet<int>();

                foreach (var book in allBooks)
                {
                    var bas = _bookAuthorRepo.GetByBookId(book.BookID) ?? new List<BookAuthor>();
                    foreach (var ba in bas)
                    {
                        if (ba == null) continue;
                        if (!string.Equals(ba.Role ?? string.Empty, role, StringComparison.OrdinalIgnoreCase)) continue;

                        try
                        {
                            var author = _authorRepo.GetById(ba.AuthorID);
                            if (author != null && !seen.Contains(author.AuthorID))
                            {
                                seen.Add(author.AuthorID);
                                result.Add(author);
                            }
                        }
                        catch
                        {
                            // ignore individual author load failures
                        }
                    }
                }

                // Sort by name for predictable UI order
                result = result.OrderBy(a => a.FullName, StringComparer.CurrentCultureIgnoreCase).ToList();
            }
            catch
            {
                // swallow exceptions and return what we have (empty on failure)
            }

            return result; 
        }

        public List<Author> GetAuthorsByBookIdAndRole(int bookId, string role)
        {
            var result = new List<Author>();
            if (bookId <= 0 || string.IsNullOrWhiteSpace(role))
                return result;

            try
            {
                var bas = _bookAuthorRepo.GetByBookId(bookId) ?? new List<BookAuthor>();
                var seen = new HashSet<int>();
                foreach (var ba in bas)
                {
                    if (ba == null) continue;
                    if (!string.Equals(ba.Role ?? string.Empty, role, StringComparison.OrdinalIgnoreCase)) continue;

                    try
                    {
                        var author = _authorRepo.GetById(ba.AuthorID);
                        if (author != null && !seen.Contains(author.AuthorID))
                        {
                            seen.Add(author.AuthorID);
                            result.Add(author);
                        }
                    }
                    catch
                    {
                        // ignore individual failures
                    }
                }

                result = result.OrderBy(a => a.FullName, StringComparer.CurrentCultureIgnoreCase).ToList();
            }
            catch
            {
                // swallow - return what we have (possibly empty)
            }

            return result;
        }
    }
}
