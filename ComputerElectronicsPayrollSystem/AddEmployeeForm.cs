using System;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class AddEmployeeForm : Form
    {
        // ==========================================================
        // EMPLOYEE VALUES
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
        // DEFAULT CONSTRUCTOR
        // ==========================================================

        // This keeps the form compatible with the WinForms Designer.
        public AddEmployeeForm()
        {
            InitializeComponent();
            Shown += (_, _) => FuturisticUiTheme.Apply(this);

            ConfigureForm();
        }

        // ==========================================================
        // CONSTRUCTOR WITH GENERATED EMPLOYEE ID
        // ==========================================================

        public AddEmployeeForm(string generatedEmployeeID)
            : this()
        {
            txtEmployeeID.Text = generatedEmployeeID;
        }

        // ==========================================================
        // FORM CONFIGURATION
        // ==========================================================

        private void ConfigureForm()
        {
            // ------------------------------------------------------
            // KEYBOARD NAVIGATION
            // ------------------------------------------------------

            KeyboardNavigation.Enable(this);

            // ------------------------------------------------------
            // EMPLOYEE ID
            // ------------------------------------------------------

            // Employee ID is automatically generated.
            txtEmployeeID.ReadOnly = true;

            // Do not allow keyboard navigation to stop on the
            // read-only Employee ID field.
            txtEmployeeID.TabStop = false;

            // ------------------------------------------------------
            // DEFAULT STATUS
            // ------------------------------------------------------

            if (cmbStatus.Items.Count > 0)
            {
                cmbStatus.SelectedIndex = 0;
            }

            // ------------------------------------------------------
            // BUTTON EVENTS
            // ------------------------------------------------------

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            // Government identification fields accept only digits and separators.
            txtSSSNo.KeyPress += GovernmentID_KeyPress;
            txtPhilHealthPIN.KeyPress += GovernmentID_KeyPress;
            txtPagIBIGMID.KeyPress += GovernmentID_KeyPress;
            txtTIN.KeyPress += GovernmentID_KeyPress;

            // ------------------------------------------------------
            // INITIAL FOCUS
            // ------------------------------------------------------

            Shown += AddEmployeeForm_Shown;
        }

        // ==========================================================
        // INITIAL FOCUS
        // ==========================================================

        private void AddEmployeeForm_Shown(
            object? sender,
            EventArgs e)
        {
            // Start entering information at First Name.
            txtFirstName.Focus();
        }

        // ==========================================================
        // SAVE EMPLOYEE
        // ==========================================================

        private void btnSave_Click(
            object? sender,
            EventArgs e)
        {
            // ------------------------------------------------------
            // READ VALUES
            // ------------------------------------------------------

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
            // VALIDATE EMPLOYEE ID
            // ------------------------------------------------------

            if (string.IsNullOrWhiteSpace(employeeID))
            {
                MessageBox.Show(
                    "Unable to generate the Employee ID.",
                    "Employee ID Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

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
            // STORE VALUES
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
            // existing EmployeesControl can save them through the
            // current DatabaseHelper compatibility overload.
            DatabaseHelper.StageEmployeeGovernmentIDs(
                employeeID,
                SSSNo,
                PhilHealthPIN,
                PagIBIGMID,
                TIN);

            // ------------------------------------------------------
            // RETURN SUCCESS
            // ------------------------------------------------------

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
