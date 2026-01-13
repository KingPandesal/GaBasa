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
using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.Model.Models.Users;

namespace LMS.Presentation.UserControls.Dashboards
{
    public partial class UCDashboard : UserControl
    {
        private readonly IReportManager _reportManager;
        private float _finesLabelOriginalFontSize = 0f;
        private const float FinesLabelMinFontSize = 8f;

        /// <summary>
        /// Action delegate to request navigation to a module (set by parent form).
        /// </summary>
        public Action<string> OnNavigateToModule { get; set; }

        public UCDashboard() : this(new ReportManager())
        {
        }

        /// <summary>
        /// Constructor for dependency injection (testability).
        /// </summary>
        /// <param name="reportManager">The report manager abstraction.</param>
        public UCDashboard(IReportManager reportManager)
        {
            InitializeComponent();
            _reportManager = reportManager ?? throw new ArgumentNullException(nameof(reportManager));

            // remember original font size so we can grow back when space is available
            if (LblValueTotalFInes != null)
                _finesLabelOriginalFontSize = LblValueTotalFInes.Font.Size;

            // Wire sizing events so the fines label will adjust whenever size or text changes
            this.Load += UCDashboard_Load_AdjustFinesLabel;
            this.SizeChanged += UCDashboard_SizeOrLayoutChanged;
            if (lostBorderPanel17 != null)
                lostBorderPanel17.SizeChanged += UCDashboard_SizeOrLayoutChanged;
            if (LblValueTotalFInes != null)
                LblValueTotalFInes.TextChanged += LblValueTotalFInes_TextChanged;
        }

        private void UCDashboard_Load_AdjustFinesLabel(object sender, EventArgs e) => TryAdjustFinesLabel();
        private void UCDashboard_SizeOrLayoutChanged(object sender, EventArgs e) => TryAdjustFinesLabel();
        private void LblValueTotalFInes_TextChanged(object sender, EventArgs e) => TryAdjustFinesLabel();

        private void TryAdjustFinesLabel()
        {
            // Safety checks: designer fields might be null during some design-time operations
            if (LblValueTotalFInes == null) return;

            // container is the immediate host panel where label sits
            var container = lostBorderPanel17 ?? LblValueTotalFInes.Parent;
            if (container == null) return;

            AdjustLabelFontToFit(LblValueTotalFInes, container, FinesLabelMinFontSize, _finesLabelOriginalFontSize);
        }

        /// <summary>
        /// Adjusts the label's font size so the rendered text fits horizontally inside the container.
        /// Attempts sizes from max down to min (integer steps).
        /// Keeps label's Font.Style and Family.
        /// </summary>
        private void AdjustLabelFontToFit(Label label, Control container, float minFontSize, float maxFontSize)
        {
            if (label == null || container == null) return;

            string text = label.Text ?? string.Empty;
            if (string.IsNullOrEmpty(text)) return;

            // Available width inside container for the label.
            // Allow small padding so text is not flush with edges.
            int padding = 8;
            int availableWidth = Math.Max(0, container.ClientSize.Width - label.Left - padding);
            if (label.Left <= 0) availableWidth = Math.Max(0, container.ClientSize.Width - (padding * 2));

            // Start from maxFontSize and decrement until it fits or reach min
            float testSize = Math.Min(maxFontSize <= 0 ? label.Font.Size : maxFontSize, label.Font.Size);
            bool fits = false;

            using (var g = label.CreateGraphics())
            {
                for (float size = testSize; size >= minFontSize; size -= 1f)
                {
                    using (var testFont = new Font(label.Font.FontFamily, size, label.Font.Style))
                    {
                        // MeasureString gives approximate width; add small cushion for safety
                        var measured = g.MeasureString(text, testFont);
                        if (measured.Width <= availableWidth)
                        {
                            testSize = size;
                            fits = true;
                            break;
                        }
                    }
                }
                // If nothing fits (very small container), clamp to minFontSize
                if (!fits) testSize = minFontSize;
            }

            // Only update if font size changed to avoid unnecessary layout invalidation
            if (Math.Abs(label.Font.Size - testSize) > 0.1f)
                label.Font = new Font(label.Font.FontFamily, testSize, label.Font.Style);
        }

        /// <summary>
        /// Sets the welcome message with the user's name.
        /// Call this method after creating the control to personalize the dashboard.
        /// </summary>
        /// <param name="user">The current logged-in user.</param>
        public void SetWelcomeUser(User user)
        {
            if (user == null) return;
            string displayName = !string.IsNullOrWhiteSpace(user.FirstName) ? user.FirstName : user.Username ?? "User";
            LblWelcomeWithName.Text = $"Welcome, {displayName}! Here's an overview of today's library activities ";
        }

        private void UCDashboardLibrarian_Load(object sender, EventArgs e) => LoadDashboardData();

        private void LoadDashboardData()
        {
            try
            {
                LblValueTotalBooks.Text = _reportManager.GetTotalBooks().ToString("N0");
                LblValueBorrowedBooks.Text = _reportManager.GetTotalBorrowedBooks().ToString("N0");
                LblValueReserveBooks.Text = _reportManager.GetTotalReservedBooks().ToString("N0");
                LblValueOverdueBooks.Text = _reportManager.GetTotalOverdueBooks().ToString("N0");
                decimal totalFines = _reportManager.GetTotalUnpaidFines();
                LblValueTotalFInes.Text = $"₱{totalFines:N2}";
                LblFinesLastUpdated.Text = $"Last updated: {DateTime.Now:MMM dd, yyyy}";
                TryAdjustFinesLabel();

                // Charts
                LoadMostUseChart();
                LoadBorrowingTrendChart();
                LoadBorrowingByUserTypeChart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
            }
        }

