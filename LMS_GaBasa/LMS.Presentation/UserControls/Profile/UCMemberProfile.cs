using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Member;
using LMS.Presentation.Popup.Profile;
using LMS.Presentation.Popup.Multipurpose;

namespace LMS.Presentation.UserControls.Profile
{
    public partial class UCMemberProfile : UserControl
    {
        private readonly IMemberProfileService _memberProfileService;
        private int _currentUserId;

        /// <summary>
        /// Raised when the user profile has been updated successfully.
        /// </summary>
        public event Action ProfileUpdated;

        // Designer-required parameterless constructor
        public UCMemberProfile() : this(null) { }

        // Dependency injection constructor
        public UCMemberProfile(IMemberProfileService memberProfileService)
        {
            InitializeComponent();
            _memberProfileService = memberProfileService
                ?? new MemberProfileService(new MemberRepository()); // Fallback for designer
        }

        public void LoadMemberProfile(int userId)
        {
            _currentUserId = userId;
            MemberProfileDto profile = _memberProfileService.GetMemberProfile(userId);

            if (profile == null)
            {
                MessageBox.Show("Member profile not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // User info
            LblFullname.Text = profile.FullName;
            LblIDNumber.Text = profile.FormattedID;  // Changed from profile.UserID.ToString()
            LblEmail.Text = profile.Email;
            LblRole.Text = profile.Role;
            LblMemberStatus.Text = profile.Status;

            // Member info
            LblMemberType.Text = profile.MemberTypeName;
            LblMemberStatus.Text = profile.MemberStatus;
            LblActualAddress.Text = profile.Address;
            LblActualContact.Text = profile.ContactNumber;
            LblActualRegDate.Text = profile.RegistrationDate.ToString("MMMM d, yyyy");
            LblActualExpDate.Text = profile.ExpirationDate.ToString("MMMM d, yyyy");

            // Calculate penalty level and effective privileges
            int penaltyLevel = CalculatePenaltyLevel(profile);
            int effectiveMaxBooks = Math.Max(0, profile.MaxBooksAllowed - penaltyLevel);
            int effectiveBorrowingPeriod = Math.Max(1, profile.BorrowingPeriod - penaltyLevel);
            int effectiveRenewalLimit = Math.Max(0, profile.RenewalLimit - penaltyLevel);
            bool effectiveReservationPrivilege = profile.ReservationPrivilege && penaltyLevel == 0;

            // Member privileges - show effective values with penalty indicator when applicable
            if (penaltyLevel > 0)
            {
                LblNumberMaxBooksAllowed.Text = $"{effectiveMaxBooks} books (-{penaltyLevel})";
                LblNumberMaxBooksAllowed.ForeColor = Color.FromArgb(200, 0, 0);

                LblNumberBorrowingPeriod.Text = $"{effectiveBorrowingPeriod} days (-{penaltyLevel})";
                LblNumberBorrowingPeriod.ForeColor = Color.FromArgb(200, 0, 0);

                LblNumberRenewalLimit.Text = $"{effectiveRenewalLimit} times (-{penaltyLevel})";
                LblNumberRenewalLimit.ForeColor = Color.FromArgb(200, 0, 0);

                if (profile.ReservationPrivilege && !effectiveReservationPrivilege)
                {
                    LblBoolReservationPrivilege.Text = "No (Penalty)";
                    LblBoolReservationPrivilege.ForeColor = Color.FromArgb(200, 0, 0);
                }
                else
                {
                    LblBoolReservationPrivilege.Text = effectiveReservationPrivilege ? "Yes" : "No";
                    LblBoolReservationPrivilege.ForeColor = Color.Black;
                }
            }
            else
            {
                LblNumberMaxBooksAllowed.Text = $"{profile.MaxBooksAllowed} books";
                LblNumberMaxBooksAllowed.ForeColor = Color.Black;

                LblNumberBorrowingPeriod.Text = $"{profile.BorrowingPeriod} days";
                LblNumberBorrowingPeriod.ForeColor = Color.Black;

                LblNumberRenewalLimit.Text = $"{profile.RenewalLimit} times";
                LblNumberRenewalLimit.ForeColor = Color.Black;

                LblBoolReservationPrivilege.Text = profile.ReservationPrivilege ? "Yes" : "No";
                LblBoolReservationPrivilege.ForeColor = Color.Black;
            }

            LblNumberFineRate.Text = $"₱ {profile.FineRate:N2} / day";

            // Account Standing
            SetAccountStanding(profile);

            // Reposition labels to prevent overlap
            RepositionHeaderLabels();

            // Load profile photo
            LoadProfileImage(profile.PhotoPath);
        }

        /// <summary>
        /// Calculates the penalty level based on fines and overdues.
        /// 0 = good standing (no fines, no overdues)
        /// 1 = has fine OR overdue
        /// 2 = has BOTH fine AND overdue
        /// </summary>
        private int CalculatePenaltyLevel(MemberProfileDto profile)
        {
            bool hasFines = profile.TotalUnpaidFines > 0m;
            bool hasOverdues = profile.OverdueCount > 0;

            if (hasFines && hasOverdues) return 2;
            if (hasFines || hasOverdues) return 1;
            return 0;
        }

        /// <summary>
        /// Determines and displays the account standing based on fines and overdues.
        /// </summary>
        private void SetAccountStanding(MemberProfileDto profile)
        {
            bool hasFines = profile.TotalUnpaidFines > 0;
            bool hasOverdues = profile.OverdueCount > 0;
            bool fineExceedsMaxCap = profile.MaxFineCap > 0 && profile.TotalUnpaidFines >= profile.MaxFineCap;

            // Suspended: fine reached MaxFineCap
            if (fineExceedsMaxCap)
            {
                LblActualAccountStanding.Text = "Suspended";
                LblActualAccountStanding.ForeColor = Color.FromArgb(200, 0, 0); // Red
            }
            // Restricted: has at least 1 fine or overdue
            else if (hasFines || hasOverdues)
            {
                LblActualAccountStanding.Text = "Restricted";
                LblActualAccountStanding.ForeColor = Color.FromArgb(255, 165, 0); // Orange
            }
            // Good: no fines and no overdues
            else
            {
                LblActualAccountStanding.Text = "Good";
                LblActualAccountStanding.ForeColor = Color.FromArgb(0, 200, 0); // Green
            }
        }

        /// <summary>
        /// Repositions LblRole, LblMemberType, and LblMemberStatus after LblFullname to prevent overlap.
        /// </summary>
        private void RepositionHeaderLabels()
        {
            const int spacing = 10;

            // Position LblRole right after LblFullname
            int roleX = LblFullname.Right + spacing;
            LblRole.Location = new Point(roleX, LblRole.Location.Y);

            // Position LblMemberType right after LblRole
            int memberTypeX = LblRole.Right + spacing;
            LblMemberType.Location = new Point(memberTypeX, LblMemberType.Location.Y);

            // Position LblMemberStatus right after LblMemberType
            int memberStatusX = LblMemberType.Right + spacing;
            LblMemberStatus.Location = new Point(memberStatusX, LblMemberStatus.Location.Y);
        }

        private void LoadProfileImage(string photoPath)
        {
            if (PicBxProfilePic.Image != null)
            {
                PicBxProfilePic.Image.Dispose();
                PicBxProfilePic.Image = null;
            }

            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
            {
                using (var stream = new FileStream(photoPath, FileMode.Open, FileAccess.Read))
                {
                    PicBxProfilePic.Image = Image.FromStream(stream);
                }
                PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void BtnEditProfile_Click(object sender, EventArgs e)
        {
            MemberProfileDto currentProfile = _memberProfileService.GetMemberProfile(_currentUserId);

            if (currentProfile == null)
            {
                MessageBox.Show("Could not load profile for editing.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var editForm = new EditMemberProfile(_memberProfileService);
            editForm.LoadProfile(currentProfile);

            editForm.ProfileUpdated += (updatedProfile) =>
            {
                // Reload this UserControl's display
                LoadMemberProfile(_currentUserId);

                // Notify MainForm to refresh the top bar
                ProfileUpdated?.Invoke();
            };

            editForm.ShowDialog();
        }

        private void LblActualAddress_Click(object sender, EventArgs e)
        {

        }

        private void LblContact_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Click handler for the View Valid ID button.
        /// Resolves stored path and opens the ViewValidID form with the image.
        /// </summary>
        private void BtnViewValidID_Click(object sender, EventArgs e)
        {
            MemberProfileDto profile = _member_profile_service_getprofile_safe();
            if (profile == null)
            {
                MessageBox.Show("Could not load member profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string resolvedPath = ResolveValidIdPath(profile.ValidIdPath);

            if (string.IsNullOrEmpty(resolvedPath) || !File.Exists(resolvedPath))
            {
                MessageBox.Show("Valid ID not found for this member.", "No Valid ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var viewForm = new ViewValidID();
            viewForm.LoadValidID(resolvedPath);
            viewForm.ShowDialog();
        }

        // small helper to safely get profile in event handlers
        private MemberProfileDto _member_profile_service_getprofile_safe()
        {
            try
            {
                return _memberProfileService.GetMemberProfile(_currentUserId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to resolve a stored valid ID path. Accepts absolute paths, project-relative paths,
        /// or filenames stored in the Assets/dataimages/ValidIDs folder.
        /// </summary>
        private string ResolveValidIdPath(string validIdPath)
        {
            if (string.IsNullOrEmpty(validIdPath))
                return null;

            try
            {
                // If path already points to an existing file, use it
                if (File.Exists(validIdPath))
                    return validIdPath;

                // Try interpreting as relative to the app base directory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string relativeCandidate = Path.Combine(baseDir, validIdPath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                if (File.Exists(relativeCandidate))
                    return relativeCandidate;

                // Try the canonical assets folder: Assets/dataimages/ValidIDs/<filename>
                string filename = Path.GetFileName(validIdPath);
                string assetsCandidate = Path.Combine(baseDir, "Assets", "dataimages", "ValidIDs", filename);
                if (File.Exists(assetsCandidate))
                    return assetsCandidate;
            }
            catch
            {
                // ignore path resolution errors and return null
            }

            return null;
        }
    }
}
