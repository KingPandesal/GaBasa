using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.MemberFeatures.Reserve;

namespace LMS.Presentation.UserControls.MemberFeatures
{
    public partial class UCReserve : UserControl
    {
        private readonly ReservationRepository _reservationRepository;
        private int _memberId;
        private List<DTOReservedBookItem> _reservedBooks;
        private List<DTOReservedBookItem> _filteredBooks;

        // Grid layout constants (2-column grid like UCOverdue)
        private const int ColumnsCount = 2;
        private const int CardWidth = 490;
        private const int CardHeight = 205;
        private const int CardSpacingX = 5;
        private const int CardSpacingY = 5;

        public UCReserve()
        {
            InitializeComponent();

            _reservationRepository = new ReservationRepository();

            // Wire up events
            this.Load += UCReserve_Load;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;

            // Enable autoscroll on the panel
            PnlReserveBooks.AutoScroll = true;

            // Hide the template panels
            Pnl1.Visible = false;
            panel1.Visible = false;
        }

        private void UCReserve_Load(object sender, EventArgs e)
        {
            LoadCurrentMember();
            LoadReservedBooks();
        }

        /// <summary>
        /// Allows external callers to set the member ID directly.
        /// </summary>
        public void SetMemberId(int memberId)
        {
            _memberId = memberId;
            LoadReservedBooks();
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

        private void LoadReservedBooks()
        {
            if (_memberId <= 0)
            {
                DisplayNoReservedMessage();
                return;
            }

            try
            {
                _reservedBooks = _reservationRepository.GetReservedBooksForMember(_memberId);

                if (_reservedBooks == null || _reservedBooks.Count == 0)
                {
                    DisplayNoReservedMessage();
                    return;
                }

                // Normalize category to "N/A" when missing
                foreach (var b in _reservedBooks)
                {
                    b.Category = string.IsNullOrWhiteSpace(b.Category) ? "N/A" : b.Category;
                }

                _filteredBooks = _reservedBooks;
                ApplySearchFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load reserved books: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            if (_reservedBooks == null || _reservedBooks.Count == 0)
            {
                DisplayNoReservedMessage();
                return;
            }

            string searchText = TxtSearchBar.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredBooks = _reservedBooks;
            }
            else
            {
                // Search by Title and AccessionNumber
                _filteredBooks = _reservedBooks.Where(b =>
                    (b.Title?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (b.AccessionNumber?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }

            DisplayReservedBooks();
        }

        private void DisplayReservedBooks()
        {
            // Clear existing cards (except the templates)
            var toRemove = PnlReserveBooks.Controls.Cast<Control>()
                .Where(c => c != Pnl1 && c != panel1 && c is Panel)
                .ToList();

            foreach (var ctrl in toRemove)
            {
                PnlReserveBooks.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            if (_filteredBooks == null || _filteredBooks.Count == 0)
            {
                DisplayNoReservedMessage();
                return;
            }

            // Remove "no reserved" label if exists
            var noReservedLabel = PnlReserveBooks.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Name == "LblNoReserved");
            if (noReservedLabel != null)
            {
                PnlReserveBooks.Controls.Remove(noReservedLabel);
                noReservedLabel.Dispose();
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
                PnlReserveBooks.Controls.Add(card);

                index++;
            }
        }

        private Panel CreateBookCard(DTOReservedBookItem book, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(CardWidth, CardHeight),
                Location = new Point(x, y),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Book cover image (matching template Pnl1 dimensions)
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

            // Queue/Pickup status badge
            bool isForPickup = book.ExpirationDate > DateTime.MinValue && book.DaysUntilExpiration >= 0;
            var lblQueueOrPickup = new Label
            {
                Text = isForPickup ? "FOR PICKUP" : "ON QUEUE",
                Font = new Font("Segoe UI", 10F),
                BackColor = isForPickup ? Color.FromArgb(0, 192, 0) : Color.FromArgb(0, 0, 192),
                ForeColor = Color.White,
                Padding = new Padding(3),
                Location = new Point(lblCategory.Right + 10, 40),
                AutoSize = true
            };
            card.Controls.Add(lblQueueOrPickup);

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

            // Reservation Date
            var lblResDate = new Label
            {
                Text = $"Reservation Date: {book.ReservationDate:MMMM d, yyyy}",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 100),
                AutoSize = true
            };
            card.Controls.Add(lblResDate);

            // Status or Expiration Date
            string statusText;
            if (isForPickup)
            {
                statusText = $"EXPIRATION DATE: {book.ExpirationDate:MMMM d, yyyy}";
            }
            else
            {
                statusText = $"STATUS: #{book.QueuePosition} in line";
            }

            var lblStatusOrExp = new Label
            {
                Text = statusText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(170, 130),
                AutoSize = true
            };
            card.Controls.Add(lblStatusOrExp);

            // Cancel Reservation link
            var lblCancel = new Label
            {
                Text = "Cancel Reservation",
                Font = new Font("Segoe UI", 10),
                Location = new Point(360, 180),
                AutoSize = true,
                Cursor = Cursors.Hand,
                ForeColor = Color.FromArgb(175, 37, 50)
            };
            lblCancel.Click += (s, e) => CancelReservation(book.ReservationID);
            card.Controls.Add(lblCancel);

            return card;
        }

        private void CancelReservation(int reservationId)
        {
            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to cancel this reservation?",
                    "Cancel Reservation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = _reservationRepository.UpdateStatus(reservationId, "Cancelled");

                    if (success)
                    {
                        MessageBox.Show("Reservation cancelled successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadReservedBooks(); // Refresh the list
                    }
                    else
                    {
                        MessageBox.Show("Failed to cancel reservation.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling reservation: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void DisplayNoReservedMessage()
        {
            // Clear existing cards
            var toRemove = PnlReserveBooks.Controls.Cast<Control>()
                .Where(c => c != Pnl1 && c != panel1)
                .ToList();

            foreach (var ctrl in toRemove)
            {
                PnlReserveBooks.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            // Add "no reserved" message
            var lblNoReserved = new Label
            {
                Name = "LblNoReserved",
                Text = "📚 You have no reserved books at the moment.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(350, 100)
            };
            PnlReserveBooks.Controls.Add(lblNoReserved);
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength - 3) + "...";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Empty handler from designer
        }
    }
}
