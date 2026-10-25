using System;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class UpdateEmployeeForm : Form
    {
        // ==========================================================
        // UPDATED EMPLOYEE VALUES
        // ==========================================================

        public string EmployeeID { get; private set; } = "";
        public string FirstName { get; private set; } = "";
        public string LastName { get; private set; } = "";
        public string Position { get; private set; } = "";
        public string Department { get; private set; } = "";
        public decimal Salary { get; private set; }
        public string SSSNo { get; private set; } = "";
        public string PhilHealthPIN { get; private set; } = "";
        public string PagIBIGMID { get; private set; } = "";
        public string TIN { get; private set; } = "";
        public string Status { get; private set; } = "";

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================

        public UpdateEmployeeForm(
            string employeeID,
            string firstName,
            string lastName,
            string position,
            string department,
            decimal salary,
            string status)
        {
            InitializeComponent();
            Shown += (_, _) => FuturisticUiTheme.Apply(this);

            // Enable keyboard navigation for this form.
            KeyboardNavigation.Enable(this);

            // ------------------------------------------------------
            // LOAD EXISTING EMPLOYEE DATA
            // ------------------------------------------------------

            txtEmployeeID.Text = employeeID;
            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;
            txtPosition.Text = position;
            txtDepartment.Text = department;
            txtSalary.Text = salary.ToString("0.00");

            // Load the employee's existing government identification
            // numbers directly from the database.
            long parsedEmployeeID = 0;

            if (long.TryParse(
                    employeeID,
                    out long employeeIDValue))
            {
                parsedEmployeeID = employeeIDValue;
            }

            if (parsedEmployeeID > 0)
            {
                DatabaseHelper.GetEmployeeGovernmentIDs(
                    parsedEmployeeID,
                    out string sssNo,
                    out string philHealthPIN,
                    out string pagIBIGMID,
                    out string tin);

                txtSSSNo.Text = sssNo;
                txtPhilHealthPIN.Text = philHealthPIN;
                txtPagIBIGMID.Text = pagIBIGMID;
                txtTIN.Text = tin;
            }

            // Select the existing status.
            if (!string.IsNullOrWhiteSpace(status))
            {
                int statusIndex = cmbStatus.Items.IndexOf(status);

                if (statusIndex >= 0)
                {
                    cmbStatus.SelectedIndex = statusIndex;
                }
            }

            // Connect buttons intentionally.
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            // Government identification fields accept only digits and separators.
            txtSSSNo.KeyPress += GovernmentID_KeyPress;
            txtPhilHealthPIN.KeyPress += GovernmentID_KeyPress;
            txtPagIBIGMID.KeyPress += GovernmentID_KeyPress;
            txtTIN.KeyPress += GovernmentID_KeyPress;

            // Put the cursor in First Name because Employee ID
            // cannot be edited.
            this.Shown += UpdateEmployeeForm_Shown;
        }

        // ==========================================================
        // INITIAL FOCUS
        // ==========================================================

        private void UpdateEmployeeForm_Shown(
            object? sender,
            EventArgs e)
        {
            txtFirstName.Focus();
        }

        // ==========================================================
        // SAVE / UPDATE
        // ==========================================================

        private void btnSave_Click(
            object? sender,
            EventArgs e)
        {
            string employeeID =
                txtEmployeeID.Text.Trim();

            string firstName =
                txtFirstName.Text.Trim();

            string lastName =
                txtLastName.Text.Trim();

            string position =
                txtPosition.Text.Trim();

            string department =
                txtDepartment.Text.Trim();

            string salaryText =
                txtSalary.Text.Trim();

            string sssNo =
                txtSSSNo.Text.Trim();

            string philHealthPIN =
                txtPhilHealthPIN.Text.Trim();

            string pagIBIGMID =
                txtPagIBIGMID.Text.Trim();

            string tin =
                txtTIN.Text.Trim();

            string status =
                cmbStatus.Text.Trim();

            // ------------------------------------------------------
            // VALIDATE FIRST NAME
            // ------------------------------------------------------

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show(
                    "Please enter the First Name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtFirstName.Focus();
                return;
            }

            // ------------------------------------------------------
            // VALIDATE LAST NAME
            // ------------------------------------------------------

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show(
                    "Please enter the Last Name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtLastName.Focus();
                return;
            }

            // ------------------------------------------------------
            // VALIDATE POSITION
            // ------------------------------------------------------

            if (string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show(
                    "Please enter the Position.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPosition.Focus();
                return;
            }

            // ------------------------------------------------------
            // VALIDATE DEPARTMENT
            // ------------------------------------------------------

            if (string.IsNullOrWhiteSpace(department))
            {
                MessageBox.Show(
                    "Please enter the Department.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDepartment.Focus();
                return;
            }

            // ------------------------------------------------------
            // VALIDATE MONTHLY SALARY
            // ------------------------------------------------------

            if (!decimal.TryParse(
                    salaryText,
                    out decimal salary))
            {
                MessageBox.Show(
                    "Please enter a valid monthly salary.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSalary.Focus();
                txtSalary.SelectAll();
                return;
            }

            if (salary < 0)
            {
                MessageBox.Show(
                    "Monthly salary cannot be negative.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSalary.Focus();
                txtSalary.SelectAll();
                return;
            }

            // ------------------------------------------------------
            // VALIDATE GOVERNMENT IDENTIFICATION NUMBERS
            // ------------------------------------------------------

            if (!ValidateGovernmentID(
                    sssNo,
                    "SSS No.",
                    10,
                    new[] { 2, 10 },
                    txtSSSNo))
            {
                return;
            }

            if (!ValidateGovernmentID(
                    philHealthPIN,
                    "PhilHealth PIN",
                    12,
                    new[] { 2, 12 },
                    txtPhilHealthPIN))
            {
                return;
            }

            if (!ValidateGovernmentID(
                    pagIBIGMID,
                    "Pag-IBIG MID",
                    12,
                    new[] { 4, 9 },
                    txtPagIBIGMID))
            {
                return;
            }

            if (!ValidateGovernmentID(
                    tin,
                    "TIN",
                    12,
                    new[] { 3, 7, 11 },
                    txtTIN))
            {
                return;
            }

            // ------------------------------------------------------
            // VALIDATE STATUS
            // ------------------------------------------------------

            if (string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show(
                    "Please select an employee status.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbStatus.Focus();
                return;
            }

            // ------------------------------------------------------
            // STORE UPDATED VALUES
            // ------------------------------------------------------

            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Department = department;
            Salary = salary;
            SSSNo = sssNo;
            PhilHealthPIN = philHealthPIN;
            PagIBIGMID = pagIBIGMID;
            TIN = tin;
            Status = status;

            // Stage the government identification values so the
            // existing EmployeesControl can save them together with
            // the employee update.
            DatabaseHelper.StageEmployeeGovernmentIDs(
                employeeID,
                SSSNo,
                PhilHealthPIN,
                PagIBIGMID,
                TIN);

            // Tell EmployeesControl that the update succeeded.
            DialogResult = DialogResult.OK;
            Close();
        }

        // ==========================================================
        // GOVERNMENT ID VALIDATION
        // ==========================================================

        private static void GovernmentID_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            // Allow digits, hyphen separators, and editing keys.
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != '-' &&
                e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private static bool ValidateGovernmentID(
            string value,
            string fieldName,
            int expectedDigits,
            int[] dashPositions,
            Control field)
        {
            // Government IDs are optional in the employee record.
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            string digitsOnly =
                value.Replace("-", string.Empty);

            bool allCharactersValid = true;

            foreach (char character in value)
            {
                if (!char.IsDigit(character) && character != '-')
                {
                    allCharactersValid = false;
                    break;
                }
            }

            bool validFormat =
                allCharactersValid &&
                digitsOnly.Length == expectedDigits;

            if (validFormat && value.Contains('-'))
            {
                // When hyphens are used, require the exact official-style
                // grouping for this particular identification number.
                int expectedLength = expectedDigits + dashPositions.Length;

                if (value.Length != expectedLength)
                {
                    validFormat = false;
                }
                else
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        bool shouldBeDash = false;

                        foreach (int dashPosition in dashPositions)
                        {
                            if (i == dashPosition)
                            {
                                shouldBeDash = true;
                                break;
                            }
                        }

                        if (shouldBeDash)
                        {
                            if (value[i] != '-')
                            {
                                validFormat = false;
                                break;
                            }
                        }
                        else if (!char.IsDigit(value[i]))
                        {
                            validFormat = false;
                            break;
                        }
                    }
                }
            }
            else if (validFormat && !value.Contains('-'))
            {
                // Digits-only entry is also accepted.
                validFormat = value.Length == expectedDigits;
            }

            if (!validFormat)
            {
                MessageBox.Show(
                    $"Please enter a valid {fieldName}. Use {expectedDigits} digits" +
                    " (hyphens may be used in the standard grouping).",
                    "Invalid Government ID",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                field.Focus();

                if (field is TextBox textBox)
                {
                    textBox.SelectAll();
                }

                return false;
            }

            return true;
        }

        // ==========================================================
        // CANCEL
        // ==========================================================

        private void btnCancel_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
