# Computer & Electronics Shop Payroll System

A Windows desktop payroll management application for a computer and
electronics components shop. Built with **C#**, **Windows Forms**,
**.NET 10**, and **SQLite**.

The system brings employee records, attendance, payroll calculations,
and reporting into one desktop application.

## Features

-   **Dashboard** --- overview of the payroll system and recent payroll
    activity.
-   **Employee management** --- add, view, update, and manage employee
    information.
-   **Attendance management** --- record attendance, including time-in
    and time-out, and review attendance history.
-   **Payroll processing** --- select an employee and pay period,
    calculate payroll figures, review deductions, and process payment.
-   **Payroll receipts** --- view payroll receipt details for processed
    payroll.
-   **Reports** --- generate employee, payroll, attendance, and
    payment-related reports, with CSV export where supported by the
    application.
-   **Search and sorting** --- search records and sort data-grid results
    in supported screens.
-   **Keyboard navigation** --- keyboard-friendly navigation across
    supported form fields.
-   **Settings** --- access the application's available configuration
    options.

> Payroll rules, deductions, and displayed totals should be reviewed
> against the shop's actual policies and applicable Philippine
> requirements before the system is used for official payroll.

## Technology Stack

-   **Language:** C#
-   **UI:** Windows Forms
-   **Framework:** .NET 10 for Windows (`net10.0-windows`)
-   **Database:** SQLite
-   **Database package:** `Microsoft.Data.Sqlite`

## Requirements

-   Windows
-   Visual Studio 2026 (or another compatible IDE with the .NET 10 SDK
    and Windows Forms support)
-   .NET 10 SDK

## Getting Started

### 1. Clone the repository

``` powershell
git clone https://github.com/JohnGenesRobertTragua/Computer-Electronics-PayrollSystem.git
cd Computer-Electronics-PayrollSystem
```

### 2. Open the solution

Open `ComputerElectronicsPayrollSystem.slnx` in Visual Studio.

### 3. Restore dependencies

Visual Studio should restore the NuGet package dependencies
automatically. If needed, run this from the directory containing the
project file:

``` powershell
dotnet restore .\ComputerElectronicsPayrollSystem\ComputerElectronicsPayrollSystem.csproj
```

### 4. Build and run

In Visual Studio, select the project as the startup project and run it
with **F5** or **Ctrl+F5**.

Alternatively, from the repository root:

``` powershell
dotnet run --project .\ComputerElectronicsPayrollSystem\ComputerElectronicsPayrollSystem.csproj
```

## Project Structure

``` text
Computer-Electronics-PayrollSystem/
├── ComputerElectronicsPayrollSystem.slnx
└── ComputerElectronicsPayrollSystem/
    ├── ComputerElectronicsPayrollSystem.csproj
    ├── Program.cs
    ├── DashboardForm.cs
    ├── LoginForm.cs
    ├── EmployeesControl.cs
    ├── AddEmployeeForm.cs
    ├── UpdateEmployeeForm.cs
    ├── AttendanceControl.cs
    ├── PayrollControl.cs
    ├── PayEmployeeForm.cs
    ├── PayrollReceiptForm.cs
    ├── ReportsControl.cs
    ├── SettingsControl.cs
    └── DatabaseHelper.cs
```

The application also includes form Designer files, UI helpers,
keyboard-navigation helpers, and payroll database utilities. The tree
above highlights the main entry points and screens rather than listing
every file.

## Database and Data Safety

-   The application uses SQLite. Review the database initialization and
    storage code before moving the application to another computer.
-   Back up the database before making changes or testing migrations.
-   Do **not** commit real employee personal information, payroll
    records, database backups, passwords, API keys, or other
    confidential data to a public repository.
-   Keep local configuration and user-specific Visual Studio files out
    of version control.
-   Test the system with sample data before using it for live payroll.

## Current Scope

This repository contains the Windows desktop application source code. It
is not a hosted web application; users run it on a compatible Windows
computer.

## Author

**John Genes Robert A. Tragua**

GitHub:
[@JohnGenesRobertTragua](https://github.com/JohnGenesRobertTragua)

## License

No license is specified in this repository yet. Unless a license is
added, others should not assume they have permission to redistribute or
modify the project.
