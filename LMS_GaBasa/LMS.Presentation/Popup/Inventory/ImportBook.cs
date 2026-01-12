using LMS.DataAccess.Repositories;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.Model.Models.Enums;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class ImportBook : Form
    {
        private string _selectedFilePath;
        private readonly BookRepository _bookRepo;
        private readonly PublisherRepository _publisherRepo;
        private readonly CategoryRepository _category_repo;
        private readonly AuthorRepository _authorRepo;
        private readonly BookAuthorRepository _bookAuthorRepo;

        public ImportBook()
        {
            InitializeComponent();

            _bookRepo = new BookRepository();
            _publisherRepo = new PublisherRepository();
            _category_repo = new CategoryRepository();
            _authorRepo = new AuthorRepository();
            _bookAuthorRepo = new BookAuthorRepository();

            // Wire events
            lostBorderPanel14.Click += LostBorderPanel14_Click;
            BtnSave.Click += BtnSave_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void LostBorderPanel14_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select CSV file to import";
                ofd.Filter = "CSV Files|*.csv";
                ofd.FilterIndex = 1;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedFilePath = ofd.FileName;
                    label2.Text = Path.GetFileName(_selectedFilePath);
                    label2.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Lenient import: valid rows inserted, invalid rows skipped and reported.
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedFilePath) || !File.Exists(_selectedFilePath))
            {
                MessageBox.Show("Please select a valid CSV file.", "No File Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var (books, parseErrors) = ParseCsvFile(_selectedFilePath);

                // If nothing valid to import and parseErrors has entries, report and return.
                if ((books == null || books.Count == 0) && parseErrors.Count > 0)
                {
                    string msg = $"Import completed — 0 record(s) imported, {parseErrors.Count} record(s) rejected.\n\n" +
                        "No valid records were found in the CSV file. Please review the file format and the errors below:\n\n" +
                        string.Join("\n", parseErrors.Take(50));
                    MessageBox.Show(msg, "Import Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int successCount = 0;
                var importErrors = new List<string>();

                foreach (var rec in books)
                {
                    try
                    {
                        int bookId = _bookRepo.Add(rec.Book);

                        // Insert BookAuthor entries
                        foreach (var a in rec.Authors)
                        {
                            int authorId = GetOrCreateAuthor(a.Name);
                            _bookAuthorRepo.Add(new BookAuthor
                            {
                                BookID = bookId,
                                AuthorID = authorId,
                                Role = a.Role
                            });
                        }

                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        importErrors.Add($"Row {rec.RowNumber}: Failed to import '{rec.Book.Title}': {ex.Message}");
                    }
                }

                int rejectedCount = parseErrors.Count + importErrors.Count;
                string summary = $"Import completed — {successCount} record(s) imported, {rejectedCount} record(s) rejected.\n\n" +
                                 "If some rows were skipped, this is due to validation errors (for example: duplicate ISBN, duplicate Call Number, missing required fields or invalid formats). " +
                                 "Please review the CSV and the error details, correct any issues, and retry if necessary.";

                var allErrors = new List<string>();
                allErrors.AddRange(parseErrors);
                allErrors.AddRange(importErrors);

                if (allErrors.Count > 0)
                {
                    string details = "\n\nErrors:\n" + string.Join("\n", allErrors.Take(50));
                    if (allErrors.Count > 50) details += $"\n...and {allErrors.Count - 50} more.";
                    MessageBox.Show(summary + details, "Import Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(summary, "Import Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing CSV file: {ex.Message}", "Import Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Parses CSV and validates rows according to rules provided by user.
        /// Returns list of valid BookImportRecord and list of string errors for skipped rows.
        /// Expected exact headers (case-insensitive): 
        /// "Standard ID","Call Number","Title","Subtitle","Author","Editor","Adviser","Publisher",
        /// "Category","Language","Pages","Edition / Volume","Publication Year","Description / Format",
        /// "ResourceType","LoanType","isDigital","Download URL"
        /// </summary>
        private (List<BookImportRecord> books, List<string> errors) ParseCsvFile(string filePath)
        {
            var books = new List<BookImportRecord>();
            var errors = new List<string>();

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 2)
            {
                errors.Add("CSV file must contain a header row and at least one data row.");
                return (books, errors);
            }

            // Parse header
            var headers = ParseCsvLine(lines[0]);
            var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
            {
                columnMap[headers[i].Trim()] = i;
            }

            // required exact header names
            string[] requiredHeaders = new[]
            {
                "Standard ID","Call Number","Title","Author","Publisher","Category","Pages",
                "Edition / Volume","Publication Year","Description / Format","ResourceType","isDigital"
            };

            foreach (var h in requiredHeaders)
            {
                if (!columnMap.ContainsKey(h))
                {
                    errors.Add($"Missing required header: '{h}'. Ensure the CSV contains this column header exactly.");
                }
            }

            if (errors.Count > 0)
                return (books, errors);

            // Track duplicates inside the CSV to reject duplicate ISBN/CallNumber within file
            var seenIsbns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenCallNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Process data rows
            for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
            {
                int lineNumber = rowIndex + 1;
                string line = lines[rowIndex];

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = ParseCsvLine(line);
                var rowErrors = new List<string>();

                // helper to get column by exact header
                string GetValue(string columnName)
                {
                    if (columnMap.TryGetValue(columnName, out int index) && index < values.Length)
                        return values[index]?.Trim();
                    return null;
                }

                // 1. Standard ID -> ISBN (required, no duplicates)
                string isbn = GetValue("Standard ID");
                if (string.IsNullOrWhiteSpace(isbn))
                {
                    rowErrors.Add($"Row {lineNumber}: 'Standard ID' is required.");
                }
                else
                {
                    // check DB duplicate
                    try
                    {
                        if (_bookRepo.ISBNExists(isbn))
                            rowErrors.Add($"Row {lineNumber}: ISBN '{isbn}' already exists in database.");
                    }
                    catch
                    {
                        // if repo check fails, still continue with file-level checks
                    }

                    if (seenIsbns.Contains(isbn))
                        rowErrors.Add($"Row {lineNumber}: Duplicate ISBN '{isbn}' found in CSV.");

                    if (!rowErrors.Any(e => e.Contains("ISBN")) && !string.IsNullOrWhiteSpace(isbn))
                        seenIsbns.Add(isbn);
                }

                // 2. Call Number -> CallNumber (required, no duplicates)
                string callNumber = GetValue("Call Number");
                if (string.IsNullOrWhiteSpace(callNumber))
                {
                    rowErrors.Add($"Row {lineNumber}: 'Call Number' is required.");
                }
                else
                {
                    try
                    {
                        if (_bookRepo.CallNumberExists(callNumber))
                            rowErrors.Add($"Row {lineNumber}: Call Number '{callNumber}' already exists in database.");
                    }
                    catch
                    {
                        // ignore repo lookup failure
                    }

                    if (seenCallNumbers.Contains(callNumber))
                        rowErrors.Add($"Row {lineNumber}: Duplicate Call Number '{callNumber}' found in CSV.");

                    if (!rowErrors.Any(e => e.Contains("Call Number")) && !string.IsNullOrWhiteSpace(callNumber))
                        seenCallNumbers.Add(callNumber);
                }

                // 3. Title required
                string title = GetValue("Title");
                if (string.IsNullOrWhiteSpace(title))
                    rowErrors.Add($"Row {lineNumber}: 'Title' is required.");

                // 4. Subtitle - nullable
                string subtitle = GetValue("Subtitle");

                // 5. Author required -> BookAuthor role Author
                string authorsRaw = GetValue("Author");
                if (string.IsNullOrWhiteSpace(authorsRaw))
                    rowErrors.Add($"Row {lineNumber}: 'Author' is required.");

                // 6. Editor nullable
                string editorsRaw = GetValue("Editor");

                // 7. Adviser - not null if ResourceType == Periodical (validate later after resourceType parsing)
                string advisersRaw = GetValue("Adviser");

                // 8. Publisher required
                string publisherRaw = GetValue("Publisher");
                if (string.IsNullOrWhiteSpace(publisherRaw))
                    rowErrors.Add($"Row {lineNumber}: 'Publisher' is required.");

                // 9. Category required
                string categoryRaw = GetValue("Category");
                if (string.IsNullOrWhiteSpace(categoryRaw))
                    rowErrors.Add($"Row {lineNumber}: 'Category' is required.");

                // 10. Language nullable
                string language = GetValue("Language");

                // 11. Pages required (integer)
                string pagesRaw = GetValue("Pages");
                int pages = 0;
                if (string.IsNullOrWhiteSpace(pagesRaw))
                {
                    rowErrors.Add($"Row {lineNumber}: 'Pages' is required.");
                }
                else if (!int.TryParse(pagesRaw, out pages) || pages < 0)
                {
                    rowErrors.Add($"Row {lineNumber}: 'Pages' must be a non-negative integer.");
                }

                // 12. Edition / Volume (one column) required
                string editionRaw = GetValue("Edition / Volume");
                if (string.IsNullOrWhiteSpace(editionRaw))
                    rowErrors.Add($"Row {lineNumber}: 'Edition / Volume' is required.");

                // 13. Publication Year required (for Periodical may be M/Y)
                string pubYearRaw = GetValue("Publication Year");
                if (string.IsNullOrWhiteSpace(pubYearRaw))
                    rowErrors.Add($"Row {lineNumber}: 'Publication Year' is required.");

                // 14. Description / Format required
                string descriptionRaw = GetValue("Description / Format");
                if (string.IsNullOrWhiteSpace(descriptionRaw))
                    rowErrors.Add($"Row {lineNumber}: 'Description / Format' is required.");

                // 15. ResourceType required IMPORTANT
                string resourceTypeRaw = GetValue("ResourceType");
                if (string.IsNullOrWhiteSpace(resourceTypeRaw))
                    rowErrors.Add($"Row {lineNumber}: 'ResourceType' is required.");

                // 16. LoanType - not null only if ResourceType == PhysicalBook (validate later)
                string loanTypeRaw = GetValue("LoanType");

                // 17. isDigital (0 or 1) required (column only inside csv)
                string isDigitalRaw = GetValue("isDigital");
                int isDigital = -1;
                if (string.IsNullOrWhiteSpace(isDigitalRaw))
                {
                    rowErrors.Add($"Row {lineNumber}: 'isDigital' is required and must be '0' or '1'.");
                }
                else if (!int.TryParse(isDigitalRaw, out isDigital) || (isDigital != 0 && isDigital != 1))
                {
                    rowErrors.Add($"Row {lineNumber}: 'isDigital' must be either 0 or 1.");
                }

                // 18. Download URL required only if isDigital == 1
                string downloadUrl = GetValue("Download URL");
                if (isDigital == 1)
                {
                    if (string.IsNullOrWhiteSpace(downloadUrl))
                        rowErrors.Add($"Row {lineNumber}: 'Download URL' is required when 'isDigital' is 1.");
                }

                // If any prior validations failed, skip further processing for this row
                if (rowErrors.Count > 0)
                {
                    errors.AddRange(rowErrors);
                    continue;
                }

                // Parse ResourceType enum
                ResourceType resourceType = ParseResourceType(resourceTypeRaw);

                // Adviser required for Periodical
                if (resourceType == ResourceType.Periodical)
                {
                    if (string.IsNullOrWhiteSpace(advisersRaw))
                    {
                        errors.Add($"Row {lineNumber}: 'Adviser' is required for resource type 'Periodical'.");
                        continue;
                    }
                }

                // If resourceType is PhysicalBook enforce LoanType required
                if (resourceType == ResourceType.PhysicalBook)
                {
                    if (string.IsNullOrWhiteSpace(loanTypeRaw))
                    {
                        errors.Add($"Row {lineNumber}: 'LoanType' is required for physical resources.");
                        continue;
                    }
                }

                // Prepare Book instance
                Book book = CreateBookByResourceType(resourceType);

                book.ISBN = isbn;
                book.CallNumber = callNumber;
                book.Title = title;
                book.Subtitle = subtitle;
                book.Language = language;
                book.Pages = pages;
                book.PhysicalDescription = descriptionRaw;
                book.ResourceType = resourceType;
                book.LoanType = loanTypeRaw;

                // set DownloadURL if provided (and required when isDigital==1)
                if (!string.IsNullOrWhiteSpace(downloadUrl))
                    book.DownloadURL = downloadUrl;

                // Edition handling: if Periodical, editionRaw may contain "vol,issue" like "10,2"
                if (resourceType == ResourceType.Periodical)
                {
                    // Convert "10, 2" or "10,2" to "Vol. 10, No. 2"
                    var parts = editionRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(p => p.Trim()).ToArray();
                    if (parts.Length >= 2)
                    {
                        book.Edition = $"Vol. {parts[0]}, No. {parts[1]}";
                    }
                    else if (parts.Length == 1)
                    {
                        book.Edition = $"Vol. {parts[0]}";
                    }
                    else
                    {
                        book.Edition = editionRaw; // fallback
                    }

                    // Publication Year expected in M/Y format (e.g., 3/2021 or Mar/2021)
                    // We'll try to extract the year portion for PublicationYear (int).
                    if (!string.IsNullOrWhiteSpace(pubYearRaw) && pubYearRaw.Contains("/"))
                    {
                        var pyParts = pubYearRaw.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(p => p.Trim()).ToArray();
                        // last part should be year
                        if (pyParts.Length >= 2 && int.TryParse(pyParts[pyParts.Length - 1], out int year))
                        {
                            book.PublicationYear = year;
                        }
                        else
                        {
                            errors.Add($"Row {lineNumber}: 'Publication Year' for Periodical must include month and year (M/Y) and contain a valid year.");
                            continue;
                        }
                    }
                    else
                    {
                        // fallback: try parse entire as year
                        if (int.TryParse(pubYearRaw, out int yearOnly))
                        {
                            book.PublicationYear = yearOnly;
                        }
                        else
                        {
                            errors.Add($"Row {lineNumber}: 'Publication Year' for Periodical must include month and year (M/Y) or a valid year.");
                            continue;
                        }
                    }
                }
                else
                {
                    // Non-periodical: Edition stored as-is, PublicationYear as integer
                    book.Edition = editionRaw;
                    if (!string.IsNullOrWhiteSpace(pubYearRaw))
                    {
                        if (int.TryParse(pubYearRaw, out int py))
                        {
                            book.PublicationYear = py;
                        }
                        else
                        {
                            errors.Add($"Row {lineNumber}: 'Publication Year' must be a valid year.");
                            continue;
                        }
                    }
                }

                // Publisher (get or create)
                if (!string.IsNullOrWhiteSpace(publisherRaw))
                {
                    book.PublisherID = GetOrCreatePublisher(publisherRaw);
                }

                // Category (get or create)
                if (!string.IsNullOrWhiteSpace(categoryRaw))
                {
                    book.CategoryID = GetOrCreateCategory(categoryRaw);
                }

                // Authors / Editors / Advisers -> BookAuthor records to insert later
                var authorInfos = new List<AuthorInfo>();

                if (!string.IsNullOrWhiteSpace(authorsRaw))
                {
                    foreach (var n in authorsRaw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var nm = n.Trim();
                        if (!string.IsNullOrWhiteSpace(nm))
                            authorInfos.Add(new AuthorInfo { Name = nm, Role = "Author" });
                    }
                }

                if (!string.IsNullOrWhiteSpace(editorsRaw))
                {
                    foreach (var n in editorsRaw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var nm = n.Trim();
                        if (!string.IsNullOrWhiteSpace(nm))
                            authorInfos.Add(new AuthorInfo { Name = nm, Role = "Editor" });
                    }
                }

                if (!string.IsNullOrWhiteSpace(advisersRaw))
                {
                    foreach (var n in advisersRaw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var nm = n.Trim();
                        if (!string.IsNullOrWhiteSpace(nm))
                            authorInfos.Add(new AuthorInfo { Name = nm, Role = "Adviser" });
                    }
                }

                // final validations: ensure required roles exist
                if (!authorInfos.Any(a => a.Role.Equals("Author", StringComparison.OrdinalIgnoreCase)))
                {
                    errors.Add($"Row {lineNumber}: At least one 'Author' is required.");
                    continue;
                }

                // Add record
                books.Add(new BookImportRecord
                {
                    Book = book,
                    Authors = authorInfos,
                    RowNumber = lineNumber,
                    IsDigitalFlag = (isDigital == 1)
                });
            }

            return (books, errors);
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var current = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            result.Add(current.ToString());
            return result.ToArray();
        }

        private ResourceType ParseResourceType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ResourceType.PhysicalBook;

            string v = value.Trim().ToLowerInvariant();

            if (v.Contains("e-book") || v.Contains("ebook") || v == "eb" || v.Contains("e book"))
                return ResourceType.EBook;
            if (v.Contains("thesis") || v.Contains("theses") || v.Contains("dissertation"))
                return ResourceType.Thesis;
            if (v.Contains("periodical") || v.Contains("magazine") || v.Contains("journal"))
                return ResourceType.Periodical;
            if (v.Contains("av") || v.Contains("audio") || v.Contains("visual") || v.Contains("video"))
                return ResourceType.AV;

            // default to PhysicalBook when ambiguous
            return ResourceType.PhysicalBook;
        }

        private Book CreateBookByResourceType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.EBook:
                    return new EBook();
                case ResourceType.Periodical:
                    return new Periodical();
                case ResourceType.Thesis:
                    return new Thesis();
                case ResourceType.AV:
                    return new AV();
                case ResourceType.PhysicalBook:
                default:
                    return new PhysicalBook();
            }
        }

        private int GetOrCreatePublisher(string name)
        {
            try
            {
                var publishers = _publisherRepo.GetAll();
                var existing = publishers?.FirstOrDefault(p =>
                    p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (existing != null)
                    return existing.PublisherID;

                var newPublisher = new Publisher { Name = name };
                return _publisherRepo.Add(newPublisher);
            }
            catch
            {
                return 0;
            }
        }

        private int GetOrCreateCategory(string name)
        {
            try
            {
                var categories = _category_repo.GetAll();
                var existing = categories?.FirstOrDefault(c =>
                    c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (existing != null)
                    return existing.CategoryID;

                var newCategory = new Category { Name = name };
                return _category_repo.Add(newCategory);
            }
            catch
            {
                return 0;
            }
        }

        private int GetOrCreateAuthor(string fullName)
        {
            try
            {
                var existing = _authorRepo.GetByName(fullName);
                if (existing != null)
                    return existing.AuthorID;

                var newAuthor = new Author { FullName = fullName };
                return _authorRepo.Add(newAuthor);
            }
            catch
            {
                return 0;
            }
        }

        // Helper classes
        private class BookImportRecord
        {
            public Book Book { get; set; }
            public List<AuthorInfo> Authors { get; set; } = new List<AuthorInfo>();
            public int RowNumber { get; set; } = 0;
            public bool IsDigitalFlag { get; set; } = false;
        }

        private class AuthorInfo
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }
    }
}