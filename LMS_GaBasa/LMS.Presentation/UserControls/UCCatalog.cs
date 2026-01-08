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
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void UCCatalog_Load(object sender, EventArgs e)
        {
            LoadNewArrivals();
            LoadPopularBooks();
        }

        private void LoadNewArrivals()
        {
            try
            {
                var newArrivals = _catalogManager.GetNewArrivals();
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
                var popularBooks = _catalogManager.GetPopularBooks(10);
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
                Enabled = !(book != null && (string.Equals(book.Status, "Available", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(book.Status, "Available Online", StringComparison.OrdinalIgnoreCase)))
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

            if (string.IsNullOrWhiteSpace(txt))
            {
                ClearSearchResults();
                ShowCatalogPanels();
                return;
            }

            HideCatalogPanels();

            var filters = GetSearchFilters();

            List<DTOCatalogBook> results = null;
            try
            {
                results = _catalogManager.SearchCatalog(filters.Query,
                    filters.Category,
                    filters.Publisher,
                    filters.Year,
                    filters.CallNumber,
                    filters.AccessionNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SearchCatalog failed: " + ex);
                MessageBox.Show("Search failed. See logs for details.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ShowSearchResults(results);
        }

        // The existing TextChanged handler is intentionally left in the file (unused) so designer or third-party control
        // behavior won't break if an event gets wired elsewhere. But it no longer triggers the live search.
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Intentionally left blank to avoid running search while typing.
            // Use Enter key to perform search (handled by TxtSearch_KeyDown).
        }

        private class SearchFilters
        {
            public string Query { get; set; }
            public string Category { get; set; }
            public string Publisher { get; set; }
            public int? Year { get; set; }
            public string CallNumber { get; set; }
            public string AccessionNumber { get; set; }
            public bool FullText { get; set; }
        }

        private SearchFilters GetSearchFilters()
        {
            var f = new SearchFilters();

            string title = ReadControlText("TxtTitle");
            string author = ReadControlText("TxtAuthor");
            string isbn = ReadControlText("TxtISBN");
            string subject = ReadControlText("TxtSubject");
            string category = ReadControlText("CmbCategory");
            string publisher = ReadControlText("TxtPublisher");
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

            f.Category = string.IsNullOrWhiteSpace(category) ? null : category;
            f.Publisher = string.IsNullOrWhiteSpace(publisher) ? null : publisher;
            f.CallNumber = string.IsNullOrWhiteSpace(callNumber) ? null : callNumber;
            f.AccessionNumber = string.IsNullOrWhiteSpace(accession) ? null : accession;
            f.FullText = fullText;

            if (int.TryParse(yearText, out int y))
                f.Year = y;
            else
                f.Year = null;

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
                    return (cb.SelectedItem?.ToString() ?? cb.Text)?.Trim() ?? string.Empty;
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

                    _lvSearchResults.Columns.Add("Cover", 70, HorizontalAlignment.Center);
                    _lvSearchResults.Columns.Add("Standard ID", 120, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Call Number", 120, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Title", 250, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Author", 200, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Publisher", 180, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Category", 140, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Publication Year", 100, HorizontalAlignment.Left);
                    _lvSearchResults.Columns.Add("Action", 100, HorizontalAlignment.Center);

                    _lvSearchResults.MouseMove += LvSearchResults_MouseMove;
                    _lvSearchResults.ItemActivate += LvSearchResults_ItemActivate;
                    _lvSearchResults.MouseClick += LvSearchResults_MouseClick;
                }

                try { _lvSearchResults.Width = Math.Max(500, FlwPnlBooks.ClientSize.Width - 10); } catch { }

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

                        // If you want to detect missing mappings, uncomment a debug line:
                        // if (string.IsNullOrEmpty(isbn)) Debug.WriteLine($"DTO BookID={dto.BookID} has no ISBN mapped.");
                        // if (string.IsNullOrEmpty(callNumber)) Debug.WriteLine($"DTO BookID={dto.BookID} has no CallNumber mapped.");

                        string publisher = dto.Publisher ?? string.Empty;
                        string year = dto.PublicationYear > 0 ? dto.PublicationYear.ToString() : string.Empty;

                        item.SubItems.Add(isbn);
                        item.SubItems.Add(callNumber);
                        item.SubItems.Add(dto.Title ?? string.Empty);
                        item.SubItems.Add(dto.Authors ?? dto.Author ?? string.Empty);
                        item.SubItems.Add(publisher ?? string.Empty);
                        item.SubItems.Add(dto.Category ?? string.Empty);
                        item.SubItems.Add(year ?? string.Empty);

                        bool isAvailable = string.Equals(dto.Status, "Available", StringComparison.OrdinalIgnoreCase)
                                           || string.Equals(dto.Status, "Available Online", StringComparison.OrdinalIgnoreCase);

                        item.SubItems.Add(isAvailable ? string.Empty : "Reserve");
                        item.Tag = dto;
                        _lvSearchResults.Items.Add(item);
                    }
                }

                _lvSearchResults.EndUpdate();

                if (!FlwPnlBooks.Controls.Contains(_lvSearchResults))
                {
                    ClearSearchResults();
                    FlwPnlBooks.Controls.Add(_lvSearchResults);
                    _lvSearchResults.BringToFront();
                }
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

        private string GetDtoPropertyString(DTOCatalogBook dto, string propName)
        {
            if (dto == null || string.IsNullOrWhiteSpace(propName)) return string.Empty;
            try
            {
                var pi = dto.GetType().GetProperty(propName);
                if (pi != null)
                {
                    var val = pi.GetValue(dto);
                    if (val == null) return string.Empty;
                    return val.ToString();
                }
            }
            catch { }
            return string.Empty;
        }

        private Form CreateViewBookDetailsIfAvailable(int bookId)
        {
            try
            {
                var t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(x => x.Name == "ViewBookDetails" && typeof(Form).IsAssignableFrom(x));
                if (t == null) return null;
                var ctor = t.GetConstructor(new[] { typeof(int) });
                if (ctor != null)
                {
                    var form = ctor.Invoke(new object[] { bookId }) as Form;
                    return form;
                }

                var paramless = t.GetConstructor(Type.EmptyTypes);
                if (paramless != null)
                {
                    var form = paramless.Invoke(null) as Form;
                    var prop = t.GetProperty("BookID");
                    if (prop != null && prop.CanWrite) prop.SetValue(form, bookId);
                    return form;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}