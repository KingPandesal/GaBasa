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
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.BtnSettlePayment = new System.Windows.Forms.Button();
            this.TxtSearchMember = new System.Windows.Forms.TextBox();
            this.LblSearchMember = new System.Windows.Forms.Label();
            this.LblType = new System.Windows.Forms.Label();
            this.LblName = new System.Windows.Forms.Label();
            this.LblStatus = new System.Windows.Forms.Label();
            this.GrpBxOustandingPenalties = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DgvFines = new System.Windows.Forms.DataGridView();
            this.LblSelectedTotal = new System.Windows.Forms.Label();
            this.LblMemberInfo = new System.Windows.Forms.Label();
            this.TabCtrl = new System.Windows.Forms.TabControl();
            this.TabPgAddCharges = new System.Windows.Forms.TabPage();
            this.FlowPnlforAddCharges = new System.Windows.Forms.FlowLayoutPanel();
            this.PnlforChargeType = new System.Windows.Forms.Panel();
            this.CmbBxChargeType = new System.Windows.Forms.ComboBox();
            this.LblChargeType = new System.Windows.Forms.Label();
            this.PnlforAccessionNumber = new System.Windows.Forms.Panel();
            this.TxtAccessionNumber = new System.Windows.Forms.TextBox();
            this.LblAccessionNumber = new System.Windows.Forms.Label();
            this.PnlforAmount = new System.Windows.Forms.Panel();
            this.LblAmount = new System.Windows.Forms.Label();
            this.NumPckAmount = new System.Windows.Forms.NumericUpDown();
            this.BtnAddToList = new System.Windows.Forms.Button();
            this.TabPgWaiver = new System.Windows.Forms.TabPage();
            this.LblReason = new System.Windows.Forms.Label();
            this.TxtReason = new System.Windows.Forms.TextBox();
            this.BtnWaive = new System.Windows.Forms.Button();
            this.TabPgPayment = new System.Windows.Forms.TabPage();
            this.NumPckAmountReceived = new System.Windows.Forms.NumericUpDown();
            this.CmbBxMode = new System.Windows.Forms.ComboBox();
            this.LblValueAmounttoPay = new System.Windows.Forms.Label();
            this.LblValueChange = new System.Windows.Forms.Label();
            this.LblAmounttoPay = new System.Windows.Forms.Label();
            this.LblChange = new System.Windows.Forms.Label();
            this.LblAmountReceived = new System.Windows.Forms.Label();
            this.LblMode = new System.Windows.Forms.Label();
            this.LblTotalFines = new System.Windows.Forms.Label();
            this.LblBooksCurrentlyBorrowed = new System.Windows.Forms.Label();
            this.LblOverdueCount = new System.Windows.Forms.Label();
            this.BtnSearchMember = new System.Windows.Forms.Button();
            this.ColumnNumbering = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnTransactionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMemberID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMemberName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFineAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDateIssued = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrpBxOustandingPenalties.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFines)).BeginInit();
            this.TabCtrl.SuspendLayout();
            this.TabPgAddCharges.SuspendLayout();
            this.FlowPnlforAddCharges.SuspendLayout();
            this.PnlforChargeType.SuspendLayout();
            this.PnlforAccessionNumber.SuspendLayout();
            this.PnlforAmount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmount)).BeginInit();
            this.TabPgWaiver.SuspendLayout();
            this.TabPgPayment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmountReceived)).BeginInit();
            this.SuspendLayout();
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
            // BtnSettlePayment
            // 
            this.BtnSettlePayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSettlePayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSettlePayment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSettlePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSettlePayment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSettlePayment.ForeColor = System.Drawing.Color.White;
            this.BtnSettlePayment.Location = new System.Drawing.Point(172, 326);
            this.BtnSettlePayment.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSettlePayment.Name = "BtnSettlePayment";
            this.BtnSettlePayment.Size = new System.Drawing.Size(166, 44);
            this.BtnSettlePayment.TabIndex = 54;
            this.BtnSettlePayment.Text = "Settle Payment";
            this.BtnSettlePayment.UseVisualStyleBackColor = false;
            this.BtnSettlePayment.Click += new System.EventHandler(this.BtnEnterMemberID_Click);
            // 
            // TxtSearchMember
            // 
            this.TxtSearchMember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TxtSearchMember.Location = new System.Drawing.Point(37, 82);
            this.TxtSearchMember.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtSearchMember.Name = "TxtSearchMember";
            this.TxtSearchMember.Size = new System.Drawing.Size(386, 34);
            this.TxtSearchMember.TabIndex = 53;
            // 
            // LblSearchMember
            // 
            this.LblSearchMember.AutoSize = true;
            this.LblSearchMember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblSearchMember.ForeColor = System.Drawing.Color.Black;
            this.LblSearchMember.Location = new System.Drawing.Point(32, 32);
            this.LblSearchMember.Name = "LblSearchMember";
            this.LblSearchMember.Size = new System.Drawing.Size(173, 28);
            this.LblSearchMember.TabIndex = 52;
            this.LblSearchMember.Text = "Search Member ID";
            this.LblSearchMember.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // LblType
            // 
            this.LblType.AutoSize = true;
            this.LblType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblType.ForeColor = System.Drawing.Color.Black;
            this.LblType.Location = new System.Drawing.Point(630, 89);
            this.LblType.Name = "LblType";
            this.LblType.Size = new System.Drawing.Size(57, 28);
            this.LblType.TabIndex = 52;
            this.LblType.Text = "Type:";
            this.LblType.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // LblName
            // 
            this.LblName.AutoSize = true;
            this.LblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblName.ForeColor = System.Drawing.Color.Black;
            this.LblName.Location = new System.Drawing.Point(630, 61);
            this.LblName.Name = "LblName";
            this.LblName.Size = new System.Drawing.Size(68, 28);
            this.LblName.TabIndex = 52;
            this.LblName.Text = "Name:";
            this.LblName.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblStatus.ForeColor = System.Drawing.Color.Black;
            this.LblStatus.Location = new System.Drawing.Point(630, 120);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(69, 28);
            this.LblStatus.TabIndex = 52;
            this.LblStatus.Text = "Status:";
            this.LblStatus.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // GrpBxOustandingPenalties
            // 
            this.GrpBxOustandingPenalties.Controls.Add(this.panel1);
            this.GrpBxOustandingPenalties.Controls.Add(this.LblSelectedTotal);
            this.GrpBxOustandingPenalties.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.GrpBxOustandingPenalties.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.GrpBxOustandingPenalties.Location = new System.Drawing.Point(532, 192);
            this.GrpBxOustandingPenalties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpBxOustandingPenalties.Name = "GrpBxOustandingPenalties";
            this.GrpBxOustandingPenalties.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpBxOustandingPenalties.Size = new System.Drawing.Size(1004, 602);
            this.GrpBxOustandingPenalties.TabIndex = 55;
            this.GrpBxOustandingPenalties.TabStop = false;
            this.GrpBxOustandingPenalties.Text = "Outstanding Penalties";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DgvFines);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.panel1.Location = new System.Drawing.Point(22, 32);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(853, 295);
            this.panel1.TabIndex = 60;
            // 
            // DgvFines
            // 
            this.DgvFines.AllowUserToAddRows = false;
            this.DgvFines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvFines.BackgroundColor = System.Drawing.Color.White;
            this.DgvFines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvFines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNumbering,
            this.ColumnCheckBox,
            this.ColumnTransactionID,
            this.ColumnMemberID,
            this.ColumnMemberName,
            this.ColumnFineAmount,
            this.ColumnFineType,
            this.ColumnDateIssued,
            this.ColumnStatus});
            this.DgvFines.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgvFines.Location = new System.Drawing.Point(4, 19);
            this.DgvFines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DgvFines.Name = "DgvFines";
            this.DgvFines.RowHeadersWidth = 62;
            this.DgvFines.RowTemplate.Height = 28;
            this.DgvFines.Size = new System.Drawing.Size(846, 272);
            this.DgvFines.TabIndex = 59;
            // 
            // LblSelectedTotal
            // 
            this.LblSelectedTotal.AutoSize = true;
            this.LblSelectedTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblSelectedTotal.ForeColor = System.Drawing.Color.Black;
            this.LblSelectedTotal.Location = new System.Drawing.Point(22, 349);
            this.LblSelectedTotal.Name = "LblSelectedTotal";
            this.LblSelectedTotal.Size = new System.Drawing.Size(137, 28);
            this.LblSelectedTotal.TabIndex = 52;
            this.LblSelectedTotal.Text = "Selected Total:";
            this.LblSelectedTotal.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // LblMemberInfo
            // 
            this.LblMemberInfo.AutoSize = true;
            this.LblMemberInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblMemberInfo.ForeColor = System.Drawing.Color.Black;
            this.LblMemberInfo.Location = new System.Drawing.Point(630, 32);
            this.LblMemberInfo.Name = "LblMemberInfo";
            this.LblMemberInfo.Size = new System.Drawing.Size(125, 28);
            this.LblMemberInfo.TabIndex = 52;
            this.LblMemberInfo.Text = "Member Info";
            this.LblMemberInfo.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // TabCtrl
            // 
            this.TabCtrl.Controls.Add(this.TabPgAddCharges);
            this.TabCtrl.Controls.Add(this.TabPgWaiver);
            this.TabCtrl.Controls.Add(this.TabPgPayment);
            this.TabCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TabCtrl.Location = new System.Drawing.Point(37, 171);
            this.TabCtrl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabCtrl.Name = "TabCtrl";
            this.TabCtrl.SelectedIndex = 0;
            this.TabCtrl.Size = new System.Drawing.Size(488, 624);
            this.TabCtrl.TabIndex = 57;
            // 
            // TabPgAddCharges
            // 
            this.TabPgAddCharges.BackColor = System.Drawing.Color.White;
            this.TabPgAddCharges.Controls.Add(this.FlowPnlforAddCharges);
            this.TabPgAddCharges.Controls.Add(this.BtnAddToList);
            this.TabPgAddCharges.Location = new System.Drawing.Point(4, 34);
            this.TabPgAddCharges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPgAddCharges.Name = "TabPgAddCharges";
            this.TabPgAddCharges.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPgAddCharges.Size = new System.Drawing.Size(480, 586);
            this.TabPgAddCharges.TabIndex = 0;
            this.TabPgAddCharges.Text = "Add Charges";
            // 
            // FlowPnlforAddCharges
            // 
            this.FlowPnlforAddCharges.Controls.Add(this.PnlforChargeType);
            this.FlowPnlforAddCharges.Controls.Add(this.PnlforAccessionNumber);
            this.FlowPnlforAddCharges.Controls.Add(this.PnlforAmount);
            this.FlowPnlforAddCharges.Location = new System.Drawing.Point(38, 39);
            this.FlowPnlforAddCharges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FlowPnlforAddCharges.Name = "FlowPnlforAddCharges";
            this.FlowPnlforAddCharges.Size = new System.Drawing.Size(404, 182);
            this.FlowPnlforAddCharges.TabIndex = 63;
            // 
            // PnlforChargeType
            // 
            this.PnlforChargeType.Controls.Add(this.CmbBxChargeType);
            this.PnlforChargeType.Controls.Add(this.LblChargeType);
            this.PnlforChargeType.Location = new System.Drawing.Point(3, 2);
            this.PnlforChargeType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlforChargeType.Name = "PnlforChargeType";
            this.PnlforChargeType.Size = new System.Drawing.Size(471, 44);
            this.PnlforChargeType.TabIndex = 1;
            // 
            // CmbBxChargeType
            // 
            this.CmbBxChargeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBxChargeType.FormattingEnabled = true;
            this.CmbBxChargeType.Items.AddRange(new object[] {
            "ID Card Replacement",
            "Damaged Book Fee"});
            this.CmbBxChargeType.Location = new System.Drawing.Point(189, 0);
            this.CmbBxChargeType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbBxChargeType.Name = "CmbBxChargeType";
            this.CmbBxChargeType.Size = new System.Drawing.Size(210, 33);
            this.CmbBxChargeType.TabIndex = 59;
            // 
            // LblChargeType
            // 
            this.LblChargeType.AutoSize = true;
            this.LblChargeType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblChargeType.ForeColor = System.Drawing.Color.Black;
            this.LblChargeType.Location = new System.Drawing.Point(-6, 0);
            this.LblChargeType.Name = "LblChargeType";
            this.LblChargeType.Size = new System.Drawing.Size(124, 28);
            this.LblChargeType.TabIndex = 58;
            this.LblChargeType.Text = "Charge Type:";
            // 
            // PnlforAccessionNumber
            // 
            this.PnlforAccessionNumber.Controls.Add(this.TxtAccessionNumber);
            this.PnlforAccessionNumber.Controls.Add(this.LblAccessionNumber);
            this.PnlforAccessionNumber.Location = new System.Drawing.Point(3, 50);
            this.PnlforAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlforAccessionNumber.Name = "PnlforAccessionNumber";
            this.PnlforAccessionNumber.Size = new System.Drawing.Size(471, 44);
            this.PnlforAccessionNumber.TabIndex = 0;
            // 
            // TxtAccessionNumber
            // 
            this.TxtAccessionNumber.Location = new System.Drawing.Point(189, 2);
            this.TxtAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtAccessionNumber.Name = "TxtAccessionNumber";
            this.TxtAccessionNumber.Size = new System.Drawing.Size(210, 30);
            this.TxtAccessionNumber.TabIndex = 62;
            // 
            // LblAccessionNumber
            // 
            this.LblAccessionNumber.AutoSize = true;
            this.LblAccessionNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAccessionNumber.ForeColor = System.Drawing.Color.Black;
            this.LblAccessionNumber.Location = new System.Drawing.Point(-6, 2);
            this.LblAccessionNumber.Name = "LblAccessionNumber";
            this.LblAccessionNumber.Size = new System.Drawing.Size(178, 28);
            this.LblAccessionNumber.TabIndex = 56;
            this.LblAccessionNumber.Text = "Accession Number:";
            // 
            // PnlforAmount
            // 
            this.PnlforAmount.Controls.Add(this.LblAmount);
            this.PnlforAmount.Controls.Add(this.NumPckAmount);
            this.PnlforAmount.Location = new System.Drawing.Point(3, 98);
            this.PnlforAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlforAmount.Name = "PnlforAmount";
            this.PnlforAmount.Size = new System.Drawing.Size(471, 44);
            this.PnlforAmount.TabIndex = 2;
            // 
            // LblAmount
            // 
            this.LblAmount.AutoSize = true;
            this.LblAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAmount.ForeColor = System.Drawing.Color.Black;
            this.LblAmount.Location = new System.Drawing.Point(-4, 0);
            this.LblAmount.Name = "LblAmount";
            this.LblAmount.Size = new System.Drawing.Size(87, 28);
            this.LblAmount.TabIndex = 57;
            this.LblAmount.Text = "Amount:";
            // 
            // NumPckAmount
            // 
            this.NumPckAmount.Location = new System.Drawing.Point(304, 1);
            this.NumPckAmount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NumPckAmount.Name = "NumPckAmount";
            this.NumPckAmount.Size = new System.Drawing.Size(96, 30);
            this.NumPckAmount.TabIndex = 60;
            // 
            // BtnAddToList
            // 
            this.BtnAddToList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddToList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAddToList.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddToList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddToList.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddToList.ForeColor = System.Drawing.Color.White;
            this.BtnAddToList.Location = new System.Drawing.Point(172, 326);
            this.BtnAddToList.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddToList.Name = "BtnAddToList";
            this.BtnAddToList.Size = new System.Drawing.Size(166, 44);
            this.BtnAddToList.TabIndex = 61;
            this.BtnAddToList.Text = "Add to List";
            this.BtnAddToList.UseVisualStyleBackColor = false;
            // 
            // TabPgWaiver
            // 
            this.TabPgWaiver.BackColor = System.Drawing.Color.White;
            this.TabPgWaiver.Controls.Add(this.LblReason);
            this.TabPgWaiver.Controls.Add(this.TxtReason);
            this.TabPgWaiver.Controls.Add(this.BtnWaive);
            this.TabPgWaiver.Location = new System.Drawing.Point(4, 34);
            this.TabPgWaiver.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPgWaiver.Name = "TabPgWaiver";
            this.TabPgWaiver.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPgWaiver.Size = new System.Drawing.Size(480, 586);
            this.TabPgWaiver.TabIndex = 1;
            this.TabPgWaiver.Text = "Waiver";
            // 
            // LblReason
            // 
            this.LblReason.AutoSize = true;
            this.LblReason.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblReason.ForeColor = System.Drawing.Color.Black;
            this.LblReason.Location = new System.Drawing.Point(34, 39);
            this.LblReason.Name = "LblReason";
            this.LblReason.Size = new System.Drawing.Size(78, 28);
            this.LblReason.TabIndex = 52;
            this.LblReason.Text = "Reason:";
            this.LblReason.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // TxtReason
            // 
            this.TxtReason.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TxtReason.Location = new System.Drawing.Point(39, 79);
            this.TxtReason.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtReason.Multiline = true;
            this.TxtReason.Name = "TxtReason";
            this.TxtReason.Size = new System.Drawing.Size(401, 82);
            this.TxtReason.TabIndex = 53;
            // 
            // BtnWaive
            // 
            this.BtnWaive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnWaive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnWaive.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnWaive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnWaive.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnWaive.ForeColor = System.Drawing.Color.White;
            this.BtnWaive.Location = new System.Drawing.Point(172, 326);
            this.BtnWaive.Margin = new System.Windows.Forms.Padding(4);
            this.BtnWaive.Name = "BtnWaive";
            this.BtnWaive.Size = new System.Drawing.Size(166, 44);
            this.BtnWaive.TabIndex = 54;
            this.BtnWaive.Text = "Waive";
            this.BtnWaive.UseVisualStyleBackColor = false;
            this.BtnWaive.Click += new System.EventHandler(this.BtnEnterMemberID_Click);
            // 
            // TabPgPayment
            // 
            this.TabPgPayment.BackColor = System.Drawing.Color.White;
            this.TabPgPayment.Controls.Add(this.NumPckAmountReceived);
            this.TabPgPayment.Controls.Add(this.CmbBxMode);
            this.TabPgPayment.Controls.Add(this.LblValueAmounttoPay);
            this.TabPgPayment.Controls.Add(this.LblValueChange);
            this.TabPgPayment.Controls.Add(this.BtnSettlePayment);
            this.TabPgPayment.Controls.Add(this.LblAmounttoPay);
            this.TabPgPayment.Controls.Add(this.LblChange);
            this.TabPgPayment.Controls.Add(this.LblAmountReceived);
            this.TabPgPayment.Controls.Add(this.LblMode);
            this.TabPgPayment.Location = new System.Drawing.Point(4, 34);
            this.TabPgPayment.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPgPayment.Name = "TabPgPayment";
            this.TabPgPayment.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPgPayment.Size = new System.Drawing.Size(480, 586);
            this.TabPgPayment.TabIndex = 2;
            this.TabPgPayment.Text = "Payment";
            // 
            // NumPckAmountReceived
            // 
            this.NumPckAmountReceived.Location = new System.Drawing.Point(241, 151);
            this.NumPckAmountReceived.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NumPckAmountReceived.Name = "NumPckAmountReceived";
            this.NumPckAmountReceived.Size = new System.Drawing.Size(198, 30);
            this.NumPckAmountReceived.TabIndex = 62;
            // 
            // CmbBxMode
            // 
            this.CmbBxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBxMode.FormattingEnabled = true;
            this.CmbBxMode.Items.AddRange(new object[] {
            "Cash",
            "Online"});
            this.CmbBxMode.Location = new System.Drawing.Point(241, 92);
            this.CmbBxMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbBxMode.Name = "CmbBxMode";
            this.CmbBxMode.Size = new System.Drawing.Size(198, 33);
            this.CmbBxMode.TabIndex = 61;
            // 
            // LblValueAmounttoPay
            // 
            this.LblValueAmounttoPay.AutoSize = true;
            this.LblValueAmounttoPay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblValueAmounttoPay.ForeColor = System.Drawing.Color.Black;
            this.LblValueAmounttoPay.Location = new System.Drawing.Point(236, 39);
            this.LblValueAmounttoPay.Name = "LblValueAmounttoPay";
            this.LblValueAmounttoPay.Size = new System.Drawing.Size(34, 28);
            this.LblValueAmounttoPay.TabIndex = 60;
            this.LblValueAmounttoPay.Text = "P0";
            // 
            // LblValueChange
            // 
            this.LblValueChange.AutoSize = true;
            this.LblValueChange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblValueChange.ForeColor = System.Drawing.Color.Black;
            this.LblValueChange.Location = new System.Drawing.Point(236, 212);
            this.LblValueChange.Name = "LblValueChange";
            this.LblValueChange.Size = new System.Drawing.Size(34, 28);
            this.LblValueChange.TabIndex = 60;
            this.LblValueChange.Text = "P0";
            // 
            // LblAmounttoPay
            // 
            this.LblAmounttoPay.AutoSize = true;
            this.LblAmounttoPay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAmounttoPay.ForeColor = System.Drawing.Color.Black;
            this.LblAmounttoPay.Location = new System.Drawing.Point(35, 39);
            this.LblAmounttoPay.Name = "LblAmounttoPay";
            this.LblAmounttoPay.Size = new System.Drawing.Size(146, 28);
            this.LblAmounttoPay.TabIndex = 60;
            this.LblAmounttoPay.Text = "Amount to Pay:";
            // 
            // LblChange
            // 
            this.LblChange.AutoSize = true;
            this.LblChange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblChange.ForeColor = System.Drawing.Color.Black;
            this.LblChange.Location = new System.Drawing.Point(35, 212);
            this.LblChange.Name = "LblChange";
            this.LblChange.Size = new System.Drawing.Size(82, 28);
            this.LblChange.TabIndex = 60;
            this.LblChange.Text = "Change:";
            // 
            // LblAmountReceived
            // 
            this.LblAmountReceived.AutoSize = true;
            this.LblAmountReceived.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAmountReceived.ForeColor = System.Drawing.Color.Black;
            this.LblAmountReceived.Location = new System.Drawing.Point(35, 151);
            this.LblAmountReceived.Name = "LblAmountReceived";
            this.LblAmountReceived.Size = new System.Drawing.Size(169, 28);
            this.LblAmountReceived.TabIndex = 60;
            this.LblAmountReceived.Text = "Amount Received:";
            // 
            // LblMode
            // 
            this.LblMode.AutoSize = true;
            this.LblMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblMode.ForeColor = System.Drawing.Color.Black;
            this.LblMode.Location = new System.Drawing.Point(35, 92);
            this.LblMode.Name = "LblMode";
            this.LblMode.Size = new System.Drawing.Size(68, 28);
            this.LblMode.TabIndex = 60;
            this.LblMode.Text = "Mode:";
            // 
            // LblTotalFines
            // 
            this.LblTotalFines.AutoSize = true;
            this.LblTotalFines.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblTotalFines.ForeColor = System.Drawing.Color.Black;
            this.LblTotalFines.Location = new System.Drawing.Point(1055, 61);
            this.LblTotalFines.Name = "LblTotalFines";
            this.LblTotalFines.Size = new System.Drawing.Size(107, 28);
            this.LblTotalFines.TabIndex = 52;
            this.LblTotalFines.Text = "Total Fines:";
            this.LblTotalFines.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // LblBooksCurrentlyBorrowed
            // 
            this.LblBooksCurrentlyBorrowed.AutoSize = true;
            this.LblBooksCurrentlyBorrowed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblBooksCurrentlyBorrowed.ForeColor = System.Drawing.Color.Black;
            this.LblBooksCurrentlyBorrowed.Location = new System.Drawing.Point(1055, 89);
            this.LblBooksCurrentlyBorrowed.Name = "LblBooksCurrentlyBorrowed";
            this.LblBooksCurrentlyBorrowed.Size = new System.Drawing.Size(244, 28);
            this.LblBooksCurrentlyBorrowed.TabIndex = 52;
            this.LblBooksCurrentlyBorrowed.Text = "Books Currently Borrowed:";
            this.LblBooksCurrentlyBorrowed.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // LblOverdueCount
            // 
            this.LblOverdueCount.AutoSize = true;
            this.LblOverdueCount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblOverdueCount.ForeColor = System.Drawing.Color.Black;
            this.LblOverdueCount.Location = new System.Drawing.Point(1055, 120);
            this.LblOverdueCount.Name = "LblOverdueCount";
            this.LblOverdueCount.Size = new System.Drawing.Size(149, 28);
            this.LblOverdueCount.TabIndex = 52;
            this.LblOverdueCount.Text = "Overdue Count:";
            this.LblOverdueCount.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // BtnSearchMember
            // 
            this.BtnSearchMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSearchMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSearchMember.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSearchMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearchMember.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearchMember.ForeColor = System.Drawing.Color.White;
            this.BtnSearchMember.Location = new System.Drawing.Point(432, 82);
            this.BtnSearchMember.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSearchMember.Name = "BtnSearchMember";
            this.BtnSearchMember.Size = new System.Drawing.Size(126, 38);
            this.BtnSearchMember.TabIndex = 54;
            this.BtnSearchMember.Text = "Search";
            this.BtnSearchMember.UseVisualStyleBackColor = false;
            this.BtnSearchMember.Click += new System.EventHandler(this.BtnEnterMemberID_Click);
            // 
            // ColumnNumbering
            // 
            this.ColumnNumbering.HeaderText = "#";
            this.ColumnNumbering.MinimumWidth = 8;
            this.ColumnNumbering.Name = "ColumnNumbering";
            this.ColumnNumbering.Width = 55;
            // 
            // ColumnCheckBox
            // 
            this.ColumnCheckBox.HeaderText = "";
            this.ColumnCheckBox.MinimumWidth = 8;
            this.ColumnCheckBox.Name = "ColumnCheckBox";
            this.ColumnCheckBox.Width = 8;
            // 
            // ColumnTransactionID
            // 
            this.ColumnTransactionID.HeaderText = "Transaction ID";
            this.ColumnTransactionID.MinimumWidth = 8;
            this.ColumnTransactionID.Name = "ColumnTransactionID";
            this.ColumnTransactionID.Width = 144;
            // 
            // ColumnMemberID
            // 
            this.ColumnMemberID.HeaderText = "Member ID";
            this.ColumnMemberID.MinimumWidth = 8;
            this.ColumnMemberID.Name = "ColumnMemberID";
            this.ColumnMemberID.Width = 124;
            // 
            // ColumnMemberName
            // 
            this.ColumnMemberName.HeaderText = "Member Name";
            this.ColumnMemberName.MinimumWidth = 8;
            this.ColumnMemberName.Name = "ColumnMemberName";
            this.ColumnMemberName.Width = 151;
            // 
            // ColumnFineAmount
            // 
            this.ColumnFineAmount.HeaderText = "Fine Amount";
            this.ColumnFineAmount.MinimumWidth = 8;
            this.ColumnFineAmount.Name = "ColumnFineAmount";
            this.ColumnFineAmount.Width = 135;
            // 
            // ColumnFineType
            // 
            this.ColumnFineType.HeaderText = "Fine Type";
            this.ColumnFineType.MinimumWidth = 8;
            this.ColumnFineType.Name = "ColumnFineType";
            this.ColumnFineType.Width = 111;
            // 
            // ColumnDateIssued
            // 
            this.ColumnDateIssued.HeaderText = "Date Issued";
            this.ColumnDateIssued.MinimumWidth = 8;
            this.ColumnDateIssued.Name = "ColumnDateIssued";
            this.ColumnDateIssued.Width = 126;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.MinimumWidth = 8;
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.Width = 88;
            // 
            // UCFines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.TabCtrl);
            this.Controls.Add(this.GrpBxOustandingPenalties);
            this.Controls.Add(this.LblSearchMember);
            this.Controls.Add(this.BtnSearchMember);
            this.Controls.Add(this.TxtSearchMember);
            this.Controls.Add(this.LblName);
            this.Controls.Add(this.LblOverdueCount);
            this.Controls.Add(this.LblBooksCurrentlyBorrowed);
            this.Controls.Add(this.LblTotalFines);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.LblType);
            this.Controls.Add(this.LblMemberInfo);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCFines";
            this.Size = new System.Drawing.Size(1580, 936);
            this.GrpBxOustandingPenalties.ResumeLayout(false);
            this.GrpBxOustandingPenalties.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvFines)).EndInit();
            this.TabCtrl.ResumeLayout(false);
            this.TabPgAddCharges.ResumeLayout(false);
            this.FlowPnlforAddCharges.ResumeLayout(false);
            this.PnlforChargeType.ResumeLayout(false);
            this.PnlforChargeType.PerformLayout();
            this.PnlforAccessionNumber.ResumeLayout(false);
            this.PnlforAccessionNumber.PerformLayout();
            this.PnlforAmount.ResumeLayout(false);
            this.PnlforAmount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmount)).EndInit();
            this.TabPgWaiver.ResumeLayout(false);
            this.TabPgWaiver.PerformLayout();
            this.TabPgPayment.ResumeLayout(false);
            this.TabPgPayment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmountReceived)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Button BtnSettlePayment;
        private System.Windows.Forms.TextBox TxtSearchMember;
        private System.Windows.Forms.Label LblSearchMember;
        private System.Windows.Forms.Label LblType;
        private System.Windows.Forms.Label LblName;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.GroupBox GrpBxOustandingPenalties;
        private System.Windows.Forms.DataGridView DgvFines;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblSelectedTotal;
        private System.Windows.Forms.Label LblMemberInfo;
        private System.Windows.Forms.TabControl TabCtrl;
        private System.Windows.Forms.TabPage TabPgAddCharges;
        private System.Windows.Forms.TabPage TabPgWaiver;
        private System.Windows.Forms.TabPage TabPgPayment;
        private System.Windows.Forms.Label LblTotalFines;
        private System.Windows.Forms.TextBox TxtAccessionNumber;
        private System.Windows.Forms.NumericUpDown NumPckAmount;
        private System.Windows.Forms.ComboBox CmbBxChargeType;
        private System.Windows.Forms.Label LblAccessionNumber;
        private System.Windows.Forms.Label LblAmount;
        private System.Windows.Forms.Button BtnAddToList;
        private System.Windows.Forms.Label LblChargeType;
        private System.Windows.Forms.Label LblBooksCurrentlyBorrowed;
        private System.Windows.Forms.Label LblOverdueCount;
        private System.Windows.Forms.Label LblReason;
        private System.Windows.Forms.TextBox TxtReason;
        private System.Windows.Forms.Button BtnWaive;
        private System.Windows.Forms.ComboBox CmbBxMode;
        private System.Windows.Forms.Label LblMode;
        private System.Windows.Forms.NumericUpDown NumPckAmountReceived;
        private System.Windows.Forms.Label LblValueChange;
        private System.Windows.Forms.Label LblChange;
        private System.Windows.Forms.Label LblAmountReceived;
        private System.Windows.Forms.Button BtnSearchMember;
        private System.Windows.Forms.FlowLayoutPanel FlowPnlforAddCharges;
        private System.Windows.Forms.Panel PnlforAccessionNumber;
        private System.Windows.Forms.Panel PnlforChargeType;
        private System.Windows.Forms.Panel PnlforAmount;
        private System.Windows.Forms.Label LblValueAmounttoPay;
        private System.Windows.Forms.Label LblAmounttoPay;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumbering;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTransactionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMemberID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMemberName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFineAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateIssued;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
    }
}
