using System;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class DashboardForm : Form
    {
        private EmployeesControl? employeesControl;
        private AttendanceControl? attendanceControl;

        // PAYROLL ADDITION
        private PayrollControl? payrollControl;

        // REPORTS ADDITION
        private ReportsControl? reportsControl;

        // SETTINGS
        private SettingsControl? settingsControl;

        public DashboardForm()
        {
            InitializeComponent();

            // Apply the refreshed Dashboard layout without changing the
            // existing Designer controls or module navigation.
            ConfigureDashboardAppearance();

            // Reflow the Dashboard when the main area is resized.
            pnlMain.Resize += (_, _) => LayoutDashboardContent();
            pnlSidebar.Resize += (_, _) => LayoutSidebarNavigation();

            // Enable keyboard navigation for the Dashboard.
            KeyboardNavigation.Enable(this);

            // Connect sidebar buttons intentionally.
            btnDashboard.Click += btnDashboard_Click;
            btnEmployees.Click += btnEmployees_Click;
            btnAttendance.Click += btnAttendance_Click;

            // PAYROLL ADDITION
            btnPayroll.Click += btnPayroll_Click;

            // REPORTS
            btnReports.Click += btnReports_Click;

            // SETTINGS
            btnSettings.Click += btnSettings_Click;

            // Keep the Dashboard synchronized with all live data changes.
            DataRefreshManager.DataChanged +=
                DataRefreshManager_DataChanged;

            FormClosed +=
                DashboardForm_FormClosed;

            // Prepare Employees screen.
            employeesControl = new EmployeesControl();

            employeesControl.Dock = DockStyle.Fill;
            employeesControl.Visible = false;

            pnlMain.Controls.Add(employeesControl);

            // Prepare Attendance screen.
            attendanceControl = new AttendanceControl();

            attendanceControl.Dock = DockStyle.Fill;
            attendanceControl.Visible = false;

            pnlMain.Controls.Add(attendanceControl);

            // PAYROLL ADDITION
            // Prepare Payroll screen.
            payrollControl = new PayrollControl();

            payrollControl.Dock = DockStyle.Fill;
            payrollControl.Visible = false;

            pnlMain.Controls.Add(payrollControl);

            // REPORTS ADDITION
            // Prepare Reports screen.
            reportsControl = new ReportsControl();

            reportsControl.Dock = DockStyle.Fill;
            reportsControl.Visible = false;

            pnlMain.Controls.Add(reportsControl);

            // SETTINGS ADDITION
            // Prepare Settings screen.
            settingsControl = new SettingsControl();
            settingsControl.Dock = DockStyle.Fill;
            settingsControl.Visible = false;
            pnlMain.Controls.Add(settingsControl);

            // Load Dashboard statistics.
            RefreshDashboardStatistics();
        }

        // ============================================================
        // LIVE DATA REFRESH
        // ============================================================

        private void DataRefreshManager_DataChanged(
            object? sender,
            DataChangedEventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        RefreshDashboardStatistics();
                    }
                }));

                return;
            }

            RefreshDashboardStatistics();
        }

        private void DashboardForm_FormClosed(
            object? sender,
            FormClosedEventArgs e)
        {
            DataRefreshManager.DataChanged -=
                DataRefreshManager_DataChanged;

            FormClosed -=
                DashboardForm_FormClosed;
        }

        // ============================================================
        // DASHBOARD
        // ============================================================

        private void btnDashboard_Click(
            object? sender,
            EventArgs e)
        {
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            SetActiveNavigationButton(btnDashboard);

            // Hide Settings when navigating to another module.
            if (settingsControl != null)
            {
                settingsControl.Visible = false;
            }

            // Hide Employees.
            if (employeesControl != null)
            {
                employeesControl.Visible = false;
            }

            // Hide Attendance.
            if (attendanceControl != null)
            {
                attendanceControl.Visible = false;
            }

            // PAYROLL ADDITION
            // Hide Payroll.
            if (payrollControl != null)
            {
                payrollControl.Visible = false;
            }

            // REPORTS ADDITION
            // Hide Reports.
            if (reportsControl != null)
            {
                reportsControl.Visible = false;
            }

            // Show Dashboard.
            lblDashboardTitle.Visible = true;
            lblWelcome.Visible = true;
            tblKpiCards.Visible = true;
            lblRecentPayroll.Visible = true;
            dgvRecentPayroll.Visible = true;

            RefreshDashboardStatistics();
        }

        // ============================================================
        // EMPLOYEES
        // ============================================================

        private void btnEmployees_Click(
            object? sender,
            EventArgs e)
        {
            ShowEmployees();
        }

        private void ShowEmployees()
        {
            SetActiveNavigationButton(btnEmployees);

            // Hide Settings when navigating to another module.
            if (settingsControl != null)
            {
                settingsControl.Visible = false;
            }

            // Hide Dashboard.
            lblDashboardTitle.Visible = false;
            lblWelcome.Visible = false;
            tblKpiCards.Visible = false;
            lblRecentPayroll.Visible = false;
            dgvRecentPayroll.Visible = false;

            // Hide Attendance.
            if (attendanceControl != null)
            {
                attendanceControl.Visible = false;
            }

            // PAYROLL ADDITION
            // Hide Payroll.
            if (payrollControl != null)
            {
                payrollControl.Visible = false;
            }

            // REPORTS ADDITION
            // Hide Reports.
            if (reportsControl != null)
            {
                reportsControl.Visible = false;
            }

            // Show Employees.
            if (employeesControl != null)
            {
                employeesControl.Visible = true;
                employeesControl.BringToFront();
            }
        }

        // ============================================================
        // ATTENDANCE
        // ============================================================

        private void btnAttendance_Click(
            object? sender,
            EventArgs e)
        {
            ShowAttendance();
        }

        private void ShowAttendance()
        {
            SetActiveNavigationButton(btnAttendance);

            // Hide Settings when navigating to another module.
            if (settingsControl != null)
            {
                settingsControl.Visible = false;
            }

            // Hide Dashboard.
            lblDashboardTitle.Visible = false;
            lblWelcome.Visible = false;
            tblKpiCards.Visible = false;
            lblRecentPayroll.Visible = false;
            dgvRecentPayroll.Visible = false;

            // Hide Employees.
            if (employeesControl != null)
            {
                employeesControl.Visible = false;
            }

            // PAYROLL ADDITION
            // Hide Payroll.
            if (payrollControl != null)
            {
                payrollControl.Visible = false;
            }

            // REPORTS ADDITION
            // Hide Reports.
            if (reportsControl != null)
            {
                reportsControl.Visible = false;
            }

            // Show Attendance.
            if (attendanceControl != null)
            {
                attendanceControl.Visible = true;
                attendanceControl.BringToFront();

                // Refresh the employee list immediately after the
                // Attendance screen is opened. This guarantees that
                // employees added moments ago appear without restarting.
                attendanceControl.RefreshEmployeeList();
            }
        }

        // ============================================================
        // PAYROLL
        // ============================================================

        // PAYROLL ADDITION
        private void btnPayroll_Click(
            object? sender,
            EventArgs e)
        {
            ShowPayroll();
        }

        // PAYROLL ADDITION
        private void ShowPayroll()
        {
            SetActiveNavigationButton(btnPayroll);

            // Hide Settings when navigating to another module.
            if (settingsControl != null)
            {
                settingsControl.Visible = false;
            }

            // Hide Dashboard.
            lblDashboardTitle.Visible = false;
            lblWelcome.Visible = false;
            tblKpiCards.Visible = false;
            lblRecentPayroll.Visible = false;
            dgvRecentPayroll.Visible = false;

            // Hide Employees.
            if (employeesControl != null)
            {
                employeesControl.Visible = false;
            }

            // Hide Attendance.
            if (attendanceControl != null)
            {
                attendanceControl.Visible = false;
            }

            // Hide Reports.
            if (reportsControl != null)
            {
                reportsControl.Visible = false;
            }

            // Show Payroll.
            if (payrollControl != null)
            {
                payrollControl.Visible = true;
                payrollControl.BringToFront();

                // Refresh the employee list immediately after the
                // Payroll screen is opened.
                payrollControl.RefreshEmployeeList();
                payrollControl.ShowPayrollEntryMode();
            }
        }

        // ============================================================
        // REPORTS
        // ============================================================

        private void btnReports_Click(
            object? sender,
            EventArgs e)
        {
            ShowReports();
        }

        private void ShowReports()
        {
            SetActiveNavigationButton(btnReports);

            // Hide Settings when navigating to another module.
            if (settingsControl != null)
            {
                settingsControl.Visible = false;
            }

            // Hide Dashboard.
            lblDashboardTitle.Visible = false;
            lblWelcome.Visible = false;
            tblKpiCards.Visible = false;
            lblRecentPayroll.Visible = false;
            dgvRecentPayroll.Visible = false;

            // Hide Employees.
            if (employeesControl != null)
            {
                employeesControl.Visible = false;
            }

            // Hide Attendance.
            if (attendanceControl != null)
            {
                attendanceControl.Visible = false;
            }

            // Hide Payroll.
            if (payrollControl != null)
            {
                payrollControl.Visible = false;
            }

            // Show Reports.
            if (reportsControl != null)
            {
                reportsControl.Visible = true;
                reportsControl.BringToFront();
            }
        }

        // ============================================================
        // SETTINGS
        // ============================================================

        private void btnSettings_Click(
            object? sender,
            EventArgs e)
        {
            ShowSettings();
        }

        private void ShowSettings()
        {
            SetActiveNavigationButton(btnSettings);

            lblDashboardTitle.Visible = false;
            lblWelcome.Visible = false;
            tblKpiCards.Visible = false;
            lblRecentPayroll.Visible = false;
            dgvRecentPayroll.Visible = false;

            if (employeesControl != null)
                employeesControl.Visible = false;

            if (attendanceControl != null)
                attendanceControl.Visible = false;

            if (payrollControl != null)
                payrollControl.Visible = false;

            if (reportsControl != null)
                reportsControl.Visible = false;

            if (settingsControl != null)
            {
                settingsControl.RefreshSettings();
                settingsControl.Visible = true;
                settingsControl.BringToFront();
            }
        }

        // ============================================================
        // DASHBOARD VISUAL DESIGN
        // ============================================================

        private readonly Color dashboardBackground = Color.FromArgb(244, 247, 251);
        private readonly Color sidebarBackground = Color.FromArgb(24, 35, 54);
        private readonly Color sidebarButton = Color.FromArgb(24, 35, 54);
        private readonly Color sidebarHover = Color.FromArgb(42, 58, 84);
        private readonly Color sidebarActive = Color.FromArgb(39, 103, 210);
        private readonly Color headingColor = Color.FromArgb(28, 39, 56);
        private readonly Color mutedTextColor = Color.FromArgb(101, 115, 135);
        private readonly Color cardBorderColor = Color.FromArgb(225, 231, 240);
        private readonly Color tableHeaderColor = Color.FromArgb(232, 238, 247);
        private readonly Color tableAlternateColor = Color.FromArgb(247, 249, 252);
        private readonly Color tableSelectionColor = Color.FromArgb(213, 229, 251);

        private void ConfigureDashboardAppearance()
        {
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            BackColor = dashboardBackground;

            // --------------------------------------------------------
            // SIDEBAR
            // --------------------------------------------------------
            pnlSidebar.BackColor = sidebarBackground;
            pnlSidebar.Width = 220;
            pnlSidebar.Dock = DockStyle.Left;

            lblCompany.AutoSize = false;
            lblCompany.Location = new Point(12, 20);
            lblCompany.Size = new Size(pnlSidebar.ClientSize.Width - 24, 88);
            lblCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCompany.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCompany.ForeColor = Color.White;
            lblCompany.BackColor = sidebarBackground;
            lblCompany.TextAlign = ContentAlignment.MiddleCenter;
            lblCompany.Text = "COMPUTER\nELECTRONICS\nSHOP";

            ConfigureNavigationButton(btnDashboard);
            ConfigureNavigationButton(btnEmployees);
            ConfigureNavigationButton(btnAttendance);
            ConfigureNavigationButton(btnPayroll);
            ConfigureNavigationButton(btnReports);
            ConfigureNavigationButton(btnSettings);
            ConfigureNavigationButton(btnLogout);

            // Restore compact navigation icons while retaining the existing buttons/events.
            btnDashboard.Text = "▦   DASHBOARD";
            btnEmployees.Text = "♙   EMPLOYEES";
            btnAttendance.Text = "◷   ATTENDANCE";
            btnPayroll.Text = "₱   PAYROLL";
            btnReports.Text = "▥   REPORTS";
            btnSettings.Text = "⚙   SETTINGS";
            btnLogout.Text = "↪   LOGOUT";

            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(130, 43, 55);
            btnLogout.FlatAppearance.MouseDownBackColor = Color.FromArgb(156, 48, 62);
            btnLogout.ForeColor = Color.FromArgb(255, 220, 224);

            LayoutSidebarNavigation();
            SetActiveNavigationButton(btnDashboard);

            // --------------------------------------------------------
            // MAIN WORK AREA AND HEADING
            // --------------------------------------------------------
            pnlMain.BackColor = dashboardBackground;

            lblDashboardTitle.AutoSize = false;
            lblDashboardTitle.Location = new Point(28, 20);
            lblDashboardTitle.Size = new Size(Math.Max(300, pnlMain.ClientSize.Width - 56), 42);
            lblDashboardTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblDashboardTitle.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
            lblDashboardTitle.ForeColor = headingColor;
            lblDashboardTitle.Text = "DASHBOARD";
            lblDashboardTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblWelcome.AutoSize = false;
            lblWelcome.Location = new Point(30, 62);
            lblWelcome.Size = new Size(Math.Max(300, pnlMain.ClientSize.Width - 60), 25);
            lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblWelcome.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblWelcome.ForeColor = mutedTextColor;
            lblWelcome.Text = "Welcome to the Payroll Management System";
            lblWelcome.TextAlign = ContentAlignment.MiddleLeft;

            // --------------------------------------------------------
            // KPI CARDS
            // --------------------------------------------------------
            StyleKpiCard(pnlEmployeesCard, lblEmployeesTitle, lblEmployeeCount,
                Color.FromArgb(39, 103, 210), false);
            StyleKpiCard(pnlActiveCard, lblActiveTitle, lblActiveCount,
                Color.FromArgb(27, 143, 104), false);
            StyleKpiCard(pnlPayrollCard, lblPayrollTitle, lblPayrollAmount,
                Color.FromArgb(39, 103, 210), true);
            StyleKpiCard(pnlPendingCard, lblPendingTitle, lblPendingCount,
                Color.FromArgb(205, 119, 35), false);

            if (tblKpiCards != null)
            {
                tblKpiCards.BackColor = dashboardBackground;
                tblKpiCards.Location = new Point(20, 101);
                tblKpiCards.Size = new Size(Math.Max(420, pnlMain.ClientSize.Width - 40), 148);
                tblKpiCards.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                if (tblKpiCards is TableLayoutPanel kpiTable)
                {
                    kpiTable.ColumnCount = 4;
                    kpiTable.RowCount = 1;
                    kpiTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
                    kpiTable.ColumnStyles.Clear();
                    for (int i = 0; i < 4; i++)
                    {
                        kpiTable.ColumnStyles.Add(
                            new ColumnStyle(SizeType.Percent, 25F));
                    }

                    kpiTable.RowStyles.Clear();
                    kpiTable.RowStyles.Add(
                        new RowStyle(SizeType.Percent, 100F));

                    Control[] cards =
                    {
                        pnlEmployeesCard,
                        pnlActiveCard,
                        pnlPayrollCard,
                        pnlPendingCard
                    };

                    for (int i = 0; i < cards.Length; i++)
                    {
                        if (cards[i].Parent == kpiTable)
                        {
                            cards[i].Dock = DockStyle.Fill;
                            cards[i].Margin = new Padding(7, 5, 7, 5);
                            kpiTable.SetColumn(cards[i], i);
                            kpiTable.SetRow(cards[i], 0);
                        }
                    }
                }
            }

            // Older Designer layouts keep the four cards directly under pnlMain.
            // Reposition those cards responsively while leaving TableLayoutPanel
            // hosted cards to the table's layout engine.
            LayoutStandaloneKpiCards();

            // --------------------------------------------------------
            // RECENT PAYROLL HEADING
            // --------------------------------------------------------
            lblRecentPayroll.AutoSize = false;
            lblRecentPayroll.Location = new Point(26, 267);
            lblRecentPayroll.Size = new Size(Math.Max(300, pnlMain.ClientSize.Width - 52), 28);
            lblRecentPayroll.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblRecentPayroll.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblRecentPayroll.ForeColor = headingColor;
            lblRecentPayroll.Text = "RECENT PAYROLL";
            lblRecentPayroll.TextAlign = ContentAlignment.MiddleLeft;

            // --------------------------------------------------------
            // RECENT PAYROLL TABLE
            // --------------------------------------------------------
            dgvRecentPayroll.Location = new Point(24, 300);
            dgvRecentPayroll.Size = new Size(
                Math.Max(300, pnlMain.ClientSize.Width - 48),
                Math.Max(180, pnlMain.ClientSize.Height - 318));
            dgvRecentPayroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                                      AnchorStyles.Left | AnchorStyles.Right;
            dgvRecentPayroll.BackgroundColor = Color.White;
            dgvRecentPayroll.BorderStyle = BorderStyle.None;
            dgvRecentPayroll.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecentPayroll.GridColor = cardBorderColor;
            dgvRecentPayroll.EnableHeadersVisualStyles = false;
            dgvRecentPayroll.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvRecentPayroll.ColumnHeadersHeight = 40;
            dgvRecentPayroll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRecentPayroll.ColumnHeadersDefaultCellStyle.BackColor = tableHeaderColor;
            dgvRecentPayroll.ColumnHeadersDefaultCellStyle.ForeColor = headingColor;
            dgvRecentPayroll.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvRecentPayroll.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecentPayroll.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            dgvRecentPayroll.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvRecentPayroll.DefaultCellStyle.ForeColor = Color.FromArgb(44, 55, 72);
            dgvRecentPayroll.DefaultCellStyle.BackColor = Color.White;
            dgvRecentPayroll.DefaultCellStyle.SelectionBackColor = tableSelectionColor;
            dgvRecentPayroll.DefaultCellStyle.SelectionForeColor = Color.FromArgb(27, 49, 79);
            dgvRecentPayroll.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecentPayroll.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            dgvRecentPayroll.AlternatingRowsDefaultCellStyle.BackColor = tableAlternateColor;
            dgvRecentPayroll.RowTemplate.Height = 34;
            dgvRecentPayroll.RowHeadersVisible = false;
            dgvRecentPayroll.AllowUserToAddRows = false;
            dgvRecentPayroll.AllowUserToDeleteRows = false;
            dgvRecentPayroll.AllowUserToResizeRows = false;
            dgvRecentPayroll.MultiSelect = false;
            dgvRecentPayroll.ReadOnly = true;
            dgvRecentPayroll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentPayroll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Hidden stable key used by the Date Modified menu. The dashboard
            // displays only a small subset of payroll fields, so its source
            // PayrollID must travel with each displayed row for sorting.
            if (!dgvRecentPayroll.Columns.Contains("__CreationOrderKey"))
            {
                DataGridViewTextBoxColumn creationOrderColumn = new DataGridViewTextBoxColumn
                {
                    Name = "__CreationOrderKey",
                    HeaderText = "",
                    Visible = false,
                    SortMode = DataGridViewColumnSortMode.Programmatic,
                    ReadOnly = true
                };
                dgvRecentPayroll.Columns.Add(creationOrderColumn);
            }

            SetRecentPayrollColumnWeight("colEmployee", 25F);
            SetRecentPayrollColumnWeight("colPayPeriod", 17F);
            SetRecentPayrollColumnWeight("colGrossPay", 15F);
            SetRecentPayrollColumnWeight("colDeductions", 15F);
            SetRecentPayrollColumnWeight("colNetPay", 16F);
            SetRecentPayrollColumnWeight("colStatus", 12F);

            LayoutDashboardContent();

            // Center every table cell and enable safe header-click sorting.
            DataGridViewSortingHelper.Configure(dgvRecentPayroll);
            FuturisticUiTheme.ApplyGrids(this);
        }

        private void ConfigureNavigationButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = sidebarHover;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(51, 76, 112);
            button.BackColor = sidebarButton;
            button.ForeColor = Color.FromArgb(220, 228, 239);
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(13, 0, 6, 0);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            button.Height = 37;
        }

        private void LayoutSidebarNavigation()
        {
            if (pnlSidebar == null || pnlSidebar.IsDisposed)
                return;

            int buttonLeft = 12;
            int buttonWidth = Math.Max(150, pnlSidebar.ClientSize.Width - 24);
            int top = 136;
            int gap = 7;

            Button[] navigationButtons =
            {
                btnDashboard,
                btnEmployees,
                btnAttendance,
                btnPayroll,
                btnReports,
                btnSettings
            };

            for (int i = 0; i < navigationButtons.Length; i++)
            {
                navigationButtons[i].Location = new Point(buttonLeft, top + i * (37 + gap));
                navigationButtons[i].Size = new Size(buttonWidth, 37);
                navigationButtons[i].Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }

            btnLogout.Location = new Point(buttonLeft, Math.Max(top + navigationButtons.Length * (37 + gap) + 20,
                pnlSidebar.ClientSize.Height - 54));
            btnLogout.Size = new Size(buttonWidth, 37);
            btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void SetActiveNavigationButton(Button activeButton)
        {
            Button[] navigationButtons =
            {
                btnDashboard,
                btnEmployees,
                btnAttendance,
                btnPayroll,
                btnReports,
                btnSettings
            };

            foreach (Button button in navigationButtons)
            {
                bool isActive = button == activeButton;
                button.BackColor = isActive ? sidebarActive : sidebarButton;
                button.ForeColor = isActive ? Color.White : Color.FromArgb(220, 228, 239);
                button.FlatAppearance.BorderSize = 0;
            }
        }

        private void StyleKpiCard(
            Panel card,
            Label title,
            Label value,
            Color valueColor,
            bool currencyValue)
        {
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.None;
            card.Padding = new Padding(8, 7, 8, 7);
            card.Paint -= KpiCard_Paint;
            card.Paint += KpiCard_Paint;
            card.Tag = valueColor;

            title.AutoSize = false;
            title.Dock = DockStyle.Top;
            title.Height = 36;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            title.ForeColor = mutedTextColor;
            title.BackColor = Color.White;

            value.AutoSize = false;
            value.Dock = DockStyle.Fill;
            value.TextAlign = ContentAlignment.MiddleCenter;
            value.Font = new Font("Segoe UI", currencyValue ? 20F : 25F, FontStyle.Bold);
            value.ForeColor = valueColor;
            value.BackColor = Color.White;
        }

        private void KpiCard_Paint(object? sender, PaintEventArgs e)
        {
            Panel? card = sender as Panel;
            if (card == null || card.ClientSize.Width < 8 || card.ClientSize.Height < 8)
                return;

            Color accent = card.Tag is Color ? (Color)card.Tag : Color.FromArgb(39, 103, 210);
            Rectangle bounds = card.ClientRectangle;
            bounds.Width -= 1;
            bounds.Height -= 1;

            // Fine accent rail and a soft lower/right edge give the KPI cards
            // depth without thick outlines or extra controls in the Designer.
            using (Pen edge = new Pen(Color.FromArgb(222, 230, 241), 1F))
            using (Pen shadow = new Pen(Color.FromArgb(235, 239, 246), 1F))
            using (SolidBrush rail = new SolidBrush(accent))
            {
                e.Graphics.DrawLine(edge, bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
                e.Graphics.DrawLine(shadow, bounds.Right, bounds.Top + 1, bounds.Right, bounds.Bottom);
                e.Graphics.FillRectangle(rail, bounds.Left + 1, bounds.Top, Math.Max(0, bounds.Width - 2), 3);
            }
        }

        private void LayoutStandaloneKpiCards()
        {
            Panel[] cards =
            {
                pnlEmployeesCard,
                pnlActiveCard,
                pnlPayrollCard,
                pnlPendingCard
            };

            // The Designer may place the cards in a TableLayoutPanel,
            // another container, or directly on pnlMain. Use the parent
            // container's layout engine when it owns the cards; manually
            // arrange cards only for parents that do not manage them.
            bool cardsHostedByKpiTable = true;
            foreach (Panel card in cards)
            {
                if (card.Parent != tblKpiCards)
                {
                    cardsHostedByKpiTable = false;
                    break;
                }
            }

            if (cardsHostedByKpiTable && tblKpiCards is TableLayoutPanel)
                return;

            int left = 24;
            int gap = 14;
            int availableWidth = Math.Max(400, pnlMain.ClientSize.Width - 48);
            int cardWidth = Math.Max(120, (availableWidth - gap * 3) / 4);

            for (int i = 0; i < cards.Length; i++)
            {
                Panel card = cards[i];
                Control? parent = card.Parent;
                if (parent == null || parent is TableLayoutPanel)
                    continue;

                int containerWidth = parent == pnlMain
                    ? pnlMain.ClientSize.Width
                    : parent.ClientSize.Width;
                int containerLeft = parent == pnlMain ? left : 8;
                int containerAvailableWidth = parent == pnlMain
                    ? Math.Max(400, containerWidth - 48)
                    : Math.Max(400, containerWidth - 16);
                int localGap = parent == pnlMain ? gap : 8;
                int localCardWidth = Math.Max(120,
                    (containerAvailableWidth - localGap * 3) / 4);
                int y = parent == pnlMain ? 105 : 5;

                card.Location = new Point(containerLeft + i * (localCardWidth + localGap), y);
                card.Size = new Size(localCardWidth, 138);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                card.Margin = new Padding(7, 5, 7, 5);
            }
        }

        private void LayoutDashboardContent()
        {
            if (IsDisposed || pnlMain == null || pnlMain.IsDisposed)
                return;

            int mainWidth = pnlMain.ClientSize.Width;
            int mainHeight = pnlMain.ClientSize.Height;
            if (mainWidth <= 0 || mainHeight <= 0)
                return;

            lblDashboardTitle.Size = new Size(Math.Max(250, mainWidth - 56), 42);
            lblWelcome.Size = new Size(Math.Max(250, mainWidth - 60), 25);

            if (tblKpiCards != null)
            {
                tblKpiCards.Location = new Point(20, 101);
                tblKpiCards.Size = new Size(Math.Max(400, mainWidth - 40), 148);
            }

            LayoutStandaloneKpiCards();

            lblRecentPayroll.Location = new Point(26, 267);
            lblRecentPayroll.Size = new Size(Math.Max(250, mainWidth - 52), 28);

            dgvRecentPayroll.Location = new Point(24, 300);
            dgvRecentPayroll.Size = new Size(
                Math.Max(250, mainWidth - 48),
                Math.Max(180, mainHeight - 318));
        }

        private void SetRecentPayrollColumnWeight(string columnName, float weight)
        {
            if (!dgvRecentPayroll.Columns.Contains(columnName))
                return;

            DataGridViewColumn? column = dgvRecentPayroll.Columns[columnName];
            if (column == null)
                return;

            column.FillWeight = weight;
            column.MinimumWidth = 70;
            column.SortMode = DataGridViewColumnSortMode.Automatic;

            if (columnName == "colEmployee" || columnName == "colPayPeriod")
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            else if (columnName == "colStatus")
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            else
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        // ============================================================
        // DASHBOARD STATISTICS
        // ============================================================

        private void RefreshDashboardStatistics()
        {
            try
            {
                // ----------------------------------------------------
                // EMPLOYEE COUNTS
                // ----------------------------------------------------

                DataTable employees =
                    DatabaseHelper.GetEmployees();

                int totalEmployees =
                    employees.Rows.Count;

                int activeEmployees = 0;

                foreach (DataRow row in employees.Rows)
                {
                    string status =
                        row["Status"]?.ToString() ?? "";

                    if (string.Equals(
                            status,
                            "Active",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        activeEmployees++;
                    }
                }

                lblEmployeeCount.Text =
                    totalEmployees.ToString();

                lblActiveCount.Text =
                    activeEmployees.ToString();

                // ----------------------------------------------------
                // PAYROLL DATA
                // ----------------------------------------------------

                DataTable payroll =
                    PayrollDatabaseHelper.GetPayrollRecords();

                string currentMonth =
                    DateTime.Today.ToString("yyyy-MM");

                decimal currentMonthNetPay = 0m;

                // Count employees who already have a payroll record
                // for the current month.
                System.Collections.Generic.HashSet<long> paidEmployeeIDs =
                    new System.Collections.Generic.HashSet<long>();

                foreach (DataRow row in payroll.Rows)
                {
                    string payrollMonth =
                        row["PayrollMonth"]?.ToString() ?? "";

                    if (!string.Equals(
                            payrollMonth,
                            currentMonth,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    decimal netPay = 0m;

                    if (row["NetPay"] != DBNull.Value)
                    {
                        netPay = Convert.ToDecimal(
                            row["NetPay"],
                            CultureInfo.InvariantCulture);
                    }

                    currentMonthNetPay += netPay;

                    if (row["EmployeeID"] != DBNull.Value)
                    {
                        paidEmployeeIDs.Add(
                            Convert.ToInt64(row["EmployeeID"]));
                    }
                }

                // Pending payroll = active employees without any
                // payroll record for the current month.
                int pendingPayroll =
                    activeEmployees - paidEmployeeIDs.Count;

                if (pendingPayroll < 0)
                {
                    pendingPayroll = 0;
                }

                lblPayrollAmount.Text =
                    currentMonthNetPay.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                lblPendingCount.Text =
                    pendingPayroll.ToString();

                // ----------------------------------------------------
                // RECENT PAYROLL
                // ----------------------------------------------------

                dgvRecentPayroll.Rows.Clear();

                int rowsToShow =
                    Math.Min(5, payroll.Rows.Count);

                for (int i = 0; i < rowsToShow; i++)
                {
                    DataRow row = payroll.Rows[i];

                    string employeeName =
                        row["EmployeeName"]?.ToString() ?? "";

                    string payPeriod =
                        row["PayPeriod"]?.ToString() ?? "";

                    decimal grossPay = 0m;
                    decimal deductions = 0m;
                    decimal netPay = 0m;

                    if (row["GrossPay"] != DBNull.Value)
                    {
                        grossPay = Convert.ToDecimal(
                            row["GrossPay"],
                            CultureInfo.InvariantCulture);
                    }

                    if (row["Deductions"] != DBNull.Value)
                    {
                        deductions = Convert.ToDecimal(
                            row["Deductions"],
                            CultureInfo.InvariantCulture);
                    }

                    if (row["NetPay"] != DBNull.Value)
                    {
                        netPay = Convert.ToDecimal(
                            row["NetPay"],
                            CultureInfo.InvariantCulture);
                    }

                    dgvRecentPayroll.Rows.Add(
                        employeeName,
                        payPeriod,
                        grossPay.ToString(
                            "₱#,##0.00",
                            CultureInfo.CurrentCulture),
                        deductions.ToString(
                            "₱#,##0.00",
                            CultureInfo.CurrentCulture),
                        netPay.ToString(
                            "₱#,##0.00",
                            CultureInfo.CurrentCulture),
                        "Processed",
                        payroll.Columns.Contains("PayrollID") && row["PayrollID"] != DBNull.Value
                            ? Convert.ToInt64(row["PayrollID"], CultureInfo.InvariantCulture)
                            : i);
                }

                // Keep the selected column sort when live data is refreshed.
                DataGridViewSortingHelper.ApplyCurrentSort(dgvRecentPayroll);
            }
            catch (Exception ex)
            {
                lblPayrollAmount.Text = "₱0.00";
                lblPendingCount.Text = "0";
                dgvRecentPayroll.Rows.Clear();

                MessageBox.Show(
                    "Unable to load Dashboard statistics.\n\n" +
                    ex.Message,
                    "Dashboard Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LOGOUT
        // ============================================================

        private void btnLogout_Click(
            object sender,
            EventArgs e)
        {
            LoginForm login =
                new LoginForm();

            login.Show();

            Close();
        }

        // ============================================================
        // EXISTING DESIGNER EVENTS
        // ============================================================

        private void dgvRecentPayroll_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            // No action required yet.
        }

        private void lblPayrollAmount_Click(
            object sender,
            EventArgs e)
        {
            // Kept for compatibility with the existing Designer event.
            // Clicking the monthly payroll amount does not change the view.
        }

        private void pnlMain_Paint(
            object sender,
            PaintEventArgs e)
        {
            // No action required.
        }
    }
}
