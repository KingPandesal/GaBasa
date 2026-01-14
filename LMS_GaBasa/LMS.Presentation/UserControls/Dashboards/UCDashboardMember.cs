using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LMS.DataAccess.Database;
using LMS.DataAccess.Repositories;
using LMS.Model.Models.Users;

namespace LMS.Presentation.UserControls.Dashboards
{
    public partial class UCDashboardMember : UserControl
    {
        private readonly CirculationRepository _circulationRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly BookRepository _bookRepository;
        private readonly DbConnection _db;

        private int _memberId;
        private User _currentUser;

        // Font shrink helpers for member total fines label
        private float _finesLabelOriginalFontSize = 0f;
        private const float FinesLabelMinFontSize = 8f;

        /// <summary>
        /// Optional delegate parent form can set to allow navigation without reflection.
        /// </summary>
        public Action<string> OnNavigateToModule { get; set; }

        public UCDashboardMember()
        {
            InitializeComponent();

            _circulationRepository = new CirculationRepository();
            _reservation_repository_init();
            _reservationRepository = new ReservationRepository();
            _bookRepository = new BookRepository();
            _db = new DbConnection();

            // Remember original fines label font size (designer may be null in design-time)
            if (LblValueTotalFinesMember != null)
                _finesLabelOriginalFontSize = LblValueTotalFinesMember.Font.Size;

            // Wire lifecycle events
            this.Load += UCDashboardMember_Load;
            this.SizeChanged += UCDashboardMember_SizeOrLayoutChanged;

            // Wire catalog button (designer name is button1)
            try
            {
                if (BtnViewCatalog != null)
                {
                    BtnViewCatalog.Click -= BtnViewCatalog_Click;
                    BtnViewCatalog.Click += BtnViewCatalog_Click;
                }
            }
            catch { }

            if (LblValueTotalFinesMember != null)
                LblValueTotalFinesMember.TextChanged += LblValueTotalFinesMember_TextChanged;

            if (lostBorderPanel17 != null)
                lostBorderPanel17.SizeChanged += UCDashboardMember_SizeOrLayoutChanged;
        }

        private void _reservation_repository_init() { /* placeholder */ }

        private void UCDashboardMember_Load(object sender, EventArgs e)
        {
            LoadCurrentUser();
            LoadDashboardData();
            TryAdjustFinesLabel();
        }

        private void UCDashboardMember_SizeOrLayoutChanged(object sender, EventArgs e)
        {
            TryAdjustFinesLabel();
        }

        private void LblValueTotalFinesMember_TextChanged(object sender, EventArgs e)
        {
            TryAdjustFinesLabel();
        }

