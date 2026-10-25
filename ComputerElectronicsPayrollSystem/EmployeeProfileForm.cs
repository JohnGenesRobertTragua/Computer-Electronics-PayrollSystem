using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class EmployeeProfileForm : Form
    {
        private readonly long employeeID;
        private readonly string employeeName;

        public EmployeeProfileForm(long employeeID, string employeeName)
        {
            this.employeeID = employeeID;
            this.employeeName = employeeName;

            InitializeComponent();
            KeyboardNavigation.Enable(this);

            lblEmployeeIDValue.Text = employeeID.ToString();
            lblEmployeeNameValue.Text = employeeName;
        }

        private void btnInformation_Click(object? sender, EventArgs e)
        {
            OpenDetails(EmployeeDetailView.Information);
        }

        private void btnAttendance_Click(object? sender, EventArgs e)
        {
            OpenDetails(EmployeeDetailView.Attendance);
        }

        private void btnPayroll_Click(object? sender, EventArgs e)
        {
            OpenDetails(EmployeeDetailView.PayrollRecords);
        }

        private void btnReceipts_Click(object? sender, EventArgs e)
        {
            OpenDetails(EmployeeDetailView.Receipts);
        }

        private void OpenDetails(EmployeeDetailView view)
        {
            using EmployeeDetailViewForm detailsForm =
                new EmployeeDetailViewForm(
                    employeeID,
                    employeeName,
                    view);

            detailsForm.ShowDialog(this);
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
