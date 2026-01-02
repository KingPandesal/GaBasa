namespace LMS.Presentation.Popup.Users
{
    partial class EditUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUser));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CmbBxUserStatus = new System.Windows.Forms.ComboBox();
            this.LblStatus = new System.Windows.Forms.Label();
            this.LblFirstName = new System.Windows.Forms.Label();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblContactNumber = new System.Windows.Forms.Label();
            this.TxtFirstName = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.TxtEmail = new System.Windows.Forms.TextBox();
            this.TxtLastName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LblLastName = new System.Windows.Forms.Label();
            this.TxtContactNumber = new System.Windows.Forms.TextBox();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.PicBxProfilePic = new System.Windows.Forms.PictureBox();
            this.LblAddProfPic = new System.Windows.Forms.Label();
            this.LblCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.PicBxProfilePicContainer = new System.Windows.Forms.Panel();
            this.Design1 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).BeginInit();
            this.PicBxProfilePicContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.CmbBxUserStatus);
            this.groupBox1.Controls.Add(this.LblStatus);
            this.groupBox1.Controls.Add(this.LblFirstName);
            this.groupBox1.Controls.Add(this.LblEmail);
            this.groupBox1.Controls.Add(this.LblContactNumber);
            this.groupBox1.Controls.Add(this.TxtFirstName);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.TxtEmail);
            this.groupBox1.Controls.Add(this.TxtLastName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LblLastName);
            this.groupBox1.Controls.Add(this.TxtContactNumber);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.groupBox1.Location = new System.Drawing.Point(26, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 485);
            this.groupBox1.TabIndex = 78;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Personal Details";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.label3.Location = new System.Drawing.Point(30, 399);
            this.label3.MaximumSize = new System.Drawing.Size(420, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(416, 63);
            this.label3.TabIndex = 71;
            this.label3.Text = "*Turning user\'s status to Inactive will archive their account and they won\'t be a" +
    "ble to login anymore. You can still change this later.";
            // 
            // CmbBxUserStatus
            // 
            this.CmbBxUserStatus.ForeColor = System.Drawing.Color.Black;
            this.CmbBxUserStatus.FormattingEnabled = true;
            this.CmbBxUserStatus.Items.AddRange(new object[] {
            "Active",
            "Inactive"});
            this.CmbBxUserStatus.Location = new System.Drawing.Point(31, 356);
            this.CmbBxUserStatus.Name = "CmbBxUserStatus";
            this.CmbBxUserStatus.Size = new System.Drawing.Size(413, 36);
            this.CmbBxUserStatus.TabIndex = 70;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStatus.ForeColor = System.Drawing.Color.Black;
            this.LblStatus.Location = new System.Drawing.Point(17, 332);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(52, 21);
            this.LblStatus.TabIndex = 69;
            this.LblStatus.Text = "Status";
            // 
            // LblFirstName
            // 
            this.LblFirstName.AutoSize = true;
            this.LblFirstName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFirstName.ForeColor = System.Drawing.Color.Black;
            this.LblFirstName.Location = new System.Drawing.Point(17, 43);
            this.LblFirstName.Name = "LblFirstName";
            this.LblFirstName.Size = new System.Drawing.Size(86, 21);
            this.LblFirstName.TabIndex = 59;
            this.LblFirstName.Text = "First Name";
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEmail.ForeColor = System.Drawing.Color.Black;
            this.LblEmail.Location = new System.Drawing.Point(17, 183);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(48, 21);
            this.LblEmail.TabIndex = 61;
            this.LblEmail.Text = "Email";
            // 
            // LblContactNumber
            // 
            this.LblContactNumber.AutoSize = true;
            this.LblContactNumber.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblContactNumber.ForeColor = System.Drawing.Color.Black;
            this.LblContactNumber.Location = new System.Drawing.Point(17, 254);
            this.LblContactNumber.Name = "LblContactNumber";
            this.LblContactNumber.Size = new System.Drawing.Size(125, 21);
            this.LblContactNumber.TabIndex = 62;
            this.LblContactNumber.Text = "Contact Number";
            // 
            // TxtFirstName
            // 
            this.TxtFirstName.ForeColor = System.Drawing.Color.Black;
            this.TxtFirstName.Location = new System.Drawing.Point(31, 67);
            this.TxtFirstName.Name = "TxtFirstName";
            this.TxtFirstName.Size = new System.Drawing.Size(192, 34);
            this.TxtFirstName.TabIndex = 63;
            // 
            // comboBox1
            // 
            this.comboBox1.ForeColor = System.Drawing.Color.Black;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Librarian / Admin",
            "Library Staff"});
            this.comboBox1.Location = new System.Drawing.Point(31, 135);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(413, 36);
            this.comboBox1.TabIndex = 68;
            // 
            // TxtEmail
            // 
            this.TxtEmail.ForeColor = System.Drawing.Color.Black;
            this.TxtEmail.Location = new System.Drawing.Point(31, 207);
            this.TxtEmail.Name = "TxtEmail";
            this.TxtEmail.Size = new System.Drawing.Size(413, 34);
            this.TxtEmail.TabIndex = 65;
            // 
            // TxtLastName
            // 
            this.TxtLastName.ForeColor = System.Drawing.Color.Black;
            this.TxtLastName.Location = new System.Drawing.Point(251, 67);
            this.TxtLastName.Name = "TxtLastName";
            this.TxtLastName.Size = new System.Drawing.Size(193, 34);
            this.TxtLastName.TabIndex = 64;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(17, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 21);
            this.label1.TabIndex = 67;
            this.label1.Text = "Role";
            // 
            // LblLastName
            // 
            this.LblLastName.AutoSize = true;
            this.LblLastName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLastName.ForeColor = System.Drawing.Color.Black;
            this.LblLastName.Location = new System.Drawing.Point(235, 43);
            this.LblLastName.Name = "LblLastName";
            this.LblLastName.Size = new System.Drawing.Size(84, 21);
            this.LblLastName.TabIndex = 60;
            this.LblLastName.Text = "Last Name";
            // 
            // TxtContactNumber
            // 
            this.TxtContactNumber.ForeColor = System.Drawing.Color.Black;
            this.TxtContactNumber.Location = new System.Drawing.Point(31, 278);
            this.TxtContactNumber.Name = "TxtContactNumber";
            this.TxtContactNumber.Size = new System.Drawing.Size(413, 34);
            this.TxtContactNumber.TabIndex = 66;
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
            // 
            // LblAddProfPic
            // 
            this.LblAddProfPic.AutoSize = true;
            this.LblAddProfPic.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAddProfPic.ForeColor = System.Drawing.Color.DimGray;
            this.LblAddProfPic.Location = new System.Drawing.Point(148, 182);
            this.LblAddProfPic.Name = "LblAddProfPic";
            this.LblAddProfPic.Size = new System.Drawing.Size(241, 28);
            this.LblAddProfPic.TabIndex = 77;
            this.LblAddProfPic.Text = "Click picture to edit photo";
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(412, 679);
            this.LblCancel.Margin = new System.Windows.Forms.Padding(4);
            this.LblCancel.Name = "LblCancel";
            this.LblCancel.Size = new System.Drawing.Size(97, 43);
            this.LblCancel.TabIndex = 74;
            this.LblCancel.Text = "Cancel";
            this.LblCancel.UseVisualStyleBackColor = false;
            this.LblCancel.Click += new System.EventHandler(this.LblCancel_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.Location = new System.Drawing.Point(307, 679);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 43);
            this.BtnSave.TabIndex = 73;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = false;
            // 
            // PicBxProfilePicContainer
            // 
            this.PicBxProfilePicContainer.BackColor = System.Drawing.Color.White;
            this.PicBxProfilePicContainer.Controls.Add(this.PicBxProfilePic);
            this.PicBxProfilePicContainer.Location = new System.Drawing.Point(193, 34);
            this.PicBxProfilePicContainer.Name = "PicBxProfilePicContainer";
            this.PicBxProfilePicContainer.Size = new System.Drawing.Size(149, 145);
            this.PicBxProfilePicContainer.TabIndex = 75;
            // 
            // Design1
            // 
            this.Design1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.Design1.Location = new System.Drawing.Point(0, 0);
            this.Design1.Name = "Design1";
            this.Design1.Size = new System.Drawing.Size(546, 85);
            this.Design1.TabIndex = 76;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(6, 223);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 437);
            this.panel1.TabIndex = 79;
            // 
            // EditUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(545, 737);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblAddProfPic);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.PicBxProfilePicContainer);
            this.Controls.Add(this.Design1);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit user";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).EndInit();
            this.PicBxProfilePicContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label LblFirstName;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.Label LblContactNumber;
        private System.Windows.Forms.TextBox TxtFirstName;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox TxtEmail;
        private System.Windows.Forms.TextBox TxtLastName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblLastName;
        private System.Windows.Forms.TextBox TxtContactNumber;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
        private System.Windows.Forms.PictureBox PicBxProfilePic;
        private System.Windows.Forms.Label LblAddProfPic;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Panel PicBxProfilePicContainer;
        private System.Windows.Forms.Panel Design1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CmbBxUserStatus;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Panel panel1;
    }
}