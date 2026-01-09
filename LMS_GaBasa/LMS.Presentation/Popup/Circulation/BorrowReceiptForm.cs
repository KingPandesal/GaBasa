using LMS.Model.DTOs.Circulation;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Circulation
{
    public partial class BorrowReceiptForm : Form
    {
        private readonly int _transactionId;
        private readonly DTOCirculationMemberInfo _member;
        private readonly DTOCirculationBookInfo _book;
        private readonly DateTime _borrowDate;
        private readonly DateTime _dueDate;

        private PrintDocument _printDoc;
        private PrintPreviewDialog _previewDialog;

        public BorrowReceiptForm(int transactionId, DTOCirculationMemberInfo member, DTOCirculationBookInfo book, DateTime borrowDate, DateTime dueDate)
        {
            InitializeComponent();

            _transactionId = transactionId;
            _member = member;
            _book = book;
            _borrowDate = borrowDate;
            _dueDate = dueDate; // fixed: assignment, not invalid expression

            // Setup printing
            _printDoc = new PrintDocument();
            _printDoc.DefaultPageSettings.Landscape = false;
            _printDoc.PrintPage += PrintDoc_PrintPage;

            _previewDialog = new PrintPreviewDialog
            {
                Document = _printDoc,
                Width = 800,
                Height = 1000
            };

            // Wire button events
            BtnPreviewReceipt.Click += BtnPreviewReceipt_Click;
            BtnPrintReceipt.Click += BtnPrintReceipt_Click;

            // Ensure we tidy up non-designer resources when form closes.
            this.FormClosed += BorrowReceiptForm_FormClosed;
        }

        private void BorrowReceiptForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Unsubscribe and dispose print document / preview dialog (designer will still dispose controls)
            if (_printDoc != null)
            {
                _printDoc.PrintPage -= PrintDoc_PrintPage;
                try { _printDoc.Dispose(); } catch { }
            }

            if (_previewDialog != null)
            {
                try { _previewDialog.Dispose(); } catch { }
            }
        }

        private void BorrowReceiptForm_Load(object sender, EventArgs e)
        {
            // Populate labels with data
            LblTransactionID.Text = $"{_transactionId}";

            // Borrower info
            LblValueName.Text = $"{_member?.FullName ?? "N/A"}";
            LblValueMemberType.Text = $"{_member?.MemberType ?? "N/A"}";

            // Book info
            LblValueAccessionNumber.Text = $"{_book?.AccessionNumber ?? "N/A"}";
            LblValueTitle.Text = $"{_book?.Title ?? "N/A"}";

            // Dates
            LblValueDateBorrowed.Text = $"{_borrowDate:MMMM d, yyyy hh:mm tt}";
            LblValueDueDate.Text = $"{_dueDate:MMMM d, yyyy}";
        }

        private void BtnPreviewReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                // If you render a bitmap for exact designer fidelity, ensure bitmap is prepared before preview.
                _previewDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Preview failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new PrintDialog())
                {
                    dlg.Document = _printDoc;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _printDoc.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Print failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Print the panel1 content (the receipt area)
            DrawReceiptForPrint(e.Graphics, e.MarginBounds);
            e.HasMorePages = false;
        }

        private void DrawReceiptForPrint(Graphics g, Rectangle bounds)
        {
            // Layout metrics
            var x = bounds.Left;
            var y = bounds.Top;

            var headerFont = new Font("Old English Text MT", 16f, FontStyle.Bold);
            var titleFont = new Font("Impact", 14f, FontStyle.Regular);
            var boldFont = new Font("Segoe UI", 11f, FontStyle.Bold);
            var regularFont = new Font("Segoe UI", 10f, FontStyle.Regular);
            var smallFont = new Font("Segoe UI", 9f, FontStyle.Regular);

            var themeColor = Color.FromArgb(175, 37, 50);
            var themeBrush = new SolidBrush(themeColor);

            int lineHeight = 28;

            // Header - University name
            g.DrawString("University of Mindanao, Inc.", headerFont, themeBrush, x, y);
            y += 40;

            g.DrawString("TAGUM CITY", regularFont, themeBrush, x + 120, y);
            y += lineHeight + 10;

            // Receipt title and transaction ID
            g.DrawString("BORROW RECEIPT", titleFont, themeBrush, x, y);
            g.DrawString($"Trans. ID: {_transactionId}", regularFont, themeBrush, x + 350, y);
            y += lineHeight + 20;

            // Separator line
            g.DrawLine(new Pen(themeColor, 1), x, y, bounds.Right, y);
            y += 10;

            // Borrower section
            g.DrawString("Borrower", boldFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Name: {_member?.FullName ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Member Type: {_member?.MemberType ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight + 10;

            // Book section
            g.DrawString("Book", boldFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Accession No.: {_book?.AccessionNumber ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Title: {_book?.Title ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight + 10;

            // Dates
            g.DrawString($"Date Borrowed: {_borrowDate:MMMM d, yyyy hh:mm tt}", regularFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Due Date: {_dueDate:MMMM d, yyyy}", regularFont, Brushes.Black, x, y); y += lineHeight + 20;

            // Separator line
            g.DrawLine(new Pen(themeColor, 1), x, y, bounds.Right, y);
            y += 10;

            // Footer message
            g.DrawString("Please return on or before due date to avoid fines.", smallFont, themeBrush, x, y);
            y += lineHeight + 20;

            // Processed by
            g.DrawString("Processed By:", regularFont, Brushes.Black, x, y);
            y += lineHeight + 10;
            g.DrawLine(new Pen(themeColor, 1), x + 100, y, x + 300, y); // signature line

            // Clean up
            headerFont.Dispose();
            titleFont.Dispose();
            boldFont.Dispose();
            regularFont.Dispose();
            smallFont.Dispose();
            themeBrush.Dispose();
        }

        private void LblDateBorrowed_Click(object sender, EventArgs e)
        {
            // Empty handler - required by Designer
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
