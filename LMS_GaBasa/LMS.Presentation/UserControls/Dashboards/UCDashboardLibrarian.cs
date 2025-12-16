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

namespace LMS.Presentation.UserControls.Dashboards
{
    public partial class UCDashboardLibrarian : UserControl
    {
        public UCDashboardLibrarian()
        {
            InitializeComponent();
            LoadBorrowingTrendChart();
            LoadBorrowingsByUserTypeChart();
        }

        // Line Graph: Borrowing Trend (Last 7 Days)
        private void LoadBorrowingTrendChart()
        {
            ChartBorrowingTrend.Series.Clear();
            ChartBorrowingTrend.ChartAreas.Clear();
            ChartBorrowingTrend.Legends.Clear();
            ChartBorrowingTrend.Titles.Clear();

            // Chart Area
            ChartArea area = new ChartArea("MainArea");
            area.AxisX.Title = "Day";
            area.AxisY.Title = "No. of Borrows";
            area.AxisX.Interval = 1;
            area.BackColor = Color.Transparent;
            ChartBorrowingTrend.ChartAreas.Add(area);

            // Series
            Series series = new Series("Borrowings")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7
            };

            // 🔹 HARD-CODED DATA (UI ONLY)
            series.Points.AddXY("Mon", 12);
            series.Points.AddXY("Tue", 18);
            series.Points.AddXY("Wed", 15);
            series.Points.AddXY("Thu", 20);
            series.Points.AddXY("Fri", 25);
            series.Points.AddXY("Sat", 10);
            series.Points.AddXY("Sun", 8);

            ChartBorrowingTrend.Series.Add(series);

            // Title
            //ChartBorrowingTrend.Titles.Add("Borrowing Trend (Last 7 Days)");
            //ChartBorrowingTrend.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Grid styling (optional but clean)
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
        }

        // Pie Chart: Borrowings by User Type
        private void LoadBorrowingsByUserTypeChart()
        {
            chartBorrowingsByUserType.Series.Clear();
            chartBorrowingsByUserType.ChartAreas.Clear();
            chartBorrowingsByUserType.Legends.Clear();

            // Chart Area
            ChartArea area = new ChartArea();
            area.BackColor = Color.Transparent;
            chartBorrowingsByUserType.ChartAreas.Add(area);

            // Legend
            Legend legend = new Legend();
            legend.Docking = Docking.Right;
            chartBorrowingsByUserType.Legends.Add(legend);

            // Series
            Series series = new Series("Borrowings")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "0'%'"
            };

            // HARD-CODED DATA (UI ONLY)
            series.Points.AddXY("Students", 62);
            series.Points.AddXY("Faculty", 18);
            series.Points.AddXY("Staff", 12);
            series.Points.AddXY("Guest", 8);

            chartBorrowingsByUserType.Series.Add(series);

            // Optional: style labels
            foreach (DataPoint point in series.Points)
            {
                point.LabelForeColor = Color.White;
                point.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        private void UCDashboardLibrarian_Load(object sender, EventArgs e)
        {

        }

        private void lostBorderPanel3_Click(object sender, EventArgs e)
        {

        }
    }
}
