namespace LMS.Presentation.Popup.Profile
{
    partial class EditLibrarianStaffProfile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditLibrarianStaffProfile));
            this.BtnSave = new System.Windows.Forms.Button();
            this.LblCancel = new System.Windows.Forms.Button();
            this.PicBxProfilePicContainer = new System.Windows.Forms.Panel();
            this.PicBxProfilePic = new System.Windows.Forms.PictureBox();
            this.Design1 = new System.Windows.Forms.Panel();
            this.LblFirstName = new System.Windows.Forms.Label();
            this.LblLastName = new System.Windows.Forms.Label();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblContactNumber = new System.Windows.Forms.Label();
            this.TxtFirstName = new System.Windows.Forms.TextBox();
            this.TxtLastName = new System.Windows.Forms.TextBox();
            this.TxtEmail = new System.Windows.Forms.TextBox();
            this.TxtContactNumber = new System.Windows.Forms.TextBox();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.LblAddProfPic = new System.Windows.Forms.Label();
            this.GrpBxPersonalDetails = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.GrpBxLoginDetails = new System.Windows.Forms.GroupBox();
            this.ChkBxShowOldPassword = new System.Windows.Forms.CheckBox();
            this.TxtEntOldPass = new System.Windows.Forms.TextBox();
            this.LblEntOldPass = new System.Windows.Forms.Label();
            this.ChkBxShowNewPassword = new System.Windows.Forms.CheckBox();
            this.ChkBxShowConfirmNewPassword = new System.Windows.Forms.CheckBox();
            this.TxtConfirmNewPass = new System.Windows.Forms.TextBox();
            this.LblConfirmNewPass = new System.Windows.Forms.Label();
            this.TxtEntNewPass = new System.Windows.Forms.TextBox();
            this.LblUsername = new System.Windows.Forms.Label();
            this.TxtUsername = new System.Windows.Forms.TextBox();
            this.LblEntNewPass = new System.Windows.Forms.Label();
            this.PicBxProfilePicContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).BeginInit();
            this.GrpBxPersonalDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.GrpBxLoginDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.Location = new System.Drawing.Point(241, 615);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 43);
            this.BtnSave.TabIndex = 40;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(346, 615);
            this.LblCancel.Margin = new System.Windows.Forms.Padding(4);
            this.LblCancel.Name = "LblCancel";
            this.LblCancel.Size = new System.Drawing.Size(97, 43);
            this.LblCancel.TabIndex = 44;
            this.LblCancel.Text = "Cancel";
            this.LblCancel.UseVisualStyleBackColor = false;
            this.LblCancel.Click += new System.EventHandler(this.LblCancel_Click);
            // 
            // PicBxProfilePicContainer
            // 
            this.PicBxProfilePicContainer.BackColor = System.Drawing.Color.White;
            this.PicBxProfilePicContainer.Controls.Add(this.PicBxProfilePic);
            this.PicBxProfilePicContainer.Location = new System.Drawing.Point(152, 38);
            this.PicBxProfilePicContainer.Name = "PicBxProfilePicContainer";
            this.PicBxProfilePicContainer.Size = new System.Drawing.Size(149, 145);
            this.PicBxProfilePicContainer.TabIndex = 45;
            // 
            // PicBxProfilePic
            // 
            this.PicBxProfilePic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicBxProfilePic.Image = ((System.Drawing.Image)(resources.GetObject("PicBxProfilePic.Image")));
            this.PicBxProfilePic.Location = new System.Drawing.Point(13, 16);
            this.PicBxProfilePic.Name = "PicBxProfilePic";
            this.PicBxProfilePic.Size = new System.Drawing.Size(123, 119);
            this.PicBxProfilePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBxProfilePic.TabIndex = 0;
            this.PicBxProfilePic.TabStop = false;
            this.TipPicBxProfilePic.SetToolTip(this.PicBxProfilePic, "Click to edit photo");
            this.PicBxProfilePic.Click += new System.EventHandler(this.PicBxProfilePic_Click);
            // 
            // Design1
            // 
            this.Design1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.Design1.Location = new System.Drawing.Point(0, 0);
            this.Design1.Name = "Design1";
            this.Design1.Size = new System.Drawing.Size(467, 85);
            this.Design1.TabIndex = 46;
            // 
            // LblFirstName
            // 
            this.LblFirstName.AutoSize = true;
            this.LblFirstName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFirstName.ForeColor = System.Drawing.Color.Black;
            this.LblFirstName.Location = new System.Drawing.Point(20, 48);
            this.LblFirstName.Name = "LblFirstName";
            this.LblFirstName.Size = new System.Drawing.Size(86, 21);
            this.LblFirstName.TabIndex = 47;
            this.LblFirstName.Text = "First Name";
            // 
            // LblLastName
            // 
            this.LblLastName.AutoSize = true;
            this.LblLastName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLastName.ForeColor = System.Drawing.Color.Black;
            this.LblLastName.Location = new System.Drawing.Point(205, 48);
            this.LblLastName.Name = "LblLastName";
            this.LblLastName.Size = new System.Drawing.Size(84, 21);
            this.LblLastName.TabIndex = 48;
            this.LblLastName.Text = "Last Name";
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEmail.ForeColor = System.Drawing.Color.Black;
            this.LblEmail.Location = new System.Drawing.Point(20, 123);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(48, 21);
            this.LblEmail.TabIndex = 49;
            this.LblEmail.Text = "Email";
            // 
            // LblContactNumber
            // 
            this.LblContactNumber.AutoSize = true;
            this.LblContactNumber.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblContactNumber.ForeColor = System.Drawing.Color.Black;
            this.LblContactNumber.Location = new System.Drawing.Point(20, 198);
            this.LblContactNumber.Name = "LblContactNumber";
            this.LblContactNumber.Size = new System.Drawing.Size(125, 21);
            this.LblContactNumber.TabIndex = 50;
            this.LblContactNumber.Text = "Contact Number";
            // 
            // TxtFirstName
            // 
            this.TxtFirstName.Location = new System.Drawing.Point(34, 72);
            this.TxtFirstName.Name = "TxtFirstName";
            this.TxtFirstName.Size = new System.Drawing.Size(152, 34);
            this.TxtFirstName.TabIndex = 51;
            // 
            // TxtLastName
            // 
            this.TxtLastName.Location = new System.Drawing.Point(221, 72);
            this.TxtLastName.Name = "TxtLastName";
            this.TxtLastName.Size = new System.Drawing.Size(152, 34);
            this.TxtLastName.TabIndex = 52;
            // 
            // TxtEmail
            // 
            this.TxtEmail.Location = new System.Drawing.Point(34, 147);
            this.TxtEmail.Name = "TxtEmail";
            this.TxtEmail.Size = new System.Drawing.Size(339, 34);
            this.TxtEmail.TabIndex = 53;
            // 
            // TxtContactNumber
            // 
            this.TxtContactNumber.Location = new System.Drawing.Point(34, 222);
            this.TxtContactNumber.Name = "TxtContactNumber";
            this.TxtContactNumber.Size = new System.Drawing.Size(339, 34);
            this.TxtContactNumber.TabIndex = 54;
            // 
            // LblAddProfPic
            // 
            this.LblAddProfPic.AutoSize = true;
            this.LblAddProfPic.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAddProfPic.ForeColor = System.Drawing.Color.DimGray;
            this.LblAddProfPic.Location = new System.Drawing.Point(137, 186);
            this.LblAddProfPic.Name = "LblAddProfPic";
            this.LblAddProfPic.Size = new System.Drawing.Size(182, 28);
            this.LblAddProfPic.TabIndex = 74;
            this.LblAddProfPic.Text = "Click picture to edit";
            // 
            // GrpBxPersonalDetails
            // 
            this.GrpBxPersonalDetails.Controls.Add(this.TxtEmail);
            this.GrpBxPersonalDetails.Controls.Add(this.LblFirstName);
            this.GrpBxPersonalDetails.Controls.Add(this.LblLastName);
            this.GrpBxPersonalDetails.Controls.Add(this.TxtContactNumber);
            this.GrpBxPersonalDetails.Controls.Add(this.LblEmail);
            this.GrpBxPersonalDetails.Controls.Add(this.LblContactNumber);
            this.GrpBxPersonalDetails.Controls.Add(this.TxtLastName);
            this.GrpBxPersonalDetails.Controls.Add(this.TxtFirstName);
            this.GrpBxPersonalDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBxPersonalDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.GrpBxPersonalDetails.Location = new System.Drawing.Point(21, 3);
            this.GrpBxPersonalDetails.Name = "GrpBxPersonalDetails";
            this.GrpBxPersonalDetails.Size = new System.Drawing.Size(398, 281);
            this.GrpBxPersonalDetails.TabIndex = 75;
            this.GrpBxPersonalDetails.TabStop = false;
            this.GrpBxPersonalDetails.Text = "Personal Details";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.GrpBxLoginDetails);
            this.panel1.Controls.Add(this.GrpBxPersonalDetails);
            this.panel1.Location = new System.Drawing.Point(0, 226);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 373);
            this.panel1.TabIndex = 76;
            // 
            // GrpBxLoginDetails
            // 
            this.GrpBxLoginDetails.Controls.Add(this.ChkBxShowOldPassword);
            this.GrpBxLoginDetails.Controls.Add(this.TxtEntOldPass);
            this.GrpBxLoginDetails.Controls.Add(this.LblEntOldPass);
            this.GrpBxLoginDetails.Controls.Add(this.ChkBxShowNewPassword);
            this.GrpBxLoginDetails.Controls.Add(this.ChkBxShowConfirmNewPassword);
            this.GrpBxLoginDetails.Controls.Add(this.TxtConfirmNewPass);
            this.GrpBxLoginDetails.Controls.Add(this.LblConfirmNewPass);
            this.GrpBxLoginDetails.Controls.Add(this.TxtEntNewPass);
            this.GrpBxLoginDetails.Controls.Add(this.LblUsername);
            this.GrpBxLoginDetails.Controls.Add(this.TxtUsername);
            this.GrpBxLoginDetails.Controls.Add(this.LblEntNewPass);
            this.GrpBxLoginDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBxLoginDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.GrpBxLoginDetails.Location = new System.Drawing.Point(21, 290);
            this.GrpBxLoginDetails.Name = "GrpBxLoginDetails";
            this.GrpBxLoginDetails.Size = new System.Drawing.Size(398, 365);
            this.GrpBxLoginDetails.TabIndex = 77;
            this.GrpBxLoginDetails.TabStop = false;
            this.GrpBxLoginDetails.Text = "Login Details";
            // 
            // ChkBxShowOldPassword
            // 
            this.ChkBxShowOldPassword.AutoSize = true;
            this.ChkBxShowOldPassword.Location = new System.Drawing.Point(351, 159);
            this.ChkBxShowOldPassword.Name = "ChkBxShowOldPassword";
            this.ChkBxShowOldPassword.Size = new System.Drawing.Size(22, 21);
            this.ChkBxShowOldPassword.TabIndex = 75;
            this.ChkBxShowOldPassword.UseVisualStyleBackColor = true;
            // 
            // TxtEntOldPass
            // 
            this.TxtEntOldPass.ForeColor = System.Drawing.Color.Black;
            this.TxtEntOldPass.Location = new System.Drawing.Point(35, 151);
            this.TxtEntOldPass.Name = "TxtEntOldPass";
            this.TxtEntOldPass.PasswordChar = '•';
            this.TxtEntOldPass.Size = new System.Drawing.Size(308, 34);
            this.TxtEntOldPass.TabIndex = 74;
            // 
            // LblEntOldPass
            // 
            this.LblEntOldPass.AutoSize = true;
            this.LblEntOldPass.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntOldPass.ForeColor = System.Drawing.Color.Black;
            this.LblEntOldPass.Location = new System.Drawing.Point(21, 127);
            this.LblEntOldPass.Name = "LblEntOldPass";
            this.LblEntOldPass.Size = new System.Drawing.Size(143, 21);
            this.LblEntOldPass.TabIndex = 73;
            this.LblEntOldPass.Text = "Enter old password";
            // 
            // ChkBxShowNewPassword
            // 
            this.ChkBxShowNewPassword.AutoSize = true;
            this.ChkBxShowNewPassword.Location = new System.Drawing.Point(351, 236);
            this.ChkBxShowNewPassword.Name = "ChkBxShowNewPassword";
            this.ChkBxShowNewPassword.Size = new System.Drawing.Size(22, 21);
            this.ChkBxShowNewPassword.TabIndex = 72;
            this.ChkBxShowNewPassword.UseVisualStyleBackColor = true;
            // 
            // ChkBxShowConfirmNewPassword
            // 
            this.ChkBxShowConfirmNewPassword.AutoSize = true;
            this.ChkBxShowConfirmNewPassword.Location = new System.Drawing.Point(351, 312);
            this.ChkBxShowConfirmNewPassword.Name = "ChkBxShowConfirmNewPassword";
            this.ChkBxShowConfirmNewPassword.Size = new System.Drawing.Size(22, 21);
            this.ChkBxShowConfirmNewPassword.TabIndex = 71;
            this.ChkBxShowConfirmNewPassword.UseVisualStyleBackColor = true;
            // 
            // TxtConfirmNewPass
            // 
            this.TxtConfirmNewPass.ForeColor = System.Drawing.Color.Black;
            this.TxtConfirmNewPass.Location = new System.Drawing.Point(35, 304);
            this.TxtConfirmNewPass.Name = "TxtConfirmNewPass";
            this.TxtConfirmNewPass.PasswordChar = '•';
            this.TxtConfirmNewPass.Size = new System.Drawing.Size(308, 34);
            this.TxtConfirmNewPass.TabIndex = 70;
            // 
            // LblConfirmNewPass
            // 
            this.LblConfirmNewPass.AutoSize = true;
            this.LblConfirmNewPass.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblConfirmNewPass.ForeColor = System.Drawing.Color.Black;
            this.LblConfirmNewPass.Location = new System.Drawing.Point(21, 280);
            this.LblConfirmNewPass.Name = "LblConfirmNewPass";
            this.LblConfirmNewPass.Size = new System.Drawing.Size(171, 21);
            this.LblConfirmNewPass.TabIndex = 69;
            this.LblConfirmNewPass.Text = "Confirm new password";
            // 
            // TxtEntNewPass
            // 
            this.TxtEntNewPass.ForeColor = System.Drawing.Color.Black;
            this.TxtEntNewPass.Location = new System.Drawing.Point(35, 228);
            this.TxtEntNewPass.Name = "TxtEntNewPass";
            this.TxtEntNewPass.PasswordChar = '•';
            this.TxtEntNewPass.Size = new System.Drawing.Size(308, 34);
            this.TxtEntNewPass.TabIndex = 68;
            // 
            // LblUsername
            // 
            this.LblUsername.AutoSize = true;
            this.LblUsername.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblUsername.ForeColor = System.Drawing.Color.Black;
            this.LblUsername.Location = new System.Drawing.Point(17, 49);
            this.LblUsername.Name = "LblUsername";
            this.LblUsername.Size = new System.Drawing.Size(81, 21);
            this.LblUsername.TabIndex = 59;
            this.LblUsername.Text = "Username";
            // 
            // TxtUsername
            // 
            this.TxtUsername.ForeColor = System.Drawing.Color.Black;
            this.TxtUsername.Location = new System.Drawing.Point(31, 73);
            this.TxtUsername.Name = "TxtUsername";
            this.TxtUsername.Size = new System.Drawing.Size(342, 34);
            this.TxtUsername.TabIndex = 63;
            // 
            // LblEntNewPass
            // 
            this.LblEntNewPass.AutoSize = true;
            this.LblEntNewPass.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntNewPass.ForeColor = System.Drawing.Color.Black;
            this.LblEntNewPass.Location = new System.Drawing.Point(21, 204);
            this.LblEntNewPass.Name = "LblEntNewPass";
            this.LblEntNewPass.Size = new System.Drawing.Size(150, 21);
            this.LblEntNewPass.TabIndex = 67;
            this.LblEntNewPass.Text = "Enter new password";
            // 
            // EditLibrarianStaffProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(468, 689);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblAddProfPic);
            this.Controls.Add(this.PicBxProfilePicContainer);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.Design1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditLibrarianStaffProfile";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Profile";
            this.PicBxProfilePicContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).EndInit();
            this.GrpBxPersonalDetails.ResumeLayout(false);
            this.GrpBxPersonalDetails.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.GrpBxLoginDetails.ResumeLayout(false);
            this.GrpBxLoginDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Panel PicBxProfilePicContainer;
        private System.Windows.Forms.PictureBox PicBxProfilePic;
        private System.Windows.Forms.Panel Design1;
        private System.Windows.Forms.Label LblFirstName;
        private System.Windows.Forms.Label LblLastName;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.Label LblContactNumber;
        private System.Windows.Forms.TextBox TxtFirstName;
        private System.Windows.Forms.TextBox TxtLastName;
        private System.Windows.Forms.TextBox TxtEmail;
        private System.Windows.Forms.TextBox TxtContactNumber;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
        private System.Windows.Forms.Label LblAddProfPic;
        private System.Windows.Forms.GroupBox GrpBxPersonalDetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox GrpBxLoginDetails;
        private System.Windows.Forms.CheckBox ChkBxShowOldPassword;
        private System.Windows.Forms.TextBox TxtEntOldPass;
        private System.Windows.Forms.Label LblEntOldPass;
        private System.Windows.Forms.CheckBox ChkBxShowNewPassword;
        private System.Windows.Forms.CheckBox ChkBxShowConfirmNewPassword;
        private System.Windows.Forms.TextBox TxtConfirmNewPass;
        private System.Windows.Forms.Label LblConfirmNewPass;
        private System.Windows.Forms.TextBox TxtEntNewPass;
        private System.Windows.Forms.Label LblUsername;
        private System.Windows.Forms.TextBox TxtUsername;
        private System.Windows.Forms.Label LblEntNewPass;
    }
}