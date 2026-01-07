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

            // Ensure DataGridView shows cell tooltips for Standard ID column
            if (DgwInventory != null)
                DgwInventory.ShowCellToolTips = true;

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
            CmbBxStatusFilter.Items.Add("Available Online"); // <- added: digital availability
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

            // Search text (match Title, ISBN, CallNumber, Authors, Category (subject), Publisher, Year, Description, AccessionNumber)
            string searchText = TxtSearchBar.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(searchText))
            {
                string s = searchText;
                filtered = filtered.Where(b =>
                {
                    // Title
                    if (!string.IsNullOrEmpty(b.Title) && b.Title.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // ISBN
                    if (!string.IsNullOrEmpty(b.ISBN) && b.ISBN.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Call Number
                    if (!string.IsNullOrEmpty(b.CallNumber) && b.CallNumber.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Subtitle / PhysicalDescription (treat as subject/description)
                    if (!string.IsNullOrEmpty(b.Subtitle) && b.Subtitle.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                    if (!string.IsNullOrEmpty(b.PhysicalDescription) && b.PhysicalDescription.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Publication Year (allow numeric or partial match)
                    if (b.PublicationYear > 0 && b.PublicationYear.ToString().IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Authors
                    var authorsCsv = GetAuthorsCsv(b.BookID);
                    if (!string.IsNullOrEmpty(authorsCsv) && authorsCsv.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Editors (in case user searches editor names)
                    var editorsCsv = GetEditorsCsv(b.BookID);
                    if (!string.IsNullOrEmpty(editorsCsv) && editorsCsv.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Advisers (search advisers too)
                    var advisersCsv = GetAdvisersCsv(b.BookID);
                    if (!string.IsNullOrEmpty(advisersCsv) && advisersCsv.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Category / Subject
                    var categoryName = GetCategoryName(b.CategoryID);
                    if (!string.IsNullOrEmpty(categoryName) && categoryName.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Publisher
                    var publisherName = GetPublisherString(b.PublisherID);
                    if (!string.IsNullOrEmpty(publisherName) && publisherName.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;

                    // Accession number (search copies for matching accession)
                    try
                    {
                        // For e-books / digital resources there are no physical copies; for others check copies
                        if (b.ResourceType != ResourceType.EBook && string.IsNullOrWhiteSpace(b.DownloadURL))
                        {
                            var copies = _bookCopyRepo.GetByBookId(b.BookID) ?? new List<BookCopy>();
                            if (copies.Any(c => !string.IsNullOrWhiteSpace(c.AccessionNumber) &&
                                                c.AccessionNumber.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        // ignore copy lookup errors and continue
                    }

                    return false;
                });
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

            // Status filter (use available copies to decide status; for digital resources use DownloadURL -> Available Online)
            string selectedStatus = CmbBxStatusFilter.SelectedItem?.ToString() ?? CmbBxStatusFilter.Text;
            if (!string.IsNullOrWhiteSpace(selectedStatus) && !selectedStatus.Equals("All Status", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(b =>
                {
                    string status;
                    bool isDigital = b.ResourceType == ResourceType.EBook || !string.IsNullOrWhiteSpace(b.DownloadURL);

                    if (isDigital)
                    {
                        // Digital resources -> Available Online when DownloadURL present
                        status = !string.IsNullOrWhiteSpace(b.DownloadURL) ? "Available Online" : "Out of Stock";
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
                string advisers = GetAdvisersCsv(book.BookID);
                string publishers = GetPublisherString(book.PublisherID);

                // Copies -> totals and availability
                // A resource is considered digital if it's an E-Book or has a DownloadURL (periodical/thesis/AV digital)
                bool isDigital = book.ResourceType == ResourceType.EBook || !string.IsNullOrWhiteSpace(book.DownloadURL);

                var copies = (isDigital) ? new List<BookCopy>() : _bookCopyRepo.GetByBookId(book.BookID) ?? new List<BookCopy>();
                int totalCopies = copies.Count;
                int availableCopies = copies.Count(c => string.Equals(c.Status, "Available", StringComparison.OrdinalIgnoreCase));

                // Determine status:
                // - Digital with DownloadURL => "Available Online"
                // - Digital without DownloadURL => "Out of Stock"
                // - Physical => "Available" if at least one available else "Out of Stock"
                string status;
                if (isDigital)
                {
                    status = !string.IsNullOrWhiteSpace(book.DownloadURL) ? "Available Online" : "Out of Stock";
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

                // Set tooltip for the Standard ID column according to resource type:
                // - PhysicalBook -> ISBN
                // - Thesis -> DOI
                // - Periodical -> ISSN
                // - AV -> UPC/ISAN
                // - EBook -> ISBN
                if (DgwInventory.Columns.Contains("ColumnISBN"))
                {
                    try
                    {
                        var idCell = row.Cells["ColumnISBN"];
                        string idTooltipLabel;
                        switch (book.ResourceType)
                        {
                            case ResourceType.Thesis:
                                idTooltipLabel = "DOI";
                                break;
                            case ResourceType.Periodical:
                                idTooltipLabel = "ISSN";
                                break;
                            case ResourceType.AV:
                                idTooltipLabel = "UPC/ISAN";
                                break;
                            case ResourceType.EBook:
                            case ResourceType.PhysicalBook:
                            default:
                                idTooltipLabel = "ISBN";
                                break;
                        }

                        // Always set the tooltip label so hovering the Standard ID cell clarifies what the value represents.
                        idCell.ToolTipText = idTooltipLabel;
                    }
                    catch
                    {
                        // ignore tooltip setting errors
                    }
                }

                SetCellValue(row, "ColumnCallNo", book.CallNumber);
                SetCellValue(row, "ColumnTitle", book.Title);
                SetCellValue(row, "ColumnSubtitle", book.Subtitle);
                SetCellValue(row, "ColumnAuthors", authors);
                SetCellValue(row, "ColumnEditors", editors);
                SetCellValue(row, "ColumnAdvisers", advisers); // new adviser column
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

                // Copies / status fields - apply digital/physical rules per your spec
                if (isDigital)
                {
                    // For digital resources show N/A for copy counts and "Available Online" (when URL present)
                    SetCellValue(row, "ColumnTotalCopies", "N/A");
                    SetCellValue(row, "ColumnAvailableCopies", "N/A");
                    SetCellValue(row, "ColumnStatus", status);
                }
                else
                {
                    SetCellValue(row, "ColumnTotalCopies", totalCopies.ToString());
                    SetCellValue(row, "ColumnAvailableCopies", availableCopies.ToString());
                    SetCellValue(row, "ColumnStatus", status);
                }

                // Buttons - store Book object on the row Tag so click handlers can find the right book
                row.Tag = book;

                // Visually disable the "View Copies" button for digital resources:
                if (DgwInventory.Columns.Contains("ColumnBtnCopies"))
                {
                    var cell = row.Cells["ColumnBtnCopies"];
                    if (isDigital)
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

        /// <summary>
        /// Returns comma-separated adviser full names for a book (Role = "Adviser")
        /// </summary>
        private string GetAdvisersCsv(int bookId)
        {
            try
            {
                var bookAuthors = _bookAuthorRepo.GetByBookId(bookId);
                if (bookAuthors == null || bookAuthors.Count == 0) return string.Empty;

                var names = new List<string>();
                foreach (var ba in bookAuthors.Where(x => string.Equals(x.Role, "Adviser", StringComparison.OrdinalIgnoreCase)))
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
                // Treat as digital if E-Book OR resource has a DownloadURL (digital periodical/thesis/AV)
                bool isDigital = book.ResourceType == ResourceType.EBook || !string.IsNullOrWhiteSpace(book.DownloadURL);

                if (isDigital)
                {
                    MessageBox.Show("This book is a digital resource. Copy information is not applicable.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Open view copies form and pass book id and title.
                try
                {
                    using (var view = new ViewBookCopy(book.BookID, book.Title))
                    {
                        var result = view.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            // Reload inventory so the main grid reflects edits done in the copy view
                            LoadInventory();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open copies view: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Helper: set cell value only if the column exists (keeps code robust to designer changes).
        // All empty/null values are displayed as "N/A" per requirement.
        private void SetCellValue(DataGridViewRow row, string columnName, object value)
        {
            if (DgwInventory.Columns.Contains(columnName))
            {
                var s = value?.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(s))
                    s = "N/A";
                row.Cells[columnName].Value = s;
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
