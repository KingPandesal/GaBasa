using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;
using LMS.Presentation.Popup.Multipurpose;
using LMS.Model.Models.Transactions;
using LMS.BusinessLogic.Services.Audit;
using LMS.DataAccess.Database;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCCirculation : UserControl
    {
        private readonly ICirculationManager _circulationManager;
        private readonly ReservationRepository _reservationRepository;
        private readonly IAuditLogService _auditLogService;
        private DTOCirculationMemberInfo _currentMember;
        private DTOCirculationBookInfo _currentBook;

        // NEW: hold currently looked-up return info so calculations can reuse it
        private DTOReturnInfo _currentReturn;

        // Add this field at the top of the class (with other fields):
        private DTORenewalInfo _currentRenewal;

        // Max width for author label before truncating
        private const int MaxAuthorLabelWidth = 650;

        public UCCirculation()
        {
            InitializeComponent();

            // Setup dependencies
            var circulationRepo = new CirculationRepository();
            _circulationManager = new CirculationManager(circulationRepo);
            _reservationRepository = new ReservationRepository();

            // Initialize audit log service
            var dbConn = new DbConnection();
            var auditLogRepo = new AuditLogRepository(dbConn);
            _auditLogService = new AuditLogService(auditLogRepo);

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

            // Wire confirm return button (now implemented)
            BtnConfirmReturn.Click -= BtnConfirmReturn_Click;
            BtnConfirmReturn.Click += BtnConfirmReturn_Click;

            TxtRenewAccessionNumber.KeyDown += TxtRenewAccessionNumber_KeyDown;
            BtnRenewEnterAccessionNumber.Click += BtnRenewEnterAccessionNumber_Click;
            BtnRenewScanAccessionNumber.Click += BtnRenewScanAccessionNumber_Click;

            // NEW: wire condition and penalty inputs (designer has these controls)
            CmbBxReturnCondition.SelectedIndexChanged += CmbBxReturnCondition_SelectedIndexChanged;
            NumPckReturnReplacementCost.ValueChanged += NumPckReturnReplacementCost_ValueChanged;

            // Ensure numeric control has sane defaults for penalty entry
            // Require minimum of 1 (no 0)
            NumPckReturnReplacementCost.Minimum = 1;
            NumPckReturnReplacementCost.DecimalPlaces = 2;
            NumPckReturnReplacementCost.ThousandsSeparator = true;
            // Limit to maximum of 10 digits (9,999,999,999)
            NumPckReturnReplacementCost.Maximum = 9999999999m;
            NumPckReturnReplacementCost.Enabled = false;

            // Initialize UI state
            ClearMemberResults();
            ClearBookResults();

            // Hide book checkout section initially
            GrpBxBookCheckout.Visible = false;

            // Ensure penalty panel visibility reflects default condition
            UpdatePenaltyPanelVisibility();
            LblReturnTotalFine.Text = "Total Fine: ₱0.00";

            // Initialize renewal UI
            ClearRenewalResults();

            // Wire cancel renew button
            BtnCancelRenew.Click += BtnCancelRenew_Click;

            // Wire confirm renew button
            BtnConfirmRenew.Click -= BtnConfirmRenew_Click;
            BtnConfirmRenew.Click += BtnConfirmRenew_Click;
        }

        private void BtnConfirmRenew_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentRenewal == null)
                {
                    MessageBox.Show("Please lookup a borrowing transaction first.", "No Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!BtnConfirmRenew.Enabled)
                {
                    MessageBox.Show("Renewal not allowed for this transaction.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Disable button to avoid duplicate clicks
                BtnConfirmRenew.Enabled = false;

                // Attempt to renew via manager. Manager should expose RenewBorrowingTransaction(transactionId, out newDueDate)
                DateTime newDueDate;
                bool success = false;

                try
                {
                    success = _circulationManager.RenewBorrowingTransaction(_currentRenewal.TransactionID, out newDueDate);
                }
                catch (MissingMethodException)
                {
                    MessageBox.Show("Renewal operation is not implemented in the manager. Add RenewBorrowingTransaction to ICirculationManager and implement it.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (success)
                {
                    // Non-fatal audit log of renewal
                    try
                    {
                        var memberName = _currentRenewal?.MemberName ?? _currentMember?.FullName ?? "Unknown";
                        var bookTitle = _currentRenewal?.Title ?? "Unknown";
                        _auditLogService?.LogRenewBook(Program.CurrentUserId, memberName, bookTitle);
                    }
                    catch
                    {
                        // Do not block user flow if audit fails
                    }

                    MessageBox.Show($"Renewal successful. New Due Date: {newDueDate:MMMM d, yyyy}", "Renewed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // After successful transaction, clear UI fields as requested
                    ClearRenewalResults();
                    TxtRenewAccessionNumber.Text = "";
                    TxtRenewAccessionNumber.Focus();
                }
                else
                {
                    MessageBox.Show("Failed to renew. The renewal limit may have been reached or the book has active reservations.", "Renewal Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Reload to refresh UI state in case server-side values changed
                    try
                    {
                        string accession = TxtRenewAccessionNumber.Text?.Trim();
                        if (!string.IsNullOrWhiteSpace(accession))
                            LookupRenewal();
                    }
                    catch
                    {
                        // ignore reload errors
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during renewal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        /// <summary>
        /// Penalty level:
        /// 0 => good (no fines, no overdues)
        /// 1 => has fine OR has overdue (minus 1 to privileges)
        /// 2 => has BOTH fine AND overdue (minus 2 to privileges)
        /// </summary>
        private int GetPenaltyLevel(DTOCirculationMemberInfo member)
        {
            if (member == null) return 0;
            bool hasFines = member.TotalUnpaidFines > 0m;
            bool hasOverdues = member.OverdueCount > 0;
            if (hasFines && hasOverdues) return 2;
            if (hasFines || hasOverdues) return 1;
            return 0;
        }

        private int EffectiveMaxBooksAllowed(DTOCirculationMemberInfo member)
        {
            if (member == null) return 0;
            int penalty = GetPenaltyLevel(member);
            return Math.Max(0, member.MaxBooksAllowed - penalty);
        }

        private int EffectiveBorrowingPeriod(DTOCirculationMemberInfo member)
        {
            if (member == null) return 1;
            int penalty = GetPenaltyLevel(member);
            return Math.Max(1, member.BorrowingPeriod - penalty);
        }

        private int EffectiveRenewalLimit(DTOCirculationMemberInfo member)
        {
            if (member == null) return 0;
            int penalty = GetPenaltyLevel(member);
            return Math.Max(0, member.RenewalLimit - penalty);
        }

        private bool EffectiveReservationPrivilege(DTOCirculationMemberInfo member)
        {
            if (member == null) return false;
            // any penalty disables reservation
            return member.ReservationPrivilege && GetPenaltyLevel(member) == 0;
        }

        /// <summary>
        /// Borrow eligibility using effective values (respects status, overdues, effective limits and fine cap).
        /// </summary>
        private bool CanBorrowWithPenalty(DTOCirculationMemberInfo member)
        {
            if (member == null) return false;
            if (!string.Equals(member.Status, "Active", StringComparison.OrdinalIgnoreCase)) return false;

            // No overdue allowed (policy)
            if (member.OverdueCount > 0) return false;

            // Borrow limit check uses effective max books
            if (member.CurrentBorrowedCount >= EffectiveMaxBooksAllowed(member)) return false;

            // Fine cap: if MaxFineCap is > 0 then unpaid fines must be below cap
            if (member.MaxFineCap > 0m && member.TotalUnpaidFines >= member.MaxFineCap) return false;

            return true;
        }

        /// <summary>
        /// Calculate due date for a member using effective borrowing period.
        /// </summary>
        private DateTime CalculateDueDateForMember(DTOCirculationMemberInfo member)
        {
            int days = member != null ? EffectiveBorrowingPeriod(member) : 14;
            return DateTime.Today.AddDays(days);
        }

        private void DisplayBorrowingStatistics(DTOCirculationMemberInfo memberInfo)
        {
            // Show current / effective limit
            LblBorrowed.Text = $"Borrowed: {memberInfo.CurrentBorrowedCount} / {EffectiveMaxBooksAllowed(memberInfo)}";
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

            if (memberInfo.OverdueCount == 0)
            {
                LblNoOverdueBooks.Text = "✔️ No Overdue Books";
                LblNoOverdueBooks.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblNoOverdueBooks.Text = $"❌ Has {memberInfo.OverdueCount} Overdue Book(s)";
                LblNoOverdueBooks.ForeColor = Color.FromArgb(200, 0, 0);
            }

            // Borrow limit uses effective max books after penalty
            if (memberInfo.CurrentBorrowedCount < EffectiveMaxBooksAllowed(memberInfo))
            {
                LblBorrowLimitOK.Text = "✔️ Borrow Limit OK";
                LblBorrowLimitOK.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblBorrowLimitOK.Text = "❌ Borrow Limit Reached";
                LblBorrowLimitOK.ForeColor = Color.FromArgb(200, 0, 0);
            }

            // Renewal limit - show effective renewal limit on the renewal label used by the renewal UI
            // (LblRenewRenewalLimit is the label that shows "✔️ Renewal limit not reached (1 / 3)")
            if (LblRenewRenewalLimit != null)
            {
                LblRenewRenewalLimit.Text = $"✔️ Renewal limit not reached (0 / {EffectiveRenewalLimit(memberInfo)})";
                LblRenewRenewalLimit.ForeColor = Color.FromArgb(0, 200, 0);
            }

            // Reservation privilege - disabled if any penalty
            //if (EffectiveReservationPrivilege(memberInfo))
            //{
            //    LblReservationPrivilege.Text = "✔️ Reservation Allowed";
            //    LblReservationPrivilege.ForeColor = Color.FromArgb(0, 200, 0);
            //}
            //else
            //{
            //    LblReservationPrivilege.Text = "❌ Cannot Reserve";
            //    LblReservationPrivilege.ForeColor = Color.FromArgb(200, 0, 0);
            //}

            string capText = $"₱{memberInfo.MaxFineCap:N2}";
            if (memberInfo.MaxFineCap <= 0 || memberInfo.TotalUnpaidFines < memberInfo.MaxFineCap)
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
            GrpBxBookCheckout.Visible = CanBorrowWithPenalty(memberInfo);
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

            // --- Reservation: find first-in-queue reservation for this book/copy (if any) ---
            Reservation firstReservation = null;
            try
            {
                var reservationManager = new ReservationManager();
                firstReservation = reservationManager.GetFirstReservationForCopy(bookInfo.CopyID);
            }
            catch
            {
                firstReservation = null;
            }

            // Status label: if loan type is Reference -> special message
            bool isReferenceLoan = string.Equals(displayLoanType, "Reference", StringComparison.OrdinalIgnoreCase);
            bool isAvailable = string.Equals(bookInfo.CopyStatus, "Available", StringComparison.OrdinalIgnoreCase);

            if (isReferenceLoan)
            {
                LblBookStatus.Text = "Status: In-Library Use";
                LblBookStatus.ForeColor = Color.FromArgb(200, 0, 0);
            }
            else if (firstReservation != null)
            {
                // Show Reserved status if copy has a queued active reservation (visual only)
                LblBookStatus.Text = "Status: Reserved";
                LblBookStatus.ForeColor = Color.FromArgb(200, 150, 0); // Orange/amber color for reserved
            }
            else
            {
                LblBookStatus.Text = $"Status: {(string.IsNullOrWhiteSpace(bookInfo.CopyStatus) ? "Unknown" : bookInfo.CopyStatus)}";
                LblBookStatus.ForeColor = isAvailable ? Color.FromArgb(0, 200, 0) : Color.FromArgb(200, 0, 0);
            }

            // Due Date rules:
            DateTime? showDue = null;
            if (_currentMember != null && isAvailable && firstReservation == null)
            {
                if (isPhysicalBook)
                {
                    if (string.Equals(displayLoanType, "Circulation", StringComparison.OrdinalIgnoreCase))
                        showDue = CalculateDueDateForMember(_currentMember);
                }
                else if (isOtherNonBook && string.IsNullOrWhiteSpace(bookInfo.DownloadURL))
                {
                    showDue = CalculateDueDateForMember(_currentMember);
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

            // Update button state based on computed rules (pass the first reservation if any)
            UpdateConfirmBorrowButton(bookInfo, firstReservation);
        }

        private void UpdateConfirmBorrowButton(DTOCirculationBookInfo bookInfo, Reservation firstReservation = null)
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

            // If current member exists but is not eligible due to fines/overdues/status, prevent borrowing
            if (_currentMember != null && !CanBorrowWithPenalty(_currentMember))
            {
                BtnConfirmBorrow.Text = "Not Eligible";
                BtnConfirmBorrow.Enabled = false;
                BtnConfirmBorrow.BackColor = Color.Gray;
                return;
            }

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

                // If not available at all, cannot borrow
                if (!isAvailable)
                {
                    BtnConfirmBorrow.Text = "Not Available";
                    return;
                }

                // If there's a reservation queue (firstReservation != null) enforce queue rules:
                if (firstReservation != null)
                {
                    // If current member is the first in queue
                    if (_currentMember != null && firstReservation.MemberID == _currentMember.MemberID)
                    {
                        // Allow only if reservation is active (ExpirationDate set and not expired)
                        if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
                        {
                            BtnConfirmBorrow.Enabled = true;
                            BtnConfirmBorrow.Text = "Confirm Borrow";
                            BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
                            return;
                        }
                        else
                        {
                            BtnConfirmBorrow.Text = "Reservation Not Ready";
                            BtnConfirmBorrow.Enabled = false;
                            BtnConfirmBorrow.BackColor = Color.Gray;
                            return;
                        }
                    }
                    else
                    {
                        // Another member is first in queue:
                        // If that reservation is active (has ExpirationDate and not expired) block borrowing.
                        if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
                        {
                            BtnConfirmBorrow.Text = "Reserved";
                            BtnConfirmBorrow.Enabled = false;
                            BtnConfirmBorrow.BackColor = Color.Gray;
                            return;
                        }
                        // If first reservation has no ExpirationDate (not yet activated) or has expired,
                        // allow borrowing (fallback). The activation/expiration job will reconcile states.
                    }
                }

                // No blocking reservation -> allow borrow
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

                // physical and available -> allow borrow (member eligibility checked earlier)
                if (isAvailable)
                {
                    // If there's a reservation queue, apply same queue rules as for physical books
                    if (firstReservation != null)
                    {
                        if (_currentMember != null && firstReservation.MemberID == _currentMember.MemberID)
                        {
                            if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
                            {
                                BtnConfirmBorrow.Enabled = true;
                                BtnConfirmBorrow.Text = "Confirm Borrow";
                                BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
                                return;
                            }
                            else
                            {
                                BtnConfirmBorrow.Text = "Reservation Not Ready";
                                BtnConfirmBorrow.Enabled = false;
                                BtnConfirmBorrow.BackColor = Color.Gray;
                                return;
                            }
                        }
                        else
                        {
                            if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
                            {
                                BtnConfirmBorrow.Text = "Reserved";
                                BtnConfirmBorrow.Enabled = false;
                                BtnConfirmBorrow.BackColor = Color.Gray;
                                return;
                            }
                            // else allow
                        }
                    }

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
                // Apply reservation queue rules if needed (reuse firstReservation logic)
                if (firstReservation != null)
                {
                    if (_currentMember != null && firstReservation.MemberID == _currentMember.MemberID)
                    {
                        if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
                        {
                            BtnConfirmBorrow.Enabled = true;
                            BtnConfirmBorrow.Text = "Confirm Borrow";
                            BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
                        }
                        else
                        {
                            BtnConfirmBorrow.Text = "Reservation Not Ready";
                            BtnConfirmBorrow.Enabled = false;
                            BtnConfirmBorrow.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
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
                }
                else
                {
                    BtnConfirmBorrow.Enabled = true;
                    BtnConfirmBorrow.Text = "Confirm Borrow";
                    BtnConfirmBorrow.BackColor = Color.FromArgb(175, 37, 50);
                }
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

                // Reservation priority check:
                // Only the member who is next in queue AND has an active (non-expired) reservation
                // may borrow. Others must wait until that reservation expires or is completed.
                var reservationManager = new LMS.BusinessLogic.Managers.ReservationManager();
                var firstReservation = reservationManager.GetFirstReservationForCopy(copyId);

                if (firstReservation != null)
                {
                    // If the first-in-queue is this member
                    if (firstReservation.MemberID == memberId)
                    {
                        // They may borrow only if their reservation is active (ExpirationDate set and not expired)
                        if (!firstReservation.ExpirationDate.HasValue || firstReservation.ExpirationDate.Value < DateTime.Now)
                        {
                            MessageBox.Show(
                                "Your reservation is not ready for pickup or has expired. You cannot borrow this copy now.",
                                "Reservation Not Active",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;
                        }
                        // otherwise allowed to continue and borrow
                    }
                    else
                    {
                        // Another member is first in queue
                        // If that reservation is active (expiration set and not expired) block borrowing
                        if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value >= DateTime.Now)
                        {
                            MessageBox.Show(
                                "This book is reserved for another member who has priority. The reserved member must pick up the book or let the reservation expire.",
                                "Reserved",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                        // If the first reservation has no ExpirationDate (not yet activated) or has expired,
                        // allow borrowing to proceed here (the expire job / activation flow should handle state cleanup).
                    }
                }

                DateTime borrowDate = DateTime.Now;
                DateTime dueDate = CalculateDueDateForMember(_currentMember);

                int transId = _circulationManager.CreateBorrowingTransaction(copyId, memberId, borrowDate, dueDate);

                if (transId > 0)
                {
                    // Complete any active reservation for this member on this book
                    try
                    {
                        // Re-fetch firstReservation to be safe (it may have changed) and complete only if it belongs to this member
                        var maybeFirst = reservationManager.GetFirstReservationForCopy(copyId);
                        if (maybeFirst != null && maybeFirst.MemberID == memberId)
                        {
                            reservationManager.CompleteReservation(maybeFirst.ReservationID);
                        }
                    }
                    catch
                    {
                        // Don't fail the borrow if reservation completion fails
                    }

                    // Log the borrow approval action to audit log (non-fatal)
                    try
                    {
                        var memberName = _currentMember?.FullName ?? "Unknown";
                        var bookTitle = _currentBook?.Title ?? "Unknown";
                        _auditLogService?.LogApproveBorrowBook(Program.CurrentUserId, memberName, bookTitle);
                    }
                    catch
                    {
                        // swallow - audit logging must not block the user flow
                    }

                    MessageBox.Show($"Borrow successful. Transaction ID: {transId}\nDue Date: {dueDate:MMMM d, yyyy}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Update UI to reflect new status
                    LblBookStatus.Text = "Status: Borrowed";
                    LblBookStatus.ForeColor = Color.FromArgb(200, 0, 0);
                    BtnConfirmBorrow.Enabled = false;
                    BtnConfirmBorrow.Text = "Borrowed";

                    // Update due date display
                    LblBookDueDate.Text = $"Due Date: {dueDate:MMMM d, yyyy}";
                    LblBookDueDate.ForeColor = Color.FromArgb(175, 37, 50);

                    // Show receipt dialog
                    try
                    {
                        using (var receipt = new LMS.Presentation.Popup.Circulation.BorrowReceiptForm(transId, _currentMember, _currentBook, borrowDate, dueDate))
                        {
                            receipt.ShowDialog(this);
                        }
                    }
                    catch (Exception ex)
                    {
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

            // Save for later calculations
            _currentReturn = info;

            // Populate UI
            LblReturnTitle.Text = $"Title: {(!string.IsNullOrWhiteSpace(info.Title) ? info.Title : "N/A")}";
            LblReturnBorrowDate.Text = $"Borrow Date: {info.BorrowDate:MMMM d, yyyy}";
            LblReturnReturnDate.Text = $"Return Date: {DateTime.Today:MMMM d, yyyy}";

            // NEW: show borrower name
            LblReturnBorrower.Text = $"Borrower: {(!string.IsNullOrWhiteSpace(info.MemberName) ? info.MemberName : "Unknown")}";
            LblReturnBorrower.ForeColor = Color.Black;

            // Fine label uses info.FineAmount (DTOReturnInfo.FineAmount is derived by manager)
            if (info.DaysOverdue > 0)
            {
                LblReturnStatus.Text = "Status: Overdue";
                LblReturnStatus.ForeColor = Color.FromArgb(200, 0, 0);
            }
            else
            {
                LblReturnStatus.Text = "Status: On time";
                LblReturnStatus.ForeColor = Color.FromArgb(0, 200, 0);
            }

            // Ensure penalty panel visibility reflects condition
            UpdatePenaltyPanelVisibility();

            // Recalculate and display fine and total
            RecalculateReturnTotals();

            // Enable confirm for actionable conditions: Good, Damage/Damaged, Lost
            var cond = CmbBxReturnCondition.Text?.Trim();
            bool actionable = string.Equals(cond, "Good", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(cond, "Damage", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(cond, "Damaged", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(cond, "Lost", StringComparison.OrdinalIgnoreCase);

            BtnConfirmReturn.Enabled = actionable;
        }

        private void ClearReturnResults()
        {
            _currentReturn = null;

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

            // Reset penalty UI
            // set to Minimum (1) to respect new requirement (no zero)
            try
            {
                NumPckReturnReplacementCost.Value = NumPckReturnReplacementCost.Minimum;
            }
            catch
            {
                // ignore if designer not yet initialized
            }
            UpdatePenaltyPanelVisibility();
            LblReturnTotalFine.Text = "Total Fine: ₱0.00";
        }

        #endregion

        #region Book Renewal

        private void TxtRenewAccessionNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                LookupRenewal();
            }
        }

        private void BtnRenewEnterAccessionNumber_Click(object sender, EventArgs e)
        {
            LookupRenewal();
        }

        private void BtnRenewScanAccessionNumber_Click(object sender, EventArgs e)
        {
            try
            {
                using (var cam = new Camera())
                {
                    var result = cam.ShowDialog(this);
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(cam.ScannedCode))
                    {
                        TxtRenewAccessionNumber.Text = cam.ScannedCode.Trim();
                        LookupRenewal();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open camera: {ex.Message}", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelRenew_Click(object sender, EventArgs e)
        {
            ClearRenewalResults();
            TxtRenewAccessionNumber.Text = "";
            TxtRenewAccessionNumber.Focus();
        }

        private void LookupRenewal()
        {
            string accession = TxtRenewAccessionNumber.Text?.Trim();
            if (string.IsNullOrWhiteSpace(accession))
            {
                MessageBox.Show("Please enter an Accession Number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtRenewAccessionNumber.Focus();
                return;
            }

            var info = _circulationManager.GetRenewalInfoByAccession(accession);
            if (info == null)
            {
                MessageBox.Show($"No active borrowing found for accession: {accession}", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearRenewalResults();
                TxtRenewAccessionNumber.Focus();
                TxtRenewAccessionNumber.SelectAll();
                return;
            }

            _currentRenewal = info;
            DisplayRenewalInfo(info);
        }

        private void DisplayRenewalInfo(DTORenewalInfo info)
        {
            // 1. Title
            LblRenewTitle.Text = $"Title: {(!string.IsNullOrWhiteSpace(info.Title) ? info.Title : "N/A")}";

            // 2. Borrower
            LblRenewBorrower.Text = $"Borrower: {(!string.IsNullOrWhiteSpace(info.MemberName) ? info.MemberName : "Unknown")}";

            // 3. Old Due Date
            LblRenewOldDueDate.Text = $"Old Due Date: {info.DueDate:MMMM d, yyyy}";

            // 4. Active Reservation Check
            if (!info.HasActiveReservation)
            {
                LblRenewActiveReservation.Text = "✔️ Book has no active reservations";
                LblRenewActiveReservation.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblRenewActiveReservation.Text = "❌ Book has active reservation(s)";
                LblRenewActiveReservation.ForeColor = Color.FromArgb(200, 0, 0);
            }

            // 5. Renewal Limit Check
            if (info.IsWithinRenewalLimit)
            {
                LblRenewRenewalLimit.Text = $"✔️ Renewal limit not reached ({info.RenewalCount} / {info.MaxRenewals})";
                LblRenewRenewalLimit.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblRenewRenewalLimit.Text = $"❌ Renewal limit reached ({info.RenewalCount} / {info.MaxRenewals})";
                LblRenewRenewalLimit.ForeColor = Color.FromArgb(200, 0, 0);
            }

            // 6. Renewal Status (both checks must pass)
            bool canRenew = info.CanRenew;
            if (canRenew)
            {
                LblRenewRenewalStatus.Text = "Renewal Status: Allowed";
                LblRenewRenewalStatus.ForeColor = Color.FromArgb(0, 200, 0);
            }
            else
            {
                LblRenewRenewalStatus.Text = "Renewal Status: Not Allowed";
                LblRenewRenewalStatus.ForeColor = Color.FromArgb(200, 0, 0);
            }

            // 7. New Due Date - only show if renewal is allowed
            if (canRenew)
            {
                DateTime newDueDate = info.CalculateNewDueDate();
                LblRenewNewDueDate.Text = $"New Due Date: {newDueDate:MMMM d, yyyy}";
                LblRenewNewDueDate.Visible = true;
            }
            else
            {
                LblRenewNewDueDate.Text = "New Due Date: ";
                LblRenewNewDueDate.Visible = false;
            }

            // Enable/disable confirm button based on eligibility
            BtnConfirmRenew.Enabled = canRenew;
            BtnConfirmRenew.BackColor = canRenew ? Color.FromArgb(175, 37, 50) : Color.Gray;
        }

        private void ClearRenewalResults()
        {
            _currentRenewal = null;

            LblRenewTitle.Text = "Title: ";
            LblRenewBorrower.Text = "Borrower: ";
            LblRenewOldDueDate.Text = "Old Due Date: ";
            LblRenewActiveReservation.Text = "";
            LblRenewActiveReservation.ForeColor = Color.Black;
            LblRenewRenewalLimit.Text = "";
            LblRenewRenewalLimit.ForeColor = Color.Black;
            LblRenewRenewalStatus.Text = "Renewal Status: ";
            LblRenewRenewalStatus.ForeColor = Color.Black;

            // NEW: only show NewDueDate when renewal is Allowed
            LblRenewNewDueDate.Text = "New Due Date: ";
            LblRenewNewDueDate.Visible = false;

            BtnConfirmRenew.Enabled = false;
            BtnConfirmRenew.BackColor = Color.FromArgb(175, 37, 50);
        }

        #endregion

        // NEW: event handlers and helpers for return condition / penalty calculations

        private void CmbBxReturnCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePenaltyPanelVisibility();
            RecalculateReturnTotals();

            // When user switches condition while a return is loaded, re-evaluate enable state
            if (_currentReturn != null)
            {
                var cond = CmbBxReturnCondition.Text?.Trim();
                bool actionable = string.Equals(cond, "Good", StringComparison.OrdinalIgnoreCase)
                                  || string.Equals(cond, "Damage", StringComparison.OrdinalIgnoreCase)
                                  || string.Equals(cond, "Damaged", StringComparison.OrdinalIgnoreCase)
                                  || string.Equals(cond, "Lost", StringComparison.OrdinalIgnoreCase);

                BtnConfirmReturn.Enabled = actionable;
            }
        }

        private void NumPckReturnReplacementCost_ValueChanged(object sender, EventArgs e)
        {
            RecalculateReturnTotals();
        }

        private void UpdatePenaltyPanelVisibility()
        {
            var cond = CmbBxReturnCondition.Text?.Trim();

            // Show penalty panel for both "Damage" and "Lost"
            bool showPenalty = string.Equals(cond, "Damage", StringComparison.OrdinalIgnoreCase)
                               || string.Equals(cond, "Lost", StringComparison.OrdinalIgnoreCase);

            PnlforReturnPenaltyFee.Visible = showPenalty;
            NumPckReturnReplacementCost.Enabled = showPenalty;

            // If enabling penalty, ensure value respects new minimum (1)
            if (showPenalty)
            {
                try
                {
                    if (NumPckReturnReplacementCost.Value < NumPckReturnReplacementCost.Minimum)
                        NumPckReturnReplacementCost.Value = NumPckReturnReplacementCost.Minimum;
                }
                catch
                {
                    // ignore control errors
                }
            }

            // If penalty is required, make it obvious to the user by focusing the control
            if (showPenalty && _currentReturn != null)
            {
                // only focus when a return is loaded
                try
                {
                    NumPckReturnReplacementCost.Focus();
                }
                catch
                {
                    // ignore focus errors
                }
            }
        }

        /// <summary>
        /// Recalculates LblReturnFine and LblReturnTotalFine.
        /// - Fine is taken from _currentReturn.FineAmount (manager-provided)
        /// - Penalty (when Damage or Lost) is taken from NumPckReturnReplacementCost.Value
        /// </summary>
        private void RecalculateReturnTotals()
        {
            decimal fine = 0m;
            if (_currentReturn != null)
            {
                fine = _currentReturn.FineAmount;
            }

            decimal penalty = 0m;
            if (PnlforReturnPenaltyFee.Visible)
            {
                penalty = NumPckReturnReplacementCost.Value;
            }

            // Update displayed labels
            LblReturnFine.Text = $"Fine: ₱{fine:N2}";
            LblReturnTotalFine.Text = $"Total Fine: ₱{(fine + penalty):N2}";
        }

        /// <summary>
        /// Confirm return button handler — implements "Good" condition behavior:
        /// - Marks transaction Returned and sets ReturnDate
        /// - Marks BookCopy Available
        /// - If overdue fine exists, inserts Fine row (handled in repository)
        /// </summary>
        private void BtnConfirmReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentReturn == null)
                {
                    MessageBox.Show("Please lookup a borrowing transaction first.", "No Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cond = CmbBxReturnCondition.Text?.Trim();
                if (string.IsNullOrWhiteSpace(cond))
                {
                    MessageBox.Show("Please select a return condition.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // compute penalty (replacement cost) only when penalty panel is visible
                decimal penalty = 0m;
                if (PnlforReturnPenaltyFee.Visible)
                    penalty = NumPckReturnReplacementCost.Value;

                // overdue fine (from manager-provided DTO)
                decimal overdueFine = _currentReturn?.FineAmount ?? 0m;

                // total fine to be recorded (overdue + replacement/penalty)
                decimal totalFine = overdueFine + penalty;

                DateTime returnDate = DateTime.Now;

                // store for receipt before clearing
                var returnInfoForReceipt = _currentReturn;
                int transactionId = _currentReturn.TransactionID;

                bool success = false;

                if (string.Equals(cond, "Good", StringComparison.OrdinalIgnoreCase))
                {
                    // existing good flow
                    success = _circulationManager.CompleteReturnGood(
                        _currentReturn.TransactionID,
                        _currentReturn.CopyID,
                        _currentReturn.MemberID,
                        returnDate,
                        overdueFine);
                }
                else if (string.Equals(cond, "Damage", StringComparison.OrdinalIgnoreCase)
                         || string.Equals(cond, "Damaged", StringComparison.OrdinalIgnoreCase)
                         || string.Equals(cond, "Lost", StringComparison.OrdinalIgnoreCase))
                {
                    // Lost/Damaged flow: record BookCopy status accordingly and create fine if totalFine > 0
                    success = _circulationManager.CompleteReturnWithCondition(
                        _currentReturn.TransactionID,
                        _currentReturn.CopyID,
                        _currentReturn.MemberID,
                        returnDate,
                        totalFine,
                        cond);
                }
                else
                {
                    MessageBox.Show("Condition not supported by this workflow.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (success)
                {
                    // Update UI to reflect return (show date + time)
                    LblReturnStatus.Text = "Status: Returned";
                    LblReturnStatus.ForeColor = Color.FromArgb(0, 200, 0);
                    LblReturnReturnDate.Text = $"Return Date: {returnDate:MMMM d, yyyy h:mm tt}";
                    LblReturnFine.Text = $"Fine: ₱{overdueFine:N2}";
                    LblReturnTotalFine.Text = $"Total Fine: ₱{totalFine:N2}";
                    BtnConfirmReturn.Enabled = false;

                    if (totalFine > 0m)
                    {
                        MessageBox.Show($"Return processed. Fine recorded (Unpaid): ₱{totalFine:N2}", "Returned with Fine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Return processed successfully.", "Returned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Log the return action to audit log (non-fatal)
                    try
                    {
                        var memberName = returnInfoForReceipt?.MemberName ?? _currentMember?.FullName ?? "Unknown";
                        var bookTitle = returnInfoForReceipt?.Title ?? _currentReturn?.Title ?? "Unknown";
                        _auditLogService?.LogReturnBook(Program.CurrentUserId, memberName, bookTitle);
                    }
                    catch
                    {
                        // Non-fatal: audit logging failed, continue normal flow
                    }

                    // Clear active return
                    _currentReturn = null;

                    // Show return receipt (includes condition and total fine)
                    try
                    {
                        using (var receipt = new LMS.Presentation.Popup.Circulation.ReturnReceiptForm(
                            transactionId,
                            returnInfoForReceipt,
                            returnDate,
                            totalFine,
                            cond))
                        {
                            receipt.ShowDialog(this);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Return recorded but failed to show receipt: {ex.Message}", "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to process the return. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing return: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LblReturnCondition_Click(object sender, EventArgs e)
        {

        }

        private void BtnEnterMemberID_Click_1(object sender, EventArgs e)
        {

        }
    }
}
