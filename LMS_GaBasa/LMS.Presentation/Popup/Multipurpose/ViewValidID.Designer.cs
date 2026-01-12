namespace LMS.Presentation.Popup.Multipurpose
{
    partial class ViewValidID
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
            this.PnlforPicBxValidIDContainer = new System.Windows.Forms.Panel();
            this.PicBxValidID = new System.Windows.Forms.PictureBox();
            this.PnlDesignOnly = new System.Windows.Forms.Panel();
            this.PnlforPicBxValidIDContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxValidID)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlforPicBxValidIDContainer
            // 
            this.PnlforPicBxValidIDContainer.BackColor = System.Drawing.Color.White;
            this.PnlforPicBxValidIDContainer.Controls.Add(this.PicBxValidID);
            this.PnlforPicBxValidIDContainer.Location = new System.Drawing.Point(53, 47);
            this.PnlforPicBxValidIDContainer.Name = "PnlforPicBxValidIDContainer";
            this.PnlforPicBxValidIDContainer.Size = new System.Drawing.Size(517, 337);
            this.PnlforPicBxValidIDContainer.TabIndex = 8;
            this.PnlforPicBxValidIDContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlforPicBxBarcodeImageContainer_Paint);
            // 
            // PicBxValidID
            // 
            this.PicBxValidID.Location = new System.Drawing.Point(35, 32);
            this.PicBxValidID.Name = "PicBxValidID";
            this.PicBxValidID.Size = new System.Drawing.Size(449, 273);
            this.PicBxValidID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxValidID.TabIndex = 0;
            this.PicBxValidID.TabStop = false;
            // 
            // PnlDesignOnly
            // 
            this.PnlDesignOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.PnlDesignOnly.Location = new System.Drawing.Point(0, 0);
            this.PnlDesignOnly.Name = "PnlDesignOnly";
            this.PnlDesignOnly.Size = new System.Drawing.Size(624, 141);
            this.PnlDesignOnly.TabIndex = 7;
            this.PnlDesignOnly.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlDesignOnly_Paint);
            // 
            // ViewValidID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 413);
            this.Controls.Add(this.PnlforPicBxValidIDContainer);
            this.Controls.Add(this.PnlDesignOnly);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewValidID";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View ValidID";
            this.PnlforPicBxValidIDContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxValidID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlforPicBxValidIDContainer;
        private System.Windows.Forms.PictureBox PicBxValidID;
        private System.Windows.Forms.Panel PnlDesignOnly;
    }
}