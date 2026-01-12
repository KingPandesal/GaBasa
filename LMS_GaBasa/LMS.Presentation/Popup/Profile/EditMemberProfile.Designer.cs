namespace LMS.Presentation.Popup.Profile
{
    partial class EditMemberProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMemberProfile));
            this.PicBxProfilePicContainer = new System.Windows.Forms.Panel();
            this.PicBxProfilePic = new System.Windows.Forms.PictureBox();
            this.LblCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.TxtContactNumber = new System.Windows.Forms.TextBox();
            this.TxtEmail = new System.Windows.Forms.TextBox();
            this.TxtLastName = new System.Windows.Forms.TextBox();
            this.TxtFirstName = new System.Windows.Forms.TextBox();
            this.LblContactNumber = new System.Windows.Forms.Label();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblLastName = new System.Windows.Forms.Label();
            this.LblFirstName = new System.Windows.Forms.Label();
            this.Design1 = new System.Windows.Forms.Panel();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.PicBxValidID = new System.Windows.Forms.PictureBox();
            this.TxtAddress = new System.Windows.Forms.TextBox();
            this.LblAddress = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LblAddProfPic = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.PicBxValidIDContainer = new System.Windows.Forms.Panel();
            this.PicBxProfilePicContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxValidID)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.PicBxValidIDContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PicBxProfilePicContainer
            // 
            this.PicBxProfilePicContainer.BackColor = System.Drawing.Color.White;
            this.PicBxProfilePicContainer.Controls.Add(this.PicBxProfilePic);
            this.PicBxProfilePicContainer.Location = new System.Drawing.Point(97, 34);
            this.PicBxProfilePicContainer.Name = "PicBxProfilePicContainer";
            this.PicBxProfilePicContainer.Size = new System.Drawing.Size(149, 145);
            this.PicBxProfilePicContainer.TabIndex = 57;
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
            this.PicBxProfilePic.Click += new System.EventHandler(this.PicBxProfilePic_Click_1);
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(416, 622);
            this.LblCancel.Margin = new System.Windows.Forms.Padding(4);
            this.LblCancel.Name = "LblCancel";
            this.LblCancel.Size = new System.Drawing.Size(97, 43);
            this.LblCancel.TabIndex = 56;
            this.LblCancel.Text = "Cancel";
            this.LblCancel.UseVisualStyleBackColor = false;
            this.LblCancel.Click += new System.EventHandler(this.LblCancel_Click_1);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.Location = new System.Drawing.Point(311, 622);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 43);
            this.BtnSave.TabIndex = 55;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click_1);
            // 
            // TxtContactNumber
            // 
            this.TxtContactNumber.ForeColor = System.Drawing.Color.Black;
            this.TxtContactNumber.Location = new System.Drawing.Point(45, 284);
            this.TxtContactNumber.Name = "TxtContactNumber";
            this.TxtContactNumber.Size = new System.Drawing.Size(399, 34);
            this.TxtContactNumber.TabIndex = 66;
            // 
            // TxtEmail
            // 
            this.TxtEmail.ForeColor = System.Drawing.Color.Black;
            this.TxtEmail.Location = new System.Drawing.Point(45, 145);
            this.TxtEmail.Name = "TxtEmail";
            this.TxtEmail.Size = new System.Drawing.Size(399, 34);
            this.TxtEmail.TabIndex = 65;
            // 
            // TxtLastName
            // 
            this.TxtLastName.ForeColor = System.Drawing.Color.Black;
            this.TxtLastName.Location = new System.Drawing.Point(263, 77);
            this.TxtLastName.Name = "TxtLastName";
            this.TxtLastName.Size = new System.Drawing.Size(181, 34);
            this.TxtLastName.TabIndex = 64;
            // 
            // TxtFirstName
            // 
            this.TxtFirstName.ForeColor = System.Drawing.Color.Black;
            this.TxtFirstName.Location = new System.Drawing.Point(45, 77);
            this.TxtFirstName.Name = "TxtFirstName";
            this.TxtFirstName.Size = new System.Drawing.Size(181, 34);
            this.TxtFirstName.TabIndex = 63;
            // 
            // LblContactNumber
            // 
            this.LblContactNumber.AutoSize = true;
            this.LblContactNumber.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblContactNumber.ForeColor = System.Drawing.Color.Black;
            this.LblContactNumber.Location = new System.Drawing.Point(31, 260);
            this.LblContactNumber.Name = "LblContactNumber";
            this.LblContactNumber.Size = new System.Drawing.Size(125, 21);
            this.LblContactNumber.TabIndex = 62;
            this.LblContactNumber.Text = "Contact Number";
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEmail.ForeColor = System.Drawing.Color.Black;
            this.LblEmail.Location = new System.Drawing.Point(31, 121);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(48, 21);
            this.LblEmail.TabIndex = 61;
            this.LblEmail.Text = "Email";
            // 
            // LblLastName
            // 
            this.LblLastName.AutoSize = true;
            this.LblLastName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLastName.ForeColor = System.Drawing.Color.Black;
            this.LblLastName.Location = new System.Drawing.Point(247, 53);
            this.LblLastName.Name = "LblLastName";
            this.LblLastName.Size = new System.Drawing.Size(84, 21);
            this.LblLastName.TabIndex = 60;
            this.LblLastName.Text = "Last Name";
            // 
            // LblFirstName
            // 
            this.LblFirstName.AutoSize = true;
            this.LblFirstName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFirstName.ForeColor = System.Drawing.Color.Black;
            this.LblFirstName.Location = new System.Drawing.Point(31, 53);
            this.LblFirstName.Name = "LblFirstName";
            this.LblFirstName.Size = new System.Drawing.Size(86, 21);
            this.LblFirstName.TabIndex = 59;
            this.LblFirstName.Text = "First Name";
            // 
            // Design1
            // 
            this.Design1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.Design1.Location = new System.Drawing.Point(0, 0);
            this.Design1.Name = "Design1";
            this.Design1.Size = new System.Drawing.Size(546, 85);
            this.Design1.TabIndex = 58;
            // 
            // PicBxValidID
            // 
            this.PicBxValidID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicBxValidID.Image = ((System.Drawing.Image)(resources.GetObject("PicBxValidID.Image")));
            this.PicBxValidID.Location = new System.Drawing.Point(13, 16);
            this.PicBxValidID.Name = "PicBxValidID";
            this.PicBxValidID.Size = new System.Drawing.Size(123, 119);
            this.PicBxValidID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBxValidID.TabIndex = 0;
            this.PicBxValidID.TabStop = false;
            this.TipPicBxProfilePic.SetToolTip(this.PicBxValidID, "Click to edit photo");
            // 
            // TxtAddress
            // 
            this.TxtAddress.ForeColor = System.Drawing.Color.Black;
            this.TxtAddress.Location = new System.Drawing.Point(45, 215);
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.Size = new System.Drawing.Size(399, 34);
            this.TxtAddress.TabIndex = 68;
            // 
            // LblAddress
            // 
            this.LblAddress.AutoSize = true;
            this.LblAddress.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAddress.ForeColor = System.Drawing.Color.Black;
            this.LblAddress.Location = new System.Drawing.Point(31, 191);
            this.LblAddress.Name = "LblAddress";
            this.LblAddress.Size = new System.Drawing.Size(66, 21);
            this.LblAddress.TabIndex = 67;
            this.LblAddress.Text = "Address";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LblFirstName);
            this.groupBox1.Controls.Add(this.TxtAddress);
            this.groupBox1.Controls.Add(this.LblLastName);
            this.groupBox1.Controls.Add(this.LblAddress);
            this.groupBox1.Controls.Add(this.LblEmail);
            this.groupBox1.Controls.Add(this.LblContactNumber);
            this.groupBox1.Controls.Add(this.TxtFirstName);
            this.groupBox1.Controls.Add(this.TxtLastName);
            this.groupBox1.Controls.Add(this.TxtContactNumber);
            this.groupBox1.Controls.Add(this.TxtEmail);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.groupBox1.Location = new System.Drawing.Point(24, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 338);
            this.groupBox1.TabIndex = 72;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Personal Details";
            // 
            // LblAddProfPic
            // 
            this.LblAddProfPic.AutoSize = true;
            this.LblAddProfPic.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAddProfPic.ForeColor = System.Drawing.Color.DimGray;
            this.LblAddProfPic.Location = new System.Drawing.Point(82, 182);
            this.LblAddProfPic.Name = "LblAddProfPic";
            this.LblAddProfPic.Size = new System.Drawing.Size(366, 28);
            this.LblAddProfPic.TabIndex = 73;
            this.LblAddProfPic.Text = "Click picture to edit your photo / ValidID";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 213);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 391);
            this.panel1.TabIndex = 74;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChkBxShowOldPassword);
            this.groupBox2.Controls.Add(this.TxtEntOldPass);
            this.groupBox2.Controls.Add(this.LblEntOldPass);
            this.groupBox2.Controls.Add(this.ChkBxShowNewPassword);
            this.groupBox2.Controls.Add(this.ChkBxShowConfirmNewPassword);
            this.groupBox2.Controls.Add(this.TxtConfirmNewPass);
            this.groupBox2.Controls.Add(this.LblConfirmNewPass);
            this.groupBox2.Controls.Add(this.TxtEntNewPass);
            this.groupBox2.Controls.Add(this.LblUsername);
            this.groupBox2.Controls.Add(this.TxtUsername);
            this.groupBox2.Controls.Add(this.LblEntNewPass);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.groupBox2.Location = new System.Drawing.Point(24, 363);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 365);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Login Details";
            // 
            // ChkBxShowOldPassword
            // 
            this.ChkBxShowOldPassword.AutoSize = true;
            this.ChkBxShowOldPassword.Location = new System.Drawing.Point(426, 159);
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
            this.TxtEntOldPass.Size = new System.Drawing.Size(380, 34);
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
            this.ChkBxShowNewPassword.Location = new System.Drawing.Point(426, 236);
            this.ChkBxShowNewPassword.Name = "ChkBxShowNewPassword";
            this.ChkBxShowNewPassword.Size = new System.Drawing.Size(22, 21);
            this.ChkBxShowNewPassword.TabIndex = 72;
            this.ChkBxShowNewPassword.UseVisualStyleBackColor = true;
            // 
            // ChkBxShowConfirmNewPassword
            // 
            this.ChkBxShowConfirmNewPassword.AutoSize = true;
            this.ChkBxShowConfirmNewPassword.Location = new System.Drawing.Point(426, 312);
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
            this.TxtConfirmNewPass.Size = new System.Drawing.Size(380, 34);
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
            this.TxtEntNewPass.Size = new System.Drawing.Size(380, 34);
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
            this.TxtUsername.Size = new System.Drawing.Size(413, 34);
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
            // PicBxValidIDContainer
            // 
            this.PicBxValidIDContainer.BackColor = System.Drawing.Color.White;
            this.PicBxValidIDContainer.Controls.Add(this.PicBxValidID);
            this.PicBxValidIDContainer.Location = new System.Drawing.Point(286, 34);
            this.PicBxValidIDContainer.Name = "PicBxValidIDContainer";
            this.PicBxValidIDContainer.Size = new System.Drawing.Size(149, 145);
            this.PicBxValidIDContainer.TabIndex = 58;
            // 
            // EditMemberProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(545, 688);
            this.Controls.Add(this.PicBxValidIDContainer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblAddProfPic);
            this.Controls.Add(this.PicBxProfilePicContainer);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.Design1);
            this.Controls.Add(this.BtnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditMemberProfile";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Profile";
            this.PicBxProfilePicContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxValidID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.PicBxValidIDContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PicBxProfilePicContainer;
        private System.Windows.Forms.PictureBox PicBxProfilePic;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.TextBox TxtContactNumber;
        private System.Windows.Forms.TextBox TxtEmail;
        private System.Windows.Forms.TextBox TxtLastName;
        private System.Windows.Forms.TextBox TxtFirstName;
        private System.Windows.Forms.Label LblContactNumber;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.Label LblLastName;
        private System.Windows.Forms.Label LblFirstName;
        private System.Windows.Forms.Panel Design1;
        private System.Windows.Forms.TextBox TxtAddress;
        private System.Windows.Forms.Label LblAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label LblAddProfPic;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ChkBxShowNewPassword;
        private System.Windows.Forms.CheckBox ChkBxShowConfirmNewPassword;
        private System.Windows.Forms.TextBox TxtConfirmNewPass;
        private System.Windows.Forms.Label LblConfirmNewPass;
        private System.Windows.Forms.TextBox TxtEntNewPass;
        private System.Windows.Forms.Label LblUsername;
        private System.Windows.Forms.TextBox TxtUsername;
        private System.Windows.Forms.Label LblEntNewPass;
        private System.Windows.Forms.CheckBox ChkBxShowOldPassword;
        private System.Windows.Forms.TextBox TxtEntOldPass;
        private System.Windows.Forms.Label LblEntOldPass;
        private System.Windows.Forms.Panel PicBxValidIDContainer;
        private System.Windows.Forms.PictureBox PicBxValidID;
    }
}