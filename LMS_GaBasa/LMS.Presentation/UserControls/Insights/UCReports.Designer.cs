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
            this.BtnAuditLogs = new System.Windows.Forms.Button();
            this.BtnFines = new System.Windows.Forms.Button();
            this.BtnTransactions = new System.Windows.Forms.Button();
            this.BtnInventory = new System.Windows.Forms.Button();
            this.GrpBxDate = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DgvTransactions = new System.Windows.Forms.DataGridView();
            this.colNumberingTransactions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransactionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateBorrowed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateReturned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvInventory = new System.Windows.Forms.DataGridView();
            this.colNumberingInventory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBookID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvFines = new System.Windows.Forms.DataGridView();
            this.colNumberingFines = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateSettled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvAuditLogs = new System.Windows.Forms.DataGridView();
            this.colNumberingAuditLogs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblReport = new System.Windows.Forms.Label();
            this.BtnApplyFromTo = new System.Windows.Forms.Button();
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
            this.LblFrom.Location = new System.Drawing.Point(18, 43);
            this.LblFrom.Name = "LblFrom";
            this.LblFrom.Size = new System.Drawing.Size(74, 32);
            this.LblFrom.TabIndex = 4;
            this.LblFrom.Text = "From:";
            // 
            // DTPckFrom
            // 
            this.DTPckFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DTPckFrom.Location = new System.Drawing.Point(103, 46);
            this.DTPckFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DTPckFrom.Name = "DTPckFrom";
            this.DTPckFrom.Size = new System.Drawing.Size(257, 30);
            this.DTPckFrom.TabIndex = 5;
            // 
            // LblTo
            // 
            this.LblTo.AutoSize = true;
            this.LblTo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.LblTo.Location = new System.Drawing.Point(372, 43);
            this.LblTo.Name = "LblTo";
            this.LblTo.Size = new System.Drawing.Size(44, 32);
            this.LblTo.TabIndex = 7;
            this.LblTo.Text = "To:";
            // 
            // DTPckTo
            // 
            this.DTPckTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DTPckTo.Location = new System.Drawing.Point(425, 46);
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
            this.BtnAuditLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAuditLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAuditLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAuditLogs.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAuditLogs.ForeColor = System.Drawing.Color.White;
            this.BtnAuditLogs.Location = new System.Drawing.Point(504, 40);
            this.BtnAuditLogs.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAuditLogs.Name = "BtnAuditLogs";
            this.BtnAuditLogs.Size = new System.Drawing.Size(144, 52);
            this.BtnAuditLogs.TabIndex = 92;
            this.BtnAuditLogs.Text = "Audit Logs";
            this.BtnAuditLogs.UseVisualStyleBackColor = false;
            // 
            // BtnFines
            // 
            this.BtnFines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFines.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnFines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFines.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnFines.ForeColor = System.Drawing.Color.White;
            this.BtnFines.Location = new System.Drawing.Point(345, 40);
            this.BtnFines.Margin = new System.Windows.Forms.Padding(4);
            this.BtnFines.Name = "BtnFines";
            this.BtnFines.Size = new System.Drawing.Size(144, 52);
            this.BtnFines.TabIndex = 92;
            this.BtnFines.Text = "Fines";
            this.BtnFines.UseVisualStyleBackColor = false;
            // 
            // BtnTransactions
            // 
            this.BtnTransactions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTransactions.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnTransactions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTransactions.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnTransactions.ForeColor = System.Drawing.Color.White;
            this.BtnTransactions.Location = new System.Drawing.Point(185, 40);
            this.BtnTransactions.Margin = new System.Windows.Forms.Padding(4);
            this.BtnTransactions.Name = "BtnTransactions";
            this.BtnTransactions.Size = new System.Drawing.Size(144, 52);
            this.BtnTransactions.TabIndex = 92;
            this.BtnTransactions.Text = "Transactions";
            this.BtnTransactions.UseVisualStyleBackColor = false;
            // 
            // BtnInventory
            // 
            this.BtnInventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnInventory.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInventory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnInventory.ForeColor = System.Drawing.Color.White;
            this.BtnInventory.Location = new System.Drawing.Point(22, 40);
            this.BtnInventory.Margin = new System.Windows.Forms.Padding(4);
            this.BtnInventory.Name = "BtnInventory";
            this.BtnInventory.Size = new System.Drawing.Size(144, 52);
            this.BtnInventory.TabIndex = 92;
            this.BtnInventory.Text = "Inventory";
            this.BtnInventory.UseVisualStyleBackColor = false;
            // 
            // GrpBxDate
            // 
            this.GrpBxDate.Controls.Add(this.BtnApplyFromTo);
            this.GrpBxDate.Controls.Add(this.DTPckFrom);
            this.GrpBxDate.Controls.Add(this.LblFrom);
            this.GrpBxDate.Controls.Add(this.LblTo);
            this.GrpBxDate.Controls.Add(this.DTPckTo);
            this.GrpBxDate.Location = new System.Drawing.Point(703, 29);
            this.GrpBxDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpBxDate.Name = "GrpBxDate";
            this.GrpBxDate.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpBxDate.Size = new System.Drawing.Size(842, 115);
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
            this.DgvTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
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
            this.colTransactionID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTransactionID.FillWeight = 426.1364F;
            this.colTransactionID.HeaderText = "Transaction ID";
            this.colTransactionID.MinimumWidth = 8;
            this.colTransactionID.Name = "colTransactionID";
            this.colTransactionID.Width = 137;
            // 
            // colName
            // 
            this.colName.FillWeight = 18.46591F;
            this.colName.HeaderText = "Member Name";
            this.colName.MinimumWidth = 8;
            this.colName.Name = "colName";
            this.colName.Width = 137;
            // 
            // colTitle
            // 
            this.colTitle.FillWeight = 18.46591F;
            this.colTitle.HeaderText = "Book Title";
            this.colTitle.MinimumWidth = 6;
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Width = 107;
            // 
            // colDateBorrowed
            // 
            this.colDateBorrowed.FillWeight = 18.46591F;
            this.colDateBorrowed.HeaderText = "Date Borrowed";
            this.colDateBorrowed.MinimumWidth = 6;
            this.colDateBorrowed.Name = "colDateBorrowed";
            this.colDateBorrowed.ReadOnly = true;
            this.colDateBorrowed.Width = 140;
            // 
            // colDueDate
            // 
            this.colDueDate.HeaderText = "Due Date";
            this.colDueDate.MinimumWidth = 8;
            this.colDueDate.Name = "colDueDate";
            this.colDueDate.Width = 106;
            // 
            // colDateReturned
            // 
            this.colDateReturned.HeaderText = "Date Returned";
            this.colDateReturned.MinimumWidth = 8;
            this.colDateReturned.Name = "colDateReturned";
            this.colDateReturned.Width = 139;
            // 
            // colStatus
            // 
            this.colStatus.FillWeight = 18.46591F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 92;
            // 
            // DgvInventory
            // 
            this.DgvInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
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
            this.DgvInventory.Location = new System.Drawing.Point(36, 234);
            this.DgvInventory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvInventory.Name = "DgvInventory";
            this.DgvInventory.RowHeadersWidth = 62;
            this.DgvInventory.RowTemplate.Height = 28;
            this.DgvInventory.Size = new System.Drawing.Size(1509, 449);
            this.DgvInventory.TabIndex = 88;
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
            this.colBookID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBookID.FillWeight = 426.1364F;
            this.colBookID.HeaderText = "Book ID";
            this.colBookID.MinimumWidth = 8;
            this.colBookID.Name = "colBookID";
            this.colBookID.Width = 96;
            // 
            // colAccNo
            // 
            this.colAccNo.FillWeight = 18.46591F;
            this.colAccNo.HeaderText = "Accession No.";
            this.colAccNo.MinimumWidth = 8;
            this.colAccNo.Name = "colAccNo";
            this.colAccNo.Width = 135;
            // 
            // colTitle2
            // 
            this.colTitle2.FillWeight = 18.46591F;
            this.colTitle2.HeaderText = "Book Title";
            this.colTitle2.MinimumWidth = 6;
            this.colTitle2.Name = "colTitle2";
            this.colTitle2.ReadOnly = true;
            this.colTitle2.Width = 107;
            // 
            // colCategory
            // 
            this.colCategory.FillWeight = 18.46591F;
            this.colCategory.HeaderText = "Category";
            this.colCategory.MinimumWidth = 6;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            this.colCategory.Width = 109;
            // 
            // colStatus2
            // 
            this.colStatus2.HeaderText = "Status";
            this.colStatus2.MinimumWidth = 8;
            this.colStatus2.Name = "colStatus2";
            this.colStatus2.Width = 92;
            // 
            // colDateAdded
            // 
            this.colDateAdded.HeaderText = "Date Added";
            this.colDateAdded.MinimumWidth = 8;
            this.colDateAdded.Name = "colDateAdded";
            this.colDateAdded.Width = 121;
            // 
            // colAddedBy
            // 
            this.colAddedBy.FillWeight = 18.46591F;
            this.colAddedBy.HeaderText = "Added By";
            this.colAddedBy.MinimumWidth = 6;
            this.colAddedBy.Name = "colAddedBy";
            this.colAddedBy.ReadOnly = true;
            this.colAddedBy.Width = 106;
            // 
            // DgvFines
            // 
            this.DgvFines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
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
            this.DgvFines.Location = new System.Drawing.Point(36, 234);
            this.DgvFines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvFines.Name = "DgvFines";
            this.DgvFines.RowHeadersWidth = 62;
            this.DgvFines.RowTemplate.Height = 28;
            this.DgvFines.Size = new System.Drawing.Size(1509, 449);
            this.DgvFines.TabIndex = 89;
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
            this.colFineID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colFineID.FillWeight = 426.1364F;
            this.colFineID.HeaderText = "Fine ID";
            this.colFineID.MinimumWidth = 8;
            this.colFineID.Name = "colFineID";
            this.colFineID.Width = 90;
            // 
            // colName2
            // 
            this.colName2.FillWeight = 18.46591F;
            this.colName2.HeaderText = "Member Name";
            this.colName2.MinimumWidth = 8;
            this.colName2.Name = "colName2";
            this.colName2.Width = 137;
            // 
            // colFineType
            // 
            this.colFineType.FillWeight = 18.46591F;
            this.colFineType.HeaderText = "Fine Type";
            this.colFineType.MinimumWidth = 6;
            this.colFineType.Name = "colFineType";
            this.colFineType.ReadOnly = true;
            this.colFineType.Width = 106;
            // 
            // colAmount
            // 
            this.colAmount.FillWeight = 18.46591F;
            this.colAmount.HeaderText = "Amount";
            this.colAmount.MinimumWidth = 6;
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            this.colAmount.Width = 101;
            // 
            // colPaymentStatus
            // 
            this.colPaymentStatus.HeaderText = "Status";
            this.colPaymentStatus.MinimumWidth = 8;
            this.colPaymentStatus.Name = "colPaymentStatus";
            this.colPaymentStatus.Width = 92;
            // 
            // colDateSettled
            // 
            this.colDateSettled.HeaderText = "Date Settled";
            this.colDateSettled.MinimumWidth = 8;
            this.colDateSettled.Name = "colDateSettled";
            this.colDateSettled.Width = 125;
            // 
            // DgvAuditLogs
            // 
            this.DgvAuditLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvAuditLogs.BackgroundColor = System.Drawing.Color.White;
            this.DgvAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumberingAuditLogs,
            this.colLogID,
            this.colUser,
            this.colAction,
            this.colTimestamp,
            this.colModule});
            this.DgvAuditLogs.Location = new System.Drawing.Point(36, 234);
            this.DgvAuditLogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvAuditLogs.Name = "DgvAuditLogs";
            this.DgvAuditLogs.RowHeadersWidth = 62;
            this.DgvAuditLogs.RowTemplate.Height = 28;
            this.DgvAuditLogs.Size = new System.Drawing.Size(1509, 449);
            this.DgvAuditLogs.TabIndex = 90;
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
            this.colLogID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLogID.FillWeight = 426.1364F;
            this.colLogID.HeaderText = "Log ID";
            this.colLogID.MinimumWidth = 8;
            this.colLogID.Name = "colLogID";
            this.colLogID.Width = 72;
            // 
            // colUser
            // 
            this.colUser.FillWeight = 18.46591F;
            this.colUser.HeaderText = "User";
            this.colUser.MinimumWidth = 8;
            this.colUser.Name = "colUser";
            this.colUser.Width = 79;
            // 
            // colAction
            // 
            this.colAction.FillWeight = 18.46591F;
            this.colAction.HeaderText = "Action Performed";
            this.colAction.MinimumWidth = 6;
            this.colAction.Name = "colAction";
            this.colAction.ReadOnly = true;
            this.colAction.Width = 154;
            // 
            // colTimestamp
            // 
            this.colTimestamp.FillWeight = 18.46591F;
            this.colTimestamp.HeaderText = "Timestamp";
            this.colTimestamp.MinimumWidth = 6;
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            this.colTimestamp.Width = 123;
            // 
            // colModule
            // 
            this.colModule.HeaderText = "Module Involved";
            this.colModule.MinimumWidth = 8;
            this.colModule.Name = "colModule";
            this.colModule.Width = 146;
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
            // BtnApplyFromTo
            // 
            this.BtnApplyFromTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApplyFromTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnApplyFromTo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnApplyFromTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnApplyFromTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnApplyFromTo.ForeColor = System.Drawing.Color.White;
            this.BtnApplyFromTo.Location = new System.Drawing.Point(700, 36);
            this.BtnApplyFromTo.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApplyFromTo.Name = "BtnApplyFromTo";
            this.BtnApplyFromTo.Size = new System.Drawing.Size(107, 52);
            this.BtnApplyFromTo.TabIndex = 92;
            this.BtnApplyFromTo.Text = "Apply";
            this.BtnApplyFromTo.UseVisualStyleBackColor = false;
            // 
            // UCReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
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
        private System.Windows.Forms.GroupBox GrpBxDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView DgvTransactions;
        private System.Windows.Forms.DataGridView DgvInventory;
        private System.Windows.Forms.DataGridView DgvFines;
        private System.Windows.Forms.DataGridView DgvAuditLogs;
        private System.Windows.Forms.Label LblReport;
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
        private System.Windows.Forms.Button BtnInventory;
        private System.Windows.Forms.Button BtnAuditLogs;
        private System.Windows.Forms.Button BtnFines;
        private System.Windows.Forms.Button BtnTransactions;
        private System.Windows.Forms.Button BtnApplyFromTo;
    }
}
