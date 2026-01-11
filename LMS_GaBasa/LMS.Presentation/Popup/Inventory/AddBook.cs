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
using System.Text.RegularExpressions;

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

            // Fix: create CatalogManager using the constructor that matches available repository instances.
            // The previous call `new CatalogManager(categoryRepo, languageRepo)` did not match any overload
            // and caused the compiler error. CatalogManager expects either the parameterless ctor or the
            // five-repository ctor. We already have the required repository instances, so pass them.
            _catalogManager = new CatalogManager(
                bookRepo,
                bookCopyRepo,
                bookAuthorRepo,
                authorRepo,
                categoryRepo);

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
                // Exclude any authors who appear in BookAuthor with role = "Editor" or role = "Adviser"
                string[] authorNames;
                try
                {
                    var allAuthors = _authorRepo?.GetAll() ?? new List<Model.Models.Catalog.Author>();

                    var editorIds = new HashSet<int>(_bookAuthorRepo?.GetDistinctAuthorIdsByRole("Editor") ?? Enumerable.Empty<int>());
                    var adviserIds = new HashSet<int>(_bookAuthorRepo?.GetDistinctAuthorIdsByRole("Adviser") ?? Enumerable.Empty<int>());

                    authorNames = allAuthors
                        .Where(a => !string.IsNullOrWhiteSpace(a.FullName) && !editorIds.Contains(a.AuthorID) && !adviserIds.Contains(a.AuthorID))
                        .Select(a => a.FullName.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                }
                catch
                {
                    authorNames = new string[0];
                }

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
            BtnPRAddEditors.Click += (s, e) => AddFromCombo(CmbBxPREditors, LstBxPREditors, _prEditors); LstBxPRAuthors.DoubleClick += (s, e) => RemoveSelectedFromList(LstBxPRAuthors, _prAuthors);
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
            // Thesis pages also limited to 10 digits as requested
            EnforceDigitsLimit(TxtBxTHNoOfPages, 10);

            // EB pages limit: enforce max 10 digits for eBook pages input
            EnforceDigitsLimit(TxtEBNoOfPages, 10);

            // AV duration should accept digits only (no unit suffix). Limit to reasonable number of digits (e.g. 6)
            EnforceDigitsLimit(TxtAVDuration, 6);
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

            // Always show for physical books
            if (RdoBtnPhysicalBook != null && RdoBtnPhysicalBook.Checked)
                showCopyInfo = true;

            // Show copy info for Periodical only when Periodical is the selected resource type and its Physical radio is checked
            if (GetSelectedResourceType() == ResourceType.Periodical
                && RdoBtnPRPhysical != null && RdoBtnPRPhysical.Checked)
            {
                showCopyInfo = true;
            }

            // Show copy info for Thesis only when Thesis is the selected resource type and its Physical radio is checked
            if (GetSelectedResourceType() == ResourceType.Thesis
                && RdoBtnTHPhysical != null && RdoBtnTHPhysical.Checked)
            {
                showCopyInfo = true;
            }

            // Show copy info for AV only when AV is the selected resource type and its Physical radio is checked
            if (GetSelectedResourceType() == ResourceType.AV
                && RdoBtnAVPhysical != null && RdoBtnAVPhysical.Checked)
            {
                showCopyInfo = true;
            }

            if (GrpBxCopyInformation != null)
                GrpBxCopyInformation.Visible = showCopyInfo;

            // refresh layout so visibility change is reflected immediately
            flowLayoutPanel1?.PerformLayout();
            flowLayoutPanel1?.Refresh();
        }

        private void LoadCategories()
        {
            // Populate category comboboxes (BK, EB, AV) from catalog manager.
            // Use distinct non-empty names and set up autocomplete for each combobox so user can pick existing or type a new one.
            var categories = _catalogManager.GetAllCategories();
            var names = categories?
                .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                .Select(c => c.Name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray() ?? new string[0];

            // Ensure autocomplete + editable behavior
            SetupComboBoxForAutocomplete(CmbBxBKCategory, names);
            SetupComboBoxForAutocomplete(CmbBxEBCategory, names);
            SetupComboBoxForAutocomplete(CmbBxAVCategory, names);

            // Fill dropdown items so the list is visible in the UI
            CmbBxBKCategory.Items.Clear();
            CmbBxEBCategory.Items.Clear();
            CmbBxAVCategory.Items.Clear();

            if (names.Length > 0)
            {
                CmbBxBKCategory.Items.AddRange(names);
                CmbBxEBCategory.Items.AddRange(names);
                CmbBxAVCategory.Items.AddRange(names);
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

            UpdateCopyInformationVisibility();
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
                var selectedType = GetSelectedResourceType() ?? ResourceType.PhysicalBook;

                // 1) Standard identifier validation first (ISBN / ISSN / DOI / UPC)
                string standardIdError;
                switch (selectedType)
                {
                    case ResourceType.PhysicalBook:
                        if (!ValidateISBN(TxtBKISBN.Text?.Trim(), out standardIdError))
                        {
                            MessageBox.Show(standardIdError, "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtBKISBN.Focus();
                            return;
                        }
                        break;

                    case ResourceType.Periodical:
                        if (!ValidateISSN(TxtPRISSN.Text?.Trim(), out standardIdError))
                        {
                            MessageBox.Show(standardIdError, "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtPRISSN.Focus();
                            return;
                        }
                        break;

                    case ResourceType.Thesis:
                        if (!ValidateDOI(TxtTHDOI.Text?.Trim(), out standardIdError))
                        {
                            MessageBox.Show(standardIdError, "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtTHDOI.Focus();
                            return;
                        }
                        break;

                    case ResourceType.AV:
                        if (!ValidateUPCISAN(TxtAVUPCISAN.Text?.Trim(), out standardIdError))
                        {
                            MessageBox.Show(standardIdError, "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtAVUPCISAN.Focus();
                            return;
                        }
                        break;

                    case ResourceType.EBook:
                        if (!ValidateISBN(TxtEBISBN.Text?.Trim(), out standardIdError))
                        {
                            MessageBox.Show(standardIdError, "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtEBISBN.Focus();
                            return;
                        }
                        break;
                }

                // 2) Perform resource-type specific validations in the requested order BEFORE building DTO.
                // This avoids confusing duplicate or out-of-order error messages.
                if (selectedType == ResourceType.PhysicalBook)
                {
                    // Order:
                    // 1. ISBN (already validated)
                    // 2. Call Number
                    if (string.IsNullOrWhiteSpace(TxtBKCallNumber.Text))
                    {
                        MessageBox.Show("Call number is required.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKCallNumber.Focus();
                        return;
                    }

                    // 3. Title
                    if (string.IsNullOrWhiteSpace(TxtBKTitle.Text))
                    {
                        MessageBox.Show("Title is required.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKTitle.Focus();
                        return;
                    }

                    // 4. Authors
                    if (_authors == null || _authors.Count == 0)
                    {
                        MessageBox.Show("Please add at least one author.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxBKAuthors?.Focus();
                        return;
                    }

                    // 5. Publisher (required and must contain a letter)
                    var pubText = CmbBxBKPublisher.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(pubText))
                    {
                        MessageBox.Show("Publisher is required.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxBKPublisher.Focus();
                        return;
                    }
                    if (!ContainsLetter(pubText))
                    {
                        MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxBKPublisher.Focus();
                        return;
                    }

                    // 6. Publication Year (presence, digits-only, not future)
                    var bkPubYear = TxtBKPublicationYear.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(bkPubYear))
                    {
                        MessageBox.Show("Publication year is required.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKPublicationYear.Focus();
                        return;
                    }
                    if (bkPubYear.Any(char.IsLetter))
                    {
                        MessageBox.Show("Publication year must contain digits only.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKPublicationYear.Focus();
                        return;
                    }
                    int bkYear = ParseInt(bkPubYear);
                    if (bkYear > DateTime.Now.Year)
                    {
                        MessageBox.Show("Publication year cannot be in the future.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKPublicationYear.Focus();
                        return;
                    }

                    // 7. Category
                    if (string.IsNullOrWhiteSpace(CmbBxBKCategory.Text))
                    {
                        MessageBox.Show("Category is required for physical books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxBKCategory.Focus();
                        return;
                    }

                    // 8. Number of Pages (required, positive)
                    var pagesTextBK = TxtBKNoOfPages.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(pagesTextBK))
                    {
                        MessageBox.Show("Number of pages is required.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKNoOfPages.Focus();
                        return;
                    }
                    int pagesBK = ParseInt(pagesTextBK);
                    if (pagesBK <= 0)
                    {
                        MessageBox.Show("Number of pages should be a positive integer.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKNoOfPages.Focus();
                        return;
                    }

                    // 9. Physical Description
                    if (string.IsNullOrWhiteSpace(TxtBKPhysicalDescription.Text))
                    {
                        MessageBox.Show("Physical description is required for physical books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBKPhysicalDescription.Focus();
                        return;
                    }

                    // 10. LoanType (Circulation or Reference)
                    if ((RdoBtnBKCirculation == null || !RdoBtnBKCirculation.Checked)
                        && (RdoBtnBKReference == null || !RdoBtnBKReference.Checked))
                    {
                        MessageBox.Show("Please select a loan type (Circulation or Reference) for physical books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (selectedType == ResourceType.Periodical)
                {
                    // Order:
                    // 1. ISSN (already validated)
                    // 2. Call Number
                    if (string.IsNullOrWhiteSpace(TxtPRCsllNumber.Text))
                    {
                        MessageBox.Show("Call number is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRCsllNumber.Focus();
                        return;
                    }

                    // 3. Title
                    if (string.IsNullOrWhiteSpace(TxtPRTitle.Text))
                    {
                        MessageBox.Show("Title is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRTitle.Focus();
                        return;
                    }

                    // 4. Authors
                    if (_prAuthors == null || _prAuthors.Count == 0)
                    {
                        MessageBox.Show("Please add at least one author for the periodical.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxPRAuthors?.Focus();
                        return;
                    }

                    // 5. Publisher
                    var prPub = CmbBxPRPublisher.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(prPub))
                    {
                        MessageBox.Show("Publisher is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxPRPublisher.Focus();
                        return;
                    }
                    if (!ContainsLetter(prPub))
                    {
                        MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxPRPublisher.Focus();
                        return;
                    }

                    // 6. Publication Date (presence). We allow formats but require presence.
                    var prPubDate = TxtPRPubDate.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(prPubDate))
                    {
                        MessageBox.Show("Publication date is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRPubDate.Focus();
                        return;
                    }

                    // 7. Volume
                    if (string.IsNullOrWhiteSpace(TxtPRVolume.Text))
                    {
                        MessageBox.Show("Volume is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRVolume.Focus();
                        return;
                    }

                    // 8. Issue
                    if (string.IsNullOrWhiteSpace(TxtPRIssue.Text))
                    {
                        MessageBox.Show("Issue is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRIssue.Focus();
                        return;
                    }

                    // 9. Pages
                    var prPagesText = TxtPRPages.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(prPagesText))
                    {
                        MessageBox.Show("Pages is required for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRPages.Focus();
                        return;
                    }
                    int prPages = ParseInt(prPagesText);
                    if (prPages <= 0)
                    {
                        MessageBox.Show("Pages must be a positive integer.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtPRPages.Focus();
                        return;
                    }

                    // 10. Material Format (Physical vs Digital) — require selection and related fields
                    if ((RdoBtnPRPhysical == null || !RdoBtnPRPhysical.Checked)
                        && (RdoBtnPRDigital == null || !RdoBtnPRDigital.Checked))
                    {
                        MessageBox.Show("Please select material format (Physical or Digital) for periodicals.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (RdoBtnPRPhysical != null && RdoBtnPRPhysical.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(CmbBxPRPhysicalDescription.Text))
                        {
                            MessageBox.Show("Please select a physical description for the periodical.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxPRPhysicalDescription.Focus();
                            return;
                        }

                        if (NumPckNoOfCopies.Value <= 0)
                        {
                            MessageBox.Show("Please specify the number of copies for a physical periodical.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            NumPckNoOfCopies.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(CmbBxCopyStatus.Text))
                        {
                            MessageBox.Show("Please select a copy status for the periodical copies.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxCopyStatus.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtLocation.Text))
                        {
                            MessageBox.Show("Please provide a location for the periodical copies.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtLocation.Focus();
                            return;
                        }
                    }
                    else if (RdoBtnPRDigital != null && RdoBtnPRDigital.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(CmbBxPRFormat.Text))
                        {
                            MessageBox.Show("Please select a digital format for the periodical.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxPRFormat.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtPRDownloadURL.Text))
                        {
                            MessageBox.Show("Please provide a Download URL for the digital periodical.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtPRDownloadURL.Focus();
                            return;
                        }
                    }
                }
                else if (selectedType == ResourceType.Thesis)
                {
                    // Order:
                    // 1. DOI (already validated)
                    // 2. Call Number
                    if (string.IsNullOrWhiteSpace(TxtTHCallNumber.Text))
                    {
                        MessageBox.Show("Call number is required for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtTHCallNumber.Focus();
                        return;
                    }

                    // 3. Title
                    if (string.IsNullOrWhiteSpace(TxtTHTitle.Text))
                    {
                        MessageBox.Show("Title is required for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtTHTitle.Focus();
                        return;
                    }

                    // 4. Author(s)
                    if (_thAuthors == null || _thAuthors.Count == 0)
                    {
                        MessageBox.Show("Please add at least one author for the thesis.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxTHAuthors?.Focus();
                        return;
                    }

                    // 5. Adviser(s)
                    if (_thAdvisers == null || _thAdvisers.Count == 0)
                    {
                        MessageBox.Show("Please add at least one adviser for the thesis.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxTHAdvisers?.Focus();
                        return;
                    }

                    // 6. Publisher
                    var thPub = CmbBxTHPublisher.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(thPub))
                    {
                        MessageBox.Show("Publisher is required for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxTHPublisher.Focus();
                        return;
                    }
                    if (!ContainsLetter(thPub))
                    {
                        MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxTHPublisher.Focus();
                        return;
                    }

                    // 7. Publication year
                    var thPubYear = TxtBxTHPublicationYear.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(thPubYear))
                    {
                        MessageBox.Show("Publication year is required for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBxTHPublicationYear.Focus();
                        return;
                    }
                    if (thPubYear.Any(char.IsLetter))
                    {
                        MessageBox.Show("Publication year must contain digits only.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBxTHPublicationYear.Focus();
                        return;
                    }
                    int thYear = ParseInt(thPubYear);
                    if (thYear > DateTime.Now.Year)
                    {
                        MessageBox.Show("Publication year cannot be in the future.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBxTHPublicationYear.Focus();
                        return;
                    }

                    // 8. Degree Level
                    if (string.IsNullOrWhiteSpace(CmbBxTHDegreeLevel.Text))
                    {
                        MessageBox.Show("Degree level is required for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxTHDegreeLevel.Focus();
                        return;
                    }

                    // 9. Number of Pages
                    var thPagesText = TxtBxTHNoOfPages.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(thPagesText))
                    {
                        MessageBox.Show("Number of pages is required for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBxTHNoOfPages.Focus();
                        return;
                    }
                    int thPages = ParseInt(thPagesText);
                    if (thPages <= 0)
                    {
                        MessageBox.Show("Number of pages must be a positive integer for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBxTHNoOfPages.Focus();
                        return;
                    }

                    // 10. Material Format (Physical or Digital)
                    if ((RdoBtnTHPhysical == null || !RdoBtnTHPhysical.Checked)
                        && (RdoBtnTHDigital == null || !RdoBtnTHDigital.Checked))
                    {
                        MessageBox.Show("Please select material format (Physical or Digital) for theses.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (RdoBtnTHPhysical != null && RdoBtnTHPhysical.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(CmbBxTHPhysicalDescription.Text))
                        {
                            MessageBox.Show("Please select a physical description for the thesis.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxTHPhysicalDescription.Focus();
                            return;
                        }

                        if (NumPckNoOfCopies.Value <= 0)
                        {
                            MessageBox.Show("Please specify the number of copies for a physical thesis.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            NumPckNoOfCopies.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(CmbBxCopyStatus.Text))
                        {
                            MessageBox.Show("Please select a copy status for the thesis copies.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxCopyStatus.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtLocation.Text))
                        {
                            MessageBox.Show("Please provide a location for the thesis copies.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtLocation.Focus();
                            return;
                        }
                    }
                    else if (RdoBtnTHDigital != null && RdoBtnTHDigital.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(CmbBxTHFormat.Text))
                        {
                            MessageBox.Show("Please select a digital format for the thesis.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxTHFormat.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtTHDownloadURL.Text))
                        {
                            MessageBox.Show("Please provide a Download URL for the digital thesis.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtTHDownloadURL.Focus();
                            return;
                        }
                    }
                }
                else if (selectedType == ResourceType.AV)
                {
                    // Order:
                    // 1. UPC/ISAN (already validated)
                    // 2. Call Number
                    if (string.IsNullOrWhiteSpace(TxtAVCallNumber.Text))
                    {
                        MessageBox.Show("Call number is required for audio-visual resources.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVCallNumber.Focus();
                        return;
                    }

                    // 3. Title
                    if (string.IsNullOrWhiteSpace(TxtAVTitle.Text))
                    {
                        MessageBox.Show("Title is required for audio-visual resources.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVTitle.Focus();
                        return;
                    }

                    // 4. Author(s)
                    if (_avAuthors == null || _avAuthors.Count == 0)
                    {
                        MessageBox.Show("Please add at least one author for the AV resource.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxAVAuthors?.Focus();
                        return;
                    }

                    // 5. Publisher
                    var avPub = CmbBxAVPublisher.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(avPub))
                    {
                        MessageBox.Show("Publisher is required for audio-visual resources.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxAVPublisher.Focus();
                        return;
                    }
                    if (!ContainsLetter(avPub))
                    {
                        MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxAVPublisher.Focus();
                        return;
                    }

                    // 6. Publication Year
                    var avPubYear = TxtAVPublicationYear.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(avPubYear))
                    {
                        MessageBox.Show("Publication year is required for audio-visual resources.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVPublicationYear.Focus();
                        return;
                    }
                    if (avPubYear.Any(char.IsLetter))
                    {
                        MessageBox.Show("Publication year must contain digits only.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVPublicationYear.Focus();
                        return;
                    }
                    int avYear = ParseInt(avPubYear);
                    if (avYear > DateTime.Now.Year)
                    {
                        MessageBox.Show("Publication year cannot be in the future.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVPublicationYear.Focus();
                        return;
                    }

                    // 7. Category
                    if (string.IsNullOrWhiteSpace(CmbBxAVCategory.Text))
                    {
                        MessageBox.Show("Category is required for audio-visual resources.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxAVCategory.Focus();
                        return;
                    }

                    // 8. Duration (seconds)
                    var durationRaw = TxtAVDuration.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(durationRaw))
                    {
                        MessageBox.Show("Duration is required. Enter a number (seconds).", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVDuration.Focus();
                        return;
                    }
                    if (!durationRaw.All(char.IsDigit))
                    {
                        MessageBox.Show("Duration must contain digits only. Do not include H/M/S suffix.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVDuration.Focus();
                        return;
                    }
                    int durationSeconds = ParseInt(durationRaw);
                    if (durationSeconds <= 0)
                    {
                        MessageBox.Show("Duration must be a positive integer (seconds).", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtAVDuration.Focus();
                        return;
                    }

                    // 9. Material Format
                    if ((RdoBtnAVPhysical == null || !RdoBtnAVPhysical.Checked)
                        && (RdoBtnAVDigital == null || !RdoBtnAVDigital.Checked))
                    {
                        MessageBox.Show("Please select material format (Physical or Digital) for audio-visual resources.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (RdoBtnAVPhysical != null && RdoBtnAVPhysical.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(CmbBxAVPhysicalDescription.Text))
                        {
                            MessageBox.Show("Please select a physical description for the AV resource.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxAVPhysicalDescription.Focus();
                            return;
                        }

                        if (NumPckNoOfCopies.Value <= 0)
                        {
                            MessageBox.Show("Please specify the number of copies for a physical AV resource.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            NumPckNoOfCopies.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(CmbBxCopyStatus.Text))
                        {
                            MessageBox.Show("Please select a copy status for the AV copies.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxCopyStatus.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtLocation.Text))
                        {
                            MessageBox.Show("Please provide a location for the AV copies.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtLocation.Focus();
                            return;
                        }
                    }
                    else if (RdoBtnAVDigital != null && RdoBtnAVDigital.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(CmbBxAVFormat.Text))
                        {
                            MessageBox.Show("Please select a digital format for the AV resource.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CmbBxAVFormat.Focus();
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtAVDownloadURL.Text))
                        {
                            MessageBox.Show("Please provide a Download URL for the digital AV resource.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtAVDownloadURL.Focus();
                            return;
                        }
                    }
                }
                else if (selectedType == ResourceType.EBook)
                {
                    // Order:
                    // 1. ISBN (already validated)
                    // 2. Call Number
                    if (string.IsNullOrWhiteSpace(TxtEBCallNumber.Text))
                    {
                        MessageBox.Show("Call number is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBCallNumber.Focus();
                        return;
                    }

                    // 3. Title
                    if (string.IsNullOrWhiteSpace(TxtEBTitle.Text))
                    {
                        MessageBox.Show("Title is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBTitle.Focus();
                        return;
                    }

                    // 4. Authors
                    if (_ebAuthors == null || _ebAuthors.Count == 0)
                    {
                        MessageBox.Show("Please add at least one author for the e-book.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxEBAuthors?.Focus();
                        return;
                    }

                    // 5. Publisher
                    var ebPub = CmbBxEBPublisher.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(ebPub))
                    {
                        MessageBox.Show("Publisher is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxEBPublisher.Focus();
                        return;
                    }
                    if (!ContainsLetter(ebPub))
                    {
                        MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxEBPublisher.Focus();
                        return;
                    }

                    // 6. Publication Year
                    var ebPubYear = TxtEBPublicationYear.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(ebPubYear))
                    {
                        MessageBox.Show("Publication year is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBPublicationYear.Focus();
                        return;
                    }
                    if (ebPubYear.Any(char.IsLetter))
                    {
                        MessageBox.Show("Publication year must contain digits only.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBPublicationYear.Focus();
                        return;
                    }
                    int ebYear = ParseInt(ebPubYear);
                    if (ebYear > DateTime.Now.Year)
                    {
                        MessageBox.Show("Publication year cannot be in the future.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBPublicationYear.Focus();
                        return;
                    }

                    // 7. Category
                    if (string.IsNullOrWhiteSpace(CmbBxEBCategory.Text))
                    {
                        MessageBox.Show("Category is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxEBCategory.Focus();
                        return;
                    }

                    // 8. Number of Pages
                    var ebPages = TxtEBNoOfPages.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(ebPages))
                    {
                        MessageBox.Show("Number of pages is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBNoOfPages.Focus();
                        return;
                    }
                    int ebPagesInt = ParseInt(ebPages);
                    if (ebPagesInt <= 0)
                    {
                        MessageBox.Show("Number of pages must be a positive integer for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBNoOfPages.Focus();
                        return;
                    }

                    // 9. Format
                    if (string.IsNullOrWhiteSpace(CmbBxEBFormat.Text))
                    {
                        MessageBox.Show("Format is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CmbBxEBFormat.Focus();
                        return;
                    }

                    // 10. Download URL
                    if (string.IsNullOrWhiteSpace(TxtEBDownloadURL.Text))
                    {
                        MessageBox.Show("Download URL is required for e-books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtEBDownloadURL.Focus();
                        return;
                    }
                }

                // 3) Build DTO now that control-level validation order is satisfied.
                var dto = BuildDTOFromForm();

                // 4) Perform any DTO-dependent adjustments/validations (copies/format handling).
                // Periodical: ensure copies cleared for digital (BuildDTOFromForm already sets this, but ensure consistency)
                if (dto.ResourceType == ResourceType.Periodical)
                {
                    if (RdoBtnPRDigital != null && RdoBtnPRDigital.Checked)
                    {
                        dto.InitialCopyCount = 0;
                        dto.CopyStatus = string.Empty;
                        dto.CopyLocation = string.Empty;
                    }
                }

                // Thesis: if digital ensure no copies and download URL present; if physical require copy info (already validated above)
                if (dto.ResourceType == ResourceType.Thesis)
                {
                    if (RdoBtnTHDigital != null && RdoBtnTHDigital.Checked)
                    {
                        dto.InitialCopyCount = 0;
                        dto.CopyStatus = string.Empty;
                        dto.CopyLocation = string.Empty;
                    }
                }

                // AV: if digital ensure no copies and download URL present; if physical keep copies (validated above)
                if (dto.ResourceType == ResourceType.AV)
                {
                    if (RdoBtnAVDigital != null && RdoBtnAVDigital.Checked)
                    {
                        dto.InitialCopyCount = 0;
                        dto.CopyStatus = string.Empty;
                        dto.CopyLocation = string.Empty;
                    }
                }

                // EBook: always digital -> ensure no copies
                if (dto.ResourceType == ResourceType.EBook)
                {
                    dto.InitialCopyCount = 0;
                    dto.CopyStatus = string.Empty;
                    dto.CopyLocation = string.Empty;
                }

                // 5) Set AddedByID and run manager-level validation
                dto.AddedByID = Program.CurrentUserId;
                if (dto.AddedByID <= 0)
                {
                    MessageBox.Show("Unable to determine the current user. Please sign in before adding books.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!_inventoryManager.ValidateBookData(dto, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 6) Save
                var result = _inventoryManager.AddBook(dto);

                if (result.Success)
                {
                    try
                    {
                        if (result.AccessionNumbers != null && result.AccessionNumbers.Count > 0)
                        {
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
            // Create a DTO with defaults from physical (BK) controls — we will overwrite fields if a different resource type is selected.
            var dto = new DTOCreateBook
            {
                ISBN = TxtBKISBN.Text.Trim(),
                Title = TxtBKTitle.Text.Trim(),
                Subtitle = TxtBKSubtitle.Text.Trim(),
                Publisher = !string.IsNullOrWhiteSpace(CmbBxBKPublisher.Text) ? CmbBxBKPublisher.Text.Trim() : string.Empty,
                PublicationYear = ParseInt(TxtBKPublicationYear.Text),
                Edition = TxtBKEdition.Text.Trim(),
                // default category comes from BK combobox; may be overwritten below for EB/AV
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

            // Now, if a specific resource type is selected (other than PhysicalBook), overwrite DTO fields using that group's controls.
            var selectedType = GetSelectedResourceType() ?? ResourceType.PhysicalBook;
            dto.ResourceType = selectedType;

            switch (selectedType)
            {
                case ResourceType.PhysicalBook:
                    // already initialized from BK controls above
                    // Set LoanType from the new radio buttons - required for physical books
                    if (RdoBtnBKCirculation != null && RdoBtnBKCirculation.Checked) dto.LoanType = "Circulation";
                    else if (RdoBtnBKReference != null && RdoBtnBKReference.Checked) dto.LoanType = "Reference";
                    else dto.LoanType = null;
                    break;

                case ResourceType.Periodical:
                    // Store ISSN in ISBN DB column
                    dto.ISBN = TxtPRISSN.Text.Trim();

                    // Publication date -> store year only. Try to extract a 4-digit year from the input.
                    dto.PublicationYear = ExtractYearFromString(TxtPRPubDate.Text.Trim());

                    // Title / subtitle / publisher / language / call number / pages
                    dto.Title = TxtPRTitle.Text.Trim();
                    dto.Subtitle = TxtPRSubtitle.Text.Trim();
                    dto.Publisher = !string.IsNullOrWhiteSpace(CmbBxPRPublisher.Text) ? CmbBxPRPublisher.Text.Trim() : string.Empty;
                    dto.Language = CmbBxPRLanguage.Text.Trim();
                    dto.CallNumber = TxtPRCsllNumber.Text.Trim(); // designer name uses TxtPRCsllNumber
                    dto.Pages = ParseInt(TxtPRPages.Text);

                    // Edition should be "Vol. xx, No. xx" (combine volume + issue)
                    var vol = TxtPRVolume.Text?.Trim() ?? string.Empty;
                    var issue = TxtPRIssue.Text?.Trim() ?? string.Empty;
                    var editionParts = new List<string>();
                    if (!string.IsNullOrEmpty(vol)) editionParts.Add($"Vol. {vol}");
                    if (!string.IsNullOrEmpty(issue)) editionParts.Add($"No. {issue}");
                    dto.Edition = editionParts.Count > 0 ? string.Join(", ", editionParts) : string.Empty;

                    // Material format: physical vs digital
                    if (RdoBtnPRPhysical != null && RdoBtnPRPhysical.Checked)
                    {
                        // physical: use selected physical description, keep copy info
                        dto.PhysicalDescription = CmbBxPRPhysicalDescription.Text.Trim();
                        // keep copy information / counts as provided by Number control and Copy group
                        dto.InitialCopyCount = NumPckNoOfCopies.Value;
                        dto.CopyStatus = CmbBxCopyStatus.Text;
                        dto.CopyLocation = TxtLocation.Text.Trim();
                    }
                    else
                    {
                        // digital: store format and download URL; do NOT create copies
                        dto.PhysicalDescription = CmbBxPRFormat.Text.Trim();
                        dto.DownloadURL = TxtPRDownloadURL.Text.Trim();
                        dto.InitialCopyCount = 0;
                        dto.CopyStatus = string.Empty;
                        dto.CopyLocation = string.Empty;
                    }

                    // Periodical doesn't use LoanType
                    dto.LoanType = null;
                    break;

                case ResourceType.Thesis:
                    // DOI stored in ISBN field
                    dto.ISBN = TxtTHDOI.Text.Trim();

                    dto.Title = TxtTHTitle.Text.Trim();
                    dto.Subtitle = TxtTHSubtitle.Text.Trim();
                    dto.Publisher = !string.IsNullOrWhiteSpace(CmbBxTHPublisher.Text) ? CmbBxTHPublisher.Text.Trim() : string.Empty;
                    dto.Language = CmbBxTHLanguage.Text.Trim();
                    dto.CallNumber = TxtTHCallNumber.Text.Trim();
                    dto.Pages = ParseInt(TxtBxTHNoOfPages.Text);
                    dto.PublicationYear = ParseInt(TxtBxTHPublicationYear.Text);

                    // Degree level stored in Edition
                    dto.Edition = CmbBxTHDegreeLevel.Text.Trim();

                    // Material format handling for Thesis: ensure DownloadURL set for digital and copy info is cleared
                    if (RdoBtnTHPhysical != null && RdoBtnTHPhysical.Checked)
                    {
                        dto.PhysicalDescription = CmbBxTHPhysicalDescription.Text.Trim();
                        // keep copy information for physical theses
                        dto.InitialCopyCount = NumPckNoOfCopies.Value;
                        dto.CopyStatus = CmbBxCopyStatus.Text;
                        dto.CopyLocation = TxtLocation.Text.Trim();
                    }
                    else if (RdoBtnTHDigital != null && RdoBtnTHDigital.Checked)
                    {
                        dto.PhysicalDescription = CmbBxTHFormat.Text.Trim();
                        dto.DownloadURL = TxtTHDownloadURL.Text.Trim();
                        // Do not create copies for digital thesis
                        dto.InitialCopyCount = 0;
                        dto.CopyStatus = string.Empty;
                        dto.CopyLocation = string.Empty;
                    }
                    else
                    {
                        // default behaviour if no material format selected: preserve values already in dto (NumPckNoOfCopies etc.)
                    }

                    dto.LoanType = null;
                    break;

                case ResourceType.AV:
                    // UPC/ISAN stored in ISBN DB column
                    dto.ISBN = TxtAVUPCISAN.Text.Trim();

                    dto.Title = TxtAVTitle.Text.Trim();
                    dto.Subtitle = TxtAVSubtitle.Text.Trim();
                    dto.Publisher = !string.IsNullOrWhiteSpace(CmbBxAVPublisher.Text) ? CmbBxAVPublisher.Text.Trim() : string.Empty;
                    dto.Language = CmbBxAVLanguage.Text.Trim();
                    dto.CallNumber = TxtAVCallNumber.Text.Trim();
                    // AV duration: user now enters a number only (seconds). Store directly in Pages column.
                    dto.Pages = ParseInt(TxtAVDuration.Text.Trim());
                    dto.PublicationYear = ParseInt(TxtAVPublicationYear.Text);

                    // AV-specific category
                    dto.CategoryName = CmbBxAVCategory.Text.Trim();

                    // Material format: physical vs digital for AV
                    if (RdoBtnAVPhysical != null && RdoBtnAVPhysical.Checked)
                    {
                        dto.PhysicalDescription = CmbBxAVPhysicalDescription.Text.Trim();
                        // keep copy information for physical AV
                        dto.InitialCopyCount = NumPckNoOfCopies.Value;
                        dto.CopyStatus = CmbBxCopyStatus.Text;
                        dto.CopyLocation = TxtLocation.Text.Trim();
                    }
                    else if (RdoBtnAVDigital != null && RdoBtnAVDigital.Checked)
                    {
                        // digital: record format + download URL and ensure no copies are created
                        dto.PhysicalDescription = CmbBxAVFormat.Text.Trim();
                        dto.DownloadURL = TxtAVDownloadURL.Text.Trim();
                        dto.InitialCopyCount = 0;
                        dto.CopyStatus = string.Empty;
                        dto.CopyLocation = string.Empty;
                    }

                    dto.LoanType = null;
                    break;

                case ResourceType.EBook:
                    dto.ISBN = TxtEBISBN.Text.Trim();
                    dto.Title = TxtEBTitle.Text.Trim();
                    dto.Subtitle = TxtEBSubtitle.Text.Trim();
                    dto.Publisher = !string.IsNullOrWhiteSpace(CmbBxEBPublisher.Text) ? CmbBxEBPublisher.Text.Trim() : string.Empty;
                    dto.Language = CmbBxEBLanguage.Text.Trim();
                    dto.CallNumber = TxtEBCallNumber.Text.Trim();
                    dto.Pages = ParseInt(TxtEBNoOfPages.Text);
                    dto.PublicationYear = ParseInt(TxtEBPublicationYear.Text);

                    // Use EB category combobox value as category for eBooks
                    dto.CategoryName = CmbBxEBCategory.Text.Trim();

                    // Format is stored in PhysicalDescription for consistency with validation
                    dto.PhysicalDescription = CmbBxEBFormat.Text.Trim();

                    // Download URL for eBooks (make sure we populate this so validation sees it)
                    dto.DownloadURL = TxtEBDownloadURL.Text.Trim();

                    // eBooks are digital: do not create physical copies
                    dto.InitialCopyCount = 0;
                    dto.CopyStatus = string.Empty;
                    dto.CopyLocation = string.Empty;

                    dto.LoanType = null;
                    break;
            }

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
            if (selectedType == ResourceType.PhysicalBook)
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
            else if (selectedType == ResourceType.Periodical)
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
            else if (selectedType == ResourceType.Thesis)
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
            else if (selectedType == ResourceType.AV)
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
            else if (selectedType == ResourceType.EBook)
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
            {
                string selectedCategoryText = null;
                if (CmbBxBKCategory.SelectedIndex >= 0) selectedCategoryText = CmbBxBKCategory.Text;
                else if (CmbBxEBCategory != null && CmbBxEBCategory.SelectedIndex >= 0) selectedCategoryText = CmbBxEBCategory.Text;
                else if (CmbBxAVCategory != null && CmbBxAVCategory.SelectedIndex >= 0) selectedCategoryText = CmbBxAVCategory.Text;

                if (!string.IsNullOrWhiteSpace(selectedCategoryText))
                {
                    try
                    {
                        var categories = _catalogManager.GetAllCategories();
                        var selectedCat = categories.FirstOrDefault(c =>
                            c.Name.Equals(selectedCategoryText, StringComparison.OrdinalIgnoreCase));
                        if (selectedCat != null)
                        {
                            dto.CategoryID = selectedCat.CategoryID;
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }

            return dto;
        }

        private int ExtractYearFromString(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            // Try to find a 4-digit year like 1900-2099
            var m = Regex.Match(s, @"\b(19|20)\d{2}\b");
            if (m.Success)
            {
                if (int.TryParse(m.Value, out int y)) return y;
            }

            // Fallback: take last 4 digits from numeric characters if present
            var digits = new string((s ?? string.Empty).Where(char.IsDigit).ToArray());
            if (digits.Length >= 4)
            {
                var last4 = digits.Substring(digits.Length - 4);
                if (int.TryParse(last4, out int y2)) return y2;
            }

            // final fallback to ParseInt (may return 0)
            return ParseInt(s);
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

            // When switching format adjust copy-info visibility and enable/disable AV-specific controls
            // so AV Digital does NOT require copies/status/location and AV Physical DOES require them.
            SetAVDigitalUIState(RdoBtnAVDigital != null && RdoBtnAVDigital.Checked);

            UpdateCopyInformationVisibility();
        }

        private void SetAVDigitalUIState(bool isDigital)
        {
            // If the AV resource group is not the current selection, don't force UI state for copy group
            // The caller still calls UpdateCopyInformationVisibility afterwards.
            // Enable/disable AV-specific format/download vs physical-description & copy controls.
            try
            {
                if (CmbBxAVFormat != null) CmbBxAVFormat.Enabled = isDigital;
                if (TxtAVDownloadURL != null) TxtAVDownloadURL.Enabled = isDigital;

                if (CmbBxAVPhysicalDescription != null) CmbBxAVPhysicalDescription.Enabled = !isDigital;

                // Copy-related controls are grouped in GrpBxCopyInformation — toggle their enabled state
                // but leave actual Visible decision to UpdateCopyInformationVisibility (it considers selected resource).
                if (NumPckNoOfCopies != null) NumPckNoOfCopies.Enabled = !isDigital;
                if (CmbBxCopyStatus != null) CmbBxCopyStatus.Enabled = !isDigital;
                if (TxtLocation != null) TxtLocation.Enabled = !isDigital;

                if (isDigital)
                {
                    // Clear copy inputs to avoid accidental validation / retention when switching back and forth.
                    try { NumPckNoOfCopies.Value = 0; } catch { /* ignore if control absent */ }
                    if (CmbBxCopyStatus != null) CmbBxCopyStatus.Text = string.Empty;
                    if (TxtLocation != null) TxtLocation.Text = string.Empty;
                }
                else
                {
                    // When switching back to physical, restore sensible defaults if empty
                    if (NumPckNoOfCopies != null && NumPckNoOfCopies.Value <= 0)
                    {
                        try { NumPckNoOfCopies.Value = 1; } catch { /* ignore */ }
                    }
                    if (string.IsNullOrWhiteSpace(CmbBxCopyStatus?.Text) && CmbBxCopyStatus != null)
                    {
                        CmbBxCopyStatus.SelectedIndex = Math.Max(0, CmbBxCopyStatus.SelectedIndex);
                    }
                }
            }
            catch
            {
                // Non-fatal: don't let UI errors block user flow.
            }
        }

        private void RdoBtnBKReference_CheckedChanged(object sender, EventArgs e)
        {
            // No UI side-effects required here; the LoanType is read during Save/BuildDTO.
            // This handler exists only to satisfy the designer event wiring and avoid the compile error.
            // If you want immediate behavior when switching to Reference (e.g., toggle controls), add it here.
        }

        private void LblAVLanguage_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateISBN(string isbn, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrWhiteSpace(isbn))
            {
                errorMessage = "ISBN is required.";
                return false;
            }

            // Remove any non-digit characters for counting
            string digitsOnly = new string(isbn.Where(char.IsDigit).ToArray());
            int digitCount = digitsOnly.Length;

            if (digitCount != 10 && digitCount != 13 && digitCount != 17)
            {
                errorMessage = "ISBN must be exactly 10, 13, or 17 digits.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates ISSN format: only numbers, letters, and hyphens; 8-9 characters total.
        /// </summary>
        private bool ValidateISSN(string issn, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrWhiteSpace(issn))
            {
                errorMessage = "ISSN is required.";
                return false;
            }

            // Check allowed characters: letters, digits, hyphens only
            if (!Regex.IsMatch(issn, @"^[A-Za-z0-9\-]+$"))
            {
                errorMessage = "ISSN must contain only numbers, letters, and hyphens.";
                return false;
            }

            if (issn.Length < 8 || issn.Length > 9)
            {
                errorMessage = "ISSN must be 8-9 characters.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates DOI format: must start with "10." and contain "/".
        /// </summary>
        private bool ValidateDOI(string doi, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrWhiteSpace(doi))
            {
                errorMessage = "DOI is required.";
                return false;
            }

            if (!doi.StartsWith("10."))
            {
                errorMessage = "DOI must start with \"10.\"";
                return false;
            }

            if (!doi.Contains("/"))
            {
                errorMessage = "DOI must contain a \"/\" character.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates UPC/ISAN format: letters, numbers, hyphens only; must be 12, 15, 24, or 32 characters.
        /// </summary>
        private bool ValidateUPCISAN(string upcIsan, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrWhiteSpace(upcIsan))
            {
                errorMessage = "UPC/ISAN is required.";
                return false;
            }

            // Check allowed characters: letters, digits, hyphens only
            if (!Regex.IsMatch(upcIsan, @"^[A-Za-z0-9\-]+$"))
            {
                errorMessage = "UPC/ISAN must contain only letters, numbers, and hyphens.";
                return false;
            }

            int len = upcIsan.Length;
            if (len != 12 && len != 15 && len != 24 && len != 32)
            {
                errorMessage = "UPC/ISAN must be 12, 15, 24, or 32 characters.";
                return false;
            }

            return true;
        }
    }
}
