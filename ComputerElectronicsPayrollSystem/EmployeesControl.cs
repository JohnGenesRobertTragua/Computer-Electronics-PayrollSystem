using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class EmployeesControl : UserControl
    {
        private DataTable employeeTable = new DataTable();
        private Button? btnEmployeeProfile;
        private Label? lblEmployeeCountBadge;

        public EmployeesControl()
        {
            InitializeComponent();

            ConfigureEmployeeGrid();
            ConfigureEmployeeProfileButton();
            ConfigureEmployeeLayoutAppearance();

            // Use centered cell contents and sortable column headers.
            DataGridViewSortingHelper.Configure(dgvEmployees);
            FuturisticUiTheme.Apply(this);

            Resize += EmployeesControl_Resize;

            btnSearch.Click += btnSearch_Click;
            btnAddEmployee.Click += btnAddEmployee_Click;
            btnUpdateEmployee.Click += btnUpdateEmployee_Click;
            btnDeleteEmployee.Click += btnDeleteEmployee_Click;

            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;

            // Listen for live changes made by other modules.
            DataRefreshManager.DataChanged += DataRefreshManager_DataChanged;
            Disposed += EmployeesControl_Disposed;

            LoadEmployees();
        }

        // ============================================================
        // GRID CONFIGURATION
        // ============================================================

        private void ConfigureEmployeeGrid()
        {
            Color navy = Color.FromArgb(24, 39, 62);
            Color accentBlue = Color.FromArgb(45, 108, 223);
            Color gridLine = Color.FromArgb(222, 228, 236);

            dgvEmployees.AutoGenerateColumns = false;
            dgvEmployees.ReadOnly = true;
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AllowUserToResizeRows = false;
            dgvEmployees.AllowUserToOrderColumns = false;
            dgvEmployees.MultiSelect = false;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvEmployees.BorderStyle = BorderStyle.FixedSingle;
            dgvEmployees.BackgroundColor = Color.White;
            dgvEmployees.GridColor = gridLine;
            dgvEmployees.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEmployees.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvEmployees.EnableHeadersVisualStyles = false;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEmployees.ColumnHeadersHeight = 40;
            dgvEmployees.RowTemplate.Height = 34;
            dgvEmployees.ScrollBars = ScrollBars.Both;

            dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor = navy;
            dgvEmployees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmployees.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvEmployees.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEmployees.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            dgvEmployees.DefaultCellStyle.BackColor = Color.White;
            dgvEmployees.DefaultCellStyle.ForeColor = Color.FromArgb(36, 47, 62);
            dgvEmployees.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvEmployees.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEmployees.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgvEmployees.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvEmployees.DefaultCellStyle.SelectionBackColor = accentBlue;
            dgvEmployees.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvEmployees.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 248, 252);
            dgvEmployees.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(36, 47, 62);
            dgvEmployees.AlternatingRowsDefaultCellStyle.SelectionBackColor = accentBlue;
            dgvEmployees.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Bind the existing Designer columns to database fields.
            colEmployeeID.DataPropertyName = "EmployeeID";
            colFirstName.DataPropertyName = "FirstName";
            colLastName.DataPropertyName = "LastName";
            colPosition.DataPropertyName = "Position";
            colDepartment.DataPropertyName = "Department";
            colSalary.DataPropertyName = "Salary";
            colStatus.DataPropertyName = "Status";

            // Give each column space in proportion to its normal content.
            colEmployeeID.FillWeight = 12F;
            colFirstName.FillWeight = 20F;
            colLastName.FillWeight = 16F;
            colPosition.FillWeight = 15F;
            colDepartment.FillWeight = 13F;
            colSalary.FillWeight = 15F;
            colStatus.FillWeight = 9F;

            colSalary.DefaultCellStyle.Format = "₱#,##0.00";
            colSalary.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        // ============================================================
        // EMPLOYEE FORM APPEARANCE AND RESPONSIVE LAYOUT
        // ============================================================

        private void ConfigureEmployeeLayoutAppearance()
        {
            Color pageBackground = Color.FromArgb(246, 248, 252);
            Color navy = Color.FromArgb(24, 39, 62);
            Color accentBlue = Color.FromArgb(45, 108, 223);
            Color secondaryText = Color.FromArgb(100, 112, 128);
            Color borderColor = Color.FromArgb(218, 225, 234);

            BackColor = pageBackground;

            lblEmployeesTitle.ForeColor = navy;
            lblEmployeesTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblEmployeesSubtitle.ForeColor = secondaryText;
            lblEmployeesSubtitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            pnlEmployeeSearch.BackColor = Color.White;
            pnlEmployeeSearch.BorderStyle = BorderStyle.FixedSingle;
            pnlEmployeeActions.BackColor = pageBackground;
            pnlEmployeeActions.BorderStyle = BorderStyle.None;

            lblSearch.ForeColor = Color.FromArgb(48, 59, 74);
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            txtSearch.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            txtSearch.BackColor = Color.White;
            txtSearch.ForeColor = Color.FromArgb(31, 41, 55);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            StyleEmployeeButton(
                btnSearch,
                accentBlue,
                Color.White,
                accentBlue,
                Color.FromArgb(36, 90, 190),
                true);
            btnSearch.Text = "\uE721";
            btnSearch.Font = new Font("Segoe MDL2 Assets", 14F, FontStyle.Regular);
            btnSearch.BackColor = Color.FromArgb(220, 225, 232);
            btnSearch.ForeColor = Color.FromArgb(45, 55, 72);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(205, 213, 224);
            btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(190, 201, 216);
            btnSearch.AccessibleName = "Search employees";
            btnSearch.AccessibleDescription = "Search employees using the entered text";
            btnSearch.TextAlign = ContentAlignment.MiddleCenter;

            StyleEmployeeButton(
                btnAddEmployee,
                accentBlue,
                Color.White,
                accentBlue,
                Color.FromArgb(36, 90, 190),
                true);

            StyleEmployeeButton(
                btnUpdateEmployee,
                Color.FromArgb(232, 240, 254),
                Color.FromArgb(36, 84, 166),
                Color.FromArgb(194, 215, 250),
                Color.FromArgb(213, 228, 252),
                false);

            StyleEmployeeButton(
                btnDeleteEmployee,
                Color.FromArgb(254, 242, 242),
                Color.FromArgb(180, 35, 24),
                Color.FromArgb(252, 205, 205),
                Color.FromArgb(254, 226, 226),
                false);

            StyleEmployeeButton(
                btnClearEmployee,
                Color.FromArgb(239, 244, 248),
                Color.FromArgb(71, 85, 105),
                Color.FromArgb(211, 220, 230),
                Color.FromArgb(224, 232, 240),
                false);

            lblEmployeeCountBadge = new Label
            {
                Name = "lblEmployeeCountBadge",
                AutoSize = false,
                Size = new Size(145, 28),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(232, 240, 254),
                ForeColor = Color.FromArgb(36, 84, 166),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            Controls.Add(lblEmployeeCountBadge);
            lblEmployeeCountBadge.BringToFront();

            UpdateEmployeesResponsiveLayout();
            UpdateEmployeeCountBadge();
        }

        private static void StyleEmployeeButton(
            Button button,
            Color background,
            Color foreground,
            Color border,
            Color hover,
            bool primary)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.BorderColor = border;
            button.FlatAppearance.MouseOverBackColor = hover;
            button.FlatAppearance.MouseDownBackColor = background;
            button.BackColor = background;
            button.ForeColor = foreground;
            button.UseVisualStyleBackColor = false;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }

        private void UpdateEmployeesResponsiveLayout()
        {
            if (pnlEmployeeSearch == null || pnlEmployeeActions == null || dgvEmployees == null)
                return;

            const int margin = 24;
            int contentWidth = Math.Max(280, ClientSize.Width - margin * 2);
            int contentHeight = Math.Max(140, ClientSize.Height);
            bool compact = contentWidth < 820;

            lblEmployeesTitle.SetBounds(margin, 18, Math.Max(260, contentWidth - 190), 34);
            lblEmployeesSubtitle.SetBounds(margin + 2, 55, Math.Max(240, contentWidth - 4), 20);

            if (lblEmployeeCountBadge != null)
                lblEmployeeCountBadge.SetBounds(
                    Math.Max(margin, ClientSize.Width - margin - lblEmployeeCountBadge.Width),
                    23, lblEmployeeCountBadge.Width, lblEmployeeCountBadge.Height);

            // Search and the four maintenance actions share one bordered panel.
            // The search field occupies the left; actions form a 2 x 2 block on the right.
            int searchPanelHeight = compact ? 168 : 124;
            pnlEmployeeSearch.SetBounds(margin, 84, contentWidth, searchPanelHeight);
            pnlEmployeeSearch.BackColor = Color.White;

            int inner = 16;
            int panelWidth = pnlEmployeeSearch.ClientSize.Width;
            int rightWidth = compact ? panelWidth - inner * 2 : Math.Max(300, (int)(panelWidth * 0.39));
            int leftWidth = compact ? panelWidth - inner * 2 : panelWidth - rightWidth - inner * 3;
            int searchButtonWidth = 42; // compact square button sized for the search icon only
            int gap = 10;

            lblSearch.AutoSize = false;
            lblSearch.TextAlign = ContentAlignment.MiddleLeft;
            // Vertically center the label + input group inside the search panel.
            int searchGroupTop = Math.Max(12, (pnlEmployeeSearch.ClientSize.Height - 58) / 2);
            lblSearch.SetBounds(inner, searchGroupTop, Math.Max(120, leftWidth), 20);
            txtSearch.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            txtSearch.SetBounds(inner, searchGroupTop + 24, Math.Max(100, leftWidth - searchButtonWidth - gap), 32);
            // WinForms may constrain a single-line TextBox to its preferred height.
            // Match the icon button to the final rendered textbox height.
            btnSearch.SetBounds(txtSearch.Right + gap, txtSearch.Top, searchButtonWidth, txtSearch.Height);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnSearch.Text = "\uE721";
            btnSearch.Font = new Font("Segoe MDL2 Assets", 14F, FontStyle.Regular);
            btnSearch.BackColor = Color.FromArgb(220, 225, 232);
            btnSearch.ForeColor = Color.FromArgb(45, 55, 72);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(205, 213, 224);
            btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(190, 201, 216);

            // Move the action buttons into the search panel, in a clean 2-column grid.
            Button[] maintenanceButtons = { btnAddEmployee, btnUpdateEmployee, btnDeleteEmployee, btnClearEmployee };
            int actionLeft = compact ? inner : leftWidth + inner * 2;
            int actionWidth = compact ? panelWidth - inner * 2 : panelWidth - actionLeft - inner;
            int actionGapX = 10;
            int actionGapY = 8;
            int buttonHeight = compact ? 32 : 36;
            int actionBlockHeight = buttonHeight * 2 + actionGapY;
            int actionTop = compact ? 82 : Math.Max(8, (pnlEmployeeSearch.ClientSize.Height - actionBlockHeight) / 2);
            int buttonWidth = Math.Max(80, (actionWidth - actionGapX) / 2);

            for (int i = 0; i < maintenanceButtons.Length; i++)
            {
                Button action = maintenanceButtons[i];
                if (action.Parent != pnlEmployeeSearch)
                {
                    action.Parent?.Controls.Remove(action);
                    pnlEmployeeSearch.Controls.Add(action);
                }
                int col = i % 2;
                int row = i / 2;
                action.SetBounds(
                    actionLeft + col * (buttonWidth + actionGapX),
                    actionTop + row * (buttonHeight + actionGapY),
                    buttonWidth,
                    buttonHeight);
                action.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                action.Font = new Font("Segoe UI", compact ? 8F : 8.5F, FontStyle.Bold);
            }

            // The profile action sits on the left of the toolbar row; the shared
            // sorting helper positions its SORT BY button on the right of this same row.
            pnlEmployeeActions.SetBounds(margin, 84 + searchPanelHeight + 22, contentWidth, 30);
            pnlEmployeeActions.BackColor = BackColor;
            if (btnEmployeeProfile != null)
            {
                if (btnEmployeeProfile.Parent != pnlEmployeeActions)
                {
                    btnEmployeeProfile.Parent?.Controls.Remove(btnEmployeeProfile);
                    pnlEmployeeActions.Controls.Add(btnEmployeeProfile);
                }
                btnEmployeeProfile.SetBounds(0, 0, compact ? 132 : 150, 28);
                btnEmployeeProfile.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            int gridTop = pnlEmployeeActions.Bottom + 2;
            const int bottomMargin = 24;
            dgvEmployees.SetBounds(margin, gridTop, contentWidth,
                Math.Max(120, contentHeight - gridTop - bottomMargin));
            dgvEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void UpdateEmployeeCountBadge()
        {
            if (lblEmployeeCountBadge == null || lblEmployeeCountBadge.IsDisposed)
            {
                return;
            }

            int count = employeeTable?.Rows.Count ?? 0;
            lblEmployeeCountBadge.Text = count == 1
                ? "1 EMPLOYEE"
                : $"{count} EMPLOYEES";
        }

        // ============================================================
        // LOAD EMPLOYEES
        // ============================================================

        private void LoadEmployees(
            string searchText = "",
            bool showNoResultsMessage = false)
        {
            try
            {
                employeeTable =
                    DatabaseHelper.GetEmployees(searchText);

                dgvEmployees.DataSource = null;
                dgvEmployees.DataSource = employeeTable;
                UpdateEmployeeCountBadge();

                if (showNoResultsMessage &&
                    employeeTable.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No employees were found.",
                        "Search",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load employees.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // EMPLOYEE PROFILE SIDE DRAWER
        // ============================================================

        private Panel? employeeProfileDrawer;
        private Panel? employeeProfileContent;
        private Button? btnCloseEmployeeProfile;
        private Label? lblProfileEmployeeName;
        private Label? lblProfileEmployeeID;
        private long profileEmployeeID = 0;
        private string profileEmployeeName = "";

        private Button? btnProfileAttendance;
        private Button? btnProfilePayroll;

        private void ConfigureEmployeeProfileButton()
        {
            btnEmployeeProfile = new Button
            {
                Name = "btnEmployeeProfile",
                Text = "EMPLOYEE PROFILE",
                Size = new Size(150, 28),
                Location = new Point(0, 0),
                TabIndex = 4,
                UseVisualStyleBackColor = false
            };

            StyleEmployeeButton(
                btnEmployeeProfile,
                Color.FromArgb(24, 39, 62),
                Color.White,
                Color.FromArgb(24, 39, 62),
                Color.FromArgb(38, 56, 84),
                false);
            btnEmployeeProfile.FlatAppearance.BorderSize = 1;
            btnEmployeeProfile.FlatAppearance.BorderColor = Color.FromArgb(111, 130, 154);

            btnEmployeeProfile.Click += btnEmployeeProfile_Click;
            pnlEmployeeActions.Controls.Add(btnEmployeeProfile);
        }

                private void btnEmployeeProfile_Click(
            object? sender,
            EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null ||
                dgvEmployees.CurrentRow.DataBoundItem is not DataRowView rowView)
            {
                MessageBox.Show(
                    "Please select an employee first.",
                    "Employee Profile",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                profileEmployeeID =
                    Convert.ToInt64(rowView["EmployeeID"]);

                profileEmployeeName =
                    $"{rowView["FirstName"]} {rowView["LastName"]}".Trim();

                CreateEmployeeProfileDrawer();

                ShowEmployeeInformation();

                AnimateEmployeeProfileDrawer(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to open the Employee Profile.\n\n" +
                    ex.Message,
                    "Employee Profile",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CreateEmployeeProfileDrawer()
        {
            if (employeeProfileDrawer != null &&
                !employeeProfileDrawer.IsDisposed)
            {
                UpdateEmployeeProfileDrawerBounds();
                employeeProfileDrawer.Visible = true;
                return;
            }

            employeeProfileDrawer = new Panel();

            employeeProfileDrawer.Name =
                "employeeProfileDrawer";

            employeeProfileDrawer.BackColor =
                Color.White;

            employeeProfileDrawer.BorderStyle =
                BorderStyle.FixedSingle;

            employeeProfileDrawer.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            employeeProfileDrawer.Visible = false;

            UpdateEmployeeProfileDrawerBounds();

            // --------------------------------------------------------
            // HEADER
            // --------------------------------------------------------

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = Color.WhiteSmoke
            };

            Label title = new Label
            {
                Text = "EMPLOYEE PROFILE",
                Font = new Font(
                    "Segoe UI",
                    12F,
                    FontStyle.Bold),
                AutoSize = true,
                Location = new Point(12, 9)
            };

            lblProfileEmployeeName = new Label
            {
                Text = profileEmployeeName,
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold),
                AutoEllipsis = true,
                Location = new Point(12, 35),
                Size = new Size(170, 20)
            };

            lblProfileEmployeeID = new Label
            {
                Text = "Employee ID: " +
                       profileEmployeeID,
                Font = new Font(
                    "Segoe UI",
                    8F),
                AutoSize = true,
                Location = new Point(12, 56)
            };

            btnCloseEmployeeProfile = new Button
            {
                Name = "btnCloseEmployeeProfile",
                Text = "X",
                Font = new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold),
                Size = new Size(30, 30),
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                UseVisualStyleBackColor = false,
                TabStop = true,
                Visible = true,
                Enabled = true,
                Cursor = Cursors.Hand
            };

            btnCloseEmployeeProfile.FlatAppearance.BorderSize = 0;
            btnCloseEmployeeProfile.Click +=
                EmployeeProfileCloseButton_Click;

            header.Controls.Add(title);
            header.Controls.Add(lblProfileEmployeeName);
            header.Controls.Add(lblProfileEmployeeID);

            // --------------------------------------------------------
            // NAVIGATION
            // --------------------------------------------------------

            Panel navigation = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.White
            };

            btnProfileAttendance = CreateProfileNavigationButton(
                "ATTENDANCE");

            btnProfilePayroll = CreateProfileNavigationButton(
                "PAYROLL");

            btnProfileAttendance.Location =
                new Point(6, 7);

            btnProfilePayroll.Location =
                new Point(108, 7);

            btnProfileAttendance.Click +=
                btnProfileAttendance_Click;

            btnProfilePayroll.Click +=
                btnProfilePayroll_Click;

            navigation.Controls.Add(
                btnProfileAttendance);

            navigation.Controls.Add(
                btnProfilePayroll);

            // --------------------------------------------------------
            // CONTENT
            // --------------------------------------------------------

            employeeProfileContent = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            employeeProfileDrawer.Controls.Add(
                employeeProfileContent);

            employeeProfileDrawer.Controls.Add(
                navigation);

            employeeProfileDrawer.Controls.Add(
                header);

            // The close button is a direct child of the drawer,
            // positioned over the header so it remains visible above
            // the docked panels regardless of WinForms z-order.
            employeeProfileDrawer.Controls.Add(
                btnCloseEmployeeProfile);

            Controls.Add(
                employeeProfileDrawer);

            employeeProfileDrawer.BringToFront();
            PositionEmployeeProfileCloseButton();
        }

        private void btnProfileAttendance_Click(
            object? sender,
            EventArgs e)
        {
            if (profileEmployeeID <= 0)
                return;

            using EmployeeDetailViewForm detailForm =
                new EmployeeDetailViewForm(
                    profileEmployeeID,
                    profileEmployeeName,
                    EmployeeDetailView.Attendance);

            detailForm.ShowDialog(this);
        }

        private void btnProfilePayroll_Click(
            object? sender,
            EventArgs e)
        {
            if (profileEmployeeID <= 0)
                return;

            using EmployeeDetailViewForm detailForm =
                new EmployeeDetailViewForm(
                    profileEmployeeID,
                    profileEmployeeName,
                    EmployeeDetailView.PayrollRecords);

            detailForm.ShowDialog(this);
        }

        private Button CreateProfileNavigationButton(
            string text)
        {
            return new Button
            {
                Text = text,
                Size = new Size(94, 25),
                Font = new Font(
                    "Segoe UI",
                    7.5F),
                UseVisualStyleBackColor = true
            };
        }

        private void UpdateEmployeeProfileDrawerBounds()
        {
            if (employeeProfileDrawer == null)
                return;

            int drawerWidth =
                Math.Max(
                    220,
                    ClientSize.Width / 4);

            int visibleLeft =
                Math.Max(
                    0,
                    ClientSize.Width - drawerWidth);

            employeeProfileDrawer.Size =
                new Size(
                    drawerWidth,
                    ClientSize.Height);

            if (!employeeProfileDrawer.Visible)
            {
                employeeProfileDrawer.Location =
                    new Point(
                        ClientSize.Width,
                        0);

                return;
            }

            employeeProfileDrawer.Location =
                new Point(
                    visibleLeft,
                    0);

            PositionEmployeeProfileCloseButton();
        }

        private void PositionEmployeeProfileCloseButton()
        {
            if (employeeProfileDrawer == null ||
                btnCloseEmployeeProfile == null ||
                employeeProfileDrawer.IsDisposed ||
                btnCloseEmployeeProfile.IsDisposed)
            {
                return;
            }

            btnCloseEmployeeProfile.Location = new Point(
                Math.Max(
                    5,
                    employeeProfileDrawer.ClientSize.Width -
                    btnCloseEmployeeProfile.Width -
                    8),
                7);

            btnCloseEmployeeProfile.Visible = true;
            btnCloseEmployeeProfile.Enabled = true;
            btnCloseEmployeeProfile.BringToFront();
        }

        private void AnimateEmployeeProfileDrawer(
            bool open)
        {
            if (employeeProfileDrawer == null)
                return;

            int drawerWidth =
                employeeProfileDrawer.Width;

            int startX =
                open
                    ? ClientSize.Width
                    : ClientSize.Width - drawerWidth;

            int endX =
                open
                    ? ClientSize.Width - drawerWidth
                    : ClientSize.Width;

            employeeProfileDrawer.Visible = true;
            PositionEmployeeProfileCloseButton();

            const int frames = 12;

            for (int i = 1; i <= frames; i++)
            {
                double progress =
                    i / (double)frames;

                int x =
                    startX +
                    (int)((endX - startX) * progress);

                employeeProfileDrawer.Location =
                    new Point(
                        x,
                        0);

                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }

            employeeProfileDrawer.Location =
                new Point(
                    endX,
                    0);

            if (!open)
            {
                employeeProfileDrawer.Visible = false;
            }
        }

        private void EmployeeProfileCloseButton_Click(
            object? sender,
            EventArgs e)
        {
            AnimateEmployeeProfileDrawer(false);
        }

        private void EmployeesControl_Resize(
            object? sender,
            EventArgs e)
        {
            UpdateEmployeesResponsiveLayout();

            if (employeeProfileDrawer != null &&
                employeeProfileDrawer.Visible)
            {
                UpdateEmployeeProfileDrawerBounds();
            }
        }

                // ============================================================
        // EMPLOYEE INFORMATION
        // ============================================================

        private void ShowEmployeeInformation()
        {
            if (!EnsureProfileContent())
                return;

            ClearProfileContent();

            AddProfileSectionTitle(
                "EMPLOYEE INFORMATION");

            try
            {
                DataTable employees =
                    DatabaseHelper.GetEmployees("");

                DataRow? employeeRow = null;

                foreach (DataRow row in employees.Rows)
                {
                    if (Convert.ToInt64(row["EmployeeID"]) ==
                        profileEmployeeID)
                    {
                        employeeRow = row;
                        break;
                    }
                }

                if (employeeRow == null)
                {
                    AddProfileValue(
                        "Status",
                        "Employee record not found.");

                    return;
                }

                AddProfileValue(
                    "Employee ID",
                    employeeRow["EmployeeID"]?.ToString() ?? "");

                AddProfileValue(
                    "First Name",
                    employeeRow["FirstName"]?.ToString() ?? "");

                AddProfileValue(
                    "Last Name",
                    employeeRow["LastName"]?.ToString() ?? "");

                AddProfileValue(
                    "Position",
                    employeeRow["Position"]?.ToString() ?? "");

                AddProfileValue(
                    "Department",
                    employeeRow["Department"]?.ToString() ?? "");

                decimal salary =
                    Convert.ToDecimal(
                        employeeRow["Salary"],
                        CultureInfo.InvariantCulture);

                AddProfileValue(
                    "Salary / Monthly",
                    salary.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture));

                AddProfileValue(
                    "Status",
                    employeeRow["Status"]?.ToString() ?? "");

                AddProfileSectionTitle(
                    "GOVERNMENT IDs");

                DatabaseHelper.GetEmployeeGovernmentIDs(
                    profileEmployeeID,
                    out string sssNo,
                    out string philHealthPIN,
                    out string pagIBIGMID,
                    out string tin);

                AddProfileValue(
                    "SSS No.",
                    string.IsNullOrWhiteSpace(sssNo)
                        ? "Not provided"
                        : sssNo);

                AddProfileValue(
                    "PhilHealth PIN",
                    string.IsNullOrWhiteSpace(philHealthPIN)
                        ? "Not provided"
                        : philHealthPIN);

                AddProfileValue(
                    "Pag-IBIG MID",
                    string.IsNullOrWhiteSpace(pagIBIGMID)
                        ? "Not provided"
                        : pagIBIGMID);

                AddProfileValue(
                    "TIN",
                    string.IsNullOrWhiteSpace(tin)
                        ? "Not provided"
                        : tin);
            }
            catch (Exception ex)
            {
                AddProfileValue(
                    "Error",
                    ex.Message);
            }

        }

        // ============================================================
        // EMPLOYEE ATTENDANCE
        // ============================================================

        private void ShowEmployeeAttendance()
        {
            if (!EnsureProfileContent())
                return;

            ClearProfileContent();

            AddProfileSectionTitle(
                "ATTENDANCE HISTORY");

            DataGridView grid = CreateProfileGrid();

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Date",
                    "Date"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Time In",
                    "TimeIn"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Time Out",
                    "TimeOut"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Status",
                    "Status"));

            try
            {
                DataTable attendance =
                    DatabaseHelper.GetAttendance("");

                foreach (DataRow row in attendance.Rows)
                {
                    if (Convert.ToInt64(
                            row["EmployeeID"]) !=
                        profileEmployeeID)
                    {
                        continue;
                    }

                    grid.Rows.Add(
                        row["Date"]?.ToString() ?? "",
                        row["TimeIn"]?.ToString() ?? "",
                        row["TimeOut"]?.ToString() ?? "",
                        row["Status"]?.ToString() ?? "");
                }

                employeeProfileContent!.Controls.Add(grid);

                Label summary = new Label
                {
                    AutoSize = true,
                    Font = new Font(
                        "Segoe UI",
                        8F,
                        FontStyle.Bold),
                    Text =
                        "Attendance records: " +
                        grid.Rows.Count,
                    Location = new Point(
                        10,
                        grid.Bottom + 8)
                };

                employeeProfileContent.Controls.Add(
                    summary);
            }
            catch (Exception ex)
            {
                AddProfileValue(
                    "Error",
                    ex.Message);
            }

            UpdateProfileNavigationState(
                btnProfileAttendance);
        }

        // ============================================================
        // EMPLOYEE PAYROLL RECORDS
        // ============================================================

        private void ShowEmployeePayroll()
        {
            if (!EnsureProfileContent())
                return;

            ClearProfileContent();

            AddProfileSectionTitle(
                "PAYROLL HISTORY");

            DataGridView grid = CreateProfileGrid();

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Period",
                    "PayPeriod"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Gross",
                    "GrossPay"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Net",
                    "NetPay"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Status",
                    "PaymentStatus"));

            grid.Columns.Add(
                CreateGridTextColumn(
                    "Receipt",
                    "ReceiptNumber"));

            try
            {
                DataTable payroll =
                    PayrollDatabaseHelper.GetPayrollRecords("");

                foreach (DataRow row in payroll.Rows)
                {
                    if (Convert.ToInt64(
                            row["EmployeeID"]) !=
                        profileEmployeeID)
                    {
                        continue;
                    }

                    grid.Rows.Add(
                        row["PayPeriod"]?.ToString() ?? "",
                        FormatCurrencyValue(
                            row["GrossPay"]),
                        FormatCurrencyValue(
                            row["NetPay"]),
                        row.Table.Columns.Contains(
                            "PaymentStatus")
                            ? row["PaymentStatus"]?.ToString() ?? ""
                            : "UNPAID",
                        row.Table.Columns.Contains(
                            "ReceiptNumber")
                            ? row["ReceiptNumber"]?.ToString() ?? ""
                            : "");
                }

                employeeProfileContent!.Controls.Add(grid);

            }
            catch (Exception ex)
            {
                AddProfileValue(
                    "Error",
                    ex.Message);
            }

            UpdateProfileNavigationState(
                btnProfilePayroll);
        }

        // ============================================================
        // PROFILE UI HELPERS
        // ============================================================

        private bool EnsureProfileContent()
        {
            if (employeeProfileDrawer == null ||
                employeeProfileContent == null)
            {
                return false;
            }

            if (lblProfileEmployeeName != null)
            {
                lblProfileEmployeeName.Text =
                    profileEmployeeName;
            }

            if (lblProfileEmployeeID != null)
            {
                lblProfileEmployeeID.Text =
                    "Employee ID: " +
                    profileEmployeeID;
            }

            return true;
        }

        private void ClearProfileContent()
        {
            if (employeeProfileContent == null)
                return;

            foreach (Control control
                     in employeeProfileContent.Controls)
            {
                control.Dispose();
            }

            employeeProfileContent.Controls.Clear();
        }

        private void AddProfileSectionTitle(
            string text)
        {
            if (employeeProfileContent == null)
                return;

            Label label = new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold),
                Location = new Point(
                    10,
                    GetNextProfileY())
            };

            employeeProfileContent.Controls.Add(label);
        }

        private void AddProfileValue(
            string labelText,
            string value)
        {
            if (employeeProfileContent == null)
                return;

            int y = GetNextProfileY();

            Label label = new Label
            {
                Text = labelText + ":",
                AutoSize = false,
                Font = new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold),
                Location = new Point(
                    10,
                    y),
                Size = new Size(
                    Math.Max(
                        75,
                        employeeProfileContent.ClientSize.Width -
                        35),
                    18)
            };

            Label valueLabel = new Label
            {
                Text = value,
                AutoSize = false,
                Font = new Font(
                    "Segoe UI",
                    8F),
                Location = new Point(
                    10,
                    y + 18),
                Size = new Size(
                    Math.Max(
                        75,
                        employeeProfileContent.ClientSize.Width -
                        35),
                    34),
                AutoEllipsis = true
            };

            employeeProfileContent.Controls.Add(
                label);

            employeeProfileContent.Controls.Add(
                valueLabel);
        }

        private int GetNextProfileY()
        {
            if (employeeProfileContent == null ||
                employeeProfileContent.Controls.Count == 0)
            {
                return 8;
            }

            int bottom = 8;

            foreach (Control control
                     in employeeProfileContent.Controls)
            {
                bottom =
                    Math.Max(
                        bottom,
                        control.Bottom + 8);
            }

            return bottom;
        }

        private DataGridView CreateProfileGrid()
        {
            DataGridView grid = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(
                    10,
                    34),
                Size = new Size(
                    Math.Max(
                        140,
                        employeeProfileContent!.ClientSize.Width -
                        20),
                    Math.Max(
                        180,
                        employeeProfileContent.ClientSize.Height -
                        85)),
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right |
                    AnchorStyles.Bottom
            };

            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            grid.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // The profile attendance and payroll record tables use the same
            // sorting and centering behavior as the main employee grid.
            DataGridViewSortingHelper.Configure(grid);

            return grid;
        }

        private static DataGridViewTextBoxColumn
            CreateGridTextColumn(
                string header,
                string name)
        {
            return new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                Name = name,
                DataPropertyName = name,
                ReadOnly = true
            };
        }

        private static string FormatCurrencyValue(
            object? value)
        {
            decimal amount =
                Convert.ToDecimal(
                    value,
                    CultureInfo.InvariantCulture);

            return amount.ToString(
                "₱#,##0.00",
                CultureInfo.CurrentCulture);
        }

        private void UpdateProfileNavigationState(
            Button? activeButton)
        {
            if (activeButton == null)
                return;

            foreach (Button? button in new[]
                     {
                         btnProfileAttendance,
                         btnProfilePayroll
                     })
            {
                if (button == null)
                    continue;

                button.Enabled = true;
            }

            activeButton.Enabled = false;
        }

        // ============================================================
        // LIVE DATA REFRESH
        // ============================================================

        private void DataRefreshManager_DataChanged(
            object? sender,
            DataChangedEventArgs e)
        {
            // This control already refreshes itself after its own
            // add/update/delete operation.
            if (ReferenceEquals(e.Source, this))
            {
                return;
            }

            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            BeginInvoke(new Action(() =>
            {
                if (IsDisposed || !IsHandleCreated)
                {
                    return;
                }

                if (e.ChangeType == DataChangeType.Employee)
                {
                    LoadEmployees(txtSearch.Text.Trim());

                    if (profileEmployeeID > 0)
                    {
                        bool stillExists = false;

                        foreach (DataRow row in employeeTable.Rows)
                        {
                            if (Convert.ToInt64(row["EmployeeID"]) ==
                                profileEmployeeID)
                            {
                                stillExists = true;
                                profileEmployeeName =
                                    $"{row["FirstName"]} {row["LastName"]}".Trim();
                                break;
                            }
                        }

                        if (!stillExists)
                        {
                            AnimateEmployeeProfileDrawer(false);
                            profileEmployeeID = 0;
                            profileEmployeeName = "";
                            return;
                        }
                    }
                }

                if (employeeProfileDrawer != null &&
                    employeeProfileDrawer.Visible &&
                    profileEmployeeID > 0 &&
                    e.ChangeType == DataChangeType.Employee)
                {
                    ShowEmployeeInformation();
                }
            }));
        }

        private void EmployeesControl_Disposed(
            object? sender,
            EventArgs e)
        {
            DataRefreshManager.DataChanged -=
                DataRefreshManager_DataChanged;

            Disposed -=
                EmployeesControl_Disposed;
        }

        // ============================================================
        // SEARCH BUTTON
        // ============================================================

        private void btnSearch_Click(
            object? sender,
            EventArgs e)
        {
            LoadEmployees(
                txtSearch.Text.Trim(),
                true);
        }

        // ============================================================
        // LIVE SEARCH
        // ============================================================

        private void txtSearch_TextChanged(
            object? sender,
            EventArgs e)
        {
            LoadEmployees(
                txtSearch.Text.Trim());
        }

        // ============================================================
        // SEARCH ENTER KEY
        // ============================================================

        private void txtSearch_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadEmployees(
                    txtSearch.Text.Trim(),
                    true);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // ============================================================
        // ADD EMPLOYEE
        // ============================================================

        private void btnAddEmployee_Click(
            object? sender,
            EventArgs e)
        {
            try
            {
                long nextEmployeeID =
                    DatabaseHelper.GetNextEmployeeID();

                using AddEmployeeForm addEmployeeForm =
                    new AddEmployeeForm(
                        nextEmployeeID.ToString());

                DialogResult result =
                    addEmployeeForm.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return;
                }

                // SQLite generates the real EmployeeID.
                long newEmployeeID =
                    DatabaseHelper.AddEmployee(
                        addEmployeeForm.FirstName,
                        addEmployeeForm.LastName,
                        addEmployeeForm.Position,
                        addEmployeeForm.Department,
                        addEmployeeForm.Salary,
                        addEmployeeForm.Status);

                // Clear search so the newly added employee
                // is visible in the normal list.
                txtSearch.Clear();

                LoadEmployees();

                SelectEmployeeByID(
                    newEmployeeID);

                // Notify the other modules that a new employee exists.
                DataRefreshManager.Notify(
                    DataChangeType.Employee,
                    this);

                MessageBox.Show(
                    $"Employee added successfully.\n\n" +
                    $"Employee ID: {newEmployeeID}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add the employee.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // UPDATE EMPLOYEE
        // ============================================================

        private void btnUpdateEmployee_Click(
            object? sender,
            EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null)
            {
                MessageBox.Show(
                    "Please select an employee first.",
                    "Update Employee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                DataGridViewRow selectedRow =
                    dgvEmployees.CurrentRow;

                // IMPORTANT:
                // Use the actual DataTable column name:
                // EmployeeID
                object idValue =
                    selectedRow.Cells[
                        colEmployeeID.Index].Value;

                if (idValue == null ||
                    idValue == DBNull.Value ||
                    !long.TryParse(
                        idValue.ToString(),
                        out long employeeID))
                {
                    MessageBox.Show(
                        "Unable to determine the selected Employee ID.",
                        "Update Employee",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string firstName =
                    Convert.ToString(
                        selectedRow.Cells[
                            colFirstName.Index].Value) ?? "";

                string lastName =
                    Convert.ToString(
                        selectedRow.Cells[
                            colLastName.Index].Value) ?? "";

                string position =
                    Convert.ToString(
                        selectedRow.Cells[
                            colPosition.Index].Value) ?? "";

                string department =
                    Convert.ToString(
                        selectedRow.Cells[
                            colDepartment.Index].Value) ?? "";

                decimal salary =
                    Convert.ToDecimal(
                        selectedRow.Cells[
                            colSalary.Index].Value);

                string status =
                    Convert.ToString(
                        selectedRow.Cells[
                            colStatus.Index].Value) ?? "";

                using UpdateEmployeeForm updateEmployeeForm =
                    new UpdateEmployeeForm(
                        employeeID.ToString(),
                        firstName,
                        lastName,
                        position,
                        department,
                        salary,
                        status);

                DialogResult result =
                    updateEmployeeForm.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return;
                }

                bool updated =
                    DatabaseHelper.UpdateEmployee(
                        employeeID,
                        updateEmployeeForm.FirstName,
                        updateEmployeeForm.LastName,
                        updateEmployeeForm.Position,
                        updateEmployeeForm.Department,
                        updateEmployeeForm.Salary,
                        updateEmployeeForm.Status);

                if (!updated)
                {
                    MessageBox.Show(
                        "The employee could not be updated.",
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                txtSearch.Clear();

                LoadEmployees();

                SelectEmployeeByID(
                    employeeID);

                // Notify the other modules that employee data changed.
                DataRefreshManager.Notify(
                    DataChangeType.Employee,
                    this);

                MessageBox.Show(
                    "Employee updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update the employee.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DELETE EMPLOYEE
        // ============================================================

        private void btnDeleteEmployee_Click(
            object? sender,
            EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null)
            {
                MessageBox.Show(
                    "Please select an employee first.",
                    "Delete Employee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                DataGridViewRow selectedRow =
                    dgvEmployees.CurrentRow;

                object idValue =
                    selectedRow.Cells[
                        colEmployeeID.Index].Value;

                if (idValue == null ||
                    idValue == DBNull.Value ||
                    !long.TryParse(
                        idValue.ToString(),
                        out long employeeID))
                {
                    MessageBox.Show(
                        "Unable to determine the selected Employee ID.",
                        "Delete Employee",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string firstName =
                    Convert.ToString(
                        selectedRow.Cells[
                            colFirstName.Index].Value) ?? "";

                string lastName =
                    Convert.ToString(
                        selectedRow.Cells[
                            colLastName.Index].Value) ?? "";

                DialogResult confirmation =
                    MessageBox.Show(
                        $"Are you sure you want to delete:\n\n" +
                        $"{firstName} {lastName}\n" +
                        $"Employee ID: {employeeID}?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                bool deleted =
                    DatabaseHelper.DeleteEmployee(
                        employeeID);

                if (!deleted)
                {
                    MessageBox.Show(
                        "The employee could not be deleted.",
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                txtSearch.Clear();

                LoadEmployees();

                // Notify Attendance, Payroll, and Dashboard.
                DataRefreshManager.Notify(
                    DataChangeType.Employee,
                    this);

                MessageBox.Show(
                    "Employee deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete the employee.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // SELECT EMPLOYEE BY ID
        // ============================================================

        private void SelectEmployeeByID(
            long employeeID)
        {
            foreach (DataGridViewRow row
                     in dgvEmployees.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                object? value =
                    row.Cells[
                        colEmployeeID.Index].Value;

                if (value != null &&
                    value != DBNull.Value &&
                    long.TryParse(
                        value.ToString(),
                        out long rowEmployeeID) &&
                    rowEmployeeID == employeeID)
                {
                    row.Selected = true;

                    dgvEmployees.CurrentCell =
                        row.Cells[
                            colEmployeeID.Index];

                    return;
                }
            }
        }

        // ============================================================
        // OLD CLEAR EVENT
        // ============================================================
        // Kept so an old Designer event hookup will not cause
        // a compile error. The Clear button itself can remain hidden.

        private void btnClearEmployee_Click(
            object? sender,
            EventArgs e)
        {
            txtSearch.Clear();

            LoadEmployees();
        }

        // ============================================================
        // OLD DESIGNER LABEL EVENT
        // ============================================================
        // Kept for compatibility with the existing Designer.

        private void lblEmployeesSubtitle_Click(
            object? sender,
            EventArgs e)
        {
            // No action required.
        }
    }
}