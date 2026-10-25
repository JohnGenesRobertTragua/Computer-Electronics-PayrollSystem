using System;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace ComputerElectronicsPayrollSystem
{
    public enum EmployeeDetailView
    {
        Information,
        Attendance,
        PayrollRecords,
        Receipts
    }

    public partial class EmployeeDetailViewForm : Form
    {
        private readonly long employeeID;
        private readonly string employeeName;
        private readonly EmployeeDetailView view;

        private static readonly string DatabasePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "payroll.db");

        private static string ConnectionString => $"Data Source={DatabasePath}";

        public EmployeeDetailViewForm(
            long employeeID,
            string employeeName,
            EmployeeDetailView view)
        {
            this.employeeID = employeeID;
            this.employeeName = employeeName;
            this.view = view;

            InitializeComponent();
            KeyboardNavigation.Enable(this);

            // The shared details grid is reused for information, attendance,
            // payroll records, and receipts; keep it centered and sortable.
            DataGridViewSortingHelper.Configure(dgvDetails);
            FuturisticUiTheme.Apply(this);

            lblEmployeeNameValue.Text = employeeName;
            lblEmployeeIDValue.Text = employeeID.ToString();

            btnViewReceipt.Visible = false;

            LoadSelectedView();

            dgvDetails.SelectionChanged += dgvDetails_SelectionChanged;
        }

        private void LoadSelectedView()
        {
            switch (view)
            {
                case EmployeeDetailView.Information:
                    lblTitle.Text = "EMPLOYEE INFORMATION";
                    btnViewReceipt.Visible = false;
                    LoadEmployeeInformation();
                    break;

                case EmployeeDetailView.Attendance:
                    lblTitle.Text = "EMPLOYEE ATTENDANCE";
                    btnViewReceipt.Visible = false;
                    LoadAttendance();
                    break;

                case EmployeeDetailView.PayrollRecords:
                    lblTitle.Text = "EMPLOYEE PAYROLL RECORDS";
                    btnViewReceipt.Visible = true;
                    btnViewReceipt.Enabled = false;
                    LoadPayrollRecords();
                    break;

                case EmployeeDetailView.Receipts:
                    lblTitle.Text = "EMPLOYEE RECEIPTS";
                    btnViewReceipt.Visible = true;
                    btnViewReceipt.Enabled = true;
                    LoadReceipts();
                    break;
            }
        }

        private void PrepareGrid()
        {
            dgvDetails.DataSource = null;
            dgvDetails.Columns.Clear();
            dgvDetails.AutoGenerateColumns = true;
            dgvDetails.ReadOnly = true;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.MultiSelect = false;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDetails.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDetails.DataSource = null;
        }

        private void LoadEmployeeInformation()
        {
            try
            {
                PrepareGrid();

                DataTable employees = DatabaseHelper.GetEmployees();
                DataRow? employeeRow = null;

                foreach (DataRow row in employees.Rows)
                {
                    if (Convert.ToInt64(row["EmployeeID"]) == employeeID)
                    {
                        employeeRow = row;
                        break;
                    }
                }

                if (employeeRow == null)
                {
                    ShowMessage("The employee could not be found.");
                    return;
                }

                DatabaseHelper.GetEmployeeGovernmentIDs(
                    employeeID,
                    out string sssNo,
                    out string philHealthPIN,
                    out string pagIBIGMID,
                    out string tin);

                DataTable table = new DataTable();
                table.Columns.Add("Information");
                table.Columns.Add("Value");

                table.Rows.Add("Employee ID", employeeID);
                table.Rows.Add("First Name", employeeRow["FirstName"]?.ToString() ?? "");
                table.Rows.Add("Last Name", employeeRow["LastName"]?.ToString() ?? "");
                table.Rows.Add("Position", employeeRow["Position"]?.ToString() ?? "");
                table.Rows.Add("Department", employeeRow["Department"]?.ToString() ?? "");

                decimal salary = Convert.ToDecimal(
                    employeeRow["Salary"],
                    CultureInfo.InvariantCulture);

                table.Rows.Add(
                    "Salary / Monthly",
                    salary.ToString("₱#,##0.00", CultureInfo.CurrentCulture));

                table.Rows.Add("Status", employeeRow["Status"]?.ToString() ?? "");
                table.Rows.Add("SSS No.", string.IsNullOrWhiteSpace(sssNo) ? "Not provided" : sssNo);
                table.Rows.Add("PhilHealth PIN", string.IsNullOrWhiteSpace(philHealthPIN) ? "Not provided" : philHealthPIN);
                table.Rows.Add("Pag-IBIG MID", string.IsNullOrWhiteSpace(pagIBIGMID) ? "Not provided" : pagIBIGMID);
                table.Rows.Add("TIN", string.IsNullOrWhiteSpace(tin) ? "Not provided" : tin);

                dgvDetails.DataSource = table;
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load employee information.\n\n" + ex.Message);
            }
        }

        private void LoadAttendance()
        {
            try
            {
                PrepareGrid();

                using SqliteConnection connection = new SqliteConnection(ConnectionString);
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
SELECT
    AttendanceID,
    Date,
    TimeIn,
    TimeOut,
    Status
FROM attendance
WHERE EmployeeID = @EmployeeID
ORDER BY Date DESC, AttendanceID DESC;";

                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                using SqliteDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                dgvDetails.DataSource = table;

                // Generated DataGridView column names can differ from the
                // underlying DataTable column names. Use safe lookup helpers
                // rather than assuming every named column exists.
                SetGridColumnHeader("AttendanceID", "Attendance ID");
                SetGridColumnHeader("Date", "Date");
                SetGridColumnHeader("TimeIn", "Time In");
                SetGridColumnHeader("TimeOut", "Time Out");
                SetGridColumnHeader("Status", "Status");

                ConfigureAttendanceGridLayout();

                SetSummaryText($"Attendance records: {table.Rows.Count}");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load attendance records.\n\n" + ex.Message);
            }
        }

        private void LoadPayrollRecords()
        {
            try
            {
                PrepareGrid();

                using SqliteConnection connection = new SqliteConnection(ConnectionString);
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
SELECT
    p.PayrollID,
    p.PayrollMonth,
    p.PayPeriod,
    p.BasicSalary,
    p.DaysPresent,
    p.DaysAbsent,
    p.OvertimeHours,
    p.GrossPay,
    p.Deductions,
    p.NetPay,
    p.PaymentStatus,
    p.PaymentDate,
    p.PaymentMethod,
    p.ReceiptNumber
FROM payroll p
WHERE p.EmployeeID = @EmployeeID
ORDER BY p.PayrollID DESC;";

                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                using SqliteDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                dgvDetails.DataSource = table;

                ConfigurePayrollGridHeaders();
                ConfigureDetailsGridColumnSizing();
                SetSummaryText($"Payroll records: {table.Rows.Count}");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load payroll records.\n\n" + ex.Message);
            }
        }

        private void LoadReceipts()
        {
            try
            {
                PrepareGrid();

                using SqliteConnection connection = new SqliteConnection(ConnectionString);
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
SELECT
    p.ReceiptNumber,
    p.PayrollID,
    p.PayrollMonth,
    p.PayPeriod,
    p.NetPay AS AmountPaid,
    p.PaymentMethod,
    p.PaymentDate,
    p.PaymentStatus
FROM payroll p
WHERE p.EmployeeID = @EmployeeID
  AND UPPER(COALESCE(p.PaymentStatus, '')) = 'PAID'
  AND COALESCE(p.ReceiptNumber, '') <> ''
ORDER BY p.PayrollID DESC;";

                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                using SqliteDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                dgvDetails.DataSource = table;

                ConfigureDetailsGridColumnSizing();

                SetGridColumnHeader("ReceiptNumber", "Receipt No.");
                SetGridColumnHeader("PayrollID", "Payroll ID");
                SetGridColumnHeader("PayrollMonth", "Payroll Month");
                SetGridColumnHeader("PayPeriod", "Pay Period");
                SetGridColumnHeader("AmountPaid", "Amount Paid");
                SetGridColumnFormat("AmountPaid", "₱#,##0.00");
                SetGridColumnHeader("PaymentMethod", "Payment Method");
                SetGridColumnHeader("PaymentDate", "Payment Date");
                SetGridColumnHeader("PaymentStatus", "Status");

                SetSummaryText($"Saved receipts: {table.Rows.Count}");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load receipts.\n\n" + ex.Message);
            }
        }

        private void ConfigurePayrollGridHeaders()
        {
            SetGridColumnHeader("PayrollID", "Payroll ID");
            SetGridColumnHeader("PayrollMonth", "Payroll Month");
            SetGridColumnHeader("PayPeriod", "Pay Period");
            SetGridColumnHeader("BasicSalary", "Basic Salary");
            SetGridColumnFormat("BasicSalary", "₱#,##0.00");
            SetGridColumnHeader("GrossPay", "Gross Pay");
            SetGridColumnFormat("GrossPay", "₱#,##0.00");
            SetGridColumnHeader("Deductions", "Deductions");
            SetGridColumnFormat("Deductions", "₱#,##0.00");
            SetGridColumnHeader("NetPay", "Net Pay");
            SetGridColumnFormat("NetPay", "₱#,##0.00");
            SetGridColumnHeader("PaymentStatus", "Payment Status");
            SetGridColumnHeader("PaymentDate", "Payment Date");
            SetGridColumnHeader("PaymentMethod", "Payment Method");
            SetGridColumnHeader("ReceiptNumber", "Receipt No.");
        }

        private void ConfigureAttendanceGridLayout()
        {
            if (dgvDetails == null || dgvDetails.Columns.Count == 0)
                return;

            // Fill the available width and keep attendance values readable.
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvDetails.BackgroundColor = Color.White;
            dgvDetails.GridColor = Color.Gainsboro;
            dgvDetails.BorderStyle = BorderStyle.FixedSingle;
            dgvDetails.ColumnHeadersHeight = 36;
            dgvDetails.RowTemplate.Height = 32;
            dgvDetails.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvDetails.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvDetails.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDetails.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            // Use FillWeight rather than changing MinimumWidth dynamically.
            // Setting MinimumWidth on the auto-generated columns was causing
            // a NullReferenceException while the attendance grid was layouting.
            SetGridColumnFillWeight("AttendanceID", 22);
            SetGridColumnFillWeight("Date", 23);
            SetGridColumnFillWeight("TimeIn", 20);
            SetGridColumnFillWeight("TimeOut", 20);
            SetGridColumnFillWeight("Status", 15);

            DataGridViewSortingHelper.ApplyStyles(dgvDetails);
        }

        // Safely find an auto-generated column by either its Name or its
        // DataPropertyName. This prevents a missing generated column from
        // throwing a NullReferenceException while the view is loading.
        private DataGridViewColumn? FindGridColumn(string columnKey)
        {
            if (dgvDetails == null)
                return null;

            foreach (DataGridViewColumn column in dgvDetails.Columns)
            {
                if (string.Equals(column.Name, columnKey, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(column.DataPropertyName, columnKey, StringComparison.OrdinalIgnoreCase))
                {
                    return column;
                }
            }

            return null;
        }

        private void SetGridColumnHeader(string columnKey, string headerText)
        {
            DataGridViewColumn? column = FindGridColumn(columnKey);
            if (column != null)
                column.HeaderText = headerText;
        }

        private void SetGridColumnFormat(string columnKey, string format)
        {
            DataGridViewColumn? column = FindGridColumn(columnKey);
            if (column != null)
                column.DefaultCellStyle.Format = format;
        }

        private void SetGridColumnFillWeight(string columnKey, float fillWeight)
        {
            DataGridViewColumn? column = FindGridColumn(columnKey);
            if (column == null)
                return;

            // The grid is already in Fill mode; proportional weights keep
            // columns readable without changing MinimumWidth at runtime.
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = fillWeight;
        }

        private void SetSummaryText(string text)
        {
            if (lblSummary != null && !lblSummary.IsDisposed)
                lblSummary.Text = text;
        }

        private void ConfigureDetailsGridColumnSizing()
        {
            if (dgvDetails == null || dgvDetails.Columns.Count == 0)
                return;

            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvDetails.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDetails.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            foreach (DataGridViewColumn column in dgvDetails.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 100;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            }

            // Give longer-text and money/date fields more space while allowing
            // the grid to adapt to the form width. No runtime MinimumWidth writes.
            SetGridColumnFillWeight("PayrollID", 70);
            SetGridColumnFillWeight("AttendanceID", 80);
            SetGridColumnFillWeight("Date", 105);
            SetGridColumnFillWeight("TimeIn", 95);
            SetGridColumnFillWeight("TimeOut", 95);
            SetGridColumnFillWeight("Status", 80);
            SetGridColumnFillWeight("PayrollMonth", 100);
            SetGridColumnFillWeight("PayPeriod", 125);
            SetGridColumnFillWeight("BasicSalary", 110);
            SetGridColumnFillWeight("DaysPresent", 75);
            SetGridColumnFillWeight("DaysAbsent", 75);
            SetGridColumnFillWeight("OvertimeHours", 90);
            SetGridColumnFillWeight("GrossPay", 105);
            SetGridColumnFillWeight("Deductions", 100);
            SetGridColumnFillWeight("NetPay", 105);
            SetGridColumnFillWeight("PaymentStatus", 100);
            SetGridColumnFillWeight("PaymentMethod", 115);
            SetGridColumnFillWeight("PaymentDate", 115);
            SetGridColumnFillWeight("ReceiptNumber", 130);
            SetGridColumnFillWeight("AmountPaid", 110);

            DataGridViewSortingHelper.ApplyStyles(dgvDetails);
        }

        private void dgvDetails_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            if (DataGridViewSortingHelper.IsSorting(dgvDetails))
                return;

            if (view != EmployeeDetailView.PayrollRecords)
                return;

            if (dgvDetails.CurrentRow?.DataBoundItem is not DataRowView rowView)
            {
                btnViewReceipt.Enabled = false;
                return;
            }

            string receiptNumber =
                rowView.Row.Table.Columns.Contains("ReceiptNumber")
                    ? rowView["ReceiptNumber"]?.ToString() ?? ""
                    : "";

            string status =
                rowView.Row.Table.Columns.Contains("PaymentStatus")
                    ? rowView["PaymentStatus"]?.ToString() ?? ""
                    : "";

            btnViewReceipt.Enabled =
                string.Equals(status, "PAID", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(receiptNumber);
        }

        private void btnViewReceipt_Click(object? sender, EventArgs e)
        {
            if (dgvDetails.CurrentRow?.DataBoundItem is not DataRowView rowView)
            {
                ShowMessage("Please select a receipt or a paid payroll record first.");
                return;
            }

            string receiptNumber = "";

            if (rowView.Row.Table.Columns.Contains("ReceiptNumber"))
            {
                receiptNumber = rowView["ReceiptNumber"]?.ToString() ?? "";
            }

            if (string.IsNullOrWhiteSpace(receiptNumber) ||
                receiptNumber.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                ShowMessage("The selected record does not have a saved receipt.");
                return;
            }

            using PayrollReceiptForm receiptForm =
                new PayrollReceiptForm(receiptNumber);

            receiptForm.ShowDialog(this);
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(
                this,
                message,
                "Employee Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
