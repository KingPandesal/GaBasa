namespace LMS.Presentation.UserControls.Management
{
    partial class UCUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCUsers));
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.DgwUsers = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLastLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Archive = new System.Windows.Forms.DataGridViewImageColumn();
            this.LblEntries = new System.Windows.Forms.Label();
            this.CmbBxPaginationNumbers = new System.Windows.Forms.ComboBox();
            this.LblShow = new System.Windows.Forms.Label();
            this.LblPaginationPrevious = new System.Windows.Forms.Button();
            this.LblPaginationNext = new System.Windows.Forms.Button();
            this.LblPaginationShowEntries = new System.Windows.Forms.Label();
            this.BtnAddUser = new System.Windows.Forms.Button();
            this.CmbBxRoleFilter = new System.Windows.Forms.ComboBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.BtnApply = new System.Windows.Forms.Button();
            this.CmbBxStatusFilter = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgwUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
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
            this.TxtSearchBar.Size = new System.Drawing.Size(897, 40);
            this.TxtSearchBar.TabIndex = 4;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            this.TxtSearchBar.TextChanged += new System.EventHandler(this.TxtSearchBar_TextChanged_1);
            // 
            // DgwUsers
            // 
            this.DgwUsers.AllowUserToAddRows = false;
            this.DgwUsers.BackgroundColor = System.Drawing.Color.White;
            this.DgwUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgwUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column1,
            this.Column4,
            this.Column2,
            this.Column3,
            this.Column6,
            this.ColumnLastLogin,
            this.Column5,
            this.Edit,
            this.Archive});
            this.DgwUsers.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgwUsers.Location = new System.Drawing.Point(36, 200);
            this.DgwUsers.Name = "DgwUsers";
            this.DgwUsers.RowHeadersWidth = 62;
            this.DgwUsers.RowTemplate.Height = 28;
            this.DgwUsers.Size = new System.Drawing.Size(1509, 490);
            this.DgwUsers.TabIndex = 10;
            this.DgwUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgwUsers_CellContentClick);
            // 
            // Column7
            // 
            this.Column7.HeaderText = "User ID";
            this.Column7.MinimumWidth = 8;
            this.Column7.Name = "Column7";
            this.Column7.Width = 150;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Full Name";
            this.Column1.MinimumWidth = 20;
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Role";
            this.Column4.MinimumWidth = 8;
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
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
            // Column6
            // 
            this.Column6.HeaderText = "Contact Number";
            this.Column6.MinimumWidth = 8;
            this.Column6.Name = "Column6";
            this.Column6.Width = 150;
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
            this.Edit.ToolTipText = "Edit user";
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
            this.Archive.ToolTipText = "Archive user";
            this.Archive.Width = 8;
            // 
            // LblEntries
            // 
            this.LblEntries.AutoSize = true;
            this.LblEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEntries.Location = new System.Drawing.Point(174, 132);
            this.LblEntries.Name = "LblEntries";
            this.LblEntries.Size = new System.Drawing.Size(70, 28);
            this.LblEntries.TabIndex = 32;
            this.LblEntries.Text = "entries";
            this.LblEntries.Click += new System.EventHandler(this.LblEntries_Click);
            // 
            // CmbBxPaginationNumbers
            // 
            this.CmbBxPaginationNumbers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CmbBxPaginationNumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CmbBxPaginationNumbers.FormattingEnabled = true;
            this.CmbBxPaginationNumbers.Items.AddRange(new object[] {
            "10",
            "20",
            "30"});
            this.CmbBxPaginationNumbers.Location = new System.Drawing.Point(93, 132);
            this.CmbBxPaginationNumbers.Name = "CmbBxPaginationNumbers";
            this.CmbBxPaginationNumbers.Size = new System.Drawing.Size(75, 33);
            this.CmbBxPaginationNumbers.TabIndex = 31;
            this.CmbBxPaginationNumbers.Text = "10";
            this.CmbBxPaginationNumbers.SelectedIndexChanged += new System.EventHandler(this.CmbBxPaginationNumbers_SelectedIndexChanged_1);
            // 
            // LblShow
            // 
            this.LblShow.AutoSize = true;
            this.LblShow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblShow.Location = new System.Drawing.Point(28, 132);
            this.LblShow.Name = "LblShow";
            this.LblShow.Size = new System.Drawing.Size(60, 28);
            this.LblShow.TabIndex = 30;
            this.LblShow.Text = "Show";
            this.LblShow.Click += new System.EventHandler(this.LblShow_Click);
            // 
            // LblPaginationPrevious
            // 
            this.LblPaginationPrevious.BackColor = System.Drawing.Color.White;
            this.LblPaginationPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblPaginationPrevious.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationPrevious.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationPrevious.Location = new System.Drawing.Point(1273, 730);
            this.LblPaginationPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationPrevious.Name = "LblPaginationPrevious";
            this.LblPaginationPrevious.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationPrevious.TabIndex = 36;
            this.LblPaginationPrevious.Text = "Previous";
            this.LblPaginationPrevious.UseVisualStyleBackColor = false;
            this.LblPaginationPrevious.Click += new System.EventHandler(this.LblPaginationPrevious_Click_1);
            // 
            // LblPaginationNext
            // 
            this.LblPaginationNext.BackColor = System.Drawing.Color.White;
            this.LblPaginationNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblPaginationNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblPaginationNext.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblPaginationNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblPaginationNext.Location = new System.Drawing.Point(1417, 730);
            this.LblPaginationNext.Margin = new System.Windows.Forms.Padding(4);
            this.LblPaginationNext.Name = "LblPaginationNext";
            this.LblPaginationNext.Size = new System.Drawing.Size(125, 46);
            this.LblPaginationNext.TabIndex = 35;
            this.LblPaginationNext.Text = "Next";
            this.LblPaginationNext.UseVisualStyleBackColor = false;
            this.LblPaginationNext.Click += new System.EventHandler(this.LblPaginationNext_Click_1);
            // 
            // LblPaginationShowEntries
            // 
            this.LblPaginationShowEntries.AutoSize = true;
            this.LblPaginationShowEntries.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPaginationShowEntries.Location = new System.Drawing.Point(28, 739);
            this.LblPaginationShowEntries.Name = "LblPaginationShowEntries";
            this.LblPaginationShowEntries.Size = new System.Drawing.Size(268, 28);
            this.LblPaginationShowEntries.TabIndex = 34;
            this.LblPaginationShowEntries.Text = "Showing 1 to 5 of 100 entries";
            this.LblPaginationShowEntries.Click += new System.EventHandler(this.LblPaginationShowEntries_Click);
            // 
            // BtnAddUser
            // 
            this.BtnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAddUser.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddUser.ForeColor = System.Drawing.Color.White;
            this.BtnAddUser.Location = new System.Drawing.Point(1406, 123);
            this.BtnAddUser.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddUser.Name = "BtnAddUser";
            this.BtnAddUser.Size = new System.Drawing.Size(135, 46);
            this.BtnAddUser.TabIndex = 37;
            this.BtnAddUser.Text = "+ Add User";
            this.BtnAddUser.UseVisualStyleBackColor = false;
            this.BtnAddUser.Click += new System.EventHandler(this.BtnAddUser_Click);
            // 
            // CmbBxRoleFilter
            // 
            this.CmbBxRoleFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxRoleFilter.FormattingEnabled = true;
            this.CmbBxRoleFilter.Location = new System.Drawing.Point(1232, 33);
            this.CmbBxRoleFilter.Name = "CmbBxRoleFilter";
            this.CmbBxRoleFilter.Size = new System.Drawing.Size(192, 36);
            this.CmbBxRoleFilter.TabIndex = 38;
            this.CmbBxRoleFilter.Text = "Librarian / Admin";
            this.CmbBxRoleFilter.SelectedIndexChanged += new System.EventHandler(this.CmbBxRoleFilter_SelectedIndexChanged);
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 36);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 39;
            this.LblSearch.Text = "Search";
            this.LblSearch.Click += new System.EventHandler(this.LblSearch_Click);
            // 
            // BtnApply
            // 
            this.BtnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApply.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnApply.ForeColor = System.Drawing.Color.White;
            this.BtnApply.Location = new System.Drawing.Point(1441, 31);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(104, 40);
            this.BtnApply.TabIndex = 40;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = false;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click_1);
            // 
            // CmbBxStatusFilter
            // 
            this.CmbBxStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxStatusFilter.FormattingEnabled = true;
            this.CmbBxStatusFilter.Items.AddRange(new object[] {
            "5",
            "10",
            "15"});
            this.CmbBxStatusFilter.Location = new System.Drawing.Point(1076, 33);
            this.CmbBxStatusFilter.Name = "CmbBxStatusFilter";
            this.CmbBxStatusFilter.Size = new System.Drawing.Size(139, 36);
            this.CmbBxStatusFilter.TabIndex = 41;
            this.CmbBxStatusFilter.Text = "Status";
            this.CmbBxStatusFilter.SelectedIndexChanged += new System.EventHandler(this.CmbBxStatusFilter_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(36, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 56;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // UCUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.CmbBxStatusFilter);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.CmbBxRoleFilter);
            this.Controls.Add(this.BtnAddUser);
            this.Controls.Add(this.LblPaginationPrevious);
            this.Controls.Add(this.LblPaginationNext);
            this.Controls.Add(this.LblPaginationShowEntries);
            this.Controls.Add(this.LblEntries);
            this.Controls.Add(this.CmbBxPaginationNumbers);
            this.Controls.Add(this.LblShow);
            this.Controls.Add(this.DgwUsers);
            this.Controls.Add(this.TxtSearchBar);
            this.Name = "UCUsers";
            this.Size = new System.Drawing.Size(1580, 936);
            ((System.ComponentModel.ISupportInitialize)(this.DgwUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.DataGridView DgwUsers;
        private System.Windows.Forms.Label LblEntries;
        private System.Windows.Forms.ComboBox CmbBxPaginationNumbers;
        private System.Windows.Forms.Label LblShow;
        private System.Windows.Forms.Button LblPaginationPrevious;
        private System.Windows.Forms.Button LblPaginationNext;
        private System.Windows.Forms.Label LblPaginationShowEntries;
        private System.Windows.Forms.Button BtnAddUser;
        private System.Windows.Forms.ComboBox CmbBxRoleFilter;
        private System.Windows.Forms.Label LblSearch;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.ComboBox CmbBxStatusFilter;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLastLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Archive;
    }
}
