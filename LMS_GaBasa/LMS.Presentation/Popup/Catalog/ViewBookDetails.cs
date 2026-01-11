using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LMS.DataAccess.Repositories;
using LMS.BusinessLogic.Managers;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Enums;
using System.Diagnostics;
using LMS.BusinessLogic.Security;
using LMS.Model.Models.Users;

namespace LMS.Presentation.Popup.Catalog
{
    public partial class ViewBookDetails : Form
    {
        // permission context (host should call SetPermissionContext after constructing the form)
        private IPermissionService _permissionService;
        private User _currentUser;

        // keep a reference to the currently-displayed book so SetPermissionContext can update UI
        private Book _currentBook;

        public ViewBookDetails()
        {
            InitializeComponent();

            // Ensure the panel scroll position is reset after the form is shown and layout completed.
            // Use Shown + BeginInvoke so layout has finished and scroll values are valid.
            this.Shown -= ViewBookDetails_Shown;
            this.Shown += ViewBookDetails_Shown;

            // Wire up the Reserve button click handler
            try { BtnReserve.Click -= BtnReserve_Click; } catch { }
            try { BtnReserve.Click += BtnReserve_Click; } catch { }
        }

        private void ViewBookDetails_Shown(object sender, EventArgs e)
        {
            try
            {
                // Defer to UI thread queue to ensure all layout/measure is done
                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        // Reset scroll position to top-left
                        if (PnlDocked != null)
                        {
                            PnlDocked.AutoScrollPosition = new Point(0, 0);

                            // Defensive: ensure vertical scroll is at minimum
                            try
                            {
                                if (PnlDocked.VerticalScroll != null)
                                    PnlDocked.VerticalScroll.Value = PnlDocked.VerticalScroll.Minimum;
                            }
                            catch { /* ignore invalid value exceptions */ }
                        }

                        // Remove focus from any control that might cause scrolling
                        try
                        {
                            this.ActiveControl = null;
                        }
                        catch { }
                    }
                    catch { }
                }));
            }
            catch { }
        }

        /// <summary>
        /// Host may call this to provide current user & permission service so UI actions (Borrow/Reserve)
        /// are shown only for allowed roles (members).
        /// </summary>
        public void SetPermissionContext(User currentUser, IPermissionService permissionService)
        {
            _currentUser = currentUser;
            _permissionService = permissionService;

            // Debug output to verify the passed user's role
            try
            {
                Debug.WriteLine($"ViewBookDetails: SetPermissionContext called. UserID={_currentUser?.UserID}, Role={_currentUser?.Role}");
            }
            catch { }

            // Re-evaluate actions/buttons now that permission context is available
            try { UpdateActionButtons(); } catch { }
        }

        /// <summary>
        /// Construct the details form for the specified book id and populate basic fields:
        ///  - cover image
        ///  - title
        ///  - subtitle (empty string when not present)
        ///  - authors (formatted "Author(s) : A, B")
        ///  - editors/advisers (formatted "Editor(s) : E1, E2" or "Adviser(s) :" for Thesis)
        /// </summary>
        public ViewBookDetails(int bookId) : this()
        {
            try
            {
                var bookRepo = new BookRepository();
                var book = bookRepo.GetById(bookId);

                if (book == null)
                {
                    MessageBox.Show("Book not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                PopulateBasicDetails(book);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ViewBookDetails ctor failed: " + ex);
                MessageBox.Show("Failed to load book details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // --- Populate helpers ---

        private void PopulateBasicDetails(Book book)
        {
            if (book == null) return;

            // store current book for subsequent permission-driven updates
            _currentBook = book;

            // Ensure copies are loaded from DB so availability checks are reliable.
            try
            {
                var repo = new BookCopyRepository();
                var repoCopies = repo.GetByBookId(book.BookID);
                if (repoCopies != null && repoCopies.Count > 0)
                {
                    // assign into navigation property so other code can reuse it
                    book.Copies = repoCopies;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to load copies from repository: " + ex);
            }

            // Cover image
            try
            {
                LoadCoverImageToPictureBox(PicBxBookCover, book.CoverImage);
            }
            catch { /* ignore and leave default/blank image */ }

            // Title
            LblTitle.Text = book.Title ?? string.Empty;

            // Subtitle (empty when not present)
            LblSubtitle.Text = string.IsNullOrWhiteSpace(book.Subtitle) ? string.Empty : book.Subtitle;

            // Category - prefer navigation property, fallback to repository lookup, show empty when unknown
            try
            {
                string categoryName = string.Empty;
                if (book.Category != null && !string.IsNullOrWhiteSpace(book.Category.Name))
                    categoryName = book.Category.Name.Trim();
                else if (book.CategoryID > 0)
                {
                    try
                    {
                        var catRepo = new LMS.DataAccess.Repositories.CategoryRepository();
                        var cat = catRepo.GetById(book.CategoryID);
                        if (cat != null && !string.IsNullOrWhiteSpace(cat.Name))
                            categoryName = cat.Name.Trim();
                    }
                    catch { categoryName = string.Empty; }
                }

                LblCategory.Text = string.IsNullOrWhiteSpace(categoryName) ? string.Empty : categoryName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: category load failed: " + ex);
                LblCategory.Text = string.Empty;
            }

            // Authors (use CatalogManager helper to prefer role="Author")
            try
            {
                var cm = new CatalogManager();
                var authors = cm.GetAuthorsByBookIdAndRole(book.BookID, "Author");
                string authorsCsv = FormatAuthorsCsv(authors);
                LblAuthors.Text = $"Author(s) : { (string.IsNullOrWhiteSpace(authorsCsv) ? string.Empty : authorsCsv) }";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: author load failed: " + ex);
                LblAuthors.Text = "Author(s) : ";
            }

            // Editors / Advisers depending on resource type
            try
            {
                var cm = new CatalogManager();
                bool isThesis = false;
                try { isThesis = (book.ResourceType == ResourceType.Thesis); } catch { isThesis = false; }

                string roleToQuery = isThesis ? "Adviser" : "Editor";
                var editors = cm.GetAuthorsByBookIdAndRole(book.BookID, roleToQuery);
                string editorsCsv = FormatAuthorsCsv(editors);

                string prefix = isThesis ? "Adviser(s) :" : "Editor(s) :";
                LblEditors.Text = $"{prefix} { (string.IsNullOrWhiteSpace(editorsCsv) ? string.Empty : editorsCsv) }";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: editor/adviser load failed: " + ex);
                LblEditors.Text = (book.ResourceType == ResourceType.Thesis) ? "Adviser(s) :" : "Editor(s) :";
            }

            // ===== New: populate the requested labels =====

            // Resource Type (friendly label)
            try
            {
                string resourceLabel;
                switch (book.ResourceType)
                {
                    case ResourceType.EBook:
                        resourceLabel = "E-Book";
                        break;
                    case ResourceType.Thesis:
                        resourceLabel = "Thesis";
                        break;
                    case ResourceType.AV:
                        resourceLabel = "Audio-Visual";
                        break;
                    case ResourceType.Periodical:
                        resourceLabel = "Periodical";
                        break;
                    case ResourceType.PhysicalBook:
                    default:
                        resourceLabel = "Book";
                        break;
                }

                // Designer label already contains the static prefix; append the value for clarity.
                LblResourceType.Text = $"Resource Type : {resourceLabel}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to set resource type: " + ex);
                LblResourceType.Text = "Resource Type :";
            }

            // Call Number
            try
            {
                var callNo = string.IsNullOrWhiteSpace(book.CallNumber) ? string.Empty : book.CallNumber.Trim();
                LblCallNumber.Text = string.IsNullOrWhiteSpace(callNo) ? "Call Number :" : $"Call Number : {callNo}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to set call number: " + ex);
                LblCallNumber.Text = "Call Number :";
            }

            // Publisher - prefer navigation property then repository fallback
            try
            {
                string publisherName = string.Empty;
                if (book.Publisher != null && !string.IsNullOrWhiteSpace(book.Publisher.Name))
                {
                    publisherName = book.Publisher.Name.Trim();
                }
                else if (book.PublisherID > 0)
                {
                    try
                    {
                        var pubRepo = new PublisherRepository();
                        var pub = pubRepo.GetById(book.PublisherID);
                        if (pub != null && !string.IsNullOrWhiteSpace(pub.Name))
                            publisherName = pub.Name.Trim();
                    }
                    catch { publisherName = string.Empty; }
                }

                LblPublisher.Text = string.IsNullOrWhiteSpace(publisherName) ? "Publisher :" : $"Publisher : {publisherName}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to set publisher: " + ex);
                LblPublisher.Text = "Publisher :";
            }

            // Language
            try
            {
                var lang = string.IsNullOrWhiteSpace(book.Language) ? string.Empty : book.Language.Trim();
                LblLanguage.Text = string.IsNullOrWhiteSpace(lang) ? "Language :" : $"Language : {lang}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to set language: " + ex);
                LblLanguage.Text = "Language :";
            }

            // Pages
            try
            {
                LblPages.Text = (book.Pages > 0) ? $"Pages : {book.Pages}" : "Pages :";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to set pages: " + ex);
                LblPages.Text = "Pages :";
            }

            // ===== New: Total copies, Available for borrow, populate DgwBookCopy =====
            try
            {
                // Ensure the grid exists
                if (DgvBookCopy != null)
                {
                    // Prepare grid: clear rows and ensure button column shows text
                    try
                    {
                        if (DgvBookCopy.Columns.Contains("ColumnBarcode"))
                        {
                            var btnCol = DgvBookCopy.Columns["ColumnBarcode"] as DataGridViewButtonColumn;
                            if (btnCol != null) btnCol.UseColumnTextForButtonValue = true;
                        }
                    }
                    catch { /* ignore */ }

                    DgvBookCopy.Rows.Clear();

                    List<BookCopy> copies = book.Copies;
                    if (copies == null)
                        copies = new List<BookCopy>();

                    int totalCopies = copies.Count;
                    int availableCopies = copies.Count(c => string.Equals(c?.Status, "Available", StringComparison.OrdinalIgnoreCase));

                    // Set labels
                    try { LblTotalCopies.Text = $"Total Copies : {totalCopies}"; } catch { }
                    try { LblAvailableForBorrow.Text = $"Available for borrow: {availableCopies}"; } catch { }

                    int idx = 1;
                    foreach (var copy in copies.OrderBy(c => c.DateAdded))
                    {
                        int rowIndex = DgvBookCopy.Rows.Add();
                        var row = DgvBookCopy.Rows[rowIndex];

                        // ColumnNumbering
                        if (DgvBookCopy.Columns.Contains("ColumnNumbering"))
                            row.Cells["ColumnNumbering"].Value = idx.ToString();

                        // Accession number
                        if (DgvBookCopy.Columns.Contains("ColumnAccessionNumber"))
                            row.Cells["ColumnAccessionNumber"].Value = string.IsNullOrWhiteSpace(copy?.AccessionNumber) ? "N/A" : copy.AccessionNumber;

                        // Location
                        if (DgvBookCopy.Columns.Contains("ColumnLocation"))
                            row.Cells["ColumnLocation"].Value = string.IsNullOrWhiteSpace(copy?.Location) ? "N/A" : copy.Location;

                        // Status
                        if (DgvBookCopy.Columns.Contains("ColumnStatus"))
                            row.Cells["ColumnStatus"].Value = string.IsNullOrWhiteSpace(copy?.Status) ? "N/A" : copy.Status;

                        // DateAdded
                        if (DgvBookCopy.Columns.Contains("ColumnDateAdded"))
                            row.Cells["ColumnDateAdded"].Value = copy?.DateAdded.ToString("MMM dd, yyyy") ?? "N/A";

                        // AddedBy - show AddedByID as fallback (you can replace with lookup to UserRepo if desired)
                        if (DgvBookCopy.Columns.Contains("ColumnAddedBy"))
                        {
                            string addedBy = (copy != null && copy.AddedByID > 0) ? $"ID:{copy.AddedByID}" : "N/A";
                            row.Cells["ColumnAddedBy"].Value = addedBy;
                        }

                        // Barcode button cell (display text via UseColumnTextForButtonValue above or set value)
                        if (DgvBookCopy.Columns.Contains("ColumnBarcode"))
                        {
                            var cell = row.Cells["ColumnBarcode"];
                            try { cell.Value = "View Barcode"; } catch { }
                        }

                        // Edit/Delete image columns leave as-is (designer images will render)
                        row.Tag = copy;
                        idx++;
                    }
                }
                else
                {
                    // If grid not present, still set labels
                    var copies = book.Copies ?? new List<BookCopy>();
                    int totalCopies = copies.Count;
                    int availableCopies = copies.Count(c => string.Equals(c?.Status, "Available", StringComparison.OrdinalIgnoreCase));
                    try { LblTotalCopies.Text = $"Total Copies : {totalCopies}"; } catch { }
                    try { LblAvailableForBorrow.Text = $"Available for borrow: {availableCopies}"; } catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to populate copies grid/labels: " + ex);
                try { LblTotalCopies.Text = "Total Copies :"; } catch { }
                try { LblAvailableForBorrow.Text = "Available for borrow:"; } catch { }
            }

            // ===== New: populate the requested labels (standard id, pub date, phys/format, edition, pages) =====
            try
            {
                bool isDigital = !string.IsNullOrWhiteSpace(book.DownloadURL) || book.ResourceType == ResourceType.EBook;

                // Helper to safely format values
                Func<string, string, string> fmt = (label, value) => string.IsNullOrWhiteSpace(value) ? label : $"{label} {value}";

                // Standard ID, Pub/Date label, PhysDesc/Format label, Edition label, Pages label
                string standardLabel = "Standard ID :";
                string pubDateLabel = string.Empty;
                string physDescLabel = string.Empty;
                string editionLabel = string.Empty;
                string pagesLabel = string.Empty;

                switch (book.ResourceType)
                {
                    case ResourceType.PhysicalBook:
                    default:
                        standardLabel = fmt("ISBN :", book.ISBN);
                        pubDateLabel = (book.PublicationYear > 0) ? $"Publication Year : {book.PublicationYear}" : "Publication Year :";
                        physDescLabel = isDigital
                            ? fmt("Format :", book.LoanType ?? "Digital")
                            : fmt("Physical Description :", book.PhysicalDescription);
                        editionLabel = fmt("Edition :", book.Edition);
                        pagesLabel = (book.Pages > 0) ? $"Pages : {book.Pages}" : "Pages :";
                        break;

                    case ResourceType.Periodical:
                        // ISSN often stored in the same field as ISBN in many systems — fallback to ISBN
                        standardLabel = fmt("ISSN :", book.ISBN);
                        pubDateLabel = (book.PublicationYear > 0) ? $"Publication Date : {book.PublicationYear}" : "Publication Date :";
                        // Try to expose volume/issue using Edition if available (best-effort)
                        if (!string.IsNullOrWhiteSpace(book.Edition))
                        {
                            // If edition contains "Vol" or "Issue", surface as-is; otherwise put into Volume.
                            if (book.Edition.IndexOf("Vol", StringComparison.OrdinalIgnoreCase) >= 0
                                || book.Edition.IndexOf("Issue", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                editionLabel = $"Volume/Issue : {book.Edition}";
                            }
                            else
                            {
                                editionLabel = $"Volume : {book.Edition} Issue :";
                            }
                        }
                        else
                        {
                            editionLabel = "Volume :  Issue :";
                        }
                        physDescLabel = isDigital ? fmt("Format :", book.LoanType ?? "Digital") : fmt("Physical Description :", book.PhysicalDescription);
                        pagesLabel = (book.Pages > 0) ? $"Pages : {book.Pages}" : "Pages :";
                        break;

                    case ResourceType.Thesis:
                        standardLabel = fmt("DOI :", book.ISBN); // DOI may be stored in ISBN column - fallback
                        pubDateLabel = (book.PublicationYear > 0) ? $"Publication Year : {book.PublicationYear}" : "Publication Year :";
                        editionLabel = fmt("Degree Level :", book.Edition);
                        physDescLabel = isDigital ? fmt("Format :", book.LoanType ?? "Digital") : fmt("Physical Description :", book.PhysicalDescription);
                        pagesLabel = (book.Pages > 0) ? $"Pages : {book.Pages}" : "Pages :";
                        break;

                    case ResourceType.AV:
                        standardLabel = fmt("ISAN/UPC :", book.ISBN);
                        pubDateLabel = (book.PublicationYear > 0) ? $"Publication Date : {book.PublicationYear}" : "Publication Date :";
                        editionLabel = fmt("Edition :", book.Edition);
                        // show duration in seconds using Pages field (as requested)
                        pagesLabel = (book.Pages > 0) ? $"Duration (s) : {book.Pages}" : "Duration (s) :";
                        physDescLabel = isDigital ? fmt("Format :", book.LoanType ?? "Digital") : fmt("Physical Description :", book.PhysicalDescription);
                        break;

                    case ResourceType.EBook:
                        standardLabel = fmt("ISBN :", book.ISBN);
                        pubDateLabel = (book.PublicationYear > 0) ? $"Publication Year : {book.PublicationYear}" : "Publication Year :";
                        physDescLabel = fmt("Format :", book.LoanType ?? "Digital");
                        editionLabel = fmt("Edition :", book.Edition);
                        pagesLabel = (book.Pages > 0) ? $"Pages : {book.Pages}" : "Pages :";
                        break;
                }

                // Apply to designer labels (defensive: ensure control exists)
                try { LblStandardID.Text = standardLabel; } catch { }
                try { LblPubDateYear.Text = string.IsNullOrWhiteSpace(pubDateLabel) ? string.Empty : pubDateLabel; } catch { }
                try { LblPhysDescFormat.Text = physDescLabel; } catch { }
                try { LblEdition.Text = editionLabel; } catch { }
                try { LblPages.Text = pagesLabel; } catch { }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateBasicDetails: failed to set standard/pub/phys/edition/pages labels: " + ex);
            }

            // update buttons (if permission context already set this will show correct controls;
            // if not, SetPermissionContext will call UpdateActionButtons after being set)
            UpdateActionButtons();
        }

        // Centralized logic that shows/hides action buttons based on current book + permission context
        private void UpdateActionButtons()
        {
            try
            {
                // hide everything first
                try { BtnBorrow.Visible = BtnBorrow.Enabled = false; } catch { }
                try { BtnReserve.Visible = BtnReserve.Enabled = false; } catch { }
                try { BtnDownloadLink.Visible = BtnDownloadLink.Enabled = false; } catch { }

                if (_currentBook == null)
                    return;

                // download button is independent of permission
                if (!string.IsNullOrWhiteSpace(_currentBook.DownloadURL))
                {
                    try { BtnDownloadLink.Visible = BtnDownloadLink.Enabled = true; } catch { }
                    return;
                }

                // Determine if this resource is reference-only
                bool isReferenceLoan = false;
                try
                {
                    var loanType = _currentBook.LoanType;
                    isReferenceLoan = !string.IsNullOrWhiteSpace(loanType) && loanType.Trim().Equals("Reference", StringComparison.OrdinalIgnoreCase);
                }
                catch { isReferenceLoan = false; }

                // use permission service if available, otherwise fall back to RolePermissionService
                IPermissionService perm = _permissionService ?? (IPermissionService)new RolePermissionService();

                bool canBorrow = false;
                bool canReserve = false;
                if (_currentUser != null)
                {
                    try { canBorrow = perm.CanBorrowBooks(_currentUser); } catch { canBorrow = false; }
                    try { canReserve = perm.CanReserveBooks(_currentUser); } catch { canReserve = false; }
                }

                int availableCopies = 0;
                try
                {
                    if (_currentBook.Copies != null && _currentBook.Copies.Count > 0)
                        availableCopies = _currentBook.Copies.Count(c => string.Equals(c?.Status, "Available", StringComparison.OrdinalIgnoreCase));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("UpdateActionButtons: error counting copies: " + ex);
                }

                // If the resource is reference-only, never show Borrow or Reserve regardless of permissions/availability.
                if (isReferenceLoan)
                {
                    // Borrow and Reserve remain hidden (download already handled above).
                    return;
                }

                if (availableCopies > 0)
                {
                    if (canBorrow)
                    {
                        try { BtnBorrow.Visible = BtnBorrow.Enabled = false; } catch { } // preserved original behavior (no borrow UI shown in this path)
                    }
                    // do not show reserve when copies are available
                }
                else
                {
                    if (canReserve)
                    {
                        try { BtnReserve.Visible = BtnReserve.Enabled = true; } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UpdateActionButtons failed: " + ex);
            }
        }

        private static string FormatAuthorsCsv(List<Model.Models.Catalog.Author> authors)
        {
            try
            {
                if (authors == null || authors.Count == 0) return string.Empty;
                var names = authors
                    .Where(a => a != null && !string.IsNullOrWhiteSpace(a.FullName))
                    .Select(a => a.FullName.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
                return string.Join(", ", names);
            }
            catch
            {
                return string.Empty;
            }
        }

        private void LoadCoverImageToPictureBox(PictureBox picBox, string coverImagePath)
        {
            if (picBox == null) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(coverImagePath))
                {
                    string fullPath = Path.Combine(Application.StartupPath, coverImagePath.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(fullPath))
                    {
                        using (var img = Image.FromFile(fullPath))
                        {
                            picBox.Image = new Bitmap(img);
                        }
                        return;
                    }
                }
            }
            catch
            {
                // fall through to placeholder
            }

            // placeholder image
            var placeholder = new Bitmap(picBox.Width > 0 ? picBox.Width : 175, picBox.Height > 0 ? picBox.Height : 220);
            using (var g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.LightGray);
                using (var font = new Font("Segoe UI", 10F))
                using (var brush = new SolidBrush(Color.DimGray))
                {
                    var text = "No Cover";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, (placeholder.Width - size.Width) / 2, (placeholder.Height - size.Height) / 2);
                }
            }
            picBox.Image = placeholder;
        }

        // existing event handlers (left intact)
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void DgwBookCopy_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                var dgv = sender as DataGridView;
                if (dgv == null) return;

                var column = dgv.Columns[e.ColumnIndex];
                if (column == null) return;

                // Only react to the barcode button column
                if (!string.Equals(column.Name, "ColumnBarcode", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(column.HeaderText, "Barcode", StringComparison.OrdinalIgnoreCase))
                    return;

                var row = dgv.Rows[e.RowIndex];
                if (row == null) return;

                // Prefer BookCopy object stored in row.Tag; fallback to cell values if necessary
                var copy = row.Tag as BookCopy;

                string barcodeText = null;
                string accession = null;

                if (copy != null)
                {
                    barcodeText = copy.Barcode;
                    accession = copy.AccessionNumber;
                }
                else
                {
                    try
                    {
                        if (dgv.Columns.Contains("ColumnAccessionNumber"))
                            accession = Convert.ToString(row.Cells["ColumnAccessionNumber"].Value);
                        // try to use status/other cells if Barcode cell contains text
                        if (dgv.Columns.Contains("ColumnBarcode"))
                            barcodeText = Convert.ToString(row.Cells["ColumnBarcode"].Value);
                    }
                    catch { }
                }

                // Choose what to show: prefer explicit barcode, else show accession number
                var toShow = !string.IsNullOrWhiteSpace(barcodeText) ? barcodeText : accession;

                if (string.IsNullOrWhiteSpace(toShow))
                {
                    MessageBox.Show("No barcode or accession number available for this copy.", "No Barcode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Open the barcode viewer (Inventory popup)
                try
                {
                    using (var viewer = new LMS.Presentation.Popup.Inventory.ViewBarcode())
                    {
                        viewer.LoadBarcode(toShow, accession);
                        viewer.ShowDialog(this);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to open ViewBarcode: " + ex);
                    MessageBox.Show("Failed to open barcode viewer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DgwBookCopy_CellContentClick error: " + ex);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                var url = _currentBook?.DownloadURL;
                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("No download link available for this resource.", "Download Link", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dlg = new ViewBookDownloadLink(url))
                {
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BtnCancel_Click (download) failed: " + ex);
                MessageBox.Show("Failed to open download link dialog.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Reserve button click in ViewBookDetails.
        /// Shows a confirmation dialog, creates reservation in DB, and displays result.
        /// </summary>
        private void BtnReserve_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentBook == null)
                {
                    MessageBox.Show("No book selected.", "Reserve", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_currentUser == null)
                {
                    MessageBox.Show("You must be logged in to reserve a book.", "Reservation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Show confirmation dialog
                var result = MessageBox.Show(
                    "Do you want to reserve this book?",
                    "Reserve Book",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var reservationManager = new LMS.BusinessLogic.Managers.ReservationManager();
                        int memberId = reservationManager.GetMemberIdByUserId(_currentUser.UserID);

                        if (memberId <= 0)
                        {
                            MessageBox.Show("Unable to find your member profile.", "Reservation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Check if already reserved
                        if (reservationManager.HasActiveReservation(_currentBook.BookID, memberId))
                        {
                            MessageBox.Show("You already have an active reservation for this book.", "Reservation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Create the reservation
                        var reservation = reservationManager.CreateReservation(_currentBook.BookID, memberId);

                        if (reservation != null)
                        {
                            MessageBox.Show(
                                "This book has been reserved for you. Please check back later for availability.",
                                "Reservation Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // Hide the reserve button after successful reservation
                            try { BtnReserve.Visible = BtnReserve.Enabled = false; } catch { }
                        }
                        else
                        {
                            MessageBox.Show(
                                "Unable to reserve this book. Please try again later.",
                                "Reservation Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("CreateReservation failed: " + ex);
                        MessageBox.Show("An error occurred while reserving the book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BtnReserve_Click failed: " + ex);
                MessageBox.Show("Failed to reserve book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
