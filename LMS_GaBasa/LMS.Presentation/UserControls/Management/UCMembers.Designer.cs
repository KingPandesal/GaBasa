namespace LMS.Presentation.UserControls.Management
{
    partial class UCMembers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMembers));
            this.CmbBxStatusFilter = new System.Windows.Forms.ComboBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.CmbBxMemberTypeFilter = new System.Windows.Forms.ComboBox();
            this.BtnAddMember = new System.Windows.Forms.Button();
            this.LblPaginationPrevious = new System.Windows.Forms.Button();
            this.LblPaginationNext = new System.Windows.Forms.Button();
            this.LblPaginationShowEntries = new System.Windows.Forms.Label();
            this.LblEntries = new System.Windows.Forms.Label();
            this.CmbBxPaginationNumbers = new System.Windows.Forms.ComboBox();
            this.LblShow = new System.Windows.Forms.Label();
            this.BtnApply = new System.Windows.Forms.Button();
            this.DgwMembers = new System.Windows.Forms.DataGridView();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLastLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Archive = new System.Windows.Forms.DataGridViewImageColumn();
            this.RenewMembership = new System.Windows.Forms.DataGridViewImageColumn();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgwMembers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CmbBxStatusFilter
            // 
            this.CmbBxStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxStatusFilter.FormattingEnabled = true;
            this.CmbBxStatusFilter.Items.AddRange(new object[] {
            "5",
            "10",
            "15"});
            this.CmbBxStatusFilter.Location = new System.Drawing.Point(1076, 32);
            this.CmbBxStatusFilter.Name = "CmbBxStatusFilter";
            this.CmbBxStatusFilter.Size = new System.Drawing.Size(139, 36);
            this.CmbBxStatusFilter.TabIndex = 54;
            this.CmbBxStatusFilter.Text = "Status";
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 35);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 52;
            this.LblSearch.Text = "Search";
            // 
            // CmbBxMemberTypeFilter
            // 
            this.CmbBxMemberTypeFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxMemberTypeFilter.FormattingEnabled = true;
            this.CmbBxMemberTypeFilter.Location = new System.Drawing.Point(1232, 32);
            this.CmbBxMemberTypeFilter.Name = "CmbBxMemberTypeFilter";
            this.CmbBxMemberTypeFilter.Size = new System.Drawing.Size(192, 36);
            this.CmbBxMemberTypeFilter.TabIndex = 51;
            this.CmbBxMemberTypeFilter.Text = "Faculty";
            // 
            // BtnAddMember
            // 
            this.BtnAddMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAddMember.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddMember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddMember.ForeColor = System.Drawing.Color.White;
            this.BtnAddMember.Location = new System.Drawing.Point(1351, 122);
            this.BtnAddMember.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddMember.Name = "BtnAddMember";
            this.BtnAddMember.Size = new System.Drawing.Size(190, 46);
            this.BtnAddMember.TabIndex = 50;
            this.BtnAddMember.Text = "+ Add Member";
            this.BtnAddMember.UseVisualStyleBackColor = false;
            this.BtnAddMember.Click += new System.EventHandler(this.BtnAddMember_Click);
            // 
            // LblPaginationPrevious
            // 
            this.LblPaginationPrevious.BackColor = System.Drawing.Color.White;
            this.LblPaginationPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblPaginationPrevious.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationPrevious.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.Location = new System.Drawing.Point(1276, 715);
            this.LblPaginationPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationPrevious.Name = "LblPaginationPrevious";
            this.LblPaginationPrevious.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationPrevious.TabIndex = 49;
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
            this.LblPaginationNext.Location = new System.Drawing.Point(1420, 715);
            this.LblPaginationNext.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationNext.Name = "LblPaginationNext";
            this.LblPaginationNext.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationNext.TabIndex = 48;
            this.LblPaginationNext.Text = "Next";
            this.LblPaginationNext.UseVisualStyleBackColor = false;
            // 
            // LblPaginationShowEntries
            // 
            this.LblPaginationShowEntries.AutoSize = true;
            this.LblPaginationShowEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPaginationShowEntries.Location = new System.Drawing.Point(31, 724);
            this.LblPaginationShowEntries.Name = "LblPaginationShowEntries";
            this.LblPaginationShowEntries.Size = new System.Drawing.Size(268, 28);
            this.LblPaginationShowEntries.TabIndex = 47;
            this.LblPaginationShowEntries.Text = "Showing 1 to 5 of 100 entries";
            // 
            // LblEntries
            // 
            this.LblEntries.AutoSize = true;
            this.LblEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntries.Location = new System.Drawing.Point(172, 131);
            this.LblEntries.Name = "LblEntries";
            this.LblEntries.Size = new System.Drawing.Size(70, 28);
            this.LblEntries.TabIndex = 46;
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
            this.CmbBxPaginationNumbers.Location = new System.Drawing.Point(93, 131);
            this.CmbBxPaginationNumbers.Name = "CmbBxPaginationNumbers";
            this.CmbBxPaginationNumbers.Size = new System.Drawing.Size(75, 33);
            this.CmbBxPaginationNumbers.TabIndex = 45;
            this.CmbBxPaginationNumbers.Text = "10";
            // 
            // LblShow
            // 
            this.LblShow.AutoSize = true;
            this.LblShow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblShow.Location = new System.Drawing.Point(28, 131);
            this.LblShow.Name = "LblShow";
            this.LblShow.Size = new System.Drawing.Size(60, 28);
            this.LblShow.TabIndex = 44;
            this.LblShow.Text = "Show";
            // 
            // BtnApply
            // 
            this.BtnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApply.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnApply.ForeColor = System.Drawing.Color.White;
            this.BtnApply.Location = new System.Drawing.Point(1441, 30);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(104, 40);
            this.BtnApply.TabIndex = 53;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = false;
            // 
            // DgwMembers
            // 
            this.DgwMembers.AllowUserToAddRows = false;
            this.DgwMembers.BackgroundColor = System.Drawing.Color.White;
            this.DgwMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgwMembers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column17,
            this.Column4,
            this.Column1,
            this.Column11,
            this.Column2,
            this.Column3,
            this.Column8,
            this.Column6,
            this.Column12,
            this.Column13,
            this.Column14,
            this.Column15,
            this.Column16,
            this.Column9,
            this.Column10,
            this.ColumnLastLogin,
            this.Column5,
            this.Edit,
            this.Archive,
            this.RenewMembership});
            this.DgwMembers.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgwMembers.Location = new System.Drawing.Point(36, 199);
            this.DgwMembers.Name = "DgwMembers";
            this.DgwMembers.RowHeadersWidth = 62;
            this.DgwMembers.RowTemplate.Height = 28;
            this.DgwMembers.Size = new System.Drawing.Size(1509, 490);
            this.DgwMembers.TabIndex = 43;
            // 
            // Column17
            // 
            this.Column17.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column17.HeaderText = "#";
            this.Column17.MinimumWidth = 8;
            this.Column17.Name = "Column17";
            this.Column17.Width = 54;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Member ID";
            this.Column4.MinimumWidth = 8;
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Full Name";
            this.Column1.MinimumWidth = 20;
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Member Type";
            this.Column11.MinimumWidth = 8;
            this.Column11.Name = "Column11";
            this.Column11.Width = 150;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Username";
            this.Column2.MinimumWidth = 8;
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Email";
            this.Column3.MinimumWidth = 8;
            this.Column3.Name = "Column3";
            this.Column3.Width = 200;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Address";
            this.Column8.MinimumWidth = 8;
            this.Column8.Name = "Column8";
            this.Column8.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Contact Number";
            this.Column6.MinimumWidth = 8;
            this.Column6.Name = "Column6";
            this.Column6.Width = 150;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Max Books Allowed";
            this.Column12.MinimumWidth = 8;
            this.Column12.Name = "Column12";
            this.Column12.Width = 150;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Borrowing Period";
            this.Column13.MinimumWidth = 8;
            this.Column13.Name = "Column13";
            this.Column13.Width = 150;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Renewal Limit";
            this.Column14.MinimumWidth = 8;
            this.Column14.Name = "Column14";
            this.Column14.Width = 150;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "Reservation Privilege";
            this.Column15.MinimumWidth = 8;
            this.Column15.Name = "Column15";
            this.Column15.Width = 150;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "Fine Rate";
            this.Column16.MinimumWidth = 8;
            this.Column16.Name = "Column16";
            this.Column16.Width = 150;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Registration Date";
            this.Column9.MinimumWidth = 8;
            this.Column9.Name = "Column9";
            this.Column9.Width = 150;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Expiration Date";
            this.Column10.MinimumWidth = 8;
            this.Column10.Name = "Column10";
            this.Column10.Width = 150;
            // 
            // ColumnLastLogin
            // 
            this.ColumnLastLogin.HeaderText = "Last Login";
            this.ColumnLastLogin.MinimumWidth = 8;
            this.ColumnLastLogin.Name = "ColumnLastLogin";
            this.ColumnLastLogin.Width = 150;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Status";
            this.Column5.MinimumWidth = 8;
            this.Column5.Name = "Column5";
            this.Column5.Width = 150;
            // 
            // Edit
            // 
            this.Edit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Edit.HeaderText = "";
            this.Edit.Image = ((System.Drawing.Image)(resources.GetObject("Edit.Image")));
            this.Edit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Edit.MinimumWidth = 8;
            this.Edit.Name = "Edit";
            this.Edit.ToolTipText = "Edit member";
            this.Edit.Width = 8;
            // 
            // Archive
            // 
            this.Archive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Archive.HeaderText = "";
            this.Archive.Image = ((System.Drawing.Image)(resources.GetObject("Archive.Image")));
            this.Archive.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Archive.MinimumWidth = 8;
            this.Archive.Name = "Archive";
            this.Archive.ToolTipText = "Archive member";
            this.Archive.Width = 8;
            // 
            // RenewMembership
            // 
            this.RenewMembership.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RenewMembership.HeaderText = "";
            this.RenewMembership.Image = ((System.Drawing.Image)(resources.GetObject("RenewMembership.Image")));
            this.RenewMembership.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.RenewMembership.MinimumWidth = 8;
            this.RenewMembership.Name = "RenewMembership";
            this.RenewMembership.Width = 8;
            // 
            // TxtSearchBar
            // 
            this.TxtSearchBar.BackColor = System.Drawing.Color.White;
            this.TxtSearchBar.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.TxtSearchBar.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearchBar.Image = null;
            this.TxtSearchBar.Location = new System.Drawing.Point(155, 31);
            this.TxtSearchBar.MaximumSize = new System.Drawing.Size(897, 40);
            this.TxtSearchBar.MaxLength = 32767;
            this.TxtSearchBar.MinimumSize = new System.Drawing.Size(0, 33);
            this.TxtSearchBar.Multiline = false;
            this.TxtSearchBar.Name = "TxtSearchBar";
            this.TxtSearchBar.ReadOnly = false;
            this.TxtSearchBar.Size = new System.Drawing.Size(897, 40);
            this.TxtSearchBar.TabIndex = 42;
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
            this.dataGridViewImageColumn1.ToolTipText = "Edit member";
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
            this.dataGridViewImageColumn2.ToolTipText = "Archive member";
            this.dataGridViewImageColumn2.Width = 150;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(34, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 55;
            this.pictureBox1.TabStop = false;
            // 
            // UCMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.CmbBxStatusFilter);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.CmbBxMemberTypeFilter);
            this.Controls.Add(this.BtnAddMember);
            this.Controls.Add(this.LblPaginationPrevious);
            this.Controls.Add(this.LblPaginationNext);
            this.Controls.Add(this.LblPaginationShowEntries);
            this.Controls.Add(this.LblEntries);
            this.Controls.Add(this.CmbBxPaginationNumbers);
            this.Controls.Add(this.LblShow);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.DgwMembers);
            this.Controls.Add(this.TxtSearchBar);
            this.Name = "UCMembers";
            this.Size = new System.Drawing.Size(1580, 832);
            ((System.ComponentModel.ISupportInitialize)(this.DgwMembers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbBxStatusFilter;
        private System.Windows.Forms.Label LblSearch;
        private System.Windows.Forms.ComboBox CmbBxMemberTypeFilter;
        private System.Windows.Forms.Button BtnAddMember;
        private System.Windows.Forms.Button LblPaginationPrevious;
        private System.Windows.Forms.Button LblPaginationNext;
        private System.Windows.Forms.Label LblPaginationShowEntries;
        private System.Windows.Forms.Label LblEntries;
        private System.Windows.Forms.ComboBox CmbBxPaginationNumbers;
        private System.Windows.Forms.Label LblShow;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.DataGridView DgwMembers;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column17;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLastLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Archive;
        private System.Windows.Forms.DataGridViewImageColumn RenewMembership;
    }
}
