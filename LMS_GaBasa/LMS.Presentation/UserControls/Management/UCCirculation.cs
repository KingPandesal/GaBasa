using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers.Circulation;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;
using LMS.Presentation.Popup.Multipurpose;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCCirculation : UserControl
    {
        private readonly ICirculationManager _circulationManager;
        private DTOCirculationMemberInfo _currentMember;
        private DTOCirculationBookInfo _currentBook;

        // Max width for author label before truncating
        private const int MaxAuthorLabelWidth = 650;

        public UCCirculation()
        {
            InitializeComponent();

            // Setup dependencies
            var circulationRepo = new CirculationRepository();
            _circulationManager = new CirculationManager(circulationRepo);

            // Wire up events
            TxtMemberID.KeyDown += TxtMemberID_KeyDown;
            BtnEnterMemberID.Click += BtnEnterMemberID_Click;
            BtnViewMemberValidID.Click += BtnViewMemberValidID_Click;

            // Wire scan accession button to open camera dialog
            BtnScanAccessionNumber.Click -= BtnScanAccessionNumber_Click;
            BtnScanAccessionNumber.Click += BtnScanAccessionNumber_Click;

            // Wire accession number entry
            TxtAccessionNumber.KeyDown += TxtAccessionNumber_KeyDown;
            BtnEnterAccessionNumber.Click += BtnEnterAccessionNumber_Click;

            // Wire cancel borrow button
            BtnCancelBorrow.Click += BtnCancelBorrow_Click;

            // Wire confirm borrow button
            BtnConfirmBorrow.Click -= BtnConfirmBorrow_Click;
            BtnConfirmBorrow.Click += BtnConfirmBorrow_Click;

            // Wire return controls
            TxtReturnAccessionNumber.KeyDown += TxtReturnAccessionNumber_KeyDown;
            BtnReturnEnterAccessionNumber.Click += BtnReturnEnterAccessionNumber_Click;
            BtnReturnScanAccessionNumber.Click += BtnReturnScanAccessionNumber_Click;
            BtnCancelReturn.Click += BtnCancelReturn_Click;

            // Initialize UI state
            ClearMemberResults();
            ClearBookResults();

            // Hide book checkout section initially
            GrpBxBookCheckout.Visible = false;
        }

        #region Member Verification

        private void TxtMemberID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                VerifyMember();
            }
        }

        private void BtnEnterMemberID_Click(object sender, EventArgs e)
        {
            VerifyMember();
        }

        private void VerifyMember()
        {
            string inputMemberId = TxtMemberID.Text.Trim();

            if (string.IsNullOrWhiteSpace(inputMemberId))
            {
                MessageBox.Show("Please enter a Member ID.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtMemberID.Focus();
                return;
            }

            var memberInfo = _circulationManager.GetMemberByFormattedId(inputMemberId);

            if (memberInfo == null)
            {
                MessageBox.Show($"Member not found: {inputMemberId}", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearMemberResults();
                TxtMemberID.Focus();
                TxtMemberID.SelectAll();
                return;
            }

            _currentMember = memberInfo;
            DisplayMemberInfo(memberInfo);
            UpdateBookCheckoutVisibility(memberInfo);

            // Clear any previous book selection when member changes
            ClearBookResults();
        }

        private void DisplayMemberInfo(DTOCirculationMemberInfo memberInfo)
        {
            LblFullName.Text = $"Full Name: {memberInfo.FullName}";
            LblMemberType.Text = $"Member Type: {memberInfo.MemberType}";
            LblStatus.Text = $"Status: {memberInfo.Status}";
            SetStatusColor(memberInfo.Status);
            LoadProfileImage(memberInfo.PhotoPath);
            DisplayBorrowingStatistics(memberInfo);
            DisplayEligibilityChecks(memberInfo);
        }

        private void DisplayBorrowingStatistics(DTOCirculationMemberInfo memberInfo)
        {
            LblBorrowed.Text = $"Borrowed: {memberInfo.CurrentBorrowedCount} / {memberInfo.MaxBooksAllowed}";
            LblOverdue.Text = $"Overdue: {memberInfo.OverdueCount}";
            LblFine.Text = $"Fine: ₱{memberInfo.TotalUnpaidFines:N2}";
        }

        private void DisplayEligibilityChecks(DTOCirculationMemberInfo memberInfo)
        {
            if (memberInfo.IsActive)
            {
                LblMemberActive.Text = "✔️ Member Active";
                LblMemberActive.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblMemberActive.Text = $"❌ Member {memberInfo.Status}";
                LblMemberActive.ForeColor = Color.FromArgb(200, 0, 0);
            }

            if (memberInfo.HasNoOverdue)
            {
                LblNoOverdueBooks.Text = "✔️ No Overdue Books";
                LblNoOverdueBooks.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblNoOverdueBooks.Text = $"❌ Has {memberInfo.OverdueCount} Overdue Book(s)";
                LblNoOverdueBooks.ForeColor = Color.FromArgb(200, 0, 0);
            }

            if (memberInfo.IsBorrowLimitOk)
            {
                LblBorrowLimitOK.Text = "✔️ Borrow Limit OK";
                LblBorrowLimitOK.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblBorrowLimitOK.Text = "❌ Borrow Limit Reached";
                LblBorrowLimitOK.ForeColor = Color.FromArgb(200, 0, 0);
            }

            string capText = $"₱{memberInfo.MaxFineCap:N2}";
            if (memberInfo.IsFineWithinLimit)
            {
                LblFineLimit.Text = $"✔️ Fine within allowed limit ({capText})";
                LblFineLimit.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblFineLimit.Text = $"❌ Fine exceeds limit ({capText})";
                LblFineLimit.ForeColor = Color.FromArgb(200, 0, 0);
            }
        }

        private void UpdateBookCheckoutVisibility(DTOCirculationMemberInfo memberInfo)
        {
            GrpBxBookCheckout.Visible = memberInfo.CanBorrow;
        }

        private void SetStatusColor(string status)
        {
            switch (status?.ToLower())
            {
                case "active":
                    LblStatus.ForeColor = Color.FromArgb(0, 200, 0);
                    break;
                case "inactive":
                    LblStatus.ForeColor = Color.FromArgb(200, 200, 0);
                    break;
                case "suspended":
                case "expired":
                    LblStatus.ForeColor = Color.FromArgb(200, 0, 0);
                    break;
                default:
                    LblStatus.ForeColor = Color.Black;
                    break;
            }
        }

        private void LoadProfileImage(string photoPath)
        {
            if (PicBxMemberProfilePicture.Image != null)
            {
                PicBxMemberProfilePicture.Image.Dispose();
                PicBxMemberProfilePicture.Image = null;
            }

            if (string.IsNullOrEmpty(photoPath))
            {
                PicBxMemberProfilePicture.BackColor = Color.Gainsboro;
                return;
            }

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photoPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        PicBxMemberProfilePicture.Image = Image.FromStream(stream);
                    }
                    PicBxMemberProfilePicture.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch
                {
                    PicBxMemberProfilePicture.BackColor = Color.Gainsboro;
                }
            }
            else
            {
                PicBxMemberProfilePicture.BackColor = Color.Gainsboro;
            }
        }

        private void BtnViewMemberValidID_Click(object sender, EventArgs e)
        {
            if (_currentMember == null)
            {
                MessageBox.Show("Please verify a member first.", "No Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_currentMember.ValidIdPath))
            {
                MessageBox.Show("Member has no valid ID on file.", "No Valid ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string candidatePath = _currentMember.ValidIdPath;

            if (!Path.IsPathRooted(candidatePath))
            {
                candidatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, candidatePath);
            }

            if (!File.Exists(candidatePath))
            {
                string alt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "dataimages", "ValidIDs", Path.GetFileName(candidatePath));
                if (File.Exists(alt))
                    candidatePath = alt;
                else
                {
                    MessageBox.Show("Valid ID image not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            using (var viewer = new ViewValidID())
            {
                viewer.LoadValidID(candidatePath);
                viewer.ShowDialog();
            }
        }

        private void ClearMemberResults()
        {
            _currentMember = null;

            LblFullName.Text = "Full Name: ";
            LblMemberType.Text = "Member Type: ";
            LblStatus.Text = "Status: ";
            LblStatus.ForeColor = Color.Black;

            LblMemberActive.Text = "";
            LblBorrowed.Text = "Borrowed: ";
            LblOverdue.Text = "Overdue: ";
            LblFine.Text = "Fine: ";

            LblBorrowLimitOK.Text = "";
            LblNoOverdueBooks.Text = "";
            LblFineLimit.Text = "";

            if (PicBxMemberProfilePicture.Image != null)
            {
                PicBxMemberProfilePicture.Image.Dispose();
                PicBxMemberProfilePicture.Image = null;
            }
            PicBxMemberProfilePicture.BackColor = Color.Gainsboro;

            GrpBxBookCheckout.Visible = false;
        }

        #endregion

        #region Book Checkout

        private void TxtAccessionNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                LookupBook();
            }
        }

        private void BtnEnterAccessionNumber_Click(object sender, EventArgs e)
        {
            LookupBook();
        }

        private void BtnScanAccessionNumber_Click(object sender, EventArgs e)
        {
            try
            {
                using (var cam = new Camera())
                {
                    var result = cam.ShowDialog(this);
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(cam.ScannedCode))
                    {
                        TxtAccessionNumber.Text = cam.ScannedCode.Trim();
                        LookupBook();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open camera: {ex.Message}", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LookupBook()
        {
            string accession = TxtAccessionNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(accession))
            {
                MessageBox.Show("Please enter an Accession Number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtAccessionNumber.Focus();
                return;
            }

            var bookInfo = _circulationManager.GetBookByAccession(accession);

            if (bookInfo == null)
            {
                MessageBox.Show($"Book copy not found: {accession}", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearBookResults();
                TxtAccessionNumber.Focus();
                TxtAccessionNumber.SelectAll();
                return;
            }

            _currentBook = bookInfo;
            DisplayBookInfo(bookInfo);
        }

        private void DisplayBookInfo(DTOCirculationBookInfo bookInfo)
        {
            // Title
            LblBookTitle.Text = $"Title: {bookInfo.Title}";

            // Resource Type label (safe)
            string resType = (bookInfo.ResourceType ?? string.Empty).Trim();
            string resourceLabel;
            if (string.IsNullOrWhiteSpace(resType))
            {
                resourceLabel = "N/A";
            }
            else
            {
                switch (resType.ToLowerInvariant())
                {
                    case "physicalbook":
                    case "physical":
                    case "book":
                        resourceLabel = "Book";
                        break;
                    case "ebook":
                    case "e-book":
                        resourceLabel = "E-Book";
                        break;
                    case "thesis":
                    case "theses":
                        resourceLabel = "Thesis";
                        break;
                    case "periodical":
                    case "periodicals":
                        resourceLabel = "Periodical";
                        break;
                    case "av":
                    case "audio-visual":
                    case "audio visual":
                        resourceLabel = "Audio-Visual";
                        break;
                    default:
                        resourceLabel = char.ToUpper(resType[0]) + resType.Substring(1);
                        break;
                }
            }

            if (LblResourceType != null)
            {
                LblResourceType.Text = $"Resource Type: {resourceLabel}";
                LblResourceType.ForeColor = Color.Black;
            }

            // Author - show only authors (role = "Author"); if empty show "N/A"
            string authors = string.IsNullOrWhiteSpace(bookInfo.Authors) ? "N/A" : bookInfo.Authors;
            string authorText = $"Author: {authors}";
            LblBookAuthor.Text = TruncateTextForLabel(authorText, LblBookAuthor, MaxAuthorLabelWidth);

            // Category
            LblBookCategory.Text = $"Category: {(!string.IsNullOrWhiteSpace(bookInfo.Category) ? bookInfo.Category : "N/A")}";

            // Determine resource kind flags
            bool isEBook = resourceLabel.Equals("E-Book", StringComparison.OrdinalIgnoreCase);
            bool isPhysicalBook = resourceLabel.Equals("Book", StringComparison.OrdinalIgnoreCase);
            bool isThesis = resourceLabel.Equals("Thesis", StringComparison.OrdinalIgnoreCase);
            bool isPeriodical = resourceLabel.Equals("Periodical", StringComparison.OrdinalIgnoreCase);
            bool isAV = resourceLabel.Equals("Audio-Visual", StringComparison.OrdinalIgnoreCase);
            bool isOtherNonBook = isThesis || isPeriodical || isAV;

            // Determine digital flag from DownloadURL or explicit EBook resource type
            bool isDigital = isEBook || !string.IsNullOrWhiteSpace(bookInfo.DownloadURL);

            // Loan Type display rules:
            string displayLoanType = string.IsNullOrWhiteSpace(bookInfo.LoanType) ? "Circulation" : bookInfo.LoanType;
            if (isEBook)
            {
                LblBookLoanType.Text = "Loan Type: Digital (no physical borrowing)";
                LblBookLoanType.ForeColor = Color.FromArgb(200, 0, 0);
            }
            else if (isPhysicalBook)
            {
                LblBookLoanType.Text = $"Loan Type: {displayLoanType}";
                LblBookLoanType.ForeColor = string.Equals(displayLoanType, "Circulation", StringComparison.OrdinalIgnoreCase)
                    ? Color.Black
                    : Color.FromArgb(200, 0, 0);
            }
            else if (isOtherNonBook)
            {
                LblBookLoanType.Text = "Loan Type: N/A";
                LblBookLoanType.ForeColor = Color.Black;
            }
            else
            {
                LblBookLoanType.Text = $"Loan Type: {displayLoanType}";
                LblBookLoanType.ForeColor = Color.Black;
            }

            // Status label: if loan type is Reference -> special message
            bool isReferenceLoan = string.Equals(displayLoanType, "Reference", StringComparison.OrdinalIgnoreCase);
            bool isAvailable = string.Equals(bookInfo.CopyStatus, "Available", StringComparison.OrdinalIgnoreCase);

            if (isReferenceLoan)
            {
                LblBookStatus.Text = "Status: In-Library Use";
                LblBookStatus.ForeColor = Color.FromArgb(200, 0, 0);
            }
            else
            {
                LblBookStatus.Text = $"Status: {(string.IsNullOrWhiteSpace(bookInfo.CopyStatus) ? "Unknown" : bookInfo.CopyStatus)}";
                LblBookStatus.ForeColor = isAvailable ? Color.FromArgb(0, 200, 0) : Color.FromArgb(200, 0, 0);
            }

            // Due Date rules:
            DateTime? showDue = null;
            if (_currentMember != null && isAvailable)
            {
                if (isPhysicalBook)
                {
                    if (string.Equals(displayLoanType, "Circulation", StringComparison.OrdinalIgnoreCase))
                        showDue = _currentMember.CalculateDueDate();
                }
                else if (isOtherNonBook && string.IsNullOrWhiteSpace(bookInfo.DownloadURL))
                {
                    showDue = _currentMember.CalculateDueDate();
                }
            }

            if (showDue.HasValue)
            {
                LblBookDueDate.Text = $"Due Date: {showDue.Value:MMMM d, yyyy}";
                LblBookDueDate.ForeColor = Color.FromArgb(175, 37, 50);
            }
            else
            {
                LblBookDueDate.Text = "Due Date: ";
                LblBookDueDate.ForeColor = Color.FromArgb(175, 37, 50);
            }

            // Update button state based on computed rules
            UpdateConfirmBorrowButton(bookInfo);
        }

        private void UpdateConfirmBorrowButton(DTOCirculationBookInfo bookInfo)
        {
            var resType = (bookInfo.ResourceType ?? string.Empty).Trim();
            bool isEBook = resType.Equals("EBook", StringComparison.OrdinalIgnoreCase);
            bool isPhysicalBook = resType.Equals("PhysicalBook", StringComparison.OrdinalIgnoreCase);
            bool isThesis = resType.Equals("Thesis", StringComparison.OrdinalIgnoreCase);
            bool isPeriodical = resType.Equals("Periodical", StringComparison.OrdinalIgnoreCase);
            bool isAV = resType.Equals("AV", StringComparison.OrdinalIgnoreCase);
            bool isOtherNonBook = isThesis || isPeriodical || isAV;

            bool isDigital = isEBook || !string.IsNullOrWhiteSpace(bookInfo.DownloadURL);
            bool isAvailable = string.Equals(bookInfo.CopyStatus, "Available", StringComparison.OrdinalIgnoreCase);

            // Default disabled state
            BtnConfirmBorrow.Enabled = false;
            BtnConfirmBorrow.BackColor = Color.Gray;

            // 1) E-Book (digital) -> never borrow
            if (isEBook)
            {
                BtnConfirmBorrow.Text = "Digital (No Borrow)";
                return;
            }

            // 2) Physical Book -> respect LoanType (must be Circulation) and availability
            if (isPhysicalBook)
            {
                var displayLoanType = string.IsNullOrWhiteSpace(bookInfo.LoanType) ? "Circulation" : bookInfo.LoanType;
                if (!string.Equals(displayLoanType, "Circulation", StringComparison.OrdinalIgnoreCase))
                {
                    BtnConfirmBorrow.Text = "Reference Only";
                    return;
                }

                if (!isAvailable)
                {
                    BtnConfirmBorrow.Text = "Not Available";
                    return;
                }

                // All good
                BtnConfirmBorrow.Enabled = true;
                BtnConfirmBorrow.Text = "Confirm Borrow";
                BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
                return;
            }

            // 3) Thesis / Periodical / AV
            if (isOtherNonBook)
            {
                // if digital -> cannot borrow
                if (isDigital)
                {
                    BtnConfirmBorrow.Text = "Digital (No Borrow)";
                    return;
                }

                // physical and available -> allow borrow
                if (isAvailable)
                {
                    BtnConfirmBorrow.Enabled = true;
                    BtnConfirmBorrow.Text = "Confirm Borrow";
                    BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
                    return;
                }

                BtnConfirmBorrow.Text = "Not Available";
                return;
            }

            // 4) Unknown resource type: be conservative — require availability and Circulation loan type if present
            var loan = string.IsNullOrWhiteSpace(bookInfo.LoanType) ? "Circulation" : bookInfo.LoanType;
            if (string.Equals(loan, "Circulation", StringComparison.OrdinalIgnoreCase) && isAvailable)
            {
                BtnConfirmBorrow.Enabled = true;
                BtnConfirmBorrow.Text = "Confirm Borrow";
                BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
            }
            else
            {
                BtnConfirmBorrow.Text = "Cannot Borrow";
            }
        }

        private string TruncateTextForLabel(string text, Label label, int maxWidth)
        {
            using (Graphics g = label.CreateGraphics())
            {
                SizeF size = g.MeasureString(text, label.Font);
                if (size.Width <= maxWidth)
                    return text;

                // Find last comma position for clean truncation
                string ellipsis = ", ...";
                int maxChars = text.Length;

                while (maxChars > 0)
                {
                    string truncated = text.Substring(0, maxChars) + ellipsis;
                    size = g.MeasureString(truncated, label.Font);
                    if (size.Width <= maxWidth)
                    {
                        // Try to end at a comma for cleaner look
                        int lastComma = text.LastIndexOf(',', maxChars - 1);
                        if (lastComma > 10)
                            return text.Substring(0, lastComma) + ", ...";
                        return truncated;
                    }
                    maxChars -= 5;
                }

                return text.Substring(0, Math.Min(20, text.Length)) + "...";
            }
        }

        private void BtnCancelBorrow_Click(object sender, EventArgs e)
        {
            ClearBookResults();
            TxtAccessionNumber.Text = "";
            TxtAccessionNumber.Focus();
        }

        private void BtnConfirmBorrow_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentMember == null)
                {
                    MessageBox.Show("Please verify a member first.", "No Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_currentBook == null)
                {
                    MessageBox.Show("Please lookup a book copy first.", "No Book", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ensure copy is borrowable according to UI rules
                if (!BtnConfirmBorrow.Enabled)
                {
                    MessageBox.Show("This copy cannot be borrowed.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int copyId = _currentBook.CopyID;
                int memberId = _currentMember.MemberID;

                DateTime borrowDate = DateTime.Now;
                DateTime dueDate = _currentMember.CalculateDueDate();

                int transId = _circulationManager.CreateBorrowingTransaction(copyId, memberId, borrowDate, dueDate);

                if (transId > 0)
                {
                    MessageBox.Show($"Borrow successful. Transaction ID: {transId}\nDue Date: {dueDate:MMMM d, yyyy}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Update UI to reflect new status
                    LblBookStatus.Text = "Status: Borrowed";
                    LblBookStatus.ForeColor = Color.FromArgb(200, 0, 0);
                    BtnConfirmBorrow.Enabled = false;
                    BtnConfirmBorrow.Text = "Borrowed";

                    // Update due date display
                    LblBookDueDate.Text = $"Due Date: {dueDate:MMMM d, yyyy}";
                    LblBookDueDate.ForeColor = Color.FromArgb(175, 37, 50);

                    // Show receipt dialog (uses the partial BorrowReceiptForm in Popup.Circulation)
                    try
                    {
                        using (var receipt = new LMS.Presentation.Popup.Circulation.BorrowReceiptForm(transId, _currentMember, _currentBook, borrowDate, dueDate))
                        {
                            receipt.ShowDialog(this);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Don't fail the borrow if receipt fails — surface a friendly message
                        MessageBox.Show($"Borrow recorded but failed to show receipt: {ex.Message}", "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to record borrowing transaction. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during borrow: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearBookResults()
        {
            _currentBook = null;

            LblBookTitle.Text = "Title: ";
            LblBookAuthor.Text = "Author: ";
            LblBookCategory.Text = "Category: ";
            LblBookLoanType.Text = "Loan Type: ";
            LblBookLoanType.ForeColor = Color.Black;
            LblBookStatus.Text = "Status: ";
            LblBookStatus.ForeColor = Color.Black;
            LblBookDueDate.Text = "Due Date: ";
            LblBookDueDate.ForeColor = Color.FromArgb(175, 37, 50);

            BtnConfirmBorrow.Enabled = false;
            BtnConfirmBorrow.Text = "Confirm Borrow";
            BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
        }

        #endregion

        #region Book Return

        private void TxtReturnAccessionNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                LookupReturn();
            }
        }

        private void BtnReturnEnterAccessionNumber_Click(object sender, EventArgs e)
        {
            LookupReturn();
        }

        private void BtnReturnScanAccessionNumber_Click(object sender, EventArgs e)
        {
            try
            {
                using (var cam = new Camera())
                {
                    var result = cam.ShowDialog(this);
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(cam.ScannedCode))
                    {
                        TxtReturnAccessionNumber.Text = cam.ScannedCode.Trim();
                        LookupReturn();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open camera: {ex.Message}", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelReturn_Click(object sender, EventArgs e)
        {
            ClearReturnResults();
            TxtReturnAccessionNumber.Text = "";
            TxtReturnAccessionNumber.Focus();
        }

        private void LookupReturn()
        {
            string accession = TxtReturnAccessionNumber.Text?.Trim();
            if (string.IsNullOrWhiteSpace(accession))
            {
                MessageBox.Show("Please enter an Accession Number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtReturnAccessionNumber.Focus();
                return;
            }

            var info = _circulationManager.GetActiveBorrowingByAccession(accession);
            if (info == null)
            {
                MessageBox.Show($"No active borrowing found for accession: {accession}", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearReturnResults();
                TxtReturnAccessionNumber.Focus();
                TxtReturnAccessionNumber.SelectAll();
                return;
            }

            // Populate UI
            LblReturnTitle.Text = $"Title: {(!string.IsNullOrWhiteSpace(info.Title) ? info.Title : "N/A")}";
            LblReturnBorrowDate.Text = $"Borrow Date: {info.BorrowDate:MMMM d, yyyy}";
            LblReturnReturnDate.Text = $"Return Date: {DateTime.Today:MMMM d, yyyy}";

            // NEW: show borrower name
            LblReturnBorrower.Text = $"Borrower: {(!string.IsNullOrWhiteSpace(info.MemberName) ? info.MemberName : "Unknown")}";
            LblReturnBorrower.ForeColor = Color.Black;

            if (info.DaysOverdue > 0)
            {
                LblReturnStatus.Text = "Status: Overdue";
                LblReturnStatus.ForeColor = Color.FromArgb(200, 0, 0);
                LblReturnFine.Text = $"Fine: ₱{info.FineAmount:N2}";
            }
            else
            {
                LblReturnStatus.Text = "Status: On time";
                LblReturnStatus.ForeColor = Color.FromArgb(0, 200, 0);
                LblReturnFine.Text = "Fine: ₱0.00";
            }

            // Keep ConfirmReturn disabled per your instruction (don't make it work yet)
            BtnConfirmReturn.Enabled = false;
        }

        private void ClearReturnResults()
        {
            LblReturnTitle.Text = "Title: ";
            LblReturnBorrowDate.Text = "Borrow Date: ";
            LblReturnReturnDate.Text = "Return Date: ";
            LblReturnStatus.Text = "Status: ";
            LblReturnStatus.ForeColor = Color.Black;
            LblReturnFine.Text = "Fine: ";
            BtnConfirmReturn.Enabled = false;

            // Reset borrower label
            LblReturnBorrower.Text = "Borrower: ";
            LblReturnBorrower.ForeColor = Color.Black;
        }

        #endregion
    }
}
