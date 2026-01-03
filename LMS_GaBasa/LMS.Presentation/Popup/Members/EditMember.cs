using LMS.BusinessLogic.Services.EditMember;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Member;
using LMS.Model.Models.Enums;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Members
{
    public partial class EditMember : Form
    {
        private readonly IEditMemberService _editMemberService;
        private int _memberId;
        private string _selectedPhotoPath;
        private string _selectedValidIdPath;
        private MemberStatus _originalStatus;

        public EditMember() : this(new EditMemberService(new MemberRepository()))
        {
        }

        public EditMember(IEditMemberService editMemberService)
        {
            InitializeComponent();
            _editMemberService = editMemberService ?? throw new ArgumentNullException(nameof(editMemberService));
            SetupForm();
        }

        private void SetupForm()
        {
            // Setup Member Type dropdown
            CmbBxMemberType.Items.Clear();
            CmbBxMemberType.Items.Add("Staff");
            CmbBxMemberType.Items.Add("Faculty");
            CmbBxMemberType.Items.Add("Student");
            CmbBxMemberType.Items.Add("Guest");

            // Wire up events
            BtnSave.Click += BtnSave_Click;
            LblCancel.Click += LblCancel_Click;
            PicBxProfilePic.Click += PicBxProfilePic_Click;
            PicBxValidID.Click += PicBxValidID_Click;
        }

        public void LoadMember(int memberId)
        {
            _memberId = memberId;

            var member = _editMemberService.GetMemberForEdit(memberId);
            if (member == null)
            {
                MessageBox.Show("Member not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _originalStatus = member.Status;

            // Populate form fields
            TxtFirstName.Text = member.FirstName;
            TxtLastName.Text = member.LastName;
            TxtEmail.Text = member.Email;
            TxtAddress.Text = member.Address;
            TxtContactNumber.Text = member.ContactNumber;
            _selectedPhotoPath = member.PhotoPath;
            _selectedValidIdPath = member.ValidIdPath;

            // Set Member Type dropdown
            for (int i = 0; i < CmbBxMemberType.Items.Count; i++)
            {
                if (CmbBxMemberType.Items[i].ToString().Equals(member.MemberTypeName, StringComparison.OrdinalIgnoreCase))
                {
                    CmbBxMemberType.SelectedIndex = i;
                    break;
                }
            }

            // Setup Status dropdown based on current status
            SetupStatusDropdown(member.Status);

            // Load profile photo
            LoadImage(PicBxProfilePic, member.PhotoPath);

            // Load valid ID
            LoadImage(PicBxValidID, member.ValidIdPath);
        }

        private void SetupStatusDropdown(MemberStatus currentStatus)
        {
            CmbBxUserStatus.Items.Clear();

            // Expired status is read-only (automatic based on expiration date)
            if (currentStatus == MemberStatus.Expired)
            {
                CmbBxUserStatus.Items.Add(new StatusComboItem("Expired", MemberStatus.Expired));
                CmbBxUserStatus.SelectedIndex = 0;
                CmbBxUserStatus.Enabled = false;
            }
            else
            {
                // Active, Inactive, and Suspended can be manually set
                CmbBxUserStatus.Items.Add(new StatusComboItem("Active", MemberStatus.Active));
                CmbBxUserStatus.Items.Add(new StatusComboItem("Inactive", MemberStatus.Inactive));
                CmbBxUserStatus.Items.Add(new StatusComboItem("Suspended", MemberStatus.Suspended));
                CmbBxUserStatus.Enabled = true;

                // Set selection based on current status
                for (int i = 0; i < CmbBxUserStatus.Items.Count; i++)
                {
                    if (CmbBxUserStatus.Items[i] is StatusComboItem item && item.Value == currentStatus)
                    {
                        CmbBxUserStatus.SelectedIndex = i;
                        break;
                    }
                }

                // Default to first item if not found
                if (CmbBxUserStatus.SelectedIndex == -1)
                    CmbBxUserStatus.SelectedIndex = 0;
            }
        }

        private void LoadImage(PictureBox pictureBox, string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (File.Exists(fullPath))
            {
                try
                {
                    pictureBox.Image = Image.FromFile(fullPath);
                }
                catch
                {
                    // Keep default image if loading fails
                }
            }
        }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Profile Picture";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedPhotoPath = ofd.FileName;
                    PicBxProfilePic.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void PicBxValidID_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Valid ID";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedValidIdPath = ofd.FileName;
                    PicBxValidID.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            var selectedStatus = (CmbBxUserStatus.SelectedItem as StatusComboItem)?.Value ?? _originalStatus;

            var dto = new DTOEditMember
            {
                MemberID = _memberId,
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                Address = TxtAddress.Text.Trim(),
                ContactNumber = TxtContactNumber.Text.Trim(),
                MemberTypeName = CmbBxMemberType.SelectedItem?.ToString() ?? "Student",
                PhotoPath = _selectedPhotoPath,
                ValidIdPath = _selectedValidIdPath,
                Status = selectedStatus
            };

            var result = _editMemberService.UpdateMember(dto);

            if (result.Success)
            {
                MessageBox.Show("Member updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                ShowError("First name is required.");
                TxtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                ShowError("Last name is required.");
                TxtLastName.Focus();
                return false;
            }

            if (CmbBxMemberType.SelectedIndex == -1)
            {
                ShowError("Member type is required.");
                CmbBxMemberType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                ShowError("Email is required.");
                TxtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(TxtEmail.Text))
            {
                ShowError("Please enter a valid email address.");
                TxtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                ShowError("Address is required.");
                TxtAddress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtContactNumber.Text))
            {
                ShowError("Contact number is required.");
                TxtContactNumber.Focus();
                return false;
            }

            if (!IsValidContactNumber(TxtContactNumber.Text))
            {
                ShowError("Contact number must be exactly 11 digits.");
                TxtContactNumber.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidContactNumber(string contactNumber)
        {
            string cleaned = Regex.Replace(contactNumber, @"[\s\-]", "");
            return Regex.IsMatch(cleaned, @"^\d{11}$");
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private class StatusComboItem
        {
            public string Text { get; }
            public MemberStatus Value { get; }

            public StatusComboItem(string text, MemberStatus value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}
