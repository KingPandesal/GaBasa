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
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new ReaLTaiizor.Controls.Button();
            this.button2 = new ReaLTaiizor.Controls.Button();
            this.button1 = new ReaLTaiizor.Controls.Button();
            this.button4 = new ReaLTaiizor.Controls.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.colTransactionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateBorrowed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateReturned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colBookID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.colFineID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateSettled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.colLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblOverdueBooks = new System.Windows.Forms.Label();
            this.button5 = new ReaLTaiizor.Controls.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label3.Location = new System.Drawing.Point(38, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 32);
            this.label3.TabIndex = 4;
            this.label3.Text = "From:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dateTimePicker1.Location = new System.Drawing.Point(114, 34);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(229, 30);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label4.Location = new System.Drawing.Point(393, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "To:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dateTimePicker2.Location = new System.Drawing.Point(440, 34);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(229, 30);
            this.dateTimePicker2.TabIndex = 8;
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
            this.comboBox1.Location = new System.Drawing.Point(1295, 231);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(57, 33);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.Text = "All";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(32, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(594, 96);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button3.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = null;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button3.Location = new System.Drawing.Point(448, 32);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button3.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button3.Size = new System.Drawing.Size(128, 42);
            this.button3.TabIndex = 33;
            this.button3.Text = "Audit Logs";
            this.button3.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = null;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.Location = new System.Drawing.Point(305, 32);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.Size = new System.Drawing.Size(128, 42);
            this.button2.TabIndex = 32;
            this.button2.Text = "Fines";
            this.button2.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button1.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = null;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button1.Location = new System.Drawing.Point(162, 32);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button1.Size = new System.Drawing.Size(128, 42);
            this.button1.TabIndex = 31;
            this.button1.Text = "Transactions";
            this.button1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.White;
            this.button4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = null;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Location = new System.Drawing.Point(19, 32);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Size = new System.Drawing.Size(128, 42);
            this.button4.TabIndex = 30;
            this.button4.Text = "Inventory";
            this.button4.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Location = new System.Drawing.Point(625, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(719, 92);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label5.Location = new System.Drawing.Point(1130, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 36);
            this.label5.TabIndex = 9;
            this.label5.Text = "Member Type:";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTransactionID,
            this.colName,
            this.colTitle,
            this.colDateBorrowed,
            this.colDueDate,
            this.colDateReturned,
            this.colStatus});
            this.dataGridView2.Location = new System.Drawing.Point(32, 187);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 28;
            this.dataGridView2.Size = new System.Drawing.Size(1341, 359);
            this.dataGridView2.TabIndex = 87;
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
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBookID,
            this.colAccNo,
            this.colTitle2,
            this.colCategory,
            this.colStatus2,
            this.colDateAdded,
            this.colAddedBy});
            this.dataGridView1.Location = new System.Drawing.Point(32, 228);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1341, 359);
            this.dataGridView1.TabIndex = 88;
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
            // dataGridView3
            // 
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFineID,
            this.colName2,
            this.colFineType,
            this.colAmount,
            this.colPaymentStatus,
            this.colDateSettled});
            this.dataGridView3.Location = new System.Drawing.Point(32, 279);
            this.dataGridView3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersWidth = 62;
            this.dataGridView3.RowTemplate.Height = 28;
            this.dataGridView3.Size = new System.Drawing.Size(1341, 359);
            this.dataGridView3.TabIndex = 89;
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
            // dataGridView4
            // 
            this.dataGridView4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView4.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLogID,
            this.colUser,
            this.colAction,
            this.colTimestamp,
            this.colModule});
            this.dataGridView4.Location = new System.Drawing.Point(32, 330);
            this.dataGridView4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowHeadersWidth = 62;
            this.dataGridView4.RowTemplate.Height = 28;
            this.dataGridView4.Size = new System.Drawing.Size(1341, 359);
            this.dataGridView4.TabIndex = 90;
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
            this.colAction.HeaderText = "Action Taken";
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
            // LblOverdueBooks
            // 
            this.LblOverdueBooks.AutoSize = true;
            this.LblOverdueBooks.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOverdueBooks.Location = new System.Drawing.Point(27, 140);
            this.LblOverdueBooks.Name = "LblOverdueBooks";
            this.LblOverdueBooks.Size = new System.Drawing.Size(217, 36);
            this.LblOverdueBooks.TabIndex = 91;
            this.LblOverdueBooks.Text = "Inventory Report";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.White;
            this.button5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button5.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Image = null;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button5.Location = new System.Drawing.Point(1245, 134);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button5.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button5.Size = new System.Drawing.Size(128, 42);
            this.button5.TabIndex = 34;
            this.button5.Text = "Export";
            this.button5.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // UCReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button5);
            this.Controls.Add(this.LblOverdueBooks);
            this.Controls.Add(this.dataGridView4);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCReports";
            this.Size = new System.Drawing.Size(1404, 749);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private ReaLTaiizor.Controls.Button button4;
        private ReaLTaiizor.Controls.Button button3;
        private ReaLTaiizor.Controls.Button button2;
        private ReaLTaiizor.Controls.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransactionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateBorrowed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateReturned;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBookID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFineID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateSettled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModule;
        private System.Windows.Forms.Label LblOverdueBooks;
        private ReaLTaiizor.Controls.Button button5;
    }
}
