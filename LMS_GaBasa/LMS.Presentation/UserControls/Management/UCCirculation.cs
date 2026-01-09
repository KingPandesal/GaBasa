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

            // NEW: wire scan accession button to open camera dialog
            BtnScanAccessionNumber.Click -= BtnScanAccessionNumber_Click;
            BtnScanAccessionNumber.Click += BtnScanAccessionNumber_Click;

            // Initialize UI state
            ClearMemberResults();

            // Hide book checkout section initially
            GrpBxBookCheckout.Visible = false;
        }

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

            // Get member by formatted ID (e.g., "MEM-0023")
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

            // Store current member for later use
            _currentMember = memberInfo;

            // Display member information
            DisplayMemberInfo(memberInfo);

            // Show/hide book checkout based on eligibility
            UpdateBookCheckoutVisibility(memberInfo);
        }

        private void DisplayMemberInfo(DTOCirculationMemberInfo memberInfo)
        {
            // Set full name
            LblFullName.Text = $"Full Name: {memberInfo.FullName}";

            // Set member type
            LblMemberType.Text = $"Member Type: {memberInfo.MemberType}";

            // Set status with color coding
            LblStatus.Text = $"Status: {memberInfo.Status}";
            SetStatusColor(memberInfo.Status);

            // Load profile picture
            LoadProfileImage(memberInfo.PhotoPath);

            // Display borrowing statistics
            DisplayBorrowingStatistics(memberInfo);

            // Display eligibility checks
            DisplayEligibilityChecks(memberInfo);
        }

        private void DisplayBorrowingStatistics(DTOCirculationMemberInfo memberInfo)
        {
            // Borrowed: X / Y (current / max allowed)
            LblBorrowed.Text = $"Borrowed: {memberInfo.CurrentBorrowedCount} / {memberInfo.MaxBooksAllowed}";

            // Overdue count
            LblOverdue.Text = $"Overdue: {memberInfo.OverdueCount}";

            // Total unpaid fines
            LblFine.Text = $"Fine: ₱{memberInfo.TotalUnpaidFines:N2}";
        }

        private void DisplayEligibilityChecks(DTOCirculationMemberInfo memberInfo)
        {
            // Member Active check
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

            // No Overdue Books check
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

            // Borrow Limit check
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

            // Fine Limit check - use MaxFineCap from member type
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
            // Only show GrpBxBookCheckout if all eligibility checks pass
            GrpBxBookCheckout.Visible = memberInfo.CanBorrow;
        }

        private void SetStatusColor(string status)
        {
            switch (status?.ToLower())
            {
                case "active":
                    LblStatus.ForeColor = Color.FromArgb(0, 200, 0); // Green
                    break;
                case "inactive":
                    LblStatus.ForeColor = Color.FromArgb(200, 200, 0); // Yellow
                    break;
                case "suspended":
                case "expired":
                    LblStatus.ForeColor = Color.FromArgb(200, 0, 0); // Red
                    break;
                default:
                    LblStatus.ForeColor = Color.Black;
                    break;
            }
        }

        private void LoadProfileImage(string photoPath)
        {
            // Dispose previous image to avoid memory leaks
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

            // Build full path from relative path
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photoPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    // Use FileStream to avoid file locking issues
                    using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        PicBxMemberProfilePicture.Image = Image.FromStream(stream);
                    }
                    PicBxMemberProfilePicture.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch
                {
                    // If loading fails, just show the default gray background
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

            // If path is not rooted, assume it's relative to application base directory
            if (!Path.IsPathRooted(candidatePath))
            {
                candidatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, candidatePath);
            }

            if (!File.Exists(candidatePath))
            {
                // Try common assets folder if stored path is just filename
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

        // NEW: Handler to open Camera form, receive scanned accession and populate accession textbox
        private void BtnScanAccessionNumber_Click(object sender, EventArgs e)
        {
            try
            {
                using (var cam = new LMS.Presentation.Popup.Multipurpose.Camera())
                {
                    var result = cam.ShowDialog(this);
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(cam.ScannedCode))
                    {
                        // Fill the accession textbox and trigger existing enter logic
                        TxtAccessionNumber.Text = cam.ScannedCode.Trim();
                        // If you have logic on Enter click, call it; otherwise simulate Enter key:
                        BtnEnterAccessionNumber?.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open camera: {ex.Message}", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Hide book checkout section
            GrpBxBookCheckout.Visible = false;
        }
    }
}
