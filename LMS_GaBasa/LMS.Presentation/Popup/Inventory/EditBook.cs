using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Database;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Book;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Enums;
using LMS.Presentation.BarcodeGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZXing;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class EditBook : Form
    {
        private readonly BookRepository _bookRepo;
        private readonly BookAuthorRepository _bookAuthorRepo;
        private readonly AuthorRepository _authorRepo;
        private readonly PublisherRepository _publisherRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly ICatalogManager _catalogManager;

        private Book _book;
        private ResourceType _originalResourceType; // track original type
        // If user selected a new local image file, store its original path here so we can copy on save.
        private string _selectedCoverPath;

        public EditBook() : this(null) { }

        // Primary constructor: pass the Book to edit. Repositories constructed here for simplicity; swap with DI if needed.
        public EditBook(Book book)
        {
            InitializeComponent();

            _bookRepo = new BookRepository(new DbConnection());
            _bookAuthorRepo = new BookAuthorRepository(new DbConnection());
            _authorRepo = new AuthorRepository(new DbConnection());
            _publisher_repo_init();
            _publisherRepo = new PublisherRepository(new DbConnection());
            _categoryRepo = new CategoryRepository(new DbConnection());

            // create a local CatalogManager (Category + Language repos) so we can reuse AddBook-style helpers
            var languageRepo = new LanguageRepository(new DbConnection());
            _catalogManager = new CatalogManager(_categoryRepo, languageRepo);

            _book = book;
            _originalResourceType = book?.ResourceType ?? ResourceType.PhysicalBook;

            WireUpEvents();

            // Populate comboboxes similar to AddBook so UI suggestions match behavior
            LoadCategories();
            LoadLanguages(); // <-- added: populate language combobox

            if (_book != null)
                LoadBookToForm(_book);
        }

        private void _publisher_repo_init()
        {
            // placeholder to keep style consistent if extra wiring needed later
        }

        private void WireUpEvents()
        {
            // Always unsubscribe first to avoid double-subscription (this fixes the double-message on save).
            if (PicBxBookCover != null) { PicBxBookCover.Click -= PicBxBookCover_Click; PicBxBookCover.Click += PicBxBookCover_Click; }
            if (BtnAddAuthor != null) { BtnAddAuthor.Click -= BtnAddAuthor_Click; BtnAddAuthor.Click += BtnAddAuthor_Click; }
            if (BtnAddEditor != null) { BtnAddEditor.Click -= BtnAddEditor_Click; BtnAddEditor.Click += BtnAddEditor_Click; }
            // BtnAddPublisher removed — EditBook uses CmbBxPublisher (single publisher)
            if (LstBxAuthor != null) { LstBxAuthor.DoubleClick -= LstBxAuthor_DoubleClick; LstBxAuthor.DoubleClick += LstBxAuthor_DoubleClick; }
            if (LstBxEditor != null) { LstBxEditor.DoubleClick -= LstBxEditor_DoubleClick; LstBxEditor.DoubleClick += LstBxEditor_DoubleClick; }
            // LstBxPublisher removed — no subscription
            if (BtnCancel != null) { BtnCancel.Click -= BtnCancel_Click; BtnCancel.Click += BtnCancel_Click; }
            if (BtnSave != null) { BtnSave.Click -= BtnSave_Click; BtnSave.Click += BtnSave_Click; }

            // Resource type radio handlers (keep UI panels and required-field state in sync)
            if (RdoBtnPhysicalBook != null) { RdoBtnPhysicalBook.CheckedChanged -= RdoBtnPhysicalBook_CheckedChanged; RdoBtnPhysicalBook.CheckedChanged += RdoBtnPhysicalBook_CheckedChanged; }
            if (RdoBtnEBook != null) { RdoBtnEBook.CheckedChanged -= RdoBtnEBook_CheckedChanged; RdoBtnEBook.CheckedChanged += RdoBtnEBook_CheckedChanged; }
            if (RdoBtnTheses != null) { RdoBtnTheses.CheckedChanged -= RdoBtnTheses_CheckedChanged; RdoBtnTheses.CheckedChanged += RdoBtnTheses_CheckedChanged; }
            if (RdoBtnPeriodical != null) { RdoBtnPeriodical.CheckedChanged -= RdoBtnPeriodical_CheckedChanged; RdoBtnPeriodical.CheckedChanged += RdoBtnPeriodical_CheckedChanged; }
            if (RdoBtnAV != null) { RdoBtnAV.CheckedChanged -= RdoBtnAV_CheckedChanged; RdoBtnAV.CheckedChanged += RdoBtnAV_CheckedChanged; }
        }

        // Populate category combobox from DB
        private void LoadCategories()
        {
            try
            {
                CmbBxCategory.Items.Clear();
                var cats = _categoryRepo.GetAll();
                if (cats != null && cats.Count > 0)
                {
                    foreach (var c in cats)
                    {
                        if (!string.IsNullOrWhiteSpace(c.Name) && !CmbBxCategory.Items.Contains(c.Name))
                            CmbBxCategory.Items.Add(c.Name);
                    }
                }
            }
            catch
            {
                // ignore category load errors; leave combo empty
            }
        }

        // Use the same approach as AddBook: ask the CatalogManager for languages
        private void LoadLanguages()
        {
            try
            {
                CmbBxLanguage.Items.Clear();
                var languages = _catalogManager.GetAllLanguages();
                if (languages != null)
                {
                    foreach (var lang in languages)
                    {
                        CmbBxLanguage.Items.Add(lang);
                    }
                }
            }
            catch
            {
                // ignore language load errors
            }
        }

        // Populate authors for suggestions and enable autocomplete + Enter-to-add
        private void LoadAuthors()
        {
            try
            {
                var authorNames = _authorRepo?.GetAll()
                    .Where(a => !string.IsNullOrWhiteSpace(a.FullName))
                    .Select(a => a.FullName.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                SetupComboBoxForAutocomplete(CmbBxAuthors, authorNames);

                if (authorNames != null && authorNames.Length > 0)
                {
                    // Ensure dropdown shows the items
                    CmbBxAuthors.Items.Clear();
                    CmbBxAuthors.Items.AddRange(authorNames);
                }
            }
            catch
            {
                // fallback: ensure combobox has autocomplete setup even if DB query failed
                SetupComboBoxForAutocomplete(CmbBxAuthors, null);
            }
        }

        // Populate editor suggestions similar to AddBook (distinct author names who've been used as editors)
        private void LoadEditors()
        {
            try
            {
                var editorNamesSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                try
                {
                    var editorIds = _bookAuthorRepo?.GetDistinctAuthorIdsByRole("Editor");
                    if (editorIds != null)
                    {
                        foreach (var id in editorIds)
                        {
                            try
                            {
                                var author = _authorRepo?.GetById(id);
                                if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                                    editorNamesSet.Add(author.FullName.Trim());
                            }
                            catch
                            {
                                // skip single lookup failure
                            }
                        }
                    }
                }
                catch
                {
                    // ignore role-query failures
                }

                var editorNames = editorNamesSet.ToArray();
                SetupComboBoxForAutocomplete(CmbBxEditor, editorNames);
                if (editorNames.Length > 0)
                {
                    CmbBxEditor.Items.Clear();
                    CmbBxEditor.Items.AddRange(editorNames);
                }
            }
            catch
            {
                SetupComboBoxForAutocomplete(CmbBxEditor, null);
            }
        }

        // Populate publisher suggestions and enable autocomplete
        private void LoadPublishers()
        {
            try
            {
                var publisherNames = _publisherRepo?.GetAll()
                    ?.Where(p => !string.IsNullOrWhiteSpace(p.Name))
                    .Select(p => p.Name.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                SetupComboBoxForAutocomplete(CmbBxPublisher, publisherNames);

                if (publisherNames != null && publisherNames.Length > 0)
                {
                    CmbBxPublisher.Items.Clear();
                    CmbBxPublisher.Items.AddRange(publisherNames);
                }
            }
            catch
            {
                SetupComboBoxForAutocomplete(CmbBxPublisher, null);
            }
        }

        private void LoadBookToForm(Book book)
        {
            // Basic fields
            TxtISBN.Text = book.ISBN;
            TxtTitle.Text = book.Title;
            TxtSubtitle.Text = book.Subtitle;
            TxtCallNumber.Text = book.CallNumber;
            TxtEdition.Text = book.Edition;
            TxtPublicationYear.Text = book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty;
            TxtNoOfPages.Text = book.Pages > 0 ? book.Pages.ToString() : string.Empty;
            TxtPhysicalDescription.Text = book.PhysicalDescription;
            TxtDownloadLink.Text = book.DownloadURL;
            CmbBxLanguage.Text = book.Language;
            // Resource type: set radio buttons based on book.ResourceType
            switch (book.ResourceType)
            {
                case ResourceType.EBook:
                    RdoBtnEBook.Checked = true;
                    break;
                case ResourceType.Thesis:
                    RdoBtnTheses.Checked = true;
                    break;
                case ResourceType.Periodical:
                    RdoBtnPeriodical.Checked = true;
                    break;
                case ResourceType.AV:
                    RdoBtnAV.Checked = true;
                    break;
                case ResourceType.PhysicalBook:
                default:
                    RdoBtnPhysicalBook.Checked = true;
                    break;
            }

            // Loan type
            if (!string.IsNullOrWhiteSpace(book.LoanType))
            {
                if (book.LoanType.Equals("Reference", StringComparison.OrdinalIgnoreCase)) RdoBtnReference.Checked = true;
                else if (book.LoanType.Equals("Circulation", StringComparison.OrdinalIgnoreCase)) RdoBtnCirculation.Checked = true;
            }

            // Cover image: load safely if relative path set
            if (!string.IsNullOrWhiteSpace(book.CoverImage))
            {
                try
                {
                    var path = Path.Combine(Application.StartupPath, book.CoverImage.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(path))
                    {
                        using (var fs = File.OpenRead(path))
                        using (var img = Image.FromStream(fs))
                        {
                            PicBxBookCover.Image = new Bitmap(img);
                        }
                    }
                }
                catch
                {
                    // ignore image load errors
                }
            }

            // Authors / Editors - load from repositories
            try
            {
                var bas = _bookAuthorRepo.GetByBookId(book.BookID);
                if (bas != null)
                {
                    LstBxAuthor.Items.Clear();
                    LstBxEditor.Items.Clear();

                    foreach (var ba in bas)
                    {
                        var author = _author_repo_getbyid_safe(ba.AuthorID);
                        if (author == null) continue;
                        var name = author.FullName?.Trim() ?? string.Empty;
                        if (string.Equals(ba.Role, "Editor", StringComparison.OrdinalIgnoreCase))
                            LstBxEditor.Items.Add(name);
                        else
                            LstBxAuthor.Items.Add(name);
                    }
                }
            }
            catch
            {
                // ignore
            }

            // Publisher: set combobox text (single publisher)
            try
            {
                if (book.PublisherID > 0)
                {
                    var pub = _publisherRepo.GetById(book.PublisherID);
                    if (pub != null)
                    {
                        // set as selected text; if not present in items add it
                        if (!CmbBxPublisher.Items.Contains(pub.Name))
                            CmbBxPublisher.Items.Add(pub.Name);
                        CmbBxPublisher.Text = pub.Name;
                    }
                }
            }
            catch
            {
                // ignore
            }

            // Category: set combobox selection using CategoryID (falls back to Category navigation if present)
            try
            {
                if (book.CategoryID > 0)
                {
                    var cat = _categoryRepo.GetById(book.CategoryID);
                    if (cat != null)
                    {
                        if (!CmbBxCategory.Items.Contains(cat.Name))
                            CmbBxCategory.Items.Add(cat.Name);
                        CmbBxCategory.Text = cat.Name;
                    }
                }
                else if (book.Category != null && !string.IsNullOrWhiteSpace(book.Category.Name))
                {
                    // If navigation property is present (sometimes), use it
                    if (!CmbBxCategory.Items.Contains(book.Category.Name))
                        CmbBxCategory.Items.Add(book.Category.Name);
                    CmbBxCategory.Text = book.Category.Name;
                }
            }
            catch
            {
                // ignore category lookup errors
            }

            // Ensure panels reflect chosen resource type (CheckedChanged handlers will run when Checked set above;
            // but call explicitly to be safe in case designer wiring/messaging differs)
            SyncPanelsForResourceType();
        }

        private void PicBxBookCover_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Book Cover Image";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedCoverPath = ofd.FileName;
                    try
                    {
                        using (var fs = File.OpenRead(_selectedCoverPath))
                        using (var img = Image.FromStream(fs))
                        {
                            // dispose previous
                            if (PicBxBookCover.Image != null) { try { PicBxBookCover.Image.Dispose(); } catch { } }
                            PicBxBookCover.Image = new Bitmap(img);
                        }
                    }
                    catch
                    {
                        // ignore load error
                    }
                }
            }
        }

        private void BtnAddAuthor_Click(object sender, EventArgs e)
        {
            var name = CmbBxAuthors.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;

            if (!ContainsLetter(name))
            {
                MessageBox.Show("Author name must contain at least one letter and may include digits.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbBxAuthors.Focus();
                return;
            }

            // Use LINQ to check existence because ListBox.ObjectCollection.Contains doesn't accept a comparer
            bool exists = LstBxAuthor.Items.Cast<object>()
                .Any(x => string.Equals(x?.ToString(), name, StringComparison.OrdinalIgnoreCase));

            if (!exists)
                LstBxAuthor.Items.Add(name);

            CmbBxAuthors.Text = string.Empty;
        }

        private void BtnAddEditor_Click(object sender, EventArgs e)
        {
            var name = CmbBxEditor.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;

            if (!ContainsLetter(name))
            {
                MessageBox.Show("Editor name must contain at least one letter and may include digits.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbBxEditor.Focus();
                return;
            }

            bool exists = LstBxEditor.Items.Cast<object>()
                .Any(x => string.Equals(x?.ToString(), name, StringComparison.OrdinalIgnoreCase));

            if (!exists)
                LstBxEditor.Items.Add(name);

            CmbBxEditor.Text = string.Empty;
        }

        // BtnAddPublisher removed: EditBook uses CmbBxPublisher so there's no Add button handler here.

        private void LstBxAuthor_DoubleClick(object sender, EventArgs e)
        {
            if (LstBxAuthor.SelectedIndex >= 0) LstBxAuthor.Items.RemoveAt(LstBxAuthor.SelectedIndex);
        }

        private void LstBxEditor_DoubleClick(object sender, EventArgs e)
        {
            if (LstBxEditor.SelectedIndex >= 0) LstBxEditor.Items.RemoveAt(LstBxEditor.SelectedIndex);
        }

        // LstBxPublisher removed — no double-click handler.

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Basic validation
                if (!string.IsNullOrWhiteSpace(TxtPublicationYear.Text) && !int.TryParse(TxtPublicationYear.Text.Trim(), out _))
                {
                    MessageBox.Show("Publication year must be numeric.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate pages input: accept large numbers, clamp if necessary
                if (!string.IsNullOrWhiteSpace(TxtNoOfPages.Text))
                {
                    int pages = ParseInt(TxtNoOfPages.Text);
                    if (pages <= 0)
                    {
                        MessageBox.Show("Number of pages must be a positive integer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Ensure we have a book to update
                if (_book == null)
                {
                    MessageBox.Show("No book loaded to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Additional required-field validation based on resource type:
                if (RdoBtnPhysicalBook.Checked)
                {
                    // Require either Reference or Circulation selected
                    if (!RdoBtnReference.Checked && !RdoBtnCirculation.Checked)
                    {
                        MessageBox.Show("Please select Loan Type: Reference or Circulation for physical books.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (RdoBtnEBook.Checked)
                {
                    // E-Book requires download link
                    if (string.IsNullOrWhiteSpace(TxtDownloadLink.Text))
                    {
                        MessageBox.Show("Please provide a download link for E-Book resources.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Publisher validation: must contain at least one letter if provided
                var publisherText = CmbBxPublisher.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(publisherText) && !ContainsLetter(publisherText))
                {
                    MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbBxPublisher.Focus();
                    return;
                }

                // Map UI -> book
                _book.ISBN = TxtISBN.Text?.Trim();
                _book.Title = TxtTitle.Text?.Trim();
                _book.Subtitle = TxtSubtitle.Text?.Trim();
                _book.CallNumber = TxtCallNumber.Text?.Trim();
                _book.Edition = TxtEdition.Text?.Trim();
                _book.PublicationYear = ParseInt(TxtPublicationYear.Text);
                _book.Pages = ParseInt(TxtNoOfPages.Text);
                _book.PhysicalDescription = TxtPhysicalDescription.Text?.Trim();
                // Only persist DownloadURL for e-books; clear it for all other resource types
                _book.DownloadURL = RdoBtnEBook.Checked ? TxtDownloadLink.Text?.Trim() : null;
                _book.Language = CmbBxLanguage.Text?.Trim();

                // Category: try to find existing or create new category, then set CategoryID
                var categoryText = CmbBxCategory.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(categoryText))
                {
                    try
                    {
                        var existingCat = _categoryRepo.GetByName(categoryText);
                        if (existingCat != null)
                        {
                            _book.CategoryID = existingCat.CategoryID;
                        }
                        else
                        {
                            // create new category (minimal info)
                            var newCat = new Category { Name = categoryText, Description = null };
                            int newId = _category_repo_add_safe(newCat);
                            if (newId > 0) _book.CategoryID = newId;
                        }
                    }
                    catch
                    {
                        // ignore category persistence errors; leave CategoryID unchanged
                    }
                }

                // Resource type
                if (RdoBtnEBook.Checked) _book.ResourceType = ResourceType.EBook;
                else if (RdoBtnTheses.Checked) _book.ResourceType = ResourceType.Thesis;
                else if (RdoBtnPeriodical.Checked) _book.ResourceType = ResourceType.Periodical;
                else if (RdoBtnAV.Checked) _book.ResourceType = ResourceType.AV;
                else _book.ResourceType = ResourceType.PhysicalBook;

                // If user is changing from non-eBook to eBook, confirm because copies will be deleted.
                if (_originalResourceType != _book.ResourceType && _book.ResourceType == ResourceType.EBook && _originalResourceType != ResourceType.EBook)
                {
                    var dlgRes = MessageBox.Show(
                        "Are you sure you want to edit this book's resource type to E-Book? All copies of this book will be permanently deleted.",
                        "Confirm change to E-Book",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dlgRes != DialogResult.Yes)
                    {
                        // user cancelled the destructive change
                        return;
                    }
                }

                // Loan type
                if (RdoBtnReference.Checked) _book.LoanType = "Reference";
                else if (RdoBtnCirculation.Checked) _book.LoanType = "Circulation";
                else _book.LoanType = null;

                // Publisher: choose combobox text (single publisher model)
                if (!string.IsNullOrWhiteSpace(CmbBxPublisher.Text))
                {
                    var publisherName = CmbBxPublisher.Text.Trim();
                    try
                    {
                        var existing = _publisherRepo.GetAll()
                            .FirstOrDefault(p => string.Equals(p.Name?.Trim(), publisherName, StringComparison.OrdinalIgnoreCase));
                        if (existing != null)
                        {
                            _book.PublisherID = existing.PublisherID;
                        }
                        else
                        {
                            var newPub = new Publisher { Name = publisherName };
                            int newId = _publisherRepo.Add(newPub);
                            if (newId > 0) _book.PublisherID = newId;
                        }
                    }
                    catch
                    {
                        // ignore publisher repo errors; leave PublisherID unchanged
                    }
                }

                // Cover image: if user selected a new local image, copy it and set relative path
                if (!string.IsNullOrWhiteSpace(_selectedCoverPath))
                {
                    var rel = SaveCoverImage(_selectedCoverPath);
                    if (!string.IsNullOrWhiteSpace(rel))
                        _book.CoverImage = rel;
                }

                // Persist book main row
                bool updated = _bookRepo.Update(_book);
                if (!updated)
                {
                    MessageBox.Show("Failed to update book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // If resource type changed, handle copies / regeneration.
                if (_originalResourceType != _book.ResourceType)
                {
                    try
                    {
                        // If changed TO EBook -> delete copies permanently
                        if (_book.ResourceType == ResourceType.EBook && _originalResourceType != ResourceType.EBook)
                        {
                            var bookCopyRepo = new BookCopyRepository(new DbConnection());
                            bool deleted = bookCopyRepo.DeleteByBookId(_book.BookID);
                            // suppressed exception on delete; if needed we could verify, but treat as non-fatal
                        }
                        else
                        {
                            // Otherwise regenerate accessions and barcodes
                            RegenerateAccessionsAndBarcodes(_book.BookID, _originalResourceType, _book.ResourceType);
                        }

                        // update original type to prevent repeated regeneration if dialog stays open
                        _originalResourceType = _book.ResourceType;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Book updated but regenerating copy accessions/barcodes failed: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Replace BookAuthor associations (simple strategy: delete existing, then insert current authors & editors)
                try
                {
                    _bookAuthorRepo.DeleteByBookId(_book.BookID);

                    // Authors
                    for (int i = 0; i < LstBxAuthor.Items.Count; i++)
                    {
                        var name = LstBxAuthor.Items[i]?.ToString()?.Trim();
                        if (string.IsNullOrWhiteSpace(name)) continue;

                        int authorId = EnsureAuthorExists(name);
                        if (authorId > 0)
                        {
                            var ba = new BookAuthor
                            {
                                BookID = _book.BookID,
                                AuthorID = authorId,
                                Role = "Author",
                                IsPrimaryAuthor = i == 0
                            };
                            _bookAuthorRepo.Add(ba);
                        }
                    }

                    // Editors
                    for (int i = 0; i < LstBxEditor.Items.Count; i++)
                    {
                        var name = LstBxEditor.Items[i]?.ToString()?.Trim();
                        if (string.IsNullOrWhiteSpace(name)) continue;

                        int authorId = EnsureAuthorExists(name);
                        if (authorId > 0)
                        {
                            var ba = new BookAuthor
                            {
                                BookID = _book.BookID,
                                AuthorID = authorId,
                                Role = "Editor",
                                IsPrimaryAuthor = false
                            };
                            _bookAuthorRepo.Add(ba);
                        }
                    }
                }
                catch
                {
                    // Non-fatal: authors/editors update failed; book main row was updated already.
                }

                MessageBox.Show("Book updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Regenerate accession numbers and barcodes when resource type changes.
        private void RegenerateAccessionsAndBarcodes(int bookId, ResourceType oldType, ResourceType newType)
        {
            var bookCopyRepo = new BookCopyRepository(new DbConnection());
            var copies = bookCopyRepo.GetByBookId(bookId) ?? new List<BookCopy>();
            if (copies.Count == 0) return;

            // ask repository for canonical prefix for new type
            string newPrefix = bookCopyRepo.GetPrefixForResourceType(newType);

            var newAccessions = new List<string>();
            var copyByNewAcc = new Dictionary<string, BookCopy>(StringComparer.OrdinalIgnoreCase);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var copy in copies)
            {
                string original = copy.AccessionNumber ?? string.Empty;
                string candidate;

                if (!string.IsNullOrWhiteSpace(original) && original.Contains("-"))
                {
                    // Replace leading alphabetic prefix before first hyphen
                    candidate = Regex.Replace(original, @"^[A-Za-z]+(?=-)", newPrefix);
                }
                else
                {
                    // fallback stable format: PREFIX-BookID-Year-CopyID
                    candidate = $"{newPrefix}-{bookId}-{DateTime.Now.Year}-{copy.CopyID:D4}";
                }

                // ensure uniqueness
                if (seen.Contains(candidate))
                {
                    candidate = $"{candidate}-{copy.CopyID}";
                }

                seen.Add(candidate);
                newAccessions.Add(candidate);
                copyByNewAcc[candidate] = copy;
            }

            if (newAccessions.Count == 0) return;

            // generate barcodes for all new accessions
            string barcodesFolder = Path.Combine(Application.StartupPath, "Assets", "dataimages", "BookCopyBarcodes");
            var generator = new ZXingBarcodeGenerator(barcodesFolder, BarcodeFormat.CODE_128, width: 300, height: 100, margin: 10);

            IDictionary<string, string> generatedMap;
            try
            {
                generatedMap = generator.GenerateMany(newAccessions);
            }
            catch (Exception ex)
            {
                throw new Exception("Barcode generation failed: " + ex.Message, ex);
            }

            var failures = new List<string>();

            foreach (var kv in generatedMap)
            {
                var acc = kv.Key;
                var path = kv.Value; // relative path expected
                if (!copyByNewAcc.TryGetValue(acc, out var copy)) continue;

                try
                {
                    bool ok = bookCopyRepo.UpdateAccessionAndBarcodeByCopyId(copy.CopyID, acc, path);
                    if (!ok) failures.Add($"CopyID {copy.CopyID} failed to update.");
                }
                catch (Exception ex)
                {
                    failures.Add($"CopyID {copy.CopyID}: {ex.Message}");
                }
            }

            if (failures.Count > 0)
                throw new Exception(string.Join("; ", failures));
        }

        private void SyncPanelsForResourceType()
        {
            if (RdoBtnPhysicalBook != null && PnlforRdoBtnPhysicalBooks != null)
            {
                PnlforRdoBtnPhysicalBooks.Visible = RdoBtnPhysicalBook.Checked;
            }

            if (RdoBtnEBook != null && PnlforRdoBtnEBook != null)
            {
                PnlforRdoBtnEBook.Visible = RdoBtnEBook.Checked;
            }

            // Loan radios enabled only when physical book selected
            bool physical = RdoBtnPhysicalBook != null && RdoBtnPhysicalBook.Checked;
            if (RdoBtnReference != null) RdoBtnReference.Enabled = physical;
            if (RdoBtnCirculation != null) RdoBtnCirculation.Enabled = physical;

            // If physical is not selected, ensure loan type radios are cleared
            if (!physical)
            {
                if (RdoBtnReference != null) RdoBtnReference.Checked = false;
                if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;
            }

            // Download link enabled only for eBook
            bool ebook = RdoBtnEBook != null && RdoBtnEBook.Checked;
            if (TxtDownloadLink != null) TxtDownloadLink.Enabled = ebook;

            // If ebook selected, clear loan options
            if (ebook)
            {
                if (RdoBtnReference != null) RdoBtnReference.Checked = false;
                if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;
            }
            else
            {
                // When ebook is not selected, clear the download link so it isn't left behind.
                if (TxtDownloadLink != null) TxtDownloadLink.Text = string.Empty;
            }
        }

        private void RdoBtnPhysicalBook_CheckedChanged(object sender, EventArgs e)
        {
            if (PnlforRdoBtnPhysicalBooks != null)
                PnlforRdoBtnPhysicalBooks.Visible = RdoBtnPhysicalBook.Checked;

            // When physical is deselected, clear loan radios
            if (!RdoBtnPhysicalBook.Checked)
            {
                if (RdoBtnReference != null) RdoBtnReference.Checked = false;
                if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;
            }

            // Ensure eBook panel hidden when switching to physical
            if (PnlforRdoBtnEBook != null && RdoBtnPhysicalBook.Checked)
                PnlforRdoBtnEBook.Visible = false;

            // Sync enabled states
            SyncPanelsForResourceType();
        }

        private void RdoBtnEBook_CheckedChanged(object sender, EventArgs e)
        {
            if (PnlforRdoBtnEBook != null)
                PnlforRdoBtnEBook.Visible = RdoBtnEBook.Checked;

            // When e-book selected, clear loan radios
            if (RdoBtnEBook.Checked)
            {
                if (RdoBtnReference != null) RdoBtnReference.Checked = false;
                if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;
            }

            // Ensure physical panel hidden when switching to eBook
            if (PnlforRdoBtnPhysicalBooks != null && RdoBtnEBook.Checked)
                PnlforRdoBtnPhysicalBooks.Visible = false;

            // Sync enabled states (this now also clears download link when e-book is deselected)
            SyncPanelsForResourceType();
        }

        private void RdoBtnTheses_CheckedChanged(object sender, EventArgs e)
        {
            if (PnlforRdoBtnEBook != null) PnlforRdoBtnEBook.Visible = false;
            if (PnlforRdoBtnPhysicalBooks != null) PnlforRdoBtnPhysicalBooks.Visible = false;
            if (RdoBtnReference != null) RdoBtnReference.Checked = false;
            if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;

            SyncPanelsForResourceType();
        }

        private void RdoBtnPeriodical_CheckedChanged(object sender, EventArgs e)
        {
            if (PnlforRdoBtnEBook != null) PnlforRdoBtnEBook.Visible = false;
            if (PnlforRdoBtnPhysicalBooks != null) PnlforRdoBtnPhysicalBooks.Visible = false;
            if (RdoBtnReference != null) RdoBtnReference.Checked = false;
            if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;

            SyncPanelsForResourceType();
        }

        private void RdoBtnAV_CheckedChanged(object sender, EventArgs e)
        {
            if (PnlforRdoBtnEBook != null) PnlforRdoBtnEBook.Visible = false;
            if (PnlforRdoBtnPhysicalBooks != null) PnlforRdoBtnPhysicalBooks.Visible = false;
            if (RdoBtnReference != null) RdoBtnReference.Checked = false;
            if (RdoBtnCirculation != null) RdoBtnCirculation.Checked = false;

            SyncPanelsForResourceType();
        }

        // --- End helpers ---

        private int EnsureAuthorExists(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return 0;
            try
            {
                var existing = _author_repo_getbyname_safe(fullName);
                if (existing != null) return existing.AuthorID;

                var a = new Author { FullName = fullName };
                return _authorRepo.Add(a);
            }
            catch
            {
                return 0;
            }
        }

        private Author _author_repo_getbyname_safe(string fullName)
        {
            try { return _authorRepo.GetByName(fullName); }
            catch { return null; }
        }

        private int _category_repo_add_safe(Category cat)
        {
            try { return _categoryRepo.Add(cat); }
            catch { return 0; }
        }

        private Author _author_repo_getbyid_safe(int id)
        {
            try { return _authorRepo.GetById(id); }
            catch { return null; }
        }

        private int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            value = value.Trim();

            // Try parse as long to accept values above int range, then clamp to int.MaxValue.
            if (long.TryParse(value, out long longVal))
            {
                if (longVal <= int.MinValue) return int.MinValue;
                if (longVal > int.MaxValue) return int.MaxValue;
                return (int)longVal;
            }

            // Accept common formatted numbers like "1,234,567" by removing non-digit chars except leading sign.
            var digits = new string(value.Where(c => char.IsDigit(c) || c == '-' || c == '+').ToArray());
            if (long.TryParse(digits, out longVal))
            {
                if (longVal <= int.MinValue) return int.MinValue;
                if (longVal > int.MaxValue) return int.MaxValue;
                return (int)longVal;
            }

            return 0;
        }

        private string SaveCoverImage(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath)) return null;

            try
            {
                string coversDirFull = Path.Combine(Application.StartupPath, "Assets", "dataimages", "Books");
                if (!Directory.Exists(coversDirFull)) Directory.CreateDirectory(coversDirFull);

                string ext = Path.GetExtension(sourceFilePath);
                if (string.IsNullOrWhiteSpace(ext)) ext = ".jpg";

                string newFileName = $"{Guid.NewGuid()}{ext}";
                string destFullPath = Path.Combine(coversDirFull, newFileName);

                // copy safely
                File.Copy(sourceFilePath, destFullPath, true);

                string relativePath = $"Assets/dataimages/Books/{newFileName}";
                return relativePath;
            }
            catch
            {
                return null;
            }
        }

        private bool ContainsLetter(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Any(char.IsLetter);
        }

        // Setup helper copied from AddBook to give consistent combobox/autocomplete behavior
        private void SetupComboBoxForAutocomplete(ComboBox comboBox, IEnumerable<string> items)
        {
            if (comboBox == null) return;

            comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var ac = new AutoCompleteStringCollection();

            comboBox.Items.Clear();
            if (items != null)
            {
                var array = items.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                if (array.Length > 0)
                {
                    comboBox.Items.AddRange(array);
                    ac.AddRange(array);
                }
            }

            comboBox.AutoCompleteCustomSource = ac;

            // Enter key should act like pressing Add button for author/editor comboboxes
            comboBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (comboBox == CmbBxAuthors) BtnAddAuthor.PerformClick();
                    else if (comboBox == CmbBxEditor) BtnAddEditor.PerformClick();
                    // publisher combobox no longer has an "Add" button

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
        }
    }
}
