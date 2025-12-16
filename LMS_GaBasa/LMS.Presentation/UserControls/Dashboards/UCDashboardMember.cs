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
    public partial class UCDashboardMember : UserControl
    {
        public UCDashboardMember()
        {
            InitializeComponent();
            LoadLibraryCompletionChart();
            LoadWeeklyBorrowingTrend();
        }

        private void LoadLibraryCompletionChart()
        {
            ChartLibraryCompletion.Series.Clear();
            ChartLibraryCompletion.ChartAreas.Clear();
            ChartLibraryCompletion.Legends.Clear();
            ChartLibraryCompletion.Titles.Clear();

            // Chart Area
            ChartArea area = new ChartArea();
            area.BackColor = Color.Transparent;
            ChartLibraryCompletion.ChartAreas.Add(area);

            // Legend
            Legend legend = new Legend
            {
                Docking = Docking.Bottom
            };
            ChartLibraryCompletion.Legends.Add(legend);

            // Series
            Series series = new Series("Completion")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "0'%'"
            };

            // 🔹 HARD-CODED DATA (UI ONLY)
            series.Points.AddXY("Completed", 75);
            series.Points.AddXY("In Progress", 25);

            ChartLibraryCompletion.Series.Add(series);

            // Title
            //ChartLibraryCompletion.Titles.Add("Library Completion Status");
            //ChartLibraryCompletion.Titles[0].Font =
            //    new Font("Segoe UI", 12, FontStyle.Bold);

            // Style labels
            foreach (DataPoint point in series.Points)
            {
                point.LabelForeColor = Color.White;
                point.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                point.ToolTip = $"{point.AxisLabel}: {point.YValues[0]}%";
            }
        }

        private void LoadWeeklyBorrowingTrend()
        {
            ChartBorrowingTrend.Series.Clear();
            ChartBorrowingTrend.ChartAreas.Clear();
            ChartBorrowingTrend.Legends.Clear();
            ChartBorrowingTrend.Titles.Clear();

            // Chart Area
            ChartArea area = new ChartArea("MainArea");
            area.AxisX.Title = "Week";
            area.AxisY.Title = "Number of Borrowings";
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

            // 🔹 Hard-coded weekly data
            series.Points.AddXY("Week 1", 5);
            series.Points.AddXY("Week 2", 8);
            series.Points.AddXY("Week 3", 6);
            series.Points.AddXY("Week 4", 10);
            series.Points.AddXY("Week 5", 4);
            series.Points.AddXY("Week 6", 7);

            ChartBorrowingTrend.Series.Add(series);

            // Optional: smooth line
            series["LineTension"] = "0.3";

            // Title
            //ChartBorrowingTrend.Titles.Add("Borrowing Trend (Last 6 Weeks)");
            //ChartBorrowingTrend.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Remove legend if only one series
            ChartBorrowingTrend.Legends.Clear();

            // Grid styling
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            // Tooltips
            foreach (var point in series.Points)
            {
                point.ToolTip = $"{point.AxisLabel}: {point.YValues[0]} books";
            }
        }

        private void UCDashboardMember_Load(object sender, EventArgs e)
        {

        }

        private void lostBorderPanel2_Click(object sender, EventArgs e)
        {

        }
    }
}
