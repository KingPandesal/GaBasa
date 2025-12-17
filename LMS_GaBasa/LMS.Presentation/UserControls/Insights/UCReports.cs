using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LMS.Presentation.UserControls.Insights
{
    public partial class UCReports : UserControl
    {
        public UCReports()
        {
            InitializeComponent();
            LoadTopBorrowedBooksChart();
            LoadTopBorrowersChart();
        }

        // ==========================
        // TOP 5 BORROWED BOOKS
        // ==========================
        private void LoadTopBorrowedBooksChart()
        {
            chartTopBooks.Series.Clear();
            chartTopBooks.ChartAreas.Clear();
            chartTopBooks.Legends.Clear();

            ChartArea area = new ChartArea("BooksArea");
            area.AxisX.Title = "Number of Borrows";
            area.AxisY.Title = "Book Title";
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.Enabled = false;
            area.BackColor = Color.Transparent;

            chartTopBooks.ChartAreas.Add(area);

            Series series = new Series("Borrows");
            series.ChartType = SeriesChartType.Bar;
            series.IsValueShownAsLabel = true;
            series.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // 🔹 HARD-CODED DATA
            series.Points.AddXY("Clean Code", 48);
            series.Points.AddXY("Design Patterns", 42);
            series.Points.AddXY("Intro to Algorithms", 38);
            series.Points.AddXY("C# in Depth", 31);
            series.Points.AddXY("Database Systems", 27);

            chartTopBooks.Series.Add(series);
        }

        // ==========================
        // TOP 5 ACTIVE BORROWERS
        // ==========================
        private void LoadTopBorrowersChart()
        {
            chartTopBorrowers.Series.Clear();
            chartTopBorrowers.ChartAreas.Clear();
            chartTopBorrowers.Legends.Clear();

            ChartArea area = new ChartArea("BorrowersArea");
            area.AxisX.Title = "Borrower";
            area.AxisY.Title = "Books Borrowed";
            area.AxisX.Interval = 1;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.Enabled = false;
            area.BackColor = Color.Transparent;

            chartTopBorrowers.ChartAreas.Add(area);

            Series series = new Series("Borrow Count");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // 🔹 HARD-CODED DATA
            series.Points.AddXY("Juan Dela Cruz", 22);
            series.Points.AddXY("Maria Santos", 19);
            series.Points.AddXY("John Reyes", 17);
            series.Points.AddXY("Ana Lopez", 15);
            series.Points.AddXY("Mark Villanueva", 13);

            chartTopBorrowers.Series.Add(series);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
