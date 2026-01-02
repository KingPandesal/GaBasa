using LMS.BusinessLogic.Services.EditUser;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Enums;
using System;
using System.IO;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Users
{
    public partial class EditUser : Form
    {
        private readonly IEditUserService _editUserService;
        private int _userId;
        private string _selectedPhotoPath;

        private const string UserPhotosFolder = @"Assets\dataimages\Users";

        public EditUser() : this(new EditUserService(new UserRepository()))
        {
        }

        public EditUser(IEditUserService editUserService)
        {
            InitializeComponent();
            _editUserService = editUserService ?? throw new ArgumentNullException(nameof(editUserService));
            SetupForm();
        }

        private void SetupForm()
        {
            // Bind role combo
            comboBox1.Items.Clear();
            comboBox1.Items.Add(new ComboItem("Librarian / Admin", Role.Librarian));
            comboBox1.Items.Add(new ComboItem("Library Staff", Role.Staff));

            // Wire up events
            BtnSave.Click += BtnSave_Click;
            PicBxProfilePic.Click += PicBxProfilePic_Click;
        }

        public void LoadUser(int userId)
        {
            _userId = userId;

            var user = _editUserService.GetUserForEdit(userId);
            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            TxtFirstName.Text = user.FirstName ?? "";
            TxtLastName.Text = user.LastName ?? "";
            TxtEmail.Text = user.Email ?? "";
            TxtContactNumber.Text = user.ContactNumber ?? "";
            _selectedPhotoPath = user.PhotoPath;

            // Set role combo selection
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i] is ComboItem item && item.Value == user.Role)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            // Load photo if exists
            if (!string.IsNullOrEmpty(user.PhotoPath))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, user.PhotoPath);
                if (File.Exists(fullPath))
                {
                    PicBxProfilePic.ImageLocation = fullPath;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var selectedRole = (comboBox1.SelectedItem as ComboItem)?.Value ?? Role.Staff;

            // Handle photo - copy new photo if selected
            string storedPhotoPath = _selectedPhotoPath;
            if (!string.IsNullOrEmpty(_selectedPhotoPath) && !_selectedPhotoPath.StartsWith(UserPhotosFolder))
            {
                try
                {
                    storedPhotoPath = CopyPhotoToAssets(_selectedPhotoPath, _userId.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save photo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var dto = new DTOEditUser
            {
                UserID = _userId,
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Email = TxtEmail.Text.Trim(),
                ContactNumber = TxtContactNumber.Text.Trim(),
                PhotoPath = storedPhotoPath,
                Role = selectedRole
            };

            var result = _editUserService.UpdateUser(dto);

            if (result.Success)
            {
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CopyPhotoToAssets(string sourceFilePath, string userId)
        {
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string destinationFolder = Path.Combine(appBasePath, UserPhotosFolder);

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            string extension = Path.GetExtension(sourceFilePath);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName = $"user_{userId}_{timestamp}{extension}";

            string destinationPath = Path.Combine(destinationFolder, newFileName);

            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            return Path.Combine(UserPhotosFolder, newFileName);
        }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedPhotoPath = ofd.FileName;
                    PicBxProfilePic.ImageLocation = _selectedPhotoPath;
                }
            }
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private class ComboItem
        {
            public string Text { get; }
            public Role Value { get; }

            public ComboItem(string text, Role value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }
    }
}
