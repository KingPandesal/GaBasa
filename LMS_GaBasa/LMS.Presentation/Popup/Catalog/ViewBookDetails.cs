using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LMS.DataAccess.Repositories;
using LMS.BusinessLogic.Managers;
using LMS.Model.Models.Catalog.Books;
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

                if (availableCopies > 0)
                {
                    if (canBorrow)
                    {
                        try { BtnBorrow.Visible = BtnBorrow.Enabled = true; } catch { }
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

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
