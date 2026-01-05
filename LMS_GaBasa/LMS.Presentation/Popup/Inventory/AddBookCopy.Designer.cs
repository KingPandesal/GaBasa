namespace LMS.Presentation.Popup.Inventory
{
    partial class AddBookCopy
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
            this.LblLocation = new System.Windows.Forms.Label();
            this.CmbBxStatus = new System.Windows.Forms.ComboBox();
            this.TxtLocation = new System.Windows.Forms.TextBox();
            this.LblStatus = new System.Windows.Forms.Label();
            this.GrpBxAddCopy = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.LblCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.GrpBxAddCopy.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblLocation
            // 
            this.LblLocation.AutoSize = true;
            this.LblLocation.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLocation.ForeColor = System.Drawing.Color.Black;
            this.LblLocation.Location = new System.Drawing.Point(17, 123);
            this.LblLocation.Name = "LblLocation";
            this.LblLocation.Size = new System.Drawing.Size(69, 21);
            this.LblLocation.TabIndex = 61;
            this.LblLocation.Text = "Location";
            // 
            // CmbBxStatus
            // 
            this.CmbBxStatus.ForeColor = System.Drawing.Color.Black;
            this.CmbBxStatus.FormattingEnabled = true;
            this.CmbBxStatus.Items.AddRange(new object[] {
            "Available",
            "Borrowed",
            "Reserved",
            "Lost",
            "Damaged",
            "Repair"});
            this.CmbBxStatus.Location = new System.Drawing.Point(31, 71);
            this.CmbBxStatus.Name = "CmbBxStatus";
            this.CmbBxStatus.Size = new System.Drawing.Size(413, 36);
            this.CmbBxStatus.TabIndex = 68;
            // 
            // TxtLocation
            // 
            this.TxtLocation.ForeColor = System.Drawing.Color.Black;
            this.TxtLocation.Location = new System.Drawing.Point(31, 147);
            this.TxtLocation.Name = "TxtLocation";
            this.TxtLocation.Size = new System.Drawing.Size(413, 34);
            this.TxtLocation.TabIndex = 65;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStatus.ForeColor = System.Drawing.Color.Black;
            this.LblStatus.Location = new System.Drawing.Point(17, 47);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(52, 21);
            this.LblStatus.TabIndex = 67;
            this.LblStatus.Text = "Status";
            // 
            // GrpBxAddCopy
            // 
            this.GrpBxAddCopy.Controls.Add(this.LblLocation);
            this.GrpBxAddCopy.Controls.Add(this.CmbBxStatus);
            this.GrpBxAddCopy.Controls.Add(this.TxtLocation);
            this.GrpBxAddCopy.Controls.Add(this.LblStatus);
            this.GrpBxAddCopy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBxAddCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.GrpBxAddCopy.Location = new System.Drawing.Point(21, 23);
            this.GrpBxAddCopy.Name = "GrpBxAddCopy";
            this.GrpBxAddCopy.Size = new System.Drawing.Size(477, 203);
            this.GrpBxAddCopy.TabIndex = 71;
            this.GrpBxAddCopy.TabStop = false;
            this.GrpBxAddCopy.Text = "Add Copy";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GrpBxAddCopy);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 234);
            this.panel1.TabIndex = 96;
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(401, 241);
            this.LblCancel.Margin = new System.Windows.Forms.Padding(4);
            this.LblCancel.Name = "LblCancel";
            this.LblCancel.Size = new System.Drawing.Size(97, 43);
            this.LblCancel.TabIndex = 95;
            this.LblCancel.Text = "Cancel";
            this.LblCancel.UseVisualStyleBackColor = false;
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.Location = new System.Drawing.Point(296, 241);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 43);
            this.BtnSave.TabIndex = 94;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = false;
            // 
            // AddBookCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 296);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.BtnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBookCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add book copy";
            this.GrpBxAddCopy.ResumeLayout(false);
            this.GrpBxAddCopy.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblLocation;
        private System.Windows.Forms.ComboBox CmbBxStatus;
        private System.Windows.Forms.TextBox TxtLocation;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.GroupBox GrpBxAddCopy;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Button BtnSave;
    }
}