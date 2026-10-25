using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public partial class PayrollControl : UserControl
    {
        private DataTable payrollTable = new DataTable();

        private long selectedPayrollID = 0;

        private bool isClearingFields = false;
        private bool isLoadingPayroll = false;

        private Button? btnPayEmployee;
        private Button? btnViewReceipt;
        private bool selectedPayrollIsPaid = false;
        private string selectedReceiptNumber = "";

        // Tracks which layout is active because Payroll Entry and Payroll Records
        // share the same grid but need different vertical space.
        private bool isPayrollRecordsMode = false;

        public PayrollControl()
        {
            InitializeComponent();

            // Use concise labels for the payroll attendance totals.
            lblDaysPresent.Text = "Present";
            lblDaysAbsent.Text = "Absent";

            Resize += PayrollControl_Resize;
            btnSearch.Paint += btnSearch_Paint;

            ConfigurePayroll();

            LoadEmployees();
            LoadPayrollRecords();

            ClearPayrollFields();
            ApplyPayrollLayout();
        }

        private void ConfigurePayroll()
        {
            PayrollDatabaseHelper.EnsurePayrollTable();

            // --------------------------------------------------------
            // FIELD SETTINGS
            // --------------------------------------------------------

            txtBasicSalary.ReadOnly = true;
            txtDaysPresent.ReadOnly = true;
            txtDaysAbsent.ReadOnly = true;
            txtGrossPay.ReadOnly = true;
            txtNetPay.ReadOnly = true;

            txtOvertimeHours.Text = "0.00";

            txtSssContribution.ReadOnly = true;
            txtPhilHealthContribution.ReadOnly = true;
            txtPagIbigContribution.ReadOnly = true;
            txtWithholdingTax.ReadOnly = true;
            txtDeductions.ReadOnly = true;

            cmbPayPeriod.SelectedIndex =
                cmbPayPeriod.Items.Count > 0 ? 0 : -1;

            // --------------------------------------------------------
            // GRID
            // --------------------------------------------------------

            dgvPayroll.AutoGenerateColumns = false;
            dgvPayroll.ReadOnly = true;
            dgvPayroll.AllowUserToAddRows = false;
            dgvPayroll.AllowUserToDeleteRows = false;
            dgvPayroll.AllowUserToResizeRows = false;
            dgvPayroll.MultiSelect = false;
            dgvPayroll.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvPayroll.RowHeadersVisible = false;
            dgvPayroll.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayroll.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvPayroll.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            // Apply the visual theme and improve the column layout.
            ConfigurePayrollAppearance();

            // Layout is calculated from the actual UserControl size by
            // ApplyPayrollLayout(), so it remains consistent after docking/resizing.

            // --------------------------------------------------------
            // EVENTS
            // --------------------------------------------------------

            btnUpdatePayroll.Click +=
                btnUpdatePayroll_Click;

            btnDeletePayroll.Click +=
                btnDeletePayroll_Click;

            btnClearPayroll.Click +=
                btnClearPayroll_Click;

            btnSearch.Click +=
                btnSearch_Click;

            cmbEmployee.SelectedIndexChanged +=
                cmbEmployee_SelectedIndexChanged;

            cmbPayPeriod.SelectedIndexChanged +=
                cmbPayPeriod_SelectedIndexChanged;

            dgvPayroll.SelectionChanged +=
                dgvPayroll_SelectionChanged;

            txtSearch.KeyDown +=
                txtSearch_KeyDown;

            txtOvertimeHours.TextChanged +=
                PayrollInput_TextChanged;

            VisibleChanged +=
                PayrollControl_VisibleChanged;

            // Listen for live changes made by other modules.
            DataRefreshManager.DataChanged +=
                DataRefreshManager_DataChanged;

            Disposed +=
                PayrollControl_Disposed;

            ConfigurePaymentButtons();

            SetAddMode();
        }

        // ============================================================
        // PAYROLL VISUAL STYLE
        // ============================================================

        private void ConfigurePayrollAppearance()
        {
            Color pageBackground = Color.FromArgb(245, 247, 250);
            Color navy = Color.FromArgb(30, 41, 59);
            Color darkText = Color.FromArgb(31, 41, 55);
            Color mutedText = Color.FromArgb(100, 116, 139);
            Color border = Color.FromArgb(203, 213, 225);
            Color lightField = Color.FromArgb(241, 245, 249);

            BackColor = pageBackground;

            lblPayrollTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblPayrollTitle.ForeColor = navy;
            lblPayrollSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblPayrollSubtitle.ForeColor = mutedText;

            pnlPayrollEntry.BackColor = Color.White;
            pnlPayrollEntry.BorderStyle = BorderStyle.FixedSingle;
            pnlSearch.BackColor = Color.White;
            pnlSearch.BorderStyle = BorderStyle.FixedSingle;

            Label[] fieldLabels =
            {
                lblEmployee,
                lblPayPeriod,
                lblBasicSalary,
                lblDaysPresent,
                lblDaysAbsent,
                lblOvertimeHours,
                lblSssContribution,
                lblPhilHealthContribution,
                lblPagIbigContribution,
                lblWithholdingTax,
                lblDeductions,
                lblGrossPay,
                lblNetPay,
                lblSearch
            };

            foreach (Label label in fieldLabels)
            {
                label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                label.ForeColor = darkText;
            }

            TextBox[] fields =
            {
                txtBasicSalary,
                txtDaysPresent,
                txtDaysAbsent,
                txtOvertimeHours,
                txtSssContribution,
                txtPhilHealthContribution,
                txtPagIbigContribution,
                txtWithholdingTax,
                txtDeductions,
                txtGrossPay,
                txtNetPay,
                txtSearch
            };

            foreach (TextBox field in fields)
            {
                field.BorderStyle = BorderStyle.FixedSingle;
                field.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                field.ForeColor = darkText;
                field.BackColor = field.ReadOnly ? lightField : Color.White;
            }

            // Emphasize the final calculated amounts without changing
            // any payroll calculation or stored values.
            txtGrossPay.BackColor = Color.FromArgb(239, 246, 255);
            txtGrossPay.ForeColor = Color.FromArgb(30, 64, 175);

            txtDeductions.BackColor = Color.FromArgb(255, 247, 237);
            txtDeductions.ForeColor = Color.FromArgb(154, 52, 18);

            txtNetPay.BackColor = Color.FromArgb(220, 252, 231);
            txtNetPay.ForeColor = Color.FromArgb(22, 101, 52);
            txtNetPay.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblNetPay.ForeColor = Color.FromArgb(22, 101, 52);

            // Standard combo-box style keeps the dropdown arrow and
            // field boundary visible on the white form background.
            ComboBox[] combos = { cmbEmployee, cmbPayPeriod };
            foreach (ComboBox combo in combos)
            {
                combo.FlatStyle = FlatStyle.Standard;
                combo.BackColor = Color.White;
                combo.ForeColor = darkText;
                combo.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            }

            // Data grid: dark header, subtle zebra rows, larger row
            // height, and a clear selection color.
            dgvPayroll.EnableHeadersVisualStyles = false;
            dgvPayroll.BackgroundColor = Color.White;
            dgvPayroll.BorderStyle = BorderStyle.FixedSingle;
            dgvPayroll.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPayroll.GridColor = border;
            dgvPayroll.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvPayroll.ColumnHeadersHeight = 40;
            dgvPayroll.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPayroll.ColumnHeadersDefaultCellStyle.BackColor = navy;
            dgvPayroll.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPayroll.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPayroll.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvPayroll.ColumnHeadersDefaultCellStyle.SelectionBackColor = navy;
            dgvPayroll.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            dgvPayroll.DefaultCellStyle.BackColor = Color.White;
            dgvPayroll.DefaultCellStyle.ForeColor = darkText;
            dgvPayroll.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgvPayroll.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPayroll.DefaultCellStyle.Padding = new Padding(3, 2, 3, 2);
            dgvPayroll.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 250, 252);
            dgvPayroll.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                Color.FromArgb(37, 99, 235);
            dgvPayroll.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgvPayroll.RowTemplate.Height = 32;

            // Allocate space by what each column needs rather than giving
            // every column the same width. Receipt numbers and employee
            // names receive extra space to reduce truncation.
            // Keep identifiers, pay periods, names, and receipt numbers wide
            // enough to read. Compact numeric columns use less space.
            SetPayrollColumnFillWeight("colPayrollID", 65F);
            SetPayrollColumnFillWeight("colEmployeeID", 78F);
            SetPayrollColumnFillWeight("colEmployeeName", 140F);
            SetPayrollColumnFillWeight("colPayPeriod", 120F);
            SetPayrollColumnFillWeight("colBasicSalary", 80F);
            SetPayrollColumnFillWeight("colDaysPresent", 58F);
            SetPayrollColumnFillWeight("colDaysAbsent", 58F);
            SetPayrollColumnFillWeight("colOvertimeHours", 72F);
            SetPayrollColumnFillWeight("colGrossPay", 78F);
            SetPayrollColumnFillWeight("colDeductions", 78F);
            SetPayrollColumnFillWeight("colNetPay", 78F);
            SetPayrollColumnFillWeight("colPaymentStatus", 90F);
            SetPayrollColumnFillWeight("colReceiptNumber", 145F);

            SetPayrollColumnAlignment("colEmployeeName", DataGridViewContentAlignment.MiddleLeft);
            SetPayrollColumnAlignment("colPayPeriod", DataGridViewContentAlignment.MiddleLeft);
            SetPayrollColumnAlignment("colReceiptNumber", DataGridViewContentAlignment.MiddleLeft);
            SetPayrollColumnAlignment("colBasicSalary", DataGridViewContentAlignment.MiddleRight);
            SetPayrollColumnAlignment("colGrossPay", DataGridViewContentAlignment.MiddleRight);
            SetPayrollColumnAlignment("colDeductions", DataGridViewContentAlignment.MiddleRight);
            SetPayrollColumnAlignment("colNetPay", DataGridViewContentAlignment.MiddleRight);

            btnSearch.Text = string.Empty;
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
            StyleActionButton(
                btnUpdatePayroll,
                Color.FromArgb(37, 99, 235),
                Color.FromArgb(226, 232, 240),
                Color.White,
                Color.FromArgb(100, 116, 139));
            StyleActionButton(
                btnDeletePayroll,
                Color.FromArgb(220, 38, 38),
                Color.FromArgb(254, 226, 226),
                Color.White,
                Color.FromArgb(153, 107, 107));
            StyleActionButton(
                btnClearPayroll,
                Color.FromArgb(71, 85, 105),
                Color.FromArgb(71, 85, 105),
                Color.White,
                Color.White);

            // Keep all payroll record cells centered and make headers sortable.
            DataGridViewSortingHelper.Configure(dgvPayroll);
            FuturisticUiTheme.Apply(this);
        }

        private void SetPayrollColumnFillWeight(string columnName, float weight)
        {
            if (dgvPayroll.Columns.Contains(columnName))
            {
                dgvPayroll.Columns[columnName]!.FillWeight = weight;
            }
        }

        private void SetPayrollColumnAlignment(
            string columnName,
            DataGridViewContentAlignment alignment)
        {
            if (dgvPayroll.Columns.Contains(columnName))
            {
                dgvPayroll.Columns[columnName]!.DefaultCellStyle.Alignment = alignment;
            }
        }

        private static void StyleActionButton(
            Button? button,
            Color enabledBackColor,
            Color disabledBackColor,
            Color enabledForeColor,
            Color disabledForeColor)
        {
            if (button == null)
                return;

            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = button.Enabled ? enabledBackColor : disabledBackColor;
            button.ForeColor = button.Enabled ? enabledForeColor : disabledForeColor;
            button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            button.Cursor = button.Enabled ? Cursors.Hand : Cursors.Default;
            button.Height = Math.Max(button.Height, 34);
        }

        private void ApplyPayrollButtonStyles()
        {
            StyleActionButton(
                btnPayEmployee,
                Color.FromArgb(22, 163, 74),
                Color.FromArgb(220, 252, 231),
                Color.White,
                Color.FromArgb(74, 124, 91));
            StyleActionButton(
                btnViewReceipt,
                Color.FromArgb(13, 148, 136),
                Color.FromArgb(204, 251, 241),
                Color.White,
                Color.FromArgb(82, 130, 125));
            StyleActionButton(
                btnUpdatePayroll,
                Color.FromArgb(37, 99, 235),
                Color.FromArgb(226, 232, 240),
                Color.White,
                Color.FromArgb(100, 116, 139));
            StyleActionButton(
                btnDeletePayroll,
                Color.FromArgb(220, 38, 38),
                Color.FromArgb(254, 226, 226),
                Color.White,
                Color.FromArgb(153, 107, 107));
            StyleActionButton(
                btnClearPayroll,
                Color.FromArgb(71, 85, 105),
                Color.FromArgb(71, 85, 105),
                Color.White,
                Color.White);
            // btnSearch is intentionally styled separately in ApplyPayrollLayout
            // so its gray icon-only appearance is not overwritten by the blue
            // action-button theme.
        }

        // ============================================================
        // PAYROLL / PAYROLL RECORDS VIEWS
        // ============================================================

        public void ShowPayrollEntryMode()
        {
            if (IsDisposed)
                return;

            isPayrollRecordsMode = false;
            lblPayrollTitle.Text = "PAYROLL MANAGEMENT";
            lblPayrollSubtitle.Text = "Calculate and pay employee payroll";
            lblSearch.Text = "Search Employee:";

            pnlPayrollEntry.Visible = true;
            pnlSearch.Visible = true;
            dgvPayroll.Visible = true;

            ApplyPayrollLayout();
            dgvPayroll.BringToFront();
            SetAddMode();
        }

        public void ShowPayrollRecordsMode()
        {
            if (IsDisposed)
                return;

            // Records mode has no entry panel, so move the search row up
            // under the page heading and let the table occupy the rest of
            // the available height instead of leaving a blank form-sized gap.
            isPayrollRecordsMode = true;
            lblPayrollTitle.Text = "PAYROLL RECORDS";
            lblPayrollSubtitle.Text = "Review payroll history, payment status, and receipts";
            lblSearch.Text = "Search Records:";

            pnlPayrollEntry.Visible = false;
            pnlSearch.Visible = true;
            dgvPayroll.Visible = true;

            ApplyPayrollLayout();
            dgvPayroll.BringToFront();
            LoadPayrollRecords(txtSearch.Text.Trim());
        }

        // ============================================================
        // RESPONSIVE PAYROLL LAYOUT
        // ============================================================

        private void PayrollControl_Resize(object? sender, EventArgs e)
        {
            if (!IsDisposed && IsHandleCreated)
            {
                ApplyPayrollLayout();
            }
        }

        private void ApplyPayrollLayout()
        {
            if (IsDisposed || ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            const int pageMargin = 12;
            int availableWidth = Math.Max(200, ClientSize.Width - pageMargin * 2);

            lblPayrollTitle.Location = new Point(pageMargin, 8);
            lblPayrollSubtitle.Location = new Point(pageMargin + 1, 38);

            pnlPayrollEntry.SetBounds(pageMargin, 68, availableWidth, 210);
            pnlPayrollEntry.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlPayrollEntry.BorderStyle = BorderStyle.FixedSingle;
            pnlPayrollEntry.BackColor = Color.White;

            // Reserve a fixed, right-aligned action area first. Scale the fields
            // into the remaining space so controls never run underneath buttons.
            const int actionButtonWidth = 74;
            const int longActionButtonWidth = 74;
            const int actionButtonHeight = 36;
            const int actionGap = 12;
            const int actionRightMargin = 18;
            const int actionGroupWidth = longActionButtonWidth + actionGap + actionButtonWidth;
            int actionStart = Math.Max(0, availableWidth - actionGroupWidth - actionRightMargin);
            int fieldAreaWidth = Math.Max(620, actionStart - 28);
            float scale = Math.Min(1.0f, fieldAreaWidth / 955.0f);

            int S(int value) => Math.Max(1, (int)Math.Round(value * scale));

            const int inputHeight = 25;
            const int topLabelY = 22;
            const int topInputY = 42;
            const int lowerLabelY = 122;
            const int lowerInputY = 142;

            // Top row: six fields in the same order and relative proportions as
            // the supplied reference, with the overtime field ending before
            // the action-button area.
            PlaceField(lblEmployee, cmbEmployee, S(20), topLabelY, topInputY, S(233), inputHeight);
            PlaceField(lblPayPeriod, cmbPayPeriod, S(276), topLabelY, topInputY, S(140), inputHeight);
            PlaceField(lblBasicSalary, txtBasicSalary, S(445), topLabelY, topInputY, S(140), inputHeight);
            PlaceField(lblDaysPresent, txtDaysPresent, S(614), topLabelY, topInputY, S(83), inputHeight);
            PlaceField(lblDaysAbsent, txtDaysAbsent, S(727), topLabelY, topInputY, S(83), inputHeight);
            PlaceField(lblOvertimeHours, txtOvertimeHours, S(840), topLabelY, topInputY, S(83), inputHeight);

            GroupBox? deductionsGroup = pnlPayrollEntry.Controls
                .OfType<GroupBox>()
                .FirstOrDefault(g => g.Name == "grpPayrollDeductionsLayout");

            if (deductionsGroup == null)
            {
                deductionsGroup = new GroupBox
                {
                    Name = "grpPayrollDeductionsLayout",
                    Text = "",
                    TabStop = false,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(35, 48, 65),
                    BackColor = Color.White
                };
                pnlPayrollEntry.Controls.Add(deductionsGroup);
            }

            deductionsGroup.SetBounds(S(20), 95, S(571), 96);
            deductionsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            deductionsGroup.SendToBack();

            Label? deductionsTitle = pnlPayrollEntry.Controls
                .OfType<Label>()
                .FirstOrDefault(label => label.Name == "lblDeductionsGroupTitle");

            if (deductionsTitle == null)
            {
                deductionsTitle = new Label
                {
                    Name = "lblDeductionsGroupTitle",
                    Text = "Deductions",
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(35, 48, 65),
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular)
                };
                pnlPayrollEntry.Controls.Add(deductionsTitle);
            }

            deductionsTitle.SetBounds(
                deductionsGroup.Left + (deductionsGroup.Width - S(100)) / 2,
                deductionsGroup.Top - 1,
                S(100),
                18);
            deductionsTitle.BringToFront();

            // Center only the five deduction fields within the existing Deductions border.
            const int deductionLabelY = 124;
            const int deductionInputY = 144;
            PlaceField(lblSssContribution, txtSssContribution, S(45), deductionLabelY, deductionInputY, S(83), inputHeight);
            PlaceField(lblPhilHealthContribution, txtPhilHealthContribution, S(148), deductionLabelY, deductionInputY, S(83), inputHeight);
            PlaceField(lblPagIbigContribution, txtPagIbigContribution, S(251), deductionLabelY, deductionInputY, S(83), inputHeight);
            PlaceField(lblWithholdingTax, txtWithholdingTax, S(354), deductionLabelY, deductionInputY, S(83), inputHeight);
            PlaceField(lblDeductions, txtDeductions, S(457), deductionLabelY, deductionInputY, S(100), inputHeight);

            // Totals sit after the deduction group, before the action column.
            PlaceField(lblGrossPay, txtGrossPay, S(620), lowerLabelY, lowerInputY, S(130), inputHeight);
            PlaceField(lblNetPay, txtNetPay, S(798), lowerLabelY, lowerInputY, S(130), inputHeight);

            int actionCol2 = actionStart + actionButtonWidth + actionGap;
            int actionCenter = actionStart + (actionGroupWidth - actionButtonWidth) / 2;

            if (btnPayEmployee != null)
            {
                btnPayEmployee.SetBounds(actionStart, topInputY, longActionButtonWidth, actionButtonHeight);
                btnPayEmployee.Text = "PAY";
                btnPayEmployee.TextAlign = ContentAlignment.MiddleCenter;
                btnPayEmployee.AutoSize = false;
                btnPayEmployee.AutoEllipsis = false;
                btnPayEmployee.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            }

            btnUpdatePayroll.SetBounds(actionCol2, topInputY, actionButtonWidth, actionButtonHeight);

            if (btnViewReceipt != null)
            {
                btnViewReceipt.SetBounds(actionStart + (longActionButtonWidth + actionButtonWidth - longActionButtonWidth) / 2, topInputY + actionButtonHeight + 10, longActionButtonWidth, actionButtonHeight);
                btnViewReceipt.Text = "VIEW";
                btnViewReceipt.TextAlign = ContentAlignment.MiddleCenter;
                btnViewReceipt.AutoSize = false;
                btnViewReceipt.AutoEllipsis = false;
                btnViewReceipt.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            }

            int thirdButtonRowY = topInputY + 2 * (actionButtonHeight + 10);
            btnDeletePayroll.SetBounds(actionStart, thirdButtonRowY, actionButtonWidth, actionButtonHeight);
            btnClearPayroll.SetBounds(actionCol2, thirdButtonRowY, actionButtonWidth, actionButtonHeight);

            int searchTop = isPayrollRecordsMode
                ? lblPayrollSubtitle.Bottom + 12
                : pnlPayrollEntry.Bottom + 8;

            pnlSearch.SetBounds(pageMargin, searchTop, availableWidth, 40);
            pnlSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSearch.BorderStyle = BorderStyle.None;
            pnlSearch.BackColor = BackColor;

            lblSearch.Location = new Point(0, 12);
            lblSearch.AutoSize = true;
            lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            const int searchButtonWidth = 36;
            const int sorterWidth = 80;
            const int controlGap = 8;
            const int labelToTextGap = 12;
            int sorterLeft = availableWidth - sorterWidth;
            int searchTextLeft = lblSearch.Right + labelToTextGap;
            int searchTextAreaWidth = Math.Max(
                120,
                sorterLeft - searchTextLeft - searchButtonWidth - controlGap * 2);

            // Keep the textbox shorter and place the search button immediately
            // after it. The sorter remains anchored at the far right.
            int searchTextWidth = Math.Max(120, (int)Math.Round(searchTextAreaWidth * 0.68));
            int searchButtonLeft = searchTextLeft + searchTextWidth + 3;

            txtSearch.SetBounds(
                searchTextLeft,
                6,
                searchTextWidth,
                24);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Keep the search icon close to the textbox while the sorter stays on the right.
            btnSearch.SetBounds(searchButtonLeft, 6, searchButtonWidth, 24);
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnSearch.Text = string.Empty;
            btnSearch.TextAlign = ContentAlignment.MiddleCenter;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.BackColor = Color.FromArgb(226, 232, 240);
            btnSearch.ForeColor = Color.FromArgb(30, 58, 95);
            btnSearch.FlatAppearance.BorderSize = 1;
            btnSearch.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(203, 213, 225);
            btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(186, 199, 216);
            btnSearch.AccessibleName = "Search payroll records";

            int gridTop = pnlSearch.Bottom + 4;
            const int gridBottomMargin = 16;
            dgvPayroll.SetBounds(
                pageMargin,
                gridTop,
                availableWidth,
                Math.Max(180, ClientSize.Height - gridTop - gridBottomMargin));
            dgvPayroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPayroll.ColumnHeadersHeight = 50;
            dgvPayroll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPayroll.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvPayroll.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            Button? sortButton = Controls.Find("btnSortBy_" + dgvPayroll.Name, true)
                .OfType<Button>()
                .FirstOrDefault();

            if (sortButton != null && !sortButton.IsDisposed)
            {
                sortButton.SetBounds(
                    pnlSearch.Right - sorterWidth,
                    pnlSearch.Top + (pnlSearch.Height - 26) / 2,
                    sorterWidth,
                    26);
                sortButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sortButton.AutoSize = false;
                sortButton.Text = "SORT BY ▼";
                sortButton.TextAlign = ContentAlignment.MiddleCenter;
                sortButton.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                sortButton.FlatStyle = FlatStyle.Flat;
                sortButton.UseVisualStyleBackColor = false;
                sortButton.BackColor = Color.FromArgb(37, 99, 235);
                sortButton.ForeColor = Color.White;
                sortButton.FlatAppearance.BorderSize = 0;
                sortButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
                sortButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 64, 175);
                sortButton.Cursor = Cursors.Hand;
                sortButton.BringToFront();
            }
        }

        private static void PlaceField(
            Label label,
            Control input,
            int x,
            int labelY,
            int inputY,
            int width,
            int height)
        {
            label.Location = new Point(x, labelY);
            label.AutoSize = false;
            label.Width = width;
            label.Height = 18;
            label.TextAlign = ContentAlignment.MiddleLeft;
            input.SetBounds(x, inputY, width, height);
        }

        // ============================================================
        // BUTTON MODES
        // ============================================================

        private void SetAddMode()
        {
            selectedPayrollID = 0;
            selectedPayrollIsPaid = false;
            selectedReceiptNumber = "";

            btnUpdatePayroll.Enabled = false;
            btnDeletePayroll.Enabled = false;
            btnClearPayroll.Enabled = true;

            UpdatePaymentButtons();
        }

        private void SetEditMode()
        {
            btnUpdatePayroll.Enabled = !selectedPayrollIsPaid;
            btnDeletePayroll.Enabled = true;
            btnClearPayroll.Enabled = true;

            UpdatePaymentButtons();
        }

        private void ConfigurePaymentButtons()
        {
            btnPayEmployee = new Button();
            btnViewReceipt = new Button();

            btnPayEmployee.Text = "PAY EMPLOYEE";
            btnViewReceipt.Text = "VIEW RECEIPT";

            btnPayEmployee.Size = new System.Drawing.Size(100, 32);
            btnViewReceipt.Size = new System.Drawing.Size(100, 32);

            btnPayEmployee.Location = new System.Drawing.Point(770, 75);
            btnViewReceipt.Location = new System.Drawing.Point(770, 124);
            btnPayEmployee.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnViewReceipt.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            btnPayEmployee.TabIndex = 30;
            btnViewReceipt.TabIndex = 31;

            btnPayEmployee.Click += btnPayEmployee_Click;
            btnViewReceipt.Click += btnViewReceipt_Click;

            pnlPayrollEntry.Controls.Add(btnPayEmployee);
            pnlPayrollEntry.Controls.Add(btnViewReceipt);

            // ApplyPayrollLayout positions the complete action group from
            // the actual available width to prevent overlap or clipping.
            btnUpdatePayroll.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnDeletePayroll.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnClearPayroll.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            ApplyPayrollButtonStyles();
        }

        private void UpdatePaymentButtons()
        {
            if (btnPayEmployee == null || btnViewReceipt == null)
                return;

            // PAY EMPLOYEE serves as the Save + Pay action.
            // In Add Mode, an employee only needs to be selected.
            // The payroll record is created automatically when payment is confirmed.
            btnPayEmployee.Enabled =
                cmbEmployee.SelectedIndex >= 0 && !selectedPayrollIsPaid;

            btnViewReceipt.Enabled =
                selectedPayrollID > 0 &&
                selectedPayrollIsPaid &&
                !string.IsNullOrWhiteSpace(selectedReceiptNumber);

            ApplyPayrollButtonStyles();
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
                    PayrollDatabaseHelper.GetPayrollEmployees();

                cmbEmployee.DataSource = null;
                cmbEmployee.DisplayMember = "EmployeeName";
                cmbEmployee.ValueMember = "EmployeeID";
                cmbEmployee.DataSource = employees;
                cmbEmployee.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load payroll employees.\n\n" +
                    ex.Message,
                    "Payroll Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LOAD PAYROLL RECORDS
        // ============================================================

        private void LoadPayrollRecords(
            string searchText = "",
            bool showNoResultsMessage = false)
        {
            try
            {
                isLoadingPayroll = true;

                payrollTable =
                    PayrollDatabaseHelper.GetPayrollRecords(
                        searchText);

                dgvPayroll.DataSource = null;
                dgvPayroll.DataSource = payrollTable;

                try
                {
                    dgvPayroll.CurrentCell = null;
                }
                catch
                {
                    // Ignore when the grid is not ready.
                }

                dgvPayroll.ClearSelection();

                if (showNoResultsMessage &&
                    payrollTable.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No payroll records were found.",
                        "Payroll Search",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load payroll records.\n\n" +
                    ex.Message,
                    "Payroll Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingPayroll = false;
            }

            SetAddMode();
        }

        // ============================================================
        // VISIBLE CHANGED
        // ============================================================

        private void PayrollControl_VisibleChanged(
            object? sender,
            EventArgs e)
        {
            if (!Visible || IsDisposed)
                return;

            // Always refresh the employee list when Payroll becomes
            // visible. Employees can be added from the Employees screen
            // while this control is already loaded in the Dashboard.
            LoadEmployees();

            if (IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        ClearPayrollFields();
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
            // payroll operation.
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

                // Employee names, salaries, and deleted employees can
                // affect the payroll list.
                LoadPayrollRecords(
                    txtSearch.Text.Trim());

                ClearPayrollFields();

                return;
            }

            if (e.ChangeType == DataChangeType.Payroll)
            {
                LoadPayrollRecords(txtSearch.Text.Trim());
                ClearPayrollFields();
                return;
            }

            if (e.ChangeType == DataChangeType.Attendance)
            {
                // Attendance affects the Days Present / Days Absent
                // values used by the payroll calculation.
                BeginInvoke(new Action(() =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        if (cmbEmployee.SelectedIndex >= 0)
                        {
                            LoadAttendanceSummary();
                            AutoCalculatePayroll();
                        }
                    }
                }));
            }
        }

        private void PayrollControl_Disposed(
            object? sender,
            EventArgs e)
        {
            DataRefreshManager.DataChanged -=
                DataRefreshManager_DataChanged;

            Disposed -=
                PayrollControl_Disposed;
        }

        // ============================================================
        // EMPLOYEE SELECTION
        // ============================================================

        private void cmbEmployee_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (isClearingFields)
                return;

            if (cmbEmployee.SelectedIndex < 0)
            {
                txtBasicSalary.Clear();
                txtDaysPresent.Text = "0";
                txtDaysAbsent.Text = "0";
                txtGrossPay.Clear();
                txtNetPay.Clear();
                UpdatePaymentButtons();
                return;
            }

            try
            {
                if (cmbEmployee.SelectedValue == null)
                    return;

                DataRowView? selectedEmployee =
                    cmbEmployee.SelectedItem as DataRowView;

                if (selectedEmployee == null)
                    return;

                decimal salary =
                    Convert.ToDecimal(
                        selectedEmployee["Salary"],
                        CultureInfo.InvariantCulture);

                txtBasicSalary.Text =
                    salary.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                LoadAttendanceSummary();
                AutoCalculatePayroll();
                UpdatePaymentButtons();
            }
            catch
            {
                txtBasicSalary.Clear();
                txtDaysPresent.Text = "0";
                txtDaysAbsent.Text = "0";
            }
        }

        private void cmbPayPeriod_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (isClearingFields)
                return;

            LoadAttendanceSummary();
            AutoCalculatePayroll();
        }

        private void LoadAttendanceSummary()
        {
            if (cmbEmployee.SelectedIndex < 0)
                return;

            try
            {
                long employeeID =
                    Convert.ToInt64(
                        cmbEmployee.SelectedValue);

                GetCurrentPayPeriodDates(
                    out DateTime startDate,
                    out DateTime endDate);

                DataTable summary =
                    PayrollDatabaseHelper.GetAttendanceSummary(
                        employeeID,
                        startDate,
                        endDate);

                if (summary.Rows.Count == 0)
                {
                    txtDaysPresent.Text = "0";
                    txtDaysAbsent.Text = "0";
                    AutoCalculatePayroll();
                    return;
                }

                DataRow row = summary.Rows[0];

                txtDaysPresent.Text =
                    Convert.ToInt32(
                        row["DaysPresent"])
                    .ToString();

                txtDaysAbsent.Text =
                    Convert.ToInt32(
                        row["DaysAbsent"])
                    .ToString();

                AutoCalculatePayroll();
            }
            catch
            {
                txtDaysPresent.Text = "0";
                txtDaysAbsent.Text = "0";
                AutoCalculatePayroll();
            }
        }

        // ============================================================
        // PAY PERIOD DATES
        // ============================================================

        private void GetCurrentPayPeriodDates(
            out DateTime startDate,
            out DateTime endDate)
        {
            DateTime today = DateTime.Today;

            DateTime firstDay =
                new DateTime(
                    today.Year,
                    today.Month,
                    1);

            DateTime lastDay =
                firstDay.AddMonths(1).AddDays(-1);

            string payPeriod =
                cmbPayPeriod.Text.Trim();

            if (payPeriod.Equals(
                    "1st - 15th",
                    StringComparison.OrdinalIgnoreCase))
            {
                startDate = firstDay;
                endDate = new DateTime(
                    today.Year,
                    today.Month,
                    15);
                return;
            }

            if (payPeriod.Equals(
                    "16th - End of Month",
                    StringComparison.OrdinalIgnoreCase))
            {
                startDate = new DateTime(
                    today.Year,
                    today.Month,
                    16);
                endDate = lastDay;
                return;
            }

            startDate = firstDay;
            endDate = lastDay;
        }

        // ============================================================
        // AUTOMATIC PAYROLL CALCULATION
        // ============================================================

        private void PayrollInput_TextChanged(
            object? sender,
            EventArgs e)
        {
            AutoCalculatePayroll();
        }

        private void AutoCalculatePayroll()
        {
            if (isClearingFields || isLoadingPayroll)
                return;

            if (cmbEmployee.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(cmbPayPeriod.Text))
            {
                ClearCalculatedAmounts();
                return;
            }

            TryCalculatePayrollValues(
                out _,
                out _,
                out _,
                out _,
                out _,
                out _,
                out _,
                out _,
                out _,
                out _,
                out _,
                out _);
        }

        private bool TryCalculatePayrollValues(
            out decimal basicSalary,
            out int daysPresent,
            out int daysAbsent,
            out decimal overtimeHours,
            out decimal overtimePay,
            out decimal grossPay,
            out decimal sssContribution,
            out decimal philHealthContribution,
            out decimal pagIbigContribution,
            out decimal withholdingTax,
            out decimal deductions,
            out decimal netPay)
        {
            basicSalary = 0m;
            daysPresent = 0;
            daysAbsent = 0;
            overtimeHours = 0m;
            overtimePay = 0m;
            grossPay = 0m;
            sssContribution = 0m;
            philHealthContribution = 0m;
            pagIbigContribution = 0m;
            withholdingTax = 0m;
            deductions = 0m;
            netPay = 0m;

            if (cmbEmployee.SelectedIndex < 0)
                return false;

            if (cmbEmployee.SelectedItem is not DataRowView selectedEmployee)
                return false;

            basicSalary = Convert.ToDecimal(
                selectedEmployee["Salary"],
                CultureInfo.InvariantCulture);

            if (!int.TryParse(
                    txtDaysPresent.Text.Trim(),
                    out daysPresent) ||
                daysPresent < 0)
            {
                daysPresent = 0;
            }

            if (!int.TryParse(
                    txtDaysAbsent.Text.Trim(),
                    out daysAbsent) ||
                daysAbsent < 0)
            {
                daysAbsent = 0;
            }

            string overtimeText = txtOvertimeHours.Text.Trim();

            if (string.IsNullOrWhiteSpace(overtimeText))
            {
                overtimeHours = 0m;
            }
            else if (!decimal.TryParse(
                         overtimeText,
                         NumberStyles.Number,
                         CultureInfo.CurrentCulture,
                         out overtimeHours) ||
                     overtimeHours < 0)
            {
                return false;
            }

            decimal periodBasicSalary =
                GetPeriodBasicSalary(basicSalary);

            // Philippine monthly-paid employee equivalent rates.
            decimal dailyRate =
                (basicSalary * 12m) / 365m;

            decimal hourlyRate =
                dailyRate / 8m;

            // Ordinary working day overtime: 125%.
            decimal overtimeRate =
                hourlyRate * 1.25m;

            overtimePay =
                overtimeHours * overtimeRate;

            decimal periodFactor =
                cmbPayPeriod.Text.Trim().Equals(
                    "Monthly",
                    StringComparison.OrdinalIgnoreCase)
                    ? 1m
                    : 0.5m;

            decimal monthlySss =
                GetMonthlySssEmployeeContribution(basicSalary);

            decimal monthlyPhilHealth =
                GetMonthlyPhilHealthEmployeeContribution(basicSalary);

            decimal monthlyPagIbig =
                GetMonthlyPagIbigEmployeeContribution(basicSalary);

            sssContribution =
                Math.Round(
                    monthlySss * periodFactor,
                    2,
                    MidpointRounding.AwayFromZero);

            philHealthContribution =
                Math.Round(
                    monthlyPhilHealth * periodFactor,
                    2,
                    MidpointRounding.AwayFromZero);

            pagIbigContribution =
                Math.Round(
                    monthlyPagIbig * periodFactor,
                    2,
                    MidpointRounding.AwayFromZero);

            grossPay =
                periodBasicSalary + overtimePay;

            // BIR treats mandatory employee contributions as non-taxable
            // deductions and overtime as supplementary compensation.
            decimal taxableCompensation =
                grossPay -
                sssContribution -
                philHealthContribution -
                pagIbigContribution;

            if (taxableCompensation < 0m)
                taxableCompensation = 0m;

            withholdingTax =
                CalculateWithholdingTax(
                    taxableCompensation,
                    cmbPayPeriod.Text.Trim());

            deductions =
                sssContribution +
                philHealthContribution +
                pagIbigContribution +
                withholdingTax;

            netPay =
                grossPay - deductions;

            UpdateCalculatedFields(
                sssContribution,
                philHealthContribution,
                pagIbigContribution,
                withholdingTax,
                deductions,
                grossPay,
                netPay);

            return true;
        }

        private bool CalculatePayrollValues(
            bool showValidationMessages,
            out decimal basicSalary,
            out int daysPresent,
            out int daysAbsent,
            out decimal overtimeHours,
            out decimal overtimePay,
            out decimal grossPay,
            out decimal sssContribution,
            out decimal philHealthContribution,
            out decimal pagIbigContribution,
            out decimal withholdingTax,
            out decimal deductions,
            out decimal netPay)
        {
            if (!TryCalculatePayrollValues(
                    out basicSalary,
                    out daysPresent,
                    out daysAbsent,
                    out overtimeHours,
                    out overtimePay,
                    out grossPay,
                    out sssContribution,
                    out philHealthContribution,
                    out pagIbigContribution,
                    out withholdingTax,
                    out deductions,
                    out netPay))
            {
                if (!showValidationMessages)
                    return false;

                if (cmbEmployee.SelectedIndex < 0)
                {
                    MessageBox.Show(
                        "Please select an employee.",
                        "Payroll Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    cmbEmployee.Focus();
                    return false;
                }

                string overtimeText =
                    txtOvertimeHours.Text.Trim();

                if (!string.IsNullOrWhiteSpace(overtimeText) &&
                    (!decimal.TryParse(
                         overtimeText,
                         NumberStyles.Number,
                         CultureInfo.CurrentCulture,
                         out decimal parsedOvertime) ||
                     parsedOvertime < 0))
                {
                    MessageBox.Show(
                        "Please enter a valid overtime hours value.",
                        "Payroll Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtOvertimeHours.Focus();
                    txtOvertimeHours.SelectAll();
                    return false;
                }

                return false;
            }

            return true;
        }

        private void ClearCalculatedAmounts()
        {
            txtGrossPay.Clear();
            txtNetPay.Clear();
            txtSssContribution.Text = "₱0.00";
            txtPhilHealthContribution.Text = "₱0.00";
            txtPagIbigContribution.Text = "₱0.00";
            txtWithholdingTax.Text = "₱0.00";
            txtDeductions.Text = "₱0.00";
        }

        private void UpdateCalculatedFields(
            decimal sssContribution,
            decimal philHealthContribution,
            decimal pagIbigContribution,
            decimal withholdingTax,
            decimal deductions,
            decimal grossPay,
            decimal netPay)
        {
            txtSssContribution.Text =
                sssContribution.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            txtPhilHealthContribution.Text =
                philHealthContribution.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            txtPagIbigContribution.Text =
                pagIbigContribution.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            txtWithholdingTax.Text =
                withholdingTax.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            txtDeductions.Text =
                deductions.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            txtGrossPay.Text =
                grossPay.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);

            txtNetPay.Text =
                netPay.ToString(
                    "₱#,##0.00",
                    CultureInfo.CurrentCulture);
        }

        // ------------------------------------------------------------
        // PHILIPPINE STATUTORY CONTRIBUTIONS
        // ------------------------------------------------------------

        private decimal GetMonthlySssEmployeeContribution(
            decimal monthlyBasicSalary)
        {
            // Current SSS employee contribution is 5% of the applicable
            // MSC, with a maximum MSC of ₱35,000.
            decimal msc =
                Math.Ceiling(
                    Math.Max(monthlyBasicSalary, 5000m) / 500m) * 500m;

            if (msc > 35000m)
                msc = 35000m;

            return Math.Round(
                msc * 0.05m,
                2,
                MidpointRounding.AwayFromZero);
        }

        private decimal GetMonthlyPhilHealthEmployeeContribution(
            decimal monthlyBasicSalary)
        {
            // Published PhilHealth premium rate: 5%, shared equally.
            decimal contributionBase =
                Math.Max(10000m,
                    Math.Min(monthlyBasicSalary, 100000m));

            decimal totalPremium =
                contributionBase * 0.05m;

            return Math.Round(
                totalPremium / 2m,
                2,
                MidpointRounding.AwayFromZero);
        }

        private decimal GetMonthlyPagIbigEmployeeContribution(
            decimal monthlyBasicSalary)
        {
            decimal contributionBase =
                Math.Min(monthlyBasicSalary, 5000m);

            decimal rate =
                contributionBase <= 1500m
                    ? 0.01m
                    : 0.02m;

            return Math.Round(
                contributionBase * rate,
                2,
                MidpointRounding.AwayFromZero);
        }

        private decimal CalculateWithholdingTax(
            decimal taxableCompensation,
            string payPeriod)
        {
            if (payPeriod.Equals(
                    "Monthly",
                    StringComparison.OrdinalIgnoreCase))
            {
                if (taxableCompensation <= 20833m)
                    return 0m;

                if (taxableCompensation <= 33332m)
                    return (taxableCompensation - 20833m) * 0.15m;

                if (taxableCompensation <= 66666m)
                    return 1875m +
                           ((taxableCompensation - 33333m) * 0.20m);

                if (taxableCompensation <= 166666m)
                    return 8541.80m +
                           ((taxableCompensation - 66667m) * 0.25m);

                if (taxableCompensation <= 666666m)
                    return 33541.80m +
                           ((taxableCompensation - 166667m) * 0.30m);

                return 183541.80m +
                       ((taxableCompensation - 666667m) * 0.35m);
            }

            // Semi-monthly table.
            if (taxableCompensation <= 10417m)
                return 0m;

            if (taxableCompensation <= 16666m)
                return (taxableCompensation - 10417m) * 0.15m;

            if (taxableCompensation <= 33332m)
                return 937.50m +
                       ((taxableCompensation - 16667m) * 0.20m);

            if (taxableCompensation <= 83332m)
                return 4270.70m +
                       ((taxableCompensation - 33333m) * 0.25m);

            if (taxableCompensation <= 333332m)
                return 16770.70m +
                       ((taxableCompensation - 83333m) * 0.30m);

            return 91770.70m +
                   ((taxableCompensation - 333333m) * 0.35m);
        }
        // ============================================================
        // PAY EMPLOYEE - AUTO-SAVE + PAYMENT
        // ============================================================

        private void btnPayEmployee_Click(
            object? sender,
            EventArgs e)
        {
            if (cmbEmployee.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select an employee first.",
                    "Pay Employee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbEmployee.Focus();
                return;
            }

            if (selectedPayrollIsPaid)
            {
                MessageBox.Show(
                    "This payroll record has already been paid.",
                    "Pay Employee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                // ----------------------------------------------------
                // EXISTING UNPAID RECORD
                // ----------------------------------------------------
                // An existing unpaid record is already a real payroll
                // record. PAY EMPLOYEE only opens payment confirmation.
                // It is not modified until the user confirms payment.

                if (selectedPayrollID > 0)
                {
                    using PayEmployeeForm existingPaymentForm =
                        new PayEmployeeForm(selectedPayrollID);

                    DialogResult existingResult =
                        existingPaymentForm.ShowDialog(this);

                    if (existingResult == DialogResult.OK)
                    {
                        LoadPayrollRecords(txtSearch.Text.Trim());
                        ClearPayrollFields();
                    }

                    return;
                }

                // ----------------------------------------------------
                // NEW PAYROLL DRAFT
                // ----------------------------------------------------
                // Nothing is written to the database yet.
                // The payroll becomes a real record only after the
                // payment is confirmed.

                if (!CalculatePayrollValues(
                        true,
                        out decimal basicSalary,
                        out int daysPresent,
                        out int daysAbsent,
                        out decimal overtimeHours,
                        out decimal overtimePay,
                        out decimal grossPay,
                        out decimal sssContribution,
                        out decimal philHealthContribution,
                        out decimal pagIbigContribution,
                        out decimal withholdingTax,
                        out decimal deductions,
                        out decimal netPay))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbPayPeriod.Text))
                {
                    MessageBox.Show(
                        "Please select a pay period.",
                        "Payroll Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmbPayPeriod.Focus();
                    return;
                }

                long employeeID =
                    Convert.ToInt64(cmbEmployee.SelectedValue);

                string employeeName =
                    cmbEmployee.Text.Trim();

                string payrollMonth =
                    DateTime.Today.ToString("yyyy-MM");

                string payPeriod =
                    cmbPayPeriod.Text.Trim();

                decimal periodBasicSalary =
                    GetPeriodBasicSalary(basicSalary);

                // ----------------------------------------------------
                // CHECK WHETHER A RECORD ALREADY EXISTS
                // ----------------------------------------------------

                DataTable existingRecords =
                    PayrollDatabaseHelper.GetPayrollRecords();

                foreach (DataRow row in existingRecords.Rows)
                {
                    long rowEmployeeID =
                        Convert.ToInt64(row["EmployeeID"]);

                    string rowMonth =
                        row["PayrollMonth"]?.ToString() ?? "";

                    string rowPayPeriod =
                        row["PayPeriod"]?.ToString() ?? "";

                    if (rowEmployeeID == employeeID &&
                        string.Equals(
                            rowMonth,
                            payrollMonth,
                            StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(
                            rowPayPeriod,
                            payPeriod,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        bool rowIsPaid =
                            row.Table.Columns.Contains("PaymentStatus") &&
                            string.Equals(
                                row["PaymentStatus"]?.ToString(),
                                "PAID",
                                StringComparison.OrdinalIgnoreCase);

                        long existingPayrollID =
                            Convert.ToInt64(row["PayrollID"]);

                        if (rowIsPaid)
                        {
                            MessageBox.Show(
                                "This employee already has a paid payroll record for the selected pay period.",
                                "Duplicate Payroll",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;
                        }

                        // Existing UNPAID record: do not create another
                        // record. Let PAY EMPLOYEE handle the payment.
                        using PayEmployeeForm existingPaymentForm =
                            new PayEmployeeForm(existingPayrollID);

                        DialogResult existingResult =
                            existingPaymentForm.ShowDialog(this);

                        if (existingResult == DialogResult.OK)
                        {
                            LoadPayrollRecords(txtSearch.Text.Trim());
                            ClearPayrollFields();
                        }

                        return;
                    }
                }

                // ----------------------------------------------------
                // OPEN PAYMENT FOR NEW DRAFT
                // ----------------------------------------------------

                PayrollPaymentDraft draft =
                    new PayrollPaymentDraft(
                        employeeID,
                        employeeName,
                        payrollMonth,
                        payPeriod,
                        periodBasicSalary,
                        daysPresent,
                        daysAbsent,
                        overtimeHours,
                        overtimePay,
                        grossPay,
                        sssContribution,
                        philHealthContribution,
                        pagIbigContribution,
                        withholdingTax,
                        deductions,
                        netPay);

                using PayEmployeeForm paymentForm =
                    new PayEmployeeForm(draft);

                DialogResult result =
                    paymentForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    LoadPayrollRecords(txtSearch.Text.Trim());
                    ClearPayrollFields();
                }
                else
                {
                    // Payment was cancelled. No new payroll record was
                    // created, so nothing is added to the grid.
                    ClearPayrollFields();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(
                        "UNIQUE",
                        StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "A payroll record for this employee and pay period already exists. Select the unpaid record and use PAY EMPLOYEE.",
                        "Duplicate Payroll",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(
                    "Unable to prepare the employee payment.\n\n" +
                    ex.Message,
                    "Payroll Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // VIEW RECEIPT
        // ============================================================

        private void btnViewReceipt_Click(
            object? sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedReceiptNumber))
            {
                MessageBox.Show(
                    "This payroll record does not have a receipt yet.",
                    "Receipt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using PayrollReceiptForm receiptForm =
                new PayrollReceiptForm(selectedReceiptNumber);

            receiptForm.ShowDialog(this);
        }

        // ============================================================
        // UPDATE
        // ============================================================

        private void btnUpdatePayroll_Click(
            object? sender,
            EventArgs e)
        {
            if (selectedPayrollID <= 0)
            {
                MessageBox.Show(
                    "Please select an existing unpaid payroll record first.",
                    "Update Payroll",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (selectedPayrollIsPaid)
            {
                MessageBox.Show(
                    "A paid payroll record cannot be edited. Payment has already been completed.",
                    "Update Payroll",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!CalculatePayrollValues(
                    true,
                    out decimal basicSalary,
                    out int daysPresent,
                    out int daysAbsent,
                    out decimal overtimeHours,
                    out _,
                    out decimal grossPay,
                    out decimal sssContribution,
                    out decimal philHealthContribution,
                    out decimal pagIbigContribution,
                    out decimal withholdingTax,
                    out decimal deductions,
                    out decimal netPay))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbPayPeriod.Text))
            {
                MessageBox.Show(
                    "Please select a pay period.",
                    "Payroll Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmbPayPeriod.Focus();
                return;
            }

            try
            {
                long employeeID =
                    Convert.ToInt64(
                        cmbEmployee.SelectedValue);

                string payrollMonth =
                    DateTime.Today.ToString("yyyy-MM");

                string payPeriod =
                    cmbPayPeriod.Text.Trim();

                bool updated =
                    PayrollDatabaseHelper.UpdatePayroll(
                        selectedPayrollID,
                        employeeID,
                        payrollMonth,
                        payPeriod,
                        GetPeriodBasicSalary(basicSalary),
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

                if (!updated)
                {
                    MessageBox.Show(
                        "The payroll record could not be updated.",
                        "Update Payroll",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    "Payroll record updated successfully.",
                    "Payroll",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPayrollRecords(
                    txtSearch.Text.Trim());

                ClearPayrollFields();

                DataRefreshManager.Notify(
                    DataChangeType.Payroll,
                    this);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(
                        "UNIQUE",
                        StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Another payroll record already exists for this employee and pay period in the current month.",
                        "Duplicate Payroll",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show(
                    "Unable to update the payroll record.\n\n" +
                    ex.Message,
                    "Payroll Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private decimal GetPeriodBasicSalary(
            decimal monthlySalary)
        {
            return cmbPayPeriod.Text.Trim().Equals(
                "Monthly",
                StringComparison.OrdinalIgnoreCase)
                ? monthlySalary
                : monthlySalary / 2m;
        }

        // ============================================================
        // DELETE
        // ============================================================

        private void btnDeletePayroll_Click(
            object? sender,
            EventArgs e)
        {
            if (selectedPayrollID <= 0)
            {
                MessageBox.Show(
                    "Please select a payroll record first.",
                    "Delete Payroll",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string employeeName =
                string.IsNullOrWhiteSpace(cmbEmployee.Text)
                    ? "this employee"
                    : cmbEmployee.Text.Trim();

            string payPeriod =
                string.IsNullOrWhiteSpace(cmbPayPeriod.Text)
                    ? "the selected pay period"
                    : cmbPayPeriod.Text.Trim();

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this payroll record?\n\n" +
                "Employee: " + employeeName + "\n" +
                "Pay Period: " + payPeriod,
                "Delete Payroll",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool deleted =
                    PayrollDatabaseHelper.DeletePayroll(
                        selectedPayrollID);

                if (!deleted)
                {
                    MessageBox.Show(
                        "The payroll record could not be found. It may have already been deleted.",
                        "Delete Payroll",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadPayrollRecords(txtSearch.Text.Trim());
                    ClearPayrollFields();
                    return;
                }

                MessageBox.Show(
                    "Payroll record deleted successfully.",
                    "Payroll",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPayrollRecords(
                    txtSearch.Text.Trim());

                ClearPayrollFields();

                // Notify the Dashboard and any other listeners.
                DataRefreshManager.Notify(
                    DataChangeType.Payroll,
                    this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete the payroll record.\n\n" +
                    ex.Message,
                    "Payroll Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // CLEAR
        // ============================================================

        private void btnClearPayroll_Click(
            object? sender,
            EventArgs e)
        {
            ClearPayrollFields();
        }

        private void ClearPayrollFields()
        {
            isClearingFields = true;

            try
            {
                selectedPayrollID = 0;

                cmbEmployee.SelectedIndex = -1;

                cmbPayPeriod.SelectedIndex =
                    cmbPayPeriod.Items.Count > 0 ? 0 : -1;

                txtBasicSalary.Clear();

                txtDaysPresent.Text = "0";
                txtDaysAbsent.Text = "0";

                txtOvertimeHours.Text = "0.00";
                ClearCalculatedAmounts();

                dgvPayroll.ClearSelection();

                try
                {
                    dgvPayroll.CurrentCell = null;
                }
                catch
                {
                    // Ignore when grid is not ready.
                }

                SetAddMode();
            }
            finally
            {
                isClearingFields = false;
            }
        }

        // ============================================================
        private void btnSearch_Paint(object? sender, PaintEventArgs e)
        {
            // Draw the magnifying glass ourselves so it renders consistently
            // instead of depending on a font-specific icon glyph.
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float diameter = 9f;
            float x = (btnSearch.ClientSize.Width - diameter - 4f) / 2f - 1f;
            float y = (btnSearch.ClientSize.Height - diameter - 4f) / 2f - 1f;
            using (Pen pen = new Pen(Color.FromArgb(30, 58, 95), 1.6f))
            {
                g.DrawEllipse(pen, x, y, diameter, diameter);
                g.DrawLine(pen, x + diameter - 1f, y + diameter - 1f,
                    x + diameter + 4f, y + diameter + 4f);
            }
        }

        // SEARCH
        // ============================================================

        private void btnSearch_Click(
            object? sender,
            EventArgs e)
        {
            LoadPayrollRecords(
                txtSearch.Text.Trim(),
                true);

            ClearPayrollFields();
        }

        private void txtSearch_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadPayrollRecords(
                    txtSearch.Text.Trim(),
                    true);

                ClearPayrollFields();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // ============================================================
        // GRID SELECTION
        // ============================================================

        private void dgvPayroll_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            if (DataGridViewSortingHelper.IsSorting(dgvPayroll))
                return;

            if (isLoadingPayroll || isClearingFields)
                return;

            if (dgvPayroll.CurrentRow == null)
            {
                SetAddMode();
                return;
            }

            if (dgvPayroll.CurrentRow.DataBoundItem
                is not DataRowView rowView)
            {
                SetAddMode();
                return;
            }

            DataRow row = rowView.Row;

            try
            {
                selectedPayrollID =
                    Convert.ToInt64(
                        row["PayrollID"]);

                selectedPayrollIsPaid =
                    row.Table.Columns.Contains("PaymentStatus") &&
                    string.Equals(
                        row["PaymentStatus"]?.ToString(),
                        "PAID",
                        StringComparison.OrdinalIgnoreCase);

                selectedReceiptNumber =
                    row.Table.Columns.Contains("ReceiptNumber")
                        ? row["ReceiptNumber"]?.ToString() ?? ""
                        : "";

                SetEditMode();

                long employeeID =
                    Convert.ToInt64(
                        row["EmployeeID"]);

                SelectEmployee(employeeID);

                string payPeriod =
                    row["PayPeriod"]?.ToString() ?? "";

                int periodIndex =
                    cmbPayPeriod.Items.IndexOf(
                        payPeriod);

                cmbPayPeriod.SelectedIndex =
                    periodIndex >= 0 ? periodIndex : -1;

                decimal storedBasicSalary =
                    Convert.ToDecimal(
                        row["BasicSalary"],
                        CultureInfo.InvariantCulture);

                txtBasicSalary.Text =
                    storedBasicSalary.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtDaysPresent.Text =
                    Convert.ToInt32(
                        row["DaysPresent"]).ToString();

                txtDaysAbsent.Text =
                    Convert.ToInt32(
                        row["DaysAbsent"]).ToString();

                txtOvertimeHours.Text =
                    Convert.ToDecimal(
                        row["OvertimeHours"],
                        CultureInfo.InvariantCulture)
                    .ToString(
                        "0.00",
                        CultureInfo.CurrentCulture);

                decimal storedGrossPay =
                    Convert.ToDecimal(
                        row["GrossPay"],
                        CultureInfo.InvariantCulture);

                decimal storedSss =
                    row.Table.Columns.Contains("SSSContribution")
                        ? Convert.ToDecimal(
                            row["SSSContribution"],
                            CultureInfo.InvariantCulture)
                        : 0m;

                decimal storedPhilHealth =
                    row.Table.Columns.Contains("PhilHealthContribution")
                        ? Convert.ToDecimal(
                            row["PhilHealthContribution"],
                            CultureInfo.InvariantCulture)
                        : 0m;

                decimal storedPagIbig =
                    row.Table.Columns.Contains("PagIBIGContribution")
                        ? Convert.ToDecimal(
                            row["PagIBIGContribution"],
                            CultureInfo.InvariantCulture)
                        : 0m;

                decimal storedWithholdingTax =
                    row.Table.Columns.Contains("WithholdingTax")
                        ? Convert.ToDecimal(
                            row["WithholdingTax"],
                            CultureInfo.InvariantCulture)
                        : 0m;

                decimal storedDeductions =
                    Convert.ToDecimal(
                        row["Deductions"],
                        CultureInfo.InvariantCulture);

                decimal storedNetPay =
                    Convert.ToDecimal(
                        row["NetPay"],
                        CultureInfo.InvariantCulture);

                txtGrossPay.Text =
                    storedGrossPay.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtSssContribution.Text =
                    storedSss.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtPhilHealthContribution.Text =
                    storedPhilHealth.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtPagIbigContribution.Text =
                    storedPagIbig.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtWithholdingTax.Text =
                    storedWithholdingTax.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtDeductions.Text =
                    storedDeductions.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);

                txtNetPay.Text =
                    storedNetPay.ToString(
                        "₱#,##0.00",
                        CultureInfo.CurrentCulture);
            }
            catch
            {
                selectedPayrollID = 0;
                SetAddMode();
            }
        }

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
                        cmbEmployee.SelectedIndex = i;
                        return;
                    }
                }
            }

            cmbEmployee.SelectedIndex = -1;
        }
    }
}
