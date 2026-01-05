using LMS.BusinessLogic.Factories;
using LMS.BusinessLogic.Services.BarcodeGenerator;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Book;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using BookModel = LMS.Model.Models.Catalog.Books.Book;

namespace LMS.BusinessLogic.Services.Book.AddBook
{
    public class AddBookService : IAddBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IAuthorRepository _authorRepo;
        private readonly IBookAuthorRepository _bookAuthorRepo;
        private readonly IBookCopyRepository _bookCopyRepo;
        private readonly IBookFactory _bookFactory;
        private readonly IPublisherRepository _publisherRepo;

        public AddBookService(
            IBookRepository bookRepo,
            IAuthorRepository authorRepo,
            IBookAuthorRepository bookAuthorRepo,
            IBookCopyRepository bookCopyRepo,
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

        public BookCreationResultService CreateBook(DTOCreateBook dto)
        {
            // ===== VALIDATION =====
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BookCreationResultService.Fail("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                return BookCreationResultService.Fail("ISBN is required.");

            if (_bookRepo.ISBNExists(dto.ISBN))
                return BookCreationResultService.Fail("A book with this ISBN already exists.");

            if (!string.IsNullOrWhiteSpace(dto.CallNumber) && _bookRepo.CallNumberExists(dto.CallNumber))
                return BookCreationResultService.Fail("A book with this call number already exists.");

            // map publisher name -> id if needed
            EnsurePublisherId(dto);

            if (dto.CategoryID <= 0)
                return BookCreationResultService.Fail("Please select a category.");

            if (dto.Authors == null || !dto.Authors.Any())
                return BookCreationResultService.Fail("At least one author is required.");

            // ===== CREATE BOOK =====
            BookModel book = _bookFactory.Create(dto.ResourceType);
            MapDtoToBook(dto, book);

            int bookId = _bookRepo.Add(book);
            if (bookId <= 0)
                return BookCreationResultService.Fail("Failed to save the book.");

            // ===== HANDLE AUTHORS =====
            foreach (var authorDto in dto.Authors)
            {
                int authorId = GetOrCreateAuthor(authorDto);
                if (authorId > 0)
                {
                    _bookAuthorRepo.Add(new BookAuthor
                    {
                        BookID = bookId,
                        AuthorID = authorId,
                        Role = "Author",
                        IsPrimaryAuthor = authorDto.IsPrimaryAuthor
                    });
                }
            }

            // ===== HANDLE EDITORS =====
            if (dto.Editors != null)
            {
                foreach (var editorDto in dto.Editors)
                {
                    int editorId = GetOrCreateAuthor(editorDto);
                    if (editorId > 0)
                    {
                        _bookAuthorRepo.Add(new BookAuthor
                        {
                            BookID = bookId,
                            AuthorID = editorId,
                            Role = "Editor",
                            IsPrimaryAuthor = false
                        });
                    }
                }
            }

            // ===== CREATE INITIAL COPIES =====
            int copyCount = Math.Max(1, dto.InitialCopyCount);
            var createdAccessions = new List<string>();

            DateTime dateAdded = DateTime.Now;
            int addedBy = dto.AddedByID;

            const int maxRetries = 3;

            // After earlier validation and after creating the book (or earlier if you prefer)
            if (dto.AddedByID <= 0)
                return BookCreationResultService.Fail("AddedByID (current user) is required to add book copies.");

            for (int i = 0; i < copyCount; i++)
            {
                bool saved = false;
                int attempt = 0;
                Exception lastEx = null;

                while (!saved && attempt < maxRetries)
                {
                    attempt++;
                    try
                    {
                        // Pre-generate accession (repository legacy method) and supply it on INSERT
                        string accessionNumber = _bookCopyRepo.GenerateAccessionNumber(bookId, dateAdded);

                        var copy = new BookCopy
                        {
                            BookID = bookId,
                            AccessionNumber = accessionNumber, // must be NOT NULL for DB
                            Status = string.IsNullOrWhiteSpace(dto.CopyStatus) ? "Available" : dto.CopyStatus,
                            Location = string.IsNullOrWhiteSpace(dto.CopyLocation) ? "Main Library" : dto.CopyLocation,
                            Barcode = null,
                            DateAdded = dateAdded,
                            AddedByID = addedBy
                        };

                        int newId = _bookCopyRepo.Add(copy);
                        if (newId <= 0)
                            throw new InvalidOperationException("Failed to insert book copy.");

                        createdAccessions.Add(accessionNumber);
                        saved = true;
                    }
                    catch (Exception ex)
                    {
                        // Likely unique constraint collision on AccessionNumber — retry.
                        lastEx = ex;
                        Trace.TraceWarning($"Attempt {attempt} to add book copy failed for BookID={bookId}: {ex.Message}");
                        System.Threading.Thread.Sleep(50 * attempt);
                    }
                }

                if (!saved)
                {
                    Trace.TraceError($"Failed to add copy for BookID={bookId} after {maxRetries} attempts. Last error: {lastEx?.Message}");
                    // Return the exact exception text (stack trace included) to the caller for diagnostics.
                    return BookCreationResultService.Fail(lastEx?.ToString() ?? "Failed to create book copies due to concurrency. Try again.");
                }
            }

            return BookCreationResultService.Ok(bookId, createdAccessions);
        }

        private void EnsurePublisherId(DTOCreateBook dto)
        {
            if (dto == null) return;

            if (dto.PublisherID > 0) return;

            if (string.IsNullOrWhiteSpace(dto.Publisher)) return;

            // Try to find by name (case-insensitive) from repository
            var all = _publisherRepo.GetAll();
            var match = all?.FirstOrDefault(p => string.Equals(p.Name?.Trim(), dto.Publisher.Trim(), StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                dto.PublisherID = match.PublisherID;
                return;
            }

            // Not found -> create new publisher record and set id
            try
            {
                var newPub = new Publisher
                {
                    Name = dto.Publisher.Trim(),
                    Address = null,
                    ContactNumber = null
                };
                int newId = _publisherRepo.Add(newPub);
                if (newId > 0)
                    dto.PublisherID = newId;
            }
            catch
            {
                // If creation fails, keep PublisherID = 0.
                // The repository insertion can fail due to DB constraints — let the caller handle error.
            }
        }

        private int GetOrCreateAuthor(DTOBookAuthor authorDto)
        {
            int authorId = authorDto.AuthorID;

            if (authorId <= 0 && !string.IsNullOrWhiteSpace(authorDto.AuthorName))
            {
                var existingAuthor = _authorRepo.GetByName(authorDto.AuthorName.Trim());
                if (existingAuthor != null)
                {
                    authorId = existingAuthor.AuthorID;
                }
                else
                {
                    authorId = _authorRepo.Add(new Author { FullName = authorDto.AuthorName.Trim() });
                }
            }

            return authorId;
        }

        private void MapDtoToBook(DTOCreateBook dto, BookModel book)
        {
            book.ISBN = dto.ISBN?.Trim();
            book.CallNumber = dto.CallNumber?.Trim();
            book.Title = dto.Title?.Trim();
            book.Subtitle = dto.Subtitle?.Trim();
            book.PublisherID = dto.PublisherID;
            book.CategoryID = dto.CategoryID;
            book.Language = dto.Language?.Trim();
            book.Pages = dto.Pages;
            book.Edition = dto.Edition?.Trim();
            book.PublicationYear = dto.PublicationYear;
            book.PhysicalDescription = dto.PhysicalDescription?.Trim();
            book.ResourceType = dto.ResourceType;
            book.CoverImage = dto.CoverImage;
            book.LoanType = dto.LoanType;
            book.DownloadURL = dto.DownloadURL;
        }
    }
}
