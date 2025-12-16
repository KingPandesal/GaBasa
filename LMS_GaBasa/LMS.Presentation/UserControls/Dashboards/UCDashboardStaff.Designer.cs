namespace LMS.Presentation.UserControls.Dashboards
{
    partial class UCDashboardStaff
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
            this.BigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            this.SuspendLayout();
            // 
            // BigLabel1
            // 
            this.BigLabel1.AutoSize = true;
            this.BigLabel1.BackColor = System.Drawing.Color.Transparent;
            this.BigLabel1.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.BigLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.BigLabel1.Location = new System.Drawing.Point(114, 103);
            this.BigLabel1.Name = "BigLabel1";
            this.BigLabel1.Size = new System.Drawing.Size(128, 67);
            this.BigLabel1.TabIndex = 1;
            this.BigLabel1.Text = "Staff";
            // 
            // UCDashboardStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BigLabel1);
            this.Name = "UCDashboardStaff";
            this.Size = new System.Drawing.Size(358, 274);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ReaLTaiizor.Controls.BigLabel BigLabel1;
    }
}
