using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class PayrollReceiptForm : Form
    {
        private readonly string receiptNumber;
        private readonly PrintDocument printDocument;
        private int printLineIndex = 0;
        private string[] receiptLines = Array.Empty<string>();

        public PayrollReceiptForm(string receiptNumber)
        {
            this.receiptNumber = receiptNumber.Trim();

            InitializeComponent();
            Shown += (_, _) => FuturisticUiTheme.Apply(this);
            KeyboardNavigation.Enable(this);

            printDocument = new PrintDocument();
            printDocument.DocumentName =
                "Payroll Receipt " + this.receiptNumber;
            printDocument.PrintPage +=
                PrintDocument_PrintPage;

            LoadReceipt();
        }

        private void LoadReceipt()
        {
            try
            {
                DataTable table =
                    PayrollDatabaseHelper.GetPayrollByReceipt(
                        receiptNumber);

                if (table.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "The receipt could not be found.",
                        "Receipt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    BeginInvoke(new Action(() =>
                    {
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }));

                    return;
                }

                DataRow row = table.Rows[0];

                decimal basicSalary =
                    Convert.ToDecimal(
                        row["BasicSalary"],
                        CultureInfo.InvariantCulture);

                decimal grossPay =
                    Convert.ToDecimal(
                        row["GrossPay"],
                        CultureInfo.InvariantCulture);

                decimal deductions =
                    Convert.ToDecimal(
                        row["Deductions"],
                        CultureInfo.InvariantCulture);

                decimal netPay =
                    Convert.ToDecimal(
                        row["NetPay"],
                        CultureInfo.InvariantCulture);

                decimal overtimePay =
                    Math.Max(0m, grossPay - basicSalary);

                decimal sss =
                    GetMoney(row, "SSSContribution");

                decimal philHealth =
                    GetMoney(row, "PhilHealthContribution");

                decimal pagIbig =
                    GetMoney(row, "PagIBIGContribution");

                decimal withholdingTax =
                    GetMoney(row, "WithholdingTax");

                string employeeName =
                    row["EmployeeName"]?.ToString() ?? "";

                string employeeID =
                    row["EmployeeID"]?.ToString() ?? "";

                string position =
                    row.Table.Columns.Contains("Position")
                        ? row["Position"]?.ToString() ?? ""
                        : "";

                string department =
                    row.Table.Columns.Contains("Department")
                        ? row["Department"]?.ToString() ?? ""
                        : "";

                string payrollMonth =
                    row["PayrollMonth"]?.ToString() ?? "";

                string payPeriod =
                    row["PayPeriod"]?.ToString() ?? "";

                string paymentMethod =
                    row.Table.Columns.Contains("PaymentMethod")
                        ? row["PaymentMethod"]?.ToString() ?? ""
                        : "";

                string paymentDate =
                    FormatDate(
                        row.Table.Columns.Contains("PaymentDate")
                            ? row["PaymentDate"]?.ToString() ?? ""
                            : "");

                string status =
                    row.Table.Columns.Contains("PaymentStatus")
                        ? row["PaymentStatus"]?.ToString() ?? "PAID"
                        : "PAID";

                receiptLines = BuildReceiptLines(
                    receiptNumber,
                    paymentDate,
                    status,
                    employeeName,
                    employeeID,
                    position,
                    department,
                    payrollMonth,
                    payPeriod,
                    basicSalary,
                    overtimePay,
                    grossPay,
                    sss,
                    philHealth,
                    pagIbig,
                    withholdingTax,
                    deductions,
                    netPay,
                    paymentMethod);

                rtbReceipt.Text =
                    string.Join(Environment.NewLine, receiptLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load the payroll receipt.\n\n" +
                    ex.Message,
                    "Receipt Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static decimal GetMoney(
            DataRow row,
            string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) ||
                row[columnName] == DBNull.Value)
            {
                return 0m;
            }

            return Convert.ToDecimal(
                row[columnName],
                CultureInfo.InvariantCulture);
        }

        private static string FormatDate(string value)
        {
            if (DateTime.TryParse(
                    value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsed))
            {
                return parsed.ToString(
                    "MMMM dd, yyyy hh:mm:ss tt",
                    CultureInfo.CurrentCulture);
            }

            return value;
        }

        private static string[] BuildReceiptLines(
            string receiptNumber,
            string paymentDate,
            string status,
            string employeeName,
            string employeeID,
            string position,
            string department,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            decimal overtimePay,
            decimal grossPay,
            decimal sss,
            decimal philHealth,
            decimal pagIbig,
            decimal withholdingTax,
            decimal deductions,
            decimal netPay,
            string paymentMethod)
        {
            const string line =
                "============================================================";

            const string dash =
                "------------------------------------------------------------";

            return new[]
            {
                "             COMPUTER & ELECTRONICS SHOP",
                "                    PAYROLL RECEIPT",
                line,
                "Receipt No.:     " + receiptNumber,
                "Payment Date:    " + paymentDate,
                "Payment Status:  " + status,
                "",
                "EMPLOYEE",
                "Name:            " + employeeName,
                "Employee ID:     " + employeeID,
                "Position:        " + position,
                "Department:      " + department,
                "",
                "PAYROLL",
                "Payroll Month:   " + payrollMonth,
                "Pay Period:      " + payPeriod,
                dash,
                "Basic Salary:                  " + FormatMoney(basicSalary),
                "Overtime Pay:                  " + FormatMoney(overtimePay),
                "                              " + dash,
                "Gross Pay:                    " + FormatMoney(grossPay),
                "",
                "DEDUCTIONS",
                "SSS:                           " + FormatMoney(sss),
                "PhilHealth:                    " + FormatMoney(philHealth),
                "Pag-IBIG:                      " + FormatMoney(pagIbig),
                "Withholding Tax:               " + FormatMoney(withholdingTax),
                "                              " + dash,
                "Total Deductions:              " + FormatMoney(deductions),
                "",
                line,
                "NET PAY:                      " + FormatMoney(netPay),
                line,
                "",
                "Payment Method:  " + paymentMethod,
                "",
                "Received by:     ______________________________",
                "Employee Signature: ___________________________",
                "",
                "Keep this receipt for your payroll records."
            };
        }

        private static string FormatMoney(decimal amount)
        {
            return amount.ToString(
                "₱#,##0.00",
                CultureInfo.CurrentCulture);
        }

        private void btnPrint_Click(
            object? sender,
            EventArgs e)
        {
            if (receiptLines.Length == 0)
                return;

            try
            {
                printLineIndex = 0;

                using PrintPreviewDialog previewDialog =
                    new PrintPreviewDialog();

                previewDialog.Document = printDocument;
                previewDialog.Width = 900;
                previewDialog.Height = 700;
                previewDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to open print preview.\n\n" +
                    ex.Message,
                    "Print Receipt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }

        private void PrintDocument_PrintPage(
            object? sender,
            PrintPageEventArgs e)
        {
            using Font font =
                new Font("Consolas", 9F);

            float lineHeight =
                font.GetHeight(e.Graphics) + 2F;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            while (printLineIndex < receiptLines.Length)
            {
                if (y + lineHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                e.Graphics.DrawString(
                    receiptLines[printLineIndex],
                    font,
                    Brushes.Black,
                    x,
                    y);

                y += lineHeight;
                printLineIndex++;
            }

            e.HasMorePages = false;
            printLineIndex = 0;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            printDocument.Dispose();
            base.OnFormClosed(e);
        }
    }
}
