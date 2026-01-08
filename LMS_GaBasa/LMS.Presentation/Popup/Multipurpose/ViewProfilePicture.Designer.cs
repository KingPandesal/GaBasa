namespace LMS.Presentation.Popup.Multipurpose
{
    partial class ViewProfilePicture
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
            this.PnlforPicBxProfilePictureContainer = new System.Windows.Forms.Panel();
            this.PicBxProfilePicture = new System.Windows.Forms.PictureBox();
            this.PnlDesignOnly = new System.Windows.Forms.Panel();
            this.PnlforPicBxProfilePictureContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlforPicBxProfilePictureContainer
            // 
            this.PnlforPicBxProfilePictureContainer.BackColor = System.Drawing.Color.White;
            this.PnlforPicBxProfilePictureContainer.Controls.Add(this.PicBxProfilePicture);
            this.PnlforPicBxProfilePictureContainer.Location = new System.Drawing.Point(49, 41);
            this.PnlforPicBxProfilePictureContainer.Name = "PnlforPicBxProfilePictureContainer";
            this.PnlforPicBxProfilePictureContainer.Size = new System.Drawing.Size(329, 440);
            this.PnlforPicBxProfilePictureContainer.TabIndex = 5;
            // 
            // PicBxProfilePicture
            // 
            this.PicBxProfilePicture.Location = new System.Drawing.Point(23, 24);
            this.PicBxProfilePicture.Name = "PicBxProfilePicture";
            this.PicBxProfilePicture.Size = new System.Drawing.Size(287, 388);
            this.PicBxProfilePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxProfilePicture.TabIndex = 0;
            this.PicBxProfilePicture.TabStop = false;
            // 
            // PnlDesignOnly
            // 
            this.PnlDesignOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.PnlDesignOnly.Location = new System.Drawing.Point(0, 0);
            this.PnlDesignOnly.Name = "PnlDesignOnly";
            this.PnlDesignOnly.Size = new System.Drawing.Size(431, 118);
            this.PnlDesignOnly.TabIndex = 4;
            // 
            // ViewProfilePicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 504);
            this.Controls.Add(this.PnlforPicBxProfilePictureContainer);
            this.Controls.Add(this.PnlDesignOnly);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewProfilePicture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View profile picture";
            this.PnlforPicBxProfilePictureContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlforPicBxProfilePictureContainer;
        private System.Windows.Forms.PictureBox PicBxProfilePicture;
        private System.Windows.Forms.Panel PnlDesignOnly;
    }
}