using System;
using System.Drawing;
using System.Windows.Forms;
using LMS.BusinessLogic.Managers.Circulation;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCFines : UserControl
    {
        private readonly ICirculationManager _circulationManager;
        private DTOCirculationMemberInfo _currentMember;

        public UCFines()
        {
            InitializeComponent();

            // Setup dependencies
            var circulationRepo = new CirculationRepository();
            _circulationManager = new CirculationManager(circulationRepo);

            // Wire up events
            TxtSearchMember.KeyDown += TxtSearchMember_KeyDown;
            BtnSearchMember.Click += BtnSearchMember_Click;

            // Initialize UI state
            ClearMemberInfo();
        }

        private void TxtSearchMember_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchMember();
            }
        }

        private void BtnSearchMember_Click(object sender, EventArgs e)
        {
            SearchMember();
        }

        private void SearchMember()
        {
            string input = TxtSearchMember.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a Member ID.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtSearchMember.Focus();
                return;
            }

            var memberInfo = _circulationManager.GetMemberByFormattedId(input);

            if (memberInfo == null)
            {
                MessageBox.Show($"Member not found: {input}", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearMemberInfo();
                TxtSearchMember.Focus();
                TxtSearchMember.SelectAll();
                return;
            }

            _currentMember = memberInfo;
            DisplayMemberInfo(memberInfo);
        }

        private void DisplayMemberInfo(DTOCirculationMemberInfo memberInfo)
        {
            // 1. LblName - member's full name
            LblName.Text = $"Name: {memberInfo.FullName}";

            // 2. LblType - member type
            LblType.Text = $"Type: {memberInfo.MemberType}";

            // 3. LblStatus - Member's current status (Active, Inactive, Suspended, Expired)
            LblStatus.Text = $"Status: {memberInfo.Status}";
            SetStatusColor(memberInfo.Status);

            // 4. LblTotalFines - total fines from Fine table
            LblTotalFines.Text = $"Total Fines: ₱{memberInfo.TotalUnpaidFines:N2}";

            // 5. LblBooksCurrentlyBorrowed - currently borrowed / limit from MemberType
            LblBooksCurrentlyBorrowed.Text = $"Books Currently Borrowed: {memberInfo.CurrentBorrowedCount} / {memberInfo.MaxBooksAllowed}";

            // 6. LblOverdueCount - how many overdues they have
            LblOverdueCount.Text = $"Overdue Count: {memberInfo.OverdueCount}";
        }

        private void SetStatusColor(string status)
        {
            switch (status?.ToLowerInvariant())
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

        private void ClearMemberInfo()
        {
            _currentMember = null;

            LblName.Text = "Name:";
            LblType.Text = "Type:";
            LblStatus.Text = "Status:";
            LblStatus.ForeColor = Color.Black;
            LblTotalFines.Text = "Total Fines:";
            LblBooksCurrentlyBorrowed.Text = "Books Currently Borrowed:";
            LblOverdueCount.Text = "Overdue Count:";
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void LblMemberID_Click(object sender, EventArgs e)
        {
        }

        private void BtnEnterMemberID_Click(object sender, EventArgs e)
        {
        }
    }
}
