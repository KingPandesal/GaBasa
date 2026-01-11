namespace LMS.Presentation.UserControls.MemberFeatures
{
    partial class UCBorrowed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBorrowed));
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.PnlOverdueBooks = new System.Windows.Forms.Panel();
            this.Pnl1 = new System.Windows.Forms.Panel();
            this.LblOverdue = new System.Windows.Forms.Label();
            this.LblViewDetails = new System.Windows.Forms.Label();
            this.LblDueDate = new System.Windows.Forms.Label();
            this.LblAccesionNumber = new System.Windows.Forms.Label();
            this.LblResourceType = new System.Windows.Forms.Label();
            this.LblCategory = new System.Windows.Forms.Label();
            this.PicBxBookCoverImage = new System.Windows.Forms.PictureBox();
            this.LblTitle = new System.Windows.Forms.Label();
            this.LblOverdueBooks = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.PnlOverdueBooks.SuspendLayout();
            this.Pnl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).BeginInit();
            this.SuspendLayout();
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(32, 28);
            this.PicBxSearchIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(29, 27);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 76;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(67, 29);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(61, 23);
            this.LblSearch.TabIndex = 75;
            this.LblSearch.Text = "Search";
            // 
            // TxtSearchBar
            // 
            this.TxtSearchBar.BackColor = System.Drawing.Color.White;
            this.TxtSearchBar.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            this.TxtSearchBar.ForeColor = System.Drawing.Color.DimGray;
            this.TxtSearchBar.Image = null;
            this.TxtSearchBar.Location = new System.Drawing.Point(138, 26);
            this.TxtSearchBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtSearchBar.MaximumSize = new System.Drawing.Size(1333, 32);
            this.TxtSearchBar.MaxLength = 32767;
            this.TxtSearchBar.MinimumSize = new System.Drawing.Size(0, 26);
            this.TxtSearchBar.Multiline = false;
            this.TxtSearchBar.Name = "TxtSearchBar";
            this.TxtSearchBar.ReadOnly = false;
            this.TxtSearchBar.Size = new System.Drawing.Size(1236, 32);
            this.TxtSearchBar.TabIndex = 74;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // PnlOverdueBooks
            // 
            this.PnlOverdueBooks.Controls.Add(this.Pnl1);
            this.PnlOverdueBooks.Location = new System.Drawing.Point(28, 130);
            this.PnlOverdueBooks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlOverdueBooks.Name = "PnlOverdueBooks";
            this.PnlOverdueBooks.Size = new System.Drawing.Size(1343, 498);
            this.PnlOverdueBooks.TabIndex = 78;
            // 
            // Pnl1
            // 
            this.Pnl1.Controls.Add(this.LblOverdue);
            this.Pnl1.Controls.Add(this.LblViewDetails);
            this.Pnl1.Controls.Add(this.LblDueDate);
            this.Pnl1.Controls.Add(this.LblAccesionNumber);
            this.Pnl1.Controls.Add(this.LblResourceType);
            this.Pnl1.Controls.Add(this.LblCategory);
            this.Pnl1.Controls.Add(this.PicBxBookCoverImage);
            this.Pnl1.Controls.Add(this.LblTitle);
            this.Pnl1.Location = new System.Drawing.Point(0, 0);
            this.Pnl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Pnl1.Name = "Pnl1";
            this.Pnl1.Size = new System.Drawing.Size(656, 242);
            this.Pnl1.TabIndex = 50;
            // 
            // LblOverdue
            // 
            this.LblOverdue.AutoSize = true;
            this.LblOverdue.BackColor = System.Drawing.Color.Red;
            this.LblOverdue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOverdue.ForeColor = System.Drawing.Color.White;
            this.LblOverdue.Location = new System.Drawing.Point(317, 45);
            this.LblOverdue.Name = "LblOverdue";
            this.LblOverdue.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LblOverdue.Size = new System.Drawing.Size(83, 29);
            this.LblOverdue.TabIndex = 49;
            this.LblOverdue.Text = "Overdue";
            // 
            // LblViewDetails
            // 
            this.LblViewDetails.AutoSize = true;
            this.LblViewDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblViewDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblViewDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblViewDetails.Location = new System.Drawing.Point(538, 206);
            this.LblViewDetails.Name = "LblViewDetails";
            this.LblViewDetails.Size = new System.Drawing.Size(102, 23);
            this.LblViewDetails.TabIndex = 36;
            this.LblViewDetails.Text = "View Details";
            // 
            // LblDueDate
            // 
            this.LblDueDate.AutoSize = true;
            this.LblDueDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDueDate.Location = new System.Drawing.Point(235, 159);
            this.LblDueDate.Name = "LblDueDate";
            this.LblDueDate.Size = new System.Drawing.Size(240, 23);
            this.LblDueDate.TabIndex = 35;
            this.LblDueDate.Text = "DUE DATE: January 12, 2026";
            // 
            // LblAccesionNumber
            // 
            this.LblAccesionNumber.AutoSize = true;
            this.LblAccesionNumber.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAccesionNumber.Location = new System.Drawing.Point(235, 113);
            this.LblAccesionNumber.Name = "LblAccesionNumber";
            this.LblAccesionNumber.Size = new System.Drawing.Size(73, 23);
            this.LblAccesionNumber.TabIndex = 6;
            this.LblAccesionNumber.Text = "Acc. No:";
            // 
            // LblResourceType
            // 
            this.LblResourceType.AutoSize = true;
            this.LblResourceType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblResourceType.Location = new System.Drawing.Point(235, 92);
            this.LblResourceType.Name = "LblResourceType";
            this.LblResourceType.Size = new System.Drawing.Size(123, 23);
            this.LblResourceType.TabIndex = 5;
            this.LblResourceType.Text = "Resource Type:";
            // 
            // LblCategory
            // 
            this.LblCategory.AutoSize = true;
            this.LblCategory.BackColor = System.Drawing.Color.OrangeRed;
            this.LblCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCategory.ForeColor = System.Drawing.Color.White;
            this.LblCategory.Location = new System.Drawing.Point(237, 45);
            this.LblCategory.Name = "LblCategory";
            this.LblCategory.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LblCategory.Size = new System.Drawing.Size(64, 29);
            this.LblCategory.TabIndex = 2;
            this.LblCategory.Text = "Thesis";
            // 
            // PicBxBookCoverImage
            // 
            this.PicBxBookCoverImage.Image = ((System.Drawing.Image)(resources.GetObject("PicBxBookCoverImage.Image")));
            this.PicBxBookCoverImage.Location = new System.Drawing.Point(3, 2);
            this.PicBxBookCoverImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PicBxBookCoverImage.Name = "PicBxBookCoverImage";
            this.PicBxBookCoverImage.Size = new System.Drawing.Size(201, 237);
            this.PicBxBookCoverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBxBookCoverImage.TabIndex = 0;
            this.PicBxBookCoverImage.TabStop = false;
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(230, 8);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(316, 30);
            this.LblTitle.TabIndex = 1;
            this.LblTitle.Text = "Inspirational Women Leaders...";
            // 
            // LblOverdueBooks
            // 
            this.LblOverdueBooks.AutoSize = true;
            this.LblOverdueBooks.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOverdueBooks.Location = new System.Drawing.Point(27, 82);
            this.LblOverdueBooks.Name = "LblOverdueBooks";
            this.LblOverdueBooks.Size = new System.Drawing.Size(176, 30);
            this.LblOverdueBooks.TabIndex = 77;
            this.LblOverdueBooks.Text = "Borrowed Books";
            // 
            // UCBorrowed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.PnlOverdueBooks);
            this.Controls.Add(this.LblOverdueBooks);
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.TxtSearchBar);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCBorrowed";
            this.Size = new System.Drawing.Size(1404, 749);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.PnlOverdueBooks.ResumeLayout(false);
            this.Pnl1.ResumeLayout(false);
            this.Pnl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.Label LblSearch;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.Panel PnlOverdueBooks;
        private System.Windows.Forms.Panel Pnl1;
        private System.Windows.Forms.Label LblOverdue;
        private System.Windows.Forms.Label LblViewDetails;
        private System.Windows.Forms.Label LblDueDate;
        private System.Windows.Forms.Label LblAccesionNumber;
        private System.Windows.Forms.Label LblResourceType;
        private System.Windows.Forms.Label LblCategory;
        private System.Windows.Forms.PictureBox PicBxBookCoverImage;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Label LblOverdueBooks;
    }
}
