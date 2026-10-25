using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;

namespace ComputerElectronicsPayrollSystem
{
    /// <summary>
    /// Applies centered cell/header alignment and reliable ascending/descending
    /// column-header sorting to the application's grids. Supports DataTable /
    /// DataView-bound grids and manually populated grids.
    /// </summary>
    public static class DataGridViewSortingHelper
    {
        private static readonly ConditionalWeakTable<DataGridView, GridState> States =
            new ConditionalWeakTable<DataGridView, GridState>();

        public static void Configure(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed)
                return;

            GridState state = GetState(grid);
            state.SortingEnabled = true;
            EnsureHandlers(grid, state);
            ApplyVisualStyles(grid, state);
            EnsureSortMenu(grid, state);
        }

        /// <summary>
        /// Reapplies centered alignment and sorting mode after another method
        /// has applied per-column alignment or formatting.
        /// </summary>
        public static void ApplyStyles(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed)
                return;

            GridState state = GetState(grid);
            EnsureHandlers(grid, state);
            ApplyVisualStyles(grid, state);
            UpdateSortGlyphs(grid, state);
        }

        /// <summary>
        /// True while a header sort is underway. Selection handlers use this to
        /// avoid accidentally changing the payroll/attendance entry fields while
        /// the grid's bound rows are being reordered.
        /// </summary>
        public static bool IsSorting(DataGridView grid)
        {
            return grid != null &&
                   States.TryGetValue(grid, out GridState? state) &&
                   state.IsSorting;
        }

        /// <summary>
        /// Reapplies the selected sort after manually populated grids are rebuilt.
        /// Bound grids use their DataView's sort expression.
        /// </summary>
        public static void ApplyCurrentSort(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed || grid.Columns.Count == 0)
                return;

            if (!States.TryGetValue(grid, out GridState? state) ||
                !state.SortingEnabled ||
                string.IsNullOrWhiteSpace(state.SortColumnKey) ||
                state.IsSorting)
            {
                return;
            }

            int columnIndex = FindColumnIndex(grid, state.SortColumnKey);
            if (columnIndex < 0)
            {
                ClearSortState(grid, state);
                return;
            }

            if (PerformSort(grid, columnIndex, state.Direction, state))
                UpdateSortGlyphs(grid, state);
        }


        private static void EnsureSortMenu(DataGridView grid, GridState state)
        {
            if (state.SortMenuInstalled) return;
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem dateMenu = new ToolStripMenuItem("Date Modified");
            dateMenu.DropDownItems.Add("Newest to Oldest", null, (_, __) => SortByDate(grid, SortOrder.Descending));
            dateMenu.DropDownItems.Add("Oldest to Newest", null, (_, __) => SortByDate(grid, SortOrder.Ascending));
            ToolStripMenuItem alphaMenu = new ToolStripMenuItem("Alphabetical");
            alphaMenu.DropDownItems.Add("A to Z", null, (_, __) => SortByEmployeeName(grid, SortOrder.Ascending));
            alphaMenu.DropDownItems.Add("Z to A", null, (_, __) => SortByEmployeeName(grid, SortOrder.Descending));
            menu.Items.Add(dateMenu);
            menu.Items.Add(alphaMenu);
            grid.ContextMenuStrip = menu;

            // Keep the menu discoverable: add a visible SORT BY button just
            // above the grid instead of requiring users to know about right-click.
            Control host = grid.Parent;
            Button sortButton = new Button
            {
                Name = "btnSortBy_" + grid.Name,
                Text = "SORT BY  ▼",
                Width = (grid.Name == "dgvEmployees" || grid.Name == "dgvRecentPayroll" || grid.Name == "dgvDetails" || grid.Name == "dgvAttendance") ? 88 : 112,
                Height = 26,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 108, 223),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            // Compact, restrained outline on the Dashboard and Employees sorter.
            // Other grids retain their existing borderless style.
            bool compactSorter = grid.Name == "dgvEmployees" || grid.Name == "dgvRecentPayroll" || grid.Name == "dgvDetails" || grid.Name == "dgvAttendance";
            sortButton.FlatAppearance.BorderSize = compactSorter ? 1 : 0;
            sortButton.FlatAppearance.BorderColor = Color.FromArgb(143, 160, 181);
            sortButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 91, 190);
            sortButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 72, 153);
            sortButton.Padding = new Padding(2, 0, 2, 0);
            sortButton.Click += (_, __) => menu.Show(sortButton, 0, sortButton.Height);
            host.Controls.Add(sortButton);
            sortButton.BringToFront();

            Action reposition = () =>
            {
                if (grid.IsDisposed || sortButton.IsDisposed) return;
                sortButton.Left = Math.Max(0, grid.Right - sortButton.Width);

                // Attendance keeps its Sort By control on the same toolbar
                // line as Search. Other grids retain the standard position
                // immediately above their grid.
                if (grid.Name == "dgvAttendance")
                {
                    Control searchPanel = host.Controls
                        .Find("pnlSearch", true)
                        .FirstOrDefault();

                    Control? searchTextBox = host.Controls.Find("txtSearch", true).FirstOrDefault();
                    sortButton.Top = searchTextBox != null
                        ? searchTextBox.Top + searchTextBox.Parent!.Top + (searchTextBox.Height - sortButton.Height) / 2
                        : searchPanel != null
                            ? searchPanel.Top + (searchPanel.Height - sortButton.Height) / 2
                            : Math.Max(0, grid.Top - sortButton.Height - 4);
                }
                else
                {
                    sortButton.Top = Math.Max(0, grid.Top - sortButton.Height - 4);
                }

                sortButton.BringToFront();
            };
            grid.LocationChanged += (_, __) => reposition();
            grid.SizeChanged += (_, __) => reposition();
            host.Resize += (_, __) => reposition();
            grid.ParentChanged += (_, __) =>
            {
                if (grid.Parent != null && sortButton.Parent != grid.Parent)
                {
                    sortButton.Parent.Controls.Remove(sortButton);
                    grid.Parent.Controls.Add(sortButton);
                    sortButton.BringToFront();
                    reposition();
                }
            };
            reposition();
            state.SortMenuInstalled = true;
        }

        private static void SortByDate(DataGridView grid, SortOrder direction)
        {
            // Prefer explicit creation timestamps. Never use a user-entered
            // attendance/pay date as a substitute for when the record was added.
            int index = FindColumnByTerms(grid, new[] { "createdat", "createdon", "createddate", "creationtimestamp", "timestamp", "datemodified", "modifieddate" });

            // Some compact views (notably Dashboard Recent Payroll) intentionally
            // hide the record ID. Their hidden key preserves record insertion order.
            if (index < 0)
                index = FindColumnByTerms(grid, new[] { "creationorderkey", "recordid", "payrollid", "attendanceid", "employeeid", "id" });

            if (index >= 0)
            {
                ApplyMenuSort(grid, index, direction);
                return;
            }

            MessageBox.Show(
                "This table does not expose a creation timestamp or stable record ID for sorting.",
                "Sort", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void SortByEmployeeName(DataGridView grid, SortOrder direction)
        {
            int index = FindColumnByTerms(grid, new[] { "employeename", "fullname", "name" });
            if (index < 0) index = FindColumnByTerms(grid, new[] { "firstname", "first name" });
            if (index >= 0) ApplyMenuSort(grid, index, direction);
            else MessageBox.Show("This table does not contain an Employee Name column to sort.", "Sort", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static int FindColumnByTerms(DataGridView grid, string[] terms)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                DataGridViewColumn c = grid.Columns[i];
                string identity = ((c.DataPropertyName ?? "") + " " + (c.Name ?? "") + " " + (c.HeaderText ?? ""))
                    .Replace(" ", "").Replace("_", "").Replace("-", "").ToLowerInvariant();
                foreach (string term in terms)
                    if (identity.Contains(term.Replace(" ", "").ToLowerInvariant())) return i;
            }
            return -1;
        }

        private static void ApplyMenuSort(DataGridView grid, int index, SortOrder direction)
        {
            if (index < 0 || index >= grid.Columns.Count) return;
            GridState state = GetState(grid);
            state.SortColumnKey = GetColumnKey(grid.Columns[index]);
            state.Direction = direction;
            if (!PerformSort(grid, index, direction, state))
            {
                // Bound grids with a sortable DataView are handled by PerformSort;
                // leave the current order unchanged if the source cannot be sorted safely.
                MessageBox.Show("This table's data source does not support sorting this column.", "Sort", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateSortGlyphs(grid, state);
        }

        private static GridState GetState(DataGridView grid)
        {
            return States.GetValue(grid, _ => new GridState());
        }

        private static void EnsureHandlers(DataGridView grid, GridState state)
        {
            if (state.HandlersInstalled)
                return;

            grid.ColumnHeaderMouseClick += (_, e) =>
                HandleColumnHeaderMouseClick(grid, e.ColumnIndex);

            grid.DataBindingComplete += (_, _) =>
                HandleDataBindingComplete(grid);

            grid.ColumnAdded += (_, e) =>
                ConfigureColumn(e.Column, state);

            state.HandlersInstalled = true;
        }

        private static void HandleDataBindingComplete(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed || grid.Columns.Count == 0)
                return;

            GridState state = GetState(grid);
            ApplyVisualStyles(grid, state);

            // Sorting a DataView sends a binding reset. Do not start another sort
            // while the first sort is restoring its selected row.
            if (state.IsSorting)
            {
                UpdateSortGlyphs(grid, state);
                return;
            }

            if (!state.SortingEnabled || string.IsNullOrWhiteSpace(state.SortColumnKey))
                return;

            int columnIndex = FindColumnIndex(grid, state.SortColumnKey);
            if (columnIndex < 0)
            {
                ClearSortState(grid, state);
                return;
            }

            ApplyCurrentSort(grid);
        }

        private static void ApplyVisualStyles(DataGridView grid, GridState state)
        {
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn column in grid.Columns)
                ConfigureColumn(column, state);
        }

        private static void ConfigureColumn(DataGridViewColumn column, GridState state)
        {
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.SortMode = state.SortingEnabled
                ? DataGridViewColumnSortMode.Programmatic
                : DataGridViewColumnSortMode.NotSortable;
        }

        private static void HandleColumnHeaderMouseClick(
            DataGridView grid,
            int columnIndex)
        {
            if (grid == null || grid.IsDisposed ||
                columnIndex < 0 || columnIndex >= grid.Columns.Count)
            {
                return;
            }

            GridState state = GetState(grid);
            if (!state.SortingEnabled || state.IsSorting)
                return;

            DataGridViewColumn column = grid.Columns[columnIndex];
            string key = GetColumnKey(column);
            if (string.IsNullOrWhiteSpace(key))
                return;

            SortOrder nextDirection =
                string.Equals(state.SortColumnKey, key, StringComparison.OrdinalIgnoreCase) &&
                state.Direction == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;

            string? previousKey = state.SortColumnKey;
            SortOrder previousDirection = state.Direction;

            // Store the new state before the bound DataView raises its reset event.
            state.SortColumnKey = key;
            state.Direction = nextDirection;

            if (!PerformSort(grid, columnIndex, nextDirection, state))
            {
                state.SortColumnKey = previousKey;
                state.Direction = previousDirection;
                UpdateSortGlyphs(grid, state);
                return;
            }

            UpdateSortGlyphs(grid, state);
        }

        private static bool PerformSort(
            DataGridView grid,
            int columnIndex,
            SortOrder direction,
            GridState state)
        {
            if (columnIndex < 0 || columnIndex >= grid.Columns.Count || state.IsSorting)
                return false;

            SelectionSnapshot selection = CaptureSelection(grid);
            state.IsSorting = true;

            try
            {
                DataGridViewColumn column = grid.Columns[columnIndex];
                bool sorted = TrySortBoundSource(grid, column, direction);

                if (!sorted && grid.DataSource == null)
                    sorted = TrySortUnboundGrid(grid, columnIndex, direction);

                if (sorted)
                    RestoreSelection(grid, selection);

                return sorted;
            }
            finally
            {
                state.IsSorting = false;
            }
        }

        private static SelectionSnapshot CaptureSelection(DataGridView grid)
        {
            DataGridViewRow? currentRow = grid.CurrentRow;
            if (currentRow == null || currentRow.IsNewRow)
                return new SelectionSnapshot();

            object? boundItem = currentRow.DataBoundItem;
            DataRow? dataRow = (boundItem as DataRowView)?.Row;
            string? currentColumnKey = grid.CurrentCell == null
                ? null
                : GetColumnKey(grid.Columns[grid.CurrentCell.ColumnIndex]);

            return new SelectionSnapshot
            {
                HadCurrentRow = true,
                DataRow = dataRow,
                BoundItem = boundItem,
                UnboundRow = grid.DataSource == null ? currentRow : null,
                CurrentColumnKey = currentColumnKey,
                CurrentColumnIndex = grid.CurrentCell?.ColumnIndex ?? 0
            };
        }

        private static void RestoreSelection(
            DataGridView grid,
            SelectionSnapshot snapshot)
        {
            try
            {
                grid.ClearSelection();

                if (!snapshot.HadCurrentRow)
                {
                    grid.CurrentCell = null;
                    return;
                }

                DataGridViewRow? matchingRow = null;

                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (snapshot.DataRow != null &&
                        row.DataBoundItem is DataRowView rowView &&
                        ReferenceEquals(rowView.Row, snapshot.DataRow))
                    {
                        matchingRow = row;
                        break;
                    }

                    if (snapshot.DataRow == null && snapshot.UnboundRow != null &&
                        ReferenceEquals(row, snapshot.UnboundRow))
                    {
                        matchingRow = row;
                        break;
                    }

                    if (snapshot.DataRow == null && snapshot.UnboundRow == null &&
                        snapshot.BoundItem != null &&
                        ReferenceEquals(row.DataBoundItem, snapshot.BoundItem))
                    {
                        matchingRow = row;
                        break;
                    }
                }

                if (matchingRow == null)
                {
                    grid.CurrentCell = null;
                    return;
                }

                int columnIndex = -1;
                if (!string.IsNullOrWhiteSpace(snapshot.CurrentColumnKey))
                    columnIndex = FindColumnIndex(grid, snapshot.CurrentColumnKey);

                if (columnIndex < 0 || columnIndex >= matchingRow.Cells.Count ||
                    !grid.Columns[columnIndex].Visible)
                {
                    columnIndex = snapshot.CurrentColumnIndex;
                }

                if (columnIndex < 0 || columnIndex >= matchingRow.Cells.Count ||
                    !grid.Columns[columnIndex].Visible)
                {
                    columnIndex = -1;
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        if (grid.Columns[i].Visible)
                        {
                            columnIndex = i;
                            break;
                        }
                    }
                }

                if (columnIndex >= 0)
                    grid.CurrentCell = matchingRow.Cells[columnIndex];

                matchingRow.Selected = true;
            }
            catch (InvalidOperationException)
            {
                // A closing/rebinding grid should not interrupt sorting.
            }
            catch (ArgumentOutOfRangeException)
            {
                // A column can be removed while report types are switching.
            }
        }

        private sealed class SelectionSnapshot
        {
            public bool HadCurrentRow { get; set; }
            public DataRow? DataRow { get; set; }
            public object? BoundItem { get; set; }
            public DataGridViewRow? UnboundRow { get; set; }
            public string? CurrentColumnKey { get; set; }
            public int CurrentColumnIndex { get; set; }
        }

        private static bool TrySortBoundSource(
            DataGridView grid,
            DataGridViewColumn column,
            SortOrder direction)
        {
            DataView? view = GetDataView(grid.DataSource);
            if (view == null || view.Table == null)
                return false;

            DataColumn? dataColumn = ResolveDataColumn(view.Table, column);
            if (dataColumn == null)
                return false;

            string directionText = direction == SortOrder.Descending ? "DESC" : "ASC";
            string escapedColumnName = "[" +
                dataColumn.ColumnName.Replace("]", "]]", StringComparison.Ordinal) + "]";
            string desiredSort = escapedColumnName + " " + directionText;

            if (!string.Equals(view.Sort, desiredSort, StringComparison.OrdinalIgnoreCase))
                view.Sort = desiredSort;

            return true;
        }

        private static DataView? GetDataView(object? source)
        {
            if (source is DataView view)
                return view;

            if (source is DataTable table)
                return table.DefaultView;

            if (source is BindingSource bindingSource)
            {
                if (bindingSource.List is DataView listView)
                    return listView;

                if (bindingSource.DataSource is DataView sourceView)
                    return sourceView;

                if (bindingSource.DataSource is DataTable sourceTable)
                    return sourceTable.DefaultView;
            }

            return null;
        }

        private static DataColumn? ResolveDataColumn(
            DataTable table,
            DataGridViewColumn column)
        {
            string[] candidates =
            {
                column.DataPropertyName ?? string.Empty,
                column.Name ?? string.Empty,
                column.HeaderText ?? string.Empty
            };

            foreach (string candidate in candidates)
            {
                if (string.IsNullOrWhiteSpace(candidate))
                    continue;

                foreach (DataColumn dataColumn in table.Columns)
                {
                    if (string.Equals(
                            dataColumn.ColumnName,
                            candidate,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        return dataColumn;
                    }
                }
            }

            return null;
        }

        private static bool TrySortUnboundGrid(
            DataGridView grid,
            int columnIndex,
            SortOrder direction)
        {
            try
            {
                grid.Sort(new GridRowComparer(
                    columnIndex,
                    direction == SortOrder.Descending));
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static void UpdateSortGlyphs(DataGridView grid, GridState state)
        {
            foreach (DataGridViewColumn column in grid.Columns)
                column.HeaderCell.SortGlyphDirection = SortOrder.None;

            if (string.IsNullOrWhiteSpace(state.SortColumnKey))
                return;

            int sortedColumnIndex = FindColumnIndex(grid, state.SortColumnKey);
            if (sortedColumnIndex >= 0)
                grid.Columns[sortedColumnIndex].HeaderCell.SortGlyphDirection = state.Direction;
        }

        private static int FindColumnIndex(DataGridView grid, string key)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                DataGridViewColumn column = grid.Columns[i];
                if (string.Equals(column.DataPropertyName, key, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(column.Name, key, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(GetColumnKey(column), key, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private static string GetColumnKey(DataGridViewColumn column)
        {
            return !string.IsNullOrWhiteSpace(column.DataPropertyName)
                ? column.DataPropertyName
                : column.Name;
        }

        private static void ClearSortState(DataGridView grid, GridState state)
        {
            state.SortColumnKey = null;
            state.Direction = SortOrder.None;
            foreach (DataGridViewColumn column in grid.Columns)
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
        }

        private sealed class GridState
        {
            public bool SortingEnabled { get; set; } = true;
            public bool HandlersInstalled { get; set; }
            public bool SortMenuInstalled { get; set; }
            public bool IsSorting { get; set; }
            public string? SortColumnKey { get; set; }
            public SortOrder Direction { get; set; } = SortOrder.None;
        }

        private sealed class GridRowComparer : IComparer
        {
            private readonly int columnIndex;
            private readonly bool descending;

            public GridRowComparer(int columnIndex, bool descending)
            {
                this.columnIndex = columnIndex;
                this.descending = descending;
            }

            public int Compare(object? x, object? y)
            {
                if (ReferenceEquals(x, y))
                    return 0;

                if (x is not DataGridViewRow leftRow)
                    return -1;

                if (y is not DataGridViewRow rightRow)
                    return 1;

                if (columnIndex >= leftRow.Cells.Count || columnIndex >= rightRow.Cells.Count)
                    return 0;

                int result = CompareValues(
                    leftRow.Cells[columnIndex].Value,
                    rightRow.Cells[columnIndex].Value);

                return descending ? -result : result;
            }
        }

        private static int CompareValues(object? left, object? right)
        {
            bool leftMissing = left == null || left == DBNull.Value;
            bool rightMissing = right == null || right == DBNull.Value;

            if (leftMissing && rightMissing)
                return 0;
            if (leftMissing)
                return -1;
            if (rightMissing)
                return 1;

            if (left is DateTime leftDate && right is DateTime rightDate)
                return leftDate.CompareTo(rightDate);

            if (IsNumericType(left!) && IsNumericType(right!))
            {
                try
                {
                    decimal leftNumber = Convert.ToDecimal(left, CultureInfo.InvariantCulture);
                    decimal rightNumber = Convert.ToDecimal(right, CultureInfo.InvariantCulture);
                    return leftNumber.CompareTo(rightNumber);
                }
                catch
                {
                    // Fall through to string-aware comparison.
                }
            }

            string leftText = Convert.ToString(left, CultureInfo.CurrentCulture) ?? string.Empty;
            string rightText = Convert.ToString(right, CultureInfo.CurrentCulture) ?? string.Empty;

            if (TryParseDateText(leftText, out DateTime leftParsedDate) &&
                TryParseDateText(rightText, out DateTime rightParsedDate))
            {
                return leftParsedDate.CompareTo(rightParsedDate);
            }

            if (TryParseNumberText(leftText, out decimal leftParsedNumber) &&
                TryParseNumberText(rightText, out decimal rightParsedNumber))
            {
                return leftParsedNumber.CompareTo(rightParsedNumber);
            }

            return string.Compare(
                leftText,
                rightText,
                StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool IsNumericType(object value)
        {
            TypeCode code = Type.GetTypeCode(value.GetType());
            return code == TypeCode.Byte || code == TypeCode.SByte ||
                   code == TypeCode.Int16 || code == TypeCode.UInt16 ||
                   code == TypeCode.Int32 || code == TypeCode.UInt32 ||
                   code == TypeCode.Int64 || code == TypeCode.UInt64 ||
                   code == TypeCode.Decimal || code == TypeCode.Double ||
                   code == TypeCode.Single;
        }

        private static bool TryParseDateText(string value, out DateTime parsed)
        {
            parsed = default;
            string text = value.Trim();
            bool looksLikeDateOrTime =
                text.Contains('/') || text.Contains(':') ||
                (text.Length >= 10 && char.IsDigit(text[0]) &&
                 text[4] == '-' && text[7] == '-');

            if (!looksLikeDateOrTime)
                return false;

            return DateTime.TryParse(
                       text,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.AllowWhiteSpaces,
                       out parsed) ||
                   DateTime.TryParse(
                       text,
                       CultureInfo.CurrentCulture,
                       DateTimeStyles.AllowWhiteSpaces,
                       out parsed);
        }

        private static bool TryParseNumberText(string value, out decimal parsed)
        {
            string cleaned = value.Trim()
                .Replace("₱", string.Empty, StringComparison.Ordinal)
                .Replace("$", string.Empty, StringComparison.Ordinal)
                .Replace("€", string.Empty, StringComparison.Ordinal)
                .Replace("£", string.Empty, StringComparison.Ordinal)
                .Replace(",", string.Empty, StringComparison.Ordinal)
                .Replace(" ", string.Empty, StringComparison.Ordinal);

            return decimal.TryParse(
                cleaned,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out parsed);
        }
    }
}
