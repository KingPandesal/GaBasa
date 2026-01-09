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
            this.CmbBxAuthor = new System.Windows.Forms.ComboBox();
            this.LblCategory = new System.Windows.Forms.Label();
            this.CmbBxCategory = new System.Windows.Forms.ComboBox();
            this.LblPublisher = new System.Windows.Forms.Label();
            this.CmbBxPublisher = new System.Windows.Forms.ComboBox();
            this.LblPublicationMonthYear = new System.Windows.Forms.Label();
            this.LblAvailability = new System.Windows.Forms.Label();
            this.CmbBxAvailability = new System.Windows.Forms.ComboBox();
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
            this.PnlBook = new System.Windows.Forms.Panel();
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
            this.CmbBxResourceType = new System.Windows.Forms.ComboBox();
            this.LblLoanType = new System.Windows.Forms.Label();
            this.CmbBxLoanType = new System.Windows.Forms.ComboBox();
            this.LblLanguage = new System.Windows.Forms.Label();
            this.CmbBxLanguage = new System.Windows.Forms.ComboBox();
            this.LblMaterialFormat = new System.Windows.Forms.Label();
            this.CmbBxMaterialFormat = new System.Windows.Forms.ComboBox();
            this.BtnSortTitle = new System.Windows.Forms.Button();
            this.BtnSortAuthor = new System.Windows.Forms.Button();
            this.BtnSortDateAdded = new System.Windows.Forms.Button();
            this.BtnSortCallNumber = new System.Windows.Forms.Button();
            this.BtnSortPublicationYear = new System.Windows.Forms.Button();
            this.NumPckPublicationYearFrom = new System.Windows.Forms.NumericUpDown();
            this.LblPublicationYearFrom = new System.Windows.Forms.Label();
            this.LblPublicationYearTo = new System.Windows.Forms.Label();
            this.NumPckPublicationYearTo = new System.Windows.Forms.NumericUpDown();
            this.BtnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.PnlSearchLogic.SuspendLayout();
            this.FlwPnlBooks.SuspendLayout();
            this.PnlNewArrivalsSection.SuspendLayout();
            this.PnlBook.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).BeginInit();
            this.PnlPopularBooksSection.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckPublicationYearFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckPublicationYearTo)).BeginInit();
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
            // 
            // CmbBxAuthor
            // 
            this.CmbBxAuthor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxAuthor.FormattingEnabled = true;
            this.CmbBxAuthor.Location = new System.Drawing.Point(173, 196);
            this.CmbBxAuthor.Name = "CmbBxAuthor";
            this.CmbBxAuthor.Size = new System.Drawing.Size(109, 29);
            this.CmbBxAuthor.TabIndex = 76;
            this.CmbBxAuthor.Text = "All";
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
            // 
            // CmbBxCategory
            // 
            this.CmbBxCategory.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxCategory.FormattingEnabled = true;
            this.CmbBxCategory.Location = new System.Drawing.Point(173, 234);
            this.CmbBxCategory.Name = "CmbBxCategory";
            this.CmbBxCategory.Size = new System.Drawing.Size(109, 29);
            this.CmbBxCategory.TabIndex = 78;
            this.CmbBxCategory.Text = "All";
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
            // 
            // CmbBxPublisher
            // 
            this.CmbBxPublisher.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxPublisher.FormattingEnabled = true;
            this.CmbBxPublisher.Location = new System.Drawing.Point(173, 270);
            this.CmbBxPublisher.Name = "CmbBxPublisher";
            this.CmbBxPublisher.Size = new System.Drawing.Size(109, 29);
            this.CmbBxPublisher.TabIndex = 78;
            this.CmbBxPublisher.Text = "All";
            // 
            // LblPublicationMonthYear
            // 
            this.LblPublicationMonthYear.AutoSize = true;
            this.LblPublicationMonthYear.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblPublicationMonthYear.Location = new System.Drawing.Point(33, 458);
            this.LblPublicationMonthYear.Name = "LblPublicationMonthYear";
            this.LblPublicationMonthYear.Size = new System.Drawing.Size(121, 21);
            this.LblPublicationMonthYear.TabIndex = 77;
            this.LblPublicationMonthYear.Text = "Publication Year";
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
            // 
            // CmbBxAvailability
            // 
            this.CmbBxAvailability.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxAvailability.FormattingEnabled = true;
            this.CmbBxAvailability.Items.AddRange(new object[] {
            "Available",
            "Available Online",
            "Out of Stock"});
            this.CmbBxAvailability.Location = new System.Drawing.Point(173, 375);
            this.CmbBxAvailability.Name = "CmbBxAvailability";
            this.CmbBxAvailability.Size = new System.Drawing.Size(108, 29);
            this.CmbBxAvailability.TabIndex = 107;
            this.CmbBxAvailability.Text = "All";
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
            this.PnlSearchLogic.Location = new System.Drawing.Point(38, 592);
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
            this.LblBtnSearchLogic.Location = new System.Drawing.Point(34, 568);
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
            this.TxtSearch.MinimumSize = new System.Drawing.Size(1200, 33);
            this.TxtSearch.Multiline = false;
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.ReadOnly = false;
            this.TxtSearch.Size = new System.Drawing.Size(1259, 40);
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
            this.lostBorderPanel14.Location = new System.Drawing.Point(302, 160);
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
            this.FlwPnlBooks.Location = new System.Drawing.Point(333, 160);
            this.FlwPnlBooks.Name = "FlwPnlBooks";
            this.FlwPnlBooks.Size = new System.Drawing.Size(1215, 687);
            this.FlwPnlBooks.TabIndex = 115;
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
            // 
            // PnlNewArrivalsSection
            // 
            this.PnlNewArrivalsSection.AutoScroll = true;
            this.PnlNewArrivalsSection.Controls.Add(this.PnlBook);
            this.PnlNewArrivalsSection.Location = new System.Drawing.Point(3, 45);
            this.PnlNewArrivalsSection.Name = "PnlNewArrivalsSection";
            this.PnlNewArrivalsSection.Size = new System.Drawing.Size(1241, 257);
            this.PnlNewArrivalsSection.TabIndex = 116;
            // 
            // PnlBook
            // 
            this.PnlBook.Controls.Add(this.BtnBookReserve);
            this.PnlBook.Controls.Add(this.LblBookStatus);
            this.PnlBook.Controls.Add(this.LblBookAuthor);
            this.PnlBook.Controls.Add(this.PicBxBookCoverImage);
            this.PnlBook.Controls.Add(this.BtnBookViewDetails);
            this.PnlBook.Controls.Add(this.LblBookTitle);
            this.PnlBook.Controls.Add(this.LblBookCategory);
            this.PnlBook.Location = new System.Drawing.Point(0, 0);
            this.PnlBook.Name = "PnlBook";
            this.PnlBook.Size = new System.Drawing.Size(533, 222);
            this.PnlBook.TabIndex = 70;
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
            this.PicBxBookCoverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
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
            this.LblBookTitle.AutoEllipsis = true;
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
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
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
            // 
            // CmbBxResourceType
            // 
            this.CmbBxResourceType.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxResourceType.FormattingEnabled = true;
            this.CmbBxResourceType.Items.AddRange(new object[] {
            "Book",
            "Periodical",
            "Thesis",
            "Audio-Visual",
            "E-Book"});
            this.CmbBxResourceType.Location = new System.Drawing.Point(173, 160);
            this.CmbBxResourceType.Name = "CmbBxResourceType";
            this.CmbBxResourceType.Size = new System.Drawing.Size(109, 29);
            this.CmbBxResourceType.TabIndex = 76;
            this.CmbBxResourceType.Text = "All";
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
            // 
            // CmbBxLoanType
            // 
            this.CmbBxLoanType.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxLoanType.FormattingEnabled = true;
            this.CmbBxLoanType.Items.AddRange(new object[] {
            "Circulation",
            "Reference"});
            this.CmbBxLoanType.Location = new System.Drawing.Point(173, 305);
            this.CmbBxLoanType.Name = "CmbBxLoanType";
            this.CmbBxLoanType.Size = new System.Drawing.Size(109, 29);
            this.CmbBxLoanType.TabIndex = 78;
            this.CmbBxLoanType.Text = "All";
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
            // 
            // CmbBxLanguage
            // 
            this.CmbBxLanguage.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxLanguage.FormattingEnabled = true;
            this.CmbBxLanguage.Location = new System.Drawing.Point(173, 340);
            this.CmbBxLanguage.Name = "CmbBxLanguage";
            this.CmbBxLanguage.Size = new System.Drawing.Size(109, 29);
            this.CmbBxLanguage.TabIndex = 78;
            this.CmbBxLanguage.Text = "All";
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
            // 
            // CmbBxMaterialFormat
            // 
            this.CmbBxMaterialFormat.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.CmbBxMaterialFormat.FormattingEnabled = true;
            this.CmbBxMaterialFormat.Items.AddRange(new object[] {
            "Physical",
            "Digital"});
            this.CmbBxMaterialFormat.Location = new System.Drawing.Point(173, 410);
            this.CmbBxMaterialFormat.Name = "CmbBxMaterialFormat";
            this.CmbBxMaterialFormat.Size = new System.Drawing.Size(108, 29);
            this.CmbBxMaterialFormat.TabIndex = 107;
            this.CmbBxMaterialFormat.Text = "All";
            // 
            // BtnSortTitle
            // 
            this.BtnSortTitle.BackColor = System.Drawing.Color.White;
            this.BtnSortTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSortTitle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSortTitle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnSortTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortTitle.Location = new System.Drawing.Point(434, 101);
            this.BtnSortTitle.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSortTitle.Name = "BtnSortTitle";
            this.BtnSortTitle.Size = new System.Drawing.Size(81, 36);
            this.BtnSortTitle.TabIndex = 71;
            this.BtnSortTitle.Text = "Title ▲";
            this.BtnSortTitle.UseVisualStyleBackColor = false;
            // 
            // BtnSortAuthor
            // 
            this.BtnSortAuthor.BackColor = System.Drawing.Color.White;
            this.BtnSortAuthor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSortAuthor.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortAuthor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSortAuthor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnSortAuthor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortAuthor.Location = new System.Drawing.Point(533, 101);
            this.BtnSortAuthor.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSortAuthor.Name = "BtnSortAuthor";
            this.BtnSortAuthor.Size = new System.Drawing.Size(81, 36);
            this.BtnSortAuthor.TabIndex = 71;
            this.BtnSortAuthor.Text = "Author";
            this.BtnSortAuthor.UseVisualStyleBackColor = false;
            // 
            // BtnSortDateAdded
            // 
            this.BtnSortDateAdded.BackColor = System.Drawing.Color.White;
            this.BtnSortDateAdded.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSortDateAdded.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortDateAdded.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSortDateAdded.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnSortDateAdded.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortDateAdded.Location = new System.Drawing.Point(633, 101);
            this.BtnSortDateAdded.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSortDateAdded.Name = "BtnSortDateAdded";
            this.BtnSortDateAdded.Size = new System.Drawing.Size(127, 36);
            this.BtnSortDateAdded.TabIndex = 71;
            this.BtnSortDateAdded.Text = "Date Added";
            this.BtnSortDateAdded.UseVisualStyleBackColor = false;
            // 
            // BtnSortCallNumber
            // 
            this.BtnSortCallNumber.BackColor = System.Drawing.Color.White;
            this.BtnSortCallNumber.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSortCallNumber.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortCallNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSortCallNumber.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnSortCallNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortCallNumber.Location = new System.Drawing.Point(779, 101);
            this.BtnSortCallNumber.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSortCallNumber.Name = "BtnSortCallNumber";
            this.BtnSortCallNumber.Size = new System.Drawing.Size(127, 36);
            this.BtnSortCallNumber.TabIndex = 71;
            this.BtnSortCallNumber.Text = "Call Number";
            this.BtnSortCallNumber.UseVisualStyleBackColor = false;
            // 
            // BtnSortPublicationYear
            // 
            this.BtnSortPublicationYear.BackColor = System.Drawing.Color.White;
            this.BtnSortPublicationYear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSortPublicationYear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortPublicationYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSortPublicationYear.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnSortPublicationYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSortPublicationYear.Location = new System.Drawing.Point(927, 101);
            this.BtnSortPublicationYear.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSortPublicationYear.Name = "BtnSortPublicationYear";
            this.BtnSortPublicationYear.Size = new System.Drawing.Size(166, 36);
            this.BtnSortPublicationYear.TabIndex = 71;
            this.BtnSortPublicationYear.Text = "Publication Year";
            this.BtnSortPublicationYear.UseVisualStyleBackColor = false;
            // 
            // NumPckPublicationYearFrom
            // 
            this.NumPckPublicationYearFrom.Location = new System.Drawing.Point(124, 486);
            this.NumPckPublicationYearFrom.Name = "NumPckPublicationYearFrom";
            this.NumPckPublicationYearFrom.Size = new System.Drawing.Size(97, 26);
            this.NumPckPublicationYearFrom.TabIndex = 116;
            // 
            // LblPublicationYearFrom
            // 
            this.LblPublicationYearFrom.AutoSize = true;
            this.LblPublicationYearFrom.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblPublicationYearFrom.Location = new System.Drawing.Point(65, 487);
            this.LblPublicationYearFrom.Name = "LblPublicationYearFrom";
            this.LblPublicationYearFrom.Size = new System.Drawing.Size(47, 21);
            this.LblPublicationYearFrom.TabIndex = 77;
            this.LblPublicationYearFrom.Text = "From";
            // 
            // LblPublicationYearTo
            // 
            this.LblPublicationYearTo.AutoSize = true;
            this.LblPublicationYearTo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblPublicationYearTo.Location = new System.Drawing.Point(65, 519);
            this.LblPublicationYearTo.Name = "LblPublicationYearTo";
            this.LblPublicationYearTo.Size = new System.Drawing.Size(25, 21);
            this.LblPublicationYearTo.TabIndex = 77;
            this.LblPublicationYearTo.Text = "To";
            // 
            // NumPckPublicationYearTo
            // 
            this.NumPckPublicationYearTo.Location = new System.Drawing.Point(124, 518);
            this.NumPckPublicationYearTo.Name = "NumPckPublicationYearTo";
            this.NumPckPublicationYearTo.Size = new System.Drawing.Size(97, 26);
            this.NumPckPublicationYearTo.TabIndex = 116;
            // 
            // BtnSearch
            // 
            this.BtnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearch.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.BtnSearch.ForeColor = System.Drawing.Color.White;
            this.BtnSearch.Location = new System.Drawing.Point(1421, 36);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(127, 36);
            this.BtnSearch.TabIndex = 66;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = false;
            // 
            // UCCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.NumPckPublicationYearTo);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.NumPckPublicationYearFrom);
            this.Controls.Add(this.BtnSortPublicationYear);
            this.Controls.Add(this.BtnSortCallNumber);
            this.Controls.Add(this.BtnSortDateAdded);
            this.Controls.Add(this.BtnSortAuthor);
            this.Controls.Add(this.BtnSortTitle);
            this.Controls.Add(this.FlwPnlBooks);
            this.Controls.Add(this.lostBorderPanel14);
            this.Controls.Add(this.TxtSearch);
            this.Controls.Add(this.LblBtnSearchLogic);
            this.Controls.Add(this.PnlSearchLogic);
            this.Controls.Add(this.BtnResetFilter);
            this.Controls.Add(this.BtnApplyFilter);
            this.Controls.Add(this.CmbBxMaterialFormat);
            this.Controls.Add(this.LblMaterialFormat);
            this.Controls.Add(this.CmbBxAvailability);
            this.Controls.Add(this.LblPublicationYearTo);
            this.Controls.Add(this.LblAvailability);
            this.Controls.Add(this.LblPublicationYearFrom);
            this.Controls.Add(this.LblPublicationMonthYear);
            this.Controls.Add(this.CmbBxLanguage);
            this.Controls.Add(this.LblLanguage);
            this.Controls.Add(this.CmbBxLoanType);
            this.Controls.Add(this.LblLoanType);
            this.Controls.Add(this.CmbBxPublisher);
            this.Controls.Add(this.LblPublisher);
            this.Controls.Add(this.CmbBxCategory);
            this.Controls.Add(this.LblCategory);
            this.Controls.Add(this.CmbBxResourceType);
            this.Controls.Add(this.LblResourceType);
            this.Controls.Add(this.CmbBxAuthor);
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
            this.PnlBook.ResumeLayout(false);
            this.PnlBook.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).EndInit();
            this.PnlPopularBooksSection.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckPublicationYearFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckPublicationYearTo)).EndInit();
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
        private System.Windows.Forms.ComboBox CmbBxAuthor;
        private System.Windows.Forms.Label LblCategory;
        private System.Windows.Forms.ComboBox CmbBxCategory;
        private System.Windows.Forms.Label LblPublisher;
        private System.Windows.Forms.ComboBox CmbBxPublisher;
        private System.Windows.Forms.Label LblPublicationMonthYear;
        private System.Windows.Forms.Label LblAvailability;
        private System.Windows.Forms.ComboBox CmbBxAvailability;
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
        private System.Windows.Forms.Panel PnlBook;
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
        private System.Windows.Forms.ComboBox CmbBxResourceType;
        private System.Windows.Forms.Label LblLoanType;
        private System.Windows.Forms.ComboBox CmbBxLoanType;
        private System.Windows.Forms.Label LblLanguage;
        private System.Windows.Forms.ComboBox CmbBxLanguage;
        private System.Windows.Forms.Label LblMaterialFormat;
        private System.Windows.Forms.ComboBox CmbBxMaterialFormat;
        private System.Windows.Forms.Button BtnSortTitle;
        private System.Windows.Forms.Button BtnSortAuthor;
        private System.Windows.Forms.Button BtnSortDateAdded;
        private System.Windows.Forms.Button BtnSortCallNumber;
        private System.Windows.Forms.Button BtnSortPublicationYear;
        private System.Windows.Forms.NumericUpDown NumPckPublicationYearFrom;
        private System.Windows.Forms.Label LblPublicationYearFrom;
        private System.Windows.Forms.Label LblPublicationYearTo;
        private System.Windows.Forms.NumericUpDown NumPckPublicationYearTo;
        private System.Windows.Forms.Button BtnSearch;
    }
}
