using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace ComputerElectronicsPayrollSystem
{
    public static class DatabaseHelper
    {
        // ============================================================
        // EMPLOYEE GOVERNMENT IDENTIFICATION
        // ============================================================

        private sealed class EmployeeGovernmentInfo
        {
            public string SSSNo { get; set; } = "";
            public string PhilHealthPIN { get; set; } = "";
            public string PagIBIGMID { get; set; } = "";
            public string TIN { get; set; } = "";
        }

        private static readonly Dictionary<string, EmployeeGovernmentInfo>
            pendingEmployeeGovernmentInfo =
                new Dictionary<string, EmployeeGovernmentInfo>();

        // ============================================================
        // DATABASE PATH
        // ============================================================

        private static readonly string DataFolder =
            Path.Combine(AppContext.BaseDirectory, "Data");

        private static readonly string DatabasePath =
            Path.Combine(DataFolder, "payroll.db");

        private static readonly string ConnectionString =
            $"Data Source={DatabasePath}";

        // ============================================================
        // DATABASE CONNECTION
        // ============================================================

        private static SqliteConnection GetConnection()
        {
            Directory.CreateDirectory(DataFolder);

            SqliteConnection connection =
                new SqliteConnection(ConnectionString);

            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    "PRAGMA foreign_keys = ON;";

                command.ExecuteNonQuery();
            }

            return connection;
        }

        // ============================================================
        // DATABASE INITIALIZATION
        // ============================================================

        public static void InitializeDatabase()
        {
            using SqliteConnection connection =
                GetConnection();

            // ========================================================
            // EMPLOYEES TABLE
            // ========================================================

            string employeesTableSql = @"
                CREATE TABLE IF NOT EXISTS employees
                (
                    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Position TEXT NOT NULL,
                    Department TEXT NOT NULL,
                    Salary REAL NOT NULL,
                    Status TEXT NOT NULL
                );";

            using (SqliteCommand command =
                   connection.CreateCommand())
            {
                command.CommandText =
                    employeesTableSql;

                command.ExecuteNonQuery();
            }

            // Add the government identification columns to existing
            // databases without deleting or rebuilding the employees
            // table.
            EnsureEmployeeGovernmentColumns(connection);

            // Add record-creation timestamps without rebuilding the existing table.
            EnsureCreationTimestampColumn(connection, "employees");

            // ========================================================
            // ATTENDANCE TABLE
            // ========================================================

            string attendanceTableSql = @"
                CREATE TABLE IF NOT EXISTS attendance
                (
                    AttendanceID INTEGER PRIMARY KEY AUTOINCREMENT,
                    EmployeeID INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    TimeIn TEXT,
                    TimeOut TEXT,
                    Status TEXT NOT NULL,

                    FOREIGN KEY (EmployeeID)
                        REFERENCES employees(EmployeeID)
                        ON DELETE CASCADE
                );";

            using (SqliteCommand command =
                   connection.CreateCommand())
            {
                command.CommandText =
                    attendanceTableSql;

                command.ExecuteNonQuery();
            }

            EnsureCreationTimestampColumn(connection, "attendance");
            EnsureCreationTimestampTriggers(connection);
        }

        private static void EnsureCreationTimestampColumn(
            SqliteConnection connection,
            string tableName)
        {
            // tableName is an internal constant at each call site.
            using (SqliteCommand check = connection.CreateCommand())
            {
                check.CommandText = $"PRAGMA table_info([{tableName}]);";
                using SqliteDataReader reader = check.ExecuteReader();
                while (reader.Read())
                {
                    if (string.Equals(reader["name"]?.ToString(), "CreatedAt", StringComparison.OrdinalIgnoreCase))
                        return;
                }
            }

            using (SqliteCommand alter = connection.CreateCommand())
            {
                alter.CommandText = $"ALTER TABLE [{tableName}] ADD COLUMN CreatedAt TEXT;";
                alter.ExecuteNonQuery();
            }

            // Existing records get a stable timestamp ordered by their auto-increment ID.
            string idColumn = tableName == "employees" ? "EmployeeID" : "AttendanceID";
            using (SqliteCommand backfill = connection.CreateCommand())
            {
                backfill.CommandText = $"UPDATE [{tableName}] SET CreatedAt = datetime('now', '+' || [{idColumn}] || ' seconds') WHERE CreatedAt IS NULL OR TRIM(CreatedAt) = '';";
                backfill.ExecuteNonQuery();
            }
        }

        private static void EnsureCreationTimestampTriggers(SqliteConnection connection)
        {
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
CREATE TRIGGER IF NOT EXISTS trg_employees_createdat
AFTER INSERT ON employees
WHEN NEW.CreatedAt IS NULL OR TRIM(NEW.CreatedAt) = ''
BEGIN
    UPDATE employees SET CreatedAt = strftime('%Y-%m-%d %H:%M:%f', 'now') WHERE EmployeeID = NEW.EmployeeID;
END;
CREATE TRIGGER IF NOT EXISTS trg_attendance_createdat
AFTER INSERT ON attendance
WHEN NEW.CreatedAt IS NULL OR TRIM(NEW.CreatedAt) = ''
BEGIN
    UPDATE attendance SET CreatedAt = strftime('%Y-%m-%d %H:%M:%f', 'now') WHERE AttendanceID = NEW.AttendanceID;
END;";
            command.ExecuteNonQuery();
        }

        // ============================================================
        // EMPLOYEE FUNCTIONS
        // ============================================================

        public static long GetNextEmployeeID()
        {
            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                SELECT COALESCE(
                    (
                        SELECT seq + 1
                        FROM sqlite_sequence
                        WHERE name = 'employees'
                    ),
                    1
                );";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            object? result =
                command.ExecuteScalar();

            if (result == null ||
                result == DBNull.Value)
            {
                return 1;
            }

            return Convert.ToInt64(result);
        }

        // ------------------------------------------------------------
        // GET EMPLOYEES
        // ------------------------------------------------------------

        public static DataTable GetEmployees(
            string searchText = "")
        {
            DataTable table =
                new DataTable();

            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                SELECT
                    EmployeeID,
                    FirstName,
                    LastName,
                    Position,
                    Department,
                    Salary,
                    SSSNo,
                    PhilHealthPIN,
                    PagIBIGMID,
                    TIN,
                    Status
                FROM employees
                WHERE
                    @Search = ''
                    OR CAST(EmployeeID AS TEXT)
                        LIKE '%' || @Search || '%'
                    OR FirstName
                        LIKE '%' || @Search || '%'
                    OR LastName
                        LIKE '%' || @Search || '%'
                    OR Position
                        LIKE '%' || @Search || '%'
                    OR Department
                        LIKE '%' || @Search || '%'
                    OR CAST(Salary AS TEXT)
                        LIKE '%' || @Search || '%'
                    OR SSSNo
                        LIKE '%' || @Search || '%'
                    OR PhilHealthPIN
                        LIKE '%' || @Search || '%'
                    OR PagIBIGMID
                        LIKE '%' || @Search || '%'
                    OR TIN
                        LIKE '%' || @Search || '%'
                    OR Status
                        LIKE '%' || @Search || '%'
                ORDER BY EmployeeID;";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@Search",
                searchText?.Trim() ?? "");

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ------------------------------------------------------------
        // ADD EMPLOYEE
        // ------------------------------------------------------------

        public static long AddEmployee(
            string firstName,
            string lastName,
            string position,
            string department,
            decimal salary,
            string status)
        {
            EmployeeGovernmentInfo? governmentInfo =
                ConsumeFirstPendingEmployeeGovernmentInfo();

            return AddEmployeeInternal(
                firstName,
                lastName,
                position,
                department,
                salary,
                status,
                governmentInfo);
        }

        // ------------------------------------------------------------
        // ADD EMPLOYEE COMPATIBILITY OVERLOAD
        // ------------------------------------------------------------
        // This matches the version used by the current
        // EmployeesControl if it passes the generated ID.

        public static long AddEmployee(
            string employeeID,
            string firstName,
            string lastName,
            string position,
            string department,
            decimal salary,
            string status)
        {
            EmployeeGovernmentInfo? governmentInfo =
                ConsumePendingEmployeeGovernmentInfo(employeeID);

            return AddEmployeeInternal(
                firstName,
                lastName,
                position,
                department,
                salary,
                status,
                governmentInfo);
        }

        // ------------------------------------------------------------
        // UPDATE EMPLOYEE
        // ------------------------------------------------------------

        public static bool UpdateEmployee(
            long employeeID,
            string firstName,
            string lastName,
            string position,
            string department,
            decimal salary,
            string status)
        {
            try
            {
                using SqliteConnection connection =
                    GetConnection();

                EmployeeGovernmentInfo? governmentInfo =
                    ConsumePendingEmployeeGovernmentInfo(
                        employeeID.ToString());

                string sql;

                if (governmentInfo != null)
                {
                    sql = @"
                        UPDATE employees
                        SET
                            FirstName = @FirstName,
                            LastName = @LastName,
                            Position = @Position,
                            Department = @Department,
                            Salary = @Salary,
                            SSSNo = @SSSNo,
                            PhilHealthPIN = @PhilHealthPIN,
                            PagIBIGMID = @PagIBIGMID,
                            TIN = @TIN,
                            Status = @Status
                        WHERE EmployeeID = @EmployeeID;";
                }
                else
                {
                    // Preserve the existing government IDs when an
                    // update is made by an older screen that does not
                    // supply the new fields.
                    sql = @"
                        UPDATE employees
                        SET
                            FirstName = @FirstName,
                            LastName = @LastName,
                            Position = @Position,
                            Department = @Department,
                            Salary = @Salary,
                            Status = @Status
                        WHERE EmployeeID = @EmployeeID;";
                }

                using SqliteCommand command =
                    connection.CreateCommand();

                command.CommandText = sql;

                command.Parameters.AddWithValue(
                    "@EmployeeID",
                    employeeID);

                command.Parameters.AddWithValue(
                    "@FirstName",
                    firstName);

                command.Parameters.AddWithValue(
                    "@LastName",
                    lastName);

                command.Parameters.AddWithValue(
                    "@Position",
                    position);

                command.Parameters.AddWithValue(
                    "@Department",
                    department);

                command.Parameters.AddWithValue(
                    "@Salary",
                    salary);

                if (governmentInfo != null)
                {
                    command.Parameters.AddWithValue(
                        "@SSSNo",
                        governmentInfo.SSSNo);

                    command.Parameters.AddWithValue(
                        "@PhilHealthPIN",
                        governmentInfo.PhilHealthPIN);

                    command.Parameters.AddWithValue(
                        "@PagIBIGMID",
                        governmentInfo.PagIBIGMID);

                    command.Parameters.AddWithValue(
                        "@TIN",
                        governmentInfo.TIN);
                }

                command.Parameters.AddWithValue(
                    "@Status",
                    status);

                int affectedRows =
                    command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }


        // ============================================================
        // EMPLOYEE GOVERNMENT ID HELPERS
        // ============================================================

        private static void EnsureEmployeeGovernmentColumns(
            SqliteConnection connection)
        {
            HashSet<string> existingColumns =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            using (SqliteCommand pragmaCommand =
                   connection.CreateCommand())
            {
                pragmaCommand.CommandText =
                    "PRAGMA table_info(employees);";

                using SqliteDataReader reader =
                    pragmaCommand.ExecuteReader();

                while (reader.Read())
                {
                    string columnName =
                        reader["name"]?.ToString() ?? "";

                    if (!string.IsNullOrWhiteSpace(columnName))
                    {
                        existingColumns.Add(columnName);
                    }
                }
            }

            string[] requiredColumns =
            {
                "SSSNo",
                "PhilHealthPIN",
                "PagIBIGMID",
                "TIN"
            };

            foreach (string column in requiredColumns)
            {
                if (existingColumns.Contains(column))
                    continue;

                using SqliteCommand alterCommand =
                    connection.CreateCommand();

                alterCommand.CommandText =
                    $"ALTER TABLE employees " +
                    $"ADD COLUMN {column} TEXT NOT NULL DEFAULT '';";

                alterCommand.ExecuteNonQuery();
            }
        }

        public static void StageEmployeeGovernmentIDs(
            string employeeID,
            string sssNo,
            string philHealthPIN,
            string pagIBIGMID,
            string tin)
        {
            if (string.IsNullOrWhiteSpace(employeeID))
                return;

            pendingEmployeeGovernmentInfo[employeeID.Trim()] =
                new EmployeeGovernmentInfo
                {
                    SSSNo = sssNo?.Trim() ?? "",
                    PhilHealthPIN =
                        philHealthPIN?.Trim() ?? "",
                    PagIBIGMID =
                        pagIBIGMID?.Trim() ?? "",
                    TIN = tin?.Trim() ?? ""
                };
        }

        private static EmployeeGovernmentInfo?
            ConsumePendingEmployeeGovernmentInfo(
                string employeeID)
        {
            string key =
                employeeID?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(key))
                return null;

            if (!pendingEmployeeGovernmentInfo.TryGetValue(
                    key,
                    out EmployeeGovernmentInfo? info))
            {
                return null;
            }

            pendingEmployeeGovernmentInfo.Remove(key);

            return info;
        }

        private static EmployeeGovernmentInfo?
            ConsumeFirstPendingEmployeeGovernmentInfo()
        {
            string? firstKey = null;
            EmployeeGovernmentInfo? firstValue = null;

            foreach (
                KeyValuePair<string, EmployeeGovernmentInfo> item
                in pendingEmployeeGovernmentInfo)
            {
                firstKey = item.Key;
                firstValue = item.Value;
                break;
            }

            if (firstKey == null || firstValue == null)
                return null;

            pendingEmployeeGovernmentInfo.Remove(firstKey);
            return firstValue;
        }

        public static void GetEmployeeGovernmentIDs(
            long employeeID,
            out string sssNo,
            out string philHealthPIN,
            out string pagIBIGMID,
            out string tin)
        {
            sssNo = "";
            philHealthPIN = "";
            pagIBIGMID = "";
            tin = "";

            try
            {
                using SqliteConnection connection =
                    GetConnection();

                string sql = @"
                    SELECT
                        COALESCE(SSSNo, '') AS SSSNo,
                        COALESCE(PhilHealthPIN, '') AS PhilHealthPIN,
                        COALESCE(PagIBIGMID, '') AS PagIBIGMID,
                        COALESCE(TIN, '') AS TIN
                    FROM employees
                    WHERE EmployeeID = @EmployeeID;";

                using SqliteCommand command =
                    connection.CreateCommand();

                command.CommandText = sql;

                command.Parameters.AddWithValue(
                    "@EmployeeID",
                    employeeID);

                using SqliteDataReader reader =
                    command.ExecuteReader();

                if (!reader.Read())
                    return;

                sssNo =
                    reader["SSSNo"]?.ToString() ?? "";

                philHealthPIN =
                    reader["PhilHealthPIN"]?.ToString() ?? "";

                pagIBIGMID =
                    reader["PagIBIGMID"]?.ToString() ?? "";

                tin =
                    reader["TIN"]?.ToString() ?? "";
            }
            catch
            {
                // Leave the output values empty if the employee
                // government information cannot be loaded.
            }
        }

        private static long AddEmployeeInternal(
            string firstName,
            string lastName,
            string position,
            string department,
            decimal salary,
            string status,
            EmployeeGovernmentInfo? governmentInfo)
        {
            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                INSERT INTO employees
                (
                    FirstName,
                    LastName,
                    Position,
                    Department,
                    Salary,
                    SSSNo,
                    PhilHealthPIN,
                    PagIBIGMID,
                    TIN,
                    Status
                )
                VALUES
                (
                    @FirstName,
                    @LastName,
                    @Position,
                    @Department,
                    @Salary,
                    @SSSNo,
                    @PhilHealthPIN,
                    @PagIBIGMID,
                    @TIN,
                    @Status
                );";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@FirstName",
                firstName);

            command.Parameters.AddWithValue(
                "@LastName",
                lastName);

            command.Parameters.AddWithValue(
                "@Position",
                position);

            command.Parameters.AddWithValue(
                "@Department",
                department);

            command.Parameters.AddWithValue(
                "@Salary",
                salary);

            command.Parameters.AddWithValue(
                "@SSSNo",
                governmentInfo?.SSSNo ?? "");

            command.Parameters.AddWithValue(
                "@PhilHealthPIN",
                governmentInfo?.PhilHealthPIN ?? "");

            command.Parameters.AddWithValue(
                "@PagIBIGMID",
                governmentInfo?.PagIBIGMID ?? "");

            command.Parameters.AddWithValue(
                "@TIN",
                governmentInfo?.TIN ?? "");

            command.Parameters.AddWithValue(
                "@Status",
                status);

            command.ExecuteNonQuery();

            using SqliteCommand idCommand =
                connection.CreateCommand();

            idCommand.CommandText =
                "SELECT last_insert_rowid();";

            object? result =
                idCommand.ExecuteScalar();

            if (result == null ||
                result == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt64(result);
        }

        // ------------------------------------------------------------
        // DELETE EMPLOYEE
        // ------------------------------------------------------------

        public static bool DeleteEmployee(
            long employeeID)
        {
            try
            {
                using SqliteConnection connection =
                    GetConnection();

                string sql = @"
                    DELETE FROM employees
                    WHERE EmployeeID = @EmployeeID;";

                using SqliteCommand command =
                    connection.CreateCommand();

                command.CommandText = sql;

                command.Parameters.AddWithValue(
                    "@EmployeeID",
                    employeeID);

                int affectedRows =
                    command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch
            {
                return false;
            }
        }

        // ============================================================
        // ATTENDANCE FUNCTIONS
        // ============================================================

        // ------------------------------------------------------------
        // GET EMPLOYEES FOR ATTENDANCE DROPDOWN
        // ------------------------------------------------------------

        public static DataTable GetAttendanceEmployees()
        {
            DataTable table =
                new DataTable();

            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                SELECT
                    EmployeeID,
                    FirstName,
                    LastName,
                    FirstName || ' ' || LastName
                        AS EmployeeName
                FROM employees
                ORDER BY
                    LastName,
                    FirstName;";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ------------------------------------------------------------
        // GET ATTENDANCE
        // ------------------------------------------------------------

        public static DataTable GetAttendance(
            string searchText = "")
        {
            DataTable table =
                new DataTable();

            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                SELECT
                    a.AttendanceID,
                    a.EmployeeID,

                    e.FirstName || ' ' || e.LastName
                        AS EmployeeName,

                    a.Date,
                    a.TimeIn,
                    a.TimeOut,
                    a.Status

                FROM attendance a

                INNER JOIN employees e
                    ON a.EmployeeID =
                       e.EmployeeID

                WHERE
                    @Search = ''

                    OR CAST(a.AttendanceID AS TEXT)
                        LIKE '%' || @Search || '%'

                    OR CAST(a.EmployeeID AS TEXT)
                        LIKE '%' || @Search || '%'

                    OR e.FirstName
                        LIKE '%' || @Search || '%'

                    OR e.LastName
                        LIKE '%' || @Search || '%'

                    OR a.Date
                        LIKE '%' || @Search || '%'

                    OR a.TimeIn
                        LIKE '%' || @Search || '%'

                    OR a.TimeOut
                        LIKE '%' || @Search || '%'

                    OR a.Status
                        LIKE '%' || @Search || '%'

                ORDER BY
                    a.Date DESC,
                    a.AttendanceID DESC;";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@Search",
                searchText?.Trim() ?? "");

            using SqliteDataReader reader =
                command.ExecuteReader();

            table.Load(reader);

            return table;
        }

        // ------------------------------------------------------------
        // ADD ATTENDANCE
        // ------------------------------------------------------------

        public static void AddAttendance(
            long employeeID,
            string date,
            string? timeIn,
            string? timeOut,
            string status)
        {
            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                INSERT INTO attendance
                (
                    EmployeeID,
                    Date,
                    TimeIn,
                    TimeOut,
                    Status
                )
                VALUES
                (
                    @EmployeeID,
                    @Date,
                    @TimeIn,
                    @TimeOut,
                    @Status
                );";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@EmployeeID",
                employeeID);

            command.Parameters.AddWithValue(
                "@Date",
                date);

            command.Parameters.AddWithValue(
                "@TimeIn",
                string.IsNullOrWhiteSpace(timeIn)
                    ? (object)DBNull.Value
                    : timeIn);

            command.Parameters.AddWithValue(
                "@TimeOut",
                string.IsNullOrWhiteSpace(timeOut)
                    ? (object)DBNull.Value
                    : timeOut);

            command.Parameters.AddWithValue(
                "@Status",
                status);

            command.ExecuteNonQuery();
        }

        // ------------------------------------------------------------
        // UPDATE ATTENDANCE
        // ------------------------------------------------------------

        public static void UpdateAttendance(
            long attendanceID,
            long employeeID,
            string date,
            string? timeIn,
            string? timeOut,
            string status)
        {
            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                UPDATE attendance
                SET
                    EmployeeID = @EmployeeID,
                    Date = @Date,
                    TimeIn = @TimeIn,
                    TimeOut = @TimeOut,
                    Status = @Status
                WHERE
                    AttendanceID =
                    @AttendanceID;";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@AttendanceID",
                attendanceID);

            command.Parameters.AddWithValue(
                "@EmployeeID",
                employeeID);

            command.Parameters.AddWithValue(
                "@Date",
                date);

            command.Parameters.AddWithValue(
                "@TimeIn",
                string.IsNullOrWhiteSpace(timeIn)
                    ? (object)DBNull.Value
                    : timeIn);

            command.Parameters.AddWithValue(
                "@TimeOut",
                string.IsNullOrWhiteSpace(timeOut)
                    ? (object)DBNull.Value
                    : timeOut);

            command.Parameters.AddWithValue(
                "@Status",
                status);

            command.ExecuteNonQuery();
        }

        // ------------------------------------------------------------
        // DELETE ATTENDANCE
        // ------------------------------------------------------------

        public static void DeleteAttendance(
            long attendanceID)
        {
            using SqliteConnection connection =
                GetConnection();

            string sql = @"
                DELETE FROM attendance
                WHERE AttendanceID =
                      @AttendanceID;";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@AttendanceID",
                attendanceID);

            command.ExecuteNonQuery();
        }

        // ============================================================
        // CONNECTION TEST
        // ============================================================

        public static bool TestConnection()
        {
            try
            {
                using SqliteConnection connection =
                    GetConnection();

                return connection.State ==
                       ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}