        private void BtnViewCatalog_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Use reflection to obtain current user object and member id from MainForm.
        /// </summary>
        private void LoadCurrentUser()
        {
            try
            {
                var mainForm = this.FindForm();
                if (mainForm == null) return;

                var currentUserField = mainForm.GetType().GetField("_currentUser",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (currentUserField == null) return;

                var currentUser = currentUserField.GetValue(mainForm);
                if (currentUser == null) return;

                _currentUser = currentUser as User;

                var userIdProp = currentUser.GetType().GetProperty("UserID");
                if (userIdProp != null)
                {
                    int userId = (int)userIdProp.GetValue(currentUser);
                    _memberId = _reservationRepository.GetMemberIdByUserId(userId);
                }
            }
            catch
            {
                _memberId = 0;
                _currentUser = null;
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                SetWelcomeName();
                LoadBorrowedBooksCount();
                LoadReservedBooksCount();
                LoadOverdueBooksCount();
                LoadTotalFinesMember();
                LoadTotalBooks();

                // Charts
                LoadLibraryCompletionChart();
                LoadMemberBorrowingTrendChart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading member dashboard data: {ex.Message}");
            }
        }

        private void SetWelcomeName()
        {
            try
            {
                string displayName = "User";
                if (_currentUser != null)
                {
                    if (!string.IsNullOrWhiteSpace(_currentUser.FirstName))
                        displayName = _currentUser.FirstName;
                    else if (!string.IsNullOrWhiteSpace(_currentUser.Username))
                        displayName = _currentUser.Username;
                }

                if (LblWelcomeName != null)
                    LblWelcomeName.Text = $"Welcome, {displayName}! Here’s an overview of your library activities.";
            }
            catch
            {
                if (LblWelcomeName != null)
                    LblWelcomeName.Text = "Welcome! Here’s an overview of your library activities.";
            }
        }

        private void LoadBorrowedBooksCount()
        {
            try
            {
                if (_memberId <= 0)
                {
                    if (LblValueBorrowedBooks != null) LblValueBorrowedBooks.Text = "0";
                    return;
                }

                int count = _circulationRepository.GetCurrentBorrowedCount(_memberId);
                if (LblValueBorrowedBooks != null) LblValueBorrowedBooks.Text = count.ToString("N0");
            }
            catch
            {
                if (LblValueBorrowedBooks != null) LblValueBorrowedBooks.Text = "0";
            }
        }

        private void LoadReservedBooksCount()
        {
            try
            {
                if (_memberId <= 0)
                {
                    if (LblValueReserveBooks != null) LblValueReserveBooks.Text = "0";
                    return;
                }

                var reservations = _reservationRepository.GetActiveByMemberId(_memberId);
                int count = reservations?.Count ?? 0;
                if (LblValueReserveBooks != null) LblValueReserveBooks.Text = count.ToString("N0");
            }
            catch
            {
                if (LblValueReserveBooks != null) LblValueReserveBooks.Text = "0";
            }
        }

        private void LoadOverdueBooksCount()
        {
            try
            {
                if (_memberId <= 0)
                {
                    if (LblValueOverdueBooks != null) LblValueOverdueBooks.Text = "0";
                    return;
                }

                int count = _circulationRepository.GetOverdueCount(_memberId);
                if (LblValueOverdueBooks != null) LblValueOverdueBooks.Text = count.ToString("N0");
            }
            catch
            {
                if (LblValueOverdueBooks != null) LblValueOverdueBooks.Text = "0";
            }
        }

        private void LoadTotalFinesMember()
        {
            try
            {
                if (_memberId <= 0)
                {
                    if (LblValueTotalFinesMember != null) LblValueTotalFinesMember.Text = "₱0.00";
                    return;
                }

                decimal total = _circulation_repository_GetTotalUnpaidFinesSafe(_memberId);
                if (LblValueTotalFinesMember != null) LblValueTotalFinesMember.Text = $"₱{total:N2}";
                TryAdjustFinesLabel();
            }
            catch
            {
                if (LblValueTotalFinesMember != null) LblValueTotalFinesMember.Text = "₱0.00";
            }
        }

        // safe wrapper because method name in repository is GetTotalUnpaidFines(int)
        private decimal _circulation_repository_GetTotalUnpaidFinesSafe(int memberId)
        {
            try { return _circulationRepository.GetTotalUnpaidFines(memberId); }
            catch { return 0m; }
        }

        private void LoadTotalBooks()
        {
            try
            {
                var books = _bookRepository.GetAll();
                int total = books?.Count ?? 0;
                if (LblValueTotalBooks != null) LblValueTotalBooks.Text = total.ToString("N0");
            }
            catch
            {
                if (LblValueTotalBooks != null) LblValueTotalBooks.Text = "0";
            }
        }

        /// <summary>
        /// Pie chart: borrowed vs available book copies (system-wide).
        /// </summary>
        private void LoadLibraryCompletionChart()
        {
            try
            {
                if (ChartLibraryCompletion == null) return;

                int borrowedCount = 0;
                int notBorrowedCount = 0;

                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"
                        SELECT 
                            SUM(CASE WHEN UPPER(ISNULL(Status,'Available')) = 'BORROWED' THEN 1 ELSE 0 END) AS Borrowed,
                            SUM(CASE WHEN UPPER(ISNULL(Status,'Available')) <> 'BORROWED' THEN 1 ELSE 0 END) AS NotBorrowed
                        FROM [BookCopy]";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            borrowedCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            notBorrowedCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        }
                    }
                }

                ChartLibraryCompletion.Series.Clear();
                ChartLibraryCompletion.ChartAreas.Clear();
                ChartLibraryCompletion.Legends.Clear();

                if (borrowedCount == 0 && notBorrowedCount == 0)
                {
                    ChartLibraryCompletion.Invalidate();
                    return;
                }

                var ca = new ChartArea("PieArea") { BackColor = Color.Transparent };
                ChartLibraryCompletion.ChartAreas.Add(ca);

                const string legendName = "Legend";
                var legend = new Legend(legendName)
                {
                    Docking = Docking.Right,
                    LegendStyle = LegendStyle.Column
                };
                ChartLibraryCompletion.Legends.Add(legend);

                var series = new Series("LibraryStatus")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#VALX\n#PERCENT{P0}",
                    Legend = legendName
                };

                int idxBorrowed = series.Points.AddXY("Borrowed", borrowedCount);
                series.Points[idxBorrowed].Color = Color.FromArgb(175, 37, 50);
                series.Points[idxBorrowed].ToolTip = $"Borrowed: {borrowedCount}";

                int idxAvailable = series.Points.AddXY("Available", notBorrowedCount);
                series.Points[idxAvailable].Color = Color.FromArgb(46, 204, 113);
                series.Points[idxAvailable].ToolTip = $"Available: {notBorrowedCount}";

