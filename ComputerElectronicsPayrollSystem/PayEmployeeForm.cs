using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{

    public sealed class PayrollPaymentDraft
    {
        public long EmployeeID { get; }
        public string EmployeeName { get; }
        public string PayrollMonth { get; }
        public string PayPeriod { get; }
        public decimal BasicSalary { get; }
        public int DaysPresent { get; }
        public int DaysAbsent { get; }
        public decimal OvertimeHours { get; }
        public decimal OvertimePay { get; }
        public decimal GrossPay { get; }
        public decimal SssContribution { get; }
        public decimal PhilHealthContribution { get; }
        public decimal PagIbigContribution { get; }
        public decimal WithholdingTax { get; }
        public decimal Deductions { get; }
        public decimal NetPay { get; }

        public PayrollPaymentDraft(
            long employeeID,
            string employeeName,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal overtimePay,
            decimal grossPay,
            decimal sssContribution,
            decimal philHealthContribution,
            decimal pagIbigContribution,
            decimal withholdingTax,
            decimal deductions,
            decimal netPay)
        {
            EmployeeID = employeeID;
            EmployeeName = employeeName;
            PayrollMonth = payrollMonth;
            PayPeriod = payPeriod;
            BasicSalary = basicSalary;
            DaysPresent = daysPresent;
            DaysAbsent = daysAbsent;
            OvertimeHours = overtimeHours;
            OvertimePay = overtimePay;
            GrossPay = grossPay;
            SssContribution = sssContribution;
            PhilHealthContribution = philHealthContribution;
            PagIbigContribution = pagIbigContribution;
            WithholdingTax = withholdingTax;
            Deductions = deductions;
            NetPay = netPay;
        }
    }

    public partial class PayEmployeeForm : Form
    {
        private readonly long payrollID;
        private readonly PayrollPaymentDraft? paymentDraft;
        private readonly bool isNewPayrollDraft;
        private bool paymentCompleted = false;

        public string ReceiptNumber { get; private set; } = "";

        public PayEmployeeForm(long payrollID)
        {
            this.payrollID = payrollID;
            isNewPayrollDraft = false;

            InitializeComponent();
            KeyboardNavigation.Enable(this);

            LoadPayrollDetails();
        }

        public PayEmployeeForm(PayrollPaymentDraft draft)
        {
            paymentDraft = draft
                ?? throw new ArgumentNullException(nameof(draft));

            payrollID = 0;
            isNewPayrollDraft = true;

            InitializeComponent();
            KeyboardNavigation.Enable(this);

            LoadPayrollDraft();
        }

        private void LoadPayrollDraft()
        {
            PayrollPaymentDraft draft = paymentDraft!;

            lblEmployeeNameValue.Text =
                draft.EmployeeName;

            lblEmployeeIDValue.Text =
                draft.EmployeeID.ToString();

            lblPayPeriodValue.Text =
                $"{draft.PayrollMonth} | {draft.PayPeriod}";

            lblGrossPayValue.Text =
                draft.GrossPay.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            lblDeductionsValue.Text =
                draft.Deductions.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            lblNetPayValue.Text =
                draft.NetPay.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            dtpPaymentDate.Value = DateTime.Now;

            cmbPaymentMethod.SelectedIndex =
                cmbPaymentMethod.Items.Count > 0 ? 0 : -1;
        }

        private void LoadPayrollDetails()
        {
            try
            {
                DataTable table =
                    PayrollDatabaseHelper.GetPayrollRecord(payrollID);

                if (table.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "The payroll record could not be found.",
                        "Pay Employee",
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

                lblEmployeeNameValue.Text =
                    row["EmployeeName"]?.ToString() ?? "";

                lblEmployeeIDValue.Text =
                    Convert.ToInt64(row["EmployeeID"]).ToString();

                lblPayPeriodValue.Text =
                    $"{row["PayrollMonth"]} | {row["PayPeriod"]}";

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

                lblGrossPayValue.Text =
                    grossPay.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                lblDeductionsValue.Text =
                    deductions.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                lblNetPayValue.Text =
                    netPay.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                string paymentStatus =
                    row.Table.Columns.Contains("PaymentStatus")
                        ? row["PaymentStatus"]?.ToString() ?? "UNPAID"
                        : "UNPAID";

                if (string.Equals(
                        paymentStatus,
                        "PAID",
                        StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "This payroll record has already been paid.",
                        "Pay Employee",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    BeginInvoke(new Action(() =>
                    {
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }));

                    return;
                }

                dtpPaymentDate.Value = DateTime.Now;
                cmbPaymentMethod.SelectedIndex =
                    cmbPaymentMethod.Items.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load payroll payment details.\n\n" +
                    ex.Message,
                    "Payment Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                BeginInvoke(new Action(() =>
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }));
            }
        }

        private void btnConfirmPayment_Click(
            object? sender,
            EventArgs e)
        {
            if (paymentCompleted)
                return;

            if (cmbPaymentMethod.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(cmbPaymentMethod.Text))
            {
                MessageBox.Show(
                    "Please select a payment method.",
                    "Payment Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbPaymentMethod.Focus();
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Confirm that the employee has been paid?\n\n" +
                "Employee: " + lblEmployeeNameValue.Text + "\n" +
                "Net Pay: " + lblNetPayValue.Text + "\n" +
                "Payment Method: " + cmbPaymentMethod.Text,
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
                return;

            try
            {
                if (isNewPayrollDraft)
                {
                    PayrollPaymentDraft draft = paymentDraft!;

                    ReceiptNumber =
                        PayrollDatabaseHelper.AddPayrollAndMarkPaid(
                            draft.EmployeeID,
                            draft.PayrollMonth,
                            draft.PayPeriod,
                            draft.BasicSalary,
                            draft.DaysPresent,
                            draft.DaysAbsent,
                            draft.OvertimeHours,
                            draft.GrossPay,
                            draft.SssContribution,
                            draft.PhilHealthContribution,
                            draft.PagIbigContribution,
                            draft.WithholdingTax,
                            draft.Deductions,
                            draft.NetPay,
                            dtpPaymentDate.Value,
                            cmbPaymentMethod.Text.Trim());
                }
                else
                {
                    ReceiptNumber =
                        PayrollDatabaseHelper.MarkPayrollPaid(
                            payrollID,
                            dtpPaymentDate.Value,
                            cmbPaymentMethod.Text.Trim());
                }

                paymentCompleted = true;

                DataRefreshManager.Notify(
                    DataChangeType.Payroll,
                    this);

                MessageBox.Show(
                    "Payment recorded successfully.\n\nReceipt No.: " +
                    ReceiptNumber,
                    "Payment Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                using PayrollReceiptForm receiptForm =
                    new PayrollReceiptForm(ReceiptNumber);

                receiptForm.ShowDialog(this);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to record the payment.\n\n" +
                    ex.Message,
                    "Payment Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelPayment_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
