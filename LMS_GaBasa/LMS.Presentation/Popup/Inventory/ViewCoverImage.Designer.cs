namespace LMS.Presentation.Popup.Inventory
{
    partial class ViewCoverImage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnlDesignOnly = new System.Windows.Forms.Panel();
            this.PnlforPicBxCoverImageContainer = new System.Windows.Forms.Panel();
            this.PicBxCoverImage = new System.Windows.Forms.PictureBox();
            this.LblTitle = new System.Windows.Forms.Label();
            this.PnlforPicBxCoverImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxCoverImage)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlDesignOnly
            // 
            this.PnlDesignOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.PnlDesignOnly.Location = new System.Drawing.Point(0, 0);
            this.PnlDesignOnly.Name = "PnlDesignOnly";
            this.PnlDesignOnly.Size = new System.Drawing.Size(431, 118);
            this.PnlDesignOnly.TabIndex = 0;
            // 
            // PnlforPicBxCoverImageContainer
            // 
            this.PnlforPicBxCoverImageContainer.BackColor = System.Drawing.Color.White;
            this.PnlforPicBxCoverImageContainer.Controls.Add(this.PicBxCoverImage);
            this.PnlforPicBxCoverImageContainer.Location = new System.Drawing.Point(49, 41);
            this.PnlforPicBxCoverImageContainer.Name = "PnlforPicBxCoverImageContainer";
            this.PnlforPicBxCoverImageContainer.Size = new System.Drawing.Size(329, 440);
            this.PnlforPicBxCoverImageContainer.TabIndex = 2;
            // 
            // PicBxCoverImage
            // 
            this.PicBxCoverImage.Location = new System.Drawing.Point(23, 24);
            this.PicBxCoverImage.Name = "PicBxCoverImage";
            this.PicBxCoverImage.Size = new System.Drawing.Size(287, 388);
            this.PicBxCoverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxCoverImage.TabIndex = 0;
            this.PicBxCoverImage.TabStop = false;
            // 
            // LblTitle
            // 
            this.LblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(0, 499);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.LblTitle.Size = new System.Drawing.Size(430, 53);
            this.LblTitle.TabIndex = 3;
            this.LblTitle.Text = "Title";
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ViewCoverImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 552);
            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.PnlforPicBxCoverImageContainer);
            this.Controls.Add(this.PnlDesignOnly);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewCoverImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View cover image";
            this.PnlforPicBxCoverImageContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxCoverImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlDesignOnly;
        private System.Windows.Forms.Panel PnlforPicBxCoverImageContainer;
        private System.Windows.Forms.PictureBox PicBxCoverImage;
        private System.Windows.Forms.Label LblTitle;
    }
}