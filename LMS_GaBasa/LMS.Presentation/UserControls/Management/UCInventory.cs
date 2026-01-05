using LMS.Presentation.Popup.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.DataAccess.Repositories;
using LMS.DataAccess.Database;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using System.IO;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCInventory : UserControl
    {
        // Repositories (presentation constructs concrete repos; can be switched to DI later)
        private readonly BookRepository _bookRepo;
        private readonly BookCopyRepository _bookCopyRepo;
        private readonly BookAuthorRepository _bookAuthorRepo;
        private readonly AuthorRepository _authorRepo;
        private readonly PublisherRepository _publisherRepo;

        public UCInventory()
        {
            InitializeComponent();

            // Default repository instances (keeps current project style).
            _bookRepo = new BookRepository();
            _bookCopyRepo = new BookCopyRepository();
            _bookAuthorRepo = new BookAuthorRepository();
            _authorRepo = new AuthorRepository();
            _publisherRepo = new PublisherRepository();

            // Ensure button columns show their text (designer set Text but not UseColumnTextForButtonValue)
            if (DgwInventory.Columns.Contains("ColumnBtnCoverImage"))
            {
                var col = DgwInventory.Columns["ColumnBtnCoverImage"] as DataGridViewButtonColumn;
                if (col != null) col.UseColumnTextForButtonValue = true;
            }

            if (DgwInventory.Columns.Contains("ColumnBtnCopies"))
            {
                var col = DgwInventory.Columns["ColumnBtnCopies"] as DataGridViewButtonColumn;
                if (col != null) col.UseColumnTextForButtonValue = true;
            }

            // Wire load
            this.Load += UCInventory_Load;
        }

        private void UCInventory_Load(object sender, EventArgs e)
        {
            LoadInventory();
        }

        /// <summary>
        /// Loads all books and populates the grid with formatted values.
        /// Keeps mapping logic in small helper methods so class follows SRP and is testable.
        /// </summary>
        private void LoadInventory()
        {
            try
            {
                DgwInventory.Rows.Clear();

                var books = _bookRepo.GetAll() ?? new List<Book>();
                int rowNumber = 1;

                foreach (var book in books)
                {
                    // Authors / Editors / Publisher strings
                    string authors = GetAuthorsCsv(book.BookID);
                    string editors = GetEditorsCsv(book.BookID);
                    string publishers = GetPublisherString(book.PublisherID);

                    // Copies -> totals and availability
                    var copies = _bookCopyRepo.GetByBookId(book.BookID) ?? new List<BookCopy>();
                    int totalCopies = copies.Count;
                    int availableCopies = copies.Count(c => string.Equals(c.Status, "Available", StringComparison.OrdinalIgnoreCase));
                    string status = availableCopies > 0 ? "Available" : "Out of Stock";

                    // Book ID formatted (INV-0001)
                    string formattedBookId = $"INV-{book.BookID:D4}";

                    // Add to grid (use column names to stay resilient to column order)
                    int rowIndex = DgwInventory.Rows.Add();
                    var row = DgwInventory.Rows[rowIndex];

                    // Standard fields
                    SetCellValue(row, "ColumnNumbering", rowNumber.ToString());
                    SetCellValue(row, "ColumnBookID", formattedBookId);
                    SetCellValue(row, "ColumnISBN", book.ISBN);
                    SetCellValue(row, "ColumnCallNo", book.CallNumber);
                    SetCellValue(row, "ColumnTitle", book.Title);
                    SetCellValue(row, "ColumnSubtitle", book.Subtitle);
                    SetCellValue(row, "ColumnAuthors", authors);
                    SetCellValue(row, "ColumnEditors", editors);
                    SetCellValue(row, "ColumnPublishers", publishers);
                    SetCellValue(row, "ColumnCategory", book.Category?.Name);
                    SetCellValue(row, "ColumnLanguage", book.Language);
                    SetCellValue(row, "ColumnPages", book.Pages > 0 ? book.Pages.ToString() : string.Empty);
                    SetCellValue(row, "ColumnEdition", book.Edition);
                    SetCellValue(row, "ColumnPublicationYear", book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty);
                    SetCellValue(row, "ColumnDescription", book.PhysicalDescription);
                    SetCellValue(row, "ColumnResourceType", book.ResourceType.ToString());
                    SetCellValue(row, "ColumnDLURL", book.DownloadURL);
                    SetCellValue(row, "ColumnLoanType", book.LoanType);

                    // Copies / status fields
                    SetCellValue(row, "ColumnTotalCopies", totalCopies.ToString());
                    SetCellValue(row, "ColumnAvailableCopies", availableCopies.ToString());
                    SetCellValue(row, "ColumnStatus", status);

                    // Buttons - store Book object on the row Tag so click handlers can find the right book
                    row.Tag = book;

                    // The button columns will display text due to UseColumnTextForButtonValue being enabled earlier

                    rowNumber++;
                }
            }
            catch (Exception ex)
            {
                // Fail gracefully in UI, but show error for debugging
                MessageBox.Show($"Failed to load inventory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper: set cell value only if the column exists (keeps code robust to designer changes)
        private void SetCellValue(DataGridViewRow row, string columnName, object value)
        {
            if (DgwInventory.Columns.Contains(columnName))
            {
                row.Cells[columnName].Value = value ?? string.Empty;
            }
        }

        // Returns comma-separated author full names for a book (Author role)
        private string GetAuthorsCsv(int bookId)
        {
            try
            {
                var bookAuthors = _bookAuthorRepo.GetByBookId(bookId);
                if (bookAuthors == null || bookAuthors.Count == 0) return string.Empty;

                var names = new List<string>();
                foreach (var ba in bookAuthors.Where(x => string.Equals(x.Role, "Author", StringComparison.OrdinalIgnoreCase)))
                {
                    var author = _authorRepo.GetById(ba.AuthorID);
                    if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                        names.Add(author.FullName.Trim());
                }

                // If none marked as role Author, fallback to list all related authors
                if (names.Count == 0)
                {
                    foreach (var ba in bookAuthors)
                    {
                        var author = _authorRepo.GetById(ba.AuthorID);
                        if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                            names.Add(author.FullName.Trim());
                    }
                }

                return string.Join(", ", names.Distinct(StringComparer.OrdinalIgnoreCase));
            }
            catch
            {
                return string.Empty;
            }
        }

        // Returns comma-separated editor names for a book
        private string GetEditorsCsv(int bookId)
        {
            try
            {
                var bookAuthors = _bookAuthorRepo.GetByBookId(bookId);
                if (bookAuthors == null || bookAuthors.Count == 0) return string.Empty;

                var names = new List<string>();
                foreach (var ba in bookAuthors.Where(x => string.Equals(x.Role, "Editor", StringComparison.OrdinalIgnoreCase)))
                {
                    var author = _authorRepo.GetById(ba.AuthorID);
                    if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                        names.Add(author.FullName.Trim());
                }

                return string.Join(", ", names.Distinct(StringComparer.OrdinalIgnoreCase));
            }
            catch
            {
                return string.Empty;
            }
        }

        // Returns publisher name(s). Currently Book has a single PublisherID; return its Name.
        private string GetPublisherString(int publisherId)
        {
            try
            {
                if (publisherId <= 0) return string.Empty;
                var pub = _publisher_repo_getbyid_safe(publisherId);
                return pub?.Name ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        // small wrapper to avoid repeating try/catch style elsewhere and handle repo null safety
        private Publisher _publisher_repo_getbyid_safe(int publisherId)
        {
            try
            {
                return _publisherRepo.GetById(publisherId);
            }
            catch
            {
                return null;
            }
        }

        private void BtnAddBook_Click(object sender, EventArgs e)
        {
            AddBook addBookForm = new AddBook();
            if (addBookForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh grid after adding a book
                LoadInventory();
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            ImportBook importBookForm = new ImportBook();
            importBookForm.ShowDialog();
        }

        private void LblPaginationPrevious_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle clicks on the DataGridView's button/image columns.
        /// For now we only open the relevant forms (ViewCoverImage / ViewBookCopy) as requested.
        /// Row.Tag contains the Book object we stored earlier.
        /// </summary>
        private void DgwInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var column = DgwInventory.Columns[e.ColumnIndex];
            var row = DgwInventory.Rows[e.RowIndex];
            if (row == null) return;

            var book = row.Tag as Book;
            if (book == null) return;

            // Identify columns by name (designer names)
            string colName = column.Name;

            if (colName == "ColumnBtnCoverImage")
            {
                // Build full path from stored relative cover image if available
                string fullPath = null;
                if (!string.IsNullOrWhiteSpace(book.CoverImage))
                {
                    try
                    {
                        // CoverImage stored as relative path like "Assets/dataimages/Books/xxx.jpg"
                        fullPath = Path.Combine(Application.StartupPath, book.CoverImage.Replace('/', Path.DirectorySeparatorChar));
                        if (!File.Exists(fullPath))
                            fullPath = null;
                    }
                    catch
                    {
                        fullPath = null;
                    }
                }

                // Open cover image viewer with image path and book title
                using (var view = new ViewCoverImage())
                {
                    view.LoadCover(fullPath, book.Title);
                    view.ShowDialog();
                }
            }
            else if (colName == "ColumnBtnCopies")
            {
                // Open view copies form. For now just show the form and pass book id as TODO later
                using (var view = new ViewBookCopy())
                {
                    // TODO: pass book.BookID to ViewBookCopy when that form accepts it.
                    view.ShowDialog();
                }
            }
            else if (colName == "Edit")
            {
                // Edit icon clicked - future implementation
                MessageBox.Show("Edit not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // end code
    }
}
