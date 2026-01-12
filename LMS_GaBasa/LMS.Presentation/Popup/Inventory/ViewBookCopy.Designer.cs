namespace LMS.Presentation.Popup.Inventory
{
    partial class ViewBookCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewBookCopy));
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
            this.BtnAddBookCopy = new System.Windows.Forms.Button();
            this.CmbBxStatusFilter = new System.Windows.Forms.ComboBox();
            this.CmbBxLocationFilter = new System.Windows.Forms.ComboBox();
            this.BtnApply = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.LblCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.LblAvailableForBorrow = new System.Windows.Forms.Label();
            this.LblTotalCopies = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgwBookCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.SuspendLayout();
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
            this.DgwBookCopy.Location = new System.Drawing.Point(32, 158);
            this.DgwBookCopy.Name = "DgwBookCopy";
            this.DgwBookCopy.RowHeadersWidth = 62;
            this.DgwBookCopy.RowTemplate.Height = 28;
            this.DgwBookCopy.Size = new System.Drawing.Size(910, 326);
            this.DgwBookCopy.TabIndex = 59;
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
            // BtnAddBookCopy
            // 
            this.BtnAddBookCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddBookCopy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddBookCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddBookCopy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddBookCopy.ForeColor = System.Drawing.Color.White;
            this.BtnAddBookCopy.Location = new System.Drawing.Point(789, 105);
            this.BtnAddBookCopy.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddBookCopy.Name = "BtnAddBookCopy";
            this.BtnAddBookCopy.Size = new System.Drawing.Size(153, 46);
            this.BtnAddBookCopy.TabIndex = 66;
            this.BtnAddBookCopy.Text = "+ Add Copy";
            this.BtnAddBookCopy.UseVisualStyleBackColor = false;
            // 
            // CmbBxStatusFilter
            // 
            this.CmbBxStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxStatusFilter.FormattingEnabled = true;
            this.CmbBxStatusFilter.Items.AddRange(new object[] {
            "Available",
            "Borrowed",
            "Reserved",
            "Lost",
            "Damaged",
            "Repair"});
            this.CmbBxStatusFilter.Location = new System.Drawing.Point(440, 27);
            this.CmbBxStatusFilter.Name = "CmbBxStatusFilter";
            this.CmbBxStatusFilter.Size = new System.Drawing.Size(193, 36);
            this.CmbBxStatusFilter.TabIndex = 79;
            this.CmbBxStatusFilter.Text = "All Status";
            // 
            // CmbBxLocationFilter
            // 
            this.CmbBxLocationFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxLocationFilter.FormattingEnabled = true;
            this.CmbBxLocationFilter.Location = new System.Drawing.Point(639, 26);
            this.CmbBxLocationFilter.Name = "CmbBxLocationFilter";
            this.CmbBxLocationFilter.Size = new System.Drawing.Size(193, 36);
            this.CmbBxLocationFilter.TabIndex = 78;
            this.CmbBxLocationFilter.Text = "All Location";
            // 
            // BtnApply
            // 
            this.BtnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApply.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnApply.ForeColor = System.Drawing.Color.White;
            this.BtnApply.Location = new System.Drawing.Point(839, 24);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(104, 40);
            this.BtnApply.TabIndex = 76;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = false;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 8;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 150;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn2.Image")));
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 8;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Width = 150;
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(845, 497);
            this.LblCancel.Margin = new System.Windows.Forms.Padding(4);
            this.LblCancel.Name = "LblCancel";
            this.LblCancel.Size = new System.Drawing.Size(97, 43);
            this.LblCancel.TabIndex = 92;
            this.LblCancel.Text = "Cancel";
            this.LblCancel.UseVisualStyleBackColor = false;
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.Location = new System.Drawing.Point(740, 497);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 43);
            this.BtnSave.TabIndex = 91;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = false;
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(33, 30);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 95;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(72, 31);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 94;
            this.LblSearch.Text = "Search";
            // 
            // TxtSearchBar
            // 
            this.TxtSearchBar.BackColor = System.Drawing.Color.White;
            this.TxtSearchBar.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.TxtSearchBar.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearchBar.Image = null;
            this.TxtSearchBar.Location = new System.Drawing.Point(148, 24);
            this.TxtSearchBar.MaximumSize = new System.Drawing.Size(897, 40);
            this.TxtSearchBar.MaxLength = 32767;
            this.TxtSearchBar.MinimumSize = new System.Drawing.Size(0, 33);
            this.TxtSearchBar.Multiline = false;
            this.TxtSearchBar.Name = "TxtSearchBar";
            this.TxtSearchBar.ReadOnly = false;
            this.TxtSearchBar.Size = new System.Drawing.Size(282, 40);
            this.TxtSearchBar.TabIndex = 93;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // LblAvailableForBorrow
            // 
            this.LblAvailableForBorrow.AutoSize = true;
            this.LblAvailableForBorrow.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAvailableForBorrow.Location = new System.Drawing.Point(28, 116);
            this.LblAvailableForBorrow.Name = "LblAvailableForBorrow";
            this.LblAvailableForBorrow.Size = new System.Drawing.Size(195, 28);
            this.LblAvailableForBorrow.TabIndex = 96;
            this.LblAvailableForBorrow.Text = "Available for borrow:";
            // 
            // LblTotalCopies
            // 
            this.LblTotalCopies.AutoSize = true;
            this.LblTotalCopies.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblTotalCopies.Location = new System.Drawing.Point(28, 79);
            this.LblTotalCopies.Name = "LblTotalCopies";
            this.LblTotalCopies.Size = new System.Drawing.Size(127, 28);
            this.LblTotalCopies.TabIndex = 97;
            this.LblTotalCopies.Text = "Total Copies :";
            // 
            // ViewBookCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(978, 553);
            this.Controls.Add(this.LblAvailableForBorrow);
            this.Controls.Add(this.LblTotalCopies);
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.TxtSearchBar);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.CmbBxStatusFilter);
            this.Controls.Add(this.CmbBxLocationFilter);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.BtnAddBookCopy);
            this.Controls.Add(this.DgwBookCopy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewBookCopy";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View book copy";
            ((System.ComponentModel.ISupportInitialize)(this.DgwBookCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgwBookCopy;
        private System.Windows.Forms.Button BtnAddBookCopy;
        private System.Windows.Forms.ComboBox CmbBxStatusFilter;
        private System.Windows.Forms.ComboBox CmbBxLocationFilter;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumbering;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAccessionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddedBy;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBarcode;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.Label LblSearch;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.Label LblAvailableForBorrow;
        private System.Windows.Forms.Label LblTotalCopies;
    }
}