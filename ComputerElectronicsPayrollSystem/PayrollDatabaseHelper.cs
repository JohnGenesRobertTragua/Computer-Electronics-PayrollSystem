using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ComputerElectronicsPayrollSystem
{
    /// <summary>
    /// Payroll-specific database operations.
    /// The helper keeps the existing employee and attendance database
    /// functionality untouched while providing payroll storage and
    /// Philippine statutory deduction fields.
    /// </summary>
    public static class PayrollDatabaseHelper
    {
        private static readonly string DatabasePath =
            System.IO.Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "payroll.db");

        private static string ConnectionString =>
            $"Data Source={DatabasePath}";

        // ============================================================
        // ENSURE PAYROLL TABLE
        // ============================================================

        public static void EnsurePayrollTable()
        {
            string? directory =
                System.IO.Path.GetDirectoryName(DatabasePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using (SqliteCommand pragmaCommand =
                   connection.CreateCommand())
            {
                pragmaCommand.CommandText =
                    "PRAGMA foreign_keys = ON;";

                pragmaCommand.ExecuteNonQuery();
            }

            using (SqliteCommand command =
                   connection.CreateCommand())
            {
                command.CommandText = @"
CREATE TABLE IF NOT EXISTS payroll
(
    PayrollID INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeID INTEGER NOT NULL,
    PayrollMonth TEXT NOT NULL,
    PayPeriod TEXT NOT NULL,
    BasicSalary REAL NOT NULL,
    DaysPresent INTEGER NOT NULL,
    DaysAbsent INTEGER NOT NULL,
    OvertimeHours REAL NOT NULL,
    SSSContribution REAL NOT NULL DEFAULT 0,
    PhilHealthContribution REAL NOT NULL DEFAULT 0,
    PagIBIGContribution REAL NOT NULL DEFAULT 0,
    WithholdingTax REAL NOT NULL DEFAULT 0,
    PaymentStatus TEXT NOT NULL DEFAULT 'UNPAID',
    PaymentDate TEXT,
    PaymentMethod TEXT,
    ReceiptNumber TEXT,
    GrossPay REAL NOT NULL,
    Deductions REAL NOT NULL,
    NetPay REAL NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (EmployeeID)
        REFERENCES employees(EmployeeID)
        ON DELETE CASCADE
);
";

                command.ExecuteNonQuery();
            }

            // --------------------------------------------------------
            // MIGRATE EXISTING PAYROLL DATABASES
            // --------------------------------------------------------
            // Existing payroll databases created before statutory
            // deduction support do not contain the four new columns.
            // Add them safely without deleting existing records.

            EnsurePayrollColumn(
                connection,
                "SSSContribution",
                "REAL NOT NULL DEFAULT 0");

            EnsurePayrollColumn(
                connection,
                "PhilHealthContribution",
                "REAL NOT NULL DEFAULT 0");

            EnsurePayrollColumn(
                connection,
                "PagIBIGContribution",
                "REAL NOT NULL DEFAULT 0");

            EnsurePayrollColumn(
                connection,
                "WithholdingTax",
                "REAL NOT NULL DEFAULT 0");

            EnsurePayrollColumn(
                connection,
                "PaymentStatus",
                "TEXT NOT NULL DEFAULT 'UNPAID'");

            EnsurePayrollColumn(
                connection,
                "PaymentDate",
                "TEXT");

            EnsurePayrollColumn(
                connection,
                "PaymentMethod",
                "TEXT");

            EnsurePayrollColumn(
                connection,
                "ReceiptNumber",
                "TEXT");

            using (SqliteCommand repairCommand =
                   connection.CreateCommand())
            {
                repairCommand.CommandText = @"
UPDATE payroll
SET PaymentStatus = 'UNPAID'
WHERE PaymentStatus IS NULL
   OR TRIM(PaymentStatus) = '';
";
                repairCommand.ExecuteNonQuery();
            }

            // Do not keep a UNIQUE SQLite index on ReceiptNumber.
            // Microsoft.Data.Sqlite can expose such an index as a unique
            // DataColumn when DataTable.Load() reads the result set.
            // Existing unpaid payroll records have NULL receipt numbers,
            // and that can cause DataTable.Load() to fail its constraints.
            // Receipt numbers are already generated from PayrollID, so they
            // are unique by design.
            using (SqliteCommand receiptIndexRepairCommand =
                   connection.CreateCommand())
            {
                receiptIndexRepairCommand.CommandText =
                    "DROP INDEX IF EXISTS IX_payroll_receipt_number;";

                receiptIndexRepairCommand.ExecuteNonQuery();
            }

            using SqliteCommand indexCommand =
                connection.CreateCommand();

            indexCommand.CommandText = @"
CREATE UNIQUE INDEX IF NOT EXISTS IX_payroll_employee_month_period
ON payroll(EmployeeID, PayrollMonth, PayPeriod);
";

            indexCommand.ExecuteNonQuery();
        }

        private static void EnsurePayrollColumn(
            SqliteConnection connection,
            string columnName,
            string columnDefinition)
        {
            using SqliteCommand checkCommand =
                connection.CreateCommand();

            checkCommand.CommandText = @"
SELECT COUNT(*)
FROM pragma_table_info('payroll')
WHERE name = @ColumnName;
";

            checkCommand.Parameters.AddWithValue(
                "@ColumnName",
                columnName);

            long count =
                Convert.ToInt64(
                    checkCommand.ExecuteScalar());

            if (count > 0)
                return;

            using SqliteCommand alterCommand =
                connection.CreateCommand();

            // Column names are fixed internal identifiers selected by
            // this application; they are not supplied by the user.
            alterCommand.CommandText =
                $"ALTER TABLE payroll ADD COLUMN {columnName} {columnDefinition};";

            alterCommand.ExecuteNonQuery();
        }

        // ============================================================
        // GET ACTIVE EMPLOYEES
        // ============================================================

        public static DataTable GetPayrollEmployees()
        {
            EnsurePayrollTable();

            DataTable table = new DataTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    EmployeeID,
    FirstName || ' ' || LastName AS EmployeeName,
    Salary,
    Status
FROM employees
WHERE UPPER(Status) = 'ACTIVE'
ORDER BY LastName, FirstName;
";

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ============================================================
        // GET PAYROLL RECORDS
        // ============================================================

        public static DataTable GetPayrollRecords(
            string searchText = "")
        {
            EnsurePayrollTable();

            DataTable table = new DataTable();

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
    p.SSSContribution,
    p.PhilHealthContribution,
    p.PagIBIGContribution,
    p.WithholdingTax,
    p.PaymentStatus,
    p.PaymentDate,
    p.PaymentMethod,
    p.ReceiptNumber,
    p.GrossPay,
    p.Deductions,
    p.NetPay
FROM payroll p
LEFT JOIN employees e
    ON e.EmployeeID = p.EmployeeID
WHERE
    @Search = ''
    OR CAST(p.PayrollID AS TEXT) LIKE '%' || @Search || '%'
    OR CAST(p.EmployeeID AS TEXT) LIKE '%' || @Search || '%'
    OR COALESCE(e.FirstName, '') LIKE '%' || @Search || '%'
    OR COALESCE(e.LastName, '') LIKE '%' || @Search || '%'
    OR COALESCE(e.FirstName || ' ' || e.LastName, '')
        LIKE '%' || @Search || '%'
    OR p.PayrollMonth LIKE '%' || @Search || '%'
    OR p.PayPeriod LIKE '%' || @Search || '%'
ORDER BY p.PayrollID DESC;
";

            command.Parameters.AddWithValue(
                "@Search",
                searchText.Trim());

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ============================================================
        // GET ATTENDANCE SUMMARY
        // ============================================================

        public static DataTable GetAttendanceSummary(
            long employeeID,
            DateTime startDate,
            DateTime endDate)
        {
            EnsurePayrollTable();

            DataTable table = new DataTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    COALESCE(
        SUM(
            CASE
                WHEN UPPER(Status) = 'PRESENT' THEN 1
                ELSE 0
            END),
        0
    ) AS DaysPresent,
    COALESCE(
        SUM(
            CASE
                WHEN UPPER(Status) = 'ABSENT' THEN 1
                ELSE 0
            END),
        0
    ) AS DaysAbsent
FROM attendance
WHERE EmployeeID = @EmployeeID
  AND Date >= @StartDate
  AND Date <= @EndDate;
";

            command.Parameters.AddWithValue(
                "@EmployeeID",
                employeeID);

            command.Parameters.AddWithValue(
                "@StartDate",
                startDate.ToString("yyyy-MM-dd"));

            command.Parameters.AddWithValue(
                "@EndDate",
                endDate.ToString("yyyy-MM-dd"));

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ============================================================
        // ADD PAYROLL - STATUTORY VERSION
        // ============================================================

        public static long AddPayroll(
            long employeeID,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal grossPay,
            decimal sssContribution,
            decimal philHealthContribution,
            decimal pagIbigContribution,
            decimal withholdingTax,
            decimal deductions,
            decimal netPay)
        {
            EnsurePayrollTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
INSERT INTO payroll
(
    EmployeeID,
    PayrollMonth,
    PayPeriod,
    BasicSalary,
    DaysPresent,
    DaysAbsent,
    OvertimeHours,
    SSSContribution,
    PhilHealthContribution,
    PagIBIGContribution,
    WithholdingTax,
    GrossPay,
    Deductions,
    NetPay
)
VALUES
(
    @EmployeeID,
    @PayrollMonth,
    @PayPeriod,
    @BasicSalary,
    @DaysPresent,
    @DaysAbsent,
    @OvertimeHours,
    @SSSContribution,
    @PhilHealthContribution,
    @PagIBIGContribution,
    @WithholdingTax,
    @GrossPay,
    @Deductions,
    @NetPay
);

SELECT last_insert_rowid();
";

            AddCommonPayrollParameters(
                command,
                employeeID,
                payrollMonth,
                payPeriod,
                basicSalary,
                daysPresent,
                daysAbsent,
                overtimeHours,
                grossPay,
                sssContribution,
                philHealthContribution,
                pagIbigContribution,
                withholdingTax,
                deductions,
                netPay);

            object? result = command.ExecuteScalar();

            return Convert.ToInt64(result);
        }

        // Backward-compatible overload for older code.
        public static long AddPayroll(
            long employeeID,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal grossPay,
            decimal deductions,
            decimal netPay)
        {
            return AddPayroll(
                employeeID,
                payrollMonth,
                payPeriod,
                basicSalary,
                daysPresent,
                daysAbsent,
                overtimeHours,
                grossPay,
                0m,
                0m,
                0m,
                0m,
                deductions,
                netPay);
        }

        // ============================================================
        // GET SINGLE PAYROLL RECORD
        // ============================================================

        public static DataTable GetPayrollRecord(
            long payrollID)
        {
            EnsurePayrollTable();

            DataTable table = new DataTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    p.PayrollID,
    p.EmployeeID,
    COALESCE(e.FirstName || ' ' || e.LastName, '') AS EmployeeName,
    COALESCE(e.Position, '') AS Position,
    COALESCE(e.Department, '') AS Department,
    p.PayrollMonth,
    p.PayPeriod,
    p.BasicSalary,
    p.DaysPresent,
    p.DaysAbsent,
    p.OvertimeHours,
    p.SSSContribution,
    p.PhilHealthContribution,
    p.PagIBIGContribution,
    p.WithholdingTax,
    p.GrossPay,
    p.Deductions,
    p.NetPay,
    p.PaymentStatus,
    p.PaymentDate,
    p.PaymentMethod,
    p.ReceiptNumber,
    p.CreatedAt
FROM payroll p
LEFT JOIN employees e
    ON e.EmployeeID = p.EmployeeID
WHERE p.PayrollID = @PayrollID;
";

            command.Parameters.AddWithValue(
                "@PayrollID",
                payrollID);

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ============================================================
        // PAYROLL BY RECEIPT NUMBER
        // ============================================================

        public static DataTable GetPayrollByReceipt(
            string receiptNumber)
        {
            EnsurePayrollTable();

            DataTable table = new DataTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
SELECT
    p.PayrollID,
    p.EmployeeID,
    COALESCE(e.FirstName || ' ' || e.LastName, '') AS EmployeeName,
    COALESCE(e.Position, '') AS Position,
    COALESCE(e.Department, '') AS Department,
    p.PayrollMonth,
    p.PayPeriod,
    p.BasicSalary,
    p.DaysPresent,
    p.DaysAbsent,
    p.OvertimeHours,
    p.SSSContribution,
    p.PhilHealthContribution,
    p.PagIBIGContribution,
    p.WithholdingTax,
    p.GrossPay,
    p.Deductions,
    p.NetPay,
    p.PaymentStatus,
    p.PaymentDate,
    p.PaymentMethod,
    p.ReceiptNumber,
    p.CreatedAt
FROM payroll p
LEFT JOIN employees e
    ON e.EmployeeID = p.EmployeeID
WHERE p.ReceiptNumber = @ReceiptNumber
  AND UPPER(p.PaymentStatus) = 'PAID';
";

            command.Parameters.AddWithValue(
                "@ReceiptNumber",
                receiptNumber.Trim());

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ============================================================
        // ADD PAYROLL AND MARK AS PAID
        // ============================================================
        // A new payroll is created and marked PAID in one transaction.
        // If payment is cancelled before this method is called, no
        // payroll record is created.

        public static string AddPayrollAndMarkPaid(
            long employeeID,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal grossPay,
            decimal sssContribution,
            decimal philHealthContribution,
            decimal pagIbigContribution,
            decimal withholdingTax,
            decimal deductions,
            decimal netPay,
            DateTime paymentDate,
            string paymentMethod)
        {
            EnsurePayrollTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteTransaction transaction =
                connection.BeginTransaction();

            try
            {
                using SqliteCommand duplicateCommand =
                    connection.CreateCommand();

                duplicateCommand.Transaction = transaction;
                duplicateCommand.CommandText = @"
SELECT
    PayrollID,
    PaymentStatus
FROM payroll
WHERE EmployeeID = @EmployeeID
  AND PayrollMonth = @PayrollMonth
  AND PayPeriod = @PayPeriod;
";

                duplicateCommand.Parameters.AddWithValue(
                    "@EmployeeID",
                    employeeID);

                duplicateCommand.Parameters.AddWithValue(
                    "@PayrollMonth",
                    payrollMonth);

                duplicateCommand.Parameters.AddWithValue(
                    "@PayPeriod",
                    payPeriod);

                using SqliteDataReader duplicateReader =
                    duplicateCommand.ExecuteReader();

                if (duplicateReader.Read())
                {
                    long existingPayrollID =
                        duplicateReader.GetInt64(0);

                    string existingStatus =
                        duplicateReader.IsDBNull(1)
                            ? "UNPAID"
                            : duplicateReader.GetString(1);

                    duplicateReader.Close();

                    throw new InvalidOperationException(
                        existingStatus.Equals(
                            "PAID",
                            StringComparison.OrdinalIgnoreCase)
                            ? "This payroll record has already been paid."
                            : "A payroll record already exists for this employee and pay period. Select it and use PAY EMPLOYEE.");
                }

                using SqliteCommand insertCommand =
                    connection.CreateCommand();

                insertCommand.Transaction = transaction;

                insertCommand.CommandText = @"
INSERT INTO payroll
(
    EmployeeID,
    PayrollMonth,
    PayPeriod,
    BasicSalary,
    DaysPresent,
    DaysAbsent,
    OvertimeHours,
    SSSContribution,
    PhilHealthContribution,
    PagIBIGContribution,
    WithholdingTax,
    PaymentStatus,
    PaymentDate,
    PaymentMethod,
    ReceiptNumber,
    GrossPay,
    Deductions,
    NetPay
)
VALUES
(
    @EmployeeID,
    @PayrollMonth,
    @PayPeriod,
    @BasicSalary,
    @DaysPresent,
    @DaysAbsent,
    @OvertimeHours,
    @SSSContribution,
    @PhilHealthContribution,
    @PagIBIGContribution,
    @WithholdingTax,
    'PAID',
    @PaymentDate,
    @PaymentMethod,
    @ReceiptNumber,
    @GrossPay,
    @Deductions,
    @NetPay
);

SELECT last_insert_rowid();
";

                string receiptNumberPreview =
                    $"TMP-{Guid.NewGuid():N}";

                insertCommand.Parameters.AddWithValue(
                    "@EmployeeID",
                    employeeID);

                insertCommand.Parameters.AddWithValue(
                    "@PayrollMonth",
                    payrollMonth);

                insertCommand.Parameters.AddWithValue(
                    "@PayPeriod",
                    payPeriod);

                insertCommand.Parameters.AddWithValue(
                    "@BasicSalary",
                    basicSalary);

                insertCommand.Parameters.AddWithValue(
                    "@DaysPresent",
                    daysPresent);

                insertCommand.Parameters.AddWithValue(
                    "@DaysAbsent",
                    daysAbsent);

                insertCommand.Parameters.AddWithValue(
                    "@OvertimeHours",
                    overtimeHours);

                insertCommand.Parameters.AddWithValue(
                    "@SSSContribution",
                    sssContribution);

                insertCommand.Parameters.AddWithValue(
                    "@PhilHealthContribution",
                    philHealthContribution);

                insertCommand.Parameters.AddWithValue(
                    "@PagIBIGContribution",
                    pagIbigContribution);

                insertCommand.Parameters.AddWithValue(
                    "@WithholdingTax",
                    withholdingTax);

                insertCommand.Parameters.AddWithValue(
                    "@PaymentDate",
                    paymentDate.ToString(
                        "yyyy-MM-dd HH:mm:ss"));

                insertCommand.Parameters.AddWithValue(
                    "@PaymentMethod",
                    paymentMethod.Trim());

                insertCommand.Parameters.AddWithValue(
                    "@ReceiptNumber",
                    receiptNumberPreview);

                insertCommand.Parameters.AddWithValue(
                    "@GrossPay",
                    grossPay);

                insertCommand.Parameters.AddWithValue(
                    "@Deductions",
                    deductions);

                insertCommand.Parameters.AddWithValue(
                    "@NetPay",
                    netPay);

                long newPayrollID =
                    Convert.ToInt64(
                        insertCommand.ExecuteScalar());

                string finalReceiptNumber =
                    $"PR-{paymentDate:yyyy}-{newPayrollID:D6}";

                using SqliteCommand receiptCommand =
                    connection.CreateCommand();

                receiptCommand.Transaction = transaction;
                receiptCommand.CommandText = @"
UPDATE payroll
SET ReceiptNumber = @ReceiptNumber
WHERE PayrollID = @PayrollID;
";

                receiptCommand.Parameters.AddWithValue(
                    "@ReceiptNumber",
                    finalReceiptNumber);

                receiptCommand.Parameters.AddWithValue(
                    "@PayrollID",
                    newPayrollID);

                int rows =
                    receiptCommand.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new InvalidOperationException(
                        "The payroll receipt could not be recorded.");
                }

                transaction.Commit();

                return finalReceiptNumber;
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Ignore rollback failures.
                }

                throw;
            }
        }

        // ============================================================
        // MARK PAYROLL AS PAID
        // ============================================================

        public static string MarkPayrollPaid(
            long payrollID,
            DateTime paymentDate,
            string paymentMethod)
        {
            EnsurePayrollTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteTransaction transaction =
                connection.BeginTransaction();

            try
            {
                using SqliteCommand statusCommand =
                    connection.CreateCommand();

                statusCommand.Transaction = transaction;
                statusCommand.CommandText = @"
SELECT
    PaymentStatus,
    ReceiptNumber
FROM payroll
WHERE PayrollID = @PayrollID;
";

                statusCommand.Parameters.AddWithValue(
                    "@PayrollID",
                    payrollID);

                using SqliteDataReader reader =
                    statusCommand.ExecuteReader();

                if (!reader.Read())
                {
                    throw new InvalidOperationException(
                        "The payroll record could not be found.");
                }

                string status =
                    reader.IsDBNull(0)
                        ? "UNPAID"
                        : reader.GetString(0);

                string existingReceipt =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1);

                reader.Close();

                if (status.Equals(
                        "PAID",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "This payroll record has already been paid. " +
                        (string.IsNullOrWhiteSpace(existingReceipt)
                            ? ""
                            : "Receipt No.: " + existingReceipt));
                }

                // Receipt numbers are permanently tied to the payroll ID.
                // AUTOINCREMENT ensures deleted payroll IDs are not reused.
                string receiptNumber =
                    $"PR-{paymentDate:yyyy}-{payrollID:D6}";

                using SqliteCommand updateCommand =
                    connection.CreateCommand();

                updateCommand.Transaction = transaction;
                updateCommand.CommandText = @"
UPDATE payroll
SET
    PaymentStatus = 'PAID',
    PaymentDate = @PaymentDate,
    PaymentMethod = @PaymentMethod,
    ReceiptNumber = @ReceiptNumber
WHERE PayrollID = @PayrollID;
";

                updateCommand.Parameters.AddWithValue(
                    "@PaymentDate",
                    paymentDate.ToString("yyyy-MM-dd HH:mm:ss"));

                updateCommand.Parameters.AddWithValue(
                    "@PaymentMethod",
                    paymentMethod.Trim());

                updateCommand.Parameters.AddWithValue(
                    "@ReceiptNumber",
                    receiptNumber);

                updateCommand.Parameters.AddWithValue(
                    "@PayrollID",
                    payrollID);

                int rows =
                    updateCommand.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new InvalidOperationException(
                        "The payroll payment could not be recorded.");
                }

                transaction.Commit();

                return receiptNumber;
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Ignore rollback failures.
                }

                throw;
            }
        }

        // ============================================================
        // DELETE PAYROLL
        // ============================================================

        public static bool DeletePayroll(long payrollID)
        {
            EnsurePayrollTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
DELETE FROM payroll
WHERE PayrollID = @PayrollID;
";

            command.Parameters.AddWithValue(
                "@PayrollID",
                payrollID);

            int rows = command.ExecuteNonQuery();

            return rows > 0;
        }

        // ============================================================
        // UPDATE PAYROLL - STATUTORY VERSION
        // ============================================================

        public static bool UpdatePayroll(
            long payrollID,
            long employeeID,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal grossPay,
            decimal sssContribution,
            decimal philHealthContribution,
            decimal pagIbigContribution,
            decimal withholdingTax,
            decimal deductions,
            decimal netPay)
        {
            EnsurePayrollTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = @"
UPDATE payroll
SET
    EmployeeID = @EmployeeID,
    PayrollMonth = @PayrollMonth,
    PayPeriod = @PayPeriod,
    BasicSalary = @BasicSalary,
    DaysPresent = @DaysPresent,
    DaysAbsent = @DaysAbsent,
    OvertimeHours = @OvertimeHours,
    SSSContribution = @SSSContribution,
    PhilHealthContribution = @PhilHealthContribution,
    PagIBIGContribution = @PagIBIGContribution,
    WithholdingTax = @WithholdingTax,
    GrossPay = @GrossPay,
    Deductions = @Deductions,
    NetPay = @NetPay
WHERE PayrollID = @PayrollID;
";

            command.Parameters.AddWithValue(
                "@PayrollID",
                payrollID);

            AddCommonPayrollParameters(
                command,
                employeeID,
                payrollMonth,
                payPeriod,
                basicSalary,
                daysPresent,
                daysAbsent,
                overtimeHours,
                grossPay,
                sssContribution,
                philHealthContribution,
                pagIbigContribution,
                withholdingTax,
                deductions,
                netPay);

            int rows = command.ExecuteNonQuery();

            return rows > 0;
        }

        // Backward-compatible overload for older code.
        public static bool UpdatePayroll(
            long payrollID,
            long employeeID,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal grossPay,
            decimal deductions,
            decimal netPay)
        {
            return UpdatePayroll(
                payrollID,
                employeeID,
                payrollMonth,
                payPeriod,
                basicSalary,
                daysPresent,
                daysAbsent,
                overtimeHours,
                grossPay,
                0m,
                0m,
                0m,
                0m,
                deductions,
                netPay);
        }

        // ============================================================
        // COMMON PAYROLL PARAMETERS
        // ============================================================

        private static void AddCommonPayrollParameters(
            SqliteCommand command,
            long employeeID,
            string payrollMonth,
            string payPeriod,
            decimal basicSalary,
            int daysPresent,
            int daysAbsent,
            decimal overtimeHours,
            decimal grossPay,
            decimal sssContribution,
            decimal philHealthContribution,
            decimal pagIbigContribution,
            decimal withholdingTax,
            decimal deductions,
            decimal netPay)
        {
            command.Parameters.AddWithValue(
                "@EmployeeID", employeeID);
            command.Parameters.AddWithValue(
                "@PayrollMonth", payrollMonth);
            command.Parameters.AddWithValue(
                "@PayPeriod", payPeriod);
            command.Parameters.AddWithValue(
                "@BasicSalary", basicSalary);
            command.Parameters.AddWithValue(
                "@DaysPresent", daysPresent);
            command.Parameters.AddWithValue(
                "@DaysAbsent", daysAbsent);
            command.Parameters.AddWithValue(
                "@OvertimeHours", overtimeHours);
            command.Parameters.AddWithValue(
                "@SSSContribution", sssContribution);
            command.Parameters.AddWithValue(
                "@PhilHealthContribution", philHealthContribution);
            command.Parameters.AddWithValue(
                "@PagIBIGContribution", pagIbigContribution);
            command.Parameters.AddWithValue(
                "@WithholdingTax", withholdingTax);
            command.Parameters.AddWithValue(
                "@GrossPay", grossPay);
            command.Parameters.AddWithValue(
                "@Deductions", deductions);
            command.Parameters.AddWithValue(
                "@NetPay", netPay);
        }
    }
}
