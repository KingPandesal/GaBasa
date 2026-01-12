using LMS.DataAccess.Repositories;
using LMS.Model.Models.Catalog;
using LMS.Model.Models.Users;
using LMS.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LMS.BusinessLogic.Services.BarcodeGenerator;
using LMS.Presentation.BarcodeGenerator;
using ZXing;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class ViewBookCopy : Form
    {
        private readonly BookCopyRepository _bookCopyRepo;
        private readonly UserRepository _userRepo;
        private readonly Dictionary<int, string> _userNameCache = new Dictionary<int, string>();

        // Staging
        private readonly Dictionary<int, BookCopy> _modifiedCopies = new Dictionary<int, BookCopy>(); // existing copies edited (CopyID -> copy)
        private readonly List<BookCopy> _newCopies = new List<BookCopy>(); // newly added, not yet saved (CopyID == 0)
        private readonly HashSet<int> _deletedCopyIds = new HashSet<int>(); // CopyIDs staged for deletion (existing only)

        private int _bookId;
        private string _bookTitle;
        private List<BookCopy> _allCopies = new List<BookCopy>();
        private List<BookCopy> _filteredCopies = new List<BookCopy>();

        public ViewBookCopy() : this(0, null) { }

        public ViewBookCopy(int bookId, string bookTitle = null)
        {
            InitializeComponent();

            _bookCopyRepo = new BookCopyRepository();
            _userRepo = new UserRepository();

            _bookId = bookId;
            _bookTitle = bookTitle;

            // Ensure button text
            if (DgwBookCopy.Columns.Contains("ColumnBarcode"))
            {
                var col = DgwBookCopy.Columns["ColumnBarcode"] as DataGridViewButtonColumn;
                if (col != null) col.UseColumnTextForButtonValue = true;
            }

            // Wire events
            this.Load -= ViewBookCopy_Load;
            this.Load += ViewBookCopy_Load;

            BtnApply.Click -= BtnApply_Click;
            BtnApply.Click += BtnApply_Click;

            BtnSave.Click -= BtnSave_Click;
            BtnSave.Click += BtnSave_Click;

            LblCancel.Click -= LblCancel_Click;
            LblCancel.Click += LblCancel_Click;

            BtnAddBookCopy.Click -= BtnAddBookCopy_Click;
            BtnAddBookCopy.Click += BtnAddBookCopy_Click;

            DgwBookCopy.CellContentClick -= DgwBookCopy_CellContentClick;
            DgwBookCopy.CellContentClick += DgwBookCopy_CellContentClick;

            // Wire search bar event for real-time filtering
            TxtSearchBar.TextChanged -= TxtSearchBar_TextChanged;
            TxtSearchBar.TextChanged += TxtSearchBar_TextChanged;

            if (!string.IsNullOrWhiteSpace(_bookTitle))
                this.Text = $"View book copy - {_bookTitle}";
        }

        private void ViewBookCopy_Load(object sender, EventArgs e) => LoadCopies();

        private void LoadCopies()
        {
            try
            {
                if (_bookId <= 0) _allCopies = new List<BookCopy>();
                else _allCopies = _bookCopyRepo.GetByBookId(_bookId) ?? new List<BookCopy>();

                // Merge staged new copies into view
                foreach (var n in _newCopies)
                {
                    if (!_allCopies.Any(x => string.Equals(x.AccessionNumber, n.AccessionNumber, StringComparison.OrdinalIgnoreCase)))
                        _allCopies.Add(n);
                }

                // Clear staging for modified/deleted if reloading fresh
                _modifiedCopies.Clear();
                _deletedCopyIds.Clear();

                PopulateLocationAndStatusFilters();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load copies: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Populate both location and status filters using actual copy data (keeps UI consistent)
        private void PopulateLocationAndStatusFilters()
        {
            try
            {
                // Locations
                CmbBxLocationFilter.Items.Clear();
                CmbBxLocationFilter.Items.Add("All Location");

                var locs = _allCopies
                    .Select(c => SafePropertyString(() => c.Location))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(s => s);

                foreach (var l in locs) CmbBxLocationFilter.Items.Add(l);
                CmbBxLocationFilter.SelectedIndex = 0;
            }
            catch
            {
                CmbBxLocationFilter.Items.Clear();
                CmbBxLocationFilter.Items.Add("All Location");
                CmbBxLocationFilter.SelectedIndex = 0;
            }

            try
            {
                // Statuses: gather distinct statuses from copies
                CmbBxStatusFilter.Items.Clear();
                CmbBxStatusFilter.Items.Add("All Status");

                var statuses = _allCopies
                    .Select(c => SafePropertyString(() => c.Status))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(s => s);

                foreach (var s in statuses) CmbBxStatusFilter.Items.Add(s);

                CmbBxStatusFilter.SelectedIndex = 0;
            }
            catch
            {
                // fallback to simple set if population fails
                if (!CmbBxStatusFilter.Items.Contains("All Status")) CmbBxStatusFilter.Items.Insert(0, "All Status");
                if (!CmbBxStatusFilter.Items.Contains("Available")) CmbBxStatusFilter.Items.Add("Available");
                if (!CmbBxStatusFilter.Items.Contains("Borrowed")) CmbBxStatusFilter.Items.Add("Borrowed");
                if (!CmbBxStatusFilter.Items.Contains("Reserved")) CmbBxStatusFilter.Items.Add("Reserved");
                if (!CmbBxStatusFilter.Items.Contains("Lost")) CmbBxStatusFilter.Items.Add("Lost");
                if (!CmbBxStatusFilter.Items.Contains("Damaged")) CmbBxStatusFilter.Items.Add("Damaged");
                if (!CmbBxStatusFilter.Items.Contains("Repair")) CmbBxStatusFilter.Items.Add("Repair");
                CmbBxStatusFilter.SelectedIndex = 0;
            }
        }

        private void TxtSearchBar_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allCopies == null) _allCopies = new List<BookCopy>();

            var filtered = _allCopies.AsEnumerable();

            // Apply search filter (accession number, location, added by)
            string searchText = TxtSearchBar.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(c =>
                    SafePropertyString(() => c.AccessionNumber).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    SafePropertyString(() => c.Location).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    GetUserName(SafeProperty(() => c.AddedByID) as int? ?? 0).IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            string selectedLocation = CmbBxLocationFilter.SelectedItem?.ToString() ?? CmbBxLocationFilter.Text;
            if (!string.IsNullOrWhiteSpace(selectedLocation) && !selectedLocation.Equals("All Location", StringComparison.OrdinalIgnoreCase))
                filtered = filtered.Where(c => SafePropertyString(() => c.Location).Equals(selectedLocation, StringComparison.OrdinalIgnoreCase));

            string selectedStatus = CmbBxStatusFilter.SelectedItem?.ToString() ?? CmbBxStatusFilter.Text;
            if (!string.IsNullOrWhiteSpace(selectedStatus) && !selectedStatus.Equals("All Status", StringComparison.OrdinalIgnoreCase))
                filtered = filtered.Where(c => SafePropertyString(() => c.Status).Equals(selectedStatus, StringComparison.OrdinalIgnoreCase));

            // Exclude copies staged for deletion from the filtered view
            filtered = filtered.Where(c => !(c.CopyID > 0 && _deletedCopyIds.Contains(c.CopyID)));

            _filteredCopies = filtered.ToList();
            DisplayCopies(_filteredCopies);
            UpdateCopyCountLabels();
        }

        private void UpdateCopyCountLabels()
        {
            // Calculate total copies (excluding deleted)
            var activeCopies = _allCopies.Where(c => !(c.CopyID > 0 && _deletedCopyIds.Contains(c.CopyID))).ToList();
            int totalCopies = activeCopies.Count;

            // Calculate available for borrow (status = "Available")
            int availableForBorrow = activeCopies.Count(c =>
                SafePropertyString(() => c.Status).Equals("Available", StringComparison.OrdinalIgnoreCase));

            LblTotalCopies.Text = $"Total Copies: {totalCopies}";
            LblAvailableForBorrow.Text = $"Available for borrow: {availableForBorrow}";
        }

        private void DisplayCopies(List<BookCopy> copies)
        {
            DgwBookCopy.Rows.Clear();

            int index = 1;
            foreach (var c in copies)
            {
                int rowIndex = DgwBookCopy.Rows.Add();
                var row = DgwBookCopy.Rows[rowIndex];

                SetCellValue(row, "ColumnNumbering", index.ToString());
                SetCellValue(row, "ColumnAccessionNumber", SafePropertyString(() => c.AccessionNumber));
                SetCellValue(row, "ColumnLocation", SafePropertyString(() => c.Location));
                SetCellValue(row, "ColumnStatus", SafePropertyString(() => c.Status));

                var dateObj = SafeProperty(() => c.DateAdded);
                if (dateObj is DateTime dt && dt > DateTime.MinValue) SetCellValue(row, "ColumnDateAdded", dt.ToString("yyyy-MM-dd"));
                else SetCellValue(row, "ColumnDateAdded", string.Empty);

                var addedByName = GetUserName(SafeProperty(() => c.AddedByID) as int? ?? 0);
                SetCellValue(row, "ColumnAddedBy", addedByName);

                if (DgwBookCopy.Columns.Contains("ColumnBarcode"))
                {
                    var cell = row.Cells["ColumnBarcode"];
                    if (cell is DataGridViewButtonCell) cell.Value = "View Barcode";
                }

                // staged new -> highlight; staged modified -> highlight; staged delete -> red
                if (_newCopies.Contains(c))
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else if (c.CopyID > 0 && _modifiedCopies.ContainsKey(c.CopyID))
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else if (c.CopyID > 0 && _deletedCopyIds.Contains(c.CopyID))
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }

                row.Tag = c;
                index++;
            }
        }

        private void SetCellValue(DataGridViewRow row, string columnName, object value)
        {
            if (DgwBookCopy.Columns.Contains(columnName)) row.Cells[columnName].Value = value ?? string.Empty;
        }

        private object SafeProperty(Func<object> getter)
        {
            try { return getter(); } catch { return null; }
        }

        private string SafePropertyString(Func<object> getter)
        {
            try { var o = getter(); return o?.ToString() ?? string.Empty; } catch { return string.Empty; }
        }

        private string GetUserName(int userId)
        {
            if (userId <= 0) return string.Empty;
            if (_userNameCache.TryGetValue(userId, out string cached)) return cached ?? string.Empty;
            try
            {
                var user = _userRepo.GetById(userId);
                string name = user != null ? user.GetFullName() : string.Empty;
                _userNameCache[userId] = name;
                return name;
            }
            catch { _userNameCache[userId] = string.Empty; return string.Empty; }
        }

        private void BtnApply_Click(object sender, EventArgs e) => ApplyFilters();

        // Save: persist deletes -> updates -> adds, then close with OK to signal parent to refresh
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // If there are no staged changes, close with Cancel
            if (_deletedCopyIds.Count == 0 && _modifiedCopies.Count == 0 && _newCopies.Count == 0)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            int deleted = 0, updated = 0, added = 0, failed = 0;
            var errors = new List<string>();

            // Deletes
            foreach (var id in _deletedCopyIds.ToList())
            {
                try
                {
                    if (id <= 0) { /* shouldn't happen for persisted copy */ continue; }
                    if (_bookCopyRepo.DeleteByCopyId(id)) deleted++;
                    else { failed++; errors.Add($"Delete failed for CopyID={id}"); }
                }
                catch (Exception ex) { failed++; errors.Add($"Delete CopyID={id}: {ex.Message}"); }
            }

            // Updates
            foreach (var kv in _modifiedCopies.ToList())
            {
                try
                {
                    var copy = kv.Value;
                    if (copy.CopyID <= 0) continue; // ignore new copies in modified map
                    if (_bookCopyRepo.Update(copy)) updated++;
                    else { failed++; errors.Add($"Update failed for {copy?.AccessionNumber ?? $"CopyID={copy.CopyID}"}"); }
                }
                catch (Exception ex) { failed++; errors.Add($"Update {kv.Key}: {ex.Message}"); }
            }

            // Adds (new copies)
            foreach (var copy in _newCopies.ToList())
            {
                try
                {
                    int newId = _bookCopyRepo.Add(copy);
                    if (newId > 0) added++;
                    else { failed++; errors.Add($"Add failed for {copy.AccessionNumber}"); }
                }
                catch (Exception ex) { failed++; errors.Add($"Add {copy.AccessionNumber}: {ex.Message}"); }
            }

            // Show result and errors if any
            if (failed == 0)
            {
                MessageBox.Show($"Saved changes: {added} added, {updated} updated, {deleted} deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            // Partial or total failure: show summary and details
            var summary = $"Completed with results: {added} added, {updated} updated, {deleted} deleted. {failed} failed.";
            var detail = string.Join(Environment.NewLine, errors.Take(10)); // limit output
            MessageBox.Show($"{summary}\n\nErrors (sample):\n{detail}", "Partial Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            if (_newCopies.Count == 0 && _modifiedCopies.Count == 0 && _deletedCopyIds.Count == 0)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            var res = MessageBox.Show("There are unsaved changes. Save before closing?\nYes=Save and close, No=Discard, Cancel=Stay",
                "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (res == DialogResult.Yes) BtnSave_Click(this, EventArgs.Empty);
            else if (res == DialogResult.No)
            {
                // discard staged changes
                _newCopies.Clear();
                _modifiedCopies.Clear();
                _deletedCopyIds.Clear();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        // Add: open Add dialog (collect status/location), then create staged BookCopy with generated accession & barcode file
        private void BtnAddBookCopy_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new AddBookCopy(_bookId))
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;

                    // Determine addedBy id: prefer Program.CurrentUserId, fallback to first staff user
                    int addedBy = 0;
                    try { addedBy = Program.CurrentUserId; } catch { addedBy = 0; }
                    if (addedBy <= 0)
                    {
                        try
                        {
                            var staff = _userRepo.GetAllStaffUsers();
                            var first = staff?.FirstOrDefault();
                            if (first != null) addedBy = first.UserID;
                        }
                        catch { /* ignore */ }
                    }

                    if (addedBy <= 0)
                    {
                        MessageBox.Show("Unable to determine the current user. Please login or ensure a staff user exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Respect user-selected number of copies (minimum 1)
                    int copiesToCreate = Math.Max(1, dlg.SelectedCopies);

                    var dateAdded = DateTime.Now;
                    int created = 0;

                    for (int i = 0; i < copiesToCreate; i++)
                    {
                        // Generate unique accession for this book and date (method already ensures uniqueness against staged/new)
                        string accession = GenerateUniqueAccession(_bookId, dateAdded);

                        // Save barcode image (returns filename or null)
                        string barcodeRelPath = TrySaveBarcodeImage(accession);

                        // Build staged copy
                        var copy = new BookCopy
                        {
                            CopyID = 0, // not yet persisted
                            BookID = _bookId,
                            AccessionNumber = accession,
                            Status = dlg.SelectedStatus,
                            Location = dlg.SelectedLocation,
                            Barcode = barcodeRelPath,
                            DateAdded = dateAdded,
                            AddedByID = addedBy
                        };

                        // Stage it
                        _newCopies.Add(copy);
                        _allCopies.Add(copy);
                        created++;
                    }

                    // Refresh view and visually mark staged new
                    ApplyFilters();

                    if (created > 0)
                        MessageBox.Show($"{created} copy(ies) staged.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Generate accession in the requested format:
        // {prefix}-{bookId}-{year}-{unique4}  e.g. TH-33-2026-0001
        // Ensures uniqueness across DB copies and staged _newCopies.
        private string GenerateUniqueAccession(int bookId, DateTime dateAdded)
        {
            // Ask repo for a baseline (prefix-year-number) and then insert bookId
            string candidate = _bookCopyRepo.GenerateAccessionNumber(bookId, dateAdded);
            if (string.IsNullOrWhiteSpace(candidate)) candidate = $"BK-{dateAdded.Year}-0001";

            var parts = candidate.Split('-');
            string prefix = parts.Length >= 1 ? parts[0] : "BK";
            string year = parts.Length >= 2 ? parts[1] : dateAdded.Year.ToString();
            string numberPart = parts.Length >= 3 ? parts[parts.Length - 1] : "0001";

            string basePart = $"{prefix}-{bookId}-{year}";

            // Collect existing accession numbers from DB + staged list
            var existing = new HashSet<string>(_allCopies.Select(c => SafePropertyString(() => c.AccessionNumber)), StringComparer.OrdinalIgnoreCase);

            int suffix = 1;
            string tryAcc = $"{basePart}-{numberPart}";
            // If candidate collides, increment numeric suffix
            while (existing.Contains(tryAcc) || _newCopies.Any(n => string.Equals(n.AccessionNumber, tryAcc, StringComparison.OrdinalIgnoreCase)))
            {
                // derive highest existing suffix for this base
                var collisions = existing.Concat(_newCopies.Select(n => n.AccessionNumber ?? string.Empty))
                                         .Where(a => a.StartsWith(basePart + "-", StringComparison.OrdinalIgnoreCase))
                                         .Select(a =>
                                         {
                                             var p = a.Split('-');
                                             int v = 0;
                                             if (p.Length > 0 && int.TryParse(p.Last(), out v)) return v;
                                             return 0;
                                         }).ToList();
                suffix = collisions.Count == 0 ? 1 : collisions.Max() + 1;
                tryAcc = $"{basePart}-{suffix:D4}";
            }

            return tryAcc;
        }

        // Try to create a simple barcode image and save as PNG; return the FILENAME only (not the full relative path).
        // This avoids exceeding the DB column length for the `barcode` field.
        private string TrySaveBarcodeImage(string accession)
        {
            try
            {
                string folder = Path.Combine(Application.StartupPath, "Assets", "dataimages", "BookCopyBarcodes");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // Use ZXingBarcodeGenerator for real, scannable CODE_128 barcodes
                var barcodeGenerator = new ZXingBarcodeGenerator(
                    folder,
                    BarcodeFormat.CODE_128,
                    width: 300,
                    height: 100,
                    margin: 10);

                var results = barcodeGenerator.GenerateMany(new[] { accession });

                if (results.TryGetValue(accession, out string barcodePath) && !string.IsNullOrWhiteSpace(barcodePath))
                {
                    // Return only the filename to keep DB column small
                    return Path.GetFileName(barcodePath);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // DataGrid delete/edit handlers use staging: delete marks CopyID for deletion (or removes staged new)
        private void DgwBookCopy_CellContentClick_Handler(object sender, DataGridViewCellEventArgs e)
        {
            DgwBookCopy_CellContentClick(sender, e); // existing handler uses staged logic
        }

        // Existing DgwBookCopy_CellContentClick is already wired to handle staging (Edit/Add/Delete)
        private void DgwBookCopy_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var col = DgwBookCopy.Columns[e.ColumnIndex];
            var row = DgwBookCopy.Rows[e.RowIndex];
            if (row == null || col == null) return;

            var copy = row.Tag as BookCopy;
            if (copy == null) return;

            string colName = col.Name;

            if (colName == "ColumnBarcode")
            {
                string barcodeSource = SafePropertyString(() => copy.Barcode);
                string accession = SafePropertyString(() => copy.AccessionNumber);

                string toPass = null;
                if (!string.IsNullOrWhiteSpace(barcodeSource))
                {
                    try
                    {
                        // 1) If it's an absolute/relative path that exists, use it
                        if (File.Exists(barcodeSource))
                        {
                            toPass = barcodeSource;
                        }
                        else
                        {
                            // 2) Try as relative to application startup path (existing behavior)
                            var rel = Path.Combine(Application.StartupPath, barcodeSource.Replace('/', Path.DirectorySeparatorChar));
                            if (File.Exists(rel))
                            {
                                toPass = rel;
                            }
                            else
                            {
                                // 3) If barcodeSource looks like a filename, try the Assets folder
                                var assetsPath = Path.Combine(Application.StartupPath, "Assets", "dataimages", "BookCopyBarcodes", Path.GetFileName(barcodeSource));
                                if (File.Exists(assetsPath))
                                {
                                    toPass = assetsPath;
                                }
                                else
                                {
                                    // 4) Also try if stored value already contained the Assets prefix but with forward-slashes
                                    var rel2 = Path.Combine(Application.StartupPath, barcodeSource.TrimStart(Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar));
                                    if (File.Exists(rel2))
                                        toPass = rel2;
                                    else
                                        toPass = barcodeSource; // fallback to passing the raw string (viewer will render text)
                                }
                            }
                        }
                    }
                    catch
                    {
                        toPass = barcodeSource;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(accession))
                {
                    toPass = accession;
                }
                else
                {
                    toPass = string.Empty;
                }

                try
                {
                    using (var vb = new ViewBarcode())
                    {
                        vb.LoadBarcode(toPass, accession);
                        vb.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to show barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (colName == "Edit")
            {
                try
                {
                    using (var edit = new EditBookCopy(copy))
                    {
                        var dlg = edit.ShowDialog();
                        if (dlg == DialogResult.OK)
                        {
                            // Mark as modified (staged for save) using the same object reference
                            if (copy.CopyID > 0)
                                _modifiedCopies[copy.CopyID] = copy;

                            // Update grid row immediately to show in-memory changes
                            SetCellValue(row, "ColumnStatus", SafePropertyString(() => copy.Status));
                            SetCellValue(row, "ColumnLocation", SafePropertyString(() => copy.Location));
                            row.DefaultCellStyle.BackColor = Color.LightYellow;

                            // Update labels in case status changed
                            UpdateCopyCountLabels();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open edit: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (colName == "Delete")
            {
                try
                {
                    var confirm = MessageBox.Show("Delete this copy?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        // Staged delete: mark persisted copy for deletion or remove staged new
                        if (copy.CopyID > 0)
                            _deletedCopyIds.Add(copy.CopyID);
                        else
                            _newCopies.Remove(copy);

                        // Refresh UI
                        ApplyFilters();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
