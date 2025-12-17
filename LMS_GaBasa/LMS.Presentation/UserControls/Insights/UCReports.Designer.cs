namespace LMS.Presentation.UserControls.Insights
{
    partial class UCReports
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button4 = new ReaLTaiizor.Controls.Button();
            this.chartTopBooks = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label6 = new System.Windows.Forms.Label();
            this.lostBorderPanel3 = new ReaLTaiizor.Controls.LostBorderPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lostBorderPanel1 = new ReaLTaiizor.Controls.LostBorderPanel();
            this.chartTopBorrowers = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartTopBooks)).BeginInit();
            this.lostBorderPanel3.SuspendLayout();
            this.lostBorderPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTopBorrowers)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(405, 54);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reports and Analytics";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label3.Location = new System.Drawing.Point(67, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 36);
            this.label3.TabIndex = 4;
            this.label3.Text = "From:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dateTimePicker1.Location = new System.Drawing.Point(153, 160);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(257, 30);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.Location = new System.Drawing.Point(68, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(857, 28);
            this.label2.TabIndex = 6;
            this.label2.Text = "Visual insights and analytics to monitor library usage, circulation trends, and s" +
    "ystem performance.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label4.Location = new System.Drawing.Point(432, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 36);
            this.label4.TabIndex = 7;
            this.label4.Text = "To:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dateTimePicker2.Location = new System.Drawing.Point(485, 160);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(257, 30);
            this.dateTimePicker2.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label5.Location = new System.Drawing.Point(769, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 36);
            this.label5.TabIndex = 9;
            this.label5.Text = "Member Type:";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Staff",
            "Guest",
            "Faculty",
            "Student"});
            this.comboBox1.Location = new System.Drawing.Point(955, 158);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(64, 33);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.Text = "All";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.White;
            this.button4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = null;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Location = new System.Drawing.Point(1360, 153);
            this.button4.Name = "button4";
            this.button4.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.button4.Size = new System.Drawing.Size(144, 46);
            this.button4.TabIndex = 30;
            this.button4.Text = "Generate";
            this.button4.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // chartTopBooks
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTopBooks.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartTopBooks.Legends.Add(legend1);
            this.chartTopBooks.Location = new System.Drawing.Point(3, 32);
            this.chartTopBooks.Name = "chartTopBooks";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartTopBooks.Series.Add(series1);
            this.chartTopBooks.Size = new System.Drawing.Size(682, 346);
            this.chartTopBooks.TabIndex = 31;
            this.chartTopBooks.Text = "chart1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label6.Location = new System.Drawing.Point(67, 304);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(271, 36);
            this.label6.TabIndex = 32;
            this.label6.Text = "Top 5 Borrowed Books";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // lostBorderPanel3
            // 
            this.lostBorderPanel3.BackColor = System.Drawing.Color.White;
            this.lostBorderPanel3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.lostBorderPanel3.Controls.Add(this.chartTopBooks);
            this.lostBorderPanel3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lostBorderPanel3.ForeColor = System.Drawing.Color.Black;
            this.lostBorderPanel3.Location = new System.Drawing.Point(73, 389);
            this.lostBorderPanel3.Name = "lostBorderPanel3";
            this.lostBorderPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.lostBorderPanel3.ShowText = true;
            this.lostBorderPanel3.Size = new System.Drawing.Size(688, 381);
            this.lostBorderPanel3.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label8.Location = new System.Drawing.Point(71, 343);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(530, 28);
            this.label8.TabIndex = 36;
            this.label8.Text = "Highlights the books with the highest borrowing frequency.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label7.Location = new System.Drawing.Point(814, 343);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(549, 28);
            this.label7.TabIndex = 39;
            this.label7.Text = "Highlights the member with the highest borrowing frequency.";
            // 
            // lostBorderPanel1
            // 
            this.lostBorderPanel1.BackColor = System.Drawing.Color.White;
            this.lostBorderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(37)))), ((int)(((byte)(50)))));
            this.lostBorderPanel1.Controls.Add(this.chartTopBorrowers);
            this.lostBorderPanel1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lostBorderPanel1.ForeColor = System.Drawing.Color.Black;
            this.lostBorderPanel1.Location = new System.Drawing.Point(816, 389);
            this.lostBorderPanel1.Name = "lostBorderPanel1";
            this.lostBorderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.lostBorderPanel1.ShowText = true;
            this.lostBorderPanel1.Size = new System.Drawing.Size(688, 381);
            this.lostBorderPanel1.TabIndex = 38;
            // 
            // chartTopBorrowers
            // 
            chartArea2.Name = "ChartArea1";
            this.chartTopBorrowers.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartTopBorrowers.Legends.Add(legend2);
            this.chartTopBorrowers.Location = new System.Drawing.Point(3, 32);
            this.chartTopBorrowers.Name = "chartTopBorrowers";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartTopBorrowers.Series.Add(series2);
            this.chartTopBorrowers.Size = new System.Drawing.Size(682, 346);
            this.chartTopBorrowers.TabIndex = 31;
            this.chartTopBorrowers.Text = "chartTopBorrowers";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label9.Location = new System.Drawing.Point(810, 304);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(200, 36);
            this.label9.TabIndex = 37;
            this.label9.Text = "Top 5 Borrowers";
            // 
            // UCReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lostBorderPanel1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lostBorderPanel3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "UCReports";
            this.Size = new System.Drawing.Size(1580, 936);
            ((System.ComponentModel.ISupportInitialize)(this.chartTopBooks)).EndInit();
            this.lostBorderPanel3.ResumeLayout(false);
            this.lostBorderPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTopBorrowers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private ReaLTaiizor.Controls.Button button4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopBooks;
        private System.Windows.Forms.Label label6;
        private ReaLTaiizor.Controls.LostBorderPanel lostBorderPanel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private ReaLTaiizor.Controls.LostBorderPanel lostBorderPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopBorrowers;
        private System.Windows.Forms.Label label9;
    }
}
