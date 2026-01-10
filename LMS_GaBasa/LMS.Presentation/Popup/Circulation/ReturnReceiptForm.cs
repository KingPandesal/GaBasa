using LMS.Model.DTOs.Circulation;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Circulation
{
    public partial class ReturnReceiptForm : Form
    {
        private readonly int _transactionId;
        private readonly DTOReturnInfo _returnInfo;
        private readonly DateTime _returnDate;
        private readonly decimal _fineAmount;
        private readonly string _condition;

        private PrintDocument _printDoc;
        private PrintPreviewDialog _previewDialog;

        public ReturnReceiptForm(int transactionId, DTOReturnInfo returnInfo, DateTime returnDate, decimal fineAmount, string condition)
        {
            InitializeComponent();

            _transactionId = transactionId;
            _returnInfo = returnInfo;
            _returnDate = returnDate;
            _fineAmount = fineAmount;
            _condition = condition ?? "Good";

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

            // Wire button events (designer has these buttons)
            BtnPreviewReceipt.Click += BtnPreviewReceipt_Click;
            BtnPrintReceipt.Click += BtnPrintReceipt_Click;

            // Ensure we tidy up non-designer resources when form closes.
            this.FormClosed += ReturnReceiptForm_FormClosed;
        }

        private void ReturnReceiptForm_FormClosed(object sender, FormClosedEventArgs e)
        {
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

        private void ReturnReceiptForm_Load(object sender, EventArgs e)
        {
            // Populate labels with data
            LblTransactionID.Text = $"{_transactionId}";

            // Borrower info
            LblValueName.Text = $"{_returnInfo?.MemberName ?? "N/A"}";

            // Book info
            LblValueAccessionNumber.Text = $"{_returnInfo?.AccessionNumber ?? "N/A"}";
            LblValueTitle.Text = $"{_returnInfo?.Title ?? "N/A"}";

            // Dates — use MM/dd/yy format per request (e.g., 10/10/26)
            if (_returnInfo != null)
            {
                LblValueDateBorrowed.Text = _returnInfo.BorrowDate.ToString("MM/dd/yy");
                //LblValueDueDate.Text = _returnInfo.DueDate.ToString("MM/dd/yy");
            }
            else
            {
                LblValueDateBorrowed.Text = "N/A";
                //LblValueDueDate.Text = "N/A";
            }

            // Return date formatted as MM/dd/yy (time intentionally omitted per request)
            LblValueDateReturned.Text = _returnDate.ToString("MM/dd/yy");

            // Condition
            LblValueCondition.Text = _condition;

            // Fine
            if (_fineAmount > 0)
            {
                LblValueFine.Text = $"₱{_fineAmount:N2} (Unpaid)";
                LblValueFine.ForeColor = Color.FromArgb(200, 0, 0);
            }
            else
            {
                LblValueFine.Text = "₱0.00";
                LblValueFine.ForeColor = Color.FromArgb(0, 200, 0);
            }
        }

        private void BtnPreviewReceipt_Click(object sender, EventArgs e)
        {
            try
            {
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
            DrawReceiptForPrint(e.Graphics, e.MarginBounds);
            e.HasMorePages = false;
        }

        private void DrawReceiptForPrint(Graphics g, Rectangle bounds)
        {
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
            g.DrawString("RETURN RECEIPT", titleFont, themeBrush, x, y);
            g.DrawString($"Trans. ID: {_transactionId}", regularFont, themeBrush, x + 350, y);
            y += lineHeight + 20;

            // Separator line
            g.DrawLine(new Pen(themeColor, 1), x, y, bounds.Right, y);
            y += 10;

            // Borrower section
            g.DrawString("Borrower", boldFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Name: {_returnInfo?.MemberName ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight + 10;

            // Book section
            g.DrawString("Book", boldFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Accession No.: {_returnInfo?.AccessionNumber ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Title: {_returnInfo?.Title ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight + 10;

            // Dates — print using MM/dd/yy format
            string borrowedText = _returnInfo != null ? _returnInfo.BorrowDate.ToString("MM/dd/yy") : "N/A";
            string dueText = _returnInfo != null ? _returnInfo.DueDate.ToString("MM/dd/yy") : "N/A";
            string returnedText = _returnDate.ToString("MM/dd/yy");

            g.DrawString($"Date Borrowed: {borrowedText}", regularFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Due Date: {dueText}", regularFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Date Returned: {returnedText}", regularFont, Brushes.Black, x, y); y += lineHeight + 10;

            // Condition and Fine
            g.DrawString($"Condition: {_condition}", regularFont, Brushes.Black, x, y); y += lineHeight;

            string fineText = _fineAmount > 0 ? $"₱{_fineAmount:N2} (Unpaid)" : "₱0.00";
            g.DrawString($"Fine: {fineText}", regularFont, _fineAmount > 0 ? Brushes.Red : Brushes.Black, x, y); y += lineHeight + 20;

            // Separator line
            g.DrawLine(new Pen(themeColor, 1), x, y, bounds.Right, y);
            y += 10;

            // Footer message
            if (_fineAmount > 0)
            {
                g.DrawString("Please settle your fine at the library counter.", smallFont, themeBrush, x, y);
            }
            else
            {
                g.DrawString("Thank you for returning the book on time.", smallFont, themeBrush, x, y);
            }
            y += lineHeight + 20;

            // Processed by
            g.DrawString("Processed By:", regularFont, Brushes.Black, x, y);
            y += lineHeight + 10;
            g.DrawLine(new Pen(themeColor, 1), x + 100, y, x + 300, y);

            // Clean up
            headerFont.Dispose();
            titleFont.Dispose();
            boldFont.Dispose();
            regularFont.Dispose();
            smallFont.Dispose();
            themeBrush.Dispose();
        }
    }
}