        private void LoadMostUseChart()
        {
            try
            {
                // chartMostUse is created in designer
                var pieChart = this.chartMostUse;
                if (pieChart == null) return;

                var usage = _reportManager.GetUsageByRole();
                if (usage == null || usage.Count == 0)
                {
                    pieChart.Series.Clear();
                    pieChart.ChartAreas.Clear();
                    pieChart.Legends.Clear();
                    pieChart.Invalidate();
                    return;
                }

                // Prepare chart
                pieChart.Series.Clear();
                pieChart.ChartAreas.Clear();
                pieChart.Legends.Clear();

                var ca = new ChartArea("PieArea") { BackColor = Color.Transparent };
                pieChart.ChartAreas.Add(ca);

                const string legendName = "Legend";
                var legend = new Legend(legendName)
                {
                    Docking = Docking.Right,
                    LegendStyle = LegendStyle.Column
                };
                pieChart.Legends.Add(legend);

                var series = new Series("UsageByRole")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#VALX: #PERCENT{P1}",
                    Legend = legendName
                };

                var palette = new[]
                {
                    Color.FromArgb(175,37,50),
                    Color.FromArgb(52,152,219),
                    Color.FromArgb(46,204,113),
                    Color.FromArgb(241,196,15),
                    Color.FromArgb(155,89,182),
                    Color.FromArgb(230,126,34)
                };

                int i = 0;
                foreach (var kvp in usage.OrderByDescending(k => k.Value))
                {
                    int p = series.Points.AddXY(kvp.Key ?? "Unknown", kvp.Value);
                    series.Points[p].Color = palette[i % palette.Length];
                    series.Points[p].ToolTip = $"{kvp.Key}: {kvp.Value}";
                    i++;
                }

                pieChart.Series.Add(series);
                pieChart.Invalidate();

                // Update panel label
                LblCategoryDistribution.Text = "Most Use by Role";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading MostUse pie chart: {ex.Message}");
            }
        }

        private void LoadBorrowingTrendChart()
        {
            try
            {
                var trendData = _reportManager.GetBorrowingTrendByWeek(8);
                if (ChartBorrowingTrends == null || trendData == null || trendData.Count == 0) return;
                ChartBorrowingTrends.Series.Clear();
                ChartBorrowingTrends.ChartAreas.Clear();

                var chartArea = new ChartArea("MainArea");
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.LabelStyle.Angle = -45;
                chartArea.AxisX.Interval = 1;
                chartArea.BackColor = Color.White;
                ChartBorrowingTrends.ChartAreas.Add(chartArea);

                var series = new Series("Borrowings")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.FromArgb(175, 37, 50),
                    BorderWidth = 3,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8,
                    MarkerColor = Color.FromArgb(175, 37, 50)
                };

                foreach (var kvp in trendData)
                    series.Points.AddXY(kvp.Key, kvp.Value);

                ChartBorrowingTrends.Series.Add(series);
                ChartBorrowingTrends.Legends.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading borrowing trend chart: {ex.Message}");
            }
        }

        private void LoadBorrowingByUserTypeChart()
        {
            try
            {
                var userTypeData = _reportManager.GetBorrowingByMemberType();
                if (chartBorrowingsByUserTypes == null || userTypeData == null || userTypeData.Count == 0) return;

                chartBorrowingsByUserTypes.Series.Clear();
                chartBorrowingsByUserTypes.ChartAreas.Clear();

                var chartArea = new ChartArea("MainArea");
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.BackColor = Color.White;
                chartBorrowingsByUserTypes.ChartAreas.Add(chartArea);

                var series = new Series("Borrowings") { ChartType = SeriesChartType.Bar };

                var palette = new[]
                {
                    Color.FromArgb(175, 37, 50),
                    Color.FromArgb(52, 152, 219),
                    Color.FromArgb(46, 204, 113),
                    Color.FromArgb(241, 196, 15),
                    Color.FromArgb(155, 89, 182)
                };

                int colorIndex = 0;
                foreach (var kvp in userTypeData)
                {
                    int pIndex = series.Points.AddXY(kvp.Key, kvp.Value);
                    series.Points[pIndex].Color = palette[colorIndex % palette.Length];
                    series.Points[pIndex].ToolTip = $"{kvp.Key}: {kvp.Value}";
                    colorIndex++;
                }

                chartBorrowingsByUserTypes.Series.Add(series);
                chartBorrowingsByUserTypes.Legends.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading borrowing by user type chart: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnNavigateToModule != null)
            {
                OnNavigateToModule.Invoke("Catalog");
                return;
            }

            try
            {
                var mainForm = this.FindForm();
                if (mainForm != null)
                {
                    var method = mainForm.GetType().GetMethod("LoadContentByName",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    method?.Invoke(mainForm, new object[] { "Catalog" });
                }
            }
            catch
            {
                MessageBox.Show("Unable to navigate to Catalog.", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // remaining empty handlers omitted for brevity...
    }
}
