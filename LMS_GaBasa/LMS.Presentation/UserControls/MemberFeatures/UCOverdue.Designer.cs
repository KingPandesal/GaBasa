namespace LMS.Presentation.UserControls.MemberFeatures
{
    partial class UCOverdue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCOverdue));
            this.LblOverdueBooks = new System.Windows.Forms.Label();
            this.PnlOverdueBooks = new System.Windows.Forms.Panel();
            this.Pnl1 = new System.Windows.Forms.Panel();
            this.LblOverdue = new System.Windows.Forms.Label();
            this.LblCurrentFine = new System.Windows.Forms.Label();
            this.LblViewDetails = new System.Windows.Forms.Label();
            this.LblDueDate = new System.Windows.Forms.Label();
            this.LblAccesionNumber = new System.Windows.Forms.Label();
            this.LblAuthors = new System.Windows.Forms.Label();
            this.LblCategory = new System.Windows.Forms.Label();
            this.PicBxBookCoverImage = new System.Windows.Forms.PictureBox();
            this.LblTitle = new System.Windows.Forms.Label();
            this.PnlforFine = new System.Windows.Forms.Panel();
            this.LblFineWarning = new System.Windows.Forms.Label();
            this.LblTotalOutstandingFine = new System.Windows.Forms.Label();
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.PnlOverdueBooks.SuspendLayout();
            this.Pnl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).BeginInit();
            this.PnlforFine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // LblOverdueBooks
            // 
            this.LblOverdueBooks.AutoSize = true;
            this.LblOverdueBooks.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOverdueBooks.Location = new System.Drawing.Point(34, 175);
            this.LblOverdueBooks.Name = "LblOverdueBooks";
            this.LblOverdueBooks.Size = new System.Drawing.Size(197, 36);
            this.LblOverdueBooks.TabIndex = 42;
            this.LblOverdueBooks.Text = "Overdue Books";
            // 
            // PnlOverdueBooks
            // 
            this.PnlOverdueBooks.Controls.Add(this.Pnl1);
            this.PnlOverdueBooks.Location = new System.Drawing.Point(35, 234);
            this.PnlOverdueBooks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlOverdueBooks.Name = "PnlOverdueBooks";
            this.PnlOverdueBooks.Size = new System.Drawing.Size(1511, 551);
            this.PnlOverdueBooks.TabIndex = 46;
            // 
            // Pnl1
            // 
            this.Pnl1.Controls.Add(this.LblOverdue);
            this.Pnl1.Controls.Add(this.LblCurrentFine);
            this.Pnl1.Controls.Add(this.LblViewDetails);
            this.Pnl1.Controls.Add(this.LblDueDate);
            this.Pnl1.Controls.Add(this.LblAccesionNumber);
            this.Pnl1.Controls.Add(this.LblAuthors);
            this.Pnl1.Controls.Add(this.LblCategory);
            this.Pnl1.Controls.Add(this.PicBxBookCoverImage);
            this.Pnl1.Controls.Add(this.LblTitle);
            this.Pnl1.Location = new System.Drawing.Point(0, 0);
            this.Pnl1.Name = "Pnl1";
            this.Pnl1.Size = new System.Drawing.Size(738, 303);
            this.Pnl1.TabIndex = 50;
            // 
            // LblOverdue
            // 
            this.LblOverdue.AutoSize = true;
            this.LblOverdue.BackColor = System.Drawing.Color.Red;
            this.LblOverdue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LblOverdue.ForeColor = System.Drawing.Color.White;
            this.LblOverdue.Location = new System.Drawing.Point(357, 56);
            this.LblOverdue.Name = "LblOverdue";
            this.LblOverdue.Padding = new System.Windows.Forms.Padding(4);
            this.LblOverdue.Size = new System.Drawing.Size(96, 33);
            this.LblOverdue.TabIndex = 49;
            this.LblOverdue.Text = "Overdue";
            // 
            // LblCurrentFine
            // 
            this.LblCurrentFine.AutoSize = true;
            this.LblCurrentFine.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCurrentFine.Location = new System.Drawing.Point(264, 228);
            this.LblCurrentFine.Name = "LblCurrentFine";
            this.LblCurrentFine.Size = new System.Drawing.Size(234, 28);
            this.LblCurrentFine.TabIndex = 43;
            this.LblCurrentFine.Text = "CURRENT FINE: P 50.00";
            // 
            // LblViewDetails
            // 
            this.LblViewDetails.AutoSize = true;
            this.LblViewDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblViewDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblViewDetails.Location = new System.Drawing.Point(605, 258);
            this.LblViewDetails.Name = "LblViewDetails";
            this.LblViewDetails.Size = new System.Drawing.Size(117, 28);
            this.LblViewDetails.TabIndex = 36;
            this.LblViewDetails.Text = "View Details";
            // 
            // LblDueDate
            // 
            this.LblDueDate.AutoSize = true;
            this.LblDueDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDueDate.Location = new System.Drawing.Point(264, 199);
            this.LblDueDate.Name = "LblDueDate";
            this.LblDueDate.Size = new System.Drawing.Size(454, 28);
            this.LblDueDate.TabIndex = 35;
            this.LblDueDate.Text = "DUE DATE: January 12, 2026 (5 Days Overdue)";
            // 
            // LblAccesionNumber
            // 
            this.LblAccesionNumber.AutoSize = true;
            this.LblAccesionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LblAccesionNumber.Location = new System.Drawing.Point(264, 141);
            this.LblAccesionNumber.Name = "LblAccesionNumber";
            this.LblAccesionNumber.Size = new System.Drawing.Size(87, 25);
            this.LblAccesionNumber.TabIndex = 6;
            this.LblAccesionNumber.Text = "Acc. No:";
            // 
            // LblAuthors
            // 
            this.LblAuthors.AutoSize = true;
            this.LblAuthors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LblAuthors.Location = new System.Drawing.Point(264, 115);
            this.LblAuthors.Name = "LblAuthors";
            this.LblAuthors.Size = new System.Drawing.Size(100, 25);
            this.LblAuthors.TabIndex = 5;
            this.LblAuthors.Text = "Author(s):";
            // 
            // LblCategory
            // 
            this.LblCategory.AutoSize = true;
            this.LblCategory.BackColor = System.Drawing.Color.OrangeRed;
            this.LblCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.LblCategory.ForeColor = System.Drawing.Color.White;
            this.LblCategory.Location = new System.Drawing.Point(267, 56);
            this.LblCategory.Name = "LblCategory";
            this.LblCategory.Padding = new System.Windows.Forms.Padding(4);
            this.LblCategory.Size = new System.Drawing.Size(79, 33);
            this.LblCategory.TabIndex = 2;
            this.LblCategory.Text = "Thesis";
            // 
            // PicBxBookCoverImage
            // 
            this.PicBxBookCoverImage.Image = ((System.Drawing.Image)(resources.GetObject("PicBxBookCoverImage.Image")));
            this.PicBxBookCoverImage.Location = new System.Drawing.Point(3, 2);
            this.PicBxBookCoverImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PicBxBookCoverImage.Name = "PicBxBookCoverImage";
            this.PicBxBookCoverImage.Size = new System.Drawing.Size(226, 296);
            this.PicBxBookCoverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBxBookCoverImage.TabIndex = 0;
            this.PicBxBookCoverImage.TabStop = false;
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(259, 10);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(380, 36);
            this.LblTitle.TabIndex = 1;
            this.LblTitle.Text = "Inspirational Women Leaders...";
            // 
            // PnlforFine
            // 
            this.PnlforFine.Controls.Add(this.LblFineWarning);
            this.PnlforFine.Controls.Add(this.LblTotalOutstandingFine);
            this.PnlforFine.Location = new System.Drawing.Point(35, 94);
            this.PnlforFine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PnlforFine.Name = "PnlforFine";
            this.PnlforFine.Size = new System.Drawing.Size(614, 66);
            this.PnlforFine.TabIndex = 48;
            // 
            // LblFineWarning
            // 
            this.LblFineWarning.AutoSize = true;
            this.LblFineWarning.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFineWarning.Location = new System.Drawing.Point(16, 39);
            this.LblFineWarning.Name = "LblFineWarning";
            this.LblFineWarning.Size = new System.Drawing.Size(447, 21);
            this.LblFineWarning.TabIndex = 1;
            this.LblFineWarning.Text = "Please return these items immediately to avoid further charges.";
            // 
            // LblTotalOutstandingFine
            // 
            this.LblTotalOutstandingFine.AutoSize = true;
            this.LblTotalOutstandingFine.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTotalOutstandingFine.Location = new System.Drawing.Point(14, 8);
            this.LblTotalOutstandingFine.Name = "LblTotalOutstandingFine";
            this.LblTotalOutstandingFine.Size = new System.Drawing.Size(213, 28);
            this.LblTotalOutstandingFine.TabIndex = 0;
            this.LblTotalOutstandingFine.Text = "Total Outstanding Fine:";
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(36, 35);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 73;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 36);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 72;
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
            this.TxtSearchBar.TabIndex = 71;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // UCOverdue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.TxtSearchBar);
            this.Controls.Add(this.PnlforFine);
            this.Controls.Add(this.PnlOverdueBooks);
            this.Controls.Add(this.LblOverdueBooks);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCOverdue";
            this.Size = new System.Drawing.Size(1580, 936);
            this.PnlOverdueBooks.ResumeLayout(false);
            this.Pnl1.ResumeLayout(false);
            this.Pnl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).EndInit();
            this.PnlforFine.ResumeLayout(false);
            this.PnlforFine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblOverdueBooks;
        private System.Windows.Forms.Panel PnlOverdueBooks;
        private System.Windows.Forms.Label LblViewDetails;
        private System.Windows.Forms.Label LblDueDate;
        private System.Windows.Forms.Label LblAccesionNumber;
        private System.Windows.Forms.Label LblAuthors;
        private System.Windows.Forms.Label LblCategory;
        private System.Windows.Forms.PictureBox PicBxBookCoverImage;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Panel PnlforFine;
        private System.Windows.Forms.Label LblTotalOutstandingFine;
        private System.Windows.Forms.Label LblCurrentFine;
        private System.Windows.Forms.Label LblFineWarning;
        private System.Windows.Forms.Label LblOverdue;
        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.Label LblSearch;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.Panel Pnl1;
    }
}
