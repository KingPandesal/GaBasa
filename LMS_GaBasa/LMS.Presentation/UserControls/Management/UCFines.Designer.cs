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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DgwInventory = new System.Windows.Forms.DataGridView();
            this.ColumnNumbering = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBtnCoverImage = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnBtnCopies = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TabCtrl = new System.Windows.Forms.TabControl();
            this.TabPgAddCharges = new System.Windows.Forms.TabPage();
            this.TxtAccessionNumber = new System.Windows.Forms.TextBox();
            this.NumPckAmount = new System.Windows.Forms.NumericUpDown();
            this.CmbBxChargeType = new System.Windows.Forms.ComboBox();
            this.LblAccessionNumber = new System.Windows.Forms.Label();
            this.LblAmount = new System.Windows.Forms.Label();
            this.BtnAddToList = new System.Windows.Forms.Button();
            this.LblChargeType = new System.Windows.Forms.Label();
            this.TabPgWaiver = new System.Windows.Forms.TabPage();
            this.TabPgPayment = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LblReason = new System.Windows.Forms.Label();
            this.TxtReason = new System.Windows.Forms.TextBox();
            this.BtnWaive = new System.Windows.Forms.Button();
            this.CmbBxMode = new System.Windows.Forms.ComboBox();
            this.LblMode = new System.Windows.Forms.Label();
            this.LblAmountReceived = new System.Windows.Forms.Label();
            this.NumPckAmountReceived = new System.Windows.Forms.NumericUpDown();
            this.LblChange = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.BtnSearchMember = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgwInventory)).BeginInit();
            this.TabCtrl.SuspendLayout();
            this.TabPgAddCharges.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmount)).BeginInit();
            this.TabPgWaiver.SuspendLayout();
            this.TabPgPayment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmountReceived)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
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
            this.BtnSettlePayment.Location = new System.Drawing.Point(45, 222);
            this.BtnSettlePayment.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSettlePayment.Name = "BtnSettlePayment";
            this.BtnSettlePayment.Size = new System.Drawing.Size(182, 44);
            this.BtnSettlePayment.TabIndex = 54;
            this.BtnSettlePayment.Text = "Settle Payment";
            this.BtnSettlePayment.UseVisualStyleBackColor = false;
            this.BtnSettlePayment.Click += new System.EventHandler(this.BtnEnterMemberID_Click);
            // 
            // TxtSearchMember
            // 
            this.TxtSearchMember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TxtSearchMember.Location = new System.Drawing.Point(37, 83);
            this.TxtSearchMember.Name = "TxtSearchMember";
            this.TxtSearchMember.Size = new System.Drawing.Size(343, 34);
            this.TxtSearchMember.TabIndex = 53;
            // 
            // LblSearchMember
            // 
            this.LblSearchMember.AutoSize = true;
            this.LblSearchMember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblSearchMember.ForeColor = System.Drawing.Color.Black;
            this.LblSearchMember.Location = new System.Drawing.Point(32, 33);
            this.LblSearchMember.Name = "LblSearchMember";
            this.LblSearchMember.Size = new System.Drawing.Size(149, 28);
            this.LblSearchMember.TabIndex = 52;
            this.LblSearchMember.Text = "Search Member";
            this.LblSearchMember.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(630, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 28);
            this.label2.TabIndex = 52;
            this.label2.Text = "Type:";
            this.label2.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(630, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 28);
            this.label3.TabIndex = 52;
            this.label3.Text = "Name:";
            this.label3.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(630, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 28);
            this.label4.TabIndex = 52;
            this.label4.Text = "Status:";
            this.label4.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.groupBox1.Location = new System.Drawing.Point(635, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(901, 603);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Outstanding Penalties";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DgwInventory);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.panel1.Location = new System.Drawing.Point(23, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(853, 295);
            this.panel1.TabIndex = 60;
            // 
            // DgwInventory
            // 
            this.DgwInventory.AllowUserToAddRows = false;
            this.DgwInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgwInventory.BackgroundColor = System.Drawing.Color.White;
            this.DgwInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgwInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNumbering,
            this.Column7,
            this.ColumnStatus,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.ColumnBtnCoverImage,
            this.ColumnBtnCopies,
            this.Edit});
            this.DgwInventory.GridColor = System.Drawing.SystemColors.ControlLight;
            this.DgwInventory.Location = new System.Drawing.Point(4, 19);
            this.DgwInventory.Name = "DgwInventory";
            this.DgwInventory.RowHeadersWidth = 62;
            this.DgwInventory.RowTemplate.Height = 28;
            this.DgwInventory.Size = new System.Drawing.Size(846, 272);
            this.DgwInventory.TabIndex = 59;
            // 
            // ColumnNumbering
            // 
            this.ColumnNumbering.HeaderText = "#";
            this.ColumnNumbering.MinimumWidth = 8;
            this.ColumnNumbering.Name = "ColumnNumbering";
            this.ColumnNumbering.Width = 55;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "";
            this.Column7.MinimumWidth = 8;
            this.Column7.Name = "Column7";
            this.Column7.Width = 8;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.HeaderText = "Transaction ID";
            this.ColumnStatus.MinimumWidth = 8;
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.Width = 144;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Member ID";
            this.Column1.MinimumWidth = 8;
            this.Column1.Name = "Column1";
            this.Column1.Width = 124;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Member Name";
            this.Column2.MinimumWidth = 8;
            this.Column2.Name = "Column2";
            this.Column2.Width = 151;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Fine Amount";
            this.Column3.MinimumWidth = 8;
            this.Column3.Name = "Column3";
            this.Column3.Width = 135;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Fine Type";
            this.Column4.MinimumWidth = 8;
            this.Column4.Name = "Column4";
            this.Column4.Width = 111;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Date Issued";
            this.Column5.MinimumWidth = 8;
            this.Column5.Name = "Column5";
            this.Column5.Width = 126;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Status";
            this.Column6.MinimumWidth = 8;
            this.Column6.Name = "Column6";
            this.Column6.Width = 88;
            // 
            // ColumnBtnCoverImage
            // 
            this.ColumnBtnCoverImage.HeaderText = "Cover Image";
            this.ColumnBtnCoverImage.MinimumWidth = 8;
            this.ColumnBtnCoverImage.Name = "ColumnBtnCoverImage";
            this.ColumnBtnCoverImage.Text = "View Cover Image";
            this.ColumnBtnCoverImage.Width = 104;
            // 
            // ColumnBtnCopies
            // 
            this.ColumnBtnCopies.HeaderText = "Copies";
            this.ColumnBtnCopies.MinimumWidth = 8;
            this.ColumnBtnCopies.Name = "ColumnBtnCopies";
            this.ColumnBtnCopies.Text = "View All Copies";
            this.ColumnBtnCopies.Width = 63;
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(22, 384);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 28);
            this.label6.TabIndex = 52;
            this.label6.Text = "Selected Total:";
            this.label6.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(22, 349);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 28);
            this.label5.TabIndex = 52;
            this.label5.Text = "Total Balance:";
            this.label5.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(630, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 28);
            this.label1.TabIndex = 52;
            this.label1.Text = "Member Info";
            this.label1.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // TabCtrl
            // 
            this.TabCtrl.Controls.Add(this.TabPgAddCharges);
            this.TabCtrl.Controls.Add(this.TabPgWaiver);
            this.TabCtrl.Controls.Add(this.TabPgPayment);
            this.TabCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.TabCtrl.Location = new System.Drawing.Point(37, 171);
            this.TabCtrl.Name = "TabCtrl";
            this.TabCtrl.SelectedIndex = 0;
            this.TabCtrl.Size = new System.Drawing.Size(571, 624);
            this.TabCtrl.TabIndex = 57;
            // 
            // TabPgAddCharges
            // 
            this.TabPgAddCharges.BackColor = System.Drawing.Color.White;
            this.TabPgAddCharges.Controls.Add(this.flowLayoutPanel1);
            this.TabPgAddCharges.Controls.Add(this.BtnAddToList);
            this.TabPgAddCharges.Location = new System.Drawing.Point(4, 34);
            this.TabPgAddCharges.Name = "TabPgAddCharges";
            this.TabPgAddCharges.Padding = new System.Windows.Forms.Padding(3);
            this.TabPgAddCharges.Size = new System.Drawing.Size(563, 586);
            this.TabPgAddCharges.TabIndex = 0;
            this.TabPgAddCharges.Text = "Add Charges";
            // 
            // TxtAccessionNumber
            // 
            this.TxtAccessionNumber.Location = new System.Drawing.Point(189, 3);
            this.TxtAccessionNumber.Name = "TxtAccessionNumber";
            this.TxtAccessionNumber.Size = new System.Drawing.Size(196, 30);
            this.TxtAccessionNumber.TabIndex = 62;
            // 
            // NumPckAmount
            // 
            this.NumPckAmount.Location = new System.Drawing.Point(190, 1);
            this.NumPckAmount.Name = "NumPckAmount";
            this.NumPckAmount.Size = new System.Drawing.Size(196, 30);
            this.NumPckAmount.TabIndex = 60;
            // 
            // CmbBxChargeType
            // 
            this.CmbBxChargeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBxChargeType.FormattingEnabled = true;
            this.CmbBxChargeType.Items.AddRange(new object[] {
            "ID Card Replacement",
            "Damaged Book Fee"});
            this.CmbBxChargeType.Location = new System.Drawing.Point(189, 0);
            this.CmbBxChargeType.Name = "CmbBxChargeType";
            this.CmbBxChargeType.Size = new System.Drawing.Size(196, 33);
            this.CmbBxChargeType.TabIndex = 59;
            // 
            // LblAccessionNumber
            // 
            this.LblAccessionNumber.AutoSize = true;
            this.LblAccessionNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAccessionNumber.ForeColor = System.Drawing.Color.Black;
            this.LblAccessionNumber.Location = new System.Drawing.Point(-6, 3);
            this.LblAccessionNumber.Name = "LblAccessionNumber";
            this.LblAccessionNumber.Size = new System.Drawing.Size(178, 28);
            this.LblAccessionNumber.TabIndex = 56;
            this.LblAccessionNumber.Text = "Accession Number:";
            // 
            // LblAmount
            // 
            this.LblAmount.AutoSize = true;
            this.LblAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAmount.ForeColor = System.Drawing.Color.Black;
            this.LblAmount.Location = new System.Drawing.Point(-5, 0);
            this.LblAmount.Name = "LblAmount";
            this.LblAmount.Size = new System.Drawing.Size(87, 28);
            this.LblAmount.TabIndex = 57;
            this.LblAmount.Text = "Amount:";
            // 
            // BtnAddToList
            // 
            this.BtnAddToList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddToList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAddToList.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnAddToList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddToList.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnAddToList.ForeColor = System.Drawing.Color.White;
            this.BtnAddToList.Location = new System.Drawing.Point(38, 241);
            this.BtnAddToList.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAddToList.Name = "BtnAddToList";
            this.BtnAddToList.Size = new System.Drawing.Size(187, 44);
            this.BtnAddToList.TabIndex = 61;
            this.BtnAddToList.Text = "Add to List";
            this.BtnAddToList.UseVisualStyleBackColor = false;
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
            // TabPgWaiver
            // 
            this.TabPgWaiver.BackColor = System.Drawing.Color.White;
            this.TabPgWaiver.Controls.Add(this.LblReason);
            this.TabPgWaiver.Controls.Add(this.TxtReason);
            this.TabPgWaiver.Controls.Add(this.BtnWaive);
            this.TabPgWaiver.Location = new System.Drawing.Point(4, 34);
            this.TabPgWaiver.Name = "TabPgWaiver";
            this.TabPgWaiver.Padding = new System.Windows.Forms.Padding(3);
            this.TabPgWaiver.Size = new System.Drawing.Size(563, 586);
            this.TabPgWaiver.TabIndex = 1;
            this.TabPgWaiver.Text = "Waiver";
            // 
            // TabPgPayment
            // 
            this.TabPgPayment.BackColor = System.Drawing.Color.White;
            this.TabPgPayment.Controls.Add(this.NumPckAmountReceived);
            this.TabPgPayment.Controls.Add(this.CmbBxMode);
            this.TabPgPayment.Controls.Add(this.label17);
            this.TabPgPayment.Controls.Add(this.BtnSettlePayment);
            this.TabPgPayment.Controls.Add(this.LblChange);
            this.TabPgPayment.Controls.Add(this.LblAmountReceived);
            this.TabPgPayment.Controls.Add(this.LblMode);
            this.TabPgPayment.Location = new System.Drawing.Point(4, 34);
            this.TabPgPayment.Name = "TabPgPayment";
            this.TabPgPayment.Padding = new System.Windows.Forms.Padding(3);
            this.TabPgPayment.Size = new System.Drawing.Size(563, 586);
            this.TabPgPayment.TabIndex = 2;
            this.TabPgPayment.Text = "Payment";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(1055, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 28);
            this.label7.TabIndex = 52;
            this.label7.Text = "Total Fines:";
            this.label7.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(1055, 89);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(244, 28);
            this.label11.TabIndex = 52;
            this.label11.Text = "Books Currently Borrowed:";
            this.label11.Click += new System.EventHandler(this.LblMemberID_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(1055, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(194, 28);
            this.label12.TabIndex = 52;
            this.label12.Text = "Overdue Count: 0 / 5";
            this.label12.Click += new System.EventHandler(this.LblMemberID_Click);
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
            this.TxtReason.Multiline = true;
            this.TxtReason.Name = "TxtReason";
            this.TxtReason.Size = new System.Drawing.Size(403, 82);
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
            this.BtnWaive.Location = new System.Drawing.Point(39, 197);
            this.BtnWaive.Margin = new System.Windows.Forms.Padding(4);
            this.BtnWaive.Name = "BtnWaive";
            this.BtnWaive.Size = new System.Drawing.Size(108, 44);
            this.BtnWaive.TabIndex = 54;
            this.BtnWaive.Text = "Waive";
            this.BtnWaive.UseVisualStyleBackColor = false;
            this.BtnWaive.Click += new System.EventHandler(this.BtnEnterMemberID_Click);
            // 
            // CmbBxMode
            // 
            this.CmbBxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBxMode.FormattingEnabled = true;
            this.CmbBxMode.Items.AddRange(new object[] {
            "Cash",
            "Online"});
            this.CmbBxMode.Location = new System.Drawing.Point(235, 39);
            this.CmbBxMode.Name = "CmbBxMode";
            this.CmbBxMode.Size = new System.Drawing.Size(196, 33);
            this.CmbBxMode.TabIndex = 61;
            // 
            // LblMode
            // 
            this.LblMode.AutoSize = true;
            this.LblMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblMode.ForeColor = System.Drawing.Color.Black;
            this.LblMode.Location = new System.Drawing.Point(40, 39);
            this.LblMode.Name = "LblMode";
            this.LblMode.Size = new System.Drawing.Size(68, 28);
            this.LblMode.TabIndex = 60;
            this.LblMode.Text = "Mode:";
            // 
            // LblAmountReceived
            // 
            this.LblAmountReceived.AutoSize = true;
            this.LblAmountReceived.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAmountReceived.ForeColor = System.Drawing.Color.Black;
            this.LblAmountReceived.Location = new System.Drawing.Point(40, 96);
            this.LblAmountReceived.Name = "LblAmountReceived";
            this.LblAmountReceived.Size = new System.Drawing.Size(169, 28);
            this.LblAmountReceived.TabIndex = 60;
            this.LblAmountReceived.Text = "Amount Received:";
            // 
            // NumPckAmountReceived
            // 
            this.NumPckAmountReceived.Location = new System.Drawing.Point(235, 97);
            this.NumPckAmountReceived.Name = "NumPckAmountReceived";
            this.NumPckAmountReceived.Size = new System.Drawing.Size(196, 30);
            this.NumPckAmountReceived.TabIndex = 62;
            // 
            // LblChange
            // 
            this.LblChange.AutoSize = true;
            this.LblChange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblChange.ForeColor = System.Drawing.Color.Black;
            this.LblChange.Location = new System.Drawing.Point(40, 158);
            this.LblChange.Name = "LblChange";
            this.LblChange.Size = new System.Drawing.Size(82, 28);
            this.LblChange.TabIndex = 60;
            this.LblChange.Text = "Change:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(230, 158);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 28);
            this.label17.TabIndex = 60;
            this.label17.Text = "P0";
            // 
            // BtnSearchMember
            // 
            this.BtnSearchMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSearchMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSearchMember.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSearchMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearchMember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSearchMember.ForeColor = System.Drawing.Color.White;
            this.BtnSearchMember.Location = new System.Drawing.Point(387, 78);
            this.BtnSearchMember.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSearchMember.Name = "BtnSearchMember";
            this.BtnSearchMember.Size = new System.Drawing.Size(131, 44);
            this.BtnSearchMember.TabIndex = 54;
            this.BtnSearchMember.Text = "Search";
            this.BtnSearchMember.UseVisualStyleBackColor = false;
            this.BtnSearchMember.Click += new System.EventHandler(this.BtnEnterMemberID_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(38, 39);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(466, 182);
            this.flowLayoutPanel1.TabIndex = 63;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TxtAccessionNumber);
            this.panel2.Controls.Add(this.LblAccessionNumber);
            this.panel2.Location = new System.Drawing.Point(3, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(405, 44);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.CmbBxChargeType);
            this.panel3.Controls.Add(this.LblChargeType);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(405, 44);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.LblAmount);
            this.panel4.Controls.Add(this.NumPckAmount);
            this.panel4.Location = new System.Drawing.Point(3, 103);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(405, 44);
            this.panel4.TabIndex = 2;
            // 
            // UCFines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.TabCtrl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LblSearchMember);
            this.Controls.Add(this.BtnSearchMember);
            this.Controls.Add(this.TxtSearchMember);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UCFines";
            this.Size = new System.Drawing.Size(1580, 936);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgwInventory)).EndInit();
            this.TabCtrl.ResumeLayout(false);
            this.TabPgAddCharges.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmount)).EndInit();
            this.TabPgWaiver.ResumeLayout(false);
            this.TabPgWaiver.PerformLayout();
            this.TabPgPayment.ResumeLayout(false);
            this.TabPgPayment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPckAmountReceived)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Button BtnSettlePayment;
        private System.Windows.Forms.TextBox TxtSearchMember;
        private System.Windows.Forms.Label LblSearchMember;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DgwInventory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumbering;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBtnCoverImage;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBtnCopies;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl TabCtrl;
        private System.Windows.Forms.TabPage TabPgAddCharges;
        private System.Windows.Forms.TabPage TabPgWaiver;
        private System.Windows.Forms.TabPage TabPgPayment;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtAccessionNumber;
        private System.Windows.Forms.NumericUpDown NumPckAmount;
        private System.Windows.Forms.ComboBox CmbBxChargeType;
        private System.Windows.Forms.Label LblAccessionNumber;
        private System.Windows.Forms.Label LblAmount;
        private System.Windows.Forms.Button BtnAddToList;
        private System.Windows.Forms.Label LblChargeType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label LblReason;
        private System.Windows.Forms.TextBox TxtReason;
        private System.Windows.Forms.Button BtnWaive;
        private System.Windows.Forms.ComboBox CmbBxMode;
        private System.Windows.Forms.Label LblMode;
        private System.Windows.Forms.NumericUpDown NumPckAmountReceived;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label LblChange;
        private System.Windows.Forms.Label LblAmountReceived;
        private System.Windows.Forms.Button BtnSearchMember;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
    }
}
