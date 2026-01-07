namespace LMS.Presentation.Popup.Catalog
{
    partial class ViewBookDownloadLink
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
            this.components = new System.ComponentModel.Container();
            this.LblDownloadURL = new System.Windows.Forms.Label();
            this.TxtDownloadURL = new System.Windows.Forms.TextBox();
            this.BtnCopyDownloadURL = new System.Windows.Forms.Button();
            this.TipforBtnCopyDownloadURL = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // LblDownloadURL
            // 
            this.LblDownloadURL.AutoSize = true;
            this.LblDownloadURL.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblDownloadURL.Location = new System.Drawing.Point(22, 20);
            this.LblDownloadURL.Name = "LblDownloadURL";
            this.LblDownloadURL.Size = new System.Drawing.Size(142, 28);
            this.LblDownloadURL.TabIndex = 0;
            this.LblDownloadURL.Text = "Download URL";
            this.LblDownloadURL.Click += new System.EventHandler(this.label1_Click);
            // 
            // TxtDownloadURL
            // 
            this.TxtDownloadURL.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TxtDownloadURL.Location = new System.Drawing.Point(27, 52);
            this.TxtDownloadURL.Name = "TxtDownloadURL";
            this.TxtDownloadURL.Size = new System.Drawing.Size(307, 34);
            this.TxtDownloadURL.TabIndex = 1;
            // 
            // BtnCopyDownloadURL
            // 
            this.BtnCopyDownloadURL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnCopyDownloadURL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCopyDownloadURL.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnCopyDownloadURL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCopyDownloadURL.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnCopyDownloadURL.ForeColor = System.Drawing.Color.White;
            this.BtnCopyDownloadURL.Location = new System.Drawing.Point(353, 46);
            this.BtnCopyDownloadURL.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCopyDownloadURL.Name = "BtnCopyDownloadURL";
            this.BtnCopyDownloadURL.Size = new System.Drawing.Size(61, 45);
            this.BtnCopyDownloadURL.TabIndex = 78;
            this.BtnCopyDownloadURL.Text = "📋";
            this.TipforBtnCopyDownloadURL.SetToolTip(this.BtnCopyDownloadURL, "Copy link");
            this.BtnCopyDownloadURL.UseVisualStyleBackColor = false;
            // 
            // ViewBookDownloadLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(441, 109);
            this.Controls.Add(this.BtnCopyDownloadURL);
            this.Controls.Add(this.TxtDownloadURL);
            this.Controls.Add(this.LblDownloadURL);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewBookDownloadLink";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Book download link";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblDownloadURL;
        private System.Windows.Forms.TextBox TxtDownloadURL;
        private System.Windows.Forms.Button BtnCopyDownloadURL;
        private System.Windows.Forms.ToolTip TipforBtnCopyDownloadURL;
    }
}