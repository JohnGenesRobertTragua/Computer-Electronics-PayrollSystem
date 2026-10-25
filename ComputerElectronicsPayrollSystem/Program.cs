using System;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                // Initialize the SQLite database.
                // This will create the database file and
                // the required tables if they do not exist.
                DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The payroll database could not be initialized.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Start the login screen.
            Application.Run(new LoginForm());
        }
    }
}