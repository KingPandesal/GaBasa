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

        // Per-resource lists (keep names aligned with controls so we don't mix them up)
        private List<string> _prAuthors = new List<string>();
        private List<string> _prEditors = new List<string>();
        private List<string> _thAuthors = new List<string>();
        private List<string> _thAdvisers = new List<string>();
        private List<string> _avAuthors = new List<string>();
        private List<string> _avEditors = new List<string>();
        private List<string> _ebAuthors = new List<string>();
        private List<string> _ebEditors = new List<string>();

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
            LoadLanguages(); // fills CmbBxBKLanguage

            // Populate author/editor/adviser suggestion lists from DB for all resource-types
            try
            {
                // Authors: all names from Author table -> reuse for all author comboboxes
                var authorNames = _authorRepo?.GetAll()
                    .Where(a => !string.IsNullOrWhiteSpace(a.FullName))
                    .Select(a => a.FullName.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                // Setup BK authors
                SetupComboBoxForAutocomplete(CmbBxBKAuthors, authorNames);
                if (authorNames != null && authorNames.Length > 0)
                {
                    CmbBxBKAuthors.Items.Clear();
                    CmbBxBKAuthors.Items.AddRange(authorNames);
                }

                // Periodical / AV / EBook / Thesis author combos
                SetupComboBoxForAutocomplete(CmbBxPRAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxTHAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxAVAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxEBAuthors, authorNames);

                // Editors: query BookAuthor table for role = "Editor"
                var editorNamesSet = GetNamesByRole("Editor");
                var editorNames = editorNamesSet.ToArray();
                SetupComboBoxForAutocomplete(CmbBxBKEditor, editorNames);
                SetupComboBoxForAutocomplete(CmbBxPREditors, editorNames);
                SetupComboBoxForAutocomplete(CmbBxAVEditors, editorNames);
                SetupComboBoxForAutocomplete(CmbBxEBEditors, editorNames);

                if (editorNames.Length > 0)
                {
                    CmbBxBKEditor.Items.Clear(); CmbBxBKEditor.Items.AddRange(editorNames);
                    CmbBxPREditors.Items.Clear(); CmbBxPREditors.Items.AddRange(editorNames);
                    CmbBxAVEditors.Items.Clear(); CmbBxAVEditors.Items.AddRange(editorNames);
                    CmbBxEBEditors.Items.Clear(); CmbBxEBEditors.Items.AddRange(editorNames);
                }

                // Advisers: query BookAuthor table for role = "Adviser" (Thesis)
                var adviserNames = GetNamesByRole("Adviser").ToArray();
                SetupComboBoxForAutocomplete(CmbBxTHAdvisers, adviserNames);
                if (adviserNames.Length > 0)
                {
                    CmbBxTHAdvisers.Items.Clear();
                    CmbBxTHAdvisers.Items.AddRange(adviserNames);
                }

                // Publishers: load from publisher table into all publisher comboboxes
                var publishers = _publisherRepo != null ? _publisherRepo.GetAll().Select(p => p.Name) : null;
                SetupComboBoxForAutocomplete(CmbBxBKPublisher, publishers);
                SetupComboBoxForAutocomplete(CmbBxPRPublisher, publishers);
                SetupComboBoxForAutocomplete(CmbBxTHPublisher, publishers);
                SetupComboBoxForAutocomplete(CmbBxAVPublisher, publishers);
                SetupComboBoxForAutocomplete(CmbBxEBPublisher, publishers);

                if (publishers != null)
                {
                    var pubArr = publishers.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                    if (pubArr.Length > 0)
                    {
                        CmbBxBKPublisher.Items.Clear(); CmbBxBKPublisher.Items.AddRange(pubArr);
                        CmbBxPRPublisher.Items.Clear(); CmbBxPRPublisher.Items.AddRange(pubArr);
                        CmbBxTHPublisher.Items.Clear(); CmbBxTHPublisher.Items.AddRange(pubArr);
                        CmbBxAVPublisher.Items.Clear(); CmbBxAVPublisher.Items.AddRange(pubArr);
                        CmbBxEBPublisher.Items.Clear(); CmbBxEBPublisher.Items.AddRange(pubArr);
                    }
                }

                // Languages: use catalog manager languages for every language combobox
                var languages = _catalogManager?.GetAllLanguages();
                if (languages != null && languages.Count > 0)
                {
                    SetupComboBoxForAutocomplete(CmbBxBKLanguage, languages);
                    SetupComboBoxForAutocomplete(CmbBxPRLanguage, languages);
                    SetupComboBoxForAutocomplete(CmbBxTHLanguage, languages);
                    SetupComboBoxForAutocomplete(CmbBxAVLanguage, languages);
                    SetupComboBoxForAutocomplete(CmbBxEBLanguage, languages);

                    CmbBxBKLanguage.Items.Clear(); CmbBxBKLanguage.Items.AddRange(languages.ToArray());
                    CmbBxPRLanguage.Items.Clear(); CmbBxPRLanguage.Items.AddRange(languages.ToArray());
                    CmbBxTHLanguage.Items.Clear(); CmbBxTHLanguage.Items.AddRange(languages.ToArray());
                    CmbBxAVLanguage.Items.Clear(); CmbBxAVLanguage.Items.AddRange(languages.ToArray());
                    CmbBxEBLanguage.Items.Clear(); CmbBxEBLanguage.Items.AddRange(languages.ToArray());
                }
                else
                {
                    // still ensure auto-complete is set
                    SetupComboBoxForAutocomplete(CmbBxPRLanguage, null);
                    SetupComboBoxForAutocomplete(CmbBxTHLanguage, null);
                    SetupComboBoxForAutocomplete(CmbBxAVLanguage, null);
                    SetupComboBoxForAutocomplete(CmbBxEBLanguage, null);
                }
            }
            catch
            {
                // Non-fatal: keep existing behavior and empty suggestion lists if DB lookup fails
                SetupComboBoxForAutocomplete(CmbBxBKAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxBKEditor, null);
                SetupComboBoxForAutocomplete(CmbBxPRAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxPREditors, null);
                SetupComboBoxForAutocomplete(CmbBxTHAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxTHAdvisers, null);
                SetupComboBoxForAutocomplete(CmbBxAVAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxAVEditors, null);
                SetupComboBoxForAutocomplete(CmbBxEBAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxEBEditors, null);
                SetupComboBoxForAutocomplete(CmbBxBKPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxPRPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxTHPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxAVPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxEBPublisher, null);
            }

            // Set default values
            NumPckNoOfCopies.Value = 1;
            CmbBxCopyStatus.SelectedIndex = 0; // Available

            // Wire up events
            PicBxBookCover.Click += PicBxBookCover_Click;
            BtnBKAddAuthor.Click += BtnAddAuthor_Click;
            BtnBKAddEditor.Click += BtnAddEditor_Click;
            LstBxBKAuthor.DoubleClick += LstBxAuthor_DoubleClick;
            LstBxBKEditor.DoubleClick += LstBxEditor_DoubleClick;
            BtnCancel.Click += BtnCancel_Click;

            // Wire up other Add buttons and list double-click removals
            BtnPRAddAuthors.Click += (s, e) => AddFromCombo(CmbBxPRAuthors, LstBxPRAuthors, _prAuthors);
            BtnPRAddEditors.Click += (s, e) => AddFromCombo(CmbBxPREditors, LstBxPREditors, _prEditors);
            LstBxPRAuthors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxPRAuthors, _prAuthors);
            LstBxPREditors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxPREditors, _prEditors);

            BtnTHAddAuthors.Click += (s, e) => AddFromCombo(CmbBxTHAuthors, LstBxTHAuthors, _thAuthors);
            BtnTHAddAdvisers.Click += (s, e) => AddFromCombo(CmbBxTHAdvisers, LstBxTHAdvisers, _thAdvisers);
            LstBxTHAuthors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxTHAuthors, _thAuthors);
            LstBxTHAdvisers.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxTHAdvisers, _thAdvisers);

            BtnAVAddAuthors.Click += (s, e) => AddFromCombo(CmbBxAVAuthors, LstBxAVAuthors, _avAuthors);
            BtnAVAddEditors.Click += (s, e) => AddFromCombo(CmbBxAVEditors, LstBxAVEditors, _avEditors);
            LstBxAVAuthors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxAVAuthors, _avAuthors);
            LstBxAVEditors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxAVEditors, _avEditors);

            BtnEBAddAuthors.Click += (s, e) => AddFromCombo(CmbBxEBAuthors, LstBxEBAuthors, _ebAuthors);
            BtnEBAddEditors.Click += (s, e) => AddFromCombo(CmbBxEBEditors, LstBxEBeditors, _ebEditors);
            LstBxEBAuthors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxEBAuthors, _ebAuthors);
            LstBxEBeditors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxEBeditors, _ebEditors);

            // wire up resource-type radios (already present in file) — these show/hide main detail groupboxes
            RdoBtnPhysicalBook.CheckedChanged += RdoBtnPhysicalBook_CheckedChanged;
            RdoBtnEBook.CheckedChanged += RdoBtnEBook_CheckedChanged;
            RdoBtnTheses.CheckedChanged += RdoBtnTheses_CheckedChanged;
            RdoBtnPeriodical.CheckedChanged += RdoBtnPeriodical_CheckedChanged;
            RdoBtnAV.CheckedChanged += RdoBtnAV_CheckedChanged;

            // wire up periodical material-format radios to toggle their panels
            RdoBtnPRPhysical.CheckedChanged += PRMaterialFormat_CheckedChanged;
            RdoBtnPRDigital.CheckedChanged += PRMaterialFormat_CheckedChanged;

            // wire up thesis material-format radios
            RdoBtnTHPhysical.CheckedChanged += THMaterialFormat_CheckedChanged;
            RdoBtnTHDigital.CheckedChanged += THMaterialFormat_CheckedChanged;

            // wire up audio-visual material-format radios
            RdoBtnAVPhysical.CheckedChanged += AVMaterialFormat_CheckedChanged;
            RdoBtnAVDigital.CheckedChanged += AVMaterialFormat_CheckedChanged;

            // wire up copy-information toggle for all PR/TH/AV radios (physical OR digital change may affect visibility)
            RdoBtnPRPhysical.CheckedChanged += CopyInfoRadio_CheckedChanged;
            RdoBtnPRDigital.CheckedChanged += CopyInfoRadio_CheckedChanged;
            RdoBtnTHPhysical.CheckedChanged += CopyInfoRadio_CheckedChanged;
            RdoBtnTHDigital.CheckedChanged += CopyInfoRadio_CheckedChanged;
            RdoBtnAVPhysical.CheckedChanged += CopyInfoRadio_CheckedChanged;
            RdoBtnAVDigital.CheckedChanged += CopyInfoRadio_CheckedChanged;

            // Enable editable/combo behaviour and Enter-to-add for publisher too
            SetupComboBoxForAutocomplete(CmbBxBKPublisher, _publisherRepo != null ? _publisherRepo.GetAll().Select(p => p.Name) : null);

            // Ensure GrpBxCopyInformation is hidden initially
            if (GrpBxCopyInformation != null) GrpBxCopyInformation.Visible = false;

            // Do NOT force any resource-type radio to Checked here — leave them unselected initially.
            // Ensure visibility matches current (possibly no) selection -> this will hide all detail panels when none selected.
            SetVisibleResourceGroup(GetSelectedResourceType());

            // Ensure material-format panels visibility matches current radio selections (may be none)
            PRMaterialFormat_CheckedChanged(this, EventArgs.Empty);
            THMaterialFormat_CheckedChanged(this, EventArgs.Empty);
            AVMaterialFormat_CheckedChanged(this, EventArgs.Empty);

            // Ensure copy info visibility matches current material-format radios
            UpdateCopyInformationVisibility();

            // Enforce maximum of 10 digits for No. of Pages
            EnforceDigitsLimit(TxtBKNoOfPages, 10);
        }

        private IEnumerable<string> GetNamesByRole(string role)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var ids = _bookAuthorRepo?.GetDistinctAuthorIdsByRole(role);
                if (ids != null)
                {
                    foreach (var id in ids)
                    {
                        try
                        {
                            var author = _authorRepo?.GetById(id);
                            if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                                set.Add(author.FullName.Trim());
                        }
                        catch
                        {
                            // ignore single lookup failures
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }

            return set;
        }

        private void AddBook_Load(object sender, EventArgs e)
        {
            // Keep radios untouched. Update UI to reflect current radio state (none selected => hide detail panels).
            SetVisibleResourceGroup(GetSelectedResourceType());
            PRMaterialFormat_CheckedChanged(this, EventArgs.Empty);
            THMaterialFormat_CheckedChanged(this, EventArgs.Empty);
            AVMaterialFormat_CheckedChanged(this, EventArgs.Empty);

            // Update copy information visibility based on current material-format radios
            UpdateCopyInformationVisibility();
        }

        /// <summary>
        /// Shared handler wired to all PR/TH/AV material radios (physical and digital).
        /// Updates the visibility of GrpBxCopyInformation when any physical radio is checked.
        /// </summary>
        private void CopyInfoRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCopyInformationVisibility();
        }

        /// <summary>
        /// Sets GrpBxCopyInformation.Visible = true if any of PR/TH/AV physical radios are checked.
        /// Otherwise hides the group box.
        /// </summary>
        private void UpdateCopyInformationVisibility()
        {
            bool showCopyInfo = false;

            if (RdoBtnPRPhysical != null && RdoBtnPRPhysical.Checked) showCopyInfo = true;
            if (RdoBtnTHPhysical != null && RdoBtnTHPhysical.Checked) showCopyInfo = true;
            if (RdoBtnAVPhysical != null && RdoBtnAVPhysical.Checked) showCopyInfo = true;

            if (GrpBxCopyInformation != null)
                GrpBxCopyInformation.Visible = showCopyInfo;

            // refresh layout so visibility change is reflected immediately
            flowLayoutPanel1?.PerformLayout();
            flowLayoutPanel1?.Refresh();
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
            AddFromCombo(CmbBxBKAuthors, LstBxBKAuthor, _authors);
        }

        private void BtnAddEditor_Click(object sender, EventArgs e)
        {
            AddFromCombo(CmbBxBKEditor, LstBxBKEditor, _editors);
        }

        /// <summary>
        /// Generic add-from-combobox implementation used by all resource groups.
        /// Validates input, prefers existing DB author name, updates suggestion list and backing listbox.
        /// </summary>
        private void AddFromCombo(ComboBox combo, ListBox targetListBox, List<string> backingList)
        {
            if (combo == null || targetListBox == null || backingList == null) return;

            string name = combo.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                combo.Focus();
                return;
            }

            if (!ContainsLetter(name))
            {
                MessageBox.Show("Name must contain at least one letter and may include digits.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                combo.Focus();
                return;
            }

            try
            {
                var existing = _authorRepo?.GetByName(name);
                if (existing != null)
                    name = existing.FullName?.Trim() ?? name;
            }
            catch
            {
                // ignore lookup failure
            }

            if (!backingList.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                backingList.Add(name);
                RefreshListBox(targetListBox, backingList);

                // keep combo suggestion list updated
                if (!combo.Items.Contains(name))
                    combo.Items.Add(name);

                combo.Text = string.Empty;
                combo.Focus();
            }
            else
            {
                MessageBox.Show("This entry is already added.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshListBox(ListBox lb, List<string> values)
        {
            if (lb == null || values == null) return;
            lb.Items.Clear();
            foreach (var v in values)
                lb.Items.Add(v);
        }

        private void RemoveSelectedFromList(ListBox lb, List<string> backingList)
        {
            if (lb == null || backingList == null) return;
            if (lb.SelectedIndex >= 0)
            {
                backingList.RemoveAt(lb.SelectedIndex);
                RefreshListBox(lb, backingList);
            }
        }

        // Publisher add/remove UI removed — publisher is single combobox input now.
        // Any persistence (create publisher) is handled in BuildDTOFromForm -> AddBookService.
        // For other groups we have wired add buttons above.

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
            if (RdoBtnPhysicalBook.Checked)
                SetVisibleResourceGroup(ResourceType.PhysicalBook);
        }

        private void RdoBtnEBook_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnEBook.Checked)
                SetVisibleResourceGroup(ResourceType.EBook);
        }

        private void RdoBtnTheses_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnTheses.Checked)
                SetVisibleResourceGroup(ResourceType.Thesis);
        }

        private void RdoBtnPeriodical_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnPeriodical.Checked)
                SetVisibleResourceGroup(ResourceType.Periodical);
        }

        private void RdoBtnAV_CheckedChanged(object sender, EventArgs e)
        {
            if (RdoBtnAV.Checked)
                SetVisibleResourceGroup(ResourceType.AV);
        }

        /// <summary>
        /// Show exactly the GroupBox matching the selected resource type and hide the others.
        /// Copy information panel remains visible for all types.
        /// </summary>
        /// <param name="type">Selected ResourceType</param>
        private void SetVisibleResourceGroup(ResourceType? type)
        {
            // If no resource type selected, hide all specific detail groupboxes.
            bool showPhysical = type.HasValue && type.Value == ResourceType.PhysicalBook;
            bool showPeriodical = type.HasValue && type.Value == ResourceType.Periodical;
            bool showThesis = type.HasValue && type.Value == ResourceType.Thesis;
            bool showAV = type.HasValue && type.Value == ResourceType.AV;
            bool showEBook = type.HasValue && type.Value == ResourceType.EBook;

            if (GrpBxPhysicalBook != null) GrpBxPhysicalBook.Visible = showPhysical;
            if (GrpBxPeriodicals != null) GrpBxPeriodicals.Visible = showPeriodical;
            if (GrpBxThesis != null) GrpBxThesis.Visible = showThesis;
            if (GrpBxAV != null) GrpBxAV.Visible = showAV;
            if (GrpBxEBook != null) GrpBxEBook.Visible = showEBook;

            // Refresh layout so FlowLayoutPanel updates the scroll area correctly.
            flowLayoutPanel1?.PerformLayout();
            flowLayoutPanel1?.Refresh();
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
                // If user hasn't selected a resource type, default to PhysicalBook for saving to preserve backwards compatibility.
                ResourceType = GetSelectedResourceType() ?? ResourceType.PhysicalBook,
                InitialCopyCount = NumPckNoOfCopies.Value,
                CopyStatus = CmbBxCopyStatus.Text,
                CopyLocation = TxtLocation.Text.Trim()
            };

            // If publisher name maps to an existing publisher in DB, set PublisherID.
            // If not, create it now (user requested missing publishers be saved on Save).
            if (!string.IsNullOrWhiteSpace(dto.Publisher) && _publisherRepo != null)
            {
                var match = _publisherRepo.GetAll()
                    .FirstOrDefault(p => string.Equals(p.Name?.Trim(), dto.Publisher.Trim(), StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    dto.PublisherID = match.PublisherID;
                }
                else
                {
                    try
                    {
                        var newPub = new Publisher { Name = dto.Publisher };
                        int newId = _publisherRepo.Add(newPub);
                        if (newId > 0) dto.PublisherID = newId;
                    }
                    catch
                    {
                        // ignore create failure - AddBookService may handle it too
                    }
                }
            }

            // If category name provided but not selected from existing list, ensure it exists (will create if necessary)
            if (!string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                try
                {
                    // GetOrCreateCategory returns existing or newly created Category
                    var cat = _catalogManager.GetOrCreateCategory(dto.CategoryName);
                    if (cat != null) dto.CategoryID = cat.CategoryID;
                }
                catch
                {
                    // ignore
                }
            }

            // Ensure language exists in catalog (add if missing)
            if (!string.IsNullOrWhiteSpace(dto.Language))
            {
                try
                {
                    _catalogManager.AddLanguageIfNotExists(dto.Language);
                }
                catch
                {
                    // ignore
                }
            }

            // Add authors/editors depending on selected resource group.
            // Physical book
            if (GetSelectedResourceType() == ResourceType.PhysicalBook || GetSelectedResourceType() == null)
            {
                foreach (var authorName in _authors)
                {
                    dto.Authors.Add(new DTOBookAuthor
                    {
                        AuthorName = authorName,
                        Role = "Author",
                        IsPrimaryAuthor = dto.Authors.Count == 0 // First author is primary
                    });
                }

                foreach (var editorName in _editors)
                {
                    dto.Editors.Add(new DTOBookAuthor
                    {
                        AuthorName = editorName,
                        Role = "Editor",
                        IsPrimaryAuthor = false
                    });
                }
            }
            else if (GetSelectedResourceType() == ResourceType.Periodical)
            {
                foreach (var authorName in _prAuthors)
                {
                    dto.Authors.Add(new DTOBookAuthor
                    {
                        AuthorName = authorName,
                        Role = "Author",
                        IsPrimaryAuthor = dto.Authors.Count == 0
                    });
                }

                foreach (var editorName in _prEditors)
                {
                    dto.Editors.Add(new DTOBookAuthor
                    {
                        AuthorName = editorName,
                        Role = "Editor",
                        IsPrimaryAuthor = false
                    });
                }
            }
            else if (GetSelectedResourceType() == ResourceType.Thesis)
            {
                foreach (var authorName in _thAuthors)
                {
                    dto.Authors.Add(new DTOBookAuthor
                    {
                        AuthorName = authorName,
                        Role = "Author",
                        IsPrimaryAuthor = dto.Authors.Count == 0
                    });
                }

                foreach (var adviserName in _thAdvisers)
                {
                    dto.Editors.Add(new DTOBookAuthor
                    {
                        AuthorName = adviserName,
                        Role = "Adviser",
                        IsPrimaryAuthor = false
                    });
                }
            }
            else if (GetSelectedResourceType() == ResourceType.AV)
            {
                foreach (var authorName in _avAuthors)
                {
                    dto.Authors.Add(new DTOBookAuthor
                    {
                        AuthorName = authorName,
                        Role = "Author",
                        IsPrimaryAuthor = dto.Authors.Count == 0
                    });
                }

                foreach (var editorName in _avEditors)
                {
                    dto.Editors.Add(new DTOBookAuthor
                    {
                        AuthorName = editorName,
                        Role = "Editor",
                        IsPrimaryAuthor = false
                    });
                }
            }
            else if (GetSelectedResourceType() == ResourceType.EBook)
            {
                foreach (var authorName in _ebAuthors)
                {
                    dto.Authors.Add(new DTOBookAuthor
                    {
                        AuthorName = authorName,
                        Role = "Author",
                        IsPrimaryAuthor = dto.Authors.Count == 0
                    });
                }

                foreach (var editorName in _ebEditors)
                {
                    dto.Editors.Add(new DTOBookAuthor
                    {
                        AuthorName = editorName,
                        Role = "Editor",
                        IsPrimaryAuthor = false
                    });
                }
            }

            // Ensure any authors/editors/advisers that don't exist in DB are created now (user requested)
            try
            {
                var namesToEnsure = dto.Authors.Select(a => a.AuthorName)
                    .Concat(dto.Editors.Select(e => e.AuthorName))
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                foreach (var name in namesToEnsure)
                {
                    try
                    {
                        var existing = _authorRepo?.GetByName(name);
                        if (existing == null)
                        {
                            var newAuthorId = _authorRepo.Add(new Model.Models.Catalog.Author { FullName = name });
                            // we do not need to propagate the id into DTOBookAuthor since AddBookService will match by name
                        }
                    }
                    catch
                    {
                        // ignore single create failure
                    }
                }
            }
            catch
            {
                // ignore
            }

            // Get category ID if existing category selected (redundant with GetOrCreate but keep for safety)
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

        private ResourceType? GetSelectedResourceType()
        {
            if (RdoBtnPhysicalBook != null && RdoBtnPhysicalBook.Checked) return ResourceType.PhysicalBook;
            if (RdoBtnEBook != null && RdoBtnEBook.Checked) return ResourceType.EBook;
            if (RdoBtnTheses != null && RdoBtnTheses.Checked) return ResourceType.Thesis;
            if (RdoBtnPeriodical != null && RdoBtnPeriodical.Checked) return ResourceType.Periodical;
            if (RdoBtnAV != null && RdoBtnAV.Checked) return ResourceType.AV;

            // No radio selected -> return null so callers can decide behaviour.
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

            // Enter key should act like pressing Add button where applicable
            comboBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (comboBox == CmbBxBKAuthors) BtnBKAddAuthor.PerformClick();
                    else if (comboBox == CmbBxBKEditor) BtnBKAddEditor.PerformClick();
                    else if (comboBox == CmbBxPRAuthors) BtnPRAddAuthors.PerformClick();
                    else if (comboBox == CmbBxPREditors) BtnPRAddEditors.PerformClick();
                    else if (comboBox == CmbBxTHAuthors) BtnTHAddAuthors.PerformClick();
                    else if (comboBox == CmbBxTHAdvisers) BtnTHAddAdvisers.PerformClick();
                    else if (comboBox == CmbBxAVAuthors) BtnAVAddAuthors.PerformClick();
                    else if (comboBox == CmbBxAVEditors) BtnAVAddEditors.PerformClick();
                    else if (comboBox == CmbBxEBAuthors) BtnEBAddAuthors.PerformClick();
                    else if (comboBox == CmbBxEBEditors) BtnEBAddEditors.PerformClick();
                    // publisher combobox has no "Add" button; languages/categories not add-buttons

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

        private void PRMaterialFormat_CheckedChanged(object sender, EventArgs e)
        {
            // periodicals: panel3 = physical controls, panel4 = digital controls (designer names)
            if (PnlPRPhysicalFormat != null) PnlPRPhysicalFormat.Visible = RdoBtnPRPhysical.Checked;
            if (PnlPRDigitalFormat != null) PnlPRDigitalFormat.Visible = RdoBtnPRDigital.Checked;

            // make sure copy information visibility is updated when material format changes
            UpdateCopyInformationVisibility();
        }

        private void THMaterialFormat_CheckedChanged(object sender, EventArgs e)
        {
            // thesis: panel1 = physical controls, panel2 = digital controls (designer names)
            if (PnlTHPhysicalFormat != null) PnlTHPhysicalFormat.Visible = RdoBtnTHPhysical.Checked;
            if (PnlTHDigitalFormat != null) PnlTHDigitalFormat.Visible = RdoBtnTHDigital.Checked;

            UpdateCopyInformationVisibility();
        }

        private void AVMaterialFormat_CheckedChanged(object sender, EventArgs e)
        {
            // audio-visual: panel5 = physical controls, panel6 = digital controls (designer names)
            if (PnlAVPhysicalFormat != null) PnlAVPhysicalFormat.Visible = RdoBtnAVPhysical.Checked;
            if (PnlAVDigitalFormat != null) PnlAVDigitalFormat.Visible = RdoBtnAVDigital.Checked;

            UpdateCopyInformationVisibility();
        }
    }
}
