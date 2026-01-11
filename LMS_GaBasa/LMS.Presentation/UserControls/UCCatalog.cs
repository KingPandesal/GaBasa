using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.Model.DTOs.Catalog;
using LMS.DataAccess.Repositories;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Enums;

namespace LMS.Presentation.UserControls
{
    public partial class UCCatalog : UserControl
    {
        private readonly ICatalogManager _catalogManager;

        // search results UI
        private ListView _lvSearchResults;
        private ToolTip _lvToolTip;

        // Image list & cache for cover thumbnails in the ListView
        private ImageList _lvImageList;
        private readonly Dictionary<string, int> _imageIndexCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Constants for PnlBook layout
        private const int BookPanelWidth = 330;
        private const int BookPanelHeight = 210;
        private const int BookPanelSpacing = 0;

        // sort state
        private bool _sortTitleAscending = true;
        private bool _sortAuthorAscending = true;
        private bool _sortCallNumberAscending = true;
        private bool _sortPublicationYearAscending = true;

        private const int CallNumberColumnIndex = 2;       // "Call Number" column index (0-based)
        private const int TitleColumnIndex = 3;            // "Title" column index (0-based)
        private const int AuthorColumnIndex = 4;           // "Author" column index (0-based)
        private const int PublicationYearColumnIndex = 7;  // "Publication Year" column index (0-based)

        private enum SortColumn { None, Title, Author, PublicationYear, CallNumber }
        private SortColumn _activeSort = SortColumn.Title; // default

        // Add these fields near the other private fields at the top of the class
        private LMS.BusinessLogic.Security.IPermissionService _permissionService;
        private LMS.Model.Models.Users.User _currentUser;

        /// <summary>
        /// Host must call this to provide the current user and permission service (RolePermissionService).
        /// </summary>
        public void SetPermissionContext(LMS.Model.Models.Users.User currentUser, LMS.BusinessLogic.Security.IPermissionService permissionService)
        {
            _currentUser = currentUser;
            _permissionService = permissionService;
        }

        public UCCatalog()
        {
            InitializeComponent();

            _catalogManager = new CatalogManager(
                new BookRepository(),
                new BookCopyRepository(),
                new BookAuthorRepository(),
                new AuthorRepository(),
                new CategoryRepository());

            // wire search textbox (designer must have TxtSearch).
            // Attach Enter-key handler so search only triggers when user presses Enter.
            var ctrl = this.Controls.Find("TxtSearch", true).FirstOrDefault();
            if (ctrl != null)
            {
                // If the control is a TextBox, ensure the live TextChanged handler is detached
                // so searches won't run while typing. Designer may have wired TxtSearch_TextChanged.
                try
                {
                    if (ctrl is TextBox tb)
                    {
                        tb.TextChanged -= TxtSearch_TextChanged;
                    }
                }
                catch { }

                try { ctrl.KeyDown -= TxtSearch_KeyDown; } catch { }
                ctrl.KeyDown += TxtSearch_KeyDown;
            }

            // New: wire search button (designer must have BtnSearch)
            var btnSearchCtrl = this.Controls.Find("BtnSearch", true).FirstOrDefault() as Button;
            if (btnSearchCtrl != null)
            {
                try { btnSearchCtrl.Click -= BtnSearch_Click; } catch { }
                btnSearchCtrl.Click += BtnSearch_Click;
            }

            // Guard: ensure panels exist before touching them (designer should create them but be defensive)
            try
            {
                var scrollWidth = new Size(1620, 0);
                if (PnlNewArrivalsSection != null) PnlNewArrivalsSection.AutoScrollMinSize = scrollWidth;
                if (PnlPopularBooksSection != null) PnlPopularBooksSection.AutoScrollMinSize = scrollWidth;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set AutoScrollMinSize: " + ex);
            }

            try { WireFilterControls(); } catch { }
            try { WireFilterControls(); } catch { }
            try { InitializeSortControls(); } catch { }
        }

        /// <summary>
        /// Checks if current user is a Member role (not Librarian/Staff).
        /// </summary>
        private bool IsUserMemberRole()
        {
            if (_currentUser == null) return false;
            return _currentUser.Role == Role.Member;
        }

