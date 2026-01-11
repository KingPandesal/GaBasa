namespace LMS.Presentation.UserControls.MemberFeatures
{
    partial class UCReserve
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCReserve));
            this.PnlOverdueBooks = new System.Windows.Forms.Panel();
            this.Pnl1 = new System.Windows.Forms.Panel();
            this.LblOverdue = new System.Windows.Forms.Label();
            this.LblViewDetails = new System.Windows.Forms.Label();
            this.LblAccesionNumber = new System.Windows.Forms.Label();
            this.LblResourceType = new System.Windows.Forms.Label();
            this.LblCategory = new System.Windows.Forms.Label();
            this.PicBxBookCoverImage = new System.Windows.Forms.PictureBox();
            this.LblTitle = new System.Windows.Forms.Label();
            this.PicBxSearchIcon = new System.Windows.Forms.PictureBox();
            this.LblSearch = new System.Windows.Forms.Label();
            this.TxtSearchBar = new ReaLTaiizor.Controls.BigTextBox();
            this.LblOverdueBooks = new System.Windows.Forms.Label();
            this.LblExpirationDate = new System.Windows.Forms.Label();
            this.LblReservationDate = new System.Windows.Forms.Label();
            this.PnlOverdueBooks.SuspendLayout();
            this.Pnl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlOverdueBooks
            // 
            this.PnlOverdueBooks.Controls.Add(this.Pnl1);
            this.PnlOverdueBooks.Location = new System.Drawing.Point(31, 162);
            this.PnlOverdueBooks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlOverdueBooks.Name = "PnlOverdueBooks";
            this.PnlOverdueBooks.Size = new System.Drawing.Size(1511, 622);
            this.PnlOverdueBooks.TabIndex = 83;
            // 
            // Pnl1
            // 
            this.Pnl1.Controls.Add(this.LblExpirationDate);
            this.Pnl1.Controls.Add(this.LblReservationDate);
            this.Pnl1.Controls.Add(this.LblOverdue);
            this.Pnl1.Controls.Add(this.LblViewDetails);
            this.Pnl1.Controls.Add(this.LblAccesionNumber);
            this.Pnl1.Controls.Add(this.LblResourceType);
            this.Pnl1.Controls.Add(this.LblCategory);
            this.Pnl1.Controls.Add(this.PicBxBookCoverImage);
            this.Pnl1.Controls.Add(this.LblTitle);
            this.Pnl1.Location = new System.Drawing.Point(7, 7);
            this.Pnl1.Name = "Pnl1";
            this.Pnl1.Size = new System.Drawing.Size(738, 303);
            this.Pnl1.TabIndex = 50;
            // 
            // LblOverdue
            // 
            this.LblOverdue.AutoSize = true;
            this.LblOverdue.BackColor = System.Drawing.Color.Red;
            this.LblOverdue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOverdue.ForeColor = System.Drawing.Color.White;
            this.LblOverdue.Location = new System.Drawing.Point(351, 49);
            this.LblOverdue.Name = "LblOverdue";
            this.LblOverdue.Padding = new System.Windows.Forms.Padding(4);
            this.LblOverdue.Size = new System.Drawing.Size(95, 36);
            this.LblOverdue.TabIndex = 49;
            this.LblOverdue.Text = "Overdue";
            // 
            // LblViewDetails
            // 
            this.LblViewDetails.AutoSize = true;
            this.LblViewDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LblViewDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblViewDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblViewDetails.Location = new System.Drawing.Point(599, 251);
            this.LblViewDetails.Name = "LblViewDetails";
            this.LblViewDetails.Size = new System.Drawing.Size(117, 28);
            this.LblViewDetails.TabIndex = 36;
            this.LblViewDetails.Text = "View Details";
            // 
            // LblAccesionNumber
            // 
            this.LblAccesionNumber.AutoSize = true;
            this.LblAccesionNumber.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAccesionNumber.Location = new System.Drawing.Point(258, 134);
            this.LblAccesionNumber.Name = "LblAccesionNumber";
            this.LblAccesionNumber.Size = new System.Drawing.Size(83, 28);
            this.LblAccesionNumber.TabIndex = 6;
            this.LblAccesionNumber.Text = "Acc. No:";
            // 
            // LblResourceType
            // 
            this.LblResourceType.AutoSize = true;
            this.LblResourceType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblResourceType.Location = new System.Drawing.Point(258, 108);
            this.LblResourceType.Name = "LblResourceType";
            this.LblResourceType.Size = new System.Drawing.Size(140, 28);
            this.LblResourceType.TabIndex = 5;
            this.LblResourceType.Text = "Resource Type:";
            // 
            // LblCategory
            // 
            this.LblCategory.AutoSize = true;
            this.LblCategory.BackColor = System.Drawing.Color.OrangeRed;
            this.LblCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCategory.ForeColor = System.Drawing.Color.White;
            this.LblCategory.Location = new System.Drawing.Point(261, 49);
            this.LblCategory.Name = "LblCategory";
            this.LblCategory.Padding = new System.Windows.Forms.Padding(4);
            this.LblCategory.Size = new System.Drawing.Size(72, 36);
            this.LblCategory.TabIndex = 2;
            this.LblCategory.Text = "Thesis";
            // 
            // PicBxBookCoverImage
            // 
            this.PicBxBookCoverImage.Image = ((System.Drawing.Image)(resources.GetObject("PicBxBookCoverImage.Image")));
            this.PicBxBookCoverImage.Location = new System.Drawing.Point(-3, -5);
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
            this.LblTitle.Location = new System.Drawing.Point(253, 3);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(380, 36);
            this.LblTitle.TabIndex = 1;
            this.LblTitle.Text = "Inspirational Women Leaders...";
            // 
            // PicBxSearchIcon
            // 
            this.PicBxSearchIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxSearchIcon.Image")));
            this.PicBxSearchIcon.Location = new System.Drawing.Point(36, 35);
            this.PicBxSearchIcon.Name = "PicBxSearchIcon";
            this.PicBxSearchIcon.Size = new System.Drawing.Size(33, 34);
            this.PicBxSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxSearchIcon.TabIndex = 81;
            this.PicBxSearchIcon.TabStop = false;
            // 
            // LblSearch
            // 
            this.LblSearch.AutoSize = true;
            this.LblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSearch.Location = new System.Drawing.Point(75, 36);
            this.LblSearch.Name = "LblSearch";
            this.LblSearch.Size = new System.Drawing.Size(70, 28);
            this.LblSearch.TabIndex = 80;
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
            this.TxtSearchBar.TabIndex = 79;
            this.TxtSearchBar.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtSearchBar.UseSystemPasswordChar = false;
            // 
            // LblOverdueBooks
            // 
            this.LblOverdueBooks.AutoSize = true;
            this.LblOverdueBooks.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOverdueBooks.Location = new System.Drawing.Point(30, 103);
            this.LblOverdueBooks.Name = "LblOverdueBooks";
            this.LblOverdueBooks.Size = new System.Drawing.Size(213, 36);
            this.LblOverdueBooks.TabIndex = 82;
            this.LblOverdueBooks.Text = "Borrowed Books";
            // 
            // LblExpirationDate
            // 
            this.LblExpirationDate.AutoSize = true;
            this.LblExpirationDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblExpirationDate.Location = new System.Drawing.Point(254, 215);
            this.LblExpirationDate.Name = "LblExpirationDate";
            this.LblExpirationDate.Size = new System.Drawing.Size(367, 28);
            this.LblExpirationDate.TabIndex = 51;
            this.LblExpirationDate.Text = "EXPIRATION DATE: January 15, 2026 ";
            // 
            // LblReservationDate
            // 
            this.LblReservationDate.AutoSize = true;
            this.LblReservationDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblReservationDate.Location = new System.Drawing.Point(254, 186);
            this.LblReservationDate.Name = "LblReservationDate";
            this.LblReservationDate.Size = new System.Drawing.Size(383, 28);
            this.LblReservationDate.TabIndex = 50;
            this.LblReservationDate.Text = "RESERVATION DATE: January 12, 2026 ";
            // 
            // UCReserve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.PnlOverdueBooks);
            this.Controls.Add(this.PicBxSearchIcon);
            this.Controls.Add(this.LblSearch);
            this.Controls.Add(this.TxtSearchBar);
            this.Controls.Add(this.LblOverdueBooks);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCReserve";
            this.Size = new System.Drawing.Size(1580, 936);
            this.PnlOverdueBooks.ResumeLayout(false);
            this.Pnl1.ResumeLayout(false);
            this.Pnl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBookCoverImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxSearchIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PnlOverdueBooks;
        private System.Windows.Forms.Panel Pnl1;
        private System.Windows.Forms.Label LblOverdue;
        private System.Windows.Forms.Label LblViewDetails;
        private System.Windows.Forms.Label LblAccesionNumber;
        private System.Windows.Forms.Label LblResourceType;
        private System.Windows.Forms.Label LblCategory;
        private System.Windows.Forms.PictureBox PicBxBookCoverImage;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.PictureBox PicBxSearchIcon;
        private System.Windows.Forms.Label LblSearch;
        private ReaLTaiizor.Controls.BigTextBox TxtSearchBar;
        private System.Windows.Forms.Label LblOverdueBooks;
        private System.Windows.Forms.Label LblExpirationDate;
        private System.Windows.Forms.Label LblReservationDate;
    }
}
