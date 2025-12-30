namespace LMS.Presentation.UserControls.Profile
{
    partial class UCMemberProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMemberProfile));
            this.Design2 = new ReaLTaiizor.Controls.LostBorderPanel();
            this.LblMemberStatus = new System.Windows.Forms.Label();
            this.LblMemberType = new System.Windows.Forms.Label();
            this.PnlRegExpDate = new ReaLTaiizor.Controls.Panel();
            this.LblActualExpDate = new System.Windows.Forms.Label();
            this.LblExpDate = new System.Windows.Forms.Label();
            this.LblActualRegDate = new System.Windows.Forms.Label();
            this.LblRegDate = new System.Windows.Forms.Label();
            this.PnlMemberPrivilege = new System.Windows.Forms.Panel();
            this.LblNumberFineRate = new System.Windows.Forms.Label();
            this.LblBoolReservationPrivilege = new System.Windows.Forms.Label();
            this.LblNumberRenewalLimit = new System.Windows.Forms.Label();
            this.LblNumberBorrowingPeriod = new System.Windows.Forms.Label();
            this.LblNumberMaxBooksAllowed = new System.Windows.Forms.Label();
            this.LblActualAccountStanding = new System.Windows.Forms.Label();
            this.LblAccontStanding = new System.Windows.Forms.Label();
            this.LblFineRate = new System.Windows.Forms.Label();
            this.LblReservationPrivilege = new System.Windows.Forms.Label();
            this.LblRenewalLimit = new System.Windows.Forms.Label();
            this.LblBorrowingPeriod = new System.Windows.Forms.Label();
            this.LblMaxBooksAllowed = new System.Windows.Forms.Label();
            this.LblMemberPrevileges = new System.Windows.Forms.Label();
            this.TxtContact = new ReaLTaiizor.Controls.HopeRichTextBox();
            this.LblContact = new System.Windows.Forms.Label();
            this.TxtAddress = new ReaLTaiizor.Controls.HopeRichTextBox();
            this.LblAddress = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PicBxProfilePic = new System.Windows.Forms.PictureBox();
            this.Design1 = new System.Windows.Forms.Panel();
            this.LblStatus = new System.Windows.Forms.Label();
            this.LblRole = new System.Windows.Forms.Label();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblIDNumber = new System.Windows.Forms.Label();
            this.LblFullname = new System.Windows.Forms.Label();
            this.Design2.SuspendLayout();
            this.PnlRegExpDate.SuspendLayout();
            this.PnlMemberPrivilege.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).BeginInit();
            this.SuspendLayout();
            // 
            // Design2
            // 
            this.Design2.BackColor = System.Drawing.Color.White;
            this.Design2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.Design2.Controls.Add(this.LblMemberStatus);
            this.Design2.Controls.Add(this.LblMemberType);
            this.Design2.Controls.Add(this.PnlRegExpDate);
            this.Design2.Controls.Add(this.PnlMemberPrivilege);
            this.Design2.Controls.Add(this.TxtContact);
            this.Design2.Controls.Add(this.LblContact);
            this.Design2.Controls.Add(this.TxtAddress);
            this.Design2.Controls.Add(this.LblAddress);
            this.Design2.Controls.Add(this.panel2);
            this.Design2.Controls.Add(this.Design1);
            this.Design2.Controls.Add(this.LblStatus);
            this.Design2.Controls.Add(this.LblRole);
            this.Design2.Controls.Add(this.LblEmail);
            this.Design2.Controls.Add(this.LblIDNumber);
            this.Design2.Controls.Add(this.LblFullname);
            this.Design2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Design2.ForeColor = System.Drawing.Color.Black;
            this.Design2.Location = new System.Drawing.Point(50, 38);
            this.Design2.Name = "Design2";
            this.Design2.Padding = new System.Windows.Forms.Padding(5);
            this.Design2.ShowText = true;
            this.Design2.Size = new System.Drawing.Size(1480, 758);
            this.Design2.TabIndex = 14;
            // 
            // LblMemberStatus
            // 
            this.LblMemberStatus.AutoSize = true;
            this.LblMemberStatus.BackColor = System.Drawing.Color.LimeGreen;
            this.LblMemberStatus.ForeColor = System.Drawing.Color.White;
            this.LblMemberStatus.Location = new System.Drawing.Point(829, 160);
            this.LblMemberStatus.Name = "LblMemberStatus";
            this.LblMemberStatus.Size = new System.Drawing.Size(79, 32);
            this.LblMemberStatus.TabIndex = 21;
            this.LblMemberStatus.Text = "Active";
            // 
            // LblMemberType
            // 
            this.LblMemberType.AutoSize = true;
            this.LblMemberType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.LblMemberType.ForeColor = System.Drawing.Color.White;
            this.LblMemberType.Location = new System.Drawing.Point(617, 160);
            this.LblMemberType.Name = "LblMemberType";
            this.LblMemberType.Size = new System.Drawing.Size(105, 32);
            this.LblMemberType.TabIndex = 20;
            this.LblMemberType.Text = "Librarian";
            // 
            // PnlRegExpDate
            // 
            this.PnlRegExpDate.BackColor = System.Drawing.Color.White;
            this.PnlRegExpDate.Controls.Add(this.LblActualExpDate);
            this.PnlRegExpDate.Controls.Add(this.LblExpDate);
            this.PnlRegExpDate.Controls.Add(this.LblActualRegDate);
            this.PnlRegExpDate.Controls.Add(this.LblRegDate);
            this.PnlRegExpDate.EdgeColor = System.Drawing.Color.White;
            this.PnlRegExpDate.Location = new System.Drawing.Point(59, 584);
            this.PnlRegExpDate.Name = "PnlRegExpDate";
            this.PnlRegExpDate.Padding = new System.Windows.Forms.Padding(5);
            this.PnlRegExpDate.Size = new System.Drawing.Size(637, 129);
            this.PnlRegExpDate.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.PnlRegExpDate.TabIndex = 19;
            this.PnlRegExpDate.Text = "panel1";
            // 
            // LblActualExpDate
            // 
            this.LblActualExpDate.AutoSize = true;
            this.LblActualExpDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.LblActualExpDate.Location = new System.Drawing.Point(398, 55);
            this.LblActualExpDate.Name = "LblActualExpDate";
            this.LblActualExpDate.Size = new System.Drawing.Size(203, 30);
            this.LblActualExpDate.TabIndex = 17;
            this.LblActualExpDate.Text = "December 19, 2025";
            // 
            // LblExpDate
            // 
            this.LblExpDate.AutoSize = true;
            this.LblExpDate.Location = new System.Drawing.Point(384, 16);
            this.LblExpDate.Name = "LblExpDate";
            this.LblExpDate.Size = new System.Drawing.Size(176, 32);
            this.LblExpDate.TabIndex = 16;
            this.LblExpDate.Text = "Expiration Date";
            // 
            // LblActualRegDate
            // 
            this.LblActualRegDate.AutoSize = true;
            this.LblActualRegDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.LblActualRegDate.Location = new System.Drawing.Point(21, 55);
            this.LblActualRegDate.Name = "LblActualRegDate";
            this.LblActualRegDate.Size = new System.Drawing.Size(157, 30);
            this.LblActualRegDate.TabIndex = 15;
            this.LblActualRegDate.Text = "August 5, 2025";
            // 
            // LblRegDate
            // 
            this.LblRegDate.AutoSize = true;
            this.LblRegDate.Location = new System.Drawing.Point(7, 16);
            this.LblRegDate.Name = "LblRegDate";
            this.LblRegDate.Size = new System.Drawing.Size(197, 32);
            this.LblRegDate.TabIndex = 14;
            this.LblRegDate.Text = "Registration Date";
            // 
            // PnlMemberPrivilege
            // 
            this.PnlMemberPrivilege.Controls.Add(this.LblNumberFineRate);
            this.PnlMemberPrivilege.Controls.Add(this.LblBoolReservationPrivilege);
            this.PnlMemberPrivilege.Controls.Add(this.LblNumberRenewalLimit);
            this.PnlMemberPrivilege.Controls.Add(this.LblNumberBorrowingPeriod);
            this.PnlMemberPrivilege.Controls.Add(this.LblNumberMaxBooksAllowed);
            this.PnlMemberPrivilege.Controls.Add(this.LblActualAccountStanding);
            this.PnlMemberPrivilege.Controls.Add(this.LblAccontStanding);
            this.PnlMemberPrivilege.Controls.Add(this.LblFineRate);
            this.PnlMemberPrivilege.Controls.Add(this.LblReservationPrivilege);
            this.PnlMemberPrivilege.Controls.Add(this.LblRenewalLimit);
            this.PnlMemberPrivilege.Controls.Add(this.LblBorrowingPeriod);
            this.PnlMemberPrivilege.Controls.Add(this.LblMaxBooksAllowed);
            this.PnlMemberPrivilege.Controls.Add(this.LblMemberPrevileges);
            this.PnlMemberPrivilege.Location = new System.Drawing.Point(817, 317);
            this.PnlMemberPrivilege.Name = "PnlMemberPrivilege";
            this.PnlMemberPrivilege.Size = new System.Drawing.Size(624, 382);
            this.PnlMemberPrivilege.TabIndex = 18;
            // 
            // LblNumberFineRate
            // 
            this.LblNumberFineRate.AutoSize = true;
            this.LblNumberFineRate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblNumberFineRate.Location = new System.Drawing.Point(507, 243);
            this.LblNumberFineRate.Name = "LblNumberFineRate";
            this.LblNumberFineRate.Size = new System.Drawing.Size(100, 28);
            this.LblNumberFineRate.TabIndex = 23;
            this.LblNumberFineRate.Text = "P 10 / day";
            // 
            // LblBoolReservationPrivilege
            // 
            this.LblBoolReservationPrivilege.AutoSize = true;
            this.LblBoolReservationPrivilege.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblBoolReservationPrivilege.Location = new System.Drawing.Point(507, 200);
            this.LblBoolReservationPrivilege.Name = "LblBoolReservationPrivilege";
            this.LblBoolReservationPrivilege.Size = new System.Drawing.Size(39, 28);
            this.LblBoolReservationPrivilege.TabIndex = 22;
            this.LblBoolReservationPrivilege.Text = "Yes";
            // 
            // LblNumberRenewalLimit
            // 
            this.LblNumberRenewalLimit.AutoSize = true;
            this.LblNumberRenewalLimit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblNumberRenewalLimit.Location = new System.Drawing.Point(507, 158);
            this.LblNumberRenewalLimit.Name = "LblNumberRenewalLimit";
            this.LblNumberRenewalLimit.Size = new System.Drawing.Size(82, 28);
            this.LblNumberRenewalLimit.TabIndex = 21;
            this.LblNumberRenewalLimit.Text = "2 books";
            // 
            // LblNumberBorrowingPeriod
            // 
            this.LblNumberBorrowingPeriod.AutoSize = true;
            this.LblNumberBorrowingPeriod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblNumberBorrowingPeriod.Location = new System.Drawing.Point(507, 115);
            this.LblNumberBorrowingPeriod.Name = "LblNumberBorrowingPeriod";
            this.LblNumberBorrowingPeriod.Size = new System.Drawing.Size(68, 28);
            this.LblNumberBorrowingPeriod.TabIndex = 20;
            this.LblNumberBorrowingPeriod.Text = "5 days";
            // 
            // LblNumberMaxBooksAllowed
            // 
            this.LblNumberMaxBooksAllowed.AutoSize = true;
            this.LblNumberMaxBooksAllowed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblNumberMaxBooksAllowed.Location = new System.Drawing.Point(507, 73);
            this.LblNumberMaxBooksAllowed.Name = "LblNumberMaxBooksAllowed";
            this.LblNumberMaxBooksAllowed.Size = new System.Drawing.Size(82, 28);
            this.LblNumberMaxBooksAllowed.TabIndex = 19;
            this.LblNumberMaxBooksAllowed.Text = "5 books";
            // 
            // LblActualAccountStanding
            // 
            this.LblActualAccountStanding.AutoSize = true;
            this.LblActualAccountStanding.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblActualAccountStanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LblActualAccountStanding.Location = new System.Drawing.Point(204, 279);
            this.LblActualAccountStanding.Name = "LblActualAccountStanding";
            this.LblActualAccountStanding.Size = new System.Drawing.Size(88, 38);
            this.LblActualAccountStanding.TabIndex = 18;
            this.LblActualAccountStanding.Text = "Good";
            // 
            // LblAccontStanding
            // 
            this.LblAccontStanding.AutoSize = true;
            this.LblAccontStanding.Location = new System.Drawing.Point(3, 283);
            this.LblAccontStanding.Name = "LblAccontStanding";
            this.LblAccontStanding.Size = new System.Drawing.Size(207, 32);
            this.LblAccontStanding.TabIndex = 17;
            this.LblAccontStanding.Text = "Account Standing:";
            // 
            // LblFineRate
            // 
            this.LblFineRate.AutoSize = true;
            this.LblFineRate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblFineRate.Location = new System.Drawing.Point(33, 243);
            this.LblFineRate.Name = "LblFineRate";
            this.LblFineRate.Size = new System.Drawing.Size(92, 28);
            this.LblFineRate.TabIndex = 16;
            this.LblFineRate.Text = "Fine Rate";
            // 
            // LblReservationPrivilege
            // 
            this.LblReservationPrivilege.AutoSize = true;
            this.LblReservationPrivilege.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblReservationPrivilege.Location = new System.Drawing.Point(33, 200);
            this.LblReservationPrivilege.Name = "LblReservationPrivilege";
            this.LblReservationPrivilege.Size = new System.Drawing.Size(193, 28);
            this.LblReservationPrivilege.TabIndex = 15;
            this.LblReservationPrivilege.Text = "Reservation Privilege";
            // 
            // LblRenewalLimit
            // 
            this.LblRenewalLimit.AutoSize = true;
            this.LblRenewalLimit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblRenewalLimit.Location = new System.Drawing.Point(33, 158);
            this.LblRenewalLimit.Name = "LblRenewalLimit";
            this.LblRenewalLimit.Size = new System.Drawing.Size(131, 28);
            this.LblRenewalLimit.TabIndex = 14;
            this.LblRenewalLimit.Text = "Renewal Limit";
            // 
            // LblBorrowingPeriod
            // 
            this.LblBorrowingPeriod.AutoSize = true;
            this.LblBorrowingPeriod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblBorrowingPeriod.Location = new System.Drawing.Point(33, 115);
            this.LblBorrowingPeriod.Name = "LblBorrowingPeriod";
            this.LblBorrowingPeriod.Size = new System.Drawing.Size(164, 28);
            this.LblBorrowingPeriod.TabIndex = 13;
            this.LblBorrowingPeriod.Text = "Borrowing Period";
            // 
            // LblMaxBooksAllowed
            // 
            this.LblMaxBooksAllowed.AutoSize = true;
            this.LblMaxBooksAllowed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblMaxBooksAllowed.Location = new System.Drawing.Point(33, 73);
            this.LblMaxBooksAllowed.Name = "LblMaxBooksAllowed";
            this.LblMaxBooksAllowed.Size = new System.Drawing.Size(183, 28);
            this.LblMaxBooksAllowed.TabIndex = 12;
            this.LblMaxBooksAllowed.Text = "Max Books Allowed";
            // 
            // LblMemberPrevileges
            // 
            this.LblMemberPrevileges.AutoSize = true;
            this.LblMemberPrevileges.Location = new System.Drawing.Point(3, 28);
            this.LblMemberPrevileges.Name = "LblMemberPrevileges";
            this.LblMemberPrevileges.Size = new System.Drawing.Size(213, 32);
            this.LblMemberPrevileges.TabIndex = 11;
            this.LblMemberPrevileges.Text = "Member Privileges";
            // 
            // TxtContact
            // 
            this.TxtContact.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.TxtContact.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TxtContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.TxtContact.Hint = "";
            this.TxtContact.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.TxtContact.Location = new System.Drawing.Point(85, 505);
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
            this.LblContact.Location = new System.Drawing.Point(66, 470);
            this.LblContact.Name = "LblContact";
            this.LblContact.Size = new System.Drawing.Size(96, 32);
            this.LblContact.TabIndex = 12;
            this.LblContact.Text = "Contact";
            // 
            // TxtAddress
            // 
            this.TxtAddress.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.TxtAddress.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TxtAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.TxtAddress.Hint = "";
            this.TxtAddress.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.TxtAddress.Location = new System.Drawing.Point(85, 380);
            this.TxtAddress.MaxLength = 32767;
            this.TxtAddress.Multiline = true;
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.PasswordChar = '\0';
            this.TxtAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TxtAddress.SelectedText = "";
            this.TxtAddress.SelectionLength = 0;
            this.TxtAddress.SelectionStart = 0;
            this.TxtAddress.Size = new System.Drawing.Size(612, 54);
            this.TxtAddress.TabIndex = 11;
            this.TxtAddress.TabStop = false;
            this.TxtAddress.Text = "Prk. Makugihon, Brgy. Cuambog, Mabini, DDO";
            this.TxtAddress.UseSystemPasswordChar = false;
            // 
            // LblAddress
            // 
            this.LblAddress.AutoSize = true;
            this.LblAddress.Location = new System.Drawing.Point(66, 345);
            this.LblAddress.Name = "LblAddress";
            this.LblAddress.Size = new System.Drawing.Size(98, 32);
            this.LblAddress.TabIndex = 10;
            this.LblAddress.Text = "Address";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.PicBxProfilePic);
            this.panel2.Location = new System.Drawing.Point(72, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(238, 232);
            this.panel2.TabIndex = 0;
            // 
            // PicBxProfilePic
            // 
            this.PicBxProfilePic.Image = ((System.Drawing.Image)(resources.GetObject("PicBxProfilePic.Image")));
            this.PicBxProfilePic.Location = new System.Drawing.Point(13, 15);
            this.PicBxProfilePic.Name = "PicBxProfilePic";
            this.PicBxProfilePic.Size = new System.Drawing.Size(212, 206);
            this.PicBxProfilePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBxProfilePic.TabIndex = 0;
            this.PicBxProfilePic.TabStop = false;
            // 
            // Design1
            // 
            this.Design1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.Design1.Location = new System.Drawing.Point(0, 0);
            this.Design1.Name = "Design1";
            this.Design1.Size = new System.Drawing.Size(1480, 112);
            this.Design1.TabIndex = 9;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.BackColor = System.Drawing.Color.LimeGreen;
            this.LblStatus.ForeColor = System.Drawing.Color.White;
            this.LblStatus.Location = new System.Drawing.Point(734, 160);
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
            this.LblRole.Location = new System.Drawing.Point(501, 160);
            this.LblRole.Name = "LblRole";
            this.LblRole.Size = new System.Drawing.Size(105, 32);
            this.LblRole.TabIndex = 5;
            this.LblRole.Text = "Librarian";
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
            // LblFullname
            // 
            this.LblFullname.AutoSize = true;
            this.LblFullname.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LblFullname.Location = new System.Drawing.Point(313, 148);
            this.LblFullname.Name = "LblFullname";
            this.LblFullname.Size = new System.Drawing.Size(190, 54);
            this.LblFullname.TabIndex = 1;
            this.LblFullname.Text = "Zy Manti";
            // 
            // UCMemberProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Design2);
            this.Name = "UCMemberProfile";
            this.Size = new System.Drawing.Size(1480, 758);
            this.Design2.ResumeLayout(false);
            this.Design2.PerformLayout();
            this.PnlRegExpDate.ResumeLayout(false);
            this.PnlRegExpDate.PerformLayout();
            this.PnlMemberPrivilege.ResumeLayout(false);
            this.PnlMemberPrivilege.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.LostBorderPanel Design2;
        private System.Windows.Forms.Label LblMemberStatus;
        private System.Windows.Forms.Label LblMemberType;
        private ReaLTaiizor.Controls.Panel PnlRegExpDate;
        private System.Windows.Forms.Label LblActualExpDate;
        private System.Windows.Forms.Label LblExpDate;
        private System.Windows.Forms.Label LblActualRegDate;
        private System.Windows.Forms.Label LblRegDate;
        private System.Windows.Forms.Panel PnlMemberPrivilege;
        private System.Windows.Forms.Label LblNumberFineRate;
        private System.Windows.Forms.Label LblBoolReservationPrivilege;
        private System.Windows.Forms.Label LblNumberRenewalLimit;
        private System.Windows.Forms.Label LblNumberBorrowingPeriod;
        private System.Windows.Forms.Label LblNumberMaxBooksAllowed;
        private System.Windows.Forms.Label LblActualAccountStanding;
        private System.Windows.Forms.Label LblAccontStanding;
        private System.Windows.Forms.Label LblFineRate;
        private System.Windows.Forms.Label LblReservationPrivilege;
        private System.Windows.Forms.Label LblRenewalLimit;
        private System.Windows.Forms.Label LblBorrowingPeriod;
        private System.Windows.Forms.Label LblMaxBooksAllowed;
        private System.Windows.Forms.Label LblMemberPrevileges;
        private ReaLTaiizor.Controls.HopeRichTextBox TxtContact;
        private System.Windows.Forms.Label LblContact;
        private ReaLTaiizor.Controls.HopeRichTextBox TxtAddress;
        private System.Windows.Forms.Label LblAddress;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox PicBxProfilePic;
        private System.Windows.Forms.Panel Design1;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label LblRole;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.Label LblIDNumber;
        private System.Windows.Forms.Label LblFullname;
    }
}
