using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LMS.BusinessLogic.Services;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Member;
using LMS.Presentation.Popup.Profile;

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

            // Member privileges
            LblNumberMaxBooksAllowed.Text = $"{profile.MaxBooksAllowed} books";
            LblNumberBorrowingPeriod.Text = $"{profile.BorrowingPeriod} days";
            LblNumberRenewalLimit.Text = $"{profile.RenewalLimit} times";
            LblBoolReservationPrivilege.Text = profile.ReservationPrivilege ? "Yes" : "No";
            LblNumberFineRate.Text = $"₱ {profile.FineRate:N2} / day";

            // Reposition labels to prevent overlap
            RepositionHeaderLabels();

            // Load profile photo
            LoadProfileImage(profile.PhotoPath);
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
    }
}
