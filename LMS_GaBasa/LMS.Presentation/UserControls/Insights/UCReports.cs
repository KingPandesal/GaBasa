using LMS.DataAccess.Database;
using LMS.DataAccess.Repositories;
using LMS.Model.Models.Catalog.Books;
using LMS.Model.Models.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        // Date range filter state
        private DateTime? _filterFrom;
        private DateTime? _filterTo;

        public UCReports()
        {
            InitializeComponent();

            // Wire up button click events
            BtnInventory.Click += BtnInventory_Click;
            BtnTransactions.Click += BtnTransactions_Click;
            BtnFines.Click += BtnFines_Click;
            BtnAuditLogs.Click += BtnAuditLogs_Click;

            // Wire date-range apply button (if present) or fallback to BtnExport
            WireDateRangeControls();

            // Set initial state - show Inventory by default
            ShowInventoryReport();
        }

        private void WireDateRangeControls()
        {
            try
            {
                // Prefer a control named "BtnApplyFromTo" if the designer added it
                var applyCtrl = this.Controls.Find("BtnApplyFromTo", true).FirstOrDefault() as Control;

                // If not present, use BtnExport as a fallback (so user can still filter)
                if (applyCtrl == null)
                    applyCtrl = this.Controls.Find("BtnExport", true).FirstOrDefault() as Control;

                if (applyCtrl != null)
                {
                    applyCtrl.Click -= ApplyDateRange_Click;
                    applyCtrl.Click += ApplyDateRange_Click;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WireDateRangeControls failed: " + ex);
            }
        }

        private void ApplyDateRange_Click(object sender, EventArgs e)
        {
            try
            {
                // Read values from designer DateTimePickers (DTPckFrom, DTPckTo)
                DateTime from, to;
                try
                {
                    from = DTPckFrom.Value.Date;
                    to = DTPckTo.Value.Date;
                }
                catch
                {
                    MessageBox.Show("Date pickers not found or invalid.", "Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (from > to)
                {
                    MessageBox.Show("The From date cannot be later than the To date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Normalize to include entire To day
                _filterFrom = from;
                _filterTo = to.Date.AddDays(1).AddTicks(-1);

                // Re-load currently visible report with the applied date range
                if (DgvInventory.Visible)
                    LoadInventory();
                else if (DgvTransactions.Visible)
                    LoadTransactions();
                else if (DgvFines.Visible)
                    LoadFines();
                else if (DgvAuditLogs.Visible)
                    LoadAuditLogs();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ApplyDateRange_Click failed: " + ex);
                MessageBox.Show("Failed to apply date filter. See debug output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            ShowInventoryReport();
        }

        private void BtnTransactions_Click(object sender, EventArgs e)
        {
            ShowTransactionsReport();
        }

        private void BtnFines_Click(object sender, EventArgs e)
        {
            ShowFinesReport();
        }

        private void BtnAuditLogs_Click(object sender, EventArgs e)
        {
            ShowAuditLogsReport();
        }

        private void ShowInventoryReport()
        {
            // Hide all grids
            HideAllGrids();

            // Load inventory from repository and populate grid
            LoadInventory();

            // Show only inventory grid
            DgvInventory.Visible = true;
            DgvInventory.BringToFront();

            // Update report label
            LblReport.Text = "Inventory Report";

            // Update button styles
            SetActiveButton(BtnInventory);
        }

        private void ShowTransactionsReport()
        {
            // Hide all grids
            HideAllGrids();

            // Load transactions from repository and populate grid
            LoadTransactions();

            // Show only transactions grid
            DgvTransactions.Visible = true;
            DgvTransactions.BringToFront();

            // Update report label
            LblReport.Text = "Transactions Report";

            // Update button styles
            SetActiveButton(BtnTransactions);
        }

        private void ShowFinesReport()
        {
            // Hide all grids
            HideAllGrids();

            // Load fines from repository and populate grid
            LoadFines();

            // Show only fines grid
            DgvFines.Visible = true;
            DgvFines.BringToFront();

            // Update report label
            LblReport.Text = "Fines Report";

            // Update button styles
            SetActiveButton(BtnFines);
        }

        private void ShowAuditLogsReport()
        {
            // Hide all grids
            HideAllGrids();

            // Ensure the audit logs grid contains columns for all AuditLog table fields.
            EnsureAuditLogColumnsExist();

            // Load audit logs from repository and populate grid
            LoadAuditLogs();

            // Show only audit logs grid
            DgvAuditLogs.Visible = true;
            DgvAuditLogs.BringToFront();

            // Update report label
            LblReport.Text = "Audit Logs Report";

            // Update button styles
            SetActiveButton(BtnAuditLogs);
        }

        private void HideAllGrids()
        {
            DgvInventory.Visible = false;
            DgvTransactions.Visible = false;
            DgvFines.Visible = false;
            DgvAuditLogs.Visible = false;
        }

        // Accept System.Windows.Forms.Control so both ReaLTaiizor.Controls.Button and standard Button work
        private void SetActiveButton(Control activeButton)
        {
            // Define active and inactive colors
            Color activeBackColor = Color.FromArgb(175, 37, 50);
            Color activeForeColor = Color.White;
            Color inactiveBackColor = Color.White;
            Color inactiveForeColor = Color.FromArgb(175, 37, 50);

            // Reset all buttons to inactive state (they are controls; safe to set BackColor/ForeColor)
            try
            {
                BtnInventory.BackColor = inactiveBackColor;
                BtnInventory.ForeColor = inactiveForeColor;
            }
            catch { }
            try
            {
                BtnTransactions.BackColor = inactiveBackColor;
                BtnTransactions.ForeColor = inactiveForeColor;
            }
            catch { }
            try
            {
                BtnFines.BackColor = inactiveBackColor;
                BtnFines.ForeColor = inactiveForeColor;
            }
            catch { }
            try
            {
                BtnAuditLogs.BackColor = inactiveBackColor;
                BtnAuditLogs.ForeColor = inactiveForeColor;
            }
            catch { }

            // Set active button style
            if (activeButton != null)
            {
                try
                {
                    activeButton.BackColor = activeBackColor;
                    activeButton.ForeColor = activeForeColor;
                }
                catch { }
            }
        }

        #region Load Inventory

        /// <summary>
        /// Loads all books from DB and populates DgvInventory.
        /// Columns: #, BookID, Accession No., Book Title, Category, Status, Date Added, Added By
        /// Honors _filterFrom/_filterTo when set (filters BookCopy.DateAdded).
        /// </summary>
        private void LoadInventory()
        {
            try
            {
                DgvInventory.Rows.Clear();

                var bookRepo = new BookRepository();
                var bookCopyRepo = new BookCopyRepository();
                var categoryRepo = new CategoryRepository();
                var userRepo = new UserRepository();

                List<Book> books = null;
                try
                {
                    books = bookRepo.GetAll() ?? new List<Book>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to retrieve books: " + ex);
                    MessageBox.Show("Failed to load inventory. See debug output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idx = 1;
                foreach (var book in books)
                {
                    try
                    {
                        // Get copies for this book to show accession, status, date added, added by
                        var copies = bookCopyRepo.GetByBookId(book.BookID) ?? new List<LMS.Model.Models.Catalog.BookCopy>();

                        // Get category name
                        string categoryName = "";
                        if (book.CategoryID > 0)
                        {
                            try
                            {
                                var cat = categoryRepo.GetById(book.CategoryID);
                                categoryName = cat?.Name ?? "";
                            }
                            catch { }
                        }

                        // If filtering by date, filter copies by DateAdded
                        if (_filterFrom.HasValue || _filterTo.HasValue)
                        {
                            copies = copies.Where(c =>
                            {
                                try
                                {
                                    var dt = c.DateAdded;
                                    if (_filterFrom.HasValue && dt < _filterFrom.Value) return false;
                                    if (_filterTo.HasValue && dt > _filterTo.Value) return false;
                                    return true;
                                }
                                catch { return false; }
                            }).ToList();
                        }

                        if (copies.Count == 0)
                        {
                            // Book with no copies (or none in range) - show one row with book info only
                            int r = DgvInventory.Rows.Add();
                            var row = DgvInventory.Rows[r];

                            if (DgvInventory.Columns.Contains("colNumberingInventory"))
                                row.Cells["colNumberingInventory"].Value = idx.ToString();
                            if (DgvInventory.Columns.Contains("colBookID"))
                                row.Cells["colBookID"].Value = book.BookID;
                            if (DgvInventory.Columns.Contains("colAccNo"))
                                row.Cells["colAccNo"].Value = "N/A";
                            if (DgvInventory.Columns.Contains("colTitle2"))
                                row.Cells["colTitle2"].Value = book.Title ?? "";
                            if (DgvInventory.Columns.Contains("colCategory"))
                                row.Cells["colCategory"].Value = categoryName;
                            if (DgvInventory.Columns.Contains("colStatus2"))
                                row.Cells["colStatus2"].Value = "No Copies";
                            if (DgvInventory.Columns.Contains("colDateAdded"))
                                row.Cells["colDateAdded"].Value = "";
                            if (DgvInventory.Columns.Contains("colAddedBy"))
                                row.Cells["colAddedBy"].Value = "";

                            row.Tag = book;
                            idx++;
                        }
                        else
                        {
                            // Show one row per copy
                            foreach (var copy in copies)
                            {
                                int r = DgvInventory.Rows.Add();
                                var row = DgvInventory.Rows[r];

                                if (DgvInventory.Columns.Contains("colNumberingInventory"))
                                    row.Cells["colNumberingInventory"].Value = idx.ToString();
                                if (DgvInventory.Columns.Contains("colBookID"))
                                    row.Cells["colBookID"].Value = book.BookID;
                                if (DgvInventory.Columns.Contains("colAccNo"))
                                    row.Cells["colAccNo"].Value = copy.AccessionNumber ?? "N/A";
                                if (DgvInventory.Columns.Contains("colTitle2"))
                                    row.Cells["colTitle2"].Value = book.Title ?? "";
                                if (DgvInventory.Columns.Contains("colCategory"))
                                    row.Cells["colCategory"].Value = categoryName;
                                if (DgvInventory.Columns.Contains("colStatus2"))
                                    row.Cells["colStatus2"].Value = copy.Status ?? "";
                                if (DgvInventory.Columns.Contains("colDateAdded"))
                                    row.Cells["colDateAdded"].Value = copy.DateAdded.ToString("MMM dd, yyyy");

                                // Added By - resolve username
                                string addedByDisplay = "";
                                if (copy.AddedByID > 0)
                                {
                                    try
                                    {
                                        var user = userRepo.GetById(copy.AddedByID);
                                        if (user != null)
                                        {
                                            string full = $"{user.FirstName} {user.LastName}".Trim();
                                            addedByDisplay = !string.IsNullOrWhiteSpace(full) ? full : user.Username ?? copy.AddedByID.ToString();
                                        }
                                        else
                                        {
                                            addedByDisplay = copy.AddedByID.ToString();
                                        }
                                    }
                                    catch
                                    {
                                        addedByDisplay = copy.AddedByID.ToString();
                                    }
                                }
                                if (DgvInventory.Columns.Contains("colAddedBy"))
                                    row.Cells["colAddedBy"].Value = addedByDisplay;

                                row.Tag = copy;
                                idx++;
                            }
                        }
                    }
                    catch (Exception exRow)
                    {
                        Debug.WriteLine("Failed to add inventory row: " + exRow);
                    }
                }

                // Scroll to top
                if (DgvInventory.Rows.Count > 0)
                {
                    try { DgvInventory.FirstDisplayedScrollingRowIndex = 0; } catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadInventory failed: " + ex);
            }
        }

        #endregion

        #region Load Transactions

        /// <summary>
        /// Loads all borrowing transactions from DB and populates DgvTransactions.
        /// Columns: #, TransactionID, Member Name, Book Title, Date Borrowed, Due Date, Date Returned, Status
        /// Honors _filterFrom/_filterTo when set (filters BorrowDate).
        /// </summary>
        private void LoadTransactions()
        {
            try
            {
                DgvTransactions.Rows.Clear();

                var dbConn = new DbConnection();
                List<TransactionReportRow> transactions = null;

                try
                {
                    transactions = GetAllBorrowingTransactions(dbConn, _filterFrom, _filterTo) ?? new List<TransactionReportRow>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to retrieve transactions: " + ex);
                    MessageBox.Show("Failed to load transactions. See debug output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idx = 1;
                foreach (var t in transactions.OrderByDescending(x => x.BorrowDate))
                {
                    try
                    {
                        int r = DgvTransactions.Rows.Add();
                        var row = DgvTransactions.Rows[r];

                        if (DgvTransactions.Columns.Contains("colNumberingTransactions"))
                            row.Cells["colNumberingTransactions"].Value = idx.ToString();
                        if (DgvTransactions.Columns.Contains("colTransactionID"))
                            row.Cells["colTransactionID"].Value = t.TransactionID;
                        if (DgvTransactions.Columns.Contains("colName"))
                            row.Cells["colName"].Value = t.MemberName ?? "";
                        if (DgvTransactions.Columns.Contains("colTitle"))
                            row.Cells["colTitle"].Value = t.BookTitle ?? "";
                        if (DgvTransactions.Columns.Contains("colDateBorrowed"))
                            row.Cells["colDateBorrowed"].Value = t.BorrowDate.ToString("MMM dd, yyyy");
                        if (DgvTransactions.Columns.Contains("colDueDate"))
                            row.Cells["colDueDate"].Value = t.DueDate.ToString("MMM dd, yyyy");
                        if (DgvTransactions.Columns.Contains("colDateReturned"))
                            row.Cells["colDateReturned"].Value = t.ReturnDate.HasValue ? t.ReturnDate.Value.ToString("MMM dd, yyyy") : "";
                        if (DgvTransactions.Columns.Contains("colStatus"))
                            row.Cells["colStatus"].Value = t.Status ?? "";

                        row.Tag = t;
                        idx++;
                    }
                    catch (Exception exRow)
                    {
                        Debug.WriteLine("Failed to add transaction row: " + exRow);
                    }
                }

                // Scroll to top
                if (DgvTransactions.Rows.Count > 0)
                {
                    try { DgvTransactions.FirstDisplayedScrollingRowIndex = 0; } catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadTransactions failed: " + ex);
            }
        }

        /// <summary>
        /// Helper class for transaction report data.
        /// </summary>
        private class TransactionReportRow
        {
            public int TransactionID { get; set; }
            public string MemberName { get; set; }
            public string BookTitle { get; set; }
            public DateTime BorrowDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
            public string Status { get; set; }
        }

        /// <summary>
        /// Gets all borrowing transactions with member name and book title.
        /// Accepts optional from/to to filter by BorrowDate.
        /// </summary>
        private List<TransactionReportRow> GetAllBorrowingTransactions(DbConnection db, DateTime? from, DateTime? to)
        {
            var result = new List<TransactionReportRow>();

            using (var conn = db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        bt.TransactionID,
                        ISNULL(u.FirstName, '') + ' ' + ISNULL(u.LastName, '') AS MemberName,
                        b.Title AS BookTitle,
                        bt.BorrowDate,
                        bt.DueDate,
                        bt.ReturnDate,
                        bt.[Status]
                    FROM [BorrowingTransaction] bt
                    INNER JOIN [Member] m ON bt.MemberID = m.MemberID
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    INNER JOIN [BookCopy] bc ON bt.CopyID = bc.CopyID
                    INNER JOIN [Book] b ON bc.BookID = b.BookID
                    WHERE 1 = 1");

                if (from.HasValue)
                    sql.Append(" AND bt.BorrowDate >= @From");
                if (to.HasValue)
                    sql.Append(" AND bt.BorrowDate <= @To");

                sql.Append(" ORDER BY bt.BorrowDate DESC");

                cmd.CommandText = sql.ToString();

                if (from.HasValue)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@From";
                    p.DbType = DbType.DateTime;
                    p.Value = from.Value;
                    cmd.Parameters.Add(p);
                }

                if (to.HasValue)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@To";
                    p.DbType = DbType.DateTime;
                    p.Value = to.Value;
                    cmd.Parameters.Add(p);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new TransactionReportRow
                        {
                            TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            MemberName = reader.IsDBNull(reader.GetOrdinal("MemberName")) ? "" : reader.GetString(reader.GetOrdinal("MemberName")).Trim(),
                            BookTitle = reader.IsDBNull(reader.GetOrdinal("BookTitle")) ? "" : reader.GetString(reader.GetOrdinal("BookTitle")),
                            BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }
                }
            }

            return result;
        }

        #endregion

        #region Load Fines

        /// <summary>
        /// Loads all fines from DB and populates DgvFines.
        /// Columns: #, FineID, Member Name, Fine Type, Amount, Status, Date Settled
        /// Honors _filterFrom/_filterTo when set (filters DateIssued).
        /// </summary>
        private void LoadFines()
        {
            try
            {
                DgvFines.Rows.Clear();

                var dbConn = new DbConnection();
                List<FineReportRow> fines = null;

                try
                {
                    fines = GetAllFines(dbConn, _filterFrom, _filterTo) ?? new List<FineReportRow>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to retrieve fines: " + ex);
                    MessageBox.Show("Failed to load fines. See debug output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idx = 1;
                foreach (var f in fines.OrderByDescending(x => x.DateIssued))
                {
                    try
                    {
                        int r = DgvFines.Rows.Add();
                        var row = DgvFines.Rows[r];

                        if (DgvFines.Columns.Contains("colNumberingFines"))
                            row.Cells["colNumberingFines"].Value = idx.ToString();
                        if (DgvFines.Columns.Contains("colFineID"))
                            row.Cells["colFineID"].Value = f.FineID;
                        if (DgvFines.Columns.Contains("colName2"))
                            row.Cells["colName2"].Value = f.MemberName ?? "";
                        if (DgvFines.Columns.Contains("colFineType"))
                            row.Cells["colFineType"].Value = f.FineType ?? "";
                        if (DgvFines.Columns.Contains("colAmount"))
                            row.Cells["colAmount"].Value = f.FineAmount.ToString("N2");
                        if (DgvFines.Columns.Contains("colPaymentStatus"))
                            row.Cells["colPaymentStatus"].Value = f.Status ?? "";
                        if (DgvFines.Columns.Contains("colDateSettled"))
                            row.Cells["colDateSettled"].Value = f.DateSettled.HasValue ? f.DateSettled.Value.ToString("MMM dd, yyyy") : "";

                        row.Tag = f;
                        idx++;
                    }
                    catch (Exception exRow)
                    {
                        Debug.WriteLine("Failed to add fine row: " + exRow);
                    }
                }

                // Scroll to top
                if (DgvFines.Rows.Count > 0)
                {
                    try { DgvFines.FirstDisplayedScrollingRowIndex = 0; } catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadFines failed: " + ex);
            }
        }

        /// <summary>
        /// Helper class for fine report data.
        /// </summary>
        private class FineReportRow
        {
            public int FineID { get; set; }
            public string MemberName { get; set; }
            public string FineType { get; set; }
            public decimal FineAmount { get; set; }
            public string Status { get; set; }
            public DateTime DateIssued { get; set; }
            public DateTime? DateSettled { get; set; }
        }

        /// <summary>
        /// Gets all fines with member name and payment date.
        /// Accepts optional from/to to filter by DateIssued.
        /// </summary>
        private List<FineReportRow> GetAllFines(DbConnection db, DateTime? from, DateTime? to)
        {
            var result = new List<FineReportRow>();

            using (var conn = db.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        f.FineID,
                        ISNULL(u.FirstName, '') + ' ' + ISNULL(u.LastName, '') AS MemberName,
                        f.FineType,
                        f.FineAmount,
                        f.[Status],
                        f.DateIssued,
                        p.PaymentDate AS DateSettled
                    FROM [Fine] f
                    INNER JOIN [Member] m ON f.MemberID = m.MemberID
                    INNER JOIN [User] u ON m.UserID = u.UserID
                    LEFT JOIN [Payment] p ON f.FineID = p.FineID
                    WHERE 1 = 1");

                if (from.HasValue)
                    sql.Append(" AND f.DateIssued >= @From");
                if (to.HasValue)
                    sql.Append(" AND f.DateIssued <= @To");

                sql.Append(" ORDER BY f.DateIssued DESC");

                cmd.CommandText = sql.ToString();

                if (from.HasValue)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@From";
                    p.DbType = DbType.DateTime;
                    p.Value = from.Value;
                    cmd.Parameters.Add(p);
                }

                if (to.HasValue)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@To";
                    p.DbType = DbType.DateTime;
                    p.Value = to.Value;
                    cmd.Parameters.Add(p);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new FineReportRow
                        {
                            FineID = reader.GetInt32(reader.GetOrdinal("FineID")),
                            MemberName = reader.IsDBNull(reader.GetOrdinal("MemberName")) ? "" : reader.GetString(reader.GetOrdinal("MemberName")).Trim(),
                            FineType = reader.IsDBNull(reader.GetOrdinal("FineType")) ? "" : reader.GetString(reader.GetOrdinal("FineType")),
                            FineAmount = reader.IsDBNull(reader.GetOrdinal("FineAmount")) ? 0m : reader.GetDecimal(reader.GetOrdinal("FineAmount")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "" : reader.GetString(reader.GetOrdinal("Status")),
                            DateIssued = reader.IsDBNull(reader.GetOrdinal("DateIssued")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateIssued")),
                            DateSettled = reader.IsDBNull(reader.GetOrdinal("DateSettled")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateSettled"))
                        });
                    }
                }
            }

            return result;
        }

        #endregion

        #region Load Audit Logs

        /// <summary>
        /// Ensure DgvAuditLogs contains columns for LogID, UserID (or User), ActionPerformed,
        /// Details, Timestamp and ModuleName. Adds the Details/UserID columns if missing.
        /// </summary>
        private void EnsureAuditLogColumnsExist()
        {
            try
            {
                // Designer already defines: colNumberingAuditLogs, colLogID, colUser, colAction, colTimestamp, colModule
                // Ensure a Details column exists (add it to the end if missing)
                var detailsColName = "colDetails";
                if (!DgvAuditLogs.Columns.Contains(detailsColName))
                {
                    var detailsCol = new DataGridViewTextBoxColumn
                    {
                        Name = detailsColName,
                        HeaderText = "Details",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        ReadOnly = true
                    };
                    DgvAuditLogs.Columns.Add(detailsCol);
                }

                // If colUser exists but is currently intended for username, it's fine.
                // We'll populate it with username when possible, otherwise show UserID.
            }
            catch
            {
                // Non-fatal - designer columns will still show most fields.
            }
        }

        /// <summary>
        /// Loads all audit logs from DB and populates DgvAuditLogs.
        /// Shows: LogID, User (username or user id), Action Performed, Details, Timestamp, Module.
        /// Honors _filterFrom/_filterTo when set (filters Timestamp).
        /// </summary>
        private void LoadAuditLogs()
        {
            try
            {
                DgvAuditLogs.Rows.Clear();

                var repo = new AuditLogRepository(new DbConnection());
                List<AuditLog> logs = null;

                try
                {
                    logs = repo.GetAll() ?? new List<AuditLog>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to retrieve audit logs: " + ex);
                    MessageBox.Show("Failed to load audit logs. See debug output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Attempt to resolve user display names if UserRepository is available (non-fatal)
                UserRepository userRepo = null;
                bool userRepoAvailable = false;
                try
                {
                    userRepo = new UserRepository();
                    userRepoAvailable = true;
                }
                catch
                {
                    userRepoAvailable = false;
                }

                int idx = 1;
                foreach (var log in logs.OrderByDescending(l => l.Timestamp))
                {
                    try
                    {
                        // Apply date filter if present
                        if (_filterFrom.HasValue && log.Timestamp < _filterFrom.Value) continue;
                        if (_filterTo.HasValue && log.Timestamp > _filterTo.Value) continue;

                        int r = DgvAuditLogs.Rows.Add();
                        var row = DgvAuditLogs.Rows[r];

                        // Numbering (designer column)
                        if (DgvAuditLogs.Columns.Contains("colNumberingAuditLogs"))
                            row.Cells["colNumberingAuditLogs"].Value = idx.ToString();

                        // LogID
                        if (DgvAuditLogs.Columns.Contains("colLogID"))
                            row.Cells["colLogID"].Value = log.LogID;

                        // User column: try to resolve username, fallback to UserID
                        string userDisplay = log.UserID.ToString();
                        if (userRepoAvailable)
                        {
                            try
                            {
                                var user = userRepo.GetById(log.UserID);
                                if (user != null)
                                {
                                    // Prefer full name when available
                                    string full = $"{user.FirstName} {user.LastName}".Trim();
                                    if (!string.IsNullOrWhiteSpace(full))
                                        userDisplay = full;
                                    else if (!string.IsNullOrWhiteSpace(user.Username))
                                        userDisplay = user.Username;
                                }
                            }
                            catch
                            {
                                // ignore resolution errors
                            }
                        }

                        if (DgvAuditLogs.Columns.Contains("colUser"))
                            row.Cells["colUser"].Value = userDisplay;

                        // Action Performed
                        if (DgvAuditLogs.Columns.Contains("colAction"))
                            row.Cells["colAction"].Value = log.ActionPerformed ?? string.Empty;

                        // Timestamp
                        if (DgvAuditLogs.Columns.Contains("colTimestamp"))
                            row.Cells["colTimestamp"].Value = log.Timestamp.ToString("MMM dd, yyyy hh:mm tt");

                        // Module
                        if (DgvAuditLogs.Columns.Contains("colModule"))
                            row.Cells["colModule"].Value = log.ModuleName ?? string.Empty;

                        // Details (programmatically added column)
                        if (DgvAuditLogs.Columns.Contains("colDetails"))
                            row.Cells["colDetails"].Value = log.Details ?? string.Empty;

                        row.Tag = log;
                        idx++;
                    }
                    catch (Exception exRow)
                    {
                        Debug.WriteLine("Failed to add audit log row: " + exRow);
                        // continue with next
                    }
                }

                // Scroll to top
                if (DgvAuditLogs.Rows.Count > 0)
                {
                    try
                    {
                        DgvAuditLogs.FirstDisplayedScrollingRowIndex = 0;
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadAuditLogs failed: " + ex);
            }
        }

        #endregion


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
