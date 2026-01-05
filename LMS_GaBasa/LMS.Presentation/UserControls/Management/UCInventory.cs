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
using LMS.Model.Models.Enums;
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
        private readonly CategoryRepository _categoryRepo;

        // simple in-memory cache to avoid repeated DB lookups for category names
        private readonly Dictionary<int, string> _categoryNameCache = new Dictionary<int, string>();

        // Pagination state
        private List<Book> _allBooks;
        private List<Book> _filteredBooks;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        public UCInventory()
        {
            InitializeComponent();

            // Default repository instances (keeps current project style).
            _bookRepo = new BookRepository();
            _bookCopyRepo = new BookCopyRepository();
            _bookAuthor_repo_init();
            _bookAuthorRepo = new BookAuthorRepository();
            _authorRepo = new AuthorRepository();
            _publisherRepo = new PublisherRepository();
            _categoryRepo = new CategoryRepository(); // <- new repo for categories

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

        private void _bookAuthor_repo_init()
        {
            // placeholder to keep style consistent if extra wiring needed later
        }

        private void UCInventory_Load(object sender, EventArgs e)
        {
            // Populate filters first
            SetupFilters();

            // Initialize pagination controls and wire events
            SetupPagination();

            // Wire filter/apply events (ensure single subscription)
            BtnApply.Click -= BtnApply_Click;
            BtnApply.Click += BtnApply_Click;

            TxtSearchBar.TextChanged -= TxtSearchBar_TextChanged;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;

            LoadInventory();
        }

        private void SetupFilters()
        {
            // Category filter
            CmbBxCategoryFilter.Items.Clear();
            CmbBxCategoryFilter.Items.Add("All Category");
            try
            {
                var cats = _categoryRepo.GetAll();
                if (cats != null)
                {
                    foreach (var c in cats)
                    {
                        if (!string.IsNullOrWhiteSpace(c?.Name) && !CmbBxCategoryFilter.Items.Contains(c.Name))
                            CmbBxCategoryFilter.Items.Add(c.Name);
                    }
                }
            }
            catch
            {
                // ignore errors populating categories
            }
            CmbBxCategoryFilter.SelectedIndex = 0;

            // Resource type filter
            CmbBxResourceTypeFilter.Items.Clear();
            CmbBxResourceTypeFilter.Items.Add("All Resource Type");
            try
            {
                foreach (var name in Enum.GetNames(typeof(ResourceType)))
                {
                    CmbBxResourceTypeFilter.Items.Add(name);
                }
            }
            catch
            {
                // ignore if enum not available or error
            }
            CmbBxResourceTypeFilter.SelectedIndex = 0;

            // Status filter
            CmbBxStatusFilter.Items.Clear();
            CmbBxStatusFilter.Items.Add("All Status");
            CmbBxStatusFilter.Items.Add("Available");
            CmbBxStatusFilter.Items.Add("Out of Stock");
            CmbBxStatusFilter.SelectedIndex = 0;
        }

        private void SetupPagination()
        {
            // Ensure the combo has a selected value and wire events
            // Designer sets items and Text; sync _pageSize from Text if possible
            if (!int.TryParse(CmbBxPaginationNumbers.Text, out _pageSize))
                _pageSize = 10;

            // Prevent duplicate event subscriptions by removing first, then adding.
            CmbBxPaginationNumbers.SelectedIndexChanged -= CmbBxPaginationNumbers_SelectedIndexChanged;
            CmbBxPaginationNumbers.SelectedIndexChanged += CmbBxPaginationNumbers_SelectedIndexChanged;

            LblPaginationPrevious.Click -= LblPaginationPrevious_Click;
            LblPaginationPrevious.Click += LblPaginationPrevious_Click;

            LblPaginationNext.Click -= LblPaginationNext_Click;
            LblPaginationNext.Click += LblPaginationNext_Click;
        }

        /// <summary>
        /// Loads all books into memory and applies filters/paging.
        /// </summary>
        private void LoadInventory()
        {
            try
            {
                _allBooks = _bookRepo.GetAll() ?? new List<Book>();
                _currentPage = 1;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load inventory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            if (_allBooks == null)
                _allBooks = new List<Book>();

            var filtered = _allBooks.AsEnumerable();

            // Search text (match Title, ISBN, CallNumber, Authors)
            string searchText = TxtSearchBar.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(searchText) && !string.Equals(searchText, ""))
            {
                string s = searchText;
                filtered = filtered.Where(b =>
                    (!string.IsNullOrEmpty(b.Title) && b.Title.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(b.ISBN) && b.ISBN.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(b.CallNumber) && b.CallNumber.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(GetAuthorsCsv(b.BookID)) && GetAuthorsCsv(b.BookID).IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            // Category filter
            string selectedCategory = CmbBxCategoryFilter.SelectedItem?.ToString() ?? CmbBxCategoryFilter.Text;
            if (!string.IsNullOrWhiteSpace(selectedCategory) && !selectedCategory.Equals("All Category", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(b => GetCategoryName(b.CategoryID).Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));
            }

            // Resource type filter
            string selectedResourceType = CmbBxResourceTypeFilter.SelectedItem?.ToString() ?? CmbBxResourceTypeFilter.Text;
            if (!string.IsNullOrWhiteSpace(selectedResourceType) && !selectedResourceType.Equals("All Resource Type", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(b => b.ResourceType.ToString().Equals(selectedResourceType, StringComparison.OrdinalIgnoreCase));
            }

            // Status filter (use available copies to decide status; for E-Books use DownloadURL)
            string selectedStatus = CmbBxStatusFilter.SelectedItem?.ToString() ?? CmbBxStatusFilter.Text;
            if (!string.IsNullOrWhiteSpace(selectedStatus) && !selectedStatus.Equals("All Status", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(b =>
                {
                    string status;
                    if (b.ResourceType == ResourceType.EBook)
                    {
                        // E-Book is available if a download URL is present; otherwise consider unavailable
                        status = !string.IsNullOrWhiteSpace(b.DownloadURL) ? "Available" : "Out of Stock";
                    }
                    else
                    {
                        var copies = _bookCopyRepo.GetByBookId(b.BookID) ?? new List<BookCopy>();
                        int availableCopies = copies.Count(c => string.Equals(c.Status, "Available", StringComparison.OrdinalIgnoreCase));
                        status = availableCopies > 0 ? "Available" : "Out of Stock";
                    }

                    return status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase);
                });
            }

            _filteredBooks = filtered.ToList();
            CalculatePagination();
            DisplayCurrentPage();
        }

        private void CalculatePagination()
        {
            int totalRecords = _filteredBooks?.Count ?? 0;
            _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
            if (_totalPages == 0) _totalPages = 1;

            if (_currentPage > _totalPages) _currentPage = _totalPages;
            if (_currentPage < 1) _currentPage = 1;

            UpdatePaginationButtons();
        }

        private void DisplayCurrentPage()
        {
            if (_filteredBooks == null)
                _filteredBooks = new List<Book>();

            var paged = _filteredBooks
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            DisplayBooks(paged);
            UpdatePaginationLabel();
            UpdatePaginationButtons();
        }

        /// <summary>
        /// Populates DgwInventory with the provided subset of books.
        /// </summary>
        private void DisplayBooks(List<Book> books)
        {
            DgwInventory.Rows.Clear();

            int startIndex = (_currentPage - 1) * _pageSize;
            int rowNumber = startIndex + 1;

            foreach (var book in books)
            {
                // Authors / Editors / Publisher strings
                string authors = GetAuthorsCsv(book.BookID);
                string editors = GetEditorsCsv(book.BookID);
                string publishers = GetPublisherString(book.PublisherID);

                // Copies -> totals and availability
                // If book is an E-Book treat as having zero copies (no copies allowed)
                bool isEbook = book.ResourceType == ResourceType.EBook;
                var copies = isEbook ? new List<BookCopy>() : _bookCopyRepo.GetByBookId(book.BookID) ?? new List<BookCopy>();
                int totalCopies = copies.Count;
                int availableCopies = copies.Count(c => string.Equals(c.Status, "Available", StringComparison.OrdinalIgnoreCase));

                // Determine status: for e-books rely on DownloadURL; for physical use copies
                string status;
                if (isEbook)
                {
                    status = !string.IsNullOrWhiteSpace(book.DownloadURL) ? "Available" : "Out of Stock";
                }
                else
                {
                    status = availableCopies > 0 ? "Available" : "Out of Stock";
                }

                // Book ID formatted (INV-0001)
                string formattedBookId = $"INV-{book.BookID:D4}";

                // Category: use CategoryID and cache lookup (Book.Category may be null because repository doesn't populate navigation)
                string categoryName = GetCategoryName(book.CategoryID);

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
                SetCellValue(row, "ColumnCategory", categoryName);
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

                // Visually disable the "View Copies" button for e-books:
                if (DgwInventory.Columns.Contains("ColumnBtnCopies"))
                {
                    var cell = row.Cells["ColumnBtnCopies"];
                    if (isEbook)
                    {
                        // mark cell read-only and gray it out; clicking will be ignored in CellContentClick handler
                        cell.ReadOnly = true;
                        cell.Style.ForeColor = Color.Gray;
                        cell.Value = "N/A";
                    }
                    else
                    {
                        cell.ReadOnly = false;
                        cell.Style.ForeColor = Color.Black;
                        cell.Value = "View Copies";
                    }
                }

                rowNumber++;
            }
        }

        private void UpdatePaginationLabel()
        {
            int totalRecords = _filteredBooks?.Count ?? 0;
            int startRecord = totalRecords == 0 ? 0 : ((_currentPage - 1) * _pageSize) + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);

            LblPaginationShowEntries.Text = $"Showing {startRecord} to {endRecord} of {totalRecords} entries";
        }

        private void UpdatePaginationButtons()
        {
            LblPaginationPrevious.Enabled = _currentPage > 1;
            LblPaginationNext.Enabled = _currentPage < _totalPages;
        }

        private void CmbBxPaginationNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = CmbBxPaginationNumbers.SelectedItem?.ToString() ?? CmbBxPaginationNumbers.Text;
            if (int.TryParse(selectedValue, out int pageSize))
            {
                _pageSize = pageSize;
                _currentPage = 1;
                CalculatePagination();
                DisplayCurrentPage();
            }
        }

        private void LblPaginationPrevious_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                DisplayCurrentPage();
            }
        }

        private void LblPaginationNext_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                DisplayCurrentPage();
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            ApplyFilters();
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            ApplyFilters();
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
                // Do not open ViewBookCopy for e-books
                if (book.ResourceType == ResourceType.EBook)
                {
                    MessageBox.Show("This is an E-Book and has no physical copies.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Open view copies form. For now just show the form and pass book id as TODO later
                using (var view = new ViewBookCopy())
                {
                    // TODO: pass book.BookID to ViewBookCopy when that form accepts it.
                    view.ShowDialog();
                }
            }
            else if (colName == "Edit")
            {
                // Open EditBook form with the Book object and refresh grid if user saved changes
                try
                {
                    using (var editForm = new EditBook(book))
                    {
                        var result = editForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            LoadInventory();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        // Category lookup with caching. Returns empty string when not found or id <= 0.
        private string GetCategoryName(int categoryId)
        {
            if (categoryId <= 0) return string.Empty;

            if (_categoryNameCache.TryGetValue(categoryId, out string cached)) return cached ?? string.Empty;

            try
            {
                var cat = _categoryRepo.GetById(categoryId);
                var name = cat?.Name ?? string.Empty;
                _categoryNameCache[categoryId] = name;
                return name;
            }
            catch
            {
                _categoryNameCache[categoryId] = string.Empty;
                return string.Empty;
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

        // end code
    }
}
