using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.MemberFeatures.Borrowed;

namespace LMS.Presentation.UserControls.MemberFeatures
{
    public partial class UCBorrowed : UserControl
    {
        private readonly CirculationRepository _circulationRepository;
        private readonly ReservationRepository _reservationRepository;
        private int _memberId;
        private List<DTOBorrowedBookItem> _borrowedBooks;
        private List<DTOBorrowedBookItem> _filteredBooks;

        // Grid layout constants (same as UCOverdue)
        private const int ColumnsCount = 2;
        private const int CardWidth = 490;
        private const int CardHeight = 205;
        private const int CardSpacingX = 5;
        private const int CardSpacingY = 5;

        public UCBorrowed()
        {
            InitializeComponent();

            _circulationRepository = new CirculationRepository();
            _reservationRepository = new ReservationRepository();

            // Wire up events
            this.Load += UCBorrowed_Load;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;

            // Enable autoscroll on the panel
            PnlOverdueBooks.AutoScroll = true;

            // Hide the template panel
            Pnl1.Visible = false;
        }

        private void UCBorrowed_Load(object sender, EventArgs e)
        {
            LoadCurrentMember();
            LoadBorrowedBooks();
        }

        /// <summary>
        /// Allows external callers to set the member ID directly.
        /// </summary>
        public void SetMemberId(int memberId)
        {
            _memberId = memberId;
            LoadBorrowedBooks();
        }

