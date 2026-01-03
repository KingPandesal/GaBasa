using LMS.BusinessLogic.Services.AddMember;
using LMS.Model.DTOs.Member;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Members
{
    public partial class AddMember : Form
    {
        private readonly IAddMemberService _memberService;
        private string _profilePicPath = null;
        private string _validIdPath = null;

        public AddMember(IAddMemberService memberService)
        {
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            InitializeComponent();
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            BtnSave.Click += BtnSave_Click;
            LblCancel.Click += LblCancel_Click;
            PicBxProfilePic.Click += PicBxProfilePic_Click;
            PicBxValidID.Click += PicBxValidID_Click;
            ChkBxShowPassword1.CheckedChanged += ChkBxShowPassword1_CheckedChanged;
            ChkBxShowPassword2.CheckedChanged += ChkBxShowPassword2_CheckedChanged;
        }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Profile Picture";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _profilePicPath = ofd.FileName;
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
                    _validIdPath = ofd.FileName;
                    PicBxValidID.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void ChkBxShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            TxtPassword.PasswordChar = ChkBxShowPassword1.Checked ? '\0' : '•';
        }

        private void ChkBxShowPassword2_CheckedChanged(object sender, EventArgs e)
        {
            TxtConfirmPassword.PasswordChar = ChkBxShowPassword2.Checked ? '\0' : '•';
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            var dto = new DTOCreateMember
            {
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                Address = TxtAddress.Text.Trim(),
                ContactNumber = TxtContactNumber.Text.Trim(),
                MemberTypeName = CmbBxMemberType.Text.Trim(),
                Username = TxtUsername.Text.Trim(),
                Password = TxtPassword.Text,
                PhotoPath = _profilePicPath,
                ValidIdPath = _validIdPath
            };

            var result = _memberService.CreateMember(dto);

            if (result.Success)
            {
                MessageBox.Show("Member added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            // Validate First Name
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                ShowError("First name is required.");
                TxtFirstName.Focus();
                return false;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                ShowError("Last name is required.");
                TxtLastName.Focus();
                return false;
            }

            // Validate Member Type
            if (CmbBxMemberType.SelectedIndex == -1 || string.IsNullOrWhiteSpace(CmbBxMemberType.Text))
            {
                ShowError("Member type is required.");
                CmbBxMemberType.Focus();
                return false;
            }

            // Validate Email
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

            // Validate Address
            if (string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                ShowError("Address is required.");
                TxtAddress.Focus();
                return false;
            }

            // Validate Contact Number
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

            // Validate Username
            if (TxtUsername.Text.Length < 8)
            {
                ShowError("Username must be at least 8 characters.");
                TxtUsername.Focus();
                return false;
            }

            // Validate Password
            if (!IsValidPassword(TxtPassword.Text, out string passwordError))
            {
                ShowError(passwordError);
                TxtPassword.Focus();
                return false;
            }

            // Validate Confirm Password
            if (TxtPassword.Text != TxtConfirmPassword.Text)
            {
                ShowError("Passwords do not match.");
                TxtConfirmPassword.Focus();
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

        private bool IsValidPassword(string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errorMessage = "Password must contain at least 1 uppercase letter.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errorMessage = "Password must contain at least 1 number.";
                return false;
            }
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            {
                errorMessage = "Password must contain at least 1 symbol.";
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnSave_Click_1(object sender, EventArgs e)
        {

        }

        private void LblCancel_Click_1(object sender, EventArgs e)
        {

        }
    }
}
