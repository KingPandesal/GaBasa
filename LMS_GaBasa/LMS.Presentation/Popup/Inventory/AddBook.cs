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
                SetupComboBoxForAutocomplete(CmbBxAuthors, authorNames);
                if (authorNames != null && authorNames.Length > 0)
                {
                    // Ensure dropdown shows the items
                    CmbBxAuthors.Items.Clear();
                    CmbBxAuthors.Items.AddRange(authorNames);
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
                SetupComboBoxForAutocomplete(CmbBxEditor, editorNames);
                if (editorNames.Length > 0)
                {
                    CmbBxEditor.Items.Clear();
                    CmbBxEditor.Items.AddRange(editorNames);
                }
            }
            catch
            {
                // Non-fatal: if DB lookup fails keep existing behavior (empty suggestion list)
                SetupComboBoxForAutocomplete(CmbBxAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxEditor, null);
            }

            // Set default values
            NumPckNoOfCopies.Value = 1;
            CmbBxCopyStatus.SelectedIndex = 0; // Available

            // Wire up events
            PicBxBookCover.Click += PicBxBookCover_Click;
            BtnAddAuthor.Click += BtnAddAuthor_Click;
            BtnAddEditor.Click += BtnAddEditor_Click;
            // BtnAddPublisher removed: publisher is single combobox now
            LstBxAuthor.DoubleClick += LstBxAuthor_DoubleClick;
            LstBxEditor.DoubleClick += LstBxEditor_DoubleClick;
            // LstBxPublisher removed from UI; do not subscribe
            BtnCancel.Click += BtnCancel_Click;

            // Enable editable/combo behaviour and Enter-to-add
            // DO NOT clear previously populated author/editor items here
            SetupComboBoxForAutocomplete(CmbBxPublisher, _publisherRepo != null ? _publisherRepo.GetAll().Select(p => p.Name) : null);

            // Hide sub-panels initially
            PnlforRdoBtnPhysicalBooks.Visible = false;
            PnlforRdoBtnEBook.Visible = false;

            // Default to Physical Book
            RdoBtnPhysicalBook.Checked = true;
        }

        private void LoadCategories()
        {
            CmbBxCategory.Items.Clear();
            var categories = _catalogManager.GetAllCategories();
            foreach (var cat in categories)
            {
                CmbBxCategory.Items.Add(cat.Name);
            }
        }

        private void LoadLanguages()
        {
            CmbBxLanguage.Items.Clear();
            var languages = _catalogManager.GetAllLanguages();
            foreach (var lang in languages)
            {
                CmbBxLanguage.Items.Add(lang);
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
            string authorName = CmbBxAuthors.Text.Trim();
            if (string.IsNullOrWhiteSpace(authorName))
            {
                MessageBox.Show("Please enter an author name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (!CmbBxAuthors.Items.Contains(authorName))
                    CmbBxAuthors.Items.Add(authorName);

                CmbBxAuthors.Text = string.Empty;
                CmbBxAuthors.Focus();
            }
            else
            {
                MessageBox.Show("This author is already added.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddEditor_Click(object sender, EventArgs e)
        {
            string editorName = CmbBxEditor.Text.Trim();
            if (string.IsNullOrWhiteSpace(editorName))
            {
                MessageBox.Show("Please enter an editor name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (!CmbBxEditor.Items.Contains(editorName))
                    CmbBxEditor.Items.Add(editorName);

                CmbBxEditor.Text = string.Empty;
                CmbBxEditor.Focus();
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
            if (LstBxAuthor.SelectedIndex >= 0)
            {
                _authors.RemoveAt(LstBxAuthor.SelectedIndex);
                RefreshAuthorListBox();
            }
        }

        private void LstBxEditor_DoubleClick(object sender, EventArgs e)
        {
            if (LstBxEditor.SelectedIndex >= 0)
            {
                _editors.RemoveAt(LstBxEditor.SelectedIndex);
                RefreshEditorListBox();
            }
        }

        private void RefreshAuthorListBox()
        {
            LstBxAuthor.Items.Clear();
            foreach (var author in _authors)
            {
                LstBxAuthor.Items.Add(author);
            }
        }

        private void RefreshEditorListBox()
        {
            LstBxEditor.Items.Clear();
            foreach (var editor in _editors)
            {
                LstBxEditor.Items.Add(editor);
            }
        }

        private void RdoBtnPhysicalBook_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnPhysicalBook.Checked)
            {
                PnlforRdoBtnPhysicalBooks.Visible = true;
                PnlforRdoBtnEBook.Visible = false;
            }
        }

        private void RdoBtnEBook_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnEBook.Checked)
            {
                PnlforRdoBtnEBook.Visible = true;
                PnlforRdoBtnPhysicalBooks.Visible = false;
                // Deselect reference/circulation
                RdoBtnReference.Checked = false;
                RdoBtnCirculation.Checked = false;
            }
        }

        private void RdoBtnTheses_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnTheses.Checked)
            {
                PnlforRdoBtnEBook.Visible = false;
                PnlforRdoBtnPhysicalBooks.Visible = false;
                RdoBtnReference.Checked = false;
                RdoBtnCirculation.Checked = false;
            }
        }

        private void RdoBtnPeriodical_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnPeriodical.Checked)
            {
                PnlforRdoBtnEBook.Visible = false;
                PnlforRdoBtnPhysicalBooks.Visible = false;
                RdoBtnReference.Checked = false;
                RdoBtnCirculation.Checked = false;
            }
        }

        private void RdoBtnAV_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnAV.Checked)
            {
                PnlforRdoBtnEBook.Visible = false;
                PnlforRdoBtnPhysicalBooks.Visible = false;
                RdoBtnReference.Checked = false;
                RdoBtnCirculation.Checked = false;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate pages input contains digits only (provide clearer message than generic required)
                var pagesText = TxtNoOfPages.Text?.Trim();
                if (!string.IsNullOrEmpty(pagesText) && !pagesText.All(char.IsDigit))
                {
                    MessageBox.Show("Number of pages should be digits only.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNoOfPages.Focus();
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
            string publisherName = !string.IsNullOrWhiteSpace(CmbBxPublisher.Text)
                ? CmbBxPublisher.Text.Trim()
                : string.Empty;

            var dto = new DTOCreateBook
            {
                ISBN = TxtISBN.Text.Trim(),
                Title = TxtTitle.Text.Trim(),
                Subtitle = TxtSubtitle.Text.Trim(),
                Publisher = publisherName,
                PublicationYear = ParseInt(TxtPublicationYear.Text),
                Edition = TxtEdition.Text.Trim(),
                CategoryName = CmbBxCategory.Text.Trim(),
                Language = CmbBxLanguage.Text.Trim(),
                Pages = ParseInt(TxtNoOfPages.Text),
                PhysicalDescription = TxtPhysicalDescription.Text.Trim(),
                CallNumber = TxtCallNumber.Text.Trim(),
                CoverImage = SaveCoverImage(),
                ResourceType = GetSelectedResourceType(),
                LoanType = GetLoanType(),
                DownloadURL = TxtDownloadLink.Text.Trim(),
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
            if (CmbBxCategory.SelectedIndex >= 0)
            {
                var categories = _catalogManager.GetAllCategories();
                var selectedCat = categories.FirstOrDefault(c =>
                    c.Name.Equals(CmbBxCategory.Text, StringComparison.OrdinalIgnoreCase));
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

        private string GetLoanType()
        {
            if (RdoBtnPhysicalBook.Checked)
            {
                if (RdoBtnReference.Checked) return "Reference";
                if (RdoBtnCirculation.Checked) return "Circulation";
            }
            return null;
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
            if (int.TryParse(value, out int result))
                return result;
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
