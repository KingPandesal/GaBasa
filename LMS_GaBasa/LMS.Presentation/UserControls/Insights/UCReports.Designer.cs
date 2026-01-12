namespace LMS.Presentation.UserControls.Insights
{
    partial class UCReports
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
            this.LblFrom = new System.Windows.Forms.Label();
            this.DTPckFrom = new System.Windows.Forms.DateTimePicker();
            this.LblTo = new System.Windows.Forms.Label();
            this.DTPckTo = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GrpBxBtns = new System.Windows.Forms.GroupBox();
            this.BtnAuditLogs = new ReaLTaiizor.Controls.Button();
            this.BtnFines = new ReaLTaiizor.Controls.Button();
            this.BtnTransactions = new ReaLTaiizor.Controls.Button();
            this.BtnInventory = new ReaLTaiizor.Controls.Button();
            this.GrpBxDate = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DgvTransactions = new System.Windows.Forms.DataGridView();
            this.DgvInventory = new System.Windows.Forms.DataGridView();
            this.DgvFines = new System.Windows.Forms.DataGridView();
            this.DgvAuditLogs = new System.Windows.Forms.DataGridView();
            this.LblReport = new System.Windows.Forms.Label();
            this.BtnExport = new ReaLTaiizor.Controls.Button();
            this.colNumberingTransactions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransactionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateBorrowed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateReturned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumberingInventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBookID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumberingFines = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateSettled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumberingAuditLogs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrpBxBtns.SuspendLayout();
            this.GrpBxDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvAuditLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // LblFrom
            // 
            this.LblFrom.AutoSize = true;
            this.LblFrom.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblFrom.Location = new System.Drawing.Point(43, 39);
            this.LblFrom.Name = "LblFrom";
            this.LblFrom.Size = new System.Drawing.Size(74, 32);
            this.LblFrom.TabIndex = 4;
            this.LblFrom.Text = "From:";
            // 
            // DTPckFrom
            // 
            this.DTPckFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DTPckFrom.Location = new System.Drawing.Point(128, 42);
            this.DTPckFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DTPckFrom.Name = "DTPckFrom";
            this.DTPckFrom.Size = new System.Drawing.Size(257, 30);
            this.DTPckFrom.TabIndex = 5;
            // 
            // LblTo
            // 
            this.LblTo.AutoSize = true;
            this.LblTo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblTo.Location = new System.Drawing.Point(442, 39);
            this.LblTo.Name = "LblTo";
            this.LblTo.Size = new System.Drawing.Size(44, 32);
            this.LblTo.TabIndex = 7;
            this.LblTo.Text = "To:";
            // 
            // DTPckTo
            // 
            this.DTPckTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DTPckTo.Location = new System.Drawing.Point(495, 42);
            this.DTPckTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DTPckTo.Name = "DTPckTo";
            this.DTPckTo.Size = new System.Drawing.Size(257, 30);
            this.DTPckTo.TabIndex = 8;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Staff",
            "Guest",
            "Faculty",
            "Student"});
            this.comboBox1.Location = new System.Drawing.Point(1457, 289);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(64, 33);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.Text = "All";
            // 
            // GrpBxBtns
            // 
            this.GrpBxBtns.Controls.Add(this.BtnAuditLogs);
            this.GrpBxBtns.Controls.Add(this.BtnFines);
            this.GrpBxBtns.Controls.Add(this.BtnTransactions);
            this.GrpBxBtns.Controls.Add(this.BtnInventory);
            this.GrpBxBtns.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBxBtns.Location = new System.Drawing.Point(36, 25);
            this.GrpBxBtns.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpBxBtns.Name = "GrpBxBtns";
            this.GrpBxBtns.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpBxBtns.Size = new System.Drawing.Size(668, 120);
            this.GrpBxBtns.TabIndex = 40;
            this.GrpBxBtns.TabStop = false;
            // 
            // BtnAuditLogs
            // 
            this.BtnAuditLogs.BackColor = System.Drawing.Color.White;
            this.BtnAuditLogs.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAuditLogs.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAuditLogs.Image = null;
            this.BtnAuditLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAuditLogs.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.Location = new System.Drawing.Point(504, 40);
            this.BtnAuditLogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnAuditLogs.Name = "BtnAuditLogs";
            this.BtnAuditLogs.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.Size = new System.Drawing.Size(144, 52);
            this.BtnAuditLogs.TabIndex = 33;
            this.BtnAuditLogs.Text = "Audit Logs";
            this.BtnAuditLogs.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // BtnFines
            // 
            this.BtnFines.BackColor = System.Drawing.Color.White;
            this.BtnFines.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFines.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnFines.Image = null;
            this.BtnFines.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnFines.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.Location = new System.Drawing.Point(343, 40);
            this.BtnFines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnFines.Name = "BtnFines";
            this.BtnFines.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.Size = new System.Drawing.Size(144, 52);
            this.BtnFines.TabIndex = 32;
            this.BtnFines.Text = "Fines";
            this.BtnFines.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // BtnTransactions
            // 
            this.BtnTransactions.BackColor = System.Drawing.Color.White;
            this.BtnTransactions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTransactions.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTransactions.Image = null;
            this.BtnTransactions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnTransactions.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.Location = new System.Drawing.Point(182, 40);
            this.BtnTransactions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnTransactions.Name = "BtnTransactions";
            this.BtnTransactions.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.Size = new System.Drawing.Size(144, 52);
            this.BtnTransactions.TabIndex = 31;
            this.BtnTransactions.Text = "Transactions";
            this.BtnTransactions.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // BtnInventory
            // 
            this.BtnInventory.BackColor = System.Drawing.Color.White;
            this.BtnInventory.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnInventory.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnInventory.Image = null;
            this.BtnInventory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnInventory.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.Location = new System.Drawing.Point(21, 40);
            this.BtnInventory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnInventory.Name = "BtnInventory";
            this.BtnInventory.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.Size = new System.Drawing.Size(144, 52);
            this.BtnInventory.TabIndex = 30;
            this.BtnInventory.Text = "Inventory";
            this.BtnInventory.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // GrpBxDate
            // 
            this.GrpBxDate.Controls.Add(this.DTPckFrom);
            this.GrpBxDate.Controls.Add(this.LblFrom);
            this.GrpBxDate.Controls.Add(this.LblTo);
            this.GrpBxDate.Controls.Add(this.DTPckTo);
            this.GrpBxDate.Location = new System.Drawing.Point(703, 29);
            this.GrpBxDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpBxDate.Name = "GrpBxDate";
            this.GrpBxDate.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpBxDate.Size = new System.Drawing.Size(809, 115);
            this.GrpBxDate.TabIndex = 41;
            this.GrpBxDate.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label5.Location = new System.Drawing.Point(1271, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 36);
            this.label5.TabIndex = 9;
            this.label5.Text = "Member Type:";
            // 
            // DgvTransactions
            // 
            this.DgvTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvTransactions.BackgroundColor = System.Drawing.Color.White;
            this.DgvTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumberingTransactions,
            this.colTransactionID,
            this.colName,
            this.colTitle,
            this.colDateBorrowed,
            this.colDueDate,
            this.colDateReturned,
            this.colStatus});
            this.DgvTransactions.Location = new System.Drawing.Point(36, 234);
            this.DgvTransactions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvTransactions.Name = "DgvTransactions";
            this.DgvTransactions.RowHeadersWidth = 62;
            this.DgvTransactions.RowTemplate.Height = 28;
            this.DgvTransactions.Size = new System.Drawing.Size(1509, 449);
            this.DgvTransactions.TabIndex = 87;
            // 
            // DgvInventory
            // 
            this.DgvInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvInventory.BackgroundColor = System.Drawing.Color.White;
            this.DgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumberingInventory,
            this.colBookID,
            this.colAccNo,
            this.colTitle2,
            this.colCategory,
            this.colStatus2,
            this.colDateAdded,
            this.colAddedBy});
            this.DgvInventory.Location = new System.Drawing.Point(36, 285);
            this.DgvInventory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvInventory.Name = "DgvInventory";
            this.DgvInventory.RowHeadersWidth = 62;
            this.DgvInventory.RowTemplate.Height = 28;
            this.DgvInventory.Size = new System.Drawing.Size(1509, 449);
            this.DgvInventory.TabIndex = 88;
            // 
            // DgvFines
            // 
            this.DgvFines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvFines.BackgroundColor = System.Drawing.Color.White;
            this.DgvFines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvFines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumberingFines,
            this.colFineID,
            this.colName2,
            this.colFineType,
            this.colAmount,
            this.colPaymentStatus,
            this.colDateSettled});
            this.DgvFines.Location = new System.Drawing.Point(36, 349);
            this.DgvFines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvFines.Name = "DgvFines";
            this.DgvFines.RowHeadersWidth = 62;
            this.DgvFines.RowTemplate.Height = 28;
            this.DgvFines.Size = new System.Drawing.Size(1509, 449);
            this.DgvFines.TabIndex = 89;
            // 
            // DgvAuditLogs
            // 
            this.DgvAuditLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvAuditLogs.BackgroundColor = System.Drawing.Color.White;
            this.DgvAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumberingAuditLogs,
            this.colLogID,
            this.colUser,
            this.colAction,
            this.colTimestamp,
            this.colModule});
            this.DgvAuditLogs.Location = new System.Drawing.Point(36, 412);
            this.DgvAuditLogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvAuditLogs.Name = "DgvAuditLogs";
            this.DgvAuditLogs.RowHeadersWidth = 62;
            this.DgvAuditLogs.RowTemplate.Height = 28;
            this.DgvAuditLogs.Size = new System.Drawing.Size(1509, 449);
            this.DgvAuditLogs.TabIndex = 90;
            // 
            // LblReport
            // 
            this.LblReport.AutoSize = true;
            this.LblReport.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblReport.Location = new System.Drawing.Point(30, 175);
            this.LblReport.Name = "LblReport";
            this.LblReport.Size = new System.Drawing.Size(217, 36);
            this.LblReport.TabIndex = 91;
            this.LblReport.Text = "Inventory Report";
            // 
            // BtnExport
            // 
            this.BtnExport.BackColor = System.Drawing.Color.White;
            this.BtnExport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnExport.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnExport.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExport.Image = null;
            this.BtnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnExport.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnExport.Location = new System.Drawing.Point(1401, 168);
            this.BtnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnExport.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnExport.Size = new System.Drawing.Size(144, 52);
            this.BtnExport.TabIndex = 34;
            this.BtnExport.Text = "Export";
            this.BtnExport.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // colNumberingTransactions
            // 
            this.colNumberingTransactions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colNumberingTransactions.HeaderText = "#";
            this.colNumberingTransactions.MinimumWidth = 8;
            this.colNumberingTransactions.Name = "colNumberingTransactions";
            this.colNumberingTransactions.Width = 54;
            // 
            // colTransactionID
            // 
            this.colTransactionID.FillWeight = 426.1364F;
            this.colTransactionID.HeaderText = "Transaction ID";
            this.colTransactionID.MinimumWidth = 8;
            this.colTransactionID.Name = "colTransactionID";
            // 
            // colName
            // 
            this.colName.FillWeight = 18.46591F;
            this.colName.HeaderText = "Member Name";
            this.colName.MinimumWidth = 8;
            this.colName.Name = "colName";
            // 
            // colTitle
            // 
            this.colTitle.FillWeight = 18.46591F;
            this.colTitle.HeaderText = "Book Title";
            this.colTitle.MinimumWidth = 6;
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colDateBorrowed
            // 
            this.colDateBorrowed.FillWeight = 18.46591F;
            this.colDateBorrowed.HeaderText = "Date Borrowed";
            this.colDateBorrowed.MinimumWidth = 6;
            this.colDateBorrowed.Name = "colDateBorrowed";
            this.colDateBorrowed.ReadOnly = true;
            // 
            // colDueDate
            // 
            this.colDueDate.HeaderText = "Due Date";
            this.colDueDate.MinimumWidth = 8;
            this.colDueDate.Name = "colDueDate";
            // 
            // colDateReturned
            // 
            this.colDateReturned.HeaderText = "Date Returned";
            this.colDateReturned.MinimumWidth = 8;
            this.colDateReturned.Name = "colDateReturned";
            // 
            // colStatus
            // 
            this.colStatus.FillWeight = 18.46591F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colNumberingInventory
            // 
            this.colNumberingInventory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colNumberingInventory.HeaderText = "#";
            this.colNumberingInventory.MinimumWidth = 8;
            this.colNumberingInventory.Name = "colNumberingInventory";
            this.colNumberingInventory.Width = 54;
            // 
            // colBookID
            // 
            this.colBookID.FillWeight = 426.1364F;
            this.colBookID.HeaderText = "Book ID";
            this.colBookID.MinimumWidth = 8;
            this.colBookID.Name = "colBookID";
            // 
            // colAccNo
            // 
            this.colAccNo.FillWeight = 18.46591F;
            this.colAccNo.HeaderText = "Accession No.";
            this.colAccNo.MinimumWidth = 8;
            this.colAccNo.Name = "colAccNo";
            // 
            // colTitle2
            // 
            this.colTitle2.FillWeight = 18.46591F;
            this.colTitle2.HeaderText = "Book Title";
            this.colTitle2.MinimumWidth = 6;
            this.colTitle2.Name = "colTitle2";
            this.colTitle2.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.FillWeight = 18.46591F;
            this.colCategory.HeaderText = "Category";
            this.colCategory.MinimumWidth = 6;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colStatus2
            // 
            this.colStatus2.HeaderText = "Status";
            this.colStatus2.MinimumWidth = 8;
            this.colStatus2.Name = "colStatus2";
            // 
            // colDateAdded
            // 
            this.colDateAdded.HeaderText = "Date Added";
            this.colDateAdded.MinimumWidth = 8;
            this.colDateAdded.Name = "colDateAdded";
            // 
            // colAddedBy
            // 
            this.colAddedBy.FillWeight = 18.46591F;
            this.colAddedBy.HeaderText = "Added By";
            this.colAddedBy.MinimumWidth = 6;
            this.colAddedBy.Name = "colAddedBy";
            this.colAddedBy.ReadOnly = true;
            // 
            // colNumberingFines
            // 
            this.colNumberingFines.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colNumberingFines.HeaderText = "#";
            this.colNumberingFines.MinimumWidth = 8;
            this.colNumberingFines.Name = "colNumberingFines";
            this.colNumberingFines.Width = 54;
            // 
            // colFineID
            // 
            this.colFineID.FillWeight = 426.1364F;
            this.colFineID.HeaderText = "Fine ID";
            this.colFineID.MinimumWidth = 8;
            this.colFineID.Name = "colFineID";
            // 
            // colName2
            // 
            this.colName2.FillWeight = 18.46591F;
            this.colName2.HeaderText = "Member Name";
            this.colName2.MinimumWidth = 8;
            this.colName2.Name = "colName2";
            // 
            // colFineType
            // 
            this.colFineType.FillWeight = 18.46591F;
            this.colFineType.HeaderText = "Fine Type";
            this.colFineType.MinimumWidth = 6;
            this.colFineType.Name = "colFineType";
            this.colFineType.ReadOnly = true;
            // 
            // colAmount
            // 
            this.colAmount.FillWeight = 18.46591F;
            this.colAmount.HeaderText = "Amount";
            this.colAmount.MinimumWidth = 6;
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            // 
            // colPaymentStatus
            // 
            this.colPaymentStatus.HeaderText = "Status";
            this.colPaymentStatus.MinimumWidth = 8;
            this.colPaymentStatus.Name = "colPaymentStatus";
            // 
            // colDateSettled
            // 
            this.colDateSettled.HeaderText = "Date Settled";
            this.colDateSettled.MinimumWidth = 8;
            this.colDateSettled.Name = "colDateSettled";
            // 
            // colNumberingAuditLogs
            // 
            this.colNumberingAuditLogs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colNumberingAuditLogs.HeaderText = "#";
            this.colNumberingAuditLogs.MinimumWidth = 8;
            this.colNumberingAuditLogs.Name = "colNumberingAuditLogs";
            this.colNumberingAuditLogs.Width = 54;
            // 
            // colLogID
            // 
            this.colLogID.FillWeight = 426.1364F;
            this.colLogID.HeaderText = "Log ID";
            this.colLogID.MinimumWidth = 8;
            this.colLogID.Name = "colLogID";
            // 
            // colUser
            // 
            this.colUser.FillWeight = 18.46591F;
            this.colUser.HeaderText = "User";
            this.colUser.MinimumWidth = 8;
            this.colUser.Name = "colUser";
            // 
            // colAction
            // 
            this.colAction.FillWeight = 18.46591F;
            this.colAction.HeaderText = "Action Performed";
            this.colAction.MinimumWidth = 6;
            this.colAction.Name = "colAction";
            this.colAction.ReadOnly = true;
            // 
            // colTimestamp
            // 
            this.colTimestamp.FillWeight = 18.46591F;
            this.colTimestamp.HeaderText = "Timestamp";
            this.colTimestamp.MinimumWidth = 6;
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            // 
            // colModule
            // 
            this.colModule.HeaderText = "Module Involved";
            this.colModule.MinimumWidth = 8;
            this.colModule.Name = "colModule";
            // 
            // UCReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.BtnExport);
            this.Controls.Add(this.LblReport);
            this.Controls.Add(this.DgvAuditLogs);
            this.Controls.Add(this.DgvFines);
            this.Controls.Add(this.DgvInventory);
            this.Controls.Add(this.DgvTransactions);
            this.Controls.Add(this.GrpBxDate);
            this.Controls.Add(this.GrpBxBtns);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCReports";
            this.Size = new System.Drawing.Size(1580, 936);
            this.GrpBxBtns.ResumeLayout(false);
            this.GrpBxDate.ResumeLayout(false);
            this.GrpBxDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvAuditLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblFrom;
        private System.Windows.Forms.DateTimePicker DTPckFrom;
        private System.Windows.Forms.Label LblTo;
        private System.Windows.Forms.DateTimePicker DTPckTo;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox GrpBxBtns;
        private ReaLTaiizor.Controls.Button BtnInventory;
        private ReaLTaiizor.Controls.Button BtnAuditLogs;
        private ReaLTaiizor.Controls.Button BtnFines;
        private ReaLTaiizor.Controls.Button BtnTransactions;
        private System.Windows.Forms.GroupBox GrpBxDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView DgvTransactions;
        private System.Windows.Forms.DataGridView DgvInventory;
        private System.Windows.Forms.DataGridView DgvFines;
        private System.Windows.Forms.DataGridView DgvAuditLogs;
        private System.Windows.Forms.Label LblReport;
        private ReaLTaiizor.Controls.Button BtnExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberingTransactions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransactionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateBorrowed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateReturned;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberingInventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBookID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberingFines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFineID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateSettled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberingAuditLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModule;
    }
}
