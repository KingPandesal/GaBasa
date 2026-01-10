using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;

namespace LMS.Presentation.Popup.Fines
{
    public partial class FinePaymentReceipt : Form
    {
        private readonly DTOCirculationMemberInfo _member;
        private readonly List<DTOFineRecord> _paidFines;
        private readonly decimal _totalPaid;
        private readonly string _paymentMode;
        private readonly DateTime _paymentDate;
        private readonly List<int> _paymentIds;

        private PrintDocument _printDoc;
        private PrintPreviewDialog _previewDialog;

        public FinePaymentReceipt(
            DTOCirculationMemberInfo member,
            List<DTOFineRecord> paidFines,
            decimal totalPaid,
            string paymentMode,
            DateTime paymentDate,
            List<int> paymentIds)
        {
            InitializeComponent();

            _member = member;
            _paidFines = paidFines ?? new List<DTOFineRecord>();
            _totalPaid = totalPaid;
            _paymentMode = paymentMode ?? "Cash";
            _paymentDate = paymentDate;
            _paymentIds = paymentIds ?? new List<int>();

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

            // Cleanup on close
            this.FormClosed += FinePaymentReceipt_FormClosed;
            this.Load += FinePaymentReceipt_Load;
        }

        private void FinePaymentReceipt_Load(object sender, EventArgs e)
        {
            // Populate labels with data

            // Payer info
            LblValueName.Text = _member?.FullName ?? "N/A";
            LblValueMemberType.Text = _member?.MemberType ?? "N/A";

            // Transaction details - fine types
            if (_paidFines.Count > 0)
            {
                var fineTypes = _paidFines
                    .Select(f => f.FineType)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                LblValueFineType.Text = fineTypes.Count > 0 ? string.Join(", ", fineTypes) : "N/A";
            }
            else
            {
                LblValueFineType.Text = "N/A";
            }

            LblValueDatePaid.Text = _paymentDate.ToString("MMMM d, yyyy hh:mm tt");
            LblValuePaymentMode.Text = _paymentMode;

            // Fine amounts
            LblValueFineAmount.Text = $"₱{_totalPaid:N2}";
            LblValueTotalPaid.Text = $"₱{_totalPaid:N2}";

            // Payment ID: show single ID if one, otherwise comma-separated list
            if (_paymentIds == null || _paymentIds.Count == 0)
            {
                LblPaymentID.Text = "N/A";
            }
            else if (_paymentIds.Count == 1)
            {
                LblPaymentID.Text = _paymentIds[0].ToString();
            }
            else
            {
                LblPaymentID.Text = string.Join(", ", _paymentIds);
            }
        }

        private void FinePaymentReceipt_FormClosed(object sender, FormClosedEventArgs e)
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

            // Header
            g.DrawString("University of Mindanao, Inc.", headerFont, themeBrush, x, y);
            y += 40;

            g.DrawString("TAGUM CITY", regularFont, themeBrush, x + 120, y);
            y += lineHeight + 10;

            // Receipt title
            g.DrawString("PAYMENT RECEIPT", titleFont, themeBrush, x, y);
            g.DrawString($"{_paidFines.Count} Fine(s) Paid", regularFont, themeBrush, x + 350, y);
            y += lineHeight + 20;

            // Separator line
            g.DrawLine(new Pen(themeColor, 1), x, y, bounds.Right, y);
            y += 10;

            // Payer section
            g.DrawString("Payer Details", boldFont, Brushes.Black, x, y); y += lineHeight;
            g.DrawString($"Name: {_member?.FullName ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Member Type: {_member?.MemberType ?? "N/A"}", regularFont, Brushes.Black, x + 16, y); y += lineHeight + 10;

            // Transaction section
            g.DrawString("Transaction Details", boldFont, Brushes.Black, x, y); y += lineHeight;

            // Fine types
            string fineTypesStr = "N/A";
            if (_paidFines.Count > 0)
            {
                var fineTypes = _paidFines
                    .Select(f => f.FineType)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                if (fineTypes.Count > 0)
                    fineTypesStr = string.Join(", ", fineTypes);
            }
            g.DrawString($"Fine Type: {fineTypesStr}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Date Paid: {_paymentDate:MMMM d, yyyy hh:mm tt}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Payment Mode: {_paymentMode}", regularFont, Brushes.Black, x + 16, y); y += lineHeight;
            g.DrawString($"Fine Amount: ₱{_totalPaid:N2}", regularFont, Brushes.Black, x + 16, y); y += lineHeight + 10;

            // Total
            g.DrawString($"Total Paid: ₱{_totalPaid:N2}", boldFont, themeBrush, x, y); y += lineHeight + 20;

            // Separator line
            g.DrawLine(new Pen(themeColor, 1), x, y, bounds.Right, y);
            y += 10;

            // Footer message
            g.DrawString("Thank you for your payment.", smallFont, themeBrush, x, y);
            y += lineHeight;
            g.DrawString("Please keep this receipt for your records.", smallFont, themeBrush, x, y);
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

        private void LblValueDueDate_Click(object sender, EventArgs e)
        {
            // Empty handler - required by Designer
        }
    }
}
