using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Database;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Book;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Enums;
using LMS.Presentation.BarcodeGenerator;
using LMS.BusinessLogic.Services.BarcodeGenerator;
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
        // Repositories / managers (keep concrete repos for simple migration; can be swapped to interfaces if DI added)
        private readonly BookRepository _bookRepo;
        private readonly BookAuthorRepository _bookAuthorRepo;
        private readonly AuthorRepository _authorRepo;
        private readonly PublisherRepository _publisherRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly ICatalogManager _catalogManager;

        // Barcode generator used when regenerating accessions/barcodes
        private readonly IBarcodeGenerator _barcodeGenerator;

        private Book _book;
        private ResourceType _originalResourceType; // track original type
        private string _selectedCoverPath; // local image path selected by user (to copy on save)

        // Per-resource lists for AddFromCombo backing (parity with AddBook)
        private List<string> _authors = new List<string>(); // BK
        private List<string> _editors = new List<string>(); // BK
        private List<string> _prAuthors = new List<string>();
        private List<string> _prEditors = new List<string>();
        private List<string> _thAuthors = new List<string>();
        private List<string> _thAdvisers = new List<string>();
        private List<string> _avAuthors = new List<string>();
        private List<string> _avEditors = new List<string>();
        private List<string> _ebAuthors = new List<string>();
        private List<string> _ebEditors = new List<string>();

        public EditBook() : this(null) { }

        // Primary constructor
        public EditBook(Book book)
        {
            InitializeComponent();

            // Initialize repositories similarly to AddBook designer ctor to preserve behavior when DI not present.
            var dbConn = new DbConnection();
            _bookRepo = new BookRepository(dbConn);
            _bookAuthorRepo = new BookAuthorRepository(new DbConnection());
            _authorRepo = new AuthorRepository(new DbConnection());
            _publisherRepo = new PublisherRepository(new DbConnection());
            _category_repo_init();
            _categoryRepo = new CategoryRepository(new DbConnection());

            // Catalog manager for categories/languages
            var languageRepo = new LanguageRepository(new DbConnection());
            _catalogManager = new CatalogManager(_categoryRepo, languageRepo);

            // barcode generator for potential regeneration (same folder as AddBook)
            string barcodesFolder = Path.Combine(Application.StartupPath, "Assets", "dataimages", "BookCopyBarcodes");
            _barcodeGenerator = new LMS.Presentation.BarcodeGenerator.ZXingBarcodeGenerator(
                barcodesFolder, ZXing.BarcodeFormat.CODE_128, width: 300, height: 100, margin: 10);

            _book = book;
            _originalResourceType = book?.ResourceType ?? ResourceType.PhysicalBook;

            // Wire events and initialize UI state + suggestions
            WireUpEvents();

            LoadCategories();
            LoadLanguages();
            LoadAuthors();     // populates all authors comboboxes
            LoadEditors();     // populates editor/adviser comboboxes
            LoadPublishers();  // populates all publisher comboboxes

            // enforce sensible digit limits like AddBook
            EnforceDigitsLimit(TxtBKNoOfPages, 10);
            EnforceDigitsLimit(TxtBxTHNoOfPages, 10);
            EnforceDigitsLimit(TxtEBNoOfPages, 10);
            EnforceDigitsLimit(TxtAVDuration, 6);

            // Make sure panels / copy info reflect current (possibly none) selection
            SyncPanelsForResourceType();
            UpdateCopyInformationVisibility();

            if (_book != null)
                LoadBookToForm(_book);
        }

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

        private void _category_repo_init()
        {
            // placeholder (kept for parity with previous iterations)
        }

        private void WireUpEvents()
        {
            // Unsubscribe then subscribe to avoid double subscriptions when reinitializing
            if (PicBxBookCover != null) { PicBxBookCover.Click -= PicBxBookCover_Click; PicBxBookCover.Click += PicBxBookCover_Click; }

            // Physical book add/remove
            if (BtnBKAddAuthor != null) { BtnBKAddAuthor.Click -= BtnBKAddAuthor_Click; BtnBKAddAuthor.Click += BtnBKAddAuthor_Click; }
            if (BtnBKAddEditor != null) { BtnBKAddEditor.Click -= BtnBKAddEditor_Click; BtnBKAddEditor.Click += BtnBKAddEditor_Click; }
            if (LstBxBKAuthor != null) { LstBxBKAuthor.DoubleClick -= LstBxBKAuthor_DoubleClick; LstBxBKAuthor.DoubleClick += LstBxBKAuthor_DoubleClick; }
            if (LstBxBKEditor != null) { LstBxBKEditor.DoubleClick -= LstBxBKEditor_DoubleClick; LstBxBKEditor.DoubleClick += LstBxBKEditor_DoubleClick; }

            // Periodical
            if (BtnPRAddAuthors != null) { BtnPRAddAuthors.Click -= BtnPRAddAuthors_Click; BtnPRAddAuthors.Click += BtnPRAddAuthors_Click; }
            if (BtnPRAddEditors != null) { BtnPRAddEditors.Click -= BtnPRAddEditors_Click; BtnPRAddEditors.Click += BtnPRAddEditors_Click; }
            if (LstBxPRAuthors != null) { LstBxPRAuthors.DoubleClick -= LstBxPRAuthors_DoubleClick; LstBxPRAuthors.DoubleClick += LstBxPRAuthors_DoubleClick; }
            if (LstBxPREditors != null) { LstBxPREditors.DoubleClick -= LstBxPREditors_DoubleClick; LstBxPREditors.DoubleClick += LstBxPREditors_DoubleClick; }

            // Thesis
            if (BtnTHAddAuthors != null) { BtnTHAddAuthors.Click -= BtnTHAddAuthors_Click; BtnTHAddAuthors.Click += BtnTHAddAuthors_Click; }
            if (BtnTHAddAdvisers != null) { BtnTHAddAdvisers.Click -= BtnTHAddAdvisers_Click; BtnTHAddAdvisers.Click += BtnTHAddAdvisers_Click; }
            if (LstBxTHAuthors != null) { LstBxTHAuthors.DoubleClick -= LstBxTHAuthors_DoubleClick; LstBxTHAuthors.DoubleClick += LstBxTHAuthors_DoubleClick; }
            if (LstBxTHAdvisers != null) { LstBxTHAdvisers.DoubleClick -= LstBxTHAdvisers_DoubleClick; LstBxTHAdvisers.DoubleClick += LstBxTHAdvisers_DoubleClick; }

            // AV
            if (BtnAVAddAuthors != null) { BtnAVAddAuthors.Click -= BtnAVAddAuthors_Click; BtnAVAddAuthors.Click += BtnAVAddAuthors_Click; }
            if (BtnAVAddEditors != null) { BtnAVAddEditors.Click -= BtnAVAddEditors_Click; BtnAVAddEditors.Click += BtnAVAddEditors_Click; }
            if (LstBxAVAuthors != null) { LstBxAVAuthors.DoubleClick -= LstBxAVAuthors_DoubleClick; LstBxAVAuthors.DoubleClick += LstBxAVAuthors_DoubleClick; }
            if (LstBxAVEditors != null) { LstBxAVEditors.DoubleClick -= LstBxAVEditors_DoubleClick; LstBxAVEditors.DoubleClick += LstBxAVEditors_DoubleClick; }

            // EBook
            if (BtnEBAddAuthors != null) { BtnEBAddAuthors.Click -= BtnEBAddAuthors_Click; BtnEBAddAuthors.Click += BtnEBAddAuthors_Click; }
            if (BtnEBAddEditors != null) { BtnEBAddEditors.Click -= BtnEBAddEditors_Click; BtnEBAddEditors.Click += BtnEBAddEditors_Click; }
            if (LstBxEBAuthors != null) { LstBxEBAuthors.DoubleClick -= LstBxEBAuthors_DoubleClick; LstBxEBAuthors.DoubleClick += LstBxEBAuthors_DoubleClick; }
            if (LstBxEBeditors != null) { LstBxEBeditors.DoubleClick -= LstBxEBeditors_DoubleClick; LstBxEBeditors.DoubleClick += LstBxEBeditors_DoubleClick; }

            // common cancel/save
            if (BtnCancel != null) { BtnCancel.Click -= BtnCancel_Click; BtnCancel.Click += BtnCancel_Click; }
            if (BtnSave != null) { BtnSave.Click -= BtnSave_Click; BtnSave.Click += BtnSave_Click; }

            // Note: resource-type radio controls were removed from the designer by your change.
            // Do not wire non-existent controls here; panels will be shown based on the book being edited.

            // Material-format radios (Thesis / Periodical / AV) -> toggle physical/digital panels
            if (RdoBtnTHPhysical != null) { RdoBtnTHPhysical.CheckedChanged -= MaterialFormat_CheckedChanged; RdoBtnTHPhysical.CheckedChanged += MaterialFormat_CheckedChanged; }
            if (RdoBtnTHDigital != null) { RdoBtnTHDigital.CheckedChanged -= MaterialFormat_CheckedChanged; RdoBtnTHDigital.CheckedChanged += MaterialFormat_CheckedChanged; }

            if (RdoBtnPRPhysical != null) { RdoBtnPRPhysical.CheckedChanged -= MaterialFormat_CheckedChanged; RdoBtnPRPhysical.CheckedChanged += MaterialFormat_CheckedChanged; }
            if (RdoBtnPRDigital != null) { RdoBtnPRDigital.CheckedChanged -= MaterialFormat_CheckedChanged; RdoBtnPRDigital.CheckedChanged += MaterialFormat_CheckedChanged; }

            if (RdoBtnAVPhysical != null) { RdoBtnAVPhysical.CheckedChanged -= MaterialFormat_CheckedChanged; RdoBtnAVPhysical.CheckedChanged += MaterialFormat_CheckedChanged; }
            if (RdoBtnAVDigital != null) { RdoBtnAVDigital.CheckedChanged -= MaterialFormat_CheckedChanged; RdoBtnAVDigital.CheckedChanged += MaterialFormat_CheckedChanged; }
        }

        private void MaterialFormat_CheckedChanged(object sender, EventArgs e)
        {
            // Any material-format radio changed -> sync material format panels
            SyncMaterialFormatPanels();
        }

        private void SyncMaterialFormatPanels()
        {
            // Thesis: show/hide physical/digital format panels
            if (RdoBtnTHPhysical != null && PnlTHPhysicalFormat != null) PnlTHPhysicalFormat.Visible = RdoBtnTHPhysical.Checked;
            if (RdoBtnTHDigital != null && PnlTHDigitalFormat != null) PnlTHDigitalFormat.Visible = RdoBtnTHDigital.Checked;

            // Periodical: show/hide physical/digital format panels
            if (RdoBtnPRPhysical != null && PnlPRPhysicalFormat != null) PnlPRPhysicalFormat.Visible = RdoBtnPRPhysical.Checked;
            if (RdoBtnPRDigital != null && PnlPRDigitalFormat != null) PnlPRDigitalFormat.Visible = RdoBtnPRDigital.Checked;

            // AV: show/hide physical/digital format panels
            if (RdoBtnAVPhysical != null && PnlAVPhysicalFormat != null) PnlAVPhysicalFormat.Visible = RdoBtnAVPhysical.Checked;
            if (RdoBtnAVDigital != null && PnlAVDigitalFormat != null) PnlAVDigitalFormat.Visible = RdoBtnAVDigital.Checked;

            // Enable/clear download URL fields when appropriate
            if (TxtTHDownloadURL != null) TxtTHDownloadURL.Enabled = RdoBtnTHDigital != null && RdoBtnTHDigital.Checked;
            if (TxtPRDownloadURL != null) TxtPRDownloadURL.Enabled = RdoBtnPRDigital != null && RdoBtnPRDigital.Checked;
            if (TxtAVDownloadURL != null) TxtAVDownloadURL.Enabled = RdoBtnAVDigital != null && RdoBtnAVDigital.Checked;

            if (TxtTHDownloadURL != null && (RdoBtnTHDigital == null || !RdoBtnTHDigital.Checked)) TxtTHDownloadURL.Text = string.Empty;
            if (TxtPRDownloadURL != null && (RdoBtnPRDigital == null || !RdoBtnPRDigital.Checked)) TxtPRDownloadURL.Text = string.Empty;
            if (TxtAVDownloadURL != null && (RdoBtnAVDigital == null || !RdoBtnAVDigital.Checked)) TxtAVDownloadURL.Text = string.Empty;

            // Ensure copy-info visibility updated because material format may affect it
            UpdateCopyInformationVisibility();
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
                            if (PicBxBookCover.Image != null) { try { PicBxBookCover.Image.Dispose(); } catch { } }
                            PicBxBookCover.Image = new Bitmap(img);
                            PicBxBookCover.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    catch
                    {
                        // ignore load errors
                    }
                }
            }
        }

        // Populate category combobox from DB
        private void LoadCategories()
        {
            try
            {
                var categories = _catalogManager.GetAllCategories();
                var names = categories?
                    .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                    .Select(c => c.Name.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray() ?? new string[0];

                // populate all category comboboxes present in designer
                SetupComboBoxForAutocomplete(CmbBxBKCategory, names);
                SetupComboBoxForAutocomplete(CmbBxEBCategory, names);
                SetupComboBoxForAutocomplete(CmbBxAVCategory, names);

                if (names.Length > 0)
                {
                    CmbBxBKCategory.Items.Clear(); CmbBxBKCategory.Items.AddRange(names);
                    CmbBxEBCategory.Items.Clear(); CmbBxEBCategory.Items.AddRange(names);
                    CmbBxAVCategory.Items.Clear(); CmbBxAVCategory.Items.AddRange(names);
                }
            }
            catch
            {
                // ignore
            }
        }

        // Use the same approach as AddBook: ask the CatalogManager for languages and populate all language combos
        private void LoadLanguages()
        {
            try
            {
                var languages = _catalogManager?.GetAllLanguages();
                var arr = languages?.ToArray();
                SetupComboBoxForAutocomplete(CmbBxBKLanguage, arr);
                SetupComboBoxForAutocomplete(CmbBxPRLanguage, arr);
                SetupComboBoxForAutocomplete(CmbBxTHLanguage, arr);
                SetupComboBoxForAutocomplete(CmbBxAVLanguage, arr);
                SetupComboBoxForAutocomplete(CmbBxEBLanguage, arr);

                if (arr != null && arr.Length > 0)
                {
                    CmbBxBKLanguage.Items.Clear(); CmbBxBKLanguage.Items.AddRange(arr);
                    CmbBxPRLanguage.Items.Clear(); CmbBxPRLanguage.Items.AddRange(arr);
                    CmbBxTHLanguage.Items.Clear(); CmbBxTHLanguage.Items.AddRange(arr);
                    CmbBxAVLanguage.Items.Clear(); CmbBxAVLanguage.Items.AddRange(arr);
                    CmbBxEBLanguage.Items.Clear(); CmbBxEBLanguage.Items.AddRange(arr);
                }
            }
            catch
            {
                SetupComboBoxForAutocomplete(CmbBxBKLanguage, null);
                SetupComboBoxForAutocomplete(CmbBxPRLanguage, null);
                SetupComboBoxForAutocomplete(CmbBxTHLanguage, null);
                SetupComboBoxForAutocomplete(CmbBxAVLanguage, null);
                SetupComboBoxForAutocomplete(CmbBxEBLanguage, null);
            }
        }

        // Populate authors for suggestions and enable autocomplete + Enter-to-add across all resource comboboxes
        private void LoadAuthors()
        {
            try
            {
                // Fetch all authors, but exclude those who are known as Editors or Advisers in BookAuthor table
                var allAuthors = _authorRepo?.GetAll() ?? new List<Author>();

                var editorIds = new HashSet<int>(_bookAuthorRepo?.GetDistinctAuthorIdsByRole("Editor") ?? Enumerable.Empty<int>());
                var adviserIds = new HashSet<int>(_bookAuthorRepo?.GetDistinctAuthorIdsByRole("Adviser") ?? Enumerable.Empty<int>());

                var authorNames = allAuthors
                    .Where(a => !string.IsNullOrWhiteSpace(a.FullName) && !editorIds.Contains(a.AuthorID) && !adviserIds.Contains(a.AuthorID))
                    .Select(a => a.FullName.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                // populate every author combobox (authors only)
                SetupComboBoxForAutocomplete(CmbBxBKAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxPRAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxTHAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxAVAuthors, authorNames);
                SetupComboBoxForAutocomplete(CmbBxEBAuthors, authorNames);

                if (authorNames != null && authorNames.Length > 0)
                {
                    CmbBxBKAuthors.Items.Clear(); CmbBxBKAuthors.Items.AddRange(authorNames);
                    CmbBxPRAuthors.Items.Clear(); CmbBxPRAuthors.Items.AddRange(authorNames);
                    CmbBxTHAuthors.Items.Clear(); CmbBxTHAuthors.Items.AddRange(authorNames);
                    CmbBxAVAuthors.Items.Clear(); CmbBxAVAuthors.Items.AddRange(authorNames);
                    CmbBxEBAuthors.Items.Clear(); CmbBxEBAuthors.Items.AddRange(authorNames);
                }
            }
            catch
            {
                // fall back to empty autocomplete to avoid showing wrong entries
                SetupComboBoxForAutocomplete(CmbBxBKAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxPRAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxTHAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxAVAuthors, null);
                SetupComboBoxForAutocomplete(CmbBxEBAuthors, null);
            }
        }

        // Populate editor/adviser suggestions similar to AddBook
        private void LoadEditors()
        {
            try
            {
                var editorNamesSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var ids = _bookAuthorRepo?.GetDistinctAuthorIdsByRole("Editor");
                if (ids != null)
                {
                    foreach (var id in ids)
                    {
                        try
                        {
                            var author = _authorRepo?.GetById(id);
                            if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                                editorNamesSet.Add(author.FullName.Trim());
                        }
                        catch { }
                    }
                }

                // Advisers are role = "Adviser"
                var adviserIds = _bookAuthorRepo?.GetDistinctAuthorIdsByRole("Adviser");
                var adviserNamesSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (adviserIds != null)
                {
                    foreach (var id in adviserIds)
                    {
                        try
                        {
                            var author = _authorRepo?.GetById(id);
                            if (author != null && !string.IsNullOrWhiteSpace(author.FullName))
                                adviserNamesSet.Add(author.FullName.Trim());
                        }
                        catch { }
                    }
                }

                var editorNames = editorNamesSet.ToArray();
                var adviserNames = adviserNamesSet.ToArray();

                SetupComboBoxForAutocomplete(CmbBxBKEditor, editorNames);
                SetupComboBoxForAutocomplete(CmbBxPREditors, editorNames);
                SetupComboBoxForAutocomplete(CmbBxAVEditors, editorNames);
                SetupComboBoxForAutocomplete(CmbBxEBEditors, editorNames);
                SetupComboBoxForAutocomplete(CmbBxTHAdvisers, adviserNames);

                if (editorNames.Length > 0)
                {
                    CmbBxBKEditor.Items.Clear(); CmbBxBKEditor.Items.AddRange(editorNames);
                    CmbBxPREditors.Items.Clear(); CmbBxPREditors.Items.AddRange(editorNames);
                    CmbBxAVEditors.Items.Clear(); CmbBxAVEditors.Items.AddRange(editorNames);
                    CmbBxEBEditors.Items.Clear(); CmbBxEBEditors.Items.AddRange(editorNames);
                }

                if (adviserNames.Length > 0)
                {
                    CmbBxTHAdvisers.Items.Clear(); CmbBxTHAdvisers.Items.AddRange(adviserNames);
                }
            }
            catch
            {
                SetupComboBoxForAutocomplete(CmbBxBKEditor, null);
                SetupComboBoxForAutocomplete(CmbBxPREditors, null);
                SetupComboBoxForAutocomplete(CmbBxAVEditors, null);
                SetupComboBoxForAutocomplete(CmbBxEBEditors, null);
                SetupComboBoxForAutocomplete(CmbBxTHAdvisers, null);
            }
        }

        // Populate publisher suggestions and enable autocomplete across all publisher comboboxes
        private void LoadPublishers()
        {
            try
            {
                var publisherNames = _publisherRepo?.GetAll()
                    ?.Where(p => !string.IsNullOrWhiteSpace(p.Name))
                    .Select(p => p.Name.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                SetupComboBoxForAutocomplete(CmbBxBKPublisher, publisherNames);
                SetupComboBoxForAutocomplete(CmbBxPRPublisher, publisherNames);
                SetupComboBoxForAutocomplete(CmbBxTHPublisher, publisherNames);
                SetupComboBoxForAutocomplete(CmbBxAVPublisher, publisherNames);
                SetupComboBoxForAutocomplete(CmbBxEBPublisher, publisherNames);

                if (publisherNames != null && publisherNames.Length > 0)
                {
                    CmbBxBKPublisher.Items.Clear(); CmbBxBKPublisher.Items.AddRange(publisherNames);
                    CmbBxPRPublisher.Items.Clear(); CmbBxPRPublisher.Items.AddRange(publisherNames);
                    CmbBxTHPublisher.Items.Clear(); CmbBxTHPublisher.Items.AddRange(publisherNames);
                    CmbBxAVPublisher.Items.Clear(); CmbBxAVPublisher.Items.AddRange(publisherNames);
                    CmbBxEBPublisher.Items.Clear(); CmbBxEBPublisher.Items.AddRange(publisherNames);
                }
            }
            catch
            {
                SetupComboBoxForAutocomplete(CmbBxBKPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxPRPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxTHPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxAVPublisher, null);
                SetupComboBoxForAutocomplete(CmbBxEBPublisher, null);
            }
        }

        private void LoadBookToForm(Book book)
        {
            if (book == null) return;

            // helper: ensure combo contains value (case-insensitive) and set Text
            void EnsureComboContainsAndSet(ComboBox combo, string value)
            {
                if (combo == null) return;
                if (string.IsNullOrWhiteSpace(value))
                {
                    combo.Text = string.Empty;
                    return;
                }

                // check case-insensitively among existing string items
                bool exists = combo.Items.Cast<object>()
                                 .OfType<string>()
                                 .Any(x => string.Equals(x?.Trim(), value.Trim(), StringComparison.OrdinalIgnoreCase));
                if (!exists) combo.Items.Add(value.Trim());
                combo.Text = value.Trim();
            }

            // helper: try to resolve publisher name from repository when navigation property is null
            Func<int, string> GetPublisherNameById = id =>
            {
                if (id <= 0) return null;
                try
                {
                    var p = _publisherRepo?.GetById(id);
                    return p?.Name;
                }
                catch
                {
                    return null;
                }
            };

            // helper: try to resolve category name from repository when navigation property is null
            Func<int, string> GetCategoryNameById = id =>
            {
                if (id <= 0) return null;
                try
                {
                    var c = _categoryRepo?.GetById(id);
                    return c?.Name;
                }
                catch
                {
                    return null;
                }
            };

            // helper: parse Edition like "Vol. 2, No. 3" or "Vol 2 No 3" or "Vol.2 No.3" into volume/issue
            void ParseVolumeIssue(string edition, out string volume, out string issue)
            {
                volume = string.Empty;
                issue = string.Empty;
                if (string.IsNullOrWhiteSpace(edition)) return;

                try
                {
                    var s = edition;

                    // Try common patterns: Vol(. )?number and No(. )?number
                    var volMatch = Regex.Match(s, @"\bVol(?:ume)?\.?\s*(\d+)\b", RegexOptions.IgnoreCase);
                    if (volMatch.Success) volume = volMatch.Groups[1].Value;

                    var noMatch = Regex.Match(s, @"\bNo(?:\.|)?\s*(\d+)\b", RegexOptions.IgnoreCase);
                    if (noMatch.Success) issue = noMatch.Groups[1].Value;

                    // Fallback: if not found, try "Vol. X, No. Y" with non-digits allowed
                    if (string.IsNullOrWhiteSpace(volume) || string.IsNullOrWhiteSpace(issue))
                    {
                        var parts = s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(p => p.Trim()).ToArray();
                        foreach (var p in parts)
                        {
                            if (string.IsNullOrWhiteSpace(volume))
                            {
                                var m = Regex.Match(p, @"\b(\d+)\b");
                                if (m.Success) volume = m.Groups[1].Value;
                            }
                        }

                        // if there are two numeric parts assume first=volume second=issue
                        var allNums = Regex.Matches(s, @"\d+").Cast<Match>().Select(m => m.Value).ToArray();
                        if (allNums.Length >= 2)
                        {
                            if (string.IsNullOrWhiteSpace(volume)) volume = allNums[0];
                            if (string.IsNullOrWhiteSpace(issue)) issue = allNums[1];
                        }
                    }
                }
                catch
                {
                    // ignore parser errors; leave volume/issue empty
                }
            }

            // Map fields depending on resource type (each control belongs to a resource group).
            switch (book.ResourceType)
            {
                case ResourceType.EBook:
                    TxtEBISBN.Text = book.ISBN;
                    TxtEBTitle.Text = book.Title;
                    TxtEBSubtitle.Text = book.Subtitle;
                    TxtEBCallNumber.Text = book.CallNumber;
                    TxtEBPublicationYear.Text = book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty;
                    TxtEBNoOfPages.Text = book.Pages > 0 ? book.Pages.ToString() : string.Empty;
                    CmbBxEBLanguage.Text = book.Language;
                    TxtEBDownloadURL.Text = book.DownloadURL;

                    // ensure publisher shown and selectable (try navigation then id)
                    EnsureComboContainsAndSet(CmbBxEBPublisher, book.Publisher?.Name ?? GetPublisherNameById(book.PublisherID));

                    // category: try navigation then id
                    EnsureComboContainsAndSet(CmbBxEBCategory, book.Category?.Name ?? GetCategoryNameById(book.CategoryID));

                    // format (stored in PhysicalDescription for e-book in this app)
                    if (!string.IsNullOrWhiteSpace(book.PhysicalDescription))
                        EnsureComboContainsAndSet(CmbBxEBFormat, book.PhysicalDescription.Trim());
                    else
                        CmbBxEBFormat.Text = string.Empty;

                    break;

                case ResourceType.Thesis:
                    TxtTHDOI.Text = book.ISBN;
                    TxtTHTitle.Text = book.Title;
                    TxtTHSubtitle.Text = book.Subtitle;
                    TxtTHCallNumber.Text = book.CallNumber;
                    TxtBxTHPublicationYear.Text = book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty;
                    TxtBxTHNoOfPages.Text = book.Pages > 0 ? book.Pages.ToString() : string.Empty;
                    CmbBxTHLanguage.Text = book.Language;

                    // ensure publisher shown and selectable
                    EnsureComboContainsAndSet(CmbBxTHPublisher, book.Publisher?.Name ?? GetPublisherNameById(book.PublisherID));

                    // degree stored in Edition
                    if (!string.IsNullOrWhiteSpace(book.Edition)) CmbBxTHDegreeLevel.Text = book.Edition;
                    break;

                case ResourceType.Periodical:
                    TxtPRISSN.Text = book.ISBN;
                    TxtPRTitle.Text = book.Title;
                    TxtPRSubtitle.Text = book.Subtitle;
                    TxtPRCsllNumber.Text = book.CallNumber;
                    TxtPRPubDate.Text = book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty;
                    TxtPRPages.Text = book.Pages > 0 ? book.Pages.ToString() : string.Empty;
                    CmbBxPRLanguage.Text = book.Language;

                    // ensure publisher shown and selectable
                    EnsureComboContainsAndSet(CmbBxPRPublisher, book.Publisher?.Name ?? GetPublisherNameById(book.PublisherID));

                    // Parse volume/issue from Edition (if present)
                    string vol, issue;
                    ParseVolumeIssue(book.Edition, out vol, out issue);
                    if (!string.IsNullOrWhiteSpace(vol)) TxtPRVolume.Text = vol;
                    else TxtPRVolume.Text = string.Empty;

                    if (!string.IsNullOrWhiteSpace(issue)) TxtPRIssue.Text = issue;
                    else TxtPRIssue.Text = string.Empty;
                    break;

                case ResourceType.AV:
                    TxtAVUPCISAN.Text = book.ISBN;
                    TxtAVTitle.Text = book.Title;
                    TxtAVSubtitle.Text = book.Subtitle;
                    TxtAVCallNumber.Text = book.CallNumber;
                    TxtAVPublicationYear.Text = book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty;
                    TxtAVDuration.Text = book.Pages > 0 ? book.Pages.ToString() : string.Empty; // duration stored in Pages column
                    CmbBxAVLanguage.Text = book.Language;

                    // ensure publisher shown and selectable
                    EnsureComboContainsAndSet(CmbBxAVPublisher, book.Publisher?.Name ?? GetPublisherNameById(book.PublisherID));

                    // category: try navigation then id
                    EnsureComboContainsAndSet(CmbBxAVCategory, book.Category?.Name ?? GetCategoryNameById(book.CategoryID));

                    break;

                case ResourceType.PhysicalBook:
                default:
                    TxtBKISBN.Text = book.ISBN;
                    TxtBKTitle.Text = book.Title;
                    TxtBKSubtitle.Text = book.Subtitle;
                    TxtBKCallNumber.Text = book.CallNumber;
                    TxtBKEdition.Text = book.Edition;
                    TxtBKPublicationYear.Text = book.PublicationYear > 0 ? book.PublicationYear.ToString() : string.Empty;
                    TxtBKNoOfPages.Text = book.Pages > 0 ? book.Pages.ToString() : string.Empty;
                    TxtBKPhysicalDescription.Text = book.PhysicalDescription;
                    CmbBxBKLanguage.Text = book.Language;

                    // ensure publisher shown and selectable
                    EnsureComboContainsAndSet(CmbBxBKPublisher, book.Publisher?.Name ?? GetPublisherNameById(book.PublisherID));

                    // category: try navigation then id
                    EnsureComboContainsAndSet(CmbBxBKCategory, book.Category?.Name ?? GetCategoryNameById(book.CategoryID));
                    break;
            }

            // Loan type mapping (designer radio names differ slightly from AddBook)
            if (!string.IsNullOrWhiteSpace(book.LoanType))
            {
                if (book.LoanType.Equals("Reference", StringComparison.OrdinalIgnoreCase)) { if (RdoBtnBKReference != null) RdoBtnBKReference.Checked = true; }
                else if (book.LoanType.Equals("Circulation", StringComparison.OrdinalIgnoreCase)) { if (RdoBtnBKCirculation != null) RdoBtnBKCirculation.Checked = true; }
            }

            // Cover image
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
                catch { }
            }

            // Authors / Editors - distribute into per-resource listboxes
            try
            {
                var bas = _bookAuthorRepo.GetByBookId(book.BookID);
                if (bas != null)
                {
                    // clear all per-resource boxes first
                    LstBxBKAuthor.Items.Clear(); LstBxBKEditor.Items.Clear();
                    LstBxPRAuthors.Items.Clear(); LstBxPREditors.Items.Clear();
                    LstBxTHAuthors.Items.Clear(); LstBxTHAdvisers.Items.Clear();
                    LstBxAVAuthors.Items.Clear(); LstBxAVEditors.Items.Clear();
                    LstBxEBAuthors.Items.Clear(); LstBxEBeditors.Items.Clear();

                    foreach (var ba in bas)
                    {
                        var author = _author_repo_getbyid_safe(ba.AuthorID);
                        if (author == null) continue;
                        var name = author.FullName?.Trim() ?? string.Empty;

                        // Place author/editor into the resource-appropriate listboxes based on role
                        if (string.Equals(ba.Role, "Editor", StringComparison.OrdinalIgnoreCase))
                        {
                            // editor entries are placed on BK/PR/AV/EB editors lists depending on book.ResourceType
                            switch (book.ResourceType)
                            {
                                case ResourceType.PhysicalBook:
                                    LstBxBKEditor.Items.Add(name);
                                    _editors.Add(name);
                                    break;
                                case ResourceType.Periodical:
                                    LstBxPREditors.Items.Add(name);
                                    _prEditors.Add(name);
                                    break;
                                case ResourceType.Thesis:
                                    LstBxTHAdvisers.Items.Add(name);
                                    _thAdvisers.Add(name);
                                    break;
                                case ResourceType.AV:
                                    LstBxAVEditors.Items.Add(name);
                                    _avEditors.Add(name);
                                    break;
                                case ResourceType.EBook:
                                    LstBxEBeditors.Items.Add(name);
                                    _ebEditors.Add(name);
                                    break;
                            }
                        }
                        else if (string.Equals(ba.Role, "Adviser", StringComparison.OrdinalIgnoreCase))
                        {
                            // thesis advisers specifically
                            LstBxTHAdvisers.Items.Add(name);
                            _thAdvisers.Add(name);
                        }
                        else
                        {
                            // authors
                            switch (book.ResourceType)
                            {
                                case ResourceType.PhysicalBook:
                                    LstBxBKAuthor.Items.Add(name);
                                    _authors.Add(name);
                                    break;
                                case ResourceType.Periodical:
                                    LstBxPRAuthors.Items.Add(name);
                                    _prAuthors.Add(name);
                                    break;
                                case ResourceType.Thesis:
                                    LstBxTHAuthors.Items.Add(name);
                                    _thAuthors.Add(name);
                                    break;
                                case ResourceType.AV:
                                    LstBxAVAuthors.Items.Add(name);
                                    _avAuthors.Add(name);
                                    break;
                                case ResourceType.EBook:
                                    LstBxEBAuthors.Items.Add(name);
                                    _ebAuthors.Add(name);
                                    break;
                            }
                        }
                    }
                }
            }
            catch { }

            // Ensure UI panels are synced
            SyncPanelsForResourceType();
            UpdateCopyInformationVisibility();
        }

        #region Add-from-combo handlers (per resource)

        private void BtnBKAddAuthor_Click(object sender, EventArgs e) => AddFromCombo(CmbBxBKAuthors, LstBxBKAuthor, _authors);
        private void BtnBKAddEditor_Click(object sender, EventArgs e) => AddFromCombo(CmbBxBKEditor, LstBxBKEditor, _editors);

        private void BtnPRAddAuthors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxPRAuthors, LstBxPRAuthors, _prAuthors);
        private void BtnPRAddEditors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxPREditors, LstBxPREditors, _prEditors);

        private void BtnTHAddAuthors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxTHAuthors, LstBxTHAuthors, _thAuthors);
        private void BtnTHAddAdvisers_Click(object sender, EventArgs e) => AddFromCombo(CmbBxTHAdvisers, LstBxTHAdvisers, _thAdvisers);

        private void BtnAVAddAuthors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxAVAuthors, LstBxAVAuthors, _avAuthors);
        private void BtnAVAddEditors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxAVEditors, LstBxAVEditors, _avEditors);

        private void BtnEBAddAuthors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxEBAuthors, LstBxEBAuthors, _ebAuthors);
        private void BtnEBAddEditors_Click(object sender, EventArgs e) => AddFromCombo(CmbBxEBEditors, LstBxEBeditors, _ebEditors);

        private void LstBxBKAuthor_DoubleClick(object sender, EventArgs e) { if (LstBxBKAuthor.SelectedIndex >= 0) { _authors.RemoveAt(LstBxBKAuthor.SelectedIndex); RefreshListBox(LstBxBKAuthor, _authors); } }
        private void LstBxBKEditor_DoubleClick(object sender, EventArgs e) { if (LstBxBKEditor.SelectedIndex >= 0) { _editors.RemoveAt(LstBxBKEditor.SelectedIndex); RefreshListBox(LstBxBKEditor, _editors); } }

        private void LstBxPRAuthors_DoubleClick(object sender, EventArgs e) { if (LstBxPRAuthors.SelectedIndex >= 0) { _prAuthors.RemoveAt(LstBxPRAuthors.SelectedIndex); RefreshListBox(LstBxPRAuthors, _prAuthors); } }
        private void LstBxPREditors_DoubleClick(object sender, EventArgs e) { if (LstBxPREditors.SelectedIndex >= 0) { _prEditors.RemoveAt(LstBxPREditors.SelectedIndex); RefreshListBox(LstBxPREditors, _prEditors); } }

        private void LstBxTHAuthors_DoubleClick(object sender, EventArgs e) { if (LstBxTHAuthors.SelectedIndex >= 0) { _thAuthors.RemoveAt(LstBxTHAuthors.SelectedIndex); RefreshListBox(LstBxTHAuthors, _thAuthors); } }
        private void LstBxTHAdvisers_DoubleClick(object sender, EventArgs e) { if (LstBxTHAdvisers.SelectedIndex >= 0) { _thAdvisers.RemoveAt(LstBxTHAdvisers.SelectedIndex); RefreshListBox(LstBxTHAdvisers, _thAdvisers); } }

        private void LstBxAVAuthors_DoubleClick(object sender, EventArgs e) { if (LstBxAVAuthors.SelectedIndex >= 0) { _avAuthors.RemoveAt(LstBxAVAuthors.SelectedIndex); RefreshListBox(LstBxAVAuthors, _avAuthors); } }
        private void LstBxAVEditors_DoubleClick(object sender, EventArgs e) { if (LstBxAVEditors.SelectedIndex >= 0) { _avEditors.RemoveAt(LstBxAVEditors.SelectedIndex); RefreshListBox(LstBxAVEditors, _avEditors); } }

        private void LstBxEBAuthors_DoubleClick(object sender, EventArgs e) { if (LstBxEBAuthors.SelectedIndex >= 0) { _ebAuthors.RemoveAt(LstBxEBAuthors.SelectedIndex); RefreshListBox(LstBxEBAuthors, _ebAuthors); } }
        private void LstBxEBeditors_DoubleClick(object sender, EventArgs e) { if (LstBxEBeditors.SelectedIndex >= 0) { _ebEditors.RemoveAt(LstBxEBeditors.SelectedIndex); RefreshListBox(LstBxEBeditors, _ebEditors); } }

        #endregion

        private void AddFromCombo(ComboBox combo, ListBox targetListBox, List<string> backingList)
        {
            if (combo == null || targetListBox == null || backingList == null) return;

            string name = combo.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                var existing = _author_repo_getbyname_safe(name);
                if (existing != null)
                    name = existing.FullName?.Trim() ?? name;
            }
            catch { }

            if (!backingList.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                backingList.Add(name);
                RefreshListBox(targetListBox, backingList);

                if (!combo.Items.Contains(name))
                    combo.Items.Add(name);

                combo.Text = string.Empty;
                combo.Focus();
            }
            else
            {
                MessageBox.Show("This entry is already added.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshListBox(ListBox lb, List<string> values)
        {
            if (lb == null || values == null) return;
            lb.Items.Clear();
            foreach (var v in values) lb.Items.Add(v);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private ResourceType? GetSelectedResourceType()
        {
            // If a book is loaded, use its resource type (Edit scenario).
            if (_book != null) return _book.ResourceType;

            // Fallback: infer from visible group boxes (useful if panels are manually shown/hidden)
            if (GrpBxPhysicalBook != null && GrpBxPhysicalBook.Visible) return ResourceType.PhysicalBook;
            if (GrpBxEBook != null && GrpBxEBook.Visible) return ResourceType.EBook;
            if (GrpBxThesis != null && GrpBxThesis.Visible) return ResourceType.Thesis;
            if (GrpBxPeriodicals != null && GrpBxPeriodicals.Visible) return ResourceType.Periodical;
            if (GrpBxAV != null && GrpBxAV.Visible) return ResourceType.AV;

            // Default when nothing indicates a type
            return ResourceType.PhysicalBook;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedType = GetSelectedResourceType() ?? ResourceType.PhysicalBook;

                // Publication year validation per selected resource-type control
                string pubYearText = null;
                Control pubYearControl = null;
                string label = "Publication year";

                switch (selectedType)
                {
                    case ResourceType.PhysicalBook:
                        pubYearText = TxtBKPublicationYear.Text;
                        pubYearControl = TxtBKPublicationYear;
                        label = "Publication year";
                        break;
                    case ResourceType.Periodical:
                        pubYearText = TxtPRPubDate.Text;
                        pubYearControl = TxtPRPubDate;
                        label = "Publication date";
                        break;
                    case ResourceType.Thesis:
                        pubYearText = TxtBxTHPublicationYear.Text;
                        pubYearControl = TxtBxTHPublicationYear;
                        label = "Publication year";
                        break;
                    case ResourceType.AV:
                        pubYearText = TxtAVPublicationYear.Text;
                        pubYearControl = TxtAVPublicationYear;
                        label = "Publication year";
                        break;
                    case ResourceType.EBook:
                        pubYearText = TxtEBPublicationYear.Text;
                        pubYearControl = TxtEBPublicationYear;
                        label = "Publication year";
                        break;
                }

                // Validate presence
                if (string.IsNullOrWhiteSpace(pubYearText))
                {
                    MessageBox.Show($"{label} is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pubYearControl?.Focus();
                    return;
                }

                if (pubYearText.Any(char.IsLetter))
                {
                    MessageBox.Show($"{label} must contain digits only.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pubYearControl?.Focus();
                    return;
                }

                // Pages/duration validation depending on resource
                if (selectedType == ResourceType.PhysicalBook)
                {
                    if (!string.IsNullOrWhiteSpace(TxtBKNoOfPages.Text))
                    {
                        int pages = ParseInt(TxtBKNoOfPages.Text);
                        if (pages <= 0) { MessageBox.Show("Number of pages should be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); TxtBKNoOfPages.Focus(); return; }
                    }
                }
                else if (selectedType == ResourceType.Thesis)
                {
                    if (!string.IsNullOrWhiteSpace(TxtBxTHNoOfPages.Text))
                    {
                        int pages = ParseInt(TxtBxTHNoOfPages.Text);
                        if (pages <= 0) { MessageBox.Show("Number of pages must be a positive integer for theses.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); TxtBxTHNoOfPages.Focus(); return; }
                    }
                }
                else if (selectedType == ResourceType.AV)
                {
                    var durationRaw = TxtAVDuration.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(durationRaw)) { MessageBox.Show("Duration is required. Enter a number (seconds).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); TxtAVDuration?.Focus(); return; }
                    if (!durationRaw.All(char.IsDigit)) { MessageBox.Show("Duration must contain digits only.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); TxtAVDuration?.Focus(); return; }
                }

                // Publisher validation per selected combobox
                string pubText = selectedType == ResourceType.PhysicalBook ? CmbBxBKPublisher.Text : selectedType == ResourceType.Periodical ? CmbBxPRPublisher.Text :
                    selectedType == ResourceType.Thesis ? CmbBxTHPublisher.Text : selectedType == ResourceType.AV ? CmbBxAVPublisher.Text : CmbBxEBPublisher.Text;

                if (!string.IsNullOrWhiteSpace(pubText) && !ContainsLetter(pubText))
                {
                    MessageBox.Show("Publisher name must contain at least one letter and may include digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // focus relevant publisher combo
                    switch (selectedType)
                    {
                        case ResourceType.PhysicalBook: CmbBxBKPublisher.Focus(); break;
                        case ResourceType.Periodical: CmbBxPRPublisher.Focus(); break;
                        case ResourceType.Thesis: CmbBxTHPublisher.Focus(); break;
                        case ResourceType.AV: CmbBxAVPublisher.Focus(); break;
                        case ResourceType.EBook: CmbBxEBPublisher.Focus(); break;
                    }
                    return;
                }

                // Basic required fields per resource type
                if (selectedType == ResourceType.Periodical)
                {
                    if (string.IsNullOrWhiteSpace(TxtPRISSN.Text)) { MessageBox.Show("ISSN is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); TxtPRISSN.Focus(); return; }
                    if (string.IsNullOrWhiteSpace(TxtPRVolume.Text) || string.IsNullOrWhiteSpace(TxtPRIssue.Text)) { MessageBox.Show("Volume and issue are required for periodicals.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                }

                if (selectedType == ResourceType.PhysicalBook)
                {
                    if ((RdoBtnBKCirculation == null || !RdoBtnBKCirculation.Checked) && (RdoBtnBKReference == null || !RdoBtnBKReference.Checked))
                    {
                        MessageBox.Show("Please select a loan type (Circulation or Reference) for physical books.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Map UI -> book depending on selectedType
                if (_book == null) { MessageBox.Show("No book loaded to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                switch (selectedType)
                {
                    case ResourceType.PhysicalBook:
                        _book.ISBN = TxtBKISBN.Text?.Trim();
                        _book.Title = TxtBKTitle.Text?.Trim();
                        _book.Subtitle = TxtBKSubtitle.Text?.Trim();
                        _book.CallNumber = TxtBKCallNumber.Text?.Trim();
                        _book.Edition = TxtBKEdition.Text?.Trim();
                        _book.PublicationYear = ParseInt(TxtBKPublicationYear.Text);
                        _book.Pages = ParseInt(TxtBKNoOfPages.Text);
                        _book.PhysicalDescription = TxtBKPhysicalDescription.Text?.Trim();
                        _book.Language = CmbBxBKLanguage.Text?.Trim();
                        break;

                    case ResourceType.Periodical:
                        _book.ISBN = TxtPRISSN.Text?.Trim();
                        _book.Title = TxtPRTitle.Text?.Trim();
                        _book.Subtitle = TxtPRSubtitle.Text?.Trim();
                        _book.CallNumber = TxtPRCsllNumber.Text?.Trim();
                        _book.PublicationYear = ExtractYearFromString(TxtPRPubDate.Text?.Trim());
                        _book.Pages = ParseInt(TxtPRPages.Text);
                        _book.Language = CmbBxPRLanguage.Text?.Trim();
                        // edition: Vol + No
                        var volParts = new List<string>();
                        if (!string.IsNullOrWhiteSpace(TxtPRVolume.Text)) volParts.Add($"Vol. {TxtPRVolume.Text.Trim()}");
                        if (!string.IsNullOrWhiteSpace(TxtPRIssue.Text)) volParts.Add($"No. {TxtPRIssue.Text.Trim()}");
                        _book.Edition = volParts.Count > 0 ? string.Join(", ", volParts) : _book.Edition;
                        break;

                    case ResourceType.Thesis:
                        _book.ISBN = TxtTHDOI.Text?.Trim();
                        _book.Title = TxtTHTitle.Text?.Trim();
                        _book.Subtitle = TxtTHSubtitle.Text?.Trim();
                        _book.CallNumber = TxtTHCallNumber.Text?.Trim();
                        _book.PublicationYear = ParseInt(TxtBxTHPublicationYear.Text);
                        _book.Pages = ParseInt(TxtBxTHNoOfPages.Text);
                        _book.Language = CmbBxTHLanguage.Text?.Trim();
                        _book.Edition = CmbBxTHDegreeLevel.Text?.Trim(); // degree stored in edition
                        break;

                    case ResourceType.AV:
                        _book.ISBN = TxtAVUPCISAN.Text?.Trim();
                        _book.Title = TxtAVTitle.Text?.Trim();
                        _book.Subtitle = TxtAVSubtitle.Text?.Trim();
                        _book.CallNumber = TxtAVCallNumber.Text?.Trim();
                        _book.PublicationYear = ParseInt(TxtAVPublicationYear.Text);
                        _book.Pages = ParseInt(TxtAVDuration.Text); // duration stored in Pages column
                        _book.Language = CmbBxAVLanguage.Text?.Trim();
                        _book.PhysicalDescription = RdoBtnAVPhysical != null && RdoBtnAVPhysical.Checked ? CmbBxAVPhysicalDescription.Text.Trim() : CmbBxAVFormat.Text.Trim();
                        _book.Category = null; // category id set below
                        break;

                    case ResourceType.EBook:
                        _book.ISBN = TxtEBISBN.Text?.Trim();
                        _book.Title = TxtEBTitle.Text?.Trim();
                        _book.Subtitle = TxtEBSubtitle.Text?.Trim();
                        _book.CallNumber = TxtEBCallNumber.Text?.Trim();
                        _book.PublicationYear = ParseInt(TxtEBPublicationYear.Text);
                        _book.Pages = ParseInt(TxtEBNoOfPages.Text);
                        _book.Language = CmbBxEBLanguage.Text?.Trim();
                        _book.DownloadURL = TxtEBDownloadURL.Text?.Trim();
                        _book.PhysicalDescription = CmbBxEBFormat.Text?.Trim();
                        break;
                }

                // Publisher persistence / mapping per selectedType
                string publisherName = selectedType == ResourceType.PhysicalBook ? CmbBxBKPublisher.Text?.Trim()
                    : selectedType == ResourceType.Periodical ? CmbBxPRPublisher.Text?.Trim()
                    : selectedType == ResourceType.Thesis ? CmbBxTHPublisher.Text?.Trim()
                    : selectedType == ResourceType.AV ? CmbBxAVPublisher.Text?.Trim()
                    : CmbBxEBPublisher.Text?.Trim();

                if (!string.IsNullOrWhiteSpace(publisherName))
                {
                    try
                    {
                        var match = _publisherRepo.GetAll().FirstOrDefault(p => string.Equals(p.Name?.Trim(), publisherName, StringComparison.OrdinalIgnoreCase));
                        if (match != null) _book.PublisherID = match.PublisherID;
                        else
                        {
                            var newPub = new Publisher { Name = publisherName };
                            int newId = _publisherRepo.Add(newPub);
                            if (newId > 0) _book.PublisherID = newId;
                        }
                    }
                    catch { }
                }

                // Category: per-resource category combos
                string categoryText = selectedType == ResourceType.PhysicalBook ? CmbBxBKCategory.Text?.Trim()
                    : selectedType == ResourceType.EBook ? CmbBxEBCategory.Text?.Trim()
                    : selectedType == ResourceType.AV ? CmbBxAVCategory.Text?.Trim()
                    : null;

                if (!string.IsNullOrWhiteSpace(categoryText))
                {
                    try
                    {
                        var cat = _catalogManager.GetOrCreateCategory(categoryText);
                        if (cat != null) _book.CategoryID = cat.CategoryID;
                    }
                    catch
                    {
                        var existingCat = _category_repo_getbyname_safe(categoryText);
                        if (existingCat == null)
                        {
                            var newCat = new Category { Name = categoryText, Description = null };
                            int newId = _category_repo_add_safe(newCat);
                            if (newId > 0) _book.CategoryID = newId;
                        }
                        else
                        {
                            _book.CategoryID = existingCat.CategoryID;
                        }
                    }
                }

                // Resource type set on book
                _book.ResourceType = selectedType;

                // Destructive change confirmation to EBook (copies deletion)
                if (_originalResourceType != _book.ResourceType && _book.ResourceType == ResourceType.EBook && _originalResourceType != ResourceType.EBook)
                {
                    var dlgRes = MessageBox.Show(
                        "Are you sure you want to edit this book's resource type to E-Book? All copies of this book will be permanently deleted.",
                        "Confirm change to E-Book",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dlgRes != DialogResult.Yes) return;
                }

                // Loan type mapping (physical)
                if (RdoBtnBKReference != null && RdoBtnBKReference.Checked) _book.LoanType = "Reference";
                else if (RdoBtnBKCirculation != null && RdoBtnBKCirculation.Checked) _book.LoanType = "Circulation";
                else _book.LoanType = null;

                // Cover image copy if new selected
                if (!string.IsNullOrWhiteSpace(_selectedCoverPath))
                {
                    var rel = SaveCoverImage(_selectedCoverPath);
                    if (!string.IsNullOrWhiteSpace(rel)) _book.CoverImage = rel;
                }

                // Persist main row
                bool updated = _bookRepo.Update(_book);
                if (!updated)
                {
                    MessageBox.Show("Failed to update book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Handle resource-type changes affecting copies / accessions
                if (_originalResourceType != _book.ResourceType)
                {
                    try
                    {
                        if (_book.ResourceType == ResourceType.EBook && _originalResourceType != ResourceType.EBook)
                        {
                            var bookCopyRepo = new BookCopyRepository(new DbConnection());
                            bookCopyRepo.DeleteByBookId(_book.BookID);
                        }
                        else
                        {
                            RegenerateAccessionsAndBarcodes(_book.BookID, _originalResourceType, _book.ResourceType);
                        }
                        _originalResourceType = _book.ResourceType;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Book updated but regenerating copy accessions/barcodes failed: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Replace BookAuthor associations based on current resource-specific listboxes
                try
                {
                    _bookAuthorRepo.DeleteByBookId(_book.BookID);

                    // Helper to iterate a listbox and create BookAuthor entries
                    void PersistListBoxAuthors(ListBox lb, string role, bool primaryByIndex)
                    {
                        for (int i = 0; i < lb.Items.Count; i++)
                        {
                            var name = lb.Items[i]?.ToString()?.Trim();
                            if (string.IsNullOrWhiteSpace(name)) continue;
                            int authorId = EnsureAuthorExists(name);
                            if (authorId > 0)
                            {
                                var ba = new BookAuthor
                                {
                                    BookID = _book.BookID,
                                    AuthorID = authorId,
                                    Role = role,
                                    IsPrimaryAuthor = primaryByIndex ? i == 0 : false
                                };
                                _bookAuthorRepo.Add(ba);
                            }
                        }
                    }

                    switch (_book.ResourceType)
                    {
                        case ResourceType.PhysicalBook:
                            PersistListBoxAuthors(LstBxBKAuthor, "Author", true);
                            PersistListBoxAuthors(LstBxBKEditor, "Editor", false);
                            break;
                        case ResourceType.Periodical:
                            PersistListBoxAuthors(LstBxPRAuthors, "Author", true);
                            PersistListBoxAuthors(LstBxPREditors, "Editor", false);
                            break;
                        case ResourceType.Thesis:
                            PersistListBoxAuthors(LstBxTHAuthors, "Author", true);
                            PersistListBoxAuthors(LstBxTHAdvisers, "Adviser", false);
                            break;
                        case ResourceType.AV:
                            PersistListBoxAuthors(LstBxAVAuthors, "Author", true);
                            PersistListBoxAuthors(LstBxAVEditors, "Editor", false);
                            break;
                        case ResourceType.EBook:
                            PersistListBoxAuthors(LstBxEBAuthors, "Author", true);
                            PersistListBoxAuthors(LstBxEBeditors, "Editor", false);
                            break;
                    }
                }
                catch { /* non-fatal */ }

                MessageBox.Show("Book updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Regenerate accessions & barcodes (keeps EditBook behavior but uses shared generator if available)
        private void RegenerateAccessionsAndBarcodes(int bookId, ResourceType oldType, ResourceType newType)
        {
            var bookCopyRepo = new BookCopyRepository(new DbConnection());
            var copies = bookCopyRepo.GetByBookId(bookId) ?? new List<BookCopy>();
            if (copies.Count == 0) return;

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
                    candidate = Regex.Replace(original, @"^[A-Za-z]+(?=-)", newPrefix);
                }
                else
                {
                    candidate = $"{newPrefix}-{bookId}-{DateTime.Now.Year}-{copy.CopyID:D4}";
                }

                if (seen.Contains(candidate))
                    candidate = $"{candidate}-{copy.CopyID}";

                seen.Add(candidate);
                newAccessions.Add(candidate);
                copyByNewAcc[candidate] = copy;
            }

            if (newAccessions.Count == 0) return;

            var generator = _barcode_generator_or_default();

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
                var path = kv.Value;
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

        private IBarcodeGenerator _barcode_generator_or_default()
        {
            return _barcodeGenerator ?? new ZXingBarcodeGenerator(Path.Combine(Application.StartupPath, "Assets", "dataimages", "BookCopyBarcodes"), BarcodeFormat.CODE_128, width: 300, height: 100, margin: 10);
        }

        private void SyncPanelsForResourceType()
        {
            // Hide all resource group boxes first so only the selected one is visible.
            if (GrpBxPhysicalBook != null) GrpBxPhysicalBook.Visible = false;
            if (GrpBxPeriodicals != null) GrpBxPeriodicals.Visible = false;
            if (GrpBxThesis != null) GrpBxThesis.Visible = false;
            if (GrpBxAV != null) GrpBxAV.Visible = false;
            if (GrpBxEBook != null) GrpBxEBook.Visible = false;

            // Decide which resource is selected (fall back to PhysicalBook when unknown)
            var selected = GetSelectedResourceType() ?? ResourceType.PhysicalBook;

            switch (selected)
            {
                case ResourceType.PhysicalBook:
                    if (GrpBxPhysicalBook != null) GrpBxPhysicalBook.Visible = true;
                    break;

                case ResourceType.Periodical:
                    if (GrpBxPeriodicals != null) GrpBxPeriodicals.Visible = true;
                    break;

                case ResourceType.Thesis:
                    if (GrpBxThesis != null) GrpBxThesis.Visible = true;
                    break;

                case ResourceType.AV:
                    if (GrpBxAV != null) GrpBxAV.Visible = true;
                    break;

                case ResourceType.EBook:
                    if (GrpBxEBook != null) GrpBxEBook.Visible = true;
                    break;
            }

            // Loan radios enabled only when physical book selected
            bool physical = selected == ResourceType.PhysicalBook;
            if (RdoBtnBKReference != null) RdoBtnBKReference.Enabled = physical;
            if (RdoBtnBKCirculation != null) RdoBtnBKCirculation.Enabled = physical;

            // If physical is not selected, ensure loan type radios are cleared
            if (!physical)
            {
                if (RdoBtnBKReference != null) RdoBtnBKReference.Checked = false;
                if (RdoBtnBKCirculation != null) RdoBtnBKCirculation.Checked = false;
            }

            // Enable/disable download URL controls according to the selected resource and material-format radios.
            bool ebook = selected == ResourceType.EBook;
            if (TxtEBDownloadURL != null) TxtEBDownloadURL.Enabled = ebook;
            if (!ebook && TxtEBDownloadURL != null) TxtEBDownloadURL.Text = string.Empty;

            bool thDigital = selected == ResourceType.Thesis && RdoBtnTHDigital != null && RdoBtnTHDigital.Checked;
            if (TxtTHDownloadURL != null) TxtTHDownloadURL.Enabled = thDigital;
            if (!thDigital && TxtTHDownloadURL != null) TxtTHDownloadURL.Text = string.Empty;

            bool avDigital = selected == ResourceType.AV && RdoBtnAVDigital != null && RdoBtnAVDigital.Checked;
            if (TxtAVDownloadURL != null) TxtAVDownloadURL.Enabled = avDigital;
            if (!avDigital && TxtAVDownloadURL != null) TxtAVDownloadURL.Text = string.Empty;

            // Sync material-format sub-panels (physical vs digital controls inside each resource group)
            SyncMaterialFormatPanels();

            // Ensure copy-info visibility updated as resource-type/panel state may have changed
            UpdateCopyInformationVisibility();
        }

        private void RdoBtnPhysicalBook_CheckedChanged(object sender, EventArgs e) => SyncPanelsForResourceType();
        private void RdoBtnEBook_CheckedChanged(object sender, EventArgs e) => SyncPanelsForResourceType();
        private void RdoBtnTheses_CheckedChanged(object sender, EventArgs e) => SyncPanelsForResourceType();
        private void RdoBtnPeriodical_CheckedChanged(object sender, EventArgs e) => SyncPanelsForResourceType();
        private void RdoBtnAV_CheckedChanged(object sender, EventArgs e) => SyncPanelsForResourceType();

        // Show/hide copy information group when appropriate (Physical books and when selected resource's physical format is chosen)
        private void UpdateCopyInformationVisibility()
        {
            bool show = false;

            var selected = GetSelectedResourceType() ?? ResourceType.PhysicalBook;
            if (selected == ResourceType.PhysicalBook) show = true;

            // If Periodical/Thesis/AV have material-format radios, you could check them here similar to AddBook.
            if (GrpBxCopyInformation != null)
                GrpBxCopyInformation.Visible = show;

            flowLayoutPanel1?.PerformLayout();
            flowLayoutPanel1?.Refresh();
        }

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
            try { return _authorRepo.GetByName(fullName); } catch { return null; }
        }

        private Author _author_repo_getbyid_safe(int id)
        {
            try { return _authorRepo.GetById(id); } catch { return null; }
        }

        private Category _category_repo_getbyname_safe(string name)
        {
            try { return _categoryRepo.GetByName(name); } catch { return null; }
        }

        private int _category_repo_add_safe(Category cat)
        {
            try { return _categoryRepo.Add(cat); } catch { return 0; }
        }

        private int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            value = value.Trim();
            if (long.TryParse(value, out long longVal))
            {
                if (longVal <= int.MinValue) return int.MinValue;
                if (longVal > int.MaxValue) return int.MaxValue;
                return (int)longVal;
            }
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
                File.Copy(sourceFilePath, destFullPath, true);
                string relativePath = $"Assets/dataimages/Books/{newFileName}";
                return relativePath;
            }
            catch { return null; }
        }

        private bool ContainsLetter(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Any(char.IsLetter);
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

            // Enter key should act like pressing Add button where applicable — support all designer combobox names
            comboBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (comboBox == CmbBxBKAuthors) BtnBKAddAuthor.PerformClick();
                    else if (comboBox == CmbBxBKEditor) BtnBKAddEditor.PerformClick();

                    else if (comboBox == CmbBxPRAuthors) BtnPRAddAuthors.PerformClick();
                    else if (comboBox == CmbBxPREditors) BtnPRAddEditors.PerformClick();

                    else if (comboBox == CmbBxTHAuthors) BtnTHAddAuthors.PerformClick();
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
    }
}