using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace ComputerElectronicsPayrollSystem
{
    public partial class ReportsControl : UserControl
    {
        private DataTable reportTable = new DataTable();
        private bool isLoadingReport = false;
        private bool isExporting = false;

        private static readonly string DatabasePath =
            Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "payroll.db");

        private static string ConnectionString =>
            $"Data Source={DatabasePath}";

        public ReportsControl()
        {
            InitializeComponent();

            ConfigureReports();

            // Reflow the filter toolbar and report table when the module is
            // hosted in a resized dashboard or on a smaller display.
            Resize += (_, _) => LayoutReportsContent();
            LayoutReportsContent();

            DataRefreshManager.DataChanged +=
                DataRefreshManager_DataChanged;

            Disposed +=
                ReportsControl_Disposed;

            GenerateReport(false);
        }

        // ============================================================
        // INITIALIZATION
        // ============================================================

        private void ConfigureReports()
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("Payroll Report");
            cmbReportType.Items.Add("Attendance Report");
            cmbReportType.Items.Add("Employee Report");
            cmbReportType.Items.Add("Payment Receipt Report");
            cmbReportType.SelectedIndex = 0;

            DateTime today = DateTime.Today;
            dtpFrom.Value =
                new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = today;

            // --------------------------------------------------------
            // REPORT TABLE APPEARANCE
            // --------------------------------------------------------
            dgvReport.ReadOnly = true;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.MultiSelect = false;
            dgvReport.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvReport.RowHeadersVisible = false;
            dgvReport.AutoGenerateColumns = true;
            dgvReport.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.FixedSingle;
            dgvReport.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReport.GridColor = Color.FromArgb(222, 229, 239);
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.ColumnHeadersHeight = 42;
            dgvReport.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReport.RowTemplate.Height = 32;

            dgvReport.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 41, 59);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;
            dgvReport.ColumnHeadersDefaultCellStyle.Padding =
                new Padding(5, 3, 5, 3);

            dgvReport.DefaultCellStyle.BackColor = Color.White;
            dgvReport.DefaultCellStyle.ForeColor =
                Color.FromArgb(31, 41, 55);
            dgvReport.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(37, 99, 235);
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReport.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvReport.DefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvReport.DefaultCellStyle.Padding =
                new Padding(5, 2, 5, 2);
            dgvReport.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvReport.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(246, 249, 253);
            dgvReport.AlternatingRowsDefaultCellStyle.ForeColor =
                Color.FromArgb(31, 41, 55);
            dgvReport.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                Color.FromArgb(37, 99, 235);
            dgvReport.AlternatingRowsDefaultCellStyle.SelectionForeColor =
                Color.White;

            // --------------------------------------------------------
            // FILTER FIELD APPEARANCE
            // --------------------------------------------------------
            cmbReportType.Font = new Font("Segoe UI", 9.5F);
            cmbReportType.BackColor = Color.White;
            cmbReportType.ForeColor = Color.FromArgb(31, 41, 55);
            cmbReportType.FlatStyle = FlatStyle.Flat;

            dtpFrom.Font = new Font("Segoe UI", 9F);
            dtpTo.Font = new Font("Segoe UI", 9F);

            txtSearch.Font = new Font("Segoe UI", 9.5F);
            txtSearch.BackColor = Color.White;
            txtSearch.ForeColor = Color.FromArgb(15, 23, 42);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            lblSummary.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(51, 65, 85);
            lblSummary.BackColor = Color.Transparent;

            StyleReportButton(
                btnGenerateReport,
                Color.FromArgb(37, 99, 235));
            StyleReportButton(
                btnExportCsv,
                Color.FromArgb(13, 148, 136));

            // Configure sortable headers and centered cell contents.
            DataGridViewSortingHelper.Configure(dgvReport);
            FuturisticUiTheme.Apply(this);

            btnGenerateReport.Click +=
                btnGenerateReport_Click;

            btnExportCsv.Click +=
                btnExportCsv_Click;

            cmbReportType.SelectedIndexChanged +=
                cmbReportType_SelectedIndexChanged;

            txtSearch.KeyDown +=
                txtSearch_KeyDown;

            btnSearch.Click +=
                (_, _) => GenerateReport(true);

            btnClearSearch.Click += (_, _) =>
            {
                txtSearch.Clear();
                GenerateReport(false);
                txtSearch.Focus();
            };

            dtpFrom.ValueChanged +=
                FilterDateChanged;

            dtpTo.ValueChanged +=
                FilterDateChanged;

            SetDateFilterState();
        }

        /// <summary>
        /// Keeps the report controls in a deliberate grid instead of relying
        /// on the Designer's fixed 964px coordinates. The filters stay on one
        /// line on a wide window and use two balanced rows when space is tight.
        /// </summary>
        private void LayoutReportsContent()
        {
            if (IsDisposed || ClientSize.Width <= 0 || pnlHeader == null ||
                pnlFilters == null || dgvReport == null)
                return;

            const int pageMargin = 22;
            int pageWidth = Math.Max(280, ClientSize.Width - pageMargin * 2);
            bool compact = pageWidth < 1020;

            pnlHeader.SetBounds(0, 0, ClientSize.Width, 78);
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblReportsTitle.Location = new Point(pageMargin, 7);
            lblReportsSubtitle.Location = new Point(pageMargin + 3, 49);

            int filterTop = 88;
            int filterHeight = compact ? 148 : 112;
            pnlFilters.SetBounds(pageMargin, filterTop, pageWidth, filterHeight);
            pnlFilters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            int left = 16;
            int top = 15;
            int rowGap = 38;
            int usable = Math.Max(240, pnlFilters.ClientSize.Width - 32);

            if (!compact)
            {
                lblReportType.SetBounds(left, top + 4, 82, 22);
                cmbReportType.SetBounds(left + 84, top, 180, 27);
                lblFrom.SetBounds(left + 280, top + 4, 38, 22);
                dtpFrom.SetBounds(left + 322, top + 1, 126, 25);
                lblTo.SetBounds(left + 462, top + 4, 24, 22);
                dtpTo.SetBounds(left + 491, top + 1, 126, 25);
                btnGenerateReport.SetBounds(left + 632, top - 2, 130, 32);
                btnExportCsv.SetBounds(left + 772, top - 2, 120, 32);
                lblSearch.SetBounds(left, top + rowGap + 1, 96, 22);
                int searchButtonWidth = 36;
                int clearButtonWidth = 72;
                int buttonGap = 4;
                int searchX = left + 100;
                int searchY = top + rowGap - 3;
                int availableSearchWidth = Math.Max(160, usable - 100 - searchButtonWidth - clearButtonWidth - buttonGap * 2);
                int searchWidth = Math.Max(180, (int)(availableSearchWidth * 0.70));
                txtSearch.SetBounds(searchX, searchY, searchWidth, 27);
                btnSearch.SetBounds(txtSearch.Right + buttonGap, searchY, searchButtonWidth, 27);
                btnClearSearch.SetBounds(btnSearch.Right + buttonGap, searchY, clearButtonWidth, 27);
            }
            else
            {
                int firstRowWidth = usable;
                int reportWidth = Math.Max(145, Math.Min(190, firstRowWidth / 3));
                lblReportType.SetBounds(left, top + 4, 82, 22);
                cmbReportType.SetBounds(left + 84, top, reportWidth, 27);

                int dateStart = left + 84 + reportWidth + 14;
                int remaining = Math.Max(250, pnlFilters.ClientSize.Width - dateStart - 14);
                int dateWidth = Math.Max(94, Math.Min(126, (remaining - 86) / 2));
                lblFrom.SetBounds(dateStart, top + 4, 38, 22);
                dtpFrom.SetBounds(dateStart + 40, top + 1, dateWidth, 25);
                lblTo.SetBounds(dateStart + 44 + dateWidth, top + 4, 24, 22);
                dtpTo.SetBounds(dateStart + 70 + dateWidth, top + 1, dateWidth, 25);

                int secondY = top + rowGap + 1;
                lblSearch.SetBounds(left, secondY + 1, 96, 22);
                int actionWidth = 112;
                btnExportCsv.SetBounds(pnlFilters.ClientSize.Width - actionWidth - 14, secondY - 3, actionWidth, 32);
                btnGenerateReport.SetBounds(btnExportCsv.Left - 126, secondY - 3, 118, 32);
                int searchX = left + 100;
                int searchButtonWidth = 36;
                int clearButtonWidth = 72;
                int buttonGap = 4;
                int searchRight = btnGenerateReport.Left - 12;
                int availableSearchWidth = Math.Max(150, searchRight - searchX - searchButtonWidth - clearButtonWidth - buttonGap * 2);
                int searchWidth = Math.Max(120, (int)(availableSearchWidth * 0.70));
                txtSearch.SetBounds(searchX, secondY - 3, searchWidth, 27);
                btnSearch.SetBounds(txtSearch.Right + buttonGap, secondY - 3, searchButtonWidth, 27);
                btnClearSearch.SetBounds(btnSearch.Right + buttonGap, secondY - 3, clearButtonWidth, 27);
            }

            lblSummary.SetBounds(pageMargin, filterTop + filterHeight + 7, pageWidth, 26);
            lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            int gridTop = lblSummary.Bottom + 5;
            dgvReport.SetBounds(pageMargin, gridTop, pageWidth,
                Math.Max(120, ClientSize.Height - gridTop - 18));
            dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private static void StyleReportButton(
            Button button,
            Color backColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        // ============================================================
        // LIVE REFRESH
        // ============================================================

        private void DataRefreshManager_DataChanged(
            object? sender,
            DataChangedEventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        GenerateReport(false);
                    }
                }));

                return;
            }

            GenerateReport(false);
        }

        private void ReportsControl_Disposed(
            object? sender,
            EventArgs e)
        {
            DataRefreshManager.DataChanged -=
                DataRefreshManager_DataChanged;

            Disposed -=
                ReportsControl_Disposed;
        }

        // ============================================================
        // FILTERS
        // ============================================================

        private void cmbReportType_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            SetDateFilterState();
        }

        private void FilterDateChanged(
            object? sender,
            EventArgs e)
        {
            if (isLoadingReport)
                return;
        }

        private void SetDateFilterState()
        {
            bool isEmployeeReport =
                string.Equals(
                    cmbReportType.Text,
                    "Employee Report",
                    StringComparison.OrdinalIgnoreCase);

            lblFrom.Enabled = !isEmployeeReport;
            dtpFrom.Enabled = !isEmployeeReport;
            lblTo.Enabled = !isEmployeeReport;
            dtpTo.Enabled = !isEmployeeReport;
        }

        // ============================================================
        // GENERATE REPORT
        // ============================================================

        private void btnGenerateReport_Click(
            object? sender,
            EventArgs e)
        {
            GenerateReport(true);
        }

        private void txtSearch_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            GenerateReport(true);

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void GenerateReport(bool showError)
        {
            if (isLoadingReport || IsDisposed)
                return;

            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                if (showError)
                {
                    MessageBox.Show(
                        "The From date cannot be later than the To date.",
                        "Reports",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                return;
            }

            try
            {
                isLoadingReport = true;

                string reportType =
                    cmbReportType.Text.Trim();

                string searchText =
                    txtSearch.Text.Trim();

                if (reportType.Equals(
                        "Payroll Report",
                        StringComparison.OrdinalIgnoreCase))
                {
                    reportTable =
                        GetPayrollReport(
                            dtpFrom.Value.Date,
                            dtpTo.Value.Date,
                            searchText);
                }
                else if (reportType.Equals(
                             "Attendance Report",
                             StringComparison.OrdinalIgnoreCase))
                {
                    reportTable =
                        GetAttendanceReport(
                            dtpFrom.Value.Date,
                            dtpTo.Value.Date,
                            searchText);
                }
                else if (reportType.Equals(
                             "Payment Receipt Report",
                             StringComparison.OrdinalIgnoreCase))
                {
                    reportTable =
                        GetPaymentReceiptReport(
                            dtpFrom.Value.Date,
                            dtpTo.Value.Date,
                            searchText);
                }
                else
                {
                    reportTable =
                        GetEmployeeReport(searchText);
                }

                dgvReport.DataSource = null;
                dgvReport.DataSource = reportTable;

                FormatReportColumns(reportType);
                UpdateSummary(reportType);
            }
            catch (Exception ex)
            {
                if (showError)
                {
                    MessageBox.Show(
                        "Unable to generate the report.\n\n" +
                        ex.Message,
                        "Reports Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                isLoadingReport = false;
            }
        }

        // ============================================================
        // REPORT QUERIES
        // ============================================================

        private DataTable GetPayrollReport(
            DateTime fromDate,
            DateTime toDate,
            string searchText)
        {
            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    p.PayrollID,
    p.EmployeeID,
    COALESCE(
        e.FirstName || ' ' || e.LastName,
        'Employee ID ' || p.EmployeeID
    ) AS EmployeeName,
    p.PayrollMonth,
    p.PayPeriod,
    p.BasicSalary,
    p.DaysPresent,
    p.DaysAbsent,
    p.OvertimeHours,
    p.GrossPay,
    p.Deductions,
    p.NetPay
FROM payroll p
LEFT JOIN employees e
    ON e.EmployeeID = p.EmployeeID
WHERE
    CAST(p.PayrollMonth || '-01' AS TEXT)
        >= @FromMonth
    AND
    CAST(p.PayrollMonth || '-01' AS TEXT)
        <= @ToMonth
    AND
    (
        @Search = ''
        OR CAST(p.PayrollID AS TEXT) LIKE '%' || @Search || '%'
        OR CAST(p.EmployeeID AS TEXT) LIKE '%' || @Search || '%'
        OR COALESCE(e.FirstName, '') LIKE '%' || @Search || '%'
        OR COALESCE(e.LastName, '') LIKE '%' || @Search || '%'
        OR COALESCE(
            e.FirstName || ' ' || e.LastName,
            ''
        ) LIKE '%' || @Search || '%'
        OR p.PayrollMonth LIKE '%' || @Search || '%'
        OR p.PayPeriod LIKE '%' || @Search || '%'
    )
ORDER BY
    p.PayrollMonth DESC,
    p.PayrollID DESC;
";

            command.Parameters.AddWithValue(
                "@FromMonth",
                fromDate.ToString(
                    "yyyy-MM-01",
                    CultureInfo.InvariantCulture));

            command.Parameters.AddWithValue(
                "@ToMonth",
                new DateTime(toDate.Year, toDate.Month, 1)
                    .ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture));

            command.Parameters.AddWithValue(
                "@Search",
                searchText);

            return ExecuteTable(command);
        }

        private DataTable GetAttendanceReport(
            DateTime fromDate,
            DateTime toDate,
            string searchText)
        {
            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    a.AttendanceID,
    a.EmployeeID,
    COALESCE(
        e.FirstName || ' ' || e.LastName,
        'Employee ID ' || a.EmployeeID
    ) AS EmployeeName,
    a.Date,
    a.TimeIn,
    a.TimeOut,
    a.Status
FROM attendance a
LEFT JOIN employees e
    ON e.EmployeeID = a.EmployeeID
WHERE
    a.Date >= @FromDate
    AND a.Date <= @ToDate
    AND
    (
        @Search = ''
        OR CAST(a.AttendanceID AS TEXT) LIKE '%' || @Search || '%'
        OR CAST(a.EmployeeID AS TEXT) LIKE '%' || @Search || '%'
        OR COALESCE(e.FirstName, '') LIKE '%' || @Search || '%'
        OR COALESCE(e.LastName, '') LIKE '%' || @Search || '%'
        OR COALESCE(
            e.FirstName || ' ' || e.LastName,
            ''
        ) LIKE '%' || @Search || '%'
        OR a.Status LIKE '%' || @Search || '%'
    )
ORDER BY
    a.Date DESC,
    a.AttendanceID DESC;
";

            command.Parameters.AddWithValue(
                "@FromDate",
                fromDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));

            command.Parameters.AddWithValue(
                "@ToDate",
                toDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));

            command.Parameters.AddWithValue(
                "@Search",
                searchText);

            return ExecuteTable(command);
        }

        private DataTable GetPaymentReceiptReport(
            DateTime fromDate,
            DateTime toDate,
            string searchText)
        {
            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    p.PayrollID,
    p.EmployeeID,
    COALESCE(
        e.FirstName || ' ' || e.LastName,
        'Employee ID ' || p.EmployeeID
    ) AS EmployeeName,
    p.PayrollMonth,
    p.PayPeriod,
    p.NetPay AS AmountPaid,
    p.PaymentStatus,
    p.PaymentDate,
    p.PaymentMethod,
    p.ReceiptNumber
FROM payroll p
LEFT JOIN employees e
    ON e.EmployeeID = p.EmployeeID
WHERE
    UPPER(COALESCE(p.PaymentStatus, 'UNPAID')) = 'PAID'
    AND p.PaymentDate IS NOT NULL
    AND date(p.PaymentDate) >= @FromDate
    AND date(p.PaymentDate) <= @ToDate
    AND
    (
        @Search = ''
        OR CAST(p.PayrollID AS TEXT) LIKE '%' || @Search || '%'
        OR CAST(p.EmployeeID AS TEXT) LIKE '%' || @Search || '%'
        OR COALESCE(e.FirstName, '') LIKE '%' || @Search || '%'
        OR COALESCE(e.LastName, '') LIKE '%' || @Search || '%'
        OR COALESCE(
            e.FirstName || ' ' || e.LastName,
            ''
        ) LIKE '%' || @Search || '%'
        OR COALESCE(p.ReceiptNumber, '') LIKE '%' || @Search || '%'
        OR COALESCE(p.PaymentMethod, '') LIKE '%' || @Search || '%'
    )
ORDER BY
    date(p.PaymentDate) DESC,
    p.PayrollID DESC;
";

            command.Parameters.AddWithValue(
                "@FromDate",
                fromDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));

            command.Parameters.AddWithValue(
                "@ToDate",
                toDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));

            command.Parameters.AddWithValue(
                "@Search",
                searchText);

            return ExecuteTable(command);
        }

        private DataTable GetEmployeeReport(string searchText)
        {
            return DatabaseHelper.GetEmployees(searchText);
        }

        private static DataTable ExecuteTable(
            SqliteCommand command)
        {
            DataTable table = new DataTable();

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ============================================================
        // TABLE FORMATTING
        // ============================================================

        private void FormatReportColumns(string reportType)
        {
            foreach (DataGridViewColumn column in dgvReport.Columns)
            {
                string propertyName =
                    !string.IsNullOrWhiteSpace(column.DataPropertyName)
                        ? column.DataPropertyName
                        : column.Name;

                string normalized = NormalizeColumnName(propertyName);

                column.HeaderText = GetFriendlyReportHeader(normalized, column.HeaderText);
                column.Visible = true;
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                column.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                column.FillWeight = GetReportColumnFillWeight(normalized);

                if (normalized.Contains("SALARY") ||
                    normalized.Contains("GROSSPAY") ||
                    normalized.Contains("DEDUCTIONS") ||
                    normalized.Contains("NETPAY") ||
                    normalized.Contains("AMOUNTPAID"))
                {
                    column.DefaultCellStyle.Format = "₱#,##0.00";
                    column.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;
                }
                else if (normalized.Contains("OVERTIMEHOURS"))
                {
                    column.DefaultCellStyle.Format = "0.00";
                    column.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;
                }
                else if (normalized.Contains("EMPLOYEENAME") ||
                         normalized.Contains("FIRSTNAME") ||
                         normalized.Contains("LASTNAME") ||
                         normalized.Contains("POSITION") ||
                         normalized.Contains("DEPARTMENT"))
                {
                    column.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleLeft;
                }
            }

            if (reportType.Equals(
                    "Employee Report",
                    StringComparison.OrdinalIgnoreCase))
            {
                HideEmptyEmployeeColumns();
            }

            // This runs after the report-specific formatting above so money,
            // names, and all other cells follow the requested center alignment.
            DataGridViewSortingHelper.ApplyStyles(dgvReport);
        }

        private static string NormalizeColumnName(string value)
        {
            return (value ?? string.Empty)
                .Replace("_", string.Empty, StringComparison.Ordinal)
                .Replace(" ", string.Empty, StringComparison.Ordinal)
                .ToUpperInvariant();
        }

        private static string GetFriendlyReportHeader(
            string normalized,
            string fallback)
        {
            return normalized switch
            {
                "PAYROLLID" => "Payroll ID",
                "EMPLOYEEID" => "Employee ID",
                "EMPLOYEENAME" => "Employee Name",
                "PAYROLLMONTH" => "Payroll Month",
                "PAYPERIOD" => "Pay Period",
                "BASICSALARY" => "Basic Salary",
                "DAYSPRESENT" => "Days Present",
                "DAYSABSENT" => "Days Absent",
                "OVERTIMEHOURS" => "Overtime Hours",
                "GROSSPAY" => "Gross Pay",
                "DEDUCTIONS" => "Deductions",
                "NETPAY" => "Net Pay",
                "PAYMENTSTATUS" => "Payment Status",
                "RECEIPTNUMBER" => "Receipt No.",
                "PAYMENTDATE" => "Payment Date",
                "PAYMENTMETHOD" => "Payment Method",
                "ATTENDANCEID" => "Attendance ID",
                "DATE" => "Date",
                "TIMEIN" => "Time In",
                "TIMEOUT" => "Time Out",
                "STATUS" => "Status",
                "FIRSTNAME" => "First Name",
                "LASTNAME" => "Last Name",
                "POSITION" => "Position",
                "DEPARTMENT" => "Department",
                "SALARY" => "Salary / Monthly",
                "SSSNO" => "SSS No.",
                "PHILHEALTHPIN" => "PhilHealth PIN",
                "PAGIBOMID" => "Pag-IBIG MID",
                "TIN" => "TIN",
                "AMOUNTPAID" => "Amount Paid",
                _ => MakeHeader(fallback)
            };
        }

        private static float GetReportColumnFillWeight(string normalized)
        {
            if (normalized == "EMPLOYEENAME") return 150F;
            if (normalized == "RECEIPTNUMBER") return 140F;
            if (normalized == "PAYPERIOD") return 115F;
            if (normalized == "PAYROLLMONTH") return 88F;
            if (normalized == "PAYMENTMETHOD") return 105F;
            if (normalized == "PAYMENTDATE") return 100F;
            if (normalized.Contains("SALARY") ||
                normalized.Contains("GROSSPAY") ||
                normalized.Contains("DEDUCTIONS") ||
                normalized.Contains("NETPAY") ||
                normalized.Contains("AMOUNTPAID")) return 105F;
            if (normalized.Contains("OVERTIMEHOURS")) return 95F;
            if (normalized.Contains("DAYS")) return 78F;
            if (normalized.Contains("EMPLOYEEID") ||
                normalized.Contains("PAYROLLID") ||
                normalized.Contains("ATTENDANCEID")) return 76F;
            if (normalized == "STATUS" || normalized == "PAYMENTSTATUS") return 88F;
            if (normalized == "DATE" || normalized == "TIMEIN" || normalized == "TIMEOUT") return 92F;
            return 100F;
        }

        private void HideEmptyEmployeeColumns()
        {
            string[] optionalColumns =
            {
                "SSSNo",
                "PhilHealthPIN",
                "PagIBIGMID",
                "TIN"
            };

            foreach (string columnName in optionalColumns)
            {
                if (!reportTable.Columns.Contains(columnName))
                    continue;

                bool anyValue = reportTable.Rows
                    .Cast<DataRow>()
                    .Any(row =>
                        !string.IsNullOrWhiteSpace(
                            row[columnName]?.ToString()));

                if (dgvReport.Columns.Contains(columnName))
                {
                    dgvReport.Columns[columnName].Visible = anyValue;
                }
            }
        }

        private static string MakeHeader(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            StringBuilder builder =
                new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                char current = value[i];

                if (i > 0 && char.IsUpper(current) &&
                    value[i - 1] != '_')
                {
                    builder.Append(' ');
                }

                if (current == '_')
                {
                    builder.Append(' ');
                    continue;
                }

                builder.Append(current);
            }

            return builder.ToString();
        }

        // ============================================================
        // SUMMARY
        // ============================================================

        private void UpdateSummary(string reportType)
        {
            int count = reportTable.Rows.Count;

            if (reportType.Equals(
                    "Payroll Report",
                    StringComparison.OrdinalIgnoreCase))
            {
                decimal totalGross = reportTable.Rows
                    .Cast<DataRow>()
                    .Sum(row =>
                        Convert.ToDecimal(
                            row["GrossPay"],
                            CultureInfo.InvariantCulture));

                decimal totalDeductions = reportTable.Rows
                    .Cast<DataRow>()
                    .Sum(row =>
                        Convert.ToDecimal(
                            row["Deductions"],
                            CultureInfo.InvariantCulture));

                decimal totalNet = reportTable.Rows
                    .Cast<DataRow>()
                    .Sum(row =>
                        Convert.ToDecimal(
                            row["NetPay"],
                            CultureInfo.InvariantCulture));

                lblSummary.Text =
                    $"{count:N0} payroll record(s)  |  " +
                    $"Gross: {totalGross.ToString("₱#,##0.00", CultureInfo.CurrentCulture)}  |  " +
                    $"Deductions: {totalDeductions.ToString("₱#,##0.00", CultureInfo.CurrentCulture)}  |  " +
                    $"Net: {totalNet.ToString("₱#,##0.00", CultureInfo.CurrentCulture)}";

                return;
            }

            if (reportType.Equals(
                    "Attendance Report",
                    StringComparison.OrdinalIgnoreCase))
            {
                int present = reportTable.Rows
                    .Cast<DataRow>()
                    .Count(row =>
                        string.Equals(
                            row["Status"]?.ToString(),
                            "Present",
                            StringComparison.OrdinalIgnoreCase));

                int absent = reportTable.Rows
                    .Cast<DataRow>()
                    .Count(row =>
                        string.Equals(
                            row["Status"]?.ToString(),
                            "Absent",
                            StringComparison.OrdinalIgnoreCase));

                lblSummary.Text =
                    $"{count:N0} attendance record(s)  |  " +
                    $"Present: {present:N0}  |  Absent: {absent:N0}";

                return;
            }

            if (reportType.Equals(
                    "Payment Receipt Report",
                    StringComparison.OrdinalIgnoreCase))
            {
                decimal totalPaid = reportTable.Rows
                    .Cast<DataRow>()
                    .Sum(row =>
                        Convert.ToDecimal(
                            row["AmountPaid"],
                            CultureInfo.InvariantCulture));

                lblSummary.Text =
                    $"{count:N0} payment receipt(s)  |  " +
                    $"Total Paid: {totalPaid.ToString("₱#,##0.00", CultureInfo.CurrentCulture)}";

                return;
            }

            int active = reportTable.Rows
                .Cast<DataRow>()
                .Count(row =>
                    string.Equals(
                        row["Status"]?.ToString(),
                        "Active",
                        StringComparison.OrdinalIgnoreCase));

            lblSummary.Text =
                $"{count:N0} employee(s)  |  Active: {active:N0}  |  " +
                $"Inactive/Other: {Math.Max(0, count - active):N0}";
        }

        // ============================================================
        // EXPORT CSV
        // ============================================================

        private void btnExportCsv_Click(
            object? sender,
            EventArgs e)
        {
            if (isExporting)
                return;

            if (reportTable.Rows.Count == 0)
            {
                MessageBox.Show(
                    "There is no report data to export.",
                    "Reports",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog dialog =
                new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    DefaultExt = "csv",
                    AddExtension = true,
                    FileName =
                        GetDefaultFileName()
                };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                isExporting = true;

                ExportDataTableToCsv(
                    reportTable,
                    dialog.FileName);

                MessageBox.Show(
                    "Report exported successfully.",
                    "Reports",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to export the report.\n\n" +
                    ex.Message,
                    "Report Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                isExporting = false;
            }
        }

        private string GetDefaultFileName()
        {
            string type =
                cmbReportType.Text
                    .Replace(" Report", "", StringComparison.OrdinalIgnoreCase)
                    .Replace(" ", "_", StringComparison.Ordinal);

            return
                $"{type}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        }

        private static void ExportDataTableToCsv(
            DataTable table,
            string filePath)
        {
            StringBuilder csv =
                new StringBuilder();

            string[] headers =
                table.Columns
                    .Cast<DataColumn>()
                    .Where(column =>
                        !string.Equals(
                            column.ColumnName,
                            "__hidden__",
                            StringComparison.OrdinalIgnoreCase))
                    .Select(column => column.ColumnName)
                    .ToArray();

            csv.AppendLine(
                string.Join(",", headers.Select(EscapeCsvField)));

            foreach (DataRow row in table.Rows)
            {
                string[] values =
                    headers.Select(header =>
                        EscapeCsvField(
                            row[header]?.ToString() ?? ""))
                    .ToArray();

                csv.AppendLine(
                    string.Join(",", values));
            }

            File.WriteAllText(
                filePath,
                csv.ToString(),
                new UTF8Encoding(true));
        }

        private static string EscapeCsvField(string value)
        {
            if (value.Contains('"') ||
                value.Contains(',') ||
                value.Contains('\r') ||
                value.Contains('\n'))
            {
                return "\"" +
                       value.Replace(
                           "\"",
                           "\"\"",
                           StringComparison.Ordinal) +
                       "\"";
            }

            return value;
        }
    }
}
