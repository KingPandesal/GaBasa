using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        // Constants for PnlBook layout
        private const int BookPanelWidth = 330;
        private const int BookPanelHeight = 210;
        private const int BookPanelSpacing = 0;

        public UCCatalog()
        {
            InitializeComponent();

            // Explicitly pass repository instances to the CatalogManager constructor.
            // This avoids the compiler error when the parameterless constructor is not seen by the compiler.
            _catalogManager = new CatalogManager(
                new BookRepository(),
                new BookCopyRepository(),
                new BookAuthorRepository(),
                new AuthorRepository(),
                new CategoryRepository());

            // Force horizontal scrollbars by setting minimum content width
            var scrollWidth = new Size(1620, 0);
            PnlNewArrivalsSection.AutoScrollMinSize = scrollWidth;
            PnlPopularBooksSection.AutoScrollMinSize = scrollWidth;
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
            sectionPanel.AutoScrollMinSize = new Size(xPosition, 0);
        }

        private Panel CreateBookPanel(DTOCatalogBook book)
        {
            // Main panel
            var panel = new Panel
            {
                Size = new Size(320, 150),  // panel size
                Tag = book, // Store DTO for later use
                //BackColor = Color.Blue
                Margin = new Padding(0)
            };

            // Cover Image
            var picCover = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(100, 140),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            LoadCoverImage(picCover, book.CoverImagePath);
            panel.Controls.Add(picCover);

            // Title - fixed width + ellipsis
            var lblTitle = new Label
            {
                Text = book.Title ?? string.Empty,
                Location = new Point(120, 10),
                AutoSize = false,
                Size = new Size(190, 28),
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                AutoEllipsis = true,
                // BackColor = Color.Pink
            };
            panel.Controls.Add(lblTitle);

            // Category (with colored background)
            var lblCategory = new Label
            {
                Text = book.Category ?? "Uncategorized",
                Location = new Point(120, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.White,
                BackColor = GetCategoryColor(book.Category),
                Padding = new Padding(4)
            };
            panel.Controls.Add(lblCategory);

            // Author - fixed width + ellipsis
            var lblAuthor = new Label
            {
                Text = $"Author: {book.Author ?? "Unknown"}",
                Location = new Point(120, 65),
                AutoSize = false,
                Size = new Size(190, 13),
                Font = new Font("Segoe UI", 8F),
                AutoEllipsis = true,
                // BackColor = Color.Blue
            };
            panel.Controls.Add(lblAuthor);

            // Status - fixed width
            var lblStatus = new Label
            {
                Text = $"Status: {book.Status}",
                Location = new Point(120, 80),
                AutoSize = false,
                Size = new Size(190, 13),
                Font = new Font("Segoe UI", 8F),
                ForeColor = (book.Status == "Available" || book.Status == "Available Online")
                    ? Color.Green
                    : Color.Red,
                AutoEllipsis = true,
                // BackColor = Color.Red
            };
            panel.Controls.Add(lblStatus);

            // View Details Button
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
                Tag = book.BookID
            };
            btnViewDetails.FlatAppearance.BorderColor = Color.FromArgb(175, 37, 50);
            btnViewDetails.Click += BtnViewDetails_Click;
            panel.Controls.Add(btnViewDetails);

            // Reserve Button
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
                Tag = book.BookID
            };
            btnReserve.FlatAppearance.BorderColor = Color.FromArgb(175, 37, 50);
            btnReserve.Click += BtnReserve_Click;
            panel.Controls.Add(btnReserve);

            return panel;
        }


        private void LoadCoverImage(PictureBox picBox, string coverImagePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(coverImagePath))
                {
                    string fullPath = Path.Combine(Application.StartupPath, 
                        coverImagePath.Replace('/', Path.DirectorySeparatorChar));
                    
                    if (File.Exists(fullPath))
                    {
                        picBox.Image = Image.FromFile(fullPath);
                        return;
                    }
                }
            }
            catch
            {
                // Fall through to default image
            }

            // Use a simple placeholder - draw a colored rectangle with text
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

        private Color GetCategoryColor(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Color.Gray;

            // Simple hash-based color assignment for variety
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
            // TODO: Implement view details - will be done later
            var btn = sender as Button;
            if (btn?.Tag is int bookId)
            {
                MessageBox.Show($"View Details for Book ID: {bookId}", "View Details");
            }
        }

        private void BtnReserve_Click(object sender, EventArgs e)
        {
            // TODO: Implement reserve - will be done later
            var btn = sender as Button;
            if (btn?.Tag is int bookId)
            {
                MessageBox.Show($"Reserve Book ID: {bookId}", "Reserve");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LblBtnSearchLogic_Click(object sender, EventArgs e)
        {

            // Toggle panel visibility
            PnlSearchLogic.Visible = !PnlSearchLogic.Visible;

            // Change arrow to show expanded/collapsed state
            if (PnlSearchLogic.Visible)
                LblBtnSearchLogic.Text = "▼ Search Logic";
            else
                LblBtnSearchLogic.Text = "▶ Search Logic";
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void NumPckNoOfCopies_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void PnlNewArrivalsSection_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
