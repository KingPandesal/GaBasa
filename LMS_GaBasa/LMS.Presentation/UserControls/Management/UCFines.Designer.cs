namespace LMS.Presentation.UserControls.Management
{
    partial class UCFines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCFines));
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.CmbBxStatusFilter = new System.Windows.Forms.ComboBox();
            this.CmbBxCategoryFilter = new System.Windows.Forms.ComboBox();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
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
            this.ColumnNumbering = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBtnCoverImage = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnBtnCopies = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgwInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(36, 35);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 87;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // CmbBxStatusFilter
            // 
            this.CmbBxStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxStatusFilter.FormattingEnabled = true;
            this.CmbBxStatusFilter.Items.AddRange(new object[] {
            "Available",
            "Available Online",
            "Out of Stock"});
            this.CmbBxStatusFilter.Location = new System.Drawing.Point(745, 35);
            this.CmbBxStatusFilter.Name = "CmbBxStatusFilter";
            this.CmbBxStatusFilter.Size = new System.Drawing.Size(215, 36);
            this.CmbBxStatusFilter.TabIndex = 90;
            this.CmbBxStatusFilter.Text = "All Status";
            // 
            // CmbBxCategoryFilter
            // 
            this.CmbBxCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxCategoryFilter.FormattingEnabled = true;
            this.CmbBxCategoryFilter.Location = new System.Drawing.Point(966, 35);
            this.CmbBxCategoryFilter.Name = "CmbBxCategoryFilter";
            this.CmbBxCategoryFilter.Size = new System.Drawing.Size(215, 36);
            this.CmbBxCategoryFilter.TabIndex = 89;
            this.CmbBxCategoryFilter.Text = "All Category";
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
            this.TxtSearchBar.Size = new System.Drawing.Size(570, 40);
            this.TxtSearchBar.TabIndex = 75;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
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
            // CmbBxResourceTypeFilter
            // 
            this.CmbBxResourceTypeFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxResourceTypeFilter.FormattingEnabled = true;
            this.CmbBxResourceTypeFilter.Items.AddRange(new object[] {
            "Physical Book",
            "Periodicals / Magazines",
            "Theses / Dissertations",
            "Audio-Visual Materials",
            "E-Books"});
            this.CmbBxResourceTypeFilter.Location = new System.Drawing.Point(1187, 35);
            this.CmbBxResourceTypeFilter.Name = "CmbBxResourceTypeFilter";
            this.CmbBxResourceTypeFilter.Size = new System.Drawing.Size(247, 36);
            this.CmbBxResourceTypeFilter.TabIndex = 86;
            this.CmbBxResourceTypeFilter.Text = "All Resource Type";
            // 
            // BtnApply
            // 
            this.BtnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApply.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnApply.ForeColor = System.Drawing.Color.White;
            this.BtnApply.Location = new System.Drawing.Point(1441, 32);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(104, 40);
            this.BtnApply.TabIndex = 85;
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
            this.LblSearch.TabIndex = 84;
            this.LblSearch.Text = "Search";
            // 
            // BtnAddBook
            // 
            this.BtnAddBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddBook.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAddBook.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddBook.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddBook.ForeColor = System.Drawing.Color.White;
            this.BtnAddBook.Location = new System.Drawing.Point(1388, 123);
            this.BtnAddBook.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddBook.Name = "BtnAddBook";
            this.BtnAddBook.Size = new System.Drawing.Size(153, 46);
            this.BtnAddBook.TabIndex = 83;
            this.BtnAddBook.Text = "+ Add Book";
            this.BtnAddBook.UseVisualStyleBackColor = false;
            // 
            // LblPaginationPrevious
            // 
            this.LblPaginationPrevious.BackColor = System.Drawing.Color.White;
            this.LblPaginationPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblPaginationPrevious.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationPrevious.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.Location = new System.Drawing.Point(1276, 711);
            this.LblPaginationPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationPrevious.Name = "LblPaginationPrevious";
            this.LblPaginationPrevious.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationPrevious.TabIndex = 82;
            this.LblPaginationPrevious.Text = "Previous";
            this.LblPaginationPrevious.UseVisualStyleBackColor = false;
            // 
            // LblPaginationNext
            // 
            this.LblPaginationNext.BackColor = System.Drawing.Color.White;
            this.LblPaginationNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblPaginationNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationNext.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationNext.Location = new System.Drawing.Point(1420, 711);
            this.LblPaginationNext.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationNext.Name = "LblPaginationNext";
            this.LblPaginationNext.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationNext.TabIndex = 81;
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
            this.LblPaginationShowEntries.TabIndex = 80;
            this.LblPaginationShowEntries.Text = "Showing 1 to 5 of 100 entries";
            // 
            // LblEntries
            // 
            this.LblEntries.AutoSize = true;
            this.LblEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntries.Location = new System.Drawing.Point(174, 132);
            this.LblEntries.Name = "LblEntries";
            this.LblEntries.Size = new System.Drawing.Size(70, 28);
            this.LblEntries.TabIndex = 79;
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
            this.CmbBxPaginationNumbers.TabIndex = 78;
            this.CmbBxPaginationNumbers.Text = "10";
            // 
            // LblShow
            // 
            this.LblShow.AutoSize = true;
            this.LblShow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblShow.Location = new System.Drawing.Point(28, 132);
            this.LblShow.Name = "LblShow";
            this.LblShow.Size = new System.Drawing.Size(60, 28);
            this.LblShow.TabIndex = 77;
            this.LblShow.Text = "Show";
            // 
            // DgwInventory
            // 
            this.DgwInventory.AllowUserToAddRows = false;
            this.DgwInventory.BackgroundColor = System.Drawing.Color.White;
            this.DgwInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgwInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNumbering,
            this.ColumnBtnCoverImage,
            this.ColumnBtnCopies,
            this.Edit});
            this.DgwInventory.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgwInventory.Location = new System.Drawing.Point(36, 200);
            this.DgwInventory.Name = "DgwInventory";
            this.DgwInventory.RowHeadersWidth = 62;
            this.DgwInventory.RowTemplate.Height = 28;
            this.DgwInventory.Size = new System.Drawing.Size(1509, 490);
            this.DgwInventory.TabIndex = 76;
            // 
            // ColumnNumbering
            // 
            this.ColumnNumbering.HeaderText = "#";
            this.ColumnNumbering.MinimumWidth = 8;
            this.ColumnNumbering.Name = "ColumnNumbering";
            this.ColumnNumbering.Width = 50;
            // 
            // ColumnBtnCoverImage
            // 
            this.ColumnBtnCoverImage.HeaderText = "Cover Image";
            this.ColumnBtnCoverImage.MinimumWidth = 8;
            this.ColumnBtnCoverImage.Name = "ColumnBtnCoverImage";
            this.ColumnBtnCoverImage.Text = "View Cover Image";
            this.ColumnBtnCoverImage.Width = 150;
            // 
            // ColumnBtnCopies
            // 
            this.ColumnBtnCopies.HeaderText = "Copies";
            this.ColumnBtnCopies.MinimumWidth = 8;
            this.ColumnBtnCopies.Name = "ColumnBtnCopies";
            this.ColumnBtnCopies.Text = "View All Copies";
            this.ColumnBtnCopies.Width = 150;
            // 
            // Edit
            // 
            this.Edit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Edit.HeaderText = "";
            this.Edit.Image = ((System.Drawing.Image)(resources.GetObject("Edit.Image")));
            this.Edit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Edit.MinimumWidth = 8;
            this.Edit.Name = "Edit";
            this.Edit.ToolTipText = "Edit book";
            this.Edit.Width = 8;
            // 
            // UCFines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.CmbBxStatusFilter);
            this.Controls.Add(this.CmbBxCategoryFilter);
            this.Controls.Add(this.TxtSearchBar);
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
            this.Name = "UCFines";
            this.Size = new System.Drawing.Size(1580, 936);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgwInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.ComboBox CmbBxStatusFilter;
        private System.Windows.Forms.ComboBox CmbBxCategoryFilter;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumbering;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBtnCoverImage;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBtnCopies;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
    }
}
