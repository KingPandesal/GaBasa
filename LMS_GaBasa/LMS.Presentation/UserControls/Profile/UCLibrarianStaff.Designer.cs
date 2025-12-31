namespace LMS.Presentation.UserControls.Profile
{
    partial class UCLibrarianStaff
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCLibrarianStaff));
            this.Design2 = new ReaLTaiizor.Controls.LostBorderPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblFullname = new System.Windows.Forms.Label();
            this.LblStatus = new System.Windows.Forms.Label();
            this.LblRole = new System.Windows.Forms.Label();
            this.TxtContact = new ReaLTaiizor.Controls.HopeRichTextBox();
            this.LblContact = new System.Windows.Forms.Label();
            this.PicBxProfilePicContainer = new System.Windows.Forms.Panel();
            this.PicBxProfilePic = new System.Windows.Forms.PictureBox();
            this.Design1 = new System.Windows.Forms.Panel();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblIDNumber = new System.Windows.Forms.Label();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.Design2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PicBxProfilePicContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).BeginInit();
            this.SuspendLayout();
            // 
            // Design2
            // 
            this.Design2.BackColor = System.Drawing.Color.White;
            this.Design2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.Design2.Controls.Add(this.panel1);
            this.Design2.Controls.Add(this.TxtContact);
            this.Design2.Controls.Add(this.LblContact);
            this.Design2.Controls.Add(this.PicBxProfilePicContainer);
            this.Design2.Controls.Add(this.Design1);
            this.Design2.Controls.Add(this.LblEmail);
            this.Design2.Controls.Add(this.LblIDNumber);
            this.Design2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Design2.ForeColor = System.Drawing.Color.Black;
            this.Design2.Location = new System.Drawing.Point(50, 38);
            this.Design2.Name = "Design2";
            this.Design2.Padding = new System.Windows.Forms.Padding(5);
            this.Design2.ShowText = true;
            this.Design2.Size = new System.Drawing.Size(1480, 758);
            this.Design2.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LblFullname);
            this.panel1.Controls.Add(this.LblStatus);
            this.panel1.Controls.Add(this.LblRole);
            this.panel1.Location = new System.Drawing.Point(322, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 53);
            this.panel1.TabIndex = 14;
            // 
            // LblFullname
            // 
            this.LblFullname.AutoSize = true;
            this.LblFullname.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LblFullname.Location = new System.Drawing.Point(-9, -1);
            this.LblFullname.Name = "LblFullname";
            this.LblFullname.Size = new System.Drawing.Size(190, 54);
            this.LblFullname.TabIndex = 1;
            this.LblFullname.Text = "Zy Manti";
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.BackColor = System.Drawing.Color.LimeGreen;
            this.LblStatus.ForeColor = System.Drawing.Color.White;
            this.LblStatus.Location = new System.Drawing.Point(305, 11);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(79, 32);
            this.LblStatus.TabIndex = 6;
            this.LblStatus.Text = "Active";
            // 
            // LblRole
            // 
            this.LblRole.AutoSize = true;
            this.LblRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.LblRole.ForeColor = System.Drawing.Color.White;
            this.LblRole.Location = new System.Drawing.Point(187, 11);
            this.LblRole.Name = "LblRole";
            this.LblRole.Size = new System.Drawing.Size(105, 32);
            this.LblRole.TabIndex = 5;
            this.LblRole.Text = "Librarian";
            // 
            // TxtContact
            // 
            this.TxtContact.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.TxtContact.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TxtContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.TxtContact.Hint = "";
            this.TxtContact.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.TxtContact.Location = new System.Drawing.Point(85, 365);
            this.TxtContact.MaxLength = 32767;
            this.TxtContact.Multiline = true;
            this.TxtContact.Name = "TxtContact";
            this.TxtContact.PasswordChar = '\0';
            this.TxtContact.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtContact.SelectedText = "";
            this.TxtContact.SelectionLength = 0;
            this.TxtContact.SelectionStart = 0;
            this.TxtContact.Size = new System.Drawing.Size(269, 52);
            this.TxtContact.TabIndex = 13;
            this.TxtContact.TabStop = false;
            this.TxtContact.Text = "0929 211  9698";
            this.TxtContact.UseSystemPasswordChar = false;
            // 
            // LblContact
            // 
            this.LblContact.AutoSize = true;
            this.LblContact.Location = new System.Drawing.Point(66, 330);
            this.LblContact.Name = "LblContact";
            this.LblContact.Size = new System.Drawing.Size(96, 32);
            this.LblContact.TabIndex = 12;
            this.LblContact.Text = "Contact";
            // 
            // PicBxProfilePicContainer
            // 
            this.PicBxProfilePicContainer.BackColor = System.Drawing.Color.White;
            this.PicBxProfilePicContainer.Controls.Add(this.PicBxProfilePic);
            this.PicBxProfilePicContainer.Location = new System.Drawing.Point(72, 55);
            this.PicBxProfilePicContainer.Name = "PicBxProfilePicContainer";
            this.PicBxProfilePicContainer.Size = new System.Drawing.Size(238, 232);
            this.PicBxProfilePicContainer.TabIndex = 0;
            // 
            // PicBxProfilePic
            // 
            this.PicBxProfilePic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicBxProfilePic.Image = ((System.Drawing.Image)(resources.GetObject("PicBxProfilePic.Image")));
            this.PicBxProfilePic.Location = new System.Drawing.Point(13, 15);
            this.PicBxProfilePic.Name = "PicBxProfilePic";
            this.PicBxProfilePic.Size = new System.Drawing.Size(212, 206);
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
            this.Design1.Size = new System.Drawing.Size(1480, 112);
            this.Design1.TabIndex = 9;
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Location = new System.Drawing.Point(316, 234);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(457, 32);
            this.LblEmail.TabIndex = 3;
            this.LblEmail.Text = "z.madayag.141776.tc@umindanao.edu.ph";
            // 
            // LblIDNumber
            // 
            this.LblIDNumber.AutoSize = true;
            this.LblIDNumber.Location = new System.Drawing.Point(316, 202);
            this.LblIDNumber.Name = "LblIDNumber";
            this.LblIDNumber.Size = new System.Drawing.Size(92, 32);
            this.LblIDNumber.TabIndex = 2;
            this.LblIDNumber.Text = "141776";
            // 
            // UCLibrarianStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Design2);
            this.Name = "UCLibrarianStaff";
            this.Size = new System.Drawing.Size(1580, 832);
            this.Design2.ResumeLayout(false);
            this.Design2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PicBxProfilePicContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.LostBorderPanel Design2;
        private ReaLTaiizor.Controls.HopeRichTextBox TxtContact;
        private System.Windows.Forms.Label LblContact;
        private System.Windows.Forms.Panel PicBxProfilePicContainer;
        private System.Windows.Forms.PictureBox PicBxProfilePic;
        private System.Windows.Forms.Panel Design1;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label LblRole;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.Label LblIDNumber;
        private System.Windows.Forms.Label LblFullname;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
    }
}
