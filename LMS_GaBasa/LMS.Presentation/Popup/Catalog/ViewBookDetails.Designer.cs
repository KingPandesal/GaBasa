namespace LMS.Presentation.Popup.Catalog
{
    partial class ViewBookDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewBookDetails));
            this.PnlDesign = new System.Windows.Forms.Panel();
            this.PnlPicBxBookCoverContainer = new System.Windows.Forms.Panel();
            this.PicBxBookCover = new System.Windows.Forms.PictureBox();
            this.PnlDocked = new System.Windows.Forms.Panel();
            this.PnlBookDetails = new System.Windows.Forms.Panel();
            this.DgwBookCopy = new System.Windows.Forms.DataGridView();
            this.ColumnNumbering = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAccessionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDateAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAddedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBarcode = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.LblResourceType = new System.Windows.Forms.Label();
            this.LblStandardID = new System.Windows.Forms.Label();
            this.LblCallNumber = new System.Windows.Forms.Label();
            this.LblCopyDetails = new System.Windows.Forms.Label();
            this.LblAvailableForBorrow = new System.Windows.Forms.Label();
            this.LblTotalCopies = new System.Windows.Forms.Label();
            this.LblPhysDescFormat = new System.Windows.Forms.Label();
            this.LblPubDateYear = new System.Windows.Forms.Label();
            this.LblEdition = new System.Windows.Forms.Label();
            this.LblPages = new System.Windows.Forms.Label();
            this.LblLanguage = new System.Windows.Forms.Label();
            this.LblPublisher = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.FlwPnlBtns = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnBorrow = new System.Windows.Forms.Button();
            this.BtnReserve = new System.Windows.Forms.Button();
            this.BtnDownloadLink = new System.Windows.Forms.Button();
            this.LblSubtitle = new System.Windows.Forms.Label();
            this.LblEditors = new System.Windows.Forms.Label();
            this.LblAuthors = new System.Windows.Forms.Label();
            this.LblCategory = new System.Windows.Forms.Label();
            this.LblTitle = new System.Windows.Forms.Label();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.PnlPicBxBookCoverContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCover)).BeginInit();
            this.PnlDocked.SuspendLayout();
            this.PnlBookDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgwBookCopy)).BeginInit();
            this.FlwPnlBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlDesign
            // 
            this.PnlDesign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.PnlDesign.Location = new System.Drawing.Point(0, 0);
            this.PnlDesign.Name = "PnlDesign";
            this.PnlDesign.Size = new System.Drawing.Size(1119, 114);
            this.PnlDesign.TabIndex = 0;
            // 
            // PnlPicBxBookCoverContainer
            // 
            this.PnlPicBxBookCoverContainer.BackColor = System.Drawing.Color.White;
            this.PnlPicBxBookCoverContainer.Controls.Add(this.PicBxBookCover);
            this.PnlPicBxBookCoverContainer.Location = new System.Drawing.Point(56, 55);
            this.PnlPicBxBookCoverContainer.Name = "PnlPicBxBookCoverContainer";
            this.PnlPicBxBookCoverContainer.Size = new System.Drawing.Size(326, 405);
            this.PnlPicBxBookCoverContainer.TabIndex = 2;
            // 
            // PicBxBookCover
            // 
            this.PicBxBookCover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.PicBxBookCover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicBxBookCover.Location = new System.Drawing.Point(24, 23);
            this.PicBxBookCover.Name = "PicBxBookCover";
            this.PicBxBookCover.Size = new System.Drawing.Size(277, 362);
            this.PicBxBookCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxBookCover.TabIndex = 1;
            this.PicBxBookCover.TabStop = false;
            // 
            // PnlDocked
            // 
            this.PnlDocked.AutoScroll = true;
            this.PnlDocked.Controls.Add(this.PnlBookDetails);
            this.PnlDocked.Controls.Add(this.panel3);
            this.PnlDocked.Controls.Add(this.FlwPnlBtns);
            this.PnlDocked.Controls.Add(this.LblSubtitle);
            this.PnlDocked.Controls.Add(this.LblEditors);
            this.PnlDocked.Controls.Add(this.LblAuthors);
            this.PnlDocked.Controls.Add(this.LblCategory);
            this.PnlDocked.Controls.Add(this.LblTitle);
            this.PnlDocked.Controls.Add(this.PnlPicBxBookCoverContainer);
            this.PnlDocked.Controls.Add(this.PnlDesign);
            this.PnlDocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlDocked.Location = new System.Drawing.Point(0, 0);
            this.PnlDocked.Name = "PnlDocked";
            this.PnlDocked.Size = new System.Drawing.Size(1145, 666);
            this.PnlDocked.TabIndex = 3;
            this.PnlDocked.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // PnlBookDetails
            // 
            this.PnlBookDetails.Controls.Add(this.DgwBookCopy);
            this.PnlBookDetails.Controls.Add(this.LblResourceType);
            this.PnlBookDetails.Controls.Add(this.LblStandardID);
            this.PnlBookDetails.Controls.Add(this.LblCallNumber);
            this.PnlBookDetails.Controls.Add(this.LblCopyDetails);
            this.PnlBookDetails.Controls.Add(this.LblAvailableForBorrow);
            this.PnlBookDetails.Controls.Add(this.LblTotalCopies);
            this.PnlBookDetails.Controls.Add(this.LblPhysDescFormat);
            this.PnlBookDetails.Controls.Add(this.LblPubDateYear);
            this.PnlBookDetails.Controls.Add(this.LblEdition);
            this.PnlBookDetails.Controls.Add(this.LblPages);
            this.PnlBookDetails.Controls.Add(this.LblLanguage);
            this.PnlBookDetails.Controls.Add(this.LblPublisher);
            this.PnlBookDetails.Location = new System.Drawing.Point(82, 518);
            this.PnlBookDetails.Name = "PnlBookDetails";
            this.PnlBookDetails.Size = new System.Drawing.Size(996, 843);
            this.PnlBookDetails.TabIndex = 13;
            // 
            // DgwBookCopy
            // 
            this.DgwBookCopy.AllowUserToAddRows = false;
            this.DgwBookCopy.BackgroundColor = System.Drawing.Color.White;
            this.DgwBookCopy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgwBookCopy.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNumbering,
            this.ColumnAccessionNumber,
            this.ColumnLocation,
            this.ColumnStatus,
            this.ColumnDateAdded,
            this.ColumnAddedBy,
            this.ColumnBarcode,
            this.Edit,
            this.Delete});
            this.DgwBookCopy.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgwBookCopy.Location = new System.Drawing.Point(36, 479);
            this.DgwBookCopy.Name = "DgwBookCopy";
            this.DgwBookCopy.RowHeadersWidth = 62;
            this.DgwBookCopy.RowTemplate.Height = 28;
            this.DgwBookCopy.Size = new System.Drawing.Size(910, 326);
            this.DgwBookCopy.TabIndex = 60;
            this.DgwBookCopy.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgwBookCopy_CellContentClick);
            // 
            // ColumnNumbering
            // 
            this.ColumnNumbering.HeaderText = "#";
            this.ColumnNumbering.MinimumWidth = 8;
            this.ColumnNumbering.Name = "ColumnNumbering";
            this.ColumnNumbering.Width = 40;
            // 
            // ColumnAccessionNumber
            // 
            this.ColumnAccessionNumber.HeaderText = "Accession Number";
            this.ColumnAccessionNumber.MinimumWidth = 8;
            this.ColumnAccessionNumber.Name = "ColumnAccessionNumber";
            this.ColumnAccessionNumber.Width = 150;
            // 
            // ColumnLocation
            // 
            this.ColumnLocation.HeaderText = "Location";
            this.ColumnLocation.MinimumWidth = 8;
            this.ColumnLocation.Name = "ColumnLocation";
            this.ColumnLocation.Width = 150;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.MinimumWidth = 8;
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.Width = 150;
            // 
            // ColumnDateAdded
            // 
            this.ColumnDateAdded.HeaderText = "Date Added";
            this.ColumnDateAdded.MinimumWidth = 8;
            this.ColumnDateAdded.Name = "ColumnDateAdded";
            this.ColumnDateAdded.Width = 150;
            // 
            // ColumnAddedBy
            // 
            this.ColumnAddedBy.HeaderText = "Added By";
            this.ColumnAddedBy.MinimumWidth = 8;
            this.ColumnAddedBy.Name = "ColumnAddedBy";
            this.ColumnAddedBy.Width = 150;
            // 
            // ColumnBarcode
            // 
            this.ColumnBarcode.HeaderText = "Barcode";
            this.ColumnBarcode.MinimumWidth = 8;
            this.ColumnBarcode.Name = "ColumnBarcode";
            this.ColumnBarcode.Text = "View Barcode";
            this.ColumnBarcode.Width = 150;
            // 
            // Edit
            // 
            this.Edit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Edit.HeaderText = "";
            this.Edit.Image = ((System.Drawing.Image)(resources.GetObject("Edit.Image")));
            this.Edit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Edit.MinimumWidth = 8;
            this.Edit.Name = "Edit";
            this.Edit.Width = 8;
            // 
            // Delete
            // 
            this.Delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Delete.HeaderText = "";
            this.Delete.Image = ((System.Drawing.Image)(resources.GetObject("Delete.Image")));
            this.Delete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Delete.MinimumWidth = 8;
            this.Delete.Name = "Delete";
            this.Delete.Width = 8;
            // 
            // LblResourceType
            // 
            this.LblResourceType.AutoSize = true;
            this.LblResourceType.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblResourceType.Location = new System.Drawing.Point(12, 0);
            this.LblResourceType.Name = "LblResourceType";
            this.LblResourceType.Size = new System.Drawing.Size(180, 32);
            this.LblResourceType.TabIndex = 11;
            this.LblResourceType.Text = "Resource Type :";
            // 
            // LblStandardID
            // 
            this.LblStandardID.AutoSize = true;
            this.LblStandardID.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblStandardID.Location = new System.Drawing.Point(12, 43);
            this.LblStandardID.Name = "LblStandardID";
            this.LblStandardID.Size = new System.Drawing.Size(77, 32);
            this.LblStandardID.TabIndex = 11;
            this.LblStandardID.Text = "ISBN :";
            // 
            // LblCallNumber
            // 
            this.LblCallNumber.AutoSize = true;
            this.LblCallNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblCallNumber.Location = new System.Drawing.Point(12, 85);
            this.LblCallNumber.Name = "LblCallNumber";
            this.LblCallNumber.Size = new System.Drawing.Size(160, 32);
            this.LblCallNumber.TabIndex = 11;
            this.LblCallNumber.Text = "Call Number :";
            // 
            // LblCopyDetails
            // 
            this.LblCopyDetails.AutoSize = true;
            this.LblCopyDetails.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblCopyDetails.Location = new System.Drawing.Point(12, 349);
            this.LblCopyDetails.Name = "LblCopyDetails";
            this.LblCopyDetails.Size = new System.Drawing.Size(148, 32);
            this.LblCopyDetails.TabIndex = 11;
            this.LblCopyDetails.Text = "Copy Details";
            // 
            // LblAvailableForBorrow
            // 
            this.LblAvailableForBorrow.AutoSize = true;
            this.LblAvailableForBorrow.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAvailableForBorrow.Location = new System.Drawing.Point(31, 431);
            this.LblAvailableForBorrow.Name = "LblAvailableForBorrow";
            this.LblAvailableForBorrow.Size = new System.Drawing.Size(195, 28);
            this.LblAvailableForBorrow.TabIndex = 11;
            this.LblAvailableForBorrow.Text = "Available for borrow:";
            // 
            // LblTotalCopies
            // 
            this.LblTotalCopies.AutoSize = true;
            this.LblTotalCopies.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblTotalCopies.Location = new System.Drawing.Point(31, 394);
            this.LblTotalCopies.Name = "LblTotalCopies";
            this.LblTotalCopies.Size = new System.Drawing.Size(127, 28);
            this.LblTotalCopies.TabIndex = 11;
            this.LblTotalCopies.Text = "Total Copies :";
            // 
            // LblPhysDescFormat
            // 
            this.LblPhysDescFormat.AutoSize = true;
            this.LblPhysDescFormat.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblPhysDescFormat.Location = new System.Drawing.Point(12, 243);
            this.LblPhysDescFormat.Name = "LblPhysDescFormat";
            this.LblPhysDescFormat.Size = new System.Drawing.Size(336, 32);
            this.LblPhysDescFormat.TabIndex = 11;
            this.LblPhysDescFormat.Text = "Physical Description / Format :";
            // 
            // LblPubDateYear
            // 
            this.LblPubDateYear.AutoSize = true;
            this.LblPubDateYear.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblPubDateYear.Location = new System.Drawing.Point(12, 197);
            this.LblPubDateYear.Name = "LblPubDateYear";
            this.LblPubDateYear.Size = new System.Drawing.Size(268, 32);
            this.LblPubDateYear.TabIndex = 11;
            this.LblPubDateYear.Text = "Publication Date / Year :";
            // 
            // LblEdition
            // 
            this.LblEdition.AutoSize = true;
            this.LblEdition.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblEdition.Location = new System.Drawing.Point(537, 0);
            this.LblEdition.Name = "LblEdition";
            this.LblEdition.Size = new System.Drawing.Size(100, 32);
            this.LblEdition.TabIndex = 11;
            this.LblEdition.Text = "Edition :";
            // 
            // LblPages
            // 
            this.LblPages.AutoSize = true;
            this.LblPages.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblPages.Location = new System.Drawing.Point(537, 85);
            this.LblPages.Name = "LblPages";
            this.LblPages.Size = new System.Drawing.Size(87, 32);
            this.LblPages.TabIndex = 11;
            this.LblPages.Text = "Pages :";
            // 
            // LblLanguage
            // 
            this.LblLanguage.AutoSize = true;
            this.LblLanguage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblLanguage.Location = new System.Drawing.Point(537, 43);
            this.LblLanguage.Name = "LblLanguage";
            this.LblLanguage.Size = new System.Drawing.Size(130, 32);
            this.LblLanguage.TabIndex = 11;
            this.LblLanguage.Text = "Language :";
            // 
            // LblPublisher
            // 
            this.LblPublisher.AutoSize = true;
            this.LblPublisher.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblPublisher.Location = new System.Drawing.Point(12, 149);
            this.LblPublisher.Name = "LblPublisher";
            this.LblPublisher.Size = new System.Drawing.Size(124, 32);
            this.LblPublisher.TabIndex = 11;
            this.LblPublisher.Text = "Publisher :";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.panel3.Location = new System.Drawing.Point(56, 488);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1022, 1);
            this.panel3.TabIndex = 12;
            // 
            // FlwPnlBtns
            // 
            this.FlwPnlBtns.Controls.Add(this.BtnBorrow);
            this.FlwPnlBtns.Controls.Add(this.BtnReserve);
            this.FlwPnlBtns.Controls.Add(this.BtnDownloadLink);
            this.FlwPnlBtns.Location = new System.Drawing.Point(413, 391);
            this.FlwPnlBtns.Name = "FlwPnlBtns";
            this.FlwPnlBtns.Size = new System.Drawing.Size(574, 57);
            this.FlwPnlBtns.TabIndex = 10;
            // 
            // BtnBorrow
            // 
            this.BtnBorrow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnBorrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnBorrow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnBorrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBorrow.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnBorrow.ForeColor = System.Drawing.Color.White;
            this.BtnBorrow.Location = new System.Drawing.Point(4, 4);
            this.BtnBorrow.Margin = new System.Windows.Forms.Padding(4);
            this.BtnBorrow.Name = "BtnBorrow";
            this.BtnBorrow.Size = new System.Drawing.Size(155, 45);
            this.BtnBorrow.TabIndex = 77;
            this.BtnBorrow.Text = "📖 Borrow";
            this.BtnBorrow.UseVisualStyleBackColor = false;
            // 
            // BtnReserve
            // 
            this.BtnReserve.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnReserve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnReserve.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnReserve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnReserve.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnReserve.ForeColor = System.Drawing.Color.White;
            this.BtnReserve.Location = new System.Drawing.Point(167, 4);
            this.BtnReserve.Margin = new System.Windows.Forms.Padding(4);
            this.BtnReserve.Name = "BtnReserve";
            this.BtnReserve.Size = new System.Drawing.Size(155, 45);
            this.BtnReserve.TabIndex = 79;
            this.BtnReserve.Text = "📌 Reserve";
            this.BtnReserve.UseVisualStyleBackColor = false;
            this.BtnReserve.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnDownloadLink
            // 
            this.BtnDownloadLink.BackColor = System.Drawing.Color.White;
            this.BtnDownloadLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDownloadLink.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnDownloadLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDownloadLink.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnDownloadLink.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnDownloadLink.Location = new System.Drawing.Point(330, 4);
            this.BtnDownloadLink.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDownloadLink.Name = "BtnDownloadLink";
            this.BtnDownloadLink.Size = new System.Drawing.Size(216, 45);
            this.BtnDownloadLink.TabIndex = 78;
            this.BtnDownloadLink.Text = "📥 Download Link";
            this.BtnDownloadLink.UseVisualStyleBackColor = false;
            this.BtnDownloadLink.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // LblSubtitle
            // 
            this.LblSubtitle.AutoSize = true;
            this.LblSubtitle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.LblSubtitle.Location = new System.Drawing.Point(406, 179);
            this.LblSubtitle.Name = "LblSubtitle";
            this.LblSubtitle.Size = new System.Drawing.Size(111, 38);
            this.LblSubtitle.TabIndex = 9;
            this.LblSubtitle.Text = "Subtitle";
            // 
            // LblEditors
            // 
            this.LblEditors.AutoSize = true;
            this.LblEditors.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblEditors.Location = new System.Drawing.Point(407, 329);
            this.LblEditors.Name = "LblEditors";
            this.LblEditors.Size = new System.Drawing.Size(112, 32);
            this.LblEditors.TabIndex = 8;
            this.LblEditors.Text = "Editor(s) :";
            // 
            // LblAuthors
            // 
            this.LblAuthors.AutoSize = true;
            this.LblAuthors.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblAuthors.Location = new System.Drawing.Point(407, 297);
            this.LblAuthors.Name = "LblAuthors";
            this.LblAuthors.Size = new System.Drawing.Size(123, 32);
            this.LblAuthors.TabIndex = 7;
            this.LblAuthors.Text = "Author(s) :";
            // 
            // LblCategory
            // 
            this.LblCategory.AutoSize = true;
            this.LblCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.LblCategory.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCategory.ForeColor = System.Drawing.Color.White;
            this.LblCategory.Location = new System.Drawing.Point(416, 226);
            this.LblCategory.Name = "LblCategory";
            this.LblCategory.Size = new System.Drawing.Size(110, 32);
            this.LblCategory.TabIndex = 6;
            this.LblCategory.Text = "Category";
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(405, 131);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(94, 48);
            this.LblTitle.TabIndex = 3;
            this.LblTitle.Text = "Title";
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 8;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 150;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn2.Image")));
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 8;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Width = 150;
            // 
            // ViewBookDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1145, 666);
            this.Controls.Add(this.PnlDocked);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewBookDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View book details";
            this.PnlPicBxBookCoverContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCover)).EndInit();
            this.PnlDocked.ResumeLayout(false);
            this.PnlDocked.PerformLayout();
            this.PnlBookDetails.ResumeLayout(false);
            this.PnlBookDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgwBookCopy)).EndInit();
            this.FlwPnlBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlDesign;
        private System.Windows.Forms.PictureBox PicBxBookCover;
        private System.Windows.Forms.Panel PnlPicBxBookCoverContainer;
        private System.Windows.Forms.Panel PnlDocked;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Label LblCategory;
        private System.Windows.Forms.Label LblEditors;
        private System.Windows.Forms.Label LblAuthors;
        private System.Windows.Forms.Label LblSubtitle;
        private System.Windows.Forms.FlowLayoutPanel FlwPnlBtns;
        private System.Windows.Forms.Button BtnBorrow;
        private System.Windows.Forms.Button BtnReserve;
        private System.Windows.Forms.Button BtnDownloadLink;
        private System.Windows.Forms.Label LblStandardID;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label LblPublisher;
        private System.Windows.Forms.Label LblCallNumber;
        private System.Windows.Forms.Panel PnlBookDetails;
        private System.Windows.Forms.Label LblPages;
        private System.Windows.Forms.Label LblLanguage;
        private System.Windows.Forms.Label LblResourceType;
        private System.Windows.Forms.Label LblPhysDescFormat;
        private System.Windows.Forms.Label LblPubDateYear;
        private System.Windows.Forms.Label LblEdition;
        private System.Windows.Forms.DataGridView DgwBookCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumbering;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAccessionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddedBy;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBarcode;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
        private System.Windows.Forms.Label LblCopyDetails;
        private System.Windows.Forms.Label LblAvailableForBorrow;
        private System.Windows.Forms.Label LblTotalCopies;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
    }
}