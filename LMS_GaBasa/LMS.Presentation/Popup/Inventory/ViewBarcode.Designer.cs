namespace LMS.Presentation.Popup.Inventory
{
    partial class ViewBarcode
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
            this.PnlforPicBxBarcodeImageContainer = new System.Windows.Forms.Panel();
            this.PicBxBarcodeImage = new System.Windows.Forms.PictureBox();
            this.PnlDesignOnly = new System.Windows.Forms.Panel();
            this.LblBarcode = new System.Windows.Forms.Label();
            this.PnlforPicBxBarcodeImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBarcodeImage)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlforPicBxBarcodeImageContainer
            // 
            this.PnlforPicBxBarcodeImageContainer.BackColor = System.Drawing.Color.White;
            this.PnlforPicBxBarcodeImageContainer.Controls.Add(this.PicBxBarcodeImage);
            this.PnlforPicBxBarcodeImageContainer.Location = new System.Drawing.Point(72, 51);
            this.PnlforPicBxBarcodeImageContainer.Name = "PnlforPicBxBarcodeImageContainer";
            this.PnlforPicBxBarcodeImageContainer.Size = new System.Drawing.Size(639, 318);
            this.PnlforPicBxBarcodeImageContainer.TabIndex = 5;
            // 
            // PicBxBarcodeImage
            // 
            this.PicBxBarcodeImage.Location = new System.Drawing.Point(23, 24);
            this.PicBxBarcodeImage.Name = "PicBxBarcodeImage";
            this.PicBxBarcodeImage.Size = new System.Drawing.Size(594, 273);
            this.PicBxBarcodeImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxBarcodeImage.TabIndex = 0;
            this.PicBxBarcodeImage.TabStop = false;
            // 
            // PnlDesignOnly
            // 
            this.PnlDesignOnly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.PnlDesignOnly.Location = new System.Drawing.Point(0, 0);
            this.PnlDesignOnly.Name = "PnlDesignOnly";
            this.PnlDesignOnly.Size = new System.Drawing.Size(800, 141);
            this.PnlDesignOnly.TabIndex = 4;
            // 
            // LblBarcode
            // 
            this.LblBarcode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LblBarcode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBarcode.Location = new System.Drawing.Point(0, 385);
            this.LblBarcode.Name = "LblBarcode";
            this.LblBarcode.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.LblBarcode.Size = new System.Drawing.Size(800, 53);
            this.LblBarcode.TabIndex = 6;
            this.LblBarcode.Text = "Barcode";
            this.LblBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ViewBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 438);
            this.Controls.Add(this.LblBarcode);
            this.Controls.Add(this.PnlforPicBxBarcodeImageContainer);
            this.Controls.Add(this.PnlDesignOnly);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewBarcode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View barcode";
            this.PnlforPicBxBarcodeImageContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBxBarcodeImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlforPicBxBarcodeImageContainer;
        private System.Windows.Forms.PictureBox PicBxBarcodeImage;
        private System.Windows.Forms.Panel PnlDesignOnly;
        private System.Windows.Forms.Label LblBarcode;
    }
}