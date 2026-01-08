namespace LMS.Presentation.Forms
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.lblH1 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.TxtUsername = new System.Windows.Forms.TextBox();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.CmbbxSelectUserType = new System.Windows.Forms.ComboBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ChkbxShowPassword = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblH1
            // 
            this.lblH1.AutoSize = true;
            this.lblH1.BackColor = System.Drawing.Color.Transparent;
            this.lblH1.Font = new System.Drawing.Font("Arial Unicode MS", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblH1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblH1.Location = new System.Drawing.Point(41, 44);
            this.lblH1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblH1.Name = "lblH1";
            this.lblH1.Size = new System.Drawing.Size(144, 43);
            this.lblH1.TabIndex = 17;
            this.lblH1.Text = "GaBasa.";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.BackColor = System.Drawing.Color.Transparent;
            this.lbl3.Font = new System.Drawing.Font("Arial Unicode MS", 20F);
            this.lbl3.ForeColor = System.Drawing.Color.White;
            this.lbl3.Location = new System.Drawing.Point(41, 208);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(467, 54);
            this.lbl3.TabIndex = 16;
            this.lbl3.Text = "Pickup where you left off.";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.BackColor = System.Drawing.Color.Transparent;
            this.lbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl2.ForeColor = System.Drawing.Color.White;
            this.lbl2.Location = new System.Drawing.Point(45, 180);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(223, 25);
            this.lbl2.TabIndex = 15;
            this.lbl2.Text = "WELCOME BACK, GA!";
            // 
            // TxtUsername
            // 
            this.TxtUsername.Location = new System.Drawing.Point(49, 362);
            this.TxtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.TxtUsername.Name = "TxtUsername";
            this.TxtUsername.Size = new System.Drawing.Size(304, 26);
            this.TxtUsername.TabIndex = 11;
            this.TxtUsername.Text = "Enter your username";
            // 
            // TxtPassword
            // 
            this.TxtPassword.Location = new System.Drawing.Point(49, 413);
            this.TxtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.Size = new System.Drawing.Size(275, 26);
            this.TxtPassword.TabIndex = 12;
            this.TxtPassword.Text = "Enter your password";
            // 
            // CmbbxSelectUserType
            // 
            this.CmbbxSelectUserType.FormattingEnabled = true;
            this.CmbbxSelectUserType.Items.AddRange(new object[] {
            "Librarian / Admin",
            "Library Staff",
            "Library Member"});
            this.CmbbxSelectUserType.Location = new System.Drawing.Point(49, 462);
            this.CmbbxSelectUserType.Margin = new System.Windows.Forms.Padding(4);
            this.CmbbxSelectUserType.Name = "CmbbxSelectUserType";
            this.CmbbxSelectUserType.Size = new System.Drawing.Size(304, 28);
            this.CmbbxSelectUserType.TabIndex = 13;
            this.CmbbxSelectUserType.Text = "Select User Type";
            // 
            // BtnLogin
            // 
            this.BtnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnLogin.FlatAppearance.BorderSize = 0;
            this.BtnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLogin.ForeColor = System.Drawing.Color.White;
            this.BtnLogin.Location = new System.Drawing.Point(49, 517);
            this.BtnLogin.Margin = new System.Windows.Forms.Padding(4);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(305, 43);
            this.BtnLogin.TabIndex = 14;
            this.BtnLogin.Text = "Login";
            this.BtnLogin.UseVisualStyleBackColor = false;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(927, 566);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 101);
            this.pictureBox2.TabIndex = 19;
            this.pictureBox2.TabStop = false;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.BackColor = System.Drawing.Color.Transparent;
            this.lbl1.ForeColor = System.Drawing.Color.White;
            this.lbl1.Location = new System.Drawing.Point(751, 59);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(276, 20);
            this.lbl1.TabIndex = 20;
            this.lbl1.Text = "UM Tagum College - Visayan Campus";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1113, 553);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(107, 116);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // ChkbxShowPassword
            // 
            this.ChkbxShowPassword.AutoSize = true;
            this.ChkbxShowPassword.BackColor = System.Drawing.Color.Transparent;
            this.ChkbxShowPassword.Location = new System.Drawing.Point(331, 417);
            this.ChkbxShowPassword.Name = "ChkbxShowPassword";
            this.ChkbxShowPassword.Size = new System.Drawing.Size(22, 21);
            this.ChkbxShowPassword.TabIndex = 22;
            this.ChkbxShowPassword.UseVisualStyleBackColor = false;
            this.ChkbxShowPassword.CheckedChanged += new System.EventHandler(this.ChkbxShowPassword_CheckedChanged);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1058, 744);
            this.Controls.Add(this.ChkbxShowPassword);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblH1);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.TxtUsername);
            this.Controls.Add(this.TxtPassword);
            this.Controls.Add(this.CmbbxSelectUserType);
            this.Controls.Add(this.BtnLogin);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblH1;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.TextBox TxtUsername;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.ComboBox CmbbxSelectUserType;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox ChkbxShowPassword;
    }
}