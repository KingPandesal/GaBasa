namespace LMS.Presentation.Popup.Multipurpose
{
    partial class Camera
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
            this.PnlDesign = new System.Windows.Forms.Panel();
            this.VideoPlayer = new AForge.Controls.VideoSourcePlayer();
            this.PnlVideoPlayerContainer = new System.Windows.Forms.Panel();
            this.CmbBxDevices = new System.Windows.Forms.ComboBox();
            this.LblResult = new System.Windows.Forms.Label();
            this.PnlVideoPlayerContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlDesign
            // 
            this.PnlDesign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.PnlDesign.ForeColor = System.Drawing.Color.White;
            this.PnlDesign.Location = new System.Drawing.Point(0, 0);
            this.PnlDesign.Name = "PnlDesign";
            this.PnlDesign.Size = new System.Drawing.Size(603, 85);
            this.PnlDesign.TabIndex = 0;
            // 
            // VideoPlayer
            // 
            this.VideoPlayer.Location = new System.Drawing.Point(27, 24);
            this.VideoPlayer.Name = "VideoPlayer";
            this.VideoPlayer.Size = new System.Drawing.Size(457, 224);
            this.VideoPlayer.TabIndex = 1;
            this.VideoPlayer.Text = "videoSourcePlayer1";
            this.VideoPlayer.VideoSource = null;
            // 
            // PnlVideoPlayerContainer
            // 
            this.PnlVideoPlayerContainer.Controls.Add(this.VideoPlayer);
            this.PnlVideoPlayerContainer.Controls.Add(this.LblResult);
            this.PnlVideoPlayerContainer.Location = new System.Drawing.Point(41, 36);
            this.PnlVideoPlayerContainer.Name = "PnlVideoPlayerContainer";
            this.PnlVideoPlayerContainer.Size = new System.Drawing.Size(513, 275);
            this.PnlVideoPlayerContainer.TabIndex = 2;
            // 
            // CmbBxDevices
            // 
            this.CmbBxDevices.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBxDevices.FormattingEnabled = true;
            this.CmbBxDevices.Location = new System.Drawing.Point(68, 344);
            this.CmbBxDevices.Name = "CmbBxDevices";
            this.CmbBxDevices.Size = new System.Drawing.Size(457, 36);
            this.CmbBxDevices.TabIndex = 3;
            // 
            // LblResult
            // 
            this.LblResult.AutoSize = true;
            this.LblResult.Location = new System.Drawing.Point(223, 251);
            this.LblResult.Name = "LblResult";
            this.LblResult.Size = new System.Drawing.Size(69, 20);
            this.LblResult.TabIndex = 4;
            this.LblResult.Text = "Barcode";
            // 
            // Camera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 406);
            this.Controls.Add(this.CmbBxDevices);
            this.Controls.Add(this.PnlVideoPlayerContainer);
            this.Controls.Add(this.PnlDesign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Camera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera";
            this.PnlVideoPlayerContainer.ResumeLayout(false);
            this.PnlVideoPlayerContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlDesign;
        private AForge.Controls.VideoSourcePlayer VideoPlayer;
        private System.Windows.Forms.Panel PnlVideoPlayerContainer;
        private System.Windows.Forms.ComboBox CmbBxDevices;
        private System.Windows.Forms.Label LblResult;
    }
}