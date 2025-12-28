namespace LMS.Presentation.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.PnlSidebar = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.LblProfileName = new System.Windows.Forms.Label();
            this.LblProfileRole = new System.Windows.Forms.Label();
            this.PnlProfileHeader = new System.Windows.Forms.Panel();
            this.PicBxProfilePic = new System.Windows.Forms.PictureBox();
            this.PnlLogo = new System.Windows.Forms.Panel();
            this.PicBxLogo = new System.Windows.Forms.PictureBox();
            this.LblTitle = new System.Windows.Forms.Label();
            this.PnlModuleName = new System.Windows.Forms.Panel();
            this.PicBxModuleIcon = new System.Windows.Forms.PictureBox();
            this.LblModuleName = new System.Windows.Forms.Label();
            this.PnlTopBar = new ReaLTaiizor.Controls.LostBorderPanel();
            this.PnlContent = new System.Windows.Forms.Panel();
            this.PnlSidebar.SuspendLayout();
            this.PnlProfileHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).BeginInit();
            this.PnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxLogo)).BeginInit();
            this.PnlModuleName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxModuleIcon)).BeginInit();
            this.PnlTopBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlSidebar
            // 
            this.PnlSidebar.Controls.Add(this.button2);
            this.PnlSidebar.Location = new System.Drawing.Point(12, 189);
            this.PnlSidebar.Name = "PnlSidebar";
            this.PnlSidebar.Size = new System.Drawing.Size(273, 785);
            this.PnlSidebar.TabIndex = 27;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(77, 193);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 133);
            this.button2.TabIndex = 18;
            this.button2.Text = "Sidebar (modules appear here";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // LblProfileName
            // 
            this.LblProfileName.AutoSize = true;
            this.LblProfileName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblProfileName.ForeColor = System.Drawing.Color.Black;
            this.LblProfileName.Location = new System.Drawing.Point(92, 16);
            this.LblProfileName.Name = "LblProfileName";
            this.LblProfileName.Size = new System.Drawing.Size(81, 32);
            this.LblProfileName.TabIndex = 0;
            this.LblProfileName.Text = "Name";
            // 
            // LblProfileRole
            // 
            this.LblProfileRole.AutoSize = true;
            this.LblProfileRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblProfileRole.ForeColor = System.Drawing.Color.Black;
            this.LblProfileRole.Location = new System.Drawing.Point(93, 43);
            this.LblProfileRole.Name = "LblProfileRole";
            this.LblProfileRole.Size = new System.Drawing.Size(50, 28);
            this.LblProfileRole.TabIndex = 1;
            this.LblProfileRole.Text = "Role";
            // 
            // PnlProfileHeader
            // 
            this.PnlProfileHeader.BackColor = System.Drawing.Color.Transparent;
            this.PnlProfileHeader.Controls.Add(this.PicBxProfilePic);
            this.PnlProfileHeader.Controls.Add(this.LblProfileName);
            this.PnlProfileHeader.Controls.Add(this.LblProfileRole);
            this.PnlProfileHeader.Location = new System.Drawing.Point(1232, 3);
            this.PnlProfileHeader.Name = "PnlProfileHeader";
            this.PnlProfileHeader.Size = new System.Drawing.Size(345, 83);
            this.PnlProfileHeader.TabIndex = 0;
            // 
            // PicBxProfilePic
            // 
            this.PicBxProfilePic.BackColor = System.Drawing.Color.Transparent;
            this.PicBxProfilePic.Location = new System.Drawing.Point(18, 16);
            this.PicBxProfilePic.Name = "PicBxProfilePic";
            this.PicBxProfilePic.Size = new System.Drawing.Size(64, 57);
            this.PicBxProfilePic.TabIndex = 2;
            this.PicBxProfilePic.TabStop = false;
            // 
            // PnlLogo
            // 
            this.PnlLogo.Controls.Add(this.PicBxLogo);
            this.PnlLogo.Controls.Add(this.LblTitle);
            this.PnlLogo.Location = new System.Drawing.Point(2, 53);
            this.PnlLogo.Name = "PnlLogo";
            this.PnlLogo.Size = new System.Drawing.Size(283, 91);
            this.PnlLogo.TabIndex = 28;
            // 
            // PicBxLogo
            // 
            this.PicBxLogo.Image = ((System.Drawing.Image)(resources.GetObject("PicBxLogo.Image")));
            this.PicBxLogo.Location = new System.Drawing.Point(33, 12);
            this.PicBxLogo.Name = "PicBxLogo";
            this.PicBxLogo.Size = new System.Drawing.Size(67, 68);
            this.PicBxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxLogo.TabIndex = 1;
            this.PicBxLogo.TabStop = false;
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.ForeColor = System.Drawing.Color.White;
            this.LblTitle.Location = new System.Drawing.Point(99, 22);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(143, 48);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "GaBasa.";
            // 
            // PnlModuleName
            // 
            this.PnlModuleName.Controls.Add(this.PicBxModuleIcon);
            this.PnlModuleName.Controls.Add(this.LblModuleName);
            this.PnlModuleName.Location = new System.Drawing.Point(12, 2);
            this.PnlModuleName.Name = "PnlModuleName";
            this.PnlModuleName.Size = new System.Drawing.Size(587, 88);
            this.PnlModuleName.TabIndex = 29;
            // 
            // PicBxModuleIcon
            // 
            this.PicBxModuleIcon.Image = ((System.Drawing.Image)(resources.GetObject("PicBxModuleIcon.Image")));
            this.PicBxModuleIcon.Location = new System.Drawing.Point(24, 14);
            this.PicBxModuleIcon.Name = "PicBxModuleIcon";
            this.PicBxModuleIcon.Size = new System.Drawing.Size(76, 60);
            this.PicBxModuleIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBxModuleIcon.TabIndex = 1;
            this.PicBxModuleIcon.TabStop = false;
            // 
            // LblModuleName
            // 
            this.LblModuleName.AutoSize = true;
            this.LblModuleName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblModuleName.ForeColor = System.Drawing.Color.Black;
            this.LblModuleName.Location = new System.Drawing.Point(103, 19);
            this.LblModuleName.Name = "LblModuleName";
            this.LblModuleName.Size = new System.Drawing.Size(247, 48);
            this.LblModuleName.TabIndex = 0;
            this.LblModuleName.Text = "Module Name";
            // 
            // PnlTopBar
            // 
            this.PnlTopBar.BackColor = System.Drawing.Color.White;
            this.PnlTopBar.BorderColor = System.Drawing.Color.White;
            this.PnlTopBar.Controls.Add(this.PnlProfileHeader);
            this.PnlTopBar.Controls.Add(this.PnlModuleName);
            this.PnlTopBar.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.PnlTopBar.ForeColor = System.Drawing.Color.Black;
            this.PnlTopBar.Location = new System.Drawing.Point(291, 53);
            this.PnlTopBar.Name = "PnlTopBar";
            this.PnlTopBar.Padding = new System.Windows.Forms.Padding(5);
            this.PnlTopBar.ShowText = true;
            this.PnlTopBar.Size = new System.Drawing.Size(1580, 91);
            this.PnlTopBar.TabIndex = 10;
            // 
            // PnlContent
            // 
            this.PnlContent.BackColor = System.Drawing.SystemColors.Control;
            this.PnlContent.Location = new System.Drawing.Point(291, 142);
            this.PnlContent.Name = "PnlContent";
            this.PnlContent.Size = new System.Drawing.Size(1580, 832);
            this.PnlContent.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1898, 1024);
            this.Controls.Add(this.PnlTopBar);
            this.Controls.Add(this.PnlLogo);
            this.Controls.Add(this.PnlSidebar);
            this.Controls.Add(this.PnlContent);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1918, 1006);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.PnlSidebar.ResumeLayout(false);
            this.PnlProfileHeader.ResumeLayout(false);
            this.PnlProfileHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxProfilePic)).EndInit();
            this.PnlLogo.ResumeLayout(false);
            this.PnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxLogo)).EndInit();
            this.PnlModuleName.ResumeLayout(false);
            this.PnlModuleName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBxModuleIcon)).EndInit();
            this.PnlTopBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel PnlSidebar;
        private System.Windows.Forms.Panel PnlProfileHeader;
        private System.Windows.Forms.Label LblProfileRole;
        private System.Windows.Forms.Label LblProfileName;
        private System.Windows.Forms.PictureBox PicBxProfilePic;
        private System.Windows.Forms.Panel PnlLogo;
        private System.Windows.Forms.PictureBox PicBxLogo;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Panel PnlModuleName;
        private System.Windows.Forms.PictureBox PicBxModuleIcon;
        private System.Windows.Forms.Label LblModuleName;
        private ReaLTaiizor.Controls.LostBorderPanel PnlTopBar;
        private System.Windows.Forms.Panel PnlContent;
    }
}