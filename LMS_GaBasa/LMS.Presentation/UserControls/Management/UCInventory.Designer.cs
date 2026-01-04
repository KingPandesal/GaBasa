namespace LMS.Presentation.UserControls.Management
{
    partial class UCInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCInventory));
            this.CmbBxResourceTypeFilter = new System.Windows.Forms.ComboBox();
            this.BtnApply = new System.Windows.Forms.Button();
            this.LblSearch = new System.Windows.Forms.Label();
            this.BtnAddBook = new System.Windows.Forms.Button();
            this.LblPaginationPrevious = new System.Windows.Forms.Button();
            this.LblPaginationNext = new System.Windows.Forms.Button();
            this.LblPaginationShowEntries = new System.Windows.Forms.Label();
            this.LblEntries = new System.Windows.Forms.Label();
            this.CmbBxPaginationNumbers = new System.Windows.Forms.ComboBox();
            this.LblShow = new System.Windows.Forms.Label();
            this.DgwInventory = new System.Windows.Forms.DataGridView();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Archive = new System.Windows.Forms.DataGridViewImageColumn();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.BtnImport = new System.Windows.Forms.Button();
            this.CmbBxCategoryFilter = new System.Windows.Forms.ComboBox();
            this.CmbBxStatusFilter = new System.Windows.Forms.ComboBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgwInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // CmbBxResourceTypeFilter
            // 
            this.CmbBxResourceTypeFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxResourceTypeFilter.FormattingEnabled = true;
            this.CmbBxResourceTypeFilter.Location = new System.Drawing.Point(1241, 35);
            this.CmbBxResourceTypeFilter.Name = "CmbBxResourceTypeFilter";
            this.CmbBxResourceTypeFilter.Size = new System.Drawing.Size(185, 36);
            this.CmbBxResourceTypeFilter.TabIndex = 69;
            this.CmbBxResourceTypeFilter.Text = "All Resource Type";
            // 
            // BtnApply
            // 
            this.BtnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApply.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnApply.ForeColor = System.Drawing.Color.White;
            this.BtnApply.Location = new System.Drawing.Point(1441, 32);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(104, 40);
            this.BtnApply.TabIndex = 68;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 36);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 67;
            this.LblSearch.Text = "Search";
            // 
            // BtnAddBook
            // 
            this.BtnAddBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddBook.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddBook.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddBook.ForeColor = System.Drawing.Color.White;
            this.BtnAddBook.Location = new System.Drawing.Point(1388, 123);
            this.BtnAddBook.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddBook.Name = "BtnAddBook";
            this.BtnAddBook.Size = new System.Drawing.Size(153, 46);
            this.BtnAddBook.TabIndex = 65;
            this.BtnAddBook.Text = "+ Add Book";
            this.BtnAddBook.UseVisualStyleBackColor = false;
            this.BtnAddBook.Click += new System.EventHandler(this.BtnAddBook_Click);
            // 
            // LblPaginationPrevious
            // 
            this.LblPaginationPrevious.BackColor = System.Drawing.Color.White;
            this.LblPaginationPrevious.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationPrevious.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.Location = new System.Drawing.Point(1276, 711);
            this.LblPaginationPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationPrevious.Name = "LblPaginationPrevious";
            this.LblPaginationPrevious.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationPrevious.TabIndex = 64;
            this.LblPaginationPrevious.Text = "Previous";
            this.LblPaginationPrevious.UseVisualStyleBackColor = false;
            this.LblPaginationPrevious.Click += new System.EventHandler(this.LblPaginationPrevious_Click);
            // 
            // LblPaginationNext
            // 
            this.LblPaginationNext.BackColor = System.Drawing.Color.White;
            this.LblPaginationNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationNext.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationNext.Location = new System.Drawing.Point(1420, 711);
            this.LblPaginationNext.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationNext.Name = "LblPaginationNext";
            this.LblPaginationNext.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationNext.TabIndex = 63;
            this.LblPaginationNext.Text = "Next";
            this.LblPaginationNext.UseVisualStyleBackColor = false;
            // 
            // LblPaginationShowEntries
            // 
            this.LblPaginationShowEntries.AutoSize = true;
            this.LblPaginationShowEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPaginationShowEntries.Location = new System.Drawing.Point(31, 720);
            this.LblPaginationShowEntries.Name = "LblPaginationShowEntries";
            this.LblPaginationShowEntries.Size = new System.Drawing.Size(268, 28);
            this.LblPaginationShowEntries.TabIndex = 62;
            this.LblPaginationShowEntries.Text = "Showing 1 to 5 of 100 entries";
            // 
            // LblEntries
            // 
            this.LblEntries.AutoSize = true;
            this.LblEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntries.Location = new System.Drawing.Point(174, 132);
            this.LblEntries.Name = "LblEntries";
            this.LblEntries.Size = new System.Drawing.Size(70, 28);
            this.LblEntries.TabIndex = 61;
            this.LblEntries.Text = "entries";
            // 
            // CmbBxPaginationNumbers
            // 
            this.CmbBxPaginationNumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CmbBxPaginationNumbers.FormattingEnabled = true;
            this.CmbBxPaginationNumbers.Items.AddRange(new object[] {
            "10",
            "20",
            "30"});
            this.CmbBxPaginationNumbers.Location = new System.Drawing.Point(93, 132);
            this.CmbBxPaginationNumbers.Name = "CmbBxPaginationNumbers";
            this.CmbBxPaginationNumbers.Size = new System.Drawing.Size(75, 33);
            this.CmbBxPaginationNumbers.TabIndex = 60;
            this.CmbBxPaginationNumbers.Text = "10";
            // 
            // LblShow
            // 
            this.LblShow.AutoSize = true;
            this.LblShow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblShow.Location = new System.Drawing.Point(28, 132);
            this.LblShow.Name = "LblShow";
            this.LblShow.Size = new System.Drawing.Size(60, 28);
            this.LblShow.TabIndex = 59;
            this.LblShow.Text = "Show";
            // 
            // DgwInventory
            // 
            this.DgwInventory.AllowUserToAddRows = false;
            this.DgwInventory.BackgroundColor = System.Drawing.Color.White;
            this.DgwInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgwInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column14,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column16,
            this.Edit,
            this.Column17,
            this.Archive});
            this.DgwInventory.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgwInventory.Location = new System.Drawing.Point(36, 200);
            this.DgwInventory.Name = "DgwInventory";
            this.DgwInventory.RowHeadersWidth = 62;
            this.DgwInventory.RowTemplate.Height = 28;
            this.DgwInventory.Size = new System.Drawing.Size(1509, 490);
            this.DgwInventory.TabIndex = 58;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "#";
            this.Column14.MinimumWidth = 8;
            this.Column14.Name = "Column14";
            this.Column14.Width = 50;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Book ID";
            this.Column1.MinimumWidth = 8;
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "ISBN";
            this.Column2.MinimumWidth = 8;
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Call Number";
            this.Column3.MinimumWidth = 8;
            this.Column3.Name = "Column3";
            this.Column3.Width = 150;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Title";
            this.Column4.MinimumWidth = 8;
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Subtitle";
            this.Column5.MinimumWidth = 8;
            this.Column5.Name = "Column5";
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Publishers";
            this.Column6.MinimumWidth = 8;
            this.Column6.Name = "Column6";
            this.Column6.Width = 150;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Category";
            this.Column7.MinimumWidth = 8;
            this.Column7.Name = "Column7";
            this.Column7.Width = 150;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Language";
            this.Column8.MinimumWidth = 8;
            this.Column8.Name = "Column8";
            this.Column8.Width = 150;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Pages";
            this.Column9.MinimumWidth = 8;
            this.Column9.Name = "Column9";
            this.Column9.Width = 150;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Edition";
            this.Column10.MinimumWidth = 8;
            this.Column10.Name = "Column10";
            this.Column10.Width = 150;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Publication Year";
            this.Column11.MinimumWidth = 8;
            this.Column11.Name = "Column11";
            this.Column11.Width = 150;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Description";
            this.Column12.MinimumWidth = 8;
            this.Column12.Name = "Column12";
            this.Column12.Width = 150;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Resource Type";
            this.Column13.MinimumWidth = 8;
            this.Column13.Name = "Column13";
            this.Column13.Width = 150;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "Download URL";
            this.Column16.MinimumWidth = 8;
            this.Column16.Name = "Column16";
            this.Column16.Width = 150;
            // 
            // Edit
            // 
            this.Edit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Edit.HeaderText = "";
            this.Edit.Image = ((System.Drawing.Image)(resources.GetObject("Edit.Image")));
            this.Edit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Edit.MinimumWidth = 8;
            this.Edit.Name = "Edit";
            this.Edit.ToolTipText = "Edit user";
            this.Edit.Width = 8;
            // 
            // Column17
            // 
            this.Column17.HeaderText = "Image";
            this.Column17.MinimumWidth = 8;
            this.Column17.Name = "Column17";
            this.Column17.Text = "View Cover Image";
            this.Column17.Width = 150;
            // 
            // Archive
            // 
            this.Archive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Archive.HeaderText = "";
            this.Archive.Image = ((System.Drawing.Image)(resources.GetObject("Archive.Image")));
            this.Archive.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Archive.MinimumWidth = 8;
            this.Archive.Name = "Archive";
            this.Archive.ToolTipText = "Archive user";
            this.Archive.Width = 8;
            // 
            // TxtSearchBar
            // 
            this.TxtSearchBar.BackColor = System.Drawing.Color.White;
            this.TxtSearchBar.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.TxtSearchBar.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearchBar.Image = null;
            this.TxtSearchBar.Location = new System.Drawing.Point(155, 32);
            this.TxtSearchBar.MaximumSize = new System.Drawing.Size(897, 40);
            this.TxtSearchBar.MaxLength = 32767;
            this.TxtSearchBar.MinimumSize = new System.Drawing.Size(0, 33);
            this.TxtSearchBar.Multiline = false;
            this.TxtSearchBar.Name = "TxtSearchBar";
            this.TxtSearchBar.ReadOnly = false;
            this.TxtSearchBar.Size = new System.Drawing.Size(761, 40);
            this.TxtSearchBar.TabIndex = 57;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // BtnImport
            // 
            this.BtnImport.BackColor = System.Drawing.Color.White;
            this.BtnImport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnImport.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnImport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnImport.Location = new System.Drawing.Point(1236, 123);
            this.BtnImport.Margin = new System.Windows.Forms.Padding(4);
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(135, 46);
            this.BtnImport.TabIndex = 71;
            this.BtnImport.Text = "📥 Import";
            this.BtnImport.UseVisualStyleBackColor = false;
            this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // CmbBxCategoryFilter
            // 
            this.CmbBxCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxCategoryFilter.FormattingEnabled = true;
            this.CmbBxCategoryFilter.Location = new System.Drawing.Point(1085, 35);
            this.CmbBxCategoryFilter.Name = "CmbBxCategoryFilter";
            this.CmbBxCategoryFilter.Size = new System.Drawing.Size(146, 36);
            this.CmbBxCategoryFilter.TabIndex = 73;
            this.CmbBxCategoryFilter.Text = "All Category";
            // 
            // CmbBxStatusFilter
            // 
            this.CmbBxStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxStatusFilter.FormattingEnabled = true;
            this.CmbBxStatusFilter.Location = new System.Drawing.Point(937, 35);
            this.CmbBxStatusFilter.Name = "CmbBxStatusFilter";
            this.CmbBxStatusFilter.Size = new System.Drawing.Size(139, 36);
            this.CmbBxStatusFilter.TabIndex = 74;
            this.CmbBxStatusFilter.Text = "All Status";
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 8;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ToolTipText = "Edit user";
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
            this.dataGridViewImageColumn2.ToolTipText = "Archive user";
            this.dataGridViewImageColumn2.Width = 150;
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(36, 35);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 70;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // UCInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.CmbBxStatusFilter);
            this.Controls.Add(this.CmbBxCategoryFilter);
            this.Controls.Add(this.BtnImport);
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.CmbBxResourceTypeFilter);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.BtnAddBook);
            this.Controls.Add(this.LblPaginationPrevious);
            this.Controls.Add(this.LblPaginationNext);
            this.Controls.Add(this.LblPaginationShowEntries);
            this.Controls.Add(this.LblEntries);
            this.Controls.Add(this.CmbBxPaginationNumbers);
            this.Controls.Add(this.LblShow);
            this.Controls.Add(this.DgwInventory);
            this.Controls.Add(this.TxtSearchBar);
            this.Name = "UCInventory";
            this.Size = new System.Drawing.Size(1580, 936);
            ((System.ComponentModel.ISupportInitialize)(this.DgwInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.ComboBox CmbBxResourceTypeFilter;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Label LblSearch;
        private System.Windows.Forms.Button BtnAddBook;
        private System.Windows.Forms.Button LblPaginationPrevious;
        private System.Windows.Forms.Button LblPaginationNext;
        private System.Windows.Forms.Label LblPaginationShowEntries;
        private System.Windows.Forms.Label LblEntries;
        private System.Windows.Forms.ComboBox CmbBxPaginationNumbers;
        private System.Windows.Forms.Label LblShow;
        private System.Windows.Forms.DataGridView DgwInventory;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.ComboBox CmbBxCategoryFilter;
        private System.Windows.Forms.ComboBox CmbBxStatusFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewButtonColumn Column17;
        private System.Windows.Forms.DataGridViewImageColumn Archive;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
    }
}
