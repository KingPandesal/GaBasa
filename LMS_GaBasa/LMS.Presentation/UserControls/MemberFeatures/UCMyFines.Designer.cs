namespace LMS.Presentation.UserControls.MemberFeatures
{
    partial class UCMyFines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMyFines));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DgvListOfFines = new System.Windows.Forms.DataGridView();
            this.PnlforTotalOustandingBalance = new System.Windows.Forms.Panel();
            this.LblValueOustandingBalance = new System.Windows.Forms.Label();
            this.LblOustandingBalance = new System.Windows.Forms.Label();
            this.PnlforAccountStanding = new System.Windows.Forms.Panel();
            this.LblValueAccountStanding = new System.Windows.Forms.Label();
            this.LblAccountStanding = new System.Windows.Forms.Label();
            this.LblListOfFines = new System.Windows.Forms.Label();
            this.GrpBxLibraryFinePolicy = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.La = new System.Windows.Forms.Label();
            this.LblOverduePerDay = new System.Windows.Forms.Label();
            this.GrpBxforBalanceandStanding = new System.Windows.Forms.GroupBox();
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.ColumnNumbering = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDateIncurred = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTitleDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDaysLate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DgvListOfFines)).BeginInit();
            this.PnlforTotalOustandingBalance.SuspendLayout();
            this.PnlforAccountStanding.SuspendLayout();
            this.GrpBxLibraryFinePolicy.SuspendLayout();
            this.GrpBxforBalanceandStanding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvListOfFines
            // 
            this.DgvListOfFines.BackgroundColor = System.Drawing.Color.White;
            this.DgvListOfFines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvListOfFines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNumbering,
            this.ColumnDateIncurred,
            this.ColumnTitleDescription,
            this.ColumnDaysLate,
            this.ColumnAmount,
            this.ColumnStatus});
            this.DgvListOfFines.Location = new System.Drawing.Point(36, 277);
            this.DgvListOfFines.Margin = new System.Windows.Forms.Padding(2);
            this.DgvListOfFines.Name = "DgvListOfFines";
            this.DgvListOfFines.RowHeadersWidth = 62;
            this.DgvListOfFines.RowTemplate.Height = 28;
            this.DgvListOfFines.Size = new System.Drawing.Size(1510, 316);
            this.DgvListOfFines.TabIndex = 31;
            // 
            // PnlforTotalOustandingBalance
            // 
            this.PnlforTotalOustandingBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlforTotalOustandingBalance.Controls.Add(this.LblValueOustandingBalance);
            this.PnlforTotalOustandingBalance.Controls.Add(this.LblOustandingBalance);
            this.PnlforTotalOustandingBalance.Location = new System.Drawing.Point(194, 31);
            this.PnlforTotalOustandingBalance.Name = "PnlforTotalOustandingBalance";
            this.PnlforTotalOustandingBalance.Size = new System.Drawing.Size(379, 53);
            this.PnlforTotalOustandingBalance.TabIndex = 86;
            // 
            // LblValueOustandingBalance
            // 
            this.LblValueOustandingBalance.AutoSize = true;
            this.LblValueOustandingBalance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblValueOustandingBalance.Location = new System.Drawing.Point(269, 11);
            this.LblValueOustandingBalance.Name = "LblValueOustandingBalance";
            this.LblValueOustandingBalance.Size = new System.Drawing.Size(84, 28);
            this.LblValueOustandingBalance.TabIndex = 1;
            this.LblValueOustandingBalance.Text = "127623";
            // 
            // LblOustandingBalance
            // 
            this.LblOustandingBalance.AutoSize = true;
            this.LblOustandingBalance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblOustandingBalance.Location = new System.Drawing.Point(24, 11);
            this.LblOustandingBalance.Name = "LblOustandingBalance";
            this.LblOustandingBalance.Size = new System.Drawing.Size(239, 28);
            this.LblOustandingBalance.TabIndex = 0;
            this.LblOustandingBalance.Text = "Total Outstanding Balance";
            // 
            // PnlforAccountStanding
            // 
            this.PnlforAccountStanding.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlforAccountStanding.Controls.Add(this.LblValueAccountStanding);
            this.PnlforAccountStanding.Controls.Add(this.LblAccountStanding);
            this.PnlforAccountStanding.Location = new System.Drawing.Point(896, 31);
            this.PnlforAccountStanding.Name = "PnlforAccountStanding";
            this.PnlforAccountStanding.Size = new System.Drawing.Size(357, 53);
            this.PnlforAccountStanding.TabIndex = 87;
            // 
            // LblValueAccountStanding
            // 
            this.LblValueAccountStanding.AutoSize = true;
            this.LblValueAccountStanding.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblValueAccountStanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LblValueAccountStanding.Location = new System.Drawing.Point(186, 5);
            this.LblValueAccountStanding.Name = "LblValueAccountStanding";
            this.LblValueAccountStanding.Size = new System.Drawing.Size(149, 36);
            this.LblValueAccountStanding.TabIndex = 1;
            this.LblValueAccountStanding.Text = "Suspended";
            // 
            // LblAccountStanding
            // 
            this.LblAccountStanding.AutoSize = true;
            this.LblAccountStanding.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblAccountStanding.Location = new System.Drawing.Point(19, 11);
            this.LblAccountStanding.Name = "LblAccountStanding";
            this.LblAccountStanding.Size = new System.Drawing.Size(167, 28);
            this.LblAccountStanding.TabIndex = 0;
            this.LblAccountStanding.Text = "Account Standing";
            // 
            // LblListOfFines
            // 
            this.LblListOfFines.AutoSize = true;
            this.LblListOfFines.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblListOfFines.Location = new System.Drawing.Point(31, 232);
            this.LblListOfFines.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblListOfFines.Name = "LblListOfFines";
            this.LblListOfFines.Size = new System.Drawing.Size(154, 36);
            this.LblListOfFines.TabIndex = 89;
            this.LblListOfFines.Text = "List of Fines";
            // 
            // GrpBxLibraryFinePolicy
            // 
            this.GrpBxLibraryFinePolicy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GrpBxLibraryFinePolicy.Controls.Add(this.label8);
            this.GrpBxLibraryFinePolicy.Controls.Add(this.label7);
            this.GrpBxLibraryFinePolicy.Controls.Add(this.La);
            this.GrpBxLibraryFinePolicy.Controls.Add(this.LblOverduePerDay);
            this.GrpBxLibraryFinePolicy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.GrpBxLibraryFinePolicy.Location = new System.Drawing.Point(36, 701);
            this.GrpBxLibraryFinePolicy.Name = "GrpBxLibraryFinePolicy";
            this.GrpBxLibraryFinePolicy.Size = new System.Drawing.Size(373, 156);
            this.GrpBxLibraryFinePolicy.TabIndex = 90;
            this.GrpBxLibraryFinePolicy.TabStop = false;
            this.GrpBxLibraryFinePolicy.Text = "Library Fine Policy";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label8.Location = new System.Drawing.Point(23, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(260, 28);
            this.label8.TabIndex = 3;
            this.label8.Text = "• ID Card Replacement: P200";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label7.Location = new System.Drawing.Point(23, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(291, 28);
            this.label7.TabIndex = 2;
            this.label7.Text = "• Damage: Based on Assessment";
            // 
            // La
            // 
            this.La.AutoSize = true;
            this.La.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.La.Location = new System.Drawing.Point(23, 59);
            this.La.Name = "La";
            this.La.Size = new System.Drawing.Size(265, 28);
            this.La.TabIndex = 1;
            this.La.Text = "• Overdue: Replacement Cost";
            // 
            // LblOverduePerDay
            // 
            this.LblOverduePerDay.AutoSize = true;
            this.LblOverduePerDay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblOverduePerDay.Location = new System.Drawing.Point(23, 31);
            this.LblOverduePerDay.Name = "LblOverduePerDay";
            this.LblOverduePerDay.Size = new System.Drawing.Size(213, 28);
            this.LblOverduePerDay.TabIndex = 0;
            this.LblOverduePerDay.Text = "• Overdue: P10 per day";
            this.LblOverduePerDay.Click += new System.EventHandler(this.LblOverduePerDay_Click);
            // 
            // GrpBxforBalanceandStanding
            // 
            this.GrpBxforBalanceandStanding.Controls.Add(this.PnlforTotalOustandingBalance);
            this.GrpBxforBalanceandStanding.Controls.Add(this.PnlforAccountStanding);
            this.GrpBxforBalanceandStanding.Location = new System.Drawing.Point(36, 99);
            this.GrpBxforBalanceandStanding.Name = "GrpBxforBalanceandStanding";
            this.GrpBxforBalanceandStanding.Size = new System.Drawing.Size(1510, 107);
            this.GrpBxforBalanceandStanding.TabIndex = 91;
            this.GrpBxforBalanceandStanding.TabStop = false;
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(36, 35);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 94;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 36);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 93;
            this.LblSearch.Text = "Search";
            // 
            // TxtSearchBar
            // 
            this.TxtSearchBar.BackColor = System.Drawing.Color.White;
            this.TxtSearchBar.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.TxtSearchBar.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearchBar.Image = null;
            this.TxtSearchBar.Location = new System.Drawing.Point(155, 32);
            this.TxtSearchBar.MaximumSize = new System.Drawing.Size(1500, 40);
            this.TxtSearchBar.MaxLength = 32767;
            this.TxtSearchBar.MinimumSize = new System.Drawing.Size(0, 33);
            this.TxtSearchBar.Multiline = false;
            this.TxtSearchBar.Name = "TxtSearchBar";
            this.TxtSearchBar.ReadOnly = false;
            this.TxtSearchBar.Size = new System.Drawing.Size(1391, 40);
            this.TxtSearchBar.TabIndex = 92;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // ColumnNumbering
            // 
            this.ColumnNumbering.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnNumbering.HeaderText = "#";
            this.ColumnNumbering.MinimumWidth = 8;
            this.ColumnNumbering.Name = "ColumnNumbering";
            this.ColumnNumbering.Width = 54;
            // 
            // ColumnDateIncurred
            // 
            this.ColumnDateIncurred.HeaderText = "Date Incurred";
            this.ColumnDateIncurred.MinimumWidth = 6;
            this.ColumnDateIncurred.Name = "ColumnDateIncurred";
            this.ColumnDateIncurred.ReadOnly = true;
            this.ColumnDateIncurred.Width = 150;
            // 
            // ColumnTitleDescription
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnTitleDescription.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnTitleDescription.HeaderText = "Title / Description";
            this.ColumnTitleDescription.MinimumWidth = 6;
            this.ColumnTitleDescription.Name = "ColumnTitleDescription";
            this.ColumnTitleDescription.ReadOnly = true;
            this.ColumnTitleDescription.Width = 322;
            // 
            // ColumnDaysLate
            // 
            this.ColumnDaysLate.HeaderText = "Days Late";
            this.ColumnDaysLate.MinimumWidth = 6;
            this.ColumnDaysLate.Name = "ColumnDaysLate";
            this.ColumnDaysLate.ReadOnly = true;
            this.ColumnDaysLate.Width = 150;
            // 
            // ColumnAmount
            // 
            dataGridViewCellStyle2.Format = "C2";
            this.ColumnAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnAmount.HeaderText = "Amount";
            this.ColumnAmount.MinimumWidth = 6;
            this.ColumnAmount.Name = "ColumnAmount";
            this.ColumnAmount.ReadOnly = true;
            this.ColumnAmount.Width = 150;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.MinimumWidth = 6;
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            this.ColumnStatus.Width = 120;
            // 
            // UCMyFines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.TxtSearchBar);
            this.Controls.Add(this.GrpBxforBalanceandStanding);
            this.Controls.Add(this.GrpBxLibraryFinePolicy);
            this.Controls.Add(this.LblListOfFines);
            this.Controls.Add(this.DgvListOfFines);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCMyFines";
            this.Size = new System.Drawing.Size(1580, 936);
            this.Load += new System.EventHandler(this.UCMyFines_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvListOfFines)).EndInit();
            this.PnlforTotalOustandingBalance.ResumeLayout(false);
            this.PnlforTotalOustandingBalance.PerformLayout();
            this.PnlforAccountStanding.ResumeLayout(false);
            this.PnlforAccountStanding.PerformLayout();
            this.GrpBxLibraryFinePolicy.ResumeLayout(false);
            this.GrpBxLibraryFinePolicy.PerformLayout();
            this.GrpBxforBalanceandStanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvListOfFines;
        private System.Windows.Forms.Panel PnlforTotalOustandingBalance;
        private System.Windows.Forms.Label LblOustandingBalance;
        private System.Windows.Forms.Label LblValueOustandingBalance;
        private System.Windows.Forms.Panel PnlforAccountStanding;
        private System.Windows.Forms.Label LblValueAccountStanding;
        private System.Windows.Forms.Label LblAccountStanding;
        private System.Windows.Forms.Label LblListOfFines;
        private System.Windows.Forms.GroupBox GrpBxLibraryFinePolicy;
        private System.Windows.Forms.Label LblOverduePerDay;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label La;
        private System.Windows.Forms.GroupBox GrpBxforBalanceandStanding;
        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.Label LblSearch;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNumbering;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateIncurred;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTitleDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDaysLate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
    }
}