                ChartLibraryCompletion.Series.Add(series);
                ChartLibraryCompletion.Invalidate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading library completion chart: {ex.Message}");
            }
        }

        /// <summary>
        /// Member borrowing trend over last 7 days.
        /// </summary>
        private void LoadMemberBorrowingTrendChart()
        {
            try
            {
                if (ChartBorrowingTrends == null) return;

                if (_memberId <= 0)
                {
                    ChartBorrowingTrends.Series.Clear();
                    ChartBorrowingTrends.ChartAreas.Clear();
                    ChartBorrowingTrends.Legends.Clear();
                    ChartBorrowingTrends.Invalidate();
                    return;
                }

                var trendData = GetMemberBorrowingTrendByDay(7);
                if (trendData == null || trendData.Count == 0)
                {
                    ChartBorrowingTrends.Series.Clear();
                    ChartBorrowingTrends.ChartAreas.Clear();
                    ChartBorrowingTrends.Invalidate();
                    return;
                }

                ChartBorrowingTrends.Series.Clear();
                ChartBorrowingTrends.ChartAreas.Clear();
                ChartBorrowingTrends.Legends.Clear();

                var chartArea = new ChartArea("MainArea") { BackColor = Color.White };
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.LabelStyle.Angle = -45;
                chartArea.AxisX.Interval = 1;
                ChartBorrowingTrends.ChartAreas.Add(chartArea);

                var series = new Series("MyBorrowings")
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
                ChartBorrowingTrends.Invalidate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading member borrowing trend chart: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns counts per day (label: "MMM dd") for member borrows over last N days.
        /// </summary>
        private Dictionary<string, int> GetMemberBorrowingTrendByDay(int days = 7)
        {
            var result = new Dictionary<string, int>();
            try
            {
                var today = DateTime.Today;
                for (int i = days - 1; i >= 0; i--)
                {
                    var d = today.AddDays(-i);
                    result[d.ToString("MMM dd", CultureInfo.InvariantCulture)] = 0;
                }

                if (_memberId <= 0) return result;

                var startDate = today.AddDays(-(days - 1));
                var endDate = today.AddDays(1);

                using (var conn = _db.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"
                        SELECT BorrowDate
                        FROM [BorrowingTransaction]
                        WHERE MemberID = @MemberID
                          AND BorrowDate >= @StartDate
                          AND BorrowDate < @EndDate";

                    var p1 = cmd.CreateParameter();
                    p1.ParameterName = "@MemberID";
                    p1.DbType = DbType.Int32;
                    p1.Value = _memberId;
                    cmd.Parameters.Add(p1);

                    var p2 = cmd.CreateParameter();
                    p2.ParameterName = "@StartDate";
                    p2.DbType = DbType.DateTime;
                    p2.Value = startDate;
                    cmd.Parameters.Add(p2);

                    var p3 = cmd.CreateParameter();
                    p3.ParameterName = "@EndDate";
                    p3.DbType = DbType.DateTime;
                    p3.Value = endDate;
                    cmd.Parameters.Add(p3);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0)) continue;
                            var borrowDate = reader.GetDateTime(0).Date;
                            var label = borrowDate.ToString("MMM dd", CultureInfo.InvariantCulture);
                            if (result.ContainsKey(label))
                                result[label]++;
                        }
                    }
                }
            }
            catch
            {
                // swallow and return partial
            }
            return result;
        }

        // Font-fitting helpers for LblValueTotalFinesMember
        private void TryAdjustFinesLabel()
        {
            if (LblValueTotalFinesMember == null) return;

            var container = lostBorderPanel17 ?? LblValueTotalFinesMember.Parent;
            if (container == null) return;

            AdjustLabelFontToFit(LblValueTotalFinesMember, container, FinesLabelMinFontSize, _finesLabelOriginalFontSize);
        }

        private void AdjustLabelFontToFit(Label label, Control container, float minFontSize, float maxFontSize)
        {
            if (label == null || container == null) return;

            string text = label.Text ?? string.Empty;
            if (string.IsNullOrEmpty(text)) return;

            int padding = 8;
            int availableWidth = Math.Max(0, container.ClientSize.Width - label.Left - padding);
            if (label.Left <= 0) availableWidth = Math.Max(0, container.ClientSize.Width - (padding * 2));

            float testSize = Math.Min(maxFontSize <= 0 ? label.Font.Size : maxFontSize, label.Font.Size);
            bool fits = false;

            using (var g = label.CreateGraphics())
            {
                for (float size = testSize; size >= minFontSize; size -= 1f)
                {
                    using (var testFont = new Font(label.Font.FontFamily, size, label.Font.Style))
                    {
                        var measured = g.MeasureString(text, testFont);
                        if (measured.Width <= availableWidth)
                        {
                            testSize = size;
                            fits = true;
                            break;
                        }
                    }
                }
                if (!fits) testSize = minFontSize;
            }

            if (Math.Abs(label.Font.Size - testSize) > 0.1f)
                label.Font = new Font(label.Font.FontFamily, testSize, label.Font.Style);
        }

        // Designer event handlers preserved (empty)
        private void lostBorderPanel2_Click(object sender, EventArgs e) { }
        private void lostBorderPanel4_Click(object sender, EventArgs e) { }
        private void bigLabel1_Click(object sender, EventArgs e) { }
        private void label20_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void lostBorderPanel15_Click(object sender, EventArgs e) { }
        private void ChartLibraryCompletion_Click(object sender, EventArgs e) { }
        private void label26_Click(object sender, EventArgs e) { }
        private void label42_Click(object sender, EventArgs e) { }
        private void lostBorderPanel20_Click(object sender, EventArgs e) { }
    }
}