        private void LoadCurrentMember()
        {
            try
            {
                // Get current user from the parent form chain
                var mainForm = this.FindForm();
                if (mainForm == null) return;

                // Use reflection to get _currentUser from MainForm
                var currentUserField = mainForm.GetType().GetField("_currentUser",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (currentUserField != null)
                {
                    var currentUser = currentUserField.GetValue(mainForm);
                    if (currentUser != null)
                    {
                        var userIdProp = currentUser.GetType().GetProperty("UserID");
                        if (userIdProp != null)
                        {
                            int userId = (int)userIdProp.GetValue(currentUser);
                            _memberId = _reservationRepository.GetMemberIdByUserId(userId);
                        }
                    }
                }
            }
            catch
            {
                // Fallback: member ID not found
                _memberId = 0;
            }
        }

        private void LoadBorrowedBooks()
        {
            if (_memberId <= 0)
            {
                DisplayNoBorrowedMessage();
                return;
            }

            try
            {
                _borrowedBooks = _circulationRepository.GetBorrowedBooksForMember(_memberId);

                if (_borrowedBooks == null || _borrowedBooks.Count == 0)
                {
                    DisplayNoBorrowedMessage();
                    return;
                }

                // Normalize category to "N/A" when missing
                foreach (var b in _borrowedBooks)
                {
                    b.Category = string.IsNullOrWhiteSpace(b.Category) ? "N/A" : b.Category;
                }

                _filteredBooks = _borrowedBooks;
                ApplySearchFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load borrowed books: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            if (_borrowedBooks == null || _borrowedBooks.Count == 0)
            {
                DisplayNoBorrowedMessage();
                return;
            }

            string searchText = TxtSearchBar.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredBooks = _borrowedBooks;
            }
            else
            {
                // Search by Title and AccessionNumber
                _filteredBooks = _borrowedBooks.Where(b =>
                    (b.Title?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (b.AccessionNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            DisplayBorrowedBooks();
        }

        private void DisplayBorrowedBooks()
        {
            // Clear existing cards (except the template)
            var toRemove = PnlOverdueBooks.Controls.Cast<Control>()
                .Where(c => c != Pnl1 && c is Panel)
                .ToList();

            foreach (var ctrl in toRemove)
            {
                PnlOverdueBooks.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            if (_filteredBooks == null || _filteredBooks.Count == 0)
            {
                DisplayNoBorrowedMessage();
                return;
            }

            // Remove "no borrowed" label if exists
            var noBorrowedLabel = PnlOverdueBooks.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Name == "LblNoBorrowed");
            if (noBorrowedLabel != null)
            {
                PnlOverdueBooks.Controls.Remove(noBorrowedLabel);
                noBorrowedLabel.Dispose();
            }

            // Create cards in a 2-column grid layout
            int index = 0;
            foreach (var book in _filteredBooks)
            {
                int col = index % ColumnsCount;
                int row = index / ColumnsCount;

                int x = col * (CardWidth + CardSpacingX);
                int y = row * (CardHeight + CardSpacingY);

                var card = CreateBookCard(book, x, y);
                PnlOverdueBooks.Controls.Add(card);

                index++;
            }
        }

        private Panel CreateBookCard(DTOBorrowedBookItem book, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(CardWidth, CardHeight),
                Location = new Point(x, y),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Book cover image (same size as UCOverdue)
            var picBox = new PictureBox
            {
                Size = new Size(150, 200),
                Location = new Point(3, 2),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.LightGray
            };
            LoadBookCover(picBox, book.CoverImage);
            card.Controls.Add(picBox);

            // Title
            var lblTitle = new Label
            {
                Text = TruncateText(book.Title ?? "Unknown Title", 35),
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                Location = new Point(170, 10),
                AutoSize = true,
                MaximumSize = new Size(300, 0)
            };
            card.Controls.Add(lblTitle);

            // Category badge
            var lblCategory = new Label
            {
                Text = string.IsNullOrWhiteSpace(book.Category) ? "N/A" : book.Category,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Padding = new Padding(3),
                Location = new Point(170, 40),
                AutoSize = true
            };
            card.Controls.Add(lblCategory);

            // Status badge (Overdue or Borrowed)
            var lblStatus = new Label
            {
                Text = book.IsOverdue ? "Overdue" : "Borrowed",
                Font = new Font("Segoe UI", 10),
                BackColor = book.IsOverdue ? Color.Red : Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                Padding = new Padding(3),
                Location = new Point(lblCategory.Right + 10, 40),
                AutoSize = true
            };
            card.Controls.Add(lblStatus);

            // Resource Type
            var lblResourceType = new Label
            {
                Text = $"Resource Type: {GetFriendlyResourceType(book.ResourceType)}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 80),
                AutoSize = true,
                MaximumSize = new Size(250, 0)
            };
            card.Controls.Add(lblResourceType);

            // Accession Number
            var lblAccession = new Label
            {
                Text = $"Acc. No: {book.AccessionNumber ?? "N/A"}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 100),
                AutoSize = true
            };
            card.Controls.Add(lblAccession);

            // Due Date
            string dueDateText;
            if (book.IsOverdue)
            {
                int daysOverdue = Math.Abs(book.DaysUntilDue);
                dueDateText = $"DUE DATE: {book.DueDate:MMMM dd, yyyy} ({daysOverdue} Days Overdue)";
            }
            else if (book.DaysUntilDue == 0)
            {
                dueDateText = $"DUE DATE: {book.DueDate:MMMM dd, yyyy} (Due Today)";
            }
            else
            {
                dueDateText = $"DUE DATE: {book.DueDate:MMMM dd, yyyy} ({book.DaysUntilDue} Days Left)";
            }

            var lblDueDate = new Label
            {
                Text = dueDateText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(170, 130),
                AutoSize = true,
                ForeColor = book.IsOverdue ? Color.FromArgb(200, 0, 0) : Color.FromArgb(0, 100, 0)
            };
            card.Controls.Add(lblDueDate);

            // View Details link
            var lblViewDetails = new Label
            {
                Text = "View Details",
                Font = new Font("Segoe UI", 10),
                Location = new Point(400, 180),
                AutoSize = true,
                Cursor = Cursors.Hand,
                ForeColor = Color.FromArgb(175, 37, 50)
            };
            lblViewDetails.Click += (s, e) => ViewBookDetails(book.BookID);
            card.Controls.Add(lblViewDetails);

            return card;
        }

        private string GetFriendlyResourceType(string resourceType)
        {
            if (string.IsNullOrWhiteSpace(resourceType))
                return "N/A";

            switch (resourceType.Trim().ToLowerInvariant())
            {
                case "physicalbook":
                case "physical":
                case "book":
                    return "Book";
                case "ebook":
                case "e-book":
                    return "E-Book";
                case "thesis":
                    return "Thesis";
                case "periodical":
                case "periodicals":
                    return "Periodical";
                case "av":
                case "audio-visual":
                case "audio visual":
                    return "Audio-Visual";
                default:
                    return char.ToUpperInvariant(resourceType[0]) + resourceType.Substring(1);
            }
        }

        private void ViewBookDetails(int bookId)
        {
            try
            {
                using (var detailsForm = new LMS.Presentation.Popup.Catalog.ViewBookDetails(bookId))
                {
                    detailsForm.ShowDialog(this.FindForm());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open book details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBookCover(PictureBox picBox, string coverPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(coverPath))
                {
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        coverPath.Replace('/', Path.DirectorySeparatorChar));

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
                // Fall through to placeholder
            }

            // Create placeholder
            var placeholder = new Bitmap(picBox.Width, picBox.Height);
            using (var g = Graphics.FromImage(placeholder))
            {
                g.Clear(Color.LightGray);
                using (var font = new Font("Segoe UI", 10F))
                using (var brush = new SolidBrush(Color.DimGray))
                {
                    var text = "No Cover";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (placeholder.Width - size.Width) / 2,
                        (placeholder.Height - size.Height) / 2);
                }
            }
            picBox.Image = placeholder;
        }

        private void DisplayNoBorrowedMessage()
        {
            // Clear existing cards
            var toRemove = PnlOverdueBooks.Controls.Cast<Control>()
                .Where(c => c != Pnl1)
                .ToList();

            foreach (var ctrl in toRemove)
            {
                PnlOverdueBooks.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            // Add "no borrowed" message
            var lblNoBorrowed = new Label
            {
                Name = "LblNoBorrowed",
                Text = "📚 You have no borrowed books at the moment.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(350, 100)
            };
            PnlOverdueBooks.Controls.Add(lblNoBorrowed);
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength - 3) + "...";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Empty handler from designer
        }
    }
}
