using LMS.BusinessLogic.Factories;
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.BusinessLogic.Services.Book.AddBook;
using LMS.DataAccess.Database;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Book;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Enums;
using LMS.Presentation.BarcodeGenerator;
using LMS.BusinessLogic.Services.BarcodeGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZXing;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class AddBook : Form
    {
        private readonly ICatalogManager _catalogManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IPublisherRepository _publisherRepo;

        // Barcode generator (presentation implements the business interface)
        private readonly IBarcodeGenerator _barcodeGenerator;

        // Author repository used to populate suggestions and lookup existing authors/editors
        private readonly IAuthorRepository _authorRepo;
        private readonly IBookAuthorRepository _bookAuthorRepo;

        private List<string> _authors = new List<string>();
        private List<string> _editors = new List<string>();
        private string _coverImagePath = null;

        // Designer / backwards-compatible ctor (keeps current behavior)
        public AddBook()
        {
            InitializeComponent();

            // Existing quick wiring remains for convenience in designer/run without DI.
            var dbConnection = new DbConnection();
            var categoryRepo = new CategoryRepository(dbConnection);
            var languageRepo = new LanguageRepository(dbConnection);
            var bookRepo = new BookRepository(dbConnection);
            var authorRepo = new AuthorRepository(dbConnection);
            var bookAuthorRepo = new BookAuthorRepository(dbConnection);
            var bookCopyRepo = new BookCopyRepository(dbConnection);
            var bookFactory = new BookFactory();
            _publisherRepo = new PublisherRepository(dbConnection);

            _catalogManager = new CatalogManager(categoryRepo, languageRepo);

            // create barcode generator with configured output folder (Presentation knows file system)
            string barcodesFolder = Path.Combine(Application.StartupPath, "Assets", "dataimages", "BookCopyBarcodes");
            _barcodeGenerator = new LMS.Presentation.BarcodeGenerator.ZXingBarcodeGenerator(
                barcodesFolder, ZXing.BarcodeFormat.CODE_128, width: 300, height: 100, margin: 10);

            var addBookService = new AddBookService(
                bookRepo, authorRepo, bookAuthorRepo,
                bookCopyRepo, bookFactory, _publisherRepo);

            _inventoryManager = new InventoryManager(addBookService, bookRepo, _catalogManager);

            // keep reference to author repo for populating suggestions
            _authorRepo = authorRepo;
            _bookAuthorRepo = bookAuthorRepo;

            InitializeForm();
        }

        // DI-friendly constructor (preferred). Compose dependencies in Program.Main and pass here.
        public AddBook(IInventoryManager inventoryManager, ICatalogManager catalogManager, IPublisherRepository publisherRepo, IBarcodeGenerator barcodeGenerator)
        {
            InitializeComponent();

            _inventoryManager = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));
            _catalogManager = catalogManager ?? throw new ArgumentNullException(nameof(catalogManager));
            _publisherRepo = publisherRepo ?? throw new ArgumentNullException(nameof(publisherRepo));
            _barcodeGenerator = barcodeGenerator; // optional

            // If DI didn't provide an author repo, create a local one for suggestions
            _authorRepo = new AuthorRepository(new DbConnection());
            _bookAuthorRepo = new BookAuthorRepository(new DbConnection());

            InitializeForm();
        }

        private void InitializeForm()
        {
            // Load categories into combobox
            LoadCategories();

            // Load predefined languages
            LoadLanguages();

            // Populate author/editor suggestion lists from DB
            try
            {
                // Authors: all names from Author table
                var authorNames = _authorRepo?.GetAll()
                    .Where(a => !string.IsNullOrWhiteSpace(a.FullName))
                    .Select(a => a.FullName.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                // Populate authors combo with DB values so users can select instead of typing
                SetupComboBoxForAutocomplete(CmbBxBKAuthors, authorNames);
                if (authorNames != null && authorNames.Length > 0)
                {
                    // Ensure dropdown shows the items
                    CmbBxBKAuthors.Items.Clear();
                    CmbBxBKAuthors.Items.AddRange(authorNames);
                }

                // Editors: find distinct author IDs that appear with Role = "Editor",
                // then map to author names and avoid duplicates.
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
                                // Skip any single lookup failure
                            }
                        }
                    }
                }
                catch
                {
                    // ignore role-query failures
                }

                var editorNames = editorNamesSet.ToArray();
                SetupComboBoxForAutocomplete(CmbBxBKEditor, editorNames);
                if (editorNames.Length > 0)
                {
                    CmbBxBKEditor.Items.Clear();
                    CmbBxBKEditor.Items.AddRange(editorNames);
                }
            }
            catch
            {
                // Non-fatal: if DB lookup fails keep existing behavior (empty suggestion list)
                SetupComboBoxForAutocomplete(CmbBxBKAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxBKEditor, null);
            }

            // Set default values
            NumPckNoOfCopies.Value = 1;
            CmbBxCopyStatus.SelectedIndex = 0; // Available

            // Wire up events
            PicBxBookCover.Click += PicBxBookCover_Click;
            BtnBKAddAuthor.Click += BtnAddAuthor_Click;
            BtnBKAddEditor.Click += BtnAddEditor_Click;
            // BtnAddPublisher removed: publisher is single combobox now
            LstBxBKAuthor.DoubleClick += LstBxAuthor_DoubleClick;
            LstBxBKEditor.DoubleClick += LstBxEditor_DoubleClick;
            // LstBxPublisher removed from UI; do not subscribe
            BtnCancel.Click += BtnCancel_Click;

            // Enable editable/combo behaviour and Enter-to-add
            // DO NOT clear previously populated author/editor items here
            SetupComboBoxForAutocomplete(CmbBxBKPublisher, _publisherRepo != null ? _publisherRepo.GetAll().Select(p => p.Name) : null);

            // Hide sub-panels initially

            // Default to Physical Book
            RdoBtnPhysicalBook.Checked = true;

            // Enforce maximum of 10 digits for No. of Pages (prevents typing an 11th digit)
            EnforceDigitsLimit(TxtBKNoOfPages, 10);
        }

        private void LoadCategories()
        {
            CmbBxBKCategory.Items.Clear();
            var categories = _catalogManager.GetAllCategories();
            foreach (var cat in categories)
            {
                CmbBxBKCategory.Items.Add(cat.Name);
            }
        }

        private void LoadLanguages()
        {
            CmbBxBKLanguage.Items.Clear();
            var languages = _catalogManager.GetAllLanguages();
            foreach (var lang in languages)
            {
                CmbBxBKLanguage.Items.Add(lang);
            }
        }

        private void PicBxBookCover_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Book Cover Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _coverImagePath = openFileDialog.FileName;
                    PicBxBookCover.Image = Image.FromFile(_coverImagePath);
                    PicBxBookCover.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void BtnAddAuthor_Click(object sender, EventArgs e)
        {
            string authorName = CmbBxBKAuthors.Text.Trim();
            if (string.IsNullOrWhiteSpace(authorName))
            {
                MessageBox.Show("Please enter an author name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ContainsLetter(authorName))
            {
                MessageBox.Show("Author name must contain at least one letter and may include digits.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbBxBKAuthors.Focus();
                return;
            }

            // Prefer existing DB author (lookup) — don't create DB record here.
            try
            {
                var existing = _authorRepo?.GetByName(authorName.Trim());
                if (existing != null)
                    authorName = existing.FullName?.Trim() ?? authorName;
            }
            catch
            {
                // Ignore DB lookup error — fall back to typed name
            }

            if (!_authors.Contains(authorName, StringComparer.OrdinalIgnoreCase))
            {
                _authors.Add(authorName);
                RefreshAuthorListBox();
                // keep the suggestion list updated
                if (!CmbBxBKAuthors.Items.Contains(authorName))
                    CmbBxBKAuthors.Items.Add(authorName);

                CmbBxBKAuthors.Text = string.Empty;
                CmbBxBKAuthors.Focus();
            }
            else
            {
                MessageBox.Show("This author is already added.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddEditor_Click(object sender, EventArgs e)
        {
            string editorName = CmbBxBKEditor.Text.Trim();
            if (string.IsNullOrWhiteSpace(editorName))
            {
                MessageBox.Show("Please enter an editor name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ContainsLetter(editorName))
            {
                MessageBox.Show("Editor name must contain at least one letter and may include digits.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbBxBKEditor.Focus();
                return;
            }

            // Prefer existing DB author record for editor as well
            try
            {
                var existing = _authorRepo?.GetByName(editorName.Trim());
                if (existing != null)
                    editorName = existing.FullName?.Trim() ?? editorName;
            }
            catch
            {
                // Ignore lookup errors
            }

            if (!_editors.Contains(editorName, StringComparer.OrdinalIgnoreCase))
            {
                _editors.Add(editorName);
                RefreshEditorListBox();
                if (!CmbBxBKEditor.Items.Contains(editorName))
                    CmbBxBKEditor.Items.Add(editorName);

                CmbBxBKEditor.Text = string.Empty;
                CmbBxBKEditor.Focus();
            }
            else
            {
                MessageBox.Show("This editor is already added.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Publisher add/remove UI removed — publisher is single combobox input now.
        // Any persistence (create publisher) is handled in BuildDTOFromForm -> AddBookService.

        private void LstBxAuthor_DoubleClick(object sender, EventArgs e)
        {
            if (LstBxBKAuthor.SelectedIndex >= 0)
            {
                _authors.RemoveAt(LstBxBKAuthor.SelectedIndex);
                RefreshAuthorListBox();
            }
        }

        private void LstBxEditor_DoubleClick(object sender, EventArgs e)
        {
            if (LstBxBKEditor.SelectedIndex >= 0)
            {
                _editors.RemoveAt(LstBxBKEditor.SelectedIndex);
                RefreshEditorListBox();
            }
        }

        private void RefreshAuthorListBox()
        {
            LstBxBKAuthor.Items.Clear();
            foreach (var author in _authors)
            {
                LstBxBKAuthor.Items.Add(author);
            }
        }

        private void RefreshEditorListBox()
        {
            LstBxBKEditor.Items.Clear();
            foreach (var editor in _editors)
            {
                LstBxBKEditor.Items.Add(editor);
            }
        }

        private void RdoBtnPhysicalBook_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RdoBtnEBook_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RdoBtnTheses_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RdoBtnPeriodical_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RdoBtnAV_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate pages input: allow large numbers and common formatting (commas), clamp if too large.
                var pagesText = TxtBKNoOfPages.Text?.Trim();
                if (!string.IsNullOrEmpty(pagesText))
                {
                    int pages = ParseInt(pagesText);
                    if (pages <= 0)
                    {
                        MessageBox.Show("Number of pages should be a positive integer.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKNoOfPages.Focus();
                        return;
                    }
                }

                // Publisher validation: must contain at least one letter if provided
                var publisherText = CmbBxBKPublisher.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(publisherText) && !ContainsLetter(publisherText))
                {
                    MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbBxBKPublisher.Focus();
                    return;
                }

                var dto = BuildDTOFromForm();

                // Set the AddedByID to the currently logged in user id (Program.CurrentUserId).
                dto.AddedByID = Program.CurrentUserId;

                // Optionally validate and stop early if not set
                if (dto.AddedByID <= 0)
                {
                    MessageBox.Show("Unable to determine the current user. Please sign in before adding books.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate
                if (!_inventoryManager.ValidateBookData(dto, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save
                var result = _inventoryManager.AddBook(dto);

                if (result.Success)
                {
                    // Generate barcode images for created accession numbers
                    try
                    {
                        if (result.AccessionNumbers != null && result.AccessionNumbers.Count > 0)
                        {
                            // generator created in constructor, reuse here
                            var map = _barcodeGenerator.GenerateMany(result.AccessionNumbers);

                            // persist barcode image paths to DB
                            var bookCopyRepo = new BookCopyRepository(new DbConnection());
                            foreach (var kv in map)
                            {
                                var accession = kv.Key;
                                var barcodePath = kv.Value; // relative path returned by generator
                                if (!string.IsNullOrWhiteSpace(barcodePath))
                                    bookCopyRepo.UpdateBarcodeImage(accession, barcodePath);
                            }
                        }
                    }
                    catch
                    {
                        // Non-fatal: barcode generation failed, but book was saved.
                    }

                    MessageBox.Show("Book added successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DTOCreateBook BuildDTOFromForm()
        {
            // Determine publisher name: prefer combobox text only (single publisher)
            string publisherName = !string.IsNullOrWhiteSpace(CmbBxBKPublisher.Text)
                ? CmbBxBKPublisher.Text.Trim()
                : string.Empty;

            var dto = new DTOCreateBook
            {
                ISBN = TxtBKISBN.Text.Trim(),
                Title = TxtBKTitle.Text.Trim(),
                Subtitle = TxtBKSubtitle.Text.Trim(),
                Publisher = publisherName,
                PublicationYear = ParseInt(TxtBKPublicationYear.Text),
                Edition = TxtBKEdition.Text.Trim(),
                CategoryName = CmbBxBKCategory.Text.Trim(),
                Language = CmbBxBKLanguage.Text.Trim(),
                Pages = ParseInt(TxtBKNoOfPages.Text),
                PhysicalDescription = TxtBKPhysicalDescription.Text.Trim(),
                CallNumber = TxtBKCallNumber.Text.Trim(),
                CoverImage = SaveCoverImage(),
                ResourceType = GetSelectedResourceType(),
                InitialCopyCount = NumPckNoOfCopies.Value,
                CopyStatus = CmbBxCopyStatus.Text,
                CopyLocation = TxtLocation.Text.Trim()
            };

            // If publisher name maps to an existing publisher in DB, set PublisherID
            if (!string.IsNullOrWhiteSpace(dto.Publisher) && _publisherRepo != null)
            {
                var match = _publisherRepo.GetAll()
                    .FirstOrDefault(p => string.Equals(p.Name?.Trim(), dto.Publisher.Trim(), StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    dto.PublisherID = match.PublisherID;
                }
                // if match == null, AddBookService will create it (EnsurePublisherId) before inserting
            }

            // Add authors
            foreach (var authorName in _authors)
            {
                dto.Authors.Add(new DTOBookAuthor
                {
                    AuthorName = authorName,
                    Role = "Author",
                    IsPrimaryAuthor = dto.Authors.Count == 0 // First author is primary
                });
            }

            // Add editors
            foreach (var editorName in _editors)
            {
                dto.Editors.Add(new DTOBookAuthor
                {
                    AuthorName = editorName,
                    Role = "Editor",
                    IsPrimaryAuthor = false
                });
            }

            // Get category ID if existing category selected
            if (CmbBxBKCategory.SelectedIndex >= 0)
            {
                var categories = _catalogManager.GetAllCategories();
                var selectedCat = categories.FirstOrDefault(c =>
                    c.Name.Equals(CmbBxBKCategory.Text, StringComparison.OrdinalIgnoreCase));
                if (selectedCat != null)
                {
                    dto.CategoryID = selectedCat.CategoryID;
                }
            }

            return dto;
        }

        private ResourceType GetSelectedResourceType()
        {
            // If a radio is explicitly checked, return the corresponding enum directly.
            if (RdoBtnPhysicalBook.Checked) return ResourceType.PhysicalBook;
            if (RdoBtnEBook.Checked) return ResourceType.EBook;
            if (RdoBtnTheses.Checked) return ResourceType.Thesis;
            if (RdoBtnPeriodical.Checked) return ResourceType.Periodical;
            if (RdoBtnAV.Checked) return ResourceType.AV;

            // Fallback: try to map by label text if UI changed to display "Audio-Visual" (or similar)
            string label = null;
            if (RdoBtnAV != null) label = RdoBtnAV.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(label))
            {
                switch (label.Trim())
                {
                    case "AV":
                    case "Audio-Visual":
                    case "Audio Visual":
                    case "AudioVisual":
                        return ResourceType.AV;
                    case "E-Book":
                    case "EBook":
                        return ResourceType.EBook;
                    case "Thesis":
                    case "Theses":
                        return ResourceType.Thesis;
                    case "Periodical":
                    case "Periodicals":
                        return ResourceType.Periodical;
                    case "Physical Book":
                    case "PhysicalBook":
                    case "Book":
                    default:
                        return ResourceType.PhysicalBook;
                }
            }

            // Default
            return ResourceType.PhysicalBook;
        }


        private string SaveCoverImage()
        {
            if (string.IsNullOrEmpty(_coverImagePath)) return null;

            try
            {
                // Destination folder inside the presentation project
                string coversDirFull = Path.Combine(Application.StartupPath, "Assets", "dataimages", "Books");

                if (!Directory.Exists(coversDirFull))
                    Directory.CreateDirectory(coversDirFull);

                // Ensure we preserve extension and avoid collisions
                string ext = Path.GetExtension(_coverImagePath);
                if (string.IsNullOrWhiteSpace(ext))
                    ext = ".jpg";

                string newFileName = $"{Guid.NewGuid()}{ext}";
                string destFullPath = Path.Combine(coversDirFull, newFileName);

                // If source and destination are the same file, skip copy
                string normalizedSource = Path.GetFullPath(_coverImagePath).TrimEnd(Path.DirectorySeparatorChar);
                string normalizedDest = Path.GetFullPath(destFullPath).TrimEnd(Path.DirectorySeparatorChar);
                if (!string.Equals(normalizedSource, normalizedDest, StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(_coverImagePath, destFullPath, true);
                }

                // Return a relative path (use forward slashes) so that's stored in DB and portable
                string relativePath = $"Assets/dataimages/Books/{newFileName}";
                return relativePath;
            }
            catch
            {
                // If copy fails, return null so caller can decide (validation may still allow)
                return null;
            }
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtCallNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void LblCallNumber_Click(object sender, EventArgs e)
        {

        }

        private void LblTitle_Click(object sender, EventArgs e)
        {

        }

        private void LblPublicationYear_Click(object sender, EventArgs e)
        {

        }

        private void TxtEdition_TextChanged(object sender, EventArgs e)
        {

        }

        private void LblEdition_Click(object sender, EventArgs e)
        {

        }

        private void TxtPublicationYear_TextChanged(object sender, EventArgs e)
        {

        }

        private void CmbBxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TxtPhysicalDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void LblPhysicalDescription_Click(object sender, EventArgs e)
        {

        }

        private void TxtNoOfPages_TextChanged(object sender, EventArgs e)
        {

        }

        private void LblNoOfPages_Click(object sender, EventArgs e)
        {

        }

        private void LblLanguage_Click(object sender, EventArgs e)
        {

        }

        private void LblCategory_Click(object sender, EventArgs e)
        {

        }

        private void CmbBxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GrpBxBookInformation_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

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

            // Enter key should act like pressing Add button
            comboBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (comboBox == CmbBxBKAuthors) BtnBKAddAuthor.PerformClick();
                    else if (comboBox == CmbBxBKEditor) BtnBKAddEditor.PerformClick();
                    // publisher combobox no longer has an "Add" button

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
        }

        // Limit number of digit characters that can be entered into a textbox.
        // Allows control keys and non-digit formatting (commas) but prevents entering more than maxDigits digits.
        private void EnforceDigitsLimit(TextBox tb, int maxDigits)
        {
            if (tb == null) return;

            tb.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                if (char.IsDigit(e.KeyChar))
                {
                    int currentDigits = tb.Text.Count(char.IsDigit);
                    // if replacing a selection, subtract digits in the selection
                    if (tb.SelectionLength > 0)
                    {
                        var sel = tb.SelectedText ?? string.Empty;
                        int selDigits = sel.Count(char.IsDigit);
                        if (currentDigits - selDigits >= maxDigits)
                        {
                            e.Handled = true;
                        }
                    }
                    else if (currentDigits >= maxDigits)
                    {
                        e.Handled = true;
                    }
                }
            };

            tb.TextChanged += (s, e) =>
            {
                var txt = tb.Text ?? string.Empty;
                int digits = 0;
                var buf = new System.Text.StringBuilder(txt.Length);
                foreach (var ch in txt)
                {
                    if (char.IsDigit(ch))
                    {
                        if (digits < maxDigits)
                        {
                            buf.Append(ch);
                            digits++;
                        }
                        // else skip extra digits
                    }
                    else
                    {
                        buf.Append(ch);
                    }
                }

                var newText = buf.ToString();
                if (!string.Equals(newText, txt, StringComparison.Ordinal))
                {
                    int selStart = tb.SelectionStart;
                    tb.Text = newText;
                    tb.SelectionStart = Math.Min(selStart, tb.Text.Length);
                }
            };
        }

        private bool ContainsLetter(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Any(char.IsLetter);
        }

        private void GrpBxCopyInformation_Enter(object sender, EventArgs e)
        {

        }

        private void GrpBxResourceType_Enter(object sender, EventArgs e)
        {


        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PnlforRdoBtnEBook_Paint(object sender, PaintEventArgs e)
        {
                    }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GrpBxPeriodicals_Enter(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox22_SelectedIndexChanged(object sender, EventArgs e)
        {
                    }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }
    }
}
