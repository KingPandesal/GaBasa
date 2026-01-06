using LMS.BusinessLogic.Factories;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Book;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.BusinessLogic.Services.Book.AddBook
{
    /// <summary>
    /// Service responsible for creating book records and their initial copies.
    /// Ensures accession numbers are generated and assigned before inserting BookCopy rows.
    /// </summary>
    public class AddBookService : IAddBookService
    {
        private readonly BookRepository _bookRepo;
        private readonly AuthorRepository _authorRepo;
        private readonly BookAuthorRepository _bookAuthorRepo;
        private readonly BookCopyRepository _bookCopyRepo;
        private readonly IBookFactory _bookFactory;
        private readonly IPublisherRepository _publisherRepo;

        public AddBookService(
            BookRepository bookRepo,
            AuthorRepository authorRepo,
            BookAuthorRepository bookAuthorRepo,
            BookCopyRepository bookCopyRepo,
            IBookFactory bookFactory,
            IPublisherRepository publisherRepo)
        {
            _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
            _authorRepo = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));
            _bookAuthorRepo = bookAuthorRepo ?? throw new ArgumentNullException(nameof(bookAuthorRepo));
            _bookCopyRepo = bookCopyRepo ?? throw new ArgumentNullException(nameof(bookCopyRepo));
            _bookFactory = bookFactory ?? throw new ArgumentNullException(nameof(bookFactory));
            _publisherRepo = publisherRepo ?? throw new ArgumentNullException(nameof(publisherRepo));
        }

        /// <summary>
        /// Creates the Book (main row) and the initial BookCopy rows.
        /// Generates accession numbers in the format:
        ///   {PREFIX}-{BookID}-{Year}-{NNNN}
        /// where PREFIX is based on resource type (BK, EB, PR, TH, AV).
        /// Returns BookCreationResultService containing created BookID and list of accession numbers.
        /// </summary>
        public BookCreationResultService CreateBook(DTOCreateBook dto)
        {
            if (dto == null) return BookCreationResultService.Fail("Book data is null.");

            try
            {
                // 1) Build Book entity from factory based on resource type, then map DTO fields onto it.
                var book = _bookFactory.Create(dto.ResourceType);

                // Map common properties (derived Book classes are expected to expose these properties)
                // Use null-checks/trim to avoid passing nulls where DB expects empty strings.
                // These property names should exist on the Book model or its derived types.
                book.ResourceType = dto.ResourceType;
                book.LoanType = dto.LoanType;
                // Most Book implementations in this project expose these properties; set by name:
                // (If some properties are missing on a concrete class, compilation will show the missing members
                // and you can adjust to the concrete model.)
                dynamic dynBook = book;
                dynBook.ISBN = dto.ISBN ?? string.Empty;
                dynBook.Title = dto.Title ?? string.Empty;
                dynBook.Subtitle = dto.Subtitle ?? string.Empty;

                // IMPORTANT: Book.Publisher is a Publisher object in the model.
                // Do NOT assign the publisher name (string) to that property.
                // Instead assign PublisherID (already resolved in BuildDTOFromForm).
                dynBook.PublisherID = dto.PublisherID;

                // NOTE: PhysicalBook (and other concrete Book types) may not expose a CategoryName property.
                // We only assign the foreign key CategoryID here; the name is handled by CatalogManager when creating/getting categories.
                dynBook.CategoryID = dto.CategoryID;
                dynBook.Language = dto.Language ?? string.Empty;
                dynBook.Pages = dto.Pages;
                dynBook.Edition = dto.Edition ?? string.Empty;
                dynBook.PublicationYear = dto.PublicationYear;
                dynBook.PhysicalDescription = dto.PhysicalDescription ?? string.Empty;
                dynBook.CallNumber = dto.CallNumber ?? string.Empty;
                dynBook.CoverImage = dto.CoverImage ?? string.Empty;

                // 2) Persist Book and get BookID
                int bookId = _bookRepo.Add(book);
                if (bookId <= 0)
                    return BookCreationResultService.Fail("Failed to insert Book record.");

                // 3) Persist authors/editors relations (by name -> ensure author exists then add BookAuthor)
                foreach (var a in dto.Authors ?? Enumerable.Empty<DTOBookAuthor>())
                {
                    var authorId = EnsureAuthorExists(a.AuthorName);
                    if (authorId > 0)
                    {
                        var ba = new BookAuthor
                        {
                            BookID = bookId,
                            AuthorID = authorId,
                            Role = a.Role ?? "Author",
                            IsPrimaryAuthor = a.IsPrimaryAuthor
                        };
                        _bookAuthorRepo.Add(ba);
                    }
                }
                foreach (var e in dto.Editors ?? Enumerable.Empty<DTOBookAuthor>())
                {
                    var authorId = EnsureAuthorExists(e.AuthorName);
                    if (authorId > 0)
                    {
                        var ba = new BookAuthor
                        {
                            BookID = bookId,
                            AuthorID = authorId,
                            Role = e.Role ?? "Editor",
                            IsPrimaryAuthor = false
                        };
                        _bookAuthorRepo.Add(ba);
                    }
                }

                // 4) Create initial copies if requested - ensure accession numbers are generated BEFORE Add(BookCopy)
                var createdAccessions = new List<string>();
                int copiesToCreate = Math.Max(0, dto.InitialCopyCount);
                if (copiesToCreate > 0)
                {
                    // Get prefix for resource type
                    var prefix = _bookCopyRepo.GetPrefixForResourceType(dto.ResourceType);

                    var year = DateTime.Now.Year;

                    // Get existing accession numbers for this book/year to compute next suffix safely
                    var existingCopies = _bookCopyRepo.GetByBookId(bookId) ?? new List<BookCopy>();
                    var existingForYear = existingCopies
                        .Select(c => c.AccessionNumber ?? string.Empty)
                        .Where(s => s.IndexOf($"-{bookId}-{year}-", StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                    // Determine starting suffix (max existing suffix + 1)
                    int maxSuffix = 0;
                    foreach (var acc in existingForYear)
                    {
                        var parts = acc.Split('-');
                        if (parts.Length >= 4)
                        {
                            if (int.TryParse(parts.Last(), out int v))
                                maxSuffix = Math.Max(maxSuffix, v);
                        }
                    }

                    int nextSuffix = maxSuffix + 1;

                    for (int i = 0; i < copiesToCreate; i++)
                    {
                        var accession = $"{prefix}-{bookId}-{year}-{nextSuffix:D4}";
                        nextSuffix++;

                        // Build BookCopy (set barcode filename placeholder here - actual barcode image may be generated later)
                        var copy = new BookCopy
                        {
                            BookID = bookId,
                            AccessionNumber = accession,
                            Status = string.IsNullOrWhiteSpace(dto.CopyStatus) ? "Available" : dto.CopyStatus,
                            Location = dto.CopyLocation,
                            Barcode = $"{accession}.png", // store filename; presentation barcode generator will create image at expected folder
                            DateAdded = DateTime.UtcNow,
                            AddedByID = dto.AddedByID
                        };

                        int newCopyId = _bookCopyRepo.Add(copy);

                        // If repository returns 0 (failed) - still accumulate accession for reporting; but treat as failure
                        if (newCopyId <= 0)
                        {
                            // Rollback strategy could be implemented here. For now return failure.
                            return BookCreationResultService.Fail($"Failed to create book copy for accession {accession}.");
                        }

                        createdAccessions.Add(accession);
                    }
                }

                // 5) Success
                return BookCreationResultService.Ok(bookId, createdAccessions);
            }
            catch (Exception ex)
            {
                return BookCreationResultService.Fail("Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Ensures an author with the given full name exists; returns AuthorID or 0 on failure.
        /// </summary>
        private int EnsureAuthorExists(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return 0;

            try
            {
                var existing = _authorRepo.GetByName(fullName.Trim());
                if (existing != null) return existing.AuthorID;

                var newAuthor = new Model.Models.Catalog.Author { FullName = fullName.Trim() };
                return _authorRepo.Add(newAuthor);
            }
            catch
            {
                return 0;
            }
        }
    }
}