        /// <summary>
        /// Checks if the current Member user can reserve books (has ReservationPrivilege, i.e., not Guest).
        /// Returns false for Librarian/Staff or if permission check fails.
        /// </summary>
        private bool CanCurrentUserReserve()
        {
            // Must be a Member first
            if (!IsUserMemberRole()) return false;

            // Use permission service to check ReservationPrivilege
            try
            {
                if (_permissionService != null && _currentUser != null)
                    return _permissionService.CanReserveBooks(_currentUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CanCurrentUserReserve failed: " + ex);
            }
            return false;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void UCCatalog_Load(object sender, EventArgs e)
        {
            LoadNewArrivals();
            LoadPopularBooks();

            // populate combo boxes from DB
            PopulateComboBoxes();
        }

        private void LoadNewArrivals()
        {
            try
            {
                var newArrivals = _catalogManager.GetNewArrivals(5);
                PopulateBookSection(PnlNewArrivalsSection, newArrivals);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load new arrivals: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPopularBooks()
        {
            try
            {
                var popularBooks = _catalogManager.GetPopularBooks(5);
                PopulateBookSection(PnlPopularBooksSection, popularBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load popular books: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateBookSection(Panel sectionPanel, List<DTOCatalogBook> books)
        {
            if (sectionPanel == null) return;

            // Clear existing controls
            sectionPanel.Controls.Clear();

            if (books == null || books.Count == 0)
            {
                var lblNoBooks = new Label
                {
                    Text = "No books to display",
                    AutoSize = true,
                    Location = new Point(10, 10),
                    Font = new Font("Segoe UI", 10F)
                };
                sectionPanel.Controls.Add(lblNoBooks);
                return;
            }

            int xPosition = 0;

            foreach (var book in books)
            {
                var bookPanel = CreateBookPanel(book);
                bookPanel.Location = new Point(xPosition, 0);
                sectionPanel.Controls.Add(bookPanel);

                xPosition += BookPanelWidth + BookPanelSpacing;
            }

            // Update scroll size based on content
            try
            {
                sectionPanel.AutoScrollMinSize = new Size(xPosition, 0);
            }
            catch { }
        }

        private Panel CreateBookPanel(DTOCatalogBook book)
        {
            var panel = new Panel
            {
                Size = new Size(320, 150),
                Tag = book,
                Margin = new Padding(0)
            };

            var picCover = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(100, 140),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            try { LoadCoverImage(picCover, book?.CoverImagePath); } catch { }
            panel.Controls.Add(picCover);

            var lblTitle = new Label
            {
                Text = book?.Title ?? string.Empty,
                Location = new Point(120, 10),
                AutoSize = false,
                Size = new Size(190, 28),
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                AutoEllipsis = true,
            };
            panel.Controls.Add(lblTitle);

            var lblCategory = new Label
            {
                Text = book?.Category ?? "Uncategorized",
                Location = new Point(120, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.White,
                BackColor = GetCategoryColor(book?.Category),
                Padding = new Padding(4)
            };
            panel.Controls.Add(lblCategory);

            var lblAuthor = new Label
            {
                Text = $"Author: {book?.Author ?? "Unknown"}",
                Location = new Point(120, 65),
                AutoSize = false,
                Size = new Size(190, 13),
                Font = new Font("Segoe UI", 8F),
                AutoEllipsis = true,
            };
            panel.Controls.Add(lblAuthor);

            var lblStatus = new Label
            {
                Text = $"Status: {book?.Status ?? string.Empty}",
                Location = new Point(120, 80),
                AutoSize = false,
                Size = new Size(190, 13),
                Font = new Font("Segoe UI", 8F),
                ForeColor = (book != null && (book.Status == "Available" || book.Status == "Available Online"))
                    ? Color.Green
                    : Color.Red,
                AutoEllipsis = true,
            };
            panel.Controls.Add(lblStatus);

            var btnViewDetails = new Button
            {
                Text = "View Details",
                Location = new Point(120, 114),
                Size = new Size(90, 28),
                BackColor = Color.FromArgb(175, 37, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F),
                Cursor = Cursors.Hand,
                Tag = book?.BookID ?? (object)null
            };
            btnViewDetails.FlatAppearance.BorderColor = Color.FromArgb(175, 37, 50);
            btnViewDetails.Click += BtnViewDetails_Click;
            panel.Controls.Add(btnViewDetails);

            // Determine availability
            bool isBookAvailable = (book != null) &&
                                   (string.Equals(book.Status, "Available", StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(book.Status, "Available Online", StringComparison.OrdinalIgnoreCase));

            // Check if book is Reference type (cannot be reserved)
            bool isReferenceLoan = false;
            try
            {
                var loanType = book?.LoanType;
                isReferenceLoan = !string.IsNullOrWhiteSpace(loanType) && loanType.Trim().Equals("Reference", StringComparison.OrdinalIgnoreCase);
            }
            catch { isReferenceLoan = false; }

            // Reserve button logic:
            // - Only show for Members (not Librarian/Staff)
            // - Only show if member has ReservationPrivilege (not Guest)
            // - Only show if book is NOT available
            // - Only show if book is NOT Reference type
            bool showReserve = IsUserMemberRole() && CanCurrentUserReserve() && !isBookAvailable && !isReferenceLoan;

            var btnReserve = new Button
            {
                Text = "Reserve",
                Location = new Point(220, 114),
                Size = new Size(90, 28),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(175, 37, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F),
                Cursor = Cursors.Hand,
                Tag = book?.BookID ?? (object)null,
                Visible = showReserve,
                Enabled = showReserve
            };
            btnReserve.FlatAppearance.BorderColor = Color.FromArgb(175, 37, 50);
            btnReserve.Click += BtnReserve_Click;
            panel.Controls.Add(btnReserve);

            return panel;
        }

        private void LoadCoverImage(PictureBox picBox, string coverImagePath)
        {
            if (picBox == null) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(coverImagePath))
                {
                    string fullPath = Path.Combine(Application.StartupPath,
                        coverImagePath.Replace('/', Path.DirectorySeparatorChar));

                    if (File.Exists(fullPath))
                    {
                        // load a copy to avoid locking
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
                // Fall through to default image
            }

            var placeholder = new Bitmap(175, 220);
            using (var g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.LightGray);
                using (var font = new Font("Segoe UI", 10F))
                using (var brush = new SolidBrush(Color.DimGray))
                {
                    var text = "No Cover";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (175 - size.Width) / 2,
                        (220 - size.Height) / 2);
                }
            }
            picBox.Image = placeholder;
        }

        private void LblBtnSearchLogic_Click(object sender, EventArgs e)
        {
            PnlSearchLogic.Visible = !PnlSearchLogic.Visible;
            LblBtnSearchLogic.Text = PnlSearchLogic.Visible ? "▼ Search Logic" : "▶ Search Logic";
        }

        private Color GetCategoryColor(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Color.Gray;

            int hash = category.GetHashCode();
            var colors = new[]
            {
                Color.OrangeRed,
                Color.DodgerBlue,
                Color.ForestGreen,
                Color.Purple,
                Color.Goldenrod,
                Color.Teal
            };

            return colors[Math.Abs(hash) % colors.Length];
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength - 3) + "...";
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag is int bookId)
            {
                using (var details = CreateViewBookDetailsIfAvailable(bookId))
                {
                    if (details != null) details.ShowDialog();
                    else MessageBox.Show($"View Details for Book ID: {bookId}", "View Details");
                }
            }
        }

        private void BtnReserve_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag is int bookId)
            {
                MessageBox.Show($"Reserve Book ID: {bookId}", "Reserve");
            }
        }

        // New: handle Enter key to trigger search
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // prevent 'ding' sound and suppress further processing
                e.SuppressKeyPress = true;
                PerformSearch();
            }
        }

        // Execute the search once (called when Enter is pressed)
        private void PerformSearch()
        {
            // get current text from TxtSearch control
            var ctrl = this.Controls.Find("TxtSearch", true).FirstOrDefault();
            var txt = ctrl != null ? (ctrl.Text ?? string.Empty).Trim() : string.Empty;

            // If there's no query and no advanced filters, show catalog panels
            var filters = GetSearchFilters();
            bool hasAnyFilter = !string.IsNullOrWhiteSpace(filters.Query) ||
                                !string.IsNullOrWhiteSpace(filters.Category) ||
                                !string.IsNullOrWhiteSpace(filters.Publisher) ||
                                filters.Year.HasValue || filters.YearFrom.HasValue || filters.YearTo.HasValue ||
                                !string.IsNullOrWhiteSpace(filters.CallNumber) ||
                                !string.IsNullOrWhiteSpace(filters.AccessionNumber) ||
                                !string.IsNullOrWhiteSpace(filters.Author) ||
                                !string.IsNullOrWhiteSpace(filters.Language) ||
                                !string.IsNullOrWhiteSpace(filters.Availability) ||
                                !string.IsNullOrWhiteSpace(filters.ResourceType) ||
                                !string.IsNullOrWhiteSpace(filters.LoanType) ||
                                !string.IsNullOrWhiteSpace(filters.MaterialFormat);

            if (!hasAnyFilter)
            {
                ClearSearchResults();
                ShowCatalogPanels();
                return;
            }

            HideCatalogPanels();

            List<DTOCatalogBook> results = new List<DTOCatalogBook>();

            try
            {
                // get all DTOs (avoid calling SearchCatalog with all-null which intentionally returns empty)
                var all = new List<DTOCatalogBook>();
                try
                {
                    all = _catalogManager.GetPopularBooks(int.MaxValue) ?? new List<DTOCatalogBook>();
                }
                catch
                {
                    try { all = _catalogManager.SearchCatalog("the", null, null, null, null, null) ?? new List<DTOCatalogBook>(); } catch { all = new List<DTOCatalogBook>(); }
                }

                Func<string, string, bool> contains = (src, term) =>
                {
                    if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(term)) return false;
                    return src.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                };

                var predicates = new List<Func<DTOCatalogBook, bool>>();

                // tokens
                var tokens = string.IsNullOrWhiteSpace(filters.Query)
                    ? new string[0]
                    : filters.Query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(t => t.Trim()).Where(t => t.Length > 0).ToArray();

                if (tokens.Length > 0)
                {
                    Func<DTOCatalogBook, string> combinedText = dto =>
                        string.Join(" ", new[]
                        {
                            dto?.Title ?? string.Empty,
                            dto?.Authors ?? string.Empty,
                            dto?.Author ?? string.Empty,
                            dto?.ISBN ?? string.Empty,
                            dto?.Publisher ?? string.Empty,
                            dto?.Category ?? string.Empty,
                            dto?.CallNumber ?? string.Empty,
                            dto?.FirstCopyAccession ?? string.Empty
                        });

                    if (filters.Logic == SearchFilters.LogicMode.And)
                    {
                        predicates.Add(dto =>
                        {
                            var txtAll = combinedText(dto);
                            return tokens.All(t => contains(txtAll, t));
                        });
                    }
                    else
                    {
                        predicates.Add(dto =>
                        {
                            var txtAll = combinedText(dto);
                            return tokens.Any(t => contains(txtAll, t));
                        });
                    }
                }

                // Helper: skip adding predicate for placeholder "All"
                Func<string, bool> isMeaningful = s => !string.IsNullOrWhiteSpace(s) && !s.Equals("All", StringComparison.OrdinalIgnoreCase);

                // Author
                if (isMeaningful(filters.Author))
                {
                    var author = filters.Author.Trim();
                    predicates.Add(dto =>
                    {
                        try
                        {
                            // Prefer authoritative book-level author relationship (only role = "Author")
                            try
                            {
                                var roleAuthors = _catalogManager.GetAuthorsByBookIdAndRole(dto.BookID, "Author");
                                if (roleAuthors != null && roleAuthors.Count > 0)
                                {
                                    // return true if any role-author matches (case-insensitive substring or exact)
                                    foreach (var a in roleAuthors)
                                    {
                                        var name = (a?.FullName ?? string.Empty).Trim();
                                        if (string.IsNullOrWhiteSpace(name)) continue;
                                        if (name.Equals(author, StringComparison.OrdinalIgnoreCase)) return true;
                                        if (name.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0) return true;
                                    }
                                    return false;
                                }
                            }
                            catch
                            {
                                // fall through to DTO-based matching on error
                            }

                            // Fallback: match against DTO fields (Authors / Author), supports comma-separated tokens
                            return AuthorMatchesFilter(dto, author);
                        }
                        catch
                        {
                            return false;
                        }
                    });
                }

                // Category
                if (isMeaningful(filters.Category))
                {
                    var cat = filters.Category.Trim();
                    predicates.Add(dto => !string.IsNullOrWhiteSpace(dto.Category) && string.Equals(dto.Category.Trim(), cat, StringComparison.OrdinalIgnoreCase));
                }

                // Publisher
                if (isMeaningful(filters.Publisher))
                {
                    var pub = filters.Publisher.Trim();
                    predicates.Add(dto => !string.IsNullOrWhiteSpace(dto.Publisher) && dto.Publisher.IndexOf(pub, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // Language (reflection) -- skip filter if DTO doesn't expose Language
                if (isMeaningful(filters.Language))
                {
                    var lang = filters.Language.Trim();
                    predicates.Add(dto =>
                    {
                        try
                        {
                            var prop = dto.GetType().GetProperty("Language");
                            if (prop == null) return true; // skip filter when DTO lacks the property
                            var val = prop.GetValue(dto) as string;
                            return string.IsNullOrWhiteSpace(val) ? false : string.Equals(val.Trim(), lang, StringComparison.OrdinalIgnoreCase);
                        }
                        catch { return false; }
                    });
                }

                // Availability filter - match UCInventory logic for "Available Online"
                if (isMeaningful(filters.Availability))
                {
                    var avail = filters.Availability.Trim();

                    if (string.Equals(avail, "Available Online", StringComparison.OrdinalIgnoreCase))
                    {
                        // Match UCInventory logic:
                        // A book is digital if ResourceType == "E-Book" OR DownloadURL is not empty
                        // Digital books with a DownloadURL are "Available Online"
                        predicates.Add(dto =>
                        {
                            try
                            {
                                // Check if digital resource (ResourceType is E-Book or has DownloadURL)
                                bool isEbook = !string.IsNullOrWhiteSpace(dto.ResourceType) &&
                                               (dto.ResourceType.IndexOf("ebook", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                dto.ResourceType.IndexOf("e-book", StringComparison.OrdinalIgnoreCase) >= 0);

                                // Try to get DownloadURL (may be null if DTO doesn't have this property yet)
                                string downloadUrl = null;
                                try
                                {
                                    var prop = dto.GetType().GetProperty("DownloadURL");
                                    if (prop != null)
                                        downloadUrl = prop.GetValue(dto) as string;
                                }
                                catch { }

                                bool hasDownloadUrl = !string.IsNullOrWhiteSpace(downloadUrl);
                                bool isDigital = isEbook || hasDownloadUrl;

                                // Per UCInventory: digital with DownloadURL => Available Online
                                if (isDigital && hasDownloadUrl)
                                    return true;

                                // Also match if MaterialFormat is "Digital"
                                bool isDigitalFormat = !string.IsNullOrWhiteSpace(dto.MaterialFormat) &&
                                                       dto.MaterialFormat.IndexOf("digital", StringComparison.OrdinalIgnoreCase) >= 0;
                                if (isDigitalFormat && hasDownloadUrl)
                                    return true;

                                // Fallback: also match if Status explicitly says "Available Online"
                                if (!string.IsNullOrWhiteSpace(dto.Status) &&
                                    dto.Status.IndexOf("Available Online", StringComparison.OrdinalIgnoreCase) >= 0)
                                    return true;

                                return false;
                            }
                            catch { return false; }
                        });
                    }
                    else
                    {
                        // Existing behavior: filter by Status string (Available / Unavailable)
                        predicates.Add(dto => !string.IsNullOrWhiteSpace(dto.Status) && string.Equals(dto.Status.Trim(), avail, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // ResourceType (reflection) -- skip when DTO doesn't contain the property
                if (isMeaningful(filters.ResourceType))
                {
                    var target = filters.ResourceType.Trim();
                    predicates.Add(dto =>
                    {
                        try
                        {
                            var prop = dto.GetType().GetProperty("ResourceType");
                            if (prop == null) return true;
                            var val = prop.GetValue(dto) as string;
                            return string.IsNullOrWhiteSpace(val) ? false : string.Equals(val.Trim(), target, StringComparison.OrdinalIgnoreCase);
                        }
                        catch { return false; }
                    });
                }

                // LoanType (reflection)
                if (isMeaningful(filters.LoanType))
                {
                    var target = filters.LoanType.Trim();
                    predicates.Add(dto =>
                    {
                        try
                        {
                            var prop = dto.GetType().GetProperty("LoanType");
                            if (prop == null) return true;
                            var val = prop.GetValue(dto) as string;
                            return string.IsNullOrWhiteSpace(val) ? false : string.Equals(val.Trim(), target, StringComparison.OrdinalIgnoreCase);
                        }
                        catch { return false; }
                    });
                }

                // MaterialFormat (reflection)
                if (isMeaningful(filters.MaterialFormat))
                {
                    var target = filters.MaterialFormat.Trim();
                    predicates.Add(dto =>
                    {
                        try
                        {
                            var prop = dto.GetType().GetProperty("MaterialFormat");
                            if (prop == null) return true;
                            var val = prop.GetValue(dto) as string;
                            return string.IsNullOrWhiteSpace(val) ? false : string.Equals(val.Trim(), target, StringComparison.OrdinalIgnoreCase);
                        }
                        catch { return false; }
                    });
                }

                // Call number
                if (isMeaningful(filters.CallNumber))
                {
                    var cnum = filters.CallNumber.Trim();
                    predicates.Add(dto => !string.IsNullOrWhiteSpace(dto.CallNumber) && dto.CallNumber.IndexOf(cnum, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // Accession number
                if (isMeaningful(filters.AccessionNumber))
                {
                    var acc = filters.AccessionNumber.Trim();
                    predicates.Add(dto => !string.IsNullOrWhiteSpace(dto.FirstCopyAccession) && dto.FirstCopyAccession.IndexOf(acc, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // Year range / exact year
                if (filters.YearFrom.HasValue || filters.YearTo.HasValue)
                {
                    int from = filters.YearFrom ?? int.MinValue;
                    int to = filters.YearTo ?? int.MaxValue;
                    predicates.Add(dto => dto.PublicationYear > 0 && dto.PublicationYear >= from && dto.PublicationYear <= to);
                }
                else if (filters.Year.HasValue)
                {
                    int y = filters.Year.Value;
                    predicates.Add(dto => dto.PublicationYear == y);
                }

                // Combine predicates according to logic. If no predicates were created, treat candidate set as 'all'
                if (predicates.Count == 0)
                {
                    results = all.ToList();
                }
                else
                {
                    if (filters.Logic == SearchFilters.LogicMode.And)
                        results = all.Where(dto => predicates.All(p => SafeInvokePredicate(p, dto))).ToList();
                    else if (filters.Logic == SearchFilters.LogicMode.Or)
                        results = all.Where(dto => predicates.Any(p => SafeInvokePredicate(p, dto))).ToList();
                    else // Not
                        results = all.Where(dto => predicates.All(p => !SafeInvokePredicate(p, dto))).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PerformSearch failed: " + ex);
                MessageBox.Show("Search failed. See logs for details.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ShowSearchResults(results);
        }

        // small helper to guard predicate execution from exceptions
        private static bool SafeInvokePredicate(Func<DTOCatalogBook, bool> pred, DTOCatalogBook dto)
        {
            try { return pred(dto); }
            catch { return false; }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Intentionally left blank to avoid running search while typing.
            // Use Enter key to perform search (handled by TxtSearch_KeyDown).
        }

        private void WireFilterControls()
        {
            try
            {
                // Apply / Reset buttons (use designer names)
                var btnApply = this.Controls.Find("BtnApplyFilter", true).FirstOrDefault() as Button;
                if (btnApply != null)
                {
                    try { btnApply.Click -= ApplyFilters_Click; } catch { }
                    btnApply.Click += ApplyFilters_Click;
                }

                var btnReset = this.Controls.Find("BtnResetFilter", true).FirstOrDefault() as Button;
                if (btnReset != null)
                {
                    try { btnReset.Click -= ResetFilters_Click; } catch { }
                    btnReset.Click += ResetFilters_Click;
                }

                // Radio buttons - use designer names and default to AND
                var rAnd = this.Controls.Find("RdoBtnBooleanAND", true).FirstOrDefault() as RadioButton;
                var rOr = this.Controls.Find("RdoBtnBooleanOR", true).FirstOrDefault() as RadioButton;
                var rNot = this.Controls.Find("RdoBtnBooleanNOT", true).FirstOrDefault() as RadioButton;
                if (rAnd != null && rOr != null && rNot != null)
                {
                    rAnd.Checked = true;
                }

                // Also wire numeric up-down value changed handlers (designer names)
                var numFrom = this.Controls.Find("NumPckPublicationYearFrom", true).FirstOrDefault() as NumericUpDown;
                var numTo = this.Controls.Find("NumPckPublicationYearTo", true).FirstOrDefault() as NumericUpDown;
                if (numFrom != null)
                {
                    try { numFrom.ValueChanged -= NumPckPublicationYear_ValueChanged; } catch { }
                    numFrom.ValueChanged += NumPckPublicationYear_ValueChanged;
                }
                if (numTo != null)
                {
                    try { numTo.ValueChanged -= NumPckPublicationYear_ValueChanged; } catch { }
                    numTo.ValueChanged += NumPckPublicationYear_ValueChanged;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WireFilterControls failed: " + ex);
            }
        }

        private void ApplyFilters_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void ResetFilters_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear text search
                var txt = this.Controls.Find("TxtSearch", true).FirstOrDefault() as TextBox;
                if (txt != null) txt.Text = string.Empty;

                // Reset combos to first item (designer names)
                var names = new[] { "CmbBxAuthor", "CmbBxCategory", "CmbBxPublisher", "CmbBxLanguage", "CmbBxAvailability", "CmbBxResourceType", "CmbBxLoanType", "CmbBxMaterialFormat" };
                foreach (var n in names)
                {
                    var c = this.Controls.Find(n, true).FirstOrDefault() as ComboBox;
                    if (c != null)
                    {
                        if (c.Items.Count > 0)
                        {
                            try { c.SelectedIndex = 0; } catch { try { c.Text = "All"; } catch { } }
                        }
                        else
                        {
                            try { c.Text = "All"; } catch { }
                        }
                    }
                }

                // Reset numeric year picks (designer names)
                var nyFrom = this.Controls.Find("NumPckPublicationYearFrom", true).FirstOrDefault() as NumericUpDown;
                var nyTo = this.Controls.Find("NumPckPublicationYearTo", true).FirstOrDefault() as NumericUpDown;
                if (nyFrom != null) nyFrom.Value = nyFrom.Minimum;
                if (nyTo != null) nyTo.Value = nyTo.Maximum;

                // Reset radio logic to AND if present (designer names)
                var rAnd = this.Controls.Find("RdoBtnBooleanAND", true).FirstOrDefault() as RadioButton;
                if (rAnd != null) rAnd.Checked = true;

                // Clear results and show catalog panels
                ClearSearchResults();
                ShowCatalogPanels();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ResetFilters_Click failed: " + ex);
            }
        }

        private class SearchFilters
        {
            public string Query { get; set; }
            public string Category { get; set; }
            public string Publisher { get; set; }
            public int? Year { get; set; }            // kept for single-year exact match when used
            public int? YearFrom { get; set; }        // range start
            public int? YearTo { get; set; }          // range end
            public string CallNumber { get; set; }
            public string AccessionNumber { get; set; }
            public bool FullText { get; set; }

            public string Author { get; set; }
            public string Language { get; set; }

            // Added filter fields for designer combo boxes
            public string Availability { get; set; }
            public string ResourceType { get; set; }
            public string LoanType { get; set; }
            public string MaterialFormat { get; set; }

            public enum LogicMode { And, Or, Not }
            public LogicMode Logic { get; set; } = LogicMode.And;
        }

        private SearchFilters GetSearchFilters()
        {
            var f = new SearchFilters();

            // Title (keep as-is)
            string title = ReadControlText("TxtTitle");

            // Robust author read: prefer SelectedItem when it's an Author object, otherwise fall back to Text
            string author = string.Empty;
            try
            {
                var cmbAuthor = this.Controls.Find("CmbBxAuthor", true).FirstOrDefault() as ComboBox;
                if (cmbAuthor != null)
                {
                    var sel = cmbAuthor.SelectedItem;
                    if (sel is Author a)
                        author = (a.FullName ?? string.Empty).Trim();
                    else
                        author = (cmbAuthor.Text ?? string.Empty).Trim();

                    // Normalise "All" to empty
                    if (string.Equals(author, "All", StringComparison.OrdinalIgnoreCase))
                        author = string.Empty;
                }
                else
                {
                    // Fallback
                    author = ReadControlText("CmbBxAuthor");
                    if (string.Equals(author, "All", StringComparison.OrdinalIgnoreCase)) author = string.Empty;
                }
            }
            catch
            {
                author = ReadControlText("CmbBxAuthor");
                if (string.Equals(author, "All", StringComparison.OrdinalIgnoreCase)) author = string.Empty;
            }

            string isbn = ReadControlText("TxtISBN");
            string subject = ReadControlText("TxtSubject");
            string category = ReadControlText("CmbBxCategory");
            string publisher = ReadControlText("CmbBxPublisher");
            string yearText = ReadControlText("TxtYear");
            string callNumber = ReadControlText("TxtCallNumber");
            string accession = ReadControlText("TxtAccessionNumber");

            bool fullText = false;
            var chk = this.Controls.Find("ChkFullText", true).FirstOrDefault() as CheckBox;
            if (chk != null) fullText = chk.Checked;

            string mainQuery = ReadControlText("TxtSearch");
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(mainQuery)) parts.Add(mainQuery);
            if (!string.IsNullOrWhiteSpace(title)) parts.Add($"title:{title}");
            if (!string.IsNullOrWhiteSpace(author)) parts.Add($"author:{author}");
            if (!string.IsNullOrWhiteSpace(isbn)) parts.Add($"isbn:{isbn}");
            if (!string.IsNullOrWhiteSpace(subject)) parts.Add($"subject:{subject}");

            f.Query = string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

            f.Category = string.IsNullOrWhiteSpace(category) || string.Equals(category, "All", StringComparison.OrdinalIgnoreCase) ? null : category;
            f.Publisher = string.IsNullOrWhiteSpace(publisher) || string.Equals(publisher, "All", StringComparison.OrdinalIgnoreCase) ? null : publisher;
            f.CallNumber = string.IsNullOrWhiteSpace(callNumber) ? null : callNumber;
            f.AccessionNumber = string.IsNullOrWhiteSpace(accession) ? null : accession;
            f.FullText = fullText;

            // Year range numeric controls (designer names)
            var numFrom = this.Controls.Find("NumPckPublicationYearFrom", true).FirstOrDefault() as NumericUpDown;
            var numTo = this.Controls.Find("NumPckPublicationYearTo", true).FirstOrDefault() as NumericUpDown;
            if (numFrom != null && numTo != null)
            {
                int yFrom = Convert.ToInt32(numFrom.Value);
                int yTo = Convert.ToInt32(numTo.Value);
                if (yFrom > 0) f.YearFrom = yFrom;
                if (yTo > 0) f.YearTo = yTo;
            }
            else
            {
                // keep single-year textbox as fallback
                if (int.TryParse(yearText, out int y))
                    f.Year = y;
                else
                    f.Year = null;
            }

            // Use the normalized author value
            f.Author = string.IsNullOrWhiteSpace(author) ? null : author;

            var lang = ReadControlText("CmbBxLanguage");
            f.Language = string.IsNullOrWhiteSpace(lang) || string.Equals(lang, "All", StringComparison.OrdinalIgnoreCase) ? null : lang;

            // Read combo filters (designer names). Map availability synonyms to DTO values.
            var availRaw = ReadControlText("CmbBxAvailability");
            if (string.IsNullOrWhiteSpace(availRaw) || string.Equals(availRaw, "All", StringComparison.OrdinalIgnoreCase))
            {
                f.Availability = null;
            }
            else
            {
                var a = availRaw.Trim();
                // Normalize common UI labels to the DTO.Status values used by CatalogManager ("Available", "Available Online", "Unavailable")
                if (a.Equals("Out of Stock", StringComparison.OrdinalIgnoreCase) || a.Equals("Unavailable", StringComparison.OrdinalIgnoreCase) || a.Equals("OutOfStock", StringComparison.OrdinalIgnoreCase))
                    f.Availability = "Unavailable";
                else if (a.Equals("Available Online", StringComparison.OrdinalIgnoreCase) || a.Equals("Available (Online)", StringComparison.OrdinalIgnoreCase))
                    f.Availability = "Available Online";
                else if (a.Equals("Available", StringComparison.OrdinalIgnoreCase))
                    f.Availability = "Available";
                else
                    f.Availability = a;
            }

            var resType = ReadControlText("CmbBxResourceType");
            f.ResourceType = string.IsNullOrWhiteSpace(resType) || string.Equals(resType, "All", StringComparison.OrdinalIgnoreCase) ? null : resType;

            var loanType = ReadControlText("CmbBxLoanType");
            f.LoanType = string.IsNullOrWhiteSpace(loanType) || string.Equals(loanType, "All", StringComparison.OrdinalIgnoreCase) ? null : loanType;

            var matFmt = ReadControlText("CmbBxMaterialFormat");
            f.MaterialFormat = string.IsNullOrWhiteSpace(matFmt) || string.Equals(matFmt, "All", StringComparison.OrdinalIgnoreCase) ? null : matFmt;

            // Logic radio buttons (designer names)
            var rAnd = this.Controls.Find("RdoBtnBooleanAND", true).FirstOrDefault() as RadioButton;
            var rOr = this.Controls.Find("RdoBtnBooleanOR", true).FirstOrDefault() as RadioButton;
            var rNot = this.Controls.Find("RdoBtnBooleanNOT", true).FirstOrDefault() as RadioButton;
            if (rOr != null && rOr.Checked) f.Logic = SearchFilters.LogicMode.Or;
            else if (rNot != null && rNot.Checked) f.Logic = SearchFilters.LogicMode.Not;
            else f.Logic = SearchFilters.LogicMode.And;

            return f;
        }

        private string ReadControlText(string controlName)
        {
            var c = this.Controls.Find(controlName, true).FirstOrDefault();
            if (c == null) return string.Empty;

            switch (c)
            {
                case TextBox tb:
                    return tb.Text?.Trim() ?? string.Empty;

                case ComboBox cb:
                    // Prefer the displayed text (works for data-bound combos where DisplayMember is set).
                    var displayed = (cb.Text ?? string.Empty).Trim();
                    if (!string.IsNullOrEmpty(displayed))
                        return displayed;

                    // If Text is empty, try to derive a meaningful string from the SelectedItem,
                    // handling common bound types used in this control.
                    var sel = cb.SelectedItem;
                    if (sel != null)
                    {
                        try
                        {
                            // Known DTO / Model types used as DataSource
                            if (sel is Author author) return (author.FullName ?? string.Empty).Trim();
                            if (sel is Category category) return (category.Name ?? string.Empty).Trim();
                            if (sel is Publisher publisher) return (publisher.Name ?? string.Empty).Trim();

                            // Fallback to SelectedItem.ToString()
                            return sel.ToString().Trim();
                        }
                        catch
                        {
                            // swallow and fall through to safe return
                        }
                    }

                    return string.Empty;

                case Label lb:
                    return lb.Text?.Trim() ?? string.Empty;

                case NumericUpDown nud:
                    return nud.Value.ToString();

                default:
                    var prop = c.GetType().GetProperty("Text");
                    if (prop != null)
                    {
                        var val = prop.GetValue(c) as string;
                        return val?.Trim() ?? string.Empty;
                    }
                    return string.Empty;
            }
        }

        private void HideCatalogPanels()
        {
            LblNewArrivals?.Hide();
            PnlNewArrivalsSection?.Hide();
            LblPopularBooks?.Hide();
            PnlPopularBooksSection?.Hide();
        }

        private void ShowCatalogPanels()
        {
            LblNewArrivals?.Show();
            PnlNewArrivalsSection?.Show();
            LblPopularBooks?.Show();
            PnlPopularBooksSection?.Show();
            ClearSearchResults();

            try { SetSortButtonsVisible(false); } catch { }
        }

        private void ShowSearchResults(List<DTOCatalogBook> results)
        {
            try
            {
                if (FlwPnlBooks == null)
                {
                    Debug.WriteLine("FlwPnlBooks is null - cannot show search results.");
                    return;
                }

                if (_lvSearchResults == null)
                {
                    _lvToolTip = new ToolTip();
                    _lvSearchResults = new ListView
                    {
                        View = View.Details,
                        Width = Math.Max(500, FlwPnlBooks.ClientSize.Width - 10),
                        Height = 360,
                        FullRowSelect = true,
                        GridLines = true,
                        MultiSelect = false,
                        UseCompatibleStateImageBehavior = false
                    };

                    _lvSearchResults.CreateControl();
                    _lvSearchResults.GridLines = false;

                    DisposeImageListAndCache();

                    _lvImageList = new ImageList { ImageSize = new Size(64, 100), ColorDepth = ColorDepth.Depth32Bit };
                    _imageIndexCache.Clear();
                    _lvSearchResults.SmallImageList = _lvImageList;

                    // Columns (removed Date Added)
                    _lvSearchResults.Columns.Add("Cover", 70, HorizontalAlignment.Center);
                    _lvSearchResults.Columns.Add("Standard ID", 120, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Call Number", 120, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Title", 250, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Author", 200, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Publisher", 180, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Category", 140, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Publication Year", 100, HorizontalAlignment.Left);
                    // _lvSearchResults.Columns.Add("Date Added", 140, HorizontalAlignment.Left); // removed
                    _lvSearchResults.Columns.Add("Action", 100, HorizontalAlignment.Center);

                    _lvSearchResults.MouseMove += LvSearchResults_MouseMove;
                    _lvSearchResults.ItemActivate += LvSearchResults_ItemActivate;
                    _lvSearchResults.MouseClick += LvSearchResults_MouseClick;
                }

                try { _lvSearchResults.Width = Math.Max(500, FlwPnlBooks.ClientSize.Width - 10); } catch { }

                // Determine if current user can reserve:
                // - Must be Member role
                // - Must have ReservationPrivilege (not Guest, not Librarian/Staff)
                bool canReserve = CanCurrentUserReserve();

                _lvSearchResults.BeginUpdate();
                _lvSearchResults.Items.Clear();

                if (results != null)
                {
                    foreach (var dto in results)
                    {
                        if (dto == null) continue;

                        int imgIndex = EnsureImageInList(dto.CoverImagePath);

                        // ensure image renders even when first column text is visually empty
                        var item = new ListViewItem("\u200B") { ImageIndex = imgIndex };

                        // STRICT: use only the explicit DTO properties (no fallbacks)
                        string isbn = (dto.ISBN ?? string.Empty).Trim();
                        string callNumber = (dto.CallNumber ?? string.Empty).Trim();

                        string publisher = dto.Publisher ?? string.Empty;
                        string year = dto.PublicationYear > 0 ? dto.PublicationYear.ToString() : string.Empty;

                        item.SubItems.Add(isbn);
                        item.SubItems.Add(callNumber);
                        item.SubItems.Add(dto.Title ?? string.Empty);

                        string authorsDisplay = string.Empty;
                        try
                        {
                            // Prefer authors for this book with role = "Author"
                            try
                            {
                                var auths = _catalogManager.GetAuthorsByBookIdAndRole(dto.BookID, "Author");
                                if (auths != null && auths.Count > 0)
                                {
                                    authorsDisplay = string.Join(", ",
                                        auths.Select(a => (a?.FullName ?? string.Empty).Trim())
                                             .Where(n => !string.IsNullOrWhiteSpace(n)));
                                }
                            }
                            catch
                            {
                                authorsDisplay = null;
                            }
                        }
                        catch
                        {
                            authorsDisplay = null;
                        }

                        // Fallback to DTO values when no role-author information available
                        if (string.IsNullOrWhiteSpace(authorsDisplay))
                            authorsDisplay = dto.Authors ?? dto.Author ?? string.Empty;

                        item.SubItems.Add(authorsDisplay);
                        item.SubItems.Add(publisher ?? string.Empty);
                        item.SubItems.Add(dto.Category ?? string.Empty);
                        item.SubItems.Add(year ?? string.Empty);

                        bool isAvailable = string.Equals(dto.Status, "Available", StringComparison.OrdinalIgnoreCase)
                                           || string.Equals(dto.Status, "Available Online", StringComparison.OrdinalIgnoreCase);

                        bool isReference = false;
                        try
                        {
                            isReference = !string.IsNullOrWhiteSpace(dto.LoanType) && dto.LoanType.Trim().Equals("Reference", StringComparison.OrdinalIgnoreCase);
                        }
                        catch { isReference = false; }

                        // Only show "Reserve" when:
                        // - Book is NOT available
                        // - Book is NOT reference-only
                        // - Current user is Member with ReservationPrivilege (not Guest, not Librarian/Staff)
                        bool showReserve = !isAvailable && !isReference && canReserve;
                        item.SubItems.Add(showReserve ? "Reserve" : string.Empty);
                        item.Tag = dto;
                        _lvSearchResults.Items.Add(item);
                    }
                }

                _lvSearchResults.EndUpdate();

                // ensure default/title sort is applied
                try { ApplyCurrentSort(); } catch { }

                if (!FlwPnlBooks.Controls.Contains(_lvSearchResults))
                {
                    ClearSearchResults();
                    FlwPnlBooks.Controls.Add(_lvSearchResults);
                    _lvSearchResults.BringToFront();
                }

                // Show sort buttons now that the ListView is created and shown
                try { SetSortButtonsVisible(true); } catch { }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShowSearchResults failed: " + ex);
                MessageBox.Show("Failed to show search results. See debug output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int EnsureImageInList(string coverImagePath)
        {
            var key = string.IsNullOrWhiteSpace(coverImagePath) ? "<no-cover>" : coverImagePath.Replace('/', Path.DirectorySeparatorChar).Trim();

            if (_imageIndexCache.TryGetValue(key, out int existingIndex))
                return existingIndex;

            Image img = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(coverImagePath))
                {
                    string fullPath = Path.Combine(Application.StartupPath, coverImagePath.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(fullPath))
                    {
                        using (var orig = Image.FromFile(fullPath))
                        {
                            img = new Bitmap(orig);
                        }
                    }
                }
            }
            catch
            {
                img = null;
            }

            if (img == null)
            {
                img = CreatePlaceholderForList();
            }

            int idx = -1;
            try
            {
                if (_lvImageList == null)
                {
                    _lvImageList = new ImageList { ImageSize = new Size(64, 100), ColorDepth = ColorDepth.Depth32Bit };
                    if (_lvSearchResults != null) _lvSearchResults.SmallImageList = _lvImageList;
                }
                _lvImageList.Images.Add(key, img);
                idx = _lvImageList.Images.Count - 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EnsureImageInList failed to add image: " + ex);
                idx = 0;
            }

            _imageIndexCache[key] = idx;
            return idx;
        }

        private Image CreatePlaceholderForList()
        {
            var width = (_lvImageList?.ImageSize.Width) ?? 64;
            var height = (_lvImageList?.ImageSize.Height) ?? 100;
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (var font = new Font("Segoe UI", 8F))
                using (var brush = new SolidBrush(Color.DimGray))
                {
                    var text = "No Cover";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (bmp.Width - size.Width) / 2,
                        (bmp.Height - size.Height) / 2);
                }
            }
            return bmp;
        }

        private void DisposeImageListAndCache()
        {
            try
            {
                if (_lvImageList != null)
                {
                    _lvImageList.Images.Clear();
                    _lvImageList.Dispose();
                    _lvImageList = null;
                }
            }
            catch { }
            _imageIndexCache.Clear();
        }

        private void ClearSearchResults()
        {
            try
            {
                if (_lvSearchResults != null && FlwPnlBooks != null && FlwPnlBooks.Controls.Contains(_lvSearchResults))
                    FlwPnlBooks.Controls.Remove(_lvSearchResults);
            }
            catch { }

            DisposeImageListAndCache();

            // hide sort buttons when there are no results shown
            try { SetSortButtonsVisible(false); } catch { }
        }

        private void LvSearchResults_ItemActivate(object sender, EventArgs e)
        {
            if (_lvSearchResults == null) return;
            if (_lvSearchResults.SelectedItems.Count == 0) return;
            var item = _lvSearchResults.SelectedItems[0];
            var dto = item.Tag as DTOCatalogBook;
            if (dto == null) return;

            using (var details = CreateViewBookDetailsIfAvailable(dto.BookID))
            {
                if (details != null) details.ShowDialog();
                else MessageBox.Show($"Open details for Book ID: {dto.BookID}", "View Details");
            }
        }

        private void LvSearchResults_MouseClick(object sender, MouseEventArgs e)
        {
            if (_lvSearchResults == null) return;
            var info = _lvSearchResults.HitTest(e.Location);
            if (info.Item == null) return;

            int colIndex = GetListViewSubItemIndex(_lvSearchResults, info.Item, e.Location);
            if (colIndex == -1) return;

            var dto = info.Item.Tag as DTOCatalogBook;
            if (dto == null) return;

            if (colIndex == _lvSearchResults.Columns.Count - 1)
            {
                var actionText = info.Item.SubItems[colIndex].Text;
                if (!string.IsNullOrWhiteSpace(actionText) && actionText.Equals("Reserve", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Reserve clicked for Book ID: {dto.BookID}", "Reserve", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
        }

        private void LvSearchResults_MouseMove(object sender, MouseEventArgs e)
        {
            if (_lvSearchResults == null) return;
            var info = _lvSearchResults.HitTest(e.Location);
            if (info.Item == null)
            {
                _lvToolTip?.Hide(_lvSearchResults);
                return;
            }

            _lvToolTip?.Show("Double click to view more details", _lvSearchResults, e.Location + new Size(12, 12), 1500);
        }

        private int GetListViewSubItemIndex(ListView lv, ListViewItem item, Point mouse)
        {
            if (lv == null || item == null) return -1;

            int x = mouse.X;
            int start = item.Bounds.Left;
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                int w = lv.Columns[i].Width;
                int colLeft = (i == 0) ? item.Bounds.Left : start;
                int colRight = colLeft + w;
                if (x >= colLeft && x <= colRight)
                    return i;
                start += w;
            }

            return -1;
        }

        // Replace the existing CreateViewBookDetailsIfAvailable method in UCCatalog with this implementation.
        // This ensures the created ViewBookDetails form receives the current user and permission service
        // by calling its SetPermissionContext(User, IPermissionService) method (via reflection if necessary).

        private Form CreateViewBookDetailsIfAvailable(int bookId)
        {
            try
            {
                var viewType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); } catch { return new Type[0]; }
                    })
                    .FirstOrDefault(t => t.Name == "ViewBookDetails" && typeof(Form).IsAssignableFrom(t));

                if (viewType == null) return null;

                Form form = null;

                // Prefer constructor (int bookId)
                var ctorWithId = viewType.GetConstructor(new[] { typeof(int) });
                if (ctorWithId != null)
                {
                    form = ctorWithId.Invoke(new object[] { bookId }) as Form;
                }
                else
                {
                    // Fallback: parameterless ctor + set BookID property if available
                    var paramless = viewType.GetConstructor(Type.EmptyTypes);
                    if (paramless != null)
                    {
                        form = paramless.Invoke(null) as Form;
                        if (form != null)
                        {
                            try
                            {
                                var prop = viewType.GetProperty("BookID");
                                if (prop != null && prop.CanWrite)
                                    prop.SetValue(form, bookId);
                            }
                            catch { /* ignore property set issues */ }
                        }
                    }
                }

                if (form != null)
                {
                    try
                    {
                        // Try to find an exact SetPermissionContext(User, IPermissionService) method first
                        var setPerm = viewType.GetMethod("SetPermissionContext", new[] {
                            typeof(LMS.Model.Models.Users.User),
                            typeof(LMS.BusinessLogic.Security.IPermissionService)
                        });

                        // Looser fallback: any method named SetPermissionContext with two parameters
                        if (setPerm == null)
                        {
                            setPerm = viewType.GetMethods().FirstOrDefault(m =>
                                m.Name == "SetPermissionContext" && m.GetParameters().Length == 2);
                        }

                        if (setPerm != null)
                        {
                            // Invoke with the catalog's cached current user and permission service.
                            // _currentUser and _permissionService are members of this UCCatalog instance.
                            setPerm.Invoke(form, new object[] { _currentUser, _permissionService });
                        }
                        else
                        {
                            Debug.WriteLine("CreateViewBookDetailsIfAvailable: SetPermissionContext method not found on ViewBookDetails.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("CreateViewBookDetailsIfAvailable: failed to set permission context: " + ex);
                    }
                }

                return form;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateViewBookDetailsIfAvailable: " + ex);
                return null;
            }
        }

        private void PopulateComboBoxes()
        {
            try
            {
                // Authors -> CmbAuthor
                var cmbAuthor = this.Controls.Find("CmbBxAuthor", true).FirstOrDefault() as ComboBox;
                if (cmbAuthor != null)
                {
                    List<Author> authors = null;
                    try
                    {
                        // Prefer authors who appear with role = "Author" in BookAuthor relationships
                        authors = _catalogManager.GetAuthorsByRole("Author") ?? new List<Author>();

                        // If none found (e.g., older DB or no role data), fallback to all authors
                        if (authors == null || authors.Count == 0)
                            authors = _catalogManager.GetAllAuthors() ?? new List<Author>();
                    }
                    catch
                    {
                        authors = new List<Author>();
                    }

                    // Deduplicate and sort by FullName (defensive)
                    var distinct = authors
                        .Where(a => a != null && !string.IsNullOrWhiteSpace(a.FullName))
                        .GroupBy(a => a.FullName.Trim(), StringComparer.OrdinalIgnoreCase)
                        .Select(g => g.First())
                        .OrderBy(a => a.FullName, StringComparer.CurrentCultureIgnoreCase)
                        .ToList();

                    // Build items with "All" first
                    var authorItems = new List<Author> { new Author { AuthorID = 0, FullName = "All" } };
                    authorItems.AddRange(distinct);

                    // Populate Items directly to avoid DataSource binding quirks
                    try
                    {
                        cmbAuthor.DataSource = null;
                        cmbAuthor.Items.Clear();
                        cmbAuthor.DisplayMember = "FullName";
                        cmbAuthor.ValueMember = "AuthorID";
                        foreach (var a in authorItems)
                            cmbAuthor.Items.Add(a);
                        cmbAuthor.SelectedIndex = 0;
                    }
                    catch
                    {
                        try { cmbAuthor.Text = "All"; } catch { }
                    }
                }

                // Categories -> CmbCategory
                var cmbCategory = this.Controls.Find("CmbBxCategory", true).FirstOrDefault() as ComboBox;
                if (cmbCategory != null)
                {
                    var cats = (_catalogManager.GetAllCategories() ?? new List<Category>());
                    var catItems = new List<Category> { new Category { CategoryID = 0, Name = "All" } };
                    catItems.AddRange(cats);
                    cmbCategory.DisplayMember = "Name";
                    cmbCategory.ValueMember = "CategoryID";
                    cmbCategory.DataSource = catItems;
                    cmbCategory.SelectedIndex = 0;
                }

                // Publishers -> CmbPublisher
                var cmbPublisher = this.Controls.Find("CmbBxPublisher", true).FirstOrDefault() as ComboBox;
                if (cmbPublisher != null)
                {
                    List<Publisher> pubs = null;
                    try { pubs = _catalogManager.GetAllPublishers() ?? new PublisherRepository().GetAll() ?? new List<Publisher>(); } catch { pubs = new List<Publisher>(); }

                    var pubItems = new List<Publisher> { new Publisher { PublisherID = 0, Name = "All" } };
                    pubItems.AddRange(pubs);
                    cmbPublisher.DisplayMember = "Name";
                    cmbPublisher.ValueMember = "PublisherID";
                    cmbPublisher.DataSource = pubItems;
                    cmbPublisher.SelectedIndex = 0;
                }

                // Languages -> CmbLanguage (explicit values, "All" first)
                var cmbLanguage = this.Controls.Find("CmbBxLanguage", true).FirstOrDefault() as ComboBox;
                if (cmbLanguage != null)
                {
                    try
                    {
                        var langs = (_catalogManager.GetAllLanguages() ?? new List<string>())
                                    .Where(l => !string.IsNullOrWhiteSpace(l))
                                    .Select(l => l.Trim())
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .OrderBy(l => l)
                                    .ToList();

                        cmbLanguage.DataSource = null;
                        cmbLanguage.Items.Clear();

                        // ensure the "All" option is first
                        cmbLanguage.Items.Add("All");
                        foreach (var ln in langs)
                            cmbLanguage.Items.Add(ln);

                        // Select "All" by default
                        if (cmbLanguage.Items.Count > 0)
                            cmbLanguage.SelectedIndex = 0;
                        else
                            cmbLanguage.Text = "All";
                    }
                    catch
                    {
                        try { cmbLanguage.Text = "All"; } catch { }
                    }
                }

                // Ensure designer-populated combos include "All" as first item and select it.
                var designerComboNames = new[] { "CmbBxAvailability", "CmbBxResourceType", "CmbBxLoanType", "CmbBxMaterialFormat" };
                foreach (var name in designerComboNames)
                {
                    var cb = this.Controls.Find(name, true).FirstOrDefault() as ComboBox;
                    if (cb == null) continue;

                    bool hasAll = false;
                    try
                    {
                        hasAll = cb.Items.Cast<object>().Any(i => string.Equals(Convert.ToString(i), "All", StringComparison.OrdinalIgnoreCase));
                    }
                    catch { hasAll = false; }

                    if (!hasAll)
                    {
                        try { cb.Items.Insert(0, "All"); } catch { }
                    }

                    // Prefer SelectedIndex = 0; if that fails, set Text = "All"
                    try { cb.SelectedIndex = 0; } catch { try { cb.Text = "All"; } catch { } }
                }

                // Publication year numeric pickers - restrict maximum to current year
                var numFrom = this.Controls.Find("NumPckPublicationYearFrom", true).FirstOrDefault() as NumericUpDown;
                var numTo = this.Controls.Find("NumPckPublicationYearTo", true).FirstOrDefault() as NumericUpDown;
                int currentYear = DateTime.Now.Year;
                if (numFrom != null)
                {
                    try
                    {
                        numFrom.Minimum = 0;
                        numFrom.Maximum = currentYear;
                        if (numFrom.Value > numFrom.Maximum) numFrom.Value = numFrom.Maximum;
                        numFrom.ValueChanged -= NumPckPublicationYear_ValueChanged;
                        numFrom.ValueChanged += NumPckPublicationYear_ValueChanged;
                    }
                    catch { }
                }

                if (numTo != null)
                {
                    try
                    {
                        numTo.Minimum = 0;
                        numTo.Maximum = currentYear;
                        if (numTo.Value > numTo.Maximum) numTo.Value = numTo.Maximum;
                        numTo.ValueChanged -= NumPckPublicationYear_ValueChanged;
                        numTo.ValueChanged += NumPckPublicationYear_ValueChanged;
                    }
                    catch { }
                }

                // Resource Type -> CmbBxResourceType (explicit, stable labels)
                var cmbResource = this.Controls.Find("CmbBxResourceType", true).FirstOrDefault() as ComboBox;
                if (cmbResource != null)
                {
                    var resourceItems = new List<string>
                    {
                        "All",
                        "Book",
                        "Periodical",
                        "Thesis",
                        "Audio-Visual",
                        "E-Book"
                    };

                    // Replace items safely
                    try
                    {
                        cmbResource.DataSource = null;
                        cmbResource.Items.Clear();
                        foreach (var r in resourceItems) cmbResource.Items.Add(r);
                        cmbResource.SelectedIndex = 0;
                    }
                    catch
                    {
                        try { cmbResource.Text = "All"; } catch { }
                    }
                }

                // Loan Type -> CmbBxLoanType (explicit values)
                var cmbLoanType = this.Controls.Find("CmbBxLoanType", true).FirstOrDefault() as ComboBox;
                if (cmbLoanType != null)
                {
                    var loanItems = new List<string>
                    {
                        "All",
                        "Circulation",
                        "Reference"
                    };

                    try
                    {
                        cmbLoanType.DataSource = null;
                        cmbLoanType.Items.Clear();
                        foreach (var l in loanItems) cmbLoanType.Items.Add(l);
                        cmbLoanType.SelectedIndex = 0;
                    }
                    catch
                    {
                        try { cmbLoanType.Text = "All"; } catch { }
                    }
                }

                // Availability -> CmbBxAvailability (explicit, includes 'Out of Stock' which maps to DTO 'Unavailable')
                var cmbAvailability = this.Controls.Find("CmbBxAvailability", true).FirstOrDefault() as ComboBox;
                if (cmbAvailability != null)
                {
                    var availItems = new List<string>
                    {
                        "All",
                        "Available",
                        "Available Online",
                        "Out of Stock"
                    };

                    try
                    {
                        cmbAvailability.DataSource = null;
                        cmbAvailability.Items.Clear();
                        foreach (var a in availItems) cmbAvailability.Items.Add(a);
                        cmbAvailability.SelectedIndex = 0;
                    }
                    catch
                    {
                        try { cmbAvailability.Text = "All"; } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PopulateComboBoxes failed: " + ex);
            }
        }

        // Ensure numeric pickers never exceed current year when user edits values
        private void NumPckPublicationYear_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown nud)) return;
            int currentYear = DateTime.Now.Year;
            if (nud.Value > currentYear)
            {
                try { nud.Value = currentYear; } catch { }
            }
        }

        // Add this helper to the UCCatalog class (place it near other private helpers, e.g. below SafeInvokePredicate).
        private bool AuthorMatchesFilter(DTOCatalogBook dto, string authorFilter)
        {
            if (dto == null || string.IsNullOrWhiteSpace(authorFilter)) return false;
            var target = authorFilter.Trim();

            try
            {
                // Primary author quick check
                if (!string.IsNullOrWhiteSpace(dto.Author) &&
                    dto.Author.IndexOf(target, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }

                if (string.IsNullOrWhiteSpace(dto.Authors))
                    return false;

                // Normalize common separators and joiners to commas so split is reliable.
                // Use Regex.Replace to handle " and " case-insensitively.
                var raw = dto.Authors;
                try
                {
                    raw = System.Text.RegularExpressions.Regex.Replace(raw, @"\band\b", ",", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
                catch
                {
                    // if regex fails for any reason, fall back to original string
                }

                // Replace a few other common separators
                raw = raw.Replace("&", ",").Replace("/", ",").Replace("|", ",");

                // Split into tokens and trim
                var tokens = raw
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim())
                    .Where(t => t.Length > 0);

                foreach (var tok in tokens)
                {
                    // exact name match
                    if (string.Equals(tok, target, StringComparison.OrdinalIgnoreCase))
                        return true;

                    // partial match (user typed partial name) -> this makes the author combobox selection match any author token
                    if (tok.IndexOf(target, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }
            catch
            {
                // swallow and return false on unexpected errors
            }

            return false;
        }

        private void InitializeSortControls()
        {
            // Wire up BtnSortTitle if present in designer
            var btnSortTitle = this.Controls.Find("BtnSortTitle", true).FirstOrDefault() as Button;
            if (btnSortTitle != null)
            {
                try { btnSortTitle.Click -= BtnSortTitle_Click; } catch { }
                btnSortTitle.Click += BtnSortTitle_Click;
            }

            // Wire up BtnSortAuthor if present in designer
            var btnSortAuthor = this.Controls.Find("BtnSortAuthor", true).FirstOrDefault() as Button;
            if (btnSortAuthor != null)
            {
                try { btnSortAuthor.Click -= BtnSortAuthor_Click; } catch { }
                btnSortAuthor.Click += BtnSortAuthor_Click;
            }

            // Wire up BtnSortPublicationYear if present in designer
            var btnSortPubYear = this.Controls.Find("BtnSortPublicationYear", true).FirstOrDefault() as Button;
            if (btnSortPubYear != null)
            {
                try { btnSortPubYear.Click -= BtnSortPublicationYear_Click; } catch { }
                btnSortPubYear.Click += BtnSortPublicationYear_Click;
            }

            // Wire up BtnSortCallNumber if present in designer
            var btnSortCallNumber = this.Controls.Find("BtnSortCallNumber", true).FirstOrDefault() as Button;
            if (btnSortCallNumber != null)
            {
                try { btnSortCallNumber.Click -= BtnSortCallNumber_Click; } catch { }
                btnSortCallNumber.Click += BtnSortCallNumber_Click;
            }

            // Set initial button text to show ascending by default (Title is default active)
            UpdateAllSortButtonTexts();

            // Hide sort buttons until the ListView is created and shown
            try { SetSortButtonsVisible(false); } catch { }
        }

        private void BtnSortTitle_Click(object sender, EventArgs e)
        {
            _sortTitleAscending = !_sortTitleAscending;
            _activeSort = SortColumn.Title;
            UpdateAllSortButtonTexts();
            ApplyCurrentSort();
        }

        private void BtnSortAuthor_Click(object sender, EventArgs e)
        {
            _sortAuthorAscending = !_sortAuthorAscending;
            _activeSort = SortColumn.Author;
            UpdateAllSortButtonTexts();
            ApplyCurrentSort();
        }

        private void BtnSortPublicationYear_Click(object sender, EventArgs e)
        {
            _sortPublicationYearAscending = !_sortPublicationYearAscending;
            _activeSort = SortColumn.PublicationYear;
            UpdateAllSortButtonTexts();
            ApplyCurrentSort();
        }

        private void BtnSortCallNumber_Click(object sender, EventArgs e)
        {
            _sortCallNumberAscending = !_sortCallNumberAscending;
            _activeSort = SortColumn.CallNumber;
            UpdateAllSortButtonTexts();
            ApplyCurrentSort();
        }

        private void UpdateAllSortButtonTexts()
        {
            // Title button
            var btnTitle = this.Controls.Find("BtnSortTitle", true).FirstOrDefault() as Button;
            if (btnTitle != null)
            {
                try
                {
                    btnTitle.Text = (_activeSort == SortColumn.Title) ? ("Title" + (_sortTitleAscending ? " ▲" : " ▼")) : "Title";
                }
                catch { }
            }

            // Author button
            var btnAuthor = this.Controls.Find("BtnSortAuthor", true).FirstOrDefault() as Button;
            if (btnAuthor != null)
            {
                try
                {
                    btnAuthor.Text = (_activeSort == SortColumn.Author) ? ("Author" + (_sortAuthorAscending ? " ▲" : " ▼")) : "Author";
                }
                catch { }
            }

            // Publication Year button
            var btnPubYear = this.Controls.Find("BtnSortPublicationYear", true).FirstOrDefault() as Button;
            if (btnPubYear != null)
            {
                try
                {
                    btnPubYear.Text = (_activeSort == SortColumn.PublicationYear)
                        ? ("Publication Year" + (_sortPublicationYearAscending ? " ▲" : " ▼"))
                        : "Publication Year";
                }
                catch { }
            }

            // Call Number button
            var btnCall = this.Controls.Find("BtnSortCallNumber", true).FirstOrDefault() as Button;
            if (btnCall != null)
            {
                try
                {
                    btnCall.Text = (_activeSort == SortColumn.CallNumber) ? ("Call Number" + (_sortCallNumberAscending ? " ▲" : " ▼")) : "Call Number";
                }
                catch { }
            }
        }

        private void ApplyCurrentSort()
        {
            if (_lvSearchResults == null) return;

            try
            {
                switch (_activeSort)
                {
                    case SortColumn.Title:
                        _lvSearchResults.ListViewItemSorter = new ListViewItemComparer(TitleColumnIndex, _sortTitleAscending);
                        break;
                    case SortColumn.Author:
                        _lvSearchResults.ListViewItemSorter = new ListViewItemComparer(AuthorColumnIndex, _sortAuthorAscending);
                        break;
                    case SortColumn.PublicationYear:
                        _lvSearchResults.ListViewItemSorter = new ListViewItemComparer(PublicationYearColumnIndex, _sortPublicationYearAscending);
                        break;
                    case SortColumn.CallNumber:
                        _lvSearchResults.ListViewItemSorter = new ListViewItemComparer(CallNumberColumnIndex, _sortCallNumberAscending);
                        break;
                    default:
                        _lvSearchResults.ListViewItemSorter = null;
                        break;
                }
                _lvSearchResults.Sort();
            }
            catch { }
        }

        // Simple, robust comparer for ListViewItem by subitem text (keeps existing behavior)
        private class ListViewItemComparer : System.Collections.IComparer
        {
            private readonly int _col;
            private readonly bool _ascending;
            public ListViewItemComparer(int col, bool ascending)
            {
                _col = col;
                _ascending = ascending;
            }

            public int Compare(object x, object y)
            {
                var ix = x as ListViewItem;
                var iy = y as ListViewItem;
                if (ix == null || iy == null) return 0;

                string sx = GetSubItemText(ix, _col);
                string sy = GetSubItemText(iy, _col);

                // Special-case: Publication Year should sort numerically (whole number),
                // not lexicographically (so "199" < "2000" correctly).
                try
                {
                    if (_col == PublicationYearColumnIndex)
                    {
                        bool parsedX = int.TryParse(sx, out int vx);
                        bool parsedY = int.TryParse(sy, out int vy);

                        if (parsedX && parsedY)
                        {
                            int cmp = vx.CompareTo(vy);
                            return _ascending ? cmp : -cmp;
                        }

                        if (parsedX && !parsedY)
                            return _ascending ? 1 : -1; // numbers after non-numbers
                        if (!parsedX && parsedY)
                            return _ascending ? -1 : 1;

                        // both non-numeric -> fall back to string compare
                    }
                }
                catch
                {
                    // swallow and fall back to string compare below
                }

                int cmpStr = string.Compare(sx, sy, StringComparison.CurrentCultureIgnoreCase);
                return _ascending ? cmpStr : -cmpStr;
            }

            private static string GetSubItemText(ListViewItem item, int col)
            {
                if (item == null) return string.Empty;
                if (col >= 0 && col < item.SubItems.Count)
                    return item.SubItems[col].Text ?? string.Empty;
                return item.Text ?? string.Empty;
            }
        }

        private void SetSortButtonsVisible(bool visible)
        {
            var buttonNames = new[] { "BtnSortTitle", "BtnSortAuthor", "BtnSortPublicationYear", "BtnSortCallNumber" };
            foreach (var name in buttonNames)
            {
                try
                {
                    var btn = this.Controls.Find(name, true).FirstOrDefault() as Button;
                    if (btn != null)
                    {
                        btn.Visible = visible;
                        // keep enabled state as designed; ensure tab stop behaves when hidden
                        btn.TabStop = visible;
                    }
                }
                catch { }
            }
        }

        private bool IsCurrentUserMember()
        {
            try
            {
                var allowedMemberTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "Student", "Guest", "Faculty", "Staff Member", "Staff", "Member"
                };

                var principal = System.Threading.Thread.CurrentPrincipal;
                if (principal == null)
                {
                    Debug.WriteLine("IsCurrentUserMember: Thread.CurrentPrincipal is null.");
                    return false;
                }

                // Quick IsInRole checks (defensive)
                foreach (var role in allowedMemberTypes)
                {
                    try
                    {
                        if (principal.IsInRole(role))
                        {
                            Debug.WriteLine($"IsCurrentUserMember: principal.IsInRole('{role}') == true");
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"IsCurrentUserMember: IsInRole('{role}') threw: {ex.Message}");
                    }
                }

                // If principal is a ClaimsPrincipal, inspect claims (many systems put role info into claims)
                var claimsPrincipal = principal as System.Security.Claims.ClaimsPrincipal;
                if (claimsPrincipal != null && claimsPrincipal.Claims != null)
                {
                    // Candidate claim types to inspect
                    var roleClaimTypes = new[] {
                        System.Security.Claims.ClaimTypes.Role,
                        "role",
                        "roles",
                        "membertype",
                        "member_type",
                        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    };

                    foreach (var c in claimsPrincipal.Claims)
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(c?.Value)) continue;

                            // If claim type looks like a role type, compare value against allowed set
                            if (roleClaimTypes.Any(t => string.Equals(t, c.Type, StringComparison.OrdinalIgnoreCase)))
                            {
                                Debug.WriteLine($"IsCurrentUserMember: Found role-like claim '{c.Type}' = '{c.Value}'");
                                if (allowedMemberTypes.Contains(c.Value.Trim()))
                                    return true;
                            }

                            // Also accept exact matches for allowedMemberTypes even if claim type is custom
                            if (allowedMemberTypes.Contains(c.Value.Trim()))
                            {
                                Debug.WriteLine($"IsCurrentUserMember: Found allowed member-type claim '{c.Type}' = '{c.Value}'");
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"IsCurrentUserMember: Inspecting claim threw: {ex}");
                        }
                    }

                    // dump all claims for debugging
                    try
                    {
                        foreach (var c in claimsPrincipal.Claims)
                            Debug.WriteLine($"Claim: Type='{c.Type}' Value='{c.Value}'");
                    }
                    catch { }
                }

                // Write identity info for debugging
                try
                {
                    Debug.WriteLine($"IsCurrentUserMember: Identity.Name='{principal.Identity?.Name}', Authenticated={principal.Identity?.IsAuthenticated}");
                }
                catch { }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsCurrentUserMember threw: " + ex);
                return false;
            }
        }

        private void BtnBookViewDetails_Click(object sender, EventArgs e)
        {

        }
    }
}