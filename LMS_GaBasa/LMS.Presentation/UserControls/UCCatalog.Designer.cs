namespace LMS.Presentation.UserControls
{
    partial class UCCatalog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCCatalog));
            this.LblBooks = new System.Windows.Forms.Label();
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.LblFilters = new System.Windows.Forms.Label();
            this.LblAuthor = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.LblCategory = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.LblPublisher = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.LblPublicationMonthYear = new System.Windows.Forms.Label();
            this.NumPckNoOfCopies = new ReaLTaiizor.Controls.FoxNumeric();
            this.LblAvailability = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.BtnApplyFilter = new System.Windows.Forms.Button();
            this.BtnResetFilter = new System.Windows.Forms.Button();
            this.PnlSearchLogic = new System.Windows.Forms.Panel();
            this.RdoBtnBooleanNOT = new System.Windows.Forms.RadioButton();
            this.RdoBtnBooleanOR = new System.Windows.Forms.RadioButton();
            this.RdoBtnBooleanAND = new System.Windows.Forms.RadioButton();
            this.LblBtnSearchLogic = new System.Windows.Forms.Label();
            this.TxtSearch = new ReaLTaiizor.Controls.BigTextBox();
            this.lostBorderPanel14 = new ReaLTaiizor.Controls.LostBorderPanel();
            this.FlwPnlBooks = new System.Windows.Forms.FlowLayoutPanel();
            this.LblNewArrivals = new System.Windows.Forms.Label();
            this.PnlNewArrivalsSection = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnBookReserve = new System.Windows.Forms.Button();
            this.LblBookStatus = new System.Windows.Forms.Label();
            this.LblBookAuthor = new System.Windows.Forms.Label();
            this.PicBxBookCoverImage = new System.Windows.Forms.PictureBox();
            this.BtnBookViewDetails = new System.Windows.Forms.Button();
            this.LblBookTitle = new System.Windows.Forms.Label();
            this.LblBookCategory = new System.Windows.Forms.Label();
            this.LblPopularBooks = new System.Windows.Forms.Label();
            this.PnlPopularBooksSection = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.LblResourceType = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.LblLoanType = new System.Windows.Forms.Label();
            this.CmbBx = new System.Windows.Forms.ComboBox();
            this.LblLanguage = new System.Windows.Forms.Label();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.LblMaterialFormat = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.PnlSearchLogic.SuspendLayout();
            this.FlwPnlBooks.SuspendLayout();
            this.PnlNewArrivalsSection.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).BeginInit();
            this.PnlPopularBooksSection.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // LblBooks
            // 
            this.LblBooks.AutoSize = true;
            this.LblBooks.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.LblBooks.Location = new System.Drawing.Point(328, 103);
            this.LblBooks.Name = "LblBooks";
            this.LblBooks.Size = new System.Drawing.Size(68, 28);
            this.LblBooks.TabIndex = 18;
            this.LblBooks.Text = "Books";
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(36, 35);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 73;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 36);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 72;
            this.LblSearch.Text = "Search";
            // 
            // TxtSearchBar
            // 
            this.TxtSearchBar.BackColor = System.Drawing.Color.White;
            this.TxtSearchBar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSearchBar.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearchBar.Image = null;
            this.TxtSearchBar.Location = new System.Drawing.Point(155, 32);
            this.TxtSearchBar.MaximumSize = new System.Drawing.Size(0, 40);
            this.TxtSearchBar.MaxLength = 32767;
            this.TxtSearchBar.MinimumSize = new System.Drawing.Size(0, 33);
            this.TxtSearchBar.Multiline = false;
            this.TxtSearchBar.Name = "TxtSearchBar";
            this.TxtSearchBar.ReadOnly = false;
            this.TxtSearchBar.Size = new System.Drawing.Size(0, 40);
            this.TxtSearchBar.TabIndex = 71;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // LblFilters
            // 
            this.LblFilters.AutoSize = true;
            this.LblFilters.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.LblFilters.Location = new System.Drawing.Point(31, 103);
            this.LblFilters.Name = "LblFilters";
            this.LblFilters.Size = new System.Drawing.Size(66, 28);
            this.LblFilters.TabIndex = 74;
            this.LblFilters.Text = "Filters";
            // 
            // LblAuthor
            // 
            this.LblAuthor.AutoSize = true;
            this.LblAuthor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblAuthor.Location = new System.Drawing.Point(32, 201);
            this.LblAuthor.Name = "LblAuthor";
            this.LblAuthor.Size = new System.Drawing.Size(58, 21);
            this.LblAuthor.TabIndex = 75;
            this.LblAuthor.Text = "Author";
            this.LblAuthor.Click += new System.EventHandler(this.label5_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(155, 196);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(109, 29);
            this.comboBox1.TabIndex = 76;
            this.comboBox1.Text = "All";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // LblCategory
            // 
            this.LblCategory.AutoSize = true;
            this.LblCategory.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblCategory.Location = new System.Drawing.Point(32, 237);
            this.LblCategory.Name = "LblCategory";
            this.LblCategory.Size = new System.Drawing.Size(73, 21);
            this.LblCategory.TabIndex = 77;
            this.LblCategory.Text = "Category";
            this.LblCategory.Click += new System.EventHandler(this.label11_Click);
            // 
            // comboBox3
            // 
            this.comboBox3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(155, 234);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(109, 29);
            this.comboBox3.TabIndex = 78;
            this.comboBox3.Text = "All";
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // LblPublisher
            // 
            this.LblPublisher.AutoSize = true;
            this.LblPublisher.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblPublisher.Location = new System.Drawing.Point(32, 273);
            this.LblPublisher.Name = "LblPublisher";
            this.LblPublisher.Size = new System.Drawing.Size(75, 21);
            this.LblPublisher.TabIndex = 77;
            this.LblPublisher.Text = "Publisher";
            this.LblPublisher.Click += new System.EventHandler(this.label12_Click);
            // 
            // comboBox4
            // 
            this.comboBox4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(155, 270);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(109, 29);
            this.comboBox4.TabIndex = 78;
            this.comboBox4.Text = "All";
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // LblPublicationMonthYear
            // 
            this.LblPublicationMonthYear.AutoSize = true;
            this.LblPublicationMonthYear.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblPublicationMonthYear.Location = new System.Drawing.Point(33, 496);
            this.LblPublicationMonthYear.Name = "LblPublicationMonthYear";
            this.LblPublicationMonthYear.Size = new System.Drawing.Size(74, 21);
            this.LblPublicationMonthYear.TabIndex = 77;
            this.LblPublicationMonthYear.Text = "Pub. Year";
            this.LblPublicationMonthYear.Click += new System.EventHandler(this.label13_Click);
            // 
            // NumPckNoOfCopies
            // 
            this.NumPckNoOfCopies.BackColor = System.Drawing.Color.Transparent;
            this.NumPckNoOfCopies.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.NumPckNoOfCopies.ButtonTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(98)))), ((int)(((byte)(110)))));
            this.NumPckNoOfCopies.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.NumPckNoOfCopies.DisabledButtonTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(198)))), ((int)(((byte)(210)))));
            this.NumPckNoOfCopies.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(178)))), ((int)(((byte)(190)))));
            this.NumPckNoOfCopies.EnabledCalc = true;
            this.NumPckNoOfCopies.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.NumPckNoOfCopies.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(78)))), ((int)(((byte)(90)))));
            this.NumPckNoOfCopies.Location = new System.Drawing.Point(156, 490);
            this.NumPckNoOfCopies.Max = 100;
            this.NumPckNoOfCopies.Min = 0;
            this.NumPckNoOfCopies.Name = "NumPckNoOfCopies";
            this.NumPckNoOfCopies.Size = new System.Drawing.Size(109, 27);
            this.NumPckNoOfCopies.TabIndex = 105;
            this.NumPckNoOfCopies.Text = "foxNumeric1";
            this.NumPckNoOfCopies.Value = 0;
            this.NumPckNoOfCopies.Click += new System.EventHandler(this.NumPckNoOfCopies_Click);
            // 
            // LblAvailability
            // 
            this.LblAvailability.AutoSize = true;
            this.LblAvailability.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblAvailability.Location = new System.Drawing.Point(33, 378);
            this.LblAvailability.Name = "LblAvailability";
            this.LblAvailability.Size = new System.Drawing.Size(86, 21);
            this.LblAvailability.TabIndex = 106;
            this.LblAvailability.Text = "Availability";
            this.LblAvailability.Click += new System.EventHandler(this.label14_Click);
            // 
            // comboBox5
            // 
            this.comboBox5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(156, 375);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(108, 29);
            this.comboBox5.TabIndex = 107;
            this.comboBox5.Text = "All";
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // BtnApplyFilter
            // 
            this.BtnApplyFilter.BackColor = System.Drawing.Color.White;
            this.BtnApplyFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnApplyFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApplyFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApplyFilter.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnApplyFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApplyFilter.Location = new System.Drawing.Point(51, 735);
            this.BtnApplyFilter.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApplyFilter.Name = "BtnApplyFilter";
            this.BtnApplyFilter.Size = new System.Drawing.Size(81, 42);
            this.BtnApplyFilter.TabIndex = 65;
            this.BtnApplyFilter.Text = "Apply";
            this.BtnApplyFilter.UseVisualStyleBackColor = false;
            // 
            // BtnResetFilter
            // 
            this.BtnResetFilter.BackColor = System.Drawing.Color.White;
            this.BtnResetFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnResetFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnResetFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnResetFilter.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnResetFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnResetFilter.Location = new System.Drawing.Point(140, 735);
            this.BtnResetFilter.Margin = new System.Windows.Forms.Padding(4);
            this.BtnResetFilter.Name = "BtnResetFilter";
            this.BtnResetFilter.Size = new System.Drawing.Size(81, 42);
            this.BtnResetFilter.TabIndex = 109;
            this.BtnResetFilter.Text = "Reset";
            this.BtnResetFilter.UseVisualStyleBackColor = false;
            // 
            // PnlSearchLogic
            // 
            this.PnlSearchLogic.Controls.Add(this.RdoBtnBooleanNOT);
            this.PnlSearchLogic.Controls.Add(this.RdoBtnBooleanOR);
            this.PnlSearchLogic.Controls.Add(this.RdoBtnBooleanAND);
            this.PnlSearchLogic.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PnlSearchLogic.Location = new System.Drawing.Point(37, 564);
            this.PnlSearchLogic.Name = "PnlSearchLogic";
            this.PnlSearchLogic.Size = new System.Drawing.Size(196, 111);
            this.PnlSearchLogic.TabIndex = 110;
            this.PnlSearchLogic.Visible = false;
            // 
            // RdoBtnBooleanNOT
            // 
            this.RdoBtnBooleanNOT.AutoSize = true;
            this.RdoBtnBooleanNOT.Location = new System.Drawing.Point(12, 74);
            this.RdoBtnBooleanNOT.Name = "RdoBtnBooleanNOT";
            this.RdoBtnBooleanNOT.Size = new System.Drawing.Size(171, 25);
            this.RdoBtnBooleanNOT.TabIndex = 2;
            this.RdoBtnBooleanNOT.TabStop = true;
            this.RdoBtnBooleanNOT.Text = "Exclude these filters";
            this.RdoBtnBooleanNOT.UseVisualStyleBackColor = true;
            // 
            // RdoBtnBooleanOR
            // 
            this.RdoBtnBooleanOR.AutoSize = true;
            this.RdoBtnBooleanOR.Location = new System.Drawing.Point(12, 43);
            this.RdoBtnBooleanOR.Name = "RdoBtnBooleanOR";
            this.RdoBtnBooleanOR.Size = new System.Drawing.Size(150, 25);
            this.RdoBtnBooleanOR.TabIndex = 1;
            this.RdoBtnBooleanOR.TabStop = true;
            this.RdoBtnBooleanOR.Text = "Match any filters";
            this.RdoBtnBooleanOR.UseVisualStyleBackColor = true;
            // 
            // RdoBtnBooleanAND
            // 
            this.RdoBtnBooleanAND.AutoSize = true;
            this.RdoBtnBooleanAND.Location = new System.Drawing.Point(12, 12);
            this.RdoBtnBooleanAND.Name = "RdoBtnBooleanAND";
            this.RdoBtnBooleanAND.Size = new System.Drawing.Size(141, 25);
            this.RdoBtnBooleanAND.TabIndex = 0;
            this.RdoBtnBooleanAND.TabStop = true;
            this.RdoBtnBooleanAND.Text = "Match all filters";
            this.RdoBtnBooleanAND.UseVisualStyleBackColor = true;
            // 
            // LblBtnSearchLogic
            // 
            this.LblBtnSearchLogic.AutoSize = true;
            this.LblBtnSearchLogic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblBtnSearchLogic.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblBtnSearchLogic.Location = new System.Drawing.Point(33, 540);
            this.LblBtnSearchLogic.Name = "LblBtnSearchLogic";
            this.LblBtnSearchLogic.Size = new System.Drawing.Size(116, 21);
            this.LblBtnSearchLogic.TabIndex = 111;
            this.LblBtnSearchLogic.Text = "▶ Search Logic";
            this.LblBtnSearchLogic.Click += new System.EventHandler(this.LblBtnSearchLogic_Click);
            // 
            // TxtSearch
            // 
            this.TxtSearch.BackColor = System.Drawing.Color.White;
            this.TxtSearch.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.TxtSearch.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearch.Image = null;
            this.TxtSearch.Location = new System.Drawing.Point(155, 32);
            this.TxtSearch.MaximumSize = new System.Drawing.Size(1500, 40);
            this.TxtSearch.MaxLength = 32767;
            this.TxtSearch.MinimumSize = new System.Drawing.Size(1393, 33);
            this.TxtSearch.Multiline = false;
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.ReadOnly = false;
            this.TxtSearch.Size = new System.Drawing.Size(1393, 40);
            this.TxtSearch.TabIndex = 113;
            this.TxtSearch.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearch.UseSystemPasswordChar = false;
            // 
            // lostBorderPanel14
            // 
            this.lostBorderPanel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.lostBorderPanel14.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.lostBorderPanel14.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lostBorderPanel14.ForeColor = System.Drawing.Color.Black;
            this.lostBorderPanel14.Location = new System.Drawing.Point(283, 160);
            this.lostBorderPanel14.Name = "lostBorderPanel14";
            this.lostBorderPanel14.Padding = new System.Windows.Forms.Padding(5);
            this.lostBorderPanel14.ShowText = true;
            this.lostBorderPanel14.Size = new System.Drawing.Size(1, 584);
            this.lostBorderPanel14.TabIndex = 114;
            // 
            // FlwPnlBooks
            // 
            this.FlwPnlBooks.Controls.Add(this.LblNewArrivals);
            this.FlwPnlBooks.Controls.Add(this.PnlNewArrivalsSection);
            this.FlwPnlBooks.Controls.Add(this.LblPopularBooks);
            this.FlwPnlBooks.Controls.Add(this.PnlPopularBooksSection);
            this.FlwPnlBooks.Location = new System.Drawing.Point(333, 150);
            this.FlwPnlBooks.Name = "FlwPnlBooks";
            this.FlwPnlBooks.Size = new System.Drawing.Size(1215, 697);
            this.FlwPnlBooks.TabIndex = 115;
            this.FlwPnlBooks.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint_1);
            // 
            // LblNewArrivals
            // 
            this.LblNewArrivals.AutoSize = true;
            this.LblNewArrivals.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LblNewArrivals.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNewArrivals.Location = new System.Drawing.Point(3, 0);
            this.LblNewArrivals.Name = "LblNewArrivals";
            this.LblNewArrivals.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.LblNewArrivals.Size = new System.Drawing.Size(160, 42);
            this.LblNewArrivals.TabIndex = 22;
            this.LblNewArrivals.Text = "New Arrivals";
            this.LblNewArrivals.Click += new System.EventHandler(this.label7_Click);
            // 
            // PnlNewArrivalsSection
            // 
            this.PnlNewArrivalsSection.AutoScroll = true;
            this.PnlNewArrivalsSection.Controls.Add(this.panel2);
            this.PnlNewArrivalsSection.Location = new System.Drawing.Point(3, 45);
            this.PnlNewArrivalsSection.Name = "PnlNewArrivalsSection";
            this.PnlNewArrivalsSection.Size = new System.Drawing.Size(1241, 257);
            this.PnlNewArrivalsSection.TabIndex = 116;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnBookReserve);
            this.panel2.Controls.Add(this.LblBookStatus);
            this.panel2.Controls.Add(this.LblBookAuthor);
            this.panel2.Controls.Add(this.PicBxBookCoverImage);
            this.panel2.Controls.Add(this.BtnBookViewDetails);
            this.panel2.Controls.Add(this.LblBookTitle);
            this.panel2.Controls.Add(this.LblBookCategory);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(533, 222);
            this.panel2.TabIndex = 70;
            // 
            // BtnBookReserve
            // 
            this.BtnBookReserve.BackColor = System.Drawing.Color.White;
            this.BtnBookReserve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBookReserve.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnBookReserve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBookReserve.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnBookReserve.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnBookReserve.Location = new System.Drawing.Point(325, 173);
            this.BtnBookReserve.Margin = new System.Windows.Forms.Padding(4);
            this.BtnBookReserve.Name = "BtnBookReserve";
            this.BtnBookReserve.Size = new System.Drawing.Size(102, 36);
            this.BtnBookReserve.TabIndex = 81;
            this.BtnBookReserve.Text = "Reserve";
            this.BtnBookReserve.UseVisualStyleBackColor = false;
            // 
            // LblBookStatus
            // 
            this.LblBookStatus.AutoSize = true;
            this.LblBookStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblBookStatus.Location = new System.Drawing.Point(183, 101);
            this.LblBookStatus.Name = "LblBookStatus";
            this.LblBookStatus.Size = new System.Drawing.Size(122, 21);
            this.LblBookStatus.TabIndex = 79;
            this.LblBookStatus.Text = "Status: Available";
            // 
            // LblBookAuthor
            // 
            this.LblBookAuthor.AutoSize = true;
            this.LblBookAuthor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblBookAuthor.Location = new System.Drawing.Point(183, 80);
            this.LblBookAuthor.Name = "LblBookAuthor";
            this.LblBookAuthor.Size = new System.Drawing.Size(147, 21);
            this.LblBookAuthor.TabIndex = 78;
            this.LblBookAuthor.Text = "Author: J.K. Rowling";
            // 
            // PicBxBookCoverImage
            // 
            this.PicBxBookCoverImage.Image = ((System.Drawing.Image)(resources.GetObject("PicBxBookCoverImage.Image")));
            this.PicBxBookCoverImage.Location = new System.Drawing.Point(0, 0);
            this.PicBxBookCoverImage.Name = "PicBxBookCoverImage";
            this.PicBxBookCoverImage.Size = new System.Drawing.Size(175, 220);
            this.PicBxBookCoverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBxBookCoverImage.TabIndex = 0;
            this.PicBxBookCoverImage.TabStop = false;
            // 
            // BtnBookViewDetails
            // 
            this.BtnBookViewDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnBookViewDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBookViewDetails.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnBookViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBookViewDetails.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnBookViewDetails.ForeColor = System.Drawing.Color.White;
            this.BtnBookViewDetails.Location = new System.Drawing.Point(190, 173);
            this.BtnBookViewDetails.Margin = new System.Windows.Forms.Padding(4);
            this.BtnBookViewDetails.Name = "BtnBookViewDetails";
            this.BtnBookViewDetails.Size = new System.Drawing.Size(127, 36);
            this.BtnBookViewDetails.TabIndex = 66;
            this.BtnBookViewDetails.Text = "View Details";
            this.BtnBookViewDetails.UseVisualStyleBackColor = false;
            // 
            // LblBookTitle
            // 
            this.LblBookTitle.AutoSize = true;
            this.LblBookTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.LblBookTitle.Location = new System.Drawing.Point(181, 0);
            this.LblBookTitle.Name = "LblBookTitle";
            this.LblBookTitle.Size = new System.Drawing.Size(349, 32);
            this.LblBookTitle.TabIndex = 1;
            this.LblBookTitle.Text = "Inspirational Women Leaders...";
            // 
            // LblBookCategory
            // 
            this.LblBookCategory.AutoSize = true;
            this.LblBookCategory.BackColor = System.Drawing.Color.OrangeRed;
            this.LblBookCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.LblBookCategory.ForeColor = System.Drawing.Color.White;
            this.LblBookCategory.Location = new System.Drawing.Point(186, 39);
            this.LblBookCategory.Name = "LblBookCategory";
            this.LblBookCategory.Padding = new System.Windows.Forms.Padding(4);
            this.LblBookCategory.Size = new System.Drawing.Size(64, 28);
            this.LblBookCategory.TabIndex = 2;
            this.LblBookCategory.Text = "Fiction";
            // 
            // LblPopularBooks
            // 
            this.LblPopularBooks.AutoSize = true;
            this.LblPopularBooks.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LblPopularBooks.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPopularBooks.Location = new System.Drawing.Point(3, 305);
            this.LblPopularBooks.Name = "LblPopularBooks";
            this.LblPopularBooks.Padding = new System.Windows.Forms.Padding(0, 20, 0, 10);
            this.LblPopularBooks.Size = new System.Drawing.Size(179, 62);
            this.LblPopularBooks.TabIndex = 25;
            this.LblPopularBooks.Text = "Popular Books";
            // 
            // PnlPopularBooksSection
            // 
            this.PnlPopularBooksSection.AutoScroll = true;
            this.PnlPopularBooksSection.Controls.Add(this.panel8);
            this.PnlPopularBooksSection.Location = new System.Drawing.Point(3, 370);
            this.PnlPopularBooksSection.Name = "PnlPopularBooksSection";
            this.PnlPopularBooksSection.Size = new System.Drawing.Size(1241, 257);
            this.PnlPopularBooksSection.TabIndex = 117;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.button4);
            this.panel8.Controls.Add(this.label33);
            this.panel8.Controls.Add(this.label34);
            this.panel8.Controls.Add(this.button11);
            this.panel8.Controls.Add(this.pictureBox3);
            this.panel8.Controls.Add(this.label10);
            this.panel8.Controls.Add(this.label15);
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(533, 222);
            this.panel8.TabIndex = 70;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.White;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Location = new System.Drawing.Point(331, 172);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(102, 36);
            this.button4.TabIndex = 86;
            this.button4.Text = "Reserve";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label33.Location = new System.Drawing.Point(189, 100);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(122, 21);
            this.label33.TabIndex = 84;
            this.label33.Text = "Status: Available";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label34.Location = new System.Drawing.Point(189, 79);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(147, 21);
            this.label34.TabIndex = 83;
            this.label34.Text = "Author: J.K. Rowling";
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button11.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button11.ForeColor = System.Drawing.Color.White;
            this.button11.Location = new System.Drawing.Point(196, 172);
            this.button11.Margin = new System.Windows.Forms.Padding(4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(127, 36);
            this.button11.TabIndex = 82;
            this.button11.Text = "View Details";
            this.button11.UseVisualStyleBackColor = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(175, 220);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(181, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(349, 32);
            this.label10.TabIndex = 1;
            this.label10.Text = "Inspirational Women Leaders...";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.OrangeRed;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(192, 38);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(4);
            this.label15.Size = new System.Drawing.Size(63, 28);
            this.label15.TabIndex = 2;
            this.label15.Text = "Thesis";
            // 
            // LblResourceType
            // 
            this.LblResourceType.AutoSize = true;
            this.LblResourceType.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblResourceType.Location = new System.Drawing.Point(32, 163);
            this.LblResourceType.Name = "LblResourceType";
            this.LblResourceType.Size = new System.Drawing.Size(110, 21);
            this.LblResourceType.TabIndex = 75;
            this.LblResourceType.Text = "Resource Type";
            this.LblResourceType.Click += new System.EventHandler(this.label5_Click);
            // 
            // comboBox6
            // 
            this.comboBox6.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(155, 160);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(109, 29);
            this.comboBox6.TabIndex = 76;
            this.comboBox6.Text = "All";
            this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // LblLoanType
            // 
            this.LblLoanType.AutoSize = true;
            this.LblLoanType.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblLoanType.Location = new System.Drawing.Point(32, 308);
            this.LblLoanType.Name = "LblLoanType";
            this.LblLoanType.Size = new System.Drawing.Size(80, 21);
            this.LblLoanType.TabIndex = 77;
            this.LblLoanType.Text = "Loan Type";
            this.LblLoanType.Click += new System.EventHandler(this.label12_Click);
            // 
            // CmbBx
            // 
            this.CmbBx.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBx.FormattingEnabled = true;
            this.CmbBx.Location = new System.Drawing.Point(155, 305);
            this.CmbBx.Name = "CmbBx";
            this.CmbBx.Size = new System.Drawing.Size(109, 29);
            this.CmbBx.TabIndex = 78;
            this.CmbBx.Text = "All";
            this.CmbBx.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // LblLanguage
            // 
            this.LblLanguage.AutoSize = true;
            this.LblLanguage.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblLanguage.Location = new System.Drawing.Point(32, 343);
            this.LblLanguage.Name = "LblLanguage";
            this.LblLanguage.Size = new System.Drawing.Size(78, 21);
            this.LblLanguage.TabIndex = 77;
            this.LblLanguage.Text = "Language";
            this.LblLanguage.Click += new System.EventHandler(this.label12_Click);
            // 
            // comboBox7
            // 
            this.comboBox7.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(155, 340);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(109, 29);
            this.comboBox7.TabIndex = 78;
            this.comboBox7.Text = "All";
            this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // LblMaterialFormat
            // 
            this.LblMaterialFormat.AutoSize = true;
            this.LblMaterialFormat.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblMaterialFormat.Location = new System.Drawing.Point(34, 413);
            this.LblMaterialFormat.Name = "LblMaterialFormat";
            this.LblMaterialFormat.Size = new System.Drawing.Size(121, 21);
            this.LblMaterialFormat.TabIndex = 106;
            this.LblMaterialFormat.Text = "Material Format";
            this.LblMaterialFormat.Click += new System.EventHandler(this.label14_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(157, 410);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(108, 29);
            this.comboBox2.TabIndex = 107;
            this.comboBox2.Text = "All";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // UCCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.FlwPnlBooks);
            this.Controls.Add(this.lostBorderPanel14);
            this.Controls.Add(this.TxtSearch);
            this.Controls.Add(this.LblBtnSearchLogic);
            this.Controls.Add(this.PnlSearchLogic);
            this.Controls.Add(this.BtnResetFilter);
            this.Controls.Add(this.BtnApplyFilter);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.LblMaterialFormat);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.LblAvailability);
            this.Controls.Add(this.NumPckNoOfCopies);
            this.Controls.Add(this.LblPublicationMonthYear);
            this.Controls.Add(this.comboBox7);
            this.Controls.Add(this.LblLanguage);
            this.Controls.Add(this.CmbBx);
            this.Controls.Add(this.LblLoanType);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.LblPublisher);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.LblCategory);
            this.Controls.Add(this.comboBox6);
            this.Controls.Add(this.LblResourceType);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.LblAuthor);
            this.Controls.Add(this.LblFilters);
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.TxtSearchBar);
            this.Controls.Add(this.LblBooks);
            this.Name = "UCCatalog";
            this.Size = new System.Drawing.Size(1580, 936);
            this.Load += new System.EventHandler(this.UCCatalog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.PnlSearchLogic.ResumeLayout(false);
            this.PnlSearchLogic.PerformLayout();
            this.FlwPnlBooks.ResumeLayout(false);
            this.FlwPnlBooks.PerformLayout();
            this.PnlNewArrivalsSection.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).EndInit();
            this.PnlPopularBooksSection.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblBooks;
        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.Label LblSearch;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.Label LblFilters;
        private System.Windows.Forms.Label LblAuthor;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label LblCategory;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label LblPublisher;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label LblPublicationMonthYear;
        private ReaLTaiizor.Controls.FoxNumeric NumPckNoOfCopies;
        private System.Windows.Forms.Label LblAvailability;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Button BtnApplyFilter;
        private System.Windows.Forms.Button BtnResetFilter;
        private System.Windows.Forms.Panel PnlSearchLogic;
        private System.Windows.Forms.RadioButton RdoBtnBooleanNOT;
        private System.Windows.Forms.RadioButton RdoBtnBooleanOR;
        private System.Windows.Forms.RadioButton RdoBtnBooleanAND;
        private System.Windows.Forms.Label LblBtnSearchLogic;
        private ReaLTaiizor.Controls.BigTextBox TxtSearch;
        private ReaLTaiizor.Controls.LostBorderPanel lostBorderPanel14;
        private System.Windows.Forms.FlowLayoutPanel FlwPnlBooks;
        private System.Windows.Forms.Label LblNewArrivals;
        private System.Windows.Forms.Label LblPopularBooks;
        private System.Windows.Forms.Panel PnlNewArrivalsSection;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox PicBxBookCoverImage;
        private System.Windows.Forms.Button BtnBookViewDetails;
        private System.Windows.Forms.Label LblBookTitle;
        private System.Windows.Forms.Label LblBookCategory;
        private System.Windows.Forms.Panel PnlPopularBooksSection;
        private System.Windows.Forms.Label LblBookStatus;
        private System.Windows.Forms.Label LblBookAuthor;
        private System.Windows.Forms.Button BtnBookReserve;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label LblResourceType;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Label LblLoanType;
        private System.Windows.Forms.ComboBox CmbBx;
        private System.Windows.Forms.Label LblLanguage;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.Label LblMaterialFormat;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}
