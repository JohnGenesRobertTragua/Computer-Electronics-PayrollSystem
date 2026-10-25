using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class AttendanceControl : UserControl
    {
        private DataTable attendanceTable =
            new DataTable();

        private long selectedAttendanceID = 0;

        // ------------------------------------------------------------
        // TIME STATES
        // ------------------------------------------------------------

        private bool timeInRecorded = false;
        private bool timeOutRecorded = false;

        // ------------------------------------------------------------
        // Prevents SelectionChanged from restoring data while
        // the form is being cleared.
        // ------------------------------------------------------------

        private bool isClearingFields = false;

        // Prevents the DataGridView's automatic selection during
        // loading/initial display from being treated as a user selection.
        private bool isLoadingAttendance = false;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public AttendanceControl()
        {
            InitializeComponent();

            ConfigureAttendance();

            // The Designer supplies the controls; this layout routine keeps
            // the entry card, search bar and history grid aligned as the
            // dashboard content area changes size.
            Resize += (_, _) => LayoutAttendanceContent();
            LayoutAttendanceContent();

            LoadEmployees();
            LoadAttendance();

            ClearAttendanceFields();
        }

        // ============================================================
        // CONFIGURATION
        // ============================================================

        private void ConfigureAttendance()
        {
            // --------------------------------------------------------
            // DATE
            // --------------------------------------------------------

            dtpAttendanceDate.Value =
                DateTime.Today;

            // --------------------------------------------------------
            // TIME IN / TIME OUT
            // --------------------------------------------------------

            dtpTimeIn.ShowCheckBox =
                true;

            dtpTimeOut.ShowCheckBox =
                true;

            dtpTimeIn.SetTime(
                DateTime.Now);

            dtpTimeOut.SetTime(
                DateTime.Now);

            dtpTimeIn.SetCheckedState(
                false);

            dtpTimeOut.SetCheckedState(
                false);

            // --------------------------------------------------------
            // STATUS
            // --------------------------------------------------------

            cmbStatus.SelectedIndex =
                -1;

            // --------------------------------------------------------
            // GRID
            // --------------------------------------------------------

            dgvAttendance.AutoGenerateColumns =
                false;

            dgvAttendance.ReadOnly =
                true;

            dgvAttendance.AllowUserToAddRows =
                false;

            dgvAttendance.AllowUserToDeleteRows =
                false;

            dgvAttendance.AllowUserToResizeRows =
                false;

            dgvAttendance.MultiSelect =
                false;

            dgvAttendance.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAttendance.RowHeadersVisible =
                false;

            dgvAttendance.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvAttendance.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvAttendance.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // Apply the updated visual theme while leaving the existing
            // Designer positions and all attendance behavior intact.
            ApplyAttendanceVisualStyle();

            // --------------------------------------------------------
            // EVENTS
            // --------------------------------------------------------
            //
            // TIME OUT and UPDATE are already connected in the
            // Designer, so they are not connected here again.
            //

            btnTimeIn.Click +=
                btnTimeIn_Click;

            btnSaveAttendance.Click +=
                btnSaveAttendance_Click;

            btnDeleteAttendance.Click +=
                btnDeleteAttendance_Click;

            btnClear.Click +=
                btnClear_Click;

            btnSearch.Click +=
                btnSearch_Click;

            txtSearch.TextChanged +=
                txtSearch_TextChanged;

            txtSearch.KeyDown +=
                txtSearch_KeyDown;

            dgvAttendance.SelectionChanged +=
                dgvAttendance_SelectionChanged;

            // When this UserControl becomes visible again from the
            // Dashboard, force it back to a clean Add state.
            VisibleChanged +=
                AttendanceControl_VisibleChanged;

            // Listen for live changes made by other modules.
            DataRefreshManager.DataChanged +=
                DataRefreshManager_DataChanged;

            Disposed +=
                AttendanceControl_Disposed;

            // --------------------------------------------------------
            // Initial button states
            // --------------------------------------------------------

            SetAddMode();
        }

        // ============================================================
        // ATTENDANCE VISUAL STYLE
        // ============================================================

        private void ApplyAttendanceVisualStyle()
        {
            Color pageBackground = Color.FromArgb(248, 250, 252);
            Color headingColor = Color.FromArgb(15, 23, 42);
            Color secondaryText = Color.FromArgb(71, 85, 105);
            Color borderColor = Color.FromArgb(226, 232, 240);
            Color headerColor = Color.FromArgb(30, 41, 59);
            Color primaryBlue = Color.FromArgb(37, 99, 235);
            Color primaryBlueHover = Color.FromArgb(29, 78, 216);
            Color timeInGreen = Color.FromArgb(22, 163, 74);
            Color timeInGreenHover = Color.FromArgb(21, 128, 61);
            Color timeOutTeal = Color.FromArgb(13, 148, 136);
            Color timeOutTealHover = Color.FromArgb(15, 118, 110);

            // Main canvas and page heading.
            BackColor = pageBackground;
            ForeColor = headingColor;

            lblAttendanceTitle.ForeColor = headingColor;
            lblAttendanceTitle.Font =
                new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);

            lblAttendanceSubtitle.ForeColor = secondaryText;
            lblAttendanceSubtitle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            // Entry area is visually separated from the page background.
            pnlEntry.BackColor = Color.White;
            pnlEntry.BorderStyle = BorderStyle.FixedSingle;

            foreach (Control control in pnlEntry.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = secondaryText;
                    label.Font =
                        new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                }
            }

            // Consistent, easy-to-read input fields.
            cmbEmployee.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            cmbEmployee.BackColor = Color.White;
            cmbEmployee.ForeColor = headingColor;
            // Standard style keeps the field outline visible against the white panel.
            cmbEmployee.FlatStyle = FlatStyle.Standard;
            cmbEmployee.DrawMode = DrawMode.Normal;

            cmbStatus.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            cmbStatus.BackColor = Color.White;
            cmbStatus.ForeColor = headingColor;
            // Standard style keeps the status selector visibly bounded.
            cmbStatus.FlatStyle = FlatStyle.Standard;
            cmbStatus.DrawMode = DrawMode.Normal;

            dtpAttendanceDate.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpTimeIn.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dtpTimeOut.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // Search row expands with the available page width instead of
            // leaving a large unused area to the right of a short textbox.
            pnlSearch.BackColor = pageBackground;
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlSearch.Resize += pnlSearch_Resize;
            UpdateAttendanceSearchLayout();

            lblSearch.ForeColor = secondaryText;
            lblSearch.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);

            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.BackColor = Color.White;
            txtSearch.ForeColor = headingColor;
            txtSearch.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            // Action buttons use consistent flat styling and distinct actions.
            StyleAttendanceButton(
                btnTimeIn,
                timeInGreen,
                Color.White,
                timeInGreenHover);

            StyleAttendanceButton(
                btnTimeOut,
                timeOutTeal,
                Color.White,
                timeOutTealHover);

            // Match the Employees form: compact icon-only search button,
            // neutral gray surface, and a thin understated outline.
            btnSearch.Text = "\uE721";
            btnSearch.Font = new Font("Segoe MDL2 Assets", 14F, FontStyle.Regular);
            btnSearch.AccessibleName = "Search attendance";
            btnSearch.AccessibleDescription = "Search attendance records using the entered text";
            btnSearch.TextAlign = ContentAlignment.MiddleCenter;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.BackColor = Color.FromArgb(226, 232, 240);
            btnSearch.ForeColor = Color.FromArgb(30, 58, 95);
            btnSearch.FlatAppearance.BorderSize = 1;
            btnSearch.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(203, 213, 225);
            btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(186, 199, 216);
            btnSearch.Cursor = Cursors.Hand;

            // CLEAR uses a strong neutral slate color so it is clearly
            // distinguishable from the primary Save/Search actions.
            StyleAttendanceButton(
                btnClear,
                Color.FromArgb(71, 85, 105),
                Color.White,
                Color.FromArgb(51, 65, 85));

            // SAVE, UPDATE, and DELETE use distinct colors. Disabled
            // buttons keep a soft tint of their action color, instead of
            // becoming indistinguishable gray controls.
            StyleAttendanceButton(
                btnSaveAttendance,
                primaryBlue,
                Color.White,
                primaryBlueHover);

            StyleAttendanceButton(
                btnEditAttendance,
                Color.FromArgb(124, 58, 237),
                Color.White,
                Color.FromArgb(109, 40, 217));

            StyleAttendanceButton(
                btnDeleteAttendance,
                Color.FromArgb(220, 38, 38),
                Color.White,
                Color.FromArgb(185, 28, 28));

            UpdateModeButtonAppearance();

            // Refined table styling: stronger header contrast, soft row stripes,
            // consistent spacing, and proportional widths across the form.
            dgvAttendance.BackgroundColor = Color.White;
            dgvAttendance.BorderStyle = BorderStyle.FixedSingle;
            dgvAttendance.GridColor = borderColor;
            dgvAttendance.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAttendance.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAttendance.EnableHeadersVisualStyles = false;
            dgvAttendance.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvAttendance.ColumnHeadersHeight = 38;
            dgvAttendance.RowTemplate.Height = 36;
            dgvAttendance.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAttendance.DefaultCellStyle.BackColor = Color.White;
            dgvAttendance.DefaultCellStyle.ForeColor = headingColor;
            dgvAttendance.DefaultCellStyle.SelectionBackColor = primaryBlue;
            dgvAttendance.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAttendance.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgvAttendance.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 250, 252);
            dgvAttendance.AlternatingRowsDefaultCellStyle.ForeColor = headingColor;
            dgvAttendance.AlternatingRowsDefaultCellStyle.SelectionBackColor = primaryBlue;
            dgvAttendance.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgvAttendance.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            dgvAttendance.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAttendance.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dgvAttendance.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerColor;
            dgvAttendance.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvAttendance.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgvAttendance.DefaultCellStyle.Font =
                new Font("Segoe UI", 9.25F, FontStyle.Regular, GraphicsUnit.Point);

            colAttendanceID.FillWeight = 75F;
            colEmployeeID.FillWeight = 75F;
            colEmployeeName.FillWeight = 150F;
            colDate.FillWeight = 105F;
            colTimeIn.FillWeight = 105F;
            colTimeOut.FillWeight = 105F;
            colStatus.FillWeight = 85F;

            colAttendanceID.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            colEmployeeID.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            colEmployeeName.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            colDate.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            colTimeIn.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            colTimeOut.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            colStatus.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // Enforce center alignment for every column and enable
            // ascending/descending sorting by clicking its header.
            DataGridViewSortingHelper.Configure(dgvAttendance);
            FuturisticUiTheme.Apply(this);
        }

        private void LayoutAttendanceContent()
        {
            if (IsDisposed || ClientSize.Width <= 0 || pnlEntry == null ||
                pnlSearch == null || dgvAttendance == null)
                return;

            const int margin = 24;
            int contentWidth = Math.Max(300, ClientSize.Width - margin * 2);

            lblAttendanceTitle.SetBounds(margin, 14, contentWidth, 34);
            lblAttendanceSubtitle.SetBounds(margin + 2, 49, contentWidth, 22);

            pnlEntry.SetBounds(margin, 82, contentWidth, 132);
            pnlEntry.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Keep the four record actions grouped toward the right-center
            // of the entry panel instead of hugging its far-right edge.
            // Their two-column layout remains consistent as the form resizes.
            int actionLeft = Math.Max(560, pnlEntry.ClientSize.Width - 340);
            btnSaveAttendance.SetBounds(actionLeft, 30, 88, 30);
            btnEditAttendance.SetBounds(actionLeft + 118, 30, 88, 30);
            btnDeleteAttendance.SetBounds(actionLeft, 84, 88, 30);
            btnClear.SetBounds(actionLeft + 118, 84, 88, 30);

            pnlSearch.SetBounds(margin, 226, contentWidth, 40);
            pnlSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UpdateAttendanceSearchLayout();

            // Place the grid's Sort By button on the same horizontal toolbar
            // as the search field, rather than floating between the search
            // panel and the grid. The search layout already reserves a lane
            // for this button, so the two buttons cannot overlap.
            Button? sortButton = Controls.Find("btnSortBy_dgvAttendance", true)
                .OfType<Button>()
                .FirstOrDefault();
            if (sortButton != null && !sortButton.IsDisposed)
            {
                sortButton.Left = Math.Max(margin, pnlSearch.Right - sortButton.Width);
                sortButton.Top = pnlSearch.Top + txtSearch.Top + (txtSearch.Height - sortButton.Height) / 2;
                sortButton.BringToFront();
            }

            // Keep a small, consistent gap between the search/sort toolbar
            // and the grid header, as in the Employees form.
            int gridTop = 274;
            dgvAttendance.SetBounds(margin, gridTop, contentWidth,
                Math.Max(120, ClientSize.Height - gridTop - 22));
            dgvAttendance.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void pnlSearch_Resize(object? sender, EventArgs e)
        {
            UpdateAttendanceSearchLayout();
        }

        private void UpdateAttendanceSearchLayout()
        {
            if (pnlSearch == null ||
                txtSearch == null ||
                btnSearch == null ||
                lblSearch == null)
            {
                return;
            }

            // Reserve a separate right-side lane for the DataGridView sorter.
            // Without this gap, the Search button and the floating Sort By
            // button occupy the same area at the top-right of the grid.
            const int sorterLaneWidth = 96;
            const int rightMargin = 8;
            const int gap = 8;
            const int searchButtonWidth = 42;

            // Icon button matches the textbox height and only reserves enough
            // width for the magnifying-glass glyph.
            btnSearch.Width = searchButtonWidth;
            btnSearch.Height = txtSearch.Height;
            btnSearch.Top = txtSearch.Top;
            btnSearch.Text = "\uE721";
            btnSearch.Font = new Font("Segoe MDL2 Assets", 14F, FontStyle.Regular);

            int buttonLeft = Math.Max(
                lblSearch.Right + 10,
                pnlSearch.ClientSize.Width - btnSearch.Width - sorterLaneWidth - rightMargin);

            btnSearch.Left = buttonLeft;
            txtSearch.Width = Math.Max(
                120,
                btnSearch.Left - txtSearch.Left - gap);
        }

        private static void StyleAttendanceButton(
            Button button,
            Color backgroundColor,
            Color foregroundColor,
            Color hoverColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.BackColor = backgroundColor;
            button.ForeColor = foregroundColor;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = hoverColor;
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(hoverColor);
            button.Cursor = Cursors.Hand;
            button.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        }

        private void UpdateModeButtonAppearance()
        {
            UpdateAttendanceModeButton(
                btnSaveAttendance,
                Color.FromArgb(37, 99, 235),
                Color.FromArgb(29, 78, 216),
                Color.FromArgb(219, 234, 254),
                Color.FromArgb(96, 133, 196));

            UpdateAttendanceModeButton(
                btnEditAttendance,
                Color.FromArgb(124, 58, 237),
                Color.FromArgb(109, 40, 217),
                Color.FromArgb(237, 233, 254),
                Color.FromArgb(139, 115, 196));

            UpdateAttendanceModeButton(
                btnDeleteAttendance,
                Color.FromArgb(220, 38, 38),
                Color.FromArgb(185, 28, 28),
                Color.FromArgb(254, 226, 226),
                Color.FromArgb(190, 105, 105));
        }

        private static void UpdateAttendanceModeButton(
            Button button,
            Color enabledColor,
            Color enabledHoverColor,
            Color disabledColor,
            Color disabledTextColor)
        {
            // Keep the intended action color even when the action is
            // temporarily unavailable in the current form mode.
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            if (button.Enabled)
            {
                button.BackColor = enabledColor;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = enabledHoverColor;
                button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(enabledHoverColor);
            }
            else
            {
                button.BackColor = disabledColor;
                button.ForeColor = disabledTextColor;
                button.FlatAppearance.MouseOverBackColor = disabledColor;
                button.FlatAppearance.MouseDownBackColor = disabledColor;
            }
        }

        // ============================================================
        // BUTTON MODES
        // ============================================================

        private void SetAddMode()
        {
            // --------------------------------------------------------
            // No attendance record is selected.
            // User is creating a NEW attendance record.
            // --------------------------------------------------------

            btnSaveAttendance.Enabled =
                true;

            btnEditAttendance.Enabled =
                false;

            btnDeleteAttendance.Enabled =
                false;

            btnClear.Enabled =
                true;

            UpdateModeButtonAppearance();
        }

        private void SetEditMode()
        {
            // --------------------------------------------------------
            // An attendance record is selected.
            // User is editing an EXISTING record.
            // --------------------------------------------------------

            btnSaveAttendance.Enabled =
                false;

            btnEditAttendance.Enabled =
                true;

            btnDeleteAttendance.Enabled =
                true;

            btnClear.Enabled =
                true;

            UpdateModeButtonAppearance();
        }

        // ============================================================
        // LOAD EMPLOYEES
        // ============================================================

        // ============================================================
        // REFRESH EMPLOYEE LIST
        // ============================================================

        public void RefreshEmployeeList()
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                DataTable employees =
                    DatabaseHelper.GetAttendanceEmployees();

                cmbEmployee.DataSource =
                    null;

                cmbEmployee.DisplayMember =
                    "EmployeeName";

                cmbEmployee.ValueMember =
                    "EmployeeID";

                cmbEmployee.DataSource =
                    employees;

                cmbEmployee.SelectedIndex =
                    -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load employees.\n\n" +
                    ex.Message,
                    "Attendance Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LOAD ATTENDANCE
        // ============================================================

        private void LoadAttendance(
            string searchText = "",
            bool showNoResultsMessage = false)
        {
            try
            {
                isLoadingAttendance = true;

                attendanceTable =
                    DatabaseHelper.GetAttendance(
                        searchText);

                dgvAttendance.DataSource =
                    null;

                dgvAttendance.DataSource =
                    attendanceTable;

                // WinForms can automatically make the first row current
                // when a DataSource is assigned. Remove that automatic
                // selection so loading is never treated as editing.
                try
                {
                    dgvAttendance.CurrentCell = null;
                }
                catch
                {
                    // Ignore when the grid is not ready.
                }

                dgvAttendance.ClearSelection();

                if (showNoResultsMessage &&
                    attendanceTable.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No attendance records were found.",
                        "Search",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load attendance records.\n\n" +
                    ex.Message,
                    "Attendance Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingAttendance = false;
            }

            SetAddMode();
        }

        // ============================================================
        // ATTENDANCE CONTROL VISIBILITY
        // ============================================================

        private void AttendanceControl_VisibleChanged(
            object? sender,
            EventArgs e)
        {
            if (!Visible || IsDisposed)
            {
                return;
            }

            // Always refresh the employee list when Attendance becomes
            // visible. Employees can be added from the Employees screen
            // while this control is already loaded in the Dashboard.
            LoadEmployees();

            // The DataGridView may restore its first-row selection after
            // the control is made visible. Run the clear after WinForms
            // finishes that automatic initialization.
            if (IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        ClearAttendanceFields();
                    }
                }));
            }
        }

        // ============================================================
        // LIVE DATA REFRESH
        // ============================================================

        private void DataRefreshManager_DataChanged(
            object? sender,
            DataChangedEventArgs e)
        {
            // This control already refreshes itself after its own
            // attendance operation.
            if (ReferenceEquals(e.Source, this))
            {
                return;
            }

            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            if (e.ChangeType == DataChangeType.Employee)
            {
                RefreshEmployeeList();

                LoadAttendance(
                    txtSearch.Text.Trim());

                ClearAttendanceFields();

                return;
            }

            if (e.ChangeType == DataChangeType.Attendance)
            {
                LoadAttendance(
                    txtSearch.Text.Trim());

                ClearAttendanceFields();
            }
        }

        private void AttendanceControl_Disposed(
            object? sender,
            EventArgs e)
        {
            DataRefreshManager.DataChanged -=
                DataRefreshManager_DataChanged;

            Disposed -=
                AttendanceControl_Disposed;
        }

        // ============================================================
        // TIME IN TOGGLE
        // ============================================================

        private void btnTimeIn_Click(
            object? sender,
            EventArgs e)
        {
            if (cmbEmployee.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select an employee first.",
                    "Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbEmployee.Focus();

                return;
            }

            // --------------------------------------------------------
            // OFF -> ON
            // --------------------------------------------------------

            if (!timeInRecorded)
            {
                DateTime now =
                    DateTime.Now;

                dtpTimeIn.SetTime(
                    now);

                dtpTimeIn.SetCheckedState(
                    true);

                timeInRecorded =
                    true;

                if (string.IsNullOrWhiteSpace(
                        cmbStatus.Text))
                {
                    cmbStatus.SelectedItem =
                        "Present";
                }

                return;
            }

            // --------------------------------------------------------
            // ON -> OFF
            // --------------------------------------------------------

            dtpTimeIn.SetTime(
                DateTime.Now);

            dtpTimeIn.SetCheckedState(
                false);

            timeInRecorded =
                false;
        }

        // ============================================================
        // TIME OUT TOGGLE
        // ============================================================

        private void btnTimeOut_Click(
            object? sender,
            EventArgs e)
        {
            if (cmbEmployee.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select an employee first.",
                    "Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbEmployee.Focus();

                return;
            }

            // --------------------------------------------------------
            // OFF -> ON
            // --------------------------------------------------------

            if (!timeOutRecorded)
            {
                DateTime now =
                    DateTime.Now;

                dtpTimeOut.SetTime(
                    now);

                dtpTimeOut.SetCheckedState(
                    true);

                timeOutRecorded =
                    true;

                if (string.IsNullOrWhiteSpace(
                        cmbStatus.Text))
                {
                    cmbStatus.SelectedItem =
                        "Present";
                }

                return;
            }

            // --------------------------------------------------------
            // ON -> OFF
            // --------------------------------------------------------

            dtpTimeOut.SetTime(
                DateTime.Now);

            dtpTimeOut.SetCheckedState(
                false);

            timeOutRecorded =
                false;
        }

        // ============================================================
        // SAVE - NEW ATTENDANCE ONLY
        // ============================================================

        private void btnSaveAttendance_Click(
            object? sender,
            EventArgs e)
        {
            // Safety check.
            // SAVE must never work while editing a selected record.

            if (selectedAttendanceID > 0)
            {
                MessageBox.Show(
                    "An attendance record is currently selected.\n\n" +
                    "Use UPDATE to save changes, or CLEAR to start a new record.",
                    "Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (!ValidateAttendanceFields())
            {
                return;
            }

            try
            {
                long employeeID =
                    Convert.ToInt64(
                        cmbEmployee.SelectedValue);

                string date =
                    dtpAttendanceDate.Value.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                string? timeIn =
                    GetTimeValue(
                        dtpTimeIn,
                        timeInRecorded);

                string? timeOut =
                    GetTimeValue(
                        dtpTimeOut,
                        timeOutRecorded);

                DatabaseHelper.AddAttendance(
                    employeeID,
                    date,
                    timeIn,
                    timeOut,
                    cmbStatus.Text.Trim());

                MessageBox.Show(
                    "Attendance record saved successfully.",
                    "Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Reload the table.
                LoadAttendance(
                    txtSearch.Text.Trim());

                // Automatically return to Add Mode.
                ClearAttendanceFields();

                // Notify Payroll and Dashboard immediately.
                DataRefreshManager.Notify(
                    DataChangeType.Attendance,
                    this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save the attendance record.\n\n" +
                    ex.Message,
                    "Attendance Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // UPDATE EXISTING ATTENDANCE
        // ============================================================

        private void btnEditAttendance_Click_1(
            object? sender,
            EventArgs e)
        {
            if (selectedAttendanceID <= 0)
            {
                MessageBox.Show(
                    "Please select an attendance record first.",
                    "Update Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!ValidateAttendanceFields())
            {
                return;
            }

            try
            {
                long employeeID =
                    Convert.ToInt64(
                        cmbEmployee.SelectedValue);

                string date =
                    dtpAttendanceDate.Value.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                string? timeIn =
                    GetTimeValue(
                        dtpTimeIn,
                        timeInRecorded);

                string? timeOut =
                    GetTimeValue(
                        dtpTimeOut,
                        timeOutRecorded);

                DatabaseHelper.UpdateAttendance(
                    selectedAttendanceID,
                    employeeID,
                    date,
                    timeIn,
                    timeOut,
                    cmbStatus.Text.Trim());

                MessageBox.Show(
                    "Attendance record updated successfully.",
                    "Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Reload the table.
                LoadAttendance(
                    txtSearch.Text.Trim());

                // Automatically return to Add Mode.
                ClearAttendanceFields();

                // Notify Payroll and Dashboard immediately.
                DataRefreshManager.Notify(
                    DataChangeType.Attendance,
                    this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update the attendance record.\n\n" +
                    ex.Message,
                    "Attendance Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DELETE
        // ============================================================

        private void btnDeleteAttendance_Click(
            object? sender,
            EventArgs e)
        {
            if (selectedAttendanceID <= 0)
            {
                MessageBox.Show(
                    "Please select an attendance record first.",
                    "Delete Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete this attendance record?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                DatabaseHelper.DeleteAttendance(
                    selectedAttendanceID);

                MessageBox.Show(
                    "Attendance record deleted successfully.",
                    "Attendance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadAttendance(
                    txtSearch.Text.Trim());

                // Automatically return to Add Mode.
                ClearAttendanceFields();

                // Notify Payroll and Dashboard immediately.
                DataRefreshManager.Notify(
                    DataChangeType.Attendance,
                    this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete the attendance record.\n\n" +
                    ex.Message,
                    "Attendance Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // CLEAR
        // ============================================================

        private void btnClear_Click(
            object? sender,
            EventArgs e)
        {
            ClearAttendanceFields();
        }

        private void ClearAttendanceFields()
        {
            isClearingFields =
                true;

            try
            {
                // ----------------------------------------------------
                // RESET SELECTED RECORD
                // ----------------------------------------------------

                selectedAttendanceID =
                    0;

                // ----------------------------------------------------
                // RESET TIME STATES
                // ----------------------------------------------------

                timeInRecorded =
                    false;

                timeOutRecorded =
                    false;

                // ----------------------------------------------------
                // EMPLOYEE
                // ----------------------------------------------------

                cmbEmployee.SelectedIndex =
                    -1;

                // ----------------------------------------------------
                // DATE
                // ----------------------------------------------------

                dtpAttendanceDate.Value =
                    DateTime.Today;

                // ----------------------------------------------------
                // TIME IN
                // ----------------------------------------------------

                dtpTimeIn.SetTime(
                    DateTime.Now);

                dtpTimeIn.SetCheckedState(
                    false);

                // ----------------------------------------------------
                // TIME OUT
                // ----------------------------------------------------

                dtpTimeOut.SetTime(
                    DateTime.Now);

                dtpTimeOut.SetCheckedState(
                    false);

                // ----------------------------------------------------
                // STATUS
                // ----------------------------------------------------

                cmbStatus.SelectedIndex =
                    -1;

                // ----------------------------------------------------
                // GRID
                // ----------------------------------------------------

                try
                {
                    dgvAttendance.CurrentCell =
                        null;
                }
                catch
                {
                    // Ignore when grid is not ready.
                }

                dgvAttendance.ClearSelection();

                // ----------------------------------------------------
                // RETURN TO ADD MODE
                // ----------------------------------------------------

                SetAddMode();
            }
            finally
            {
                isClearingFields =
                    false;
            }
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private bool ValidateAttendanceFields()
        {
            if (cmbEmployee.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select an employee.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbEmployee.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    cmbStatus.Text))
            {
                MessageBox.Show(
                    "Please select an attendance status.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbStatus.Focus();

                return false;
            }

            // Only validate time order when both buttons
            // have actually recorded a time.

            if (timeInRecorded &&
                timeOutRecorded &&
                dtpTimeOut.Value <
                dtpTimeIn.Value)
            {
                MessageBox.Show(
                    "Time Out cannot be earlier than Time In.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        // ============================================================
        // GET TIME VALUE
        // ============================================================

        private string? GetTimeValue(
            DateTimePicker picker,
            bool recorded)
        {
            if (!recorded)
            {
                return null;
            }

            return picker.Value.ToString(
                "HH:mm:ss",
                CultureInfo.InvariantCulture);
        }

        // ============================================================
        // SEARCH
        // ============================================================

        private void btnSearch_Click(
            object? sender,
            EventArgs e)
        {
            LoadAttendance(
                txtSearch.Text.Trim(),
                true);

            // Searching should leave us ready to
            // create a new attendance record.
            ClearAttendanceFields();
        }

        private void txtSearch_TextChanged(
            object? sender,
            EventArgs e)
        {
            LoadAttendance(
                txtSearch.Text.Trim());

            ClearAttendanceFields();
        }

        private void txtSearch_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadAttendance(
                    txtSearch.Text.Trim(),
                    true);

                ClearAttendanceFields();

                e.Handled =
                    true;

                e.SuppressKeyPress =
                    true;
            }
        }

        // ============================================================
        // GRID SELECTION
        // ============================================================

        private void dgvAttendance_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            if (DataGridViewSortingHelper.IsSorting(dgvAttendance))
                return;

            if (isLoadingAttendance)
            {
                return;
            }

            if (isClearingFields)
            {
                return;
            }

            if (dgvAttendance.CurrentRow == null)
            {
                SetAddMode();

                return;
            }

            if (dgvAttendance.CurrentRow.DataBoundItem
                is not DataRowView rowView)
            {
                SetAddMode();

                return;
            }

            DataRow row =
                rowView.Row;

            try
            {
                // ----------------------------------------------------
                // ATTENDANCE ID
                // ----------------------------------------------------

                selectedAttendanceID =
                    Convert.ToInt64(
                        row["AttendanceID"]);

                // ----------------------------------------------------
                // ENTER EDIT MODE
                // ----------------------------------------------------

                SetEditMode();

                // ----------------------------------------------------
                // EMPLOYEE
                // ----------------------------------------------------

                long employeeID =
                    Convert.ToInt64(
                        row["EmployeeID"]);

                SelectEmployee(
                    employeeID);

                // ----------------------------------------------------
                // DATE
                // ----------------------------------------------------

                if (DateTime.TryParse(
                        row["Date"]?.ToString(),
                        out DateTime attendanceDate))
                {
                    dtpAttendanceDate.Value =
                        attendanceDate;
                }

                // ----------------------------------------------------
                // TIME IN
                // ----------------------------------------------------

                string timeIn =
                    row["TimeIn"]?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(timeIn) &&
                    TimeSpan.TryParse(
                        timeIn,
                        out TimeSpan timeInValue))
                {
                    dtpTimeIn.SetTime(
                        DateTime.Today.Add(
                            timeInValue));

                    dtpTimeIn.SetCheckedState(
                        true);

                    timeInRecorded =
                        true;
                }
                else
                {
                    dtpTimeIn.SetTime(
                        DateTime.Now);

                    dtpTimeIn.SetCheckedState(
                        false);

                    timeInRecorded =
                        false;
                }

                // ----------------------------------------------------
                // TIME OUT
                // ----------------------------------------------------

                string timeOut =
                    row["TimeOut"]?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(timeOut) &&
                    TimeSpan.TryParse(
                        timeOut,
                        out TimeSpan timeOutValue))
                {
                    dtpTimeOut.SetTime(
                        DateTime.Today.Add(
                            timeOutValue));

                    dtpTimeOut.SetCheckedState(
                        true);

                    timeOutRecorded =
                        true;
                }
                else
                {
                    dtpTimeOut.SetTime(
                        DateTime.Now);

                    dtpTimeOut.SetCheckedState(
                        false);

                    timeOutRecorded =
                        false;
                }

                // ----------------------------------------------------
                // STATUS
                // ----------------------------------------------------

                string status =
                    row["Status"]?.ToString() ?? "";

                int statusIndex =
                    cmbStatus.Items.IndexOf(
                        status);

                if (statusIndex >= 0)
                {
                    cmbStatus.SelectedIndex =
                        statusIndex;
                }
                else
                {
                    cmbStatus.SelectedIndex =
                        -1;
                }
            }
            catch
            {
                selectedAttendanceID =
                    0;

                timeInRecorded =
                    false;

                timeOutRecorded =
                    false;

                SetAddMode();
            }
        }

        // ============================================================
        // SELECT EMPLOYEE
        // ============================================================

        private void SelectEmployee(
            long employeeID)
        {
            for (int i = 0;
                 i < cmbEmployee.Items.Count;
                 i++)
            {
                if (cmbEmployee.Items[i]
                    is DataRowView rowView)
                {
                    long id =
                        Convert.ToInt64(
                            rowView["EmployeeID"]);

                    if (id == employeeID)
                    {
                        cmbEmployee.SelectedIndex =
                            i;

                        return;
                    }
                }
            }

            cmbEmployee.SelectedIndex =
                -1;
        }
    }
}