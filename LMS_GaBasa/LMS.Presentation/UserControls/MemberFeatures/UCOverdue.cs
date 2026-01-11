using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.MemberFeatures.Overdue;

namespace LMS.Presentation.UserControls.MemberFeatures
{
    public partial class UCOverdue : UserControl
    {
        private readonly CirculationRepository _circulationRepository;
        private readonly ReservationRepository _reservationRepository;
        private int _memberId;
        private List<DTOOverdueBookItem> _overdueBooks;
        private List<DTOOverdueBookItem> _filteredBooks;

        // Grid layout constants
        private const int ColumnsCount = 2;
        private const int CardWidth = 490;
        private const int CardHeight = 205;
        private const int CardSpacingX = 5;
        private const int CardSpacingY = 5;

        public UCOverdue()
        {
            InitializeComponent();

            _circulationRepository = new CirculationRepository();
            _reservationRepository = new ReservationRepository();

            // Wire up events
            this.Load += UCOverdue_Load;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;

            // Enable autoscroll on the panel
            PnlOverdueBooks.AutoScroll = true;

            // Hide the template panel
            Pnl1.Visible = false;
        }

        private void UCOverdue_Load(object sender, EventArgs e)
        {
            LoadCurrentMember();
            LoadOverdueBooks();
        }

        /// <summary>
        /// Allows external callers to set the member ID directly.
        /// </summary>
        public void SetMemberId(int memberId)
        {
            _memberId = memberId;
            LoadOverdueBooks();
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

        private void LoadOverdueBooks()
        {
            if (_memberId <= 0)
            {
                DisplayNoOverdueMessage();
                return;
            }

            try
            {
                _overdueBooks = _circulationRepository.GetOverdueBooksForMember(_memberId);

                if (_overdueBooks == null || _overdueBooks.Count == 0)
                {
                    DisplayNoOverdueMessage();
                    UpdateTotalFine(0m);
                    return;
                }

                // Normalize category to "N/A" when missing to keep UI consistent and simplify searches
                foreach (var b in _overdueBooks)
                {
                    b.Category = string.IsNullOrWhiteSpace(b.Category) ? "N/A" : b.Category;
                }

                _filteredBooks = _overdueBooks;
                ApplySearchFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load overdue books: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            if (_overdueBooks == null || _overdueBooks.Count == 0)
            {
                DisplayNoOverdueMessage();
                return;
            }

            string searchText = TxtSearchBar.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredBooks = _overdueBooks;
            }
            else
            {
                // Only search by Title and AccessionNumber per request.
                _filteredBooks = _overdueBooks.Where(b =>
                    (b.Title?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (b.AccessionNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            DisplayOverdueBooks();
            UpdateTotalFine(_filteredBooks?.Sum(b => b.CurrentFine) ?? 0m);
        }

        private void DisplayOverdueBooks()
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
                DisplayNoOverdueMessage();
                return;
            }

            // Remove "no overdue" label if exists
            var noOverdueLabel = PnlOverdueBooks.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Name == "LblNoOverdue");
            if (noOverdueLabel != null)
            {
                PnlOverdueBooks.Controls.Remove(noOverdueLabel);
                noOverdueLabel.Dispose();
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

        private Panel CreateBookCard(DTOOverdueBookItem book, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(CardWidth, CardHeight),
                Location = new Point(x, y),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Book cover image
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
                MaximumSize = new Size(460, 0)
            };
            card.Controls.Add(lblTitle);

            // Category badge - ensure "N/A" for null/empty/whitespace
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

            // Overdue badge
            var lblOverdue = new Label
            {
                Text = "Overdue",
                Font = new Font("Segoe UI", 10),
                BackColor = Color.Red,
                ForeColor = Color.White,
                Padding = new Padding(3),
                Location = new Point(lblCategory.Right + 10, 40),
                AutoSize = true
            };
            card.Controls.Add(lblOverdue);

            // Authors
            var lblAuthors = new Label
            {
                Text = $"Author(s): {TruncateText(book.Authors ?? "N/A", 45)}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 80),
                AutoSize = true,
                MaximumSize = new Size(250, 0)
            };
            card.Controls.Add(lblAuthors);

            // Accession Number
            var lblAccession = new Label
            {
                Text = $"Acc. No: {book.AccessionNumber ?? "N/A"}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 100),
                AutoSize = true
            };
            card.Controls.Add(lblAccession);

            // Due Date with days overdue
            string dueDateText = $"DUE DATE: {book.DueDate:MMMM dd, yyyy} ({book.DaysOverdue} Days Overdue)";
            var lblDueDate = new Label
            {
                Text = dueDateText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(170, 130),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 0, 0)
            };
            card.Controls.Add(lblDueDate);

            // Current Fine
            var lblFine = new Label
            {
                Text = $"CURRENT FINE: ₱ {book.CurrentFine:N2}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(170, 150),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 0, 0)
            };
            card.Controls.Add(lblFine);

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

        private void DisplayNoOverdueMessage()
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

            // Add "no overdue" message
            var lblNoOverdue = new Label
            {
                Name = "LblNoOverdue",
                Text = "🎉 Great news! You have no overdue books.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(0, 0, 0),
                AutoSize = true,
                Location = new Point(350, 100)
            };
            PnlOverdueBooks.Controls.Add(lblNoOverdue);

            UpdateTotalFine(0m);
        }

        private void UpdateTotalFine(decimal totalFine)
        {
            LblTotalOutstandingFine.Text = $"Total Outstanding Fine: ₱ {totalFine:N2}";

            if (totalFine > 0)
            {
                LblFineWarning.Visible = true;
            }
            else
            {
                LblFineWarning.Visible = false;
            }
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength - 3) + "...";
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Empty handler from designer
        }
    }
}
