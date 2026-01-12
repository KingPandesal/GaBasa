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
            this.GrpBxAddCopyDetails = new System.Windows.Forms.GroupBox();
            this.LblNote = new System.Windows.Forms.Label();
            this.LblNote2 = new System.Windows.Forms.Label();
            this.LblNote1 = new System.Windows.Forms.Label();
            this.LblNoOfCopies = new System.Windows.Forms.Label();
            this.NumPckNoOfCopies = new ReaLTaiizor.Controls.FoxNumeric();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TipPicBxProfilePic = new System.Windows.Forms.ToolTip(this.components);
            this.LblCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.GrpBxAddCopyDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblLocation
            // 
            this.LblLocation.AutoSize = true;
            this.LblLocation.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLocation.ForeColor = System.Drawing.Color.Black;
            this.LblLocation.Location = new System.Drawing.Point(17, 167);
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
            this.CmbBxStatus.Location = new System.Drawing.Point(31, 115);
            this.CmbBxStatus.Name = "CmbBxStatus";
            this.CmbBxStatus.Size = new System.Drawing.Size(413, 36);
            this.CmbBxStatus.TabIndex = 68;
            // 
            // TxtLocation
            // 
            this.TxtLocation.ForeColor = System.Drawing.Color.Black;
            this.TxtLocation.Location = new System.Drawing.Point(31, 191);
            this.TxtLocation.Name = "TxtLocation";
            this.TxtLocation.Size = new System.Drawing.Size(413, 34);
            this.TxtLocation.TabIndex = 65;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStatus.ForeColor = System.Drawing.Color.Black;
            this.LblStatus.Location = new System.Drawing.Point(17, 91);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(52, 21);
            this.LblStatus.TabIndex = 67;
            this.LblStatus.Text = "Status";
            // 
            // GrpBxAddCopyDetails
            // 
            this.GrpBxAddCopyDetails.Controls.Add(this.LblNote);
            this.GrpBxAddCopyDetails.Controls.Add(this.LblNote2);
            this.GrpBxAddCopyDetails.Controls.Add(this.LblNote1);
            this.GrpBxAddCopyDetails.Controls.Add(this.LblNoOfCopies);
            this.GrpBxAddCopyDetails.Controls.Add(this.NumPckNoOfCopies);
            this.GrpBxAddCopyDetails.Controls.Add(this.LblLocation);
            this.GrpBxAddCopyDetails.Controls.Add(this.CmbBxStatus);
            this.GrpBxAddCopyDetails.Controls.Add(this.TxtLocation);
            this.GrpBxAddCopyDetails.Controls.Add(this.LblStatus);
            this.GrpBxAddCopyDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBxAddCopyDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.GrpBxAddCopyDetails.Location = new System.Drawing.Point(21, 10);
            this.GrpBxAddCopyDetails.Name = "GrpBxAddCopyDetails";
            this.GrpBxAddCopyDetails.Size = new System.Drawing.Size(477, 358);
            this.GrpBxAddCopyDetails.TabIndex = 71;
            this.GrpBxAddCopyDetails.TabStop = false;
            this.GrpBxAddCopyDetails.Text = "Add Copy Details";
            // 
            // LblNote
            // 
            this.LblNote.AutoSize = true;
            this.LblNote.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LblNote.Location = new System.Drawing.Point(27, 249);
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
            this.LblNote2.Location = new System.Drawing.Point(27, 316);
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
            this.LblNote1.Location = new System.Drawing.Point(27, 271);
            this.LblNote1.MaximumSize = new System.Drawing.Size(410, 0);
            this.LblNote1.Name = "LblNote1";
            this.LblNote1.Size = new System.Drawing.Size(407, 42);
            this.LblNote1.TabIndex = 112;
            this.LblNote1.Text = "• Status and location are applied to all copies initially and can be edited per c" +
    "opy later.";
            // 
            // LblNoOfCopies
            // 
            this.LblNoOfCopies.AutoSize = true;
            this.LblNoOfCopies.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LblNoOfCopies.ForeColor = System.Drawing.Color.Black;
            this.LblNoOfCopies.Location = new System.Drawing.Point(17, 51);
            this.LblNoOfCopies.Name = "LblNoOfCopies";
            this.LblNoOfCopies.Size = new System.Drawing.Size(103, 21);
            this.LblNoOfCopies.TabIndex = 107;
            this.LblNoOfCopies.Text = "No. of Copies";
            // 
            // NumPckNoOfCopies
            // 
            this.NumPckNoOfCopies.BackColor = System.Drawing.Color.Transparent;
            this.NumPckNoOfCopies.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.NumPckNoOfCopies.ButtonTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(98)))), ((int)(((byte)(110)))));
            this.NumPckNoOfCopies.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.NumPckNoOfCopies.DisabledButtonTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(198)))), ((int)(((byte)(210)))));
            this.NumPckNoOfCopies.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(178)))), ((int)(((byte)(190)))));
            this.NumPckNoOfCopies.EnabledCalc = true;
            this.NumPckNoOfCopies.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.NumPckNoOfCopies.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(78)))), ((int)(((byte)(90)))));
            this.NumPckNoOfCopies.Location = new System.Drawing.Point(148, 51);
            this.NumPckNoOfCopies.Max = 100000;
            this.NumPckNoOfCopies.Min = 1;
            this.NumPckNoOfCopies.Name = "NumPckNoOfCopies";
            this.NumPckNoOfCopies.Size = new System.Drawing.Size(75, 27);
            this.NumPckNoOfCopies.TabIndex = 106;
            this.NumPckNoOfCopies.Text = "foxNumeric1";
            this.NumPckNoOfCopies.Value = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GrpBxAddCopyDetails);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 372);
            this.panel1.TabIndex = 96;
            // 
            // LblCancel
            // 
            this.LblCancel.BackColor = System.Drawing.Color.White;
            this.LblCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.LblCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.LblCancel.Location = new System.Drawing.Point(402, 380);
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
            this.BtnSave.Location = new System.Drawing.Point(297, 380);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 43);
            this.BtnSave.TabIndex = 94;
            this.BtnSave.Text = "Ok";
            this.BtnSave.UseVisualStyleBackColor = false;
            // 
            // AddBookCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 434);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LblCancel);
            this.Controls.Add(this.BtnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBookCopy";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add book copy";
            this.GrpBxAddCopyDetails.ResumeLayout(false);
            this.GrpBxAddCopyDetails.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblLocation;
        private System.Windows.Forms.ComboBox CmbBxStatus;
        private System.Windows.Forms.TextBox TxtLocation;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.GroupBox GrpBxAddCopyDetails;
        private System.Windows.Forms.Label LblNote;
        private System.Windows.Forms.Label LblNote2;
        private System.Windows.Forms.Label LblNote1;
        private System.Windows.Forms.Label LblNoOfCopies;
        private ReaLTaiizor.Controls.FoxNumeric NumPckNoOfCopies;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip TipPicBxProfilePic;
        private System.Windows.Forms.Button LblCancel;
        private System.Windows.Forms.Button BtnSave;
    }
}