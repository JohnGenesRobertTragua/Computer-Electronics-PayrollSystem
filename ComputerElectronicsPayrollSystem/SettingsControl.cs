using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    /// <summary>
    /// Application settings for company information. A recovery backup is
    /// created automatically at application startup and is not exposed as
    /// a manual operation in the Settings interface.
    /// </summary>
    public sealed class SettingsControl : UserControl
    {
        private static readonly string DataFolder =
            Path.Combine(AppContext.BaseDirectory, "Data");

        private static readonly string DatabasePath =
            Path.Combine(DataFolder, "payroll.db");

        // Store backups outside the application's bin/output folder so a
        // Visual Studio rebuild or app update does not normally remove them.
        private static readonly string BackupFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ComputerElectronicsShopPayrollSystem",
            "Backups");

        private const string AutomaticBackupPrefix = "payroll_auto_backup_";
        private const int AutomaticBackupRetentionCount = 14;
        private static readonly object AutomaticBackupLock = new object();

        private static readonly Color PageBackColor = Color.FromArgb(244, 247, 251);
        private static readonly Color CardBackColor = Color.White;
        private static readonly Color HeadingColor = Color.FromArgb(24, 38, 61);
        private static readonly Color MutedTextColor = Color.FromArgb(91, 107, 129);
        private static readonly Color PrimaryColor = Color.FromArgb(37, 99, 235);
        private static readonly Color SoftSuccessColor = Color.FromArgb(236, 253, 245);
        private static readonly Color SoftErrorColor = Color.FromArgb(254, 242, 242);

        private static string ConnectionString =>
            $"Data Source={DatabasePath}";

        private readonly TextBox txtCompanyName = new TextBox();
        private readonly TextBox txtPhone = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtAddress = new TextBox();
        private readonly Label lblSaveStatus = new Label();
        private readonly Label lblAutomaticBackupStatus = new Label();

        private readonly Label lblCompanyName = new Label();
        private readonly Label lblPhone = new Label();
        private readonly Label lblEmail = new Label();
        private readonly Label lblAddress = new Label();
        private readonly Label lblAboutDetails = new Label();
        private readonly Button btnSave = new Button();

        private GroupBox companyGroup = new GroupBox();
        private GroupBox aboutGroup = new GroupBox();
        private Panel contentPanel = new Panel();

        public SettingsControl()
        {
            BackColor = SystemColors.Control;
            BuildLayout();
            VisibleChanged += (_, _) => { if (Visible) FuturisticUiTheme.Apply(this); };
            EnsureSettingsTable();
            RefreshSettings();
            Resize += SettingsControl_Resize;

            // Keep recovery snapshots behind the scenes; no manual backup
            // action is required from the shop user.
            DataRefreshManager.DataChanged += DataRefreshManager_DataChanged;
            Disposed += SettingsControl_Disposed;
            CreateAutomaticBackup("application startup");
        }

        public void RefreshSettings()
        {
            try
            {
                EnsureSettingsTable();
                txtCompanyName.Text = GetSettingValue(
                    "CompanyName", "Computer & Electronics Shop");
                txtPhone.Text = GetSettingValue("CompanyPhone", "");
                txtEmail.Text = GetSettingValue("CompanyEmail", "");
                txtAddress.Text = GetSettingValue("CompanyAddress", "");
                lblSaveStatus.Text = "";
            }
            catch (Exception ex)
            {
                lblSaveStatus.Text = "Unable to load settings: " + ex.Message;
            }
        }

        public static string GetSettingValue(
            string key,
            string defaultValue = "")
        {
            EnsureSettingsTable();

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
SELECT SettingValue
FROM application_settings
WHERE SettingKey = @SettingKey
LIMIT 1;";
            command.Parameters.AddWithValue("@SettingKey", key);

            object? result = command.ExecuteScalar();
            return result == null || result == DBNull.Value
                ? defaultValue
                : Convert.ToString(result) ?? defaultValue;
        }

        private static void EnsureSettingsTable()
        {
            Directory.CreateDirectory(DataFolder);

            using SqliteConnection connection =
                new SqliteConnection(ConnectionString);
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS application_settings
(
    SettingKey TEXT PRIMARY KEY NOT NULL,
    SettingValue TEXT NOT NULL DEFAULT ''
);";
            command.ExecuteNonQuery();
        }

        private void BuildLayout()
        {
            BackColor = PageBackColor;
            Padding = new Padding(0);

            Label title = new Label
            {
                Name = "lblSettingsTitle",
                Text = "SETTINGS",
                Font = new Font("Segoe UI", 21F, FontStyle.Bold),
                ForeColor = HeadingColor,
                AutoSize = true,
                Location = new Point(24, 13)
            };

            Label subtitle = new Label
            {
                Name = "lblSettingsSubtitle",
                Text = "Manage company details and application recovery protection",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = MutedTextColor,
                AutoSize = true,
                Location = new Point(27, 53)
            };

            contentPanel = new Panel
            {
                Name = "pnlSettingsContent",
                Location = new Point(20, 82),
                Size = new Size(Math.Max(500, ClientSize.Width - 40),
                                Math.Max(350, ClientSize.Height - 96)),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                AutoScroll = true,
                BackColor = PageBackColor
            };

            BuildCompanyGroup();
            BuildAboutGroup();

            contentPanel.Controls.Add(companyGroup);
            contentPanel.Controls.Add(aboutGroup);

            Controls.Add(contentPanel);
            Controls.Add(subtitle);
            Controls.Add(title);

            LayoutGroups();
        }

        private void BuildCompanyGroup()
        {
            companyGroup = new GroupBox
            {
                Name = "grpCompanyInformation",
                Text = "  COMPANY INFORMATION  ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = HeadingColor,
                BackColor = CardBackColor,
                Location = new Point(0, 0),
                Size = new Size(900, 252),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            ConfigureFieldLabel(lblCompanyName, "Company Name", 22, 35);
            ConfigureFieldLabel(lblPhone, "Contact Number", 22, 35);
            ConfigureFieldLabel(lblEmail, "Email Address", 22, 110);
            ConfigureFieldLabel(lblAddress, "Business Address", 22, 110);

            ConfigureTextBox(txtCompanyName);
            ConfigureTextBox(txtPhone);
            ConfigureTextBox(txtEmail);
            ConfigureTextBox(txtAddress);

            txtAddress.Multiline = true;
            txtAddress.ScrollBars = ScrollBars.Vertical;
            txtAddress.Height = 54;

            btnSave.Name = "btnSaveCompanySettings";
            btnSave.Text = "SAVE CHANGES";
            btnSave.Size = new Size(142, 36);
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.BackColor = PrimaryColor;
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 64, 175);
            btnSave.Cursor = Cursors.Hand;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            lblSaveStatus.AutoSize = false;
            lblSaveStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSaveStatus.ForeColor = MutedTextColor;
            lblSaveStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblSaveStatus.AutoEllipsis = true;

            companyGroup.Controls.AddRange(new Control[]
            {
                lblCompanyName, txtCompanyName,
                lblPhone, txtPhone,
                lblEmail, txtEmail,
                lblAddress, txtAddress,
                btnSave, lblSaveStatus
            });
        }

        private static void ConfigureFieldLabel(Label label, string text, int x, int y)
        {
            label.Text = text;
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label.ForeColor = MutedTextColor;
            label.Location = new Point(x, y);
        }

        private static void ConfigureTextBox(TextBox textBox)
        {
            textBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            textBox.ForeColor = HeadingColor;
            textBox.BackColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void BuildAboutGroup()
        {
            aboutGroup = new GroupBox
            {
                Name = "grpApplicationInformation",
                Text = "  APPLICATION & BACKUP PROTECTION  ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = HeadingColor,
                BackColor = CardBackColor,
                Location = new Point(0, 268),
                Size = new Size(900, 175),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            lblAboutDetails.Text =
                "Computer & Electronics Shop Payroll System\r\n" +
                "Company details are saved in the existing payroll database. " +
                "Automatic recovery copies are created at startup and after saved data changes.";
            lblAboutDetails.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblAboutDetails.ForeColor = MutedTextColor;
            lblAboutDetails.Location = new Point(22, 34);
            lblAboutDetails.Size = new Size(820, 43);
            lblAboutDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblAutomaticBackupStatus.AutoSize = false;
            lblAutomaticBackupStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAutomaticBackupStatus.Location = new Point(22, 88);
            lblAutomaticBackupStatus.Size = new Size(820, 55);
            lblAutomaticBackupStatus.Anchor =
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAutomaticBackupStatus.ForeColor = MutedTextColor;
            lblAutomaticBackupStatus.BackColor = Color.FromArgb(239, 246, 255);
            lblAutomaticBackupStatus.Padding = new Padding(12, 4, 10, 4);
            lblAutomaticBackupStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblAutomaticBackupStatus.AutoEllipsis = true;
            lblAutomaticBackupStatus.Text =
                "Automatic recovery copies are managed by the application; no manual backup action is needed.";

            aboutGroup.Controls.AddRange(new Control[]
            {
                lblAboutDetails,
                lblAutomaticBackupStatus
            });
        }

        private void LayoutGroups()
        {
            if (contentPanel == null || contentPanel.IsDisposed ||
                companyGroup == null || aboutGroup == null)
            {
                return;
            }

            int width = Math.Max(500, contentPanel.ClientSize.Width - 18);
            companyGroup.Width = width;
            aboutGroup.Width = width;

            const int leftMargin = 22;
            const int rightMargin = 22;
            const int columnGap = 34;
            int columnWidth = Math.Max(210,
                (width - leftMargin - rightMargin - columnGap) / 2);
            int rightX = leftMargin + columnWidth + columnGap;

            lblCompanyName.Location = new Point(leftMargin, 35);
            txtCompanyName.Location = new Point(leftMargin, 58);
            txtCompanyName.Size = new Size(columnWidth, 29);

            lblPhone.Location = new Point(rightX, 35);
            txtPhone.Location = new Point(rightX, 58);
            txtPhone.Size = new Size(columnWidth, 29);

            lblEmail.Location = new Point(leftMargin, 108);
            txtEmail.Location = new Point(leftMargin, 131);
            txtEmail.Size = new Size(columnWidth, 29);

            lblAddress.Location = new Point(rightX, 108);
            txtAddress.Location = new Point(rightX, 131);
            txtAddress.Size = new Size(columnWidth, 55);

            btnSave.Location = new Point(leftMargin, 198);
            lblSaveStatus.Location = new Point(btnSave.Right + 16, 202);
            lblSaveStatus.Size = new Size(Math.Max(150, width - lblSaveStatus.Left - rightMargin), 30);

            lblAboutDetails.Location = new Point(leftMargin, 34);
            lblAboutDetails.Size = new Size(Math.Max(250, width - leftMargin - rightMargin), 43);
            lblAutomaticBackupStatus.Location = new Point(leftMargin, 88);
            lblAutomaticBackupStatus.Size = new Size(Math.Max(250, width - leftMargin - rightMargin), 55);

            aboutGroup.Location = new Point(0, companyGroup.Bottom + 14);
            contentPanel.AutoScrollMinSize = new Size(0, aboutGroup.Bottom + 20);
        }

        private void SettingsControl_Resize(object? sender, EventArgs e)
        {
            if (contentPanel == null || contentPanel.IsDisposed)
                return;

            contentPanel.Size = new Size(
                Math.Max(500, ClientSize.Width - 40),
                Math.Max(350, ClientSize.Height - 96));

            LayoutGroups();
        }



        private void btnSave_Click(object? sender, EventArgs e)
        {
            string companyName = txtCompanyName.Text.Trim();
            if (string.IsNullOrWhiteSpace(companyName))
            {
                lblSaveStatus.ForeColor = Color.Firebrick;
                lblSaveStatus.Text = "Company name is required.";
                txtCompanyName.Focus();
                return;
            }

            try
            {
                EnsureSettingsTable();
                using SqliteConnection connection = new SqliteConnection(ConnectionString);
                connection.Open();
                using SqliteTransaction transaction = connection.BeginTransaction();

                SaveSetting(connection, transaction, "CompanyName", companyName);
                SaveSetting(connection, transaction, "CompanyPhone", txtPhone.Text.Trim());
                SaveSetting(connection, transaction, "CompanyEmail", txtEmail.Text.Trim());
                SaveSetting(connection, transaction, "CompanyAddress", txtAddress.Text.Trim());

                transaction.Commit();

                CreateAutomaticBackup("company settings saved");

                lblSaveStatus.ForeColor = Color.DarkGreen;
                lblSaveStatus.Text = "✓ Company information saved successfully.";
            }
            catch (Exception ex)
            {
                lblSaveStatus.ForeColor = Color.Firebrick;
                lblSaveStatus.Text = "Unable to save settings: " + ex.Message;
            }
        }

        private static void SaveSetting(
            SqliteConnection connection,
            SqliteTransaction transaction,
            string key,
            string value)
        {
            using SqliteCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"
INSERT INTO application_settings (SettingKey, SettingValue)
VALUES (@SettingKey, @SettingValue)
ON CONFLICT(SettingKey) DO UPDATE SET
    SettingValue = excluded.SettingValue;";
            command.Parameters.AddWithValue("@SettingKey", key);
            command.Parameters.AddWithValue("@SettingValue", value ?? "");
            command.ExecuteNonQuery();
        }

        private void DataRefreshManager_DataChanged(
            object? sender,
            DataChangedEventArgs e)
        {
            string reason = e.ChangeType switch
            {
                DataChangeType.Employee => "employee data changed",
                DataChangeType.Attendance => "attendance data changed",
                DataChangeType.Payroll => "payroll data changed",
                _ => "application data changed"
            };

            CreateAutomaticBackup(reason);
        }

        private void SettingsControl_Disposed(object? sender, EventArgs e)
        {
            DataRefreshManager.DataChanged -= DataRefreshManager_DataChanged;
            Disposed -= SettingsControl_Disposed;
        }

        private void CreateAutomaticBackup(string reason)
        {
            string backupPath = "";
            bool backupCompleted = false;
            string statusMessage;
            Color statusColor;

            lock (AutomaticBackupLock)
            {
                try
                {
                    if (!File.Exists(DatabasePath))
                    {
                        throw new FileNotFoundException(
                            "The payroll database was not found.",
                            DatabasePath);
                    }

                    Directory.CreateDirectory(BackupFolder);

                    string uniqueStamp =
                        DateTime.Now.ToString("yyyyMMdd_HHmmss_fffffff") + "_" +
                        Guid.NewGuid().ToString("N").Substring(0, 6);

                    backupPath = Path.Combine(
                        BackupFolder,
                        AutomaticBackupPrefix + uniqueStamp + ".db");

                    // SQLite's online backup API produces a consistent copy
                    // even if the database is in use by the application.
                    using (SqliteConnection source =
                           new SqliteConnection(ConnectionString))
                    {
                        source.Open();

                        using SqliteConnection destination =
                            new SqliteConnection($"Data Source={backupPath}");
                        destination.Open();

                        source.BackupDatabase(destination);
                    }

                    if (!File.Exists(backupPath) ||
                        new FileInfo(backupPath).Length == 0)
                    {
                        throw new IOException(
                            "The automatic backup file was not created correctly.");
                    }

                    backupCompleted = true;
                    CleanupOldAutomaticBackups();

                    statusMessage =
                        $"Recovery copy created after {reason} at {DateTime.Now:HH:mm:ss}. " +
                        $"The latest {AutomaticBackupRetentionCount} automatic copies are retained in your Windows user data folder.";
                    statusColor = Color.DarkGreen;
                }
                catch (Exception ex)
                {
                    if (!backupCompleted && !string.IsNullOrWhiteSpace(backupPath))
                    {
                        try
                        {
                            if (File.Exists(backupPath))
                                File.Delete(backupPath);
                        }
                        catch
                        {
                            // Leave cleanup for a later run if the file is locked.
                        }
                    }

                    statusMessage =
                        "Automatic database backup failed. Please ask the system administrator to check the automatic backup folder. Details: " +
                        ex.Message;
                    statusColor = Color.Firebrick;
                }
            }

            SetAutomaticBackupStatus(statusMessage, statusColor);
        }

        private void SetAutomaticBackupStatus(string message, Color color)
        {
            if (IsDisposed || Disposing || lblAutomaticBackupStatus.IsDisposed)
                return;

            if (InvokeRequired && IsHandleCreated)
            {
                try
                {
                    BeginInvoke(new Action(() =>
                        SetAutomaticBackupStatus(message, color)));
                }
                catch (InvalidOperationException)
                {
                    // The Settings control may be closing while a data event is raised.
                }

                return;
            }

            lblAutomaticBackupStatus.Text = message;
            lblAutomaticBackupStatus.ForeColor = color;
            lblAutomaticBackupStatus.BackColor =
                color == Color.Firebrick ? SoftErrorColor :
                color == Color.DarkGreen ? SoftSuccessColor :
                Color.FromArgb(239, 246, 255);
        }

        private static void CleanupOldAutomaticBackups()
        {
            if (!Directory.Exists(BackupFolder))
                return;

            FileInfo[] backupFiles = Directory
                .GetFiles(BackupFolder, AutomaticBackupPrefix + "*.db")
                .Select(path => new FileInfo(path))
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .ToArray();

            foreach (FileInfo oldBackup in backupFiles.Skip(AutomaticBackupRetentionCount))
            {
                try
                {
                    oldBackup.Delete();
                }
                catch
                {
                    // A locked backup is left in place; it will be considered again later.
                }
            }
        }
    }
}
