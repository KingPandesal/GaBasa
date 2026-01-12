namespace LMS.Presentation.Popup.Inventory
{
    partial class EditBookCopy
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
            this.GrpBxEditCopyDetails = new System.Windows.Forms.GroupBox();
            this.LblNote = new System.Windows.Forms.Label();
            this.LblNote2 = new System.Windows.Forms.Label();
            this.LblNote1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.LblCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.GrpBxEditCopyDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblLocation
            // 
            this.LblLocation.AutoSize = true;
            this.LblLocation.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLocation.ForeColor = System.Drawing.Color.Black;
            this.LblLocation.Location = new System.Drawing.Point(17, 120);
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
            "Lost",
            "Damaged",
            "Repair"});
            this.CmbBxStatus.Location = new System.Drawing.Point(31, 68);
            this.CmbBxStatus.Name = "CmbBxStatus";
            this.CmbBxStatus.Size = new System.Drawing.Size(413, 36);
            this.CmbBxStatus.TabIndex = 68;
            // 
            // TxtLocation
            // 
            this.TxtLocation.ForeColor = System.Drawing.Color.Black;
            this.TxtLocation.Location = new System.Drawing.Point(31, 144);
            this.TxtLocation.Name = "TxtLocation";
            this.TxtLocation.Size = new System.Drawing.Size(413, 34);
            this.TxtLocation.TabIndex = 65;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStatus.ForeColor = System.Drawing.Color.Black;
            this.LblStatus.Location = new System.Drawing.Point(17, 44);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(52, 21);
            this.LblStatus.TabIndex = 67;
            this.LblStatus.Text = "Status";
            // 
            // GrpBxEditCopyDetails
            // 
            this.GrpBxEditCopyDetails.Controls.Add(this.LblNote);
            this.GrpBxEditCopyDetails.Controls.Add(this.LblNote2);
            this.GrpBxEditCopyDetails.Controls.Add(this.LblNote1);
            this.GrpBxEditCopyDetails.Controls.Add(this.LblLocation);
            this.GrpBxEditCopyDetails.Controls.Add(this.CmbBxStatus);
            this.GrpBxEditCopyDetails.Controls.Add(this.TxtLocation);
            this.GrpBxEditCopyDetails.Controls.Add(this.LblStatus);
            this.GrpBxEditCopyDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBxEditCopyDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.GrpBxEditCopyDetails.Location = new System.Drawing.Point(21, 10);
            this.GrpBxEditCopyDetails.Name = "GrpBxEditCopyDetails";
            this.GrpBxEditCopyDetails.Size = new System.Drawing.Size(477, 308);
            this.GrpBxEditCopyDetails.TabIndex = 71;
            this.GrpBxEditCopyDetails.TabStop = false;
            this.GrpBxEditCopyDetails.Text = "Edit Copy Details";
            // 
            // LblNote
            // 
            this.LblNote.AutoSize = true;
            this.LblNote.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LblNote.Location = new System.Drawing.Point(27, 202);
            this.LblNote.MaximumSize = new System.Drawing.Size(530, 0);
            this.LblNote.Name = "LblNote";
            this.LblNote.Size = new System.Drawing.Size(47, 21);
            this.LblNote.TabIndex = 114;
            this.LblNote.Text = "Note:";
            // 
            // LblNote2
            // 
            this.LblNote2.AutoSize = true;
            this.LblNote2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblNote2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LblNote2.Location = new System.Drawing.Point(27, 269);
            this.LblNote2.MaximumSize = new System.Drawing.Size(530, 0);
            this.LblNote2.Name = "LblNote2";
            this.LblNote2.Size = new System.Drawing.Size(405, 21);
            this.LblNote2.TabIndex = 113;
            this.LblNote2.Text = "• Each book copy receives a unique barcode upon saving.";
            // 
            // LblNote1
            // 
            this.LblNote1.AutoSize = true;
            this.LblNote1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblNote1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LblNote1.Location = new System.Drawing.Point(27, 224);
            this.LblNote1.MaximumSize = new System.Drawing.Size(410, 0);
            this.LblNote1.Name = "LblNote1";
            this.LblNote1.Size = new System.Drawing.Size(407, 42);
            this.LblNote1.TabIndex = 112;
            this.LblNote1.Text = "• Status and location are applied to all copies initially and can be edited per c" +
    "opy later.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GrpBxEditCopyDetails);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 320);
            this.panel1.TabIndex = 93;
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(401, 327);
            this.LblCancel.Margin = new System.Windows.Forms.Padding(4);
            this.LblCancel.Name = "LblCancel";
            this.LblCancel.Size = new System.Drawing.Size(97, 43);
            this.LblCancel.TabIndex = 90;
            this.LblCancel.Text = "Cancel";
            this.LblCancel.UseVisualStyleBackColor = false;
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnOk.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOk.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BtnOk.ForeColor = System.Drawing.Color.White;
            this.BtnOk.Location = new System.Drawing.Point(296, 327);
            this.BtnOk.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(97, 43);
            this.BtnOk.TabIndex = 88;
            this.BtnOk.Text = "Ok";
            this.BtnOk.UseVisualStyleBackColor = false;
            // 
            // EditBookCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 392);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.BtnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditBookCopy";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit book copy";
            this.GrpBxEditCopyDetails.ResumeLayout(false);
            this.GrpBxEditCopyDetails.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblLocation;
        private System.Windows.Forms.ComboBox CmbBxStatus;
        private System.Windows.Forms.TextBox TxtLocation;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.GroupBox GrpBxEditCopyDetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Label LblNote;
        private System.Windows.Forms.Label LblNote2;
        private System.Windows.Forms.Label LblNote1;
    }
}