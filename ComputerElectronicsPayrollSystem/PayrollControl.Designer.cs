namespace ComputerElectronicsPayrollSystem
{
    partial class PayrollControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // ============================================================
        // HEADER
        // ============================================================

        private System.Windows.Forms.Label lblPayrollTitle;
        private System.Windows.Forms.Label lblPayrollSubtitle;

        // ============================================================
        // ENTRY PANEL
        // ============================================================

        private System.Windows.Forms.Panel pnlPayrollEntry;

        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cmbEmployee;

        private System.Windows.Forms.Label lblPayPeriod;
        private System.Windows.Forms.ComboBox cmbPayPeriod;

        private System.Windows.Forms.Label lblBasicSalary;
        private System.Windows.Forms.TextBox txtBasicSalary;

        private System.Windows.Forms.Label lblDaysPresent;
        private System.Windows.Forms.TextBox txtDaysPresent;

        private System.Windows.Forms.Label lblDaysAbsent;
        private System.Windows.Forms.TextBox txtDaysAbsent;

        private System.Windows.Forms.Label lblOvertimeHours;
        private System.Windows.Forms.TextBox txtOvertimeHours;

        private System.Windows.Forms.Label lblSssContribution;
        private System.Windows.Forms.TextBox txtSssContribution;

        private System.Windows.Forms.Label lblPhilHealthContribution;
        private System.Windows.Forms.TextBox txtPhilHealthContribution;

        private System.Windows.Forms.Label lblPagIbigContribution;
        private System.Windows.Forms.TextBox txtPagIbigContribution;

        private System.Windows.Forms.Label lblWithholdingTax;
        private System.Windows.Forms.TextBox txtWithholdingTax;

        private System.Windows.Forms.Label lblDeductions;
        private System.Windows.Forms.TextBox txtDeductions;

        private System.Windows.Forms.Label lblGrossPay;
        private System.Windows.Forms.TextBox txtGrossPay;

        private System.Windows.Forms.Label lblNetPay;
        private System.Windows.Forms.TextBox txtNetPay;

        // ============================================================
        // BUTTONS
        // ============================================================

        private System.Windows.Forms.Button btnUpdatePayroll;
        private System.Windows.Forms.Button btnDeletePayroll;
        private System.Windows.Forms.Button btnClearPayroll;

        // ============================================================
        // SEARCH
        // ============================================================

        private System.Windows.Forms.Panel pnlSearch;

        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;

        // ============================================================
        // TABLE
        // ============================================================

        private System.Windows.Forms.DataGridView dgvPayroll;

        private System.Windows.Forms.DataGridViewTextBoxColumn colPayrollID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBasicSalary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysPresent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysAbsent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOvertimeHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrossPay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeductions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetPay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReceiptNumber;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed;
        /// otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            lblPayrollTitle = new Label();
            lblPayrollSubtitle = new Label();
            pnlPayrollEntry = new Panel();
            lblEmployee = new Label();
            cmbEmployee = new ComboBox();
            lblPayPeriod = new Label();
            cmbPayPeriod = new ComboBox();
            lblBasicSalary = new Label();
            txtBasicSalary = new TextBox();
            lblDaysPresent = new Label();
            txtDaysPresent = new TextBox();
            lblDaysAbsent = new Label();
            txtDaysAbsent = new TextBox();
            lblOvertimeHours = new Label();
            txtOvertimeHours = new TextBox();
            lblSssContribution = new Label();
            txtSssContribution = new TextBox();
            lblPhilHealthContribution = new Label();
            txtPhilHealthContribution = new TextBox();
            lblPagIbigContribution = new Label();
            txtPagIbigContribution = new TextBox();
            lblWithholdingTax = new Label();
            txtWithholdingTax = new TextBox();
            lblDeductions = new Label();
            txtDeductions = new TextBox();
            lblGrossPay = new Label();
            txtGrossPay = new TextBox();
            lblNetPay = new Label();
            txtNetPay = new TextBox();
            btnUpdatePayroll = new Button();
            btnDeletePayroll = new Button();
            btnClearPayroll = new Button();
            pnlSearch = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            dgvPayroll = new DataGridView();
            colPayrollID = new DataGridViewTextBoxColumn();
            colEmployeeID = new DataGridViewTextBoxColumn();
            colEmployeeName = new DataGridViewTextBoxColumn();
            colPayPeriod = new DataGridViewTextBoxColumn();
            colBasicSalary = new DataGridViewTextBoxColumn();
            colDaysPresent = new DataGridViewTextBoxColumn();
            colDaysAbsent = new DataGridViewTextBoxColumn();
            colOvertimeHours = new DataGridViewTextBoxColumn();
            colGrossPay = new DataGridViewTextBoxColumn();
            colDeductions = new DataGridViewTextBoxColumn();
            colNetPay = new DataGridViewTextBoxColumn();
            colPaymentStatus = new DataGridViewTextBoxColumn();
            colReceiptNumber = new DataGridViewTextBoxColumn();
            pnlPayrollEntry.SuspendLayout();
            pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPayroll).BeginInit();
            SuspendLayout();
            // 
            // lblPayrollTitle
            // 
            lblPayrollTitle.AutoSize = true;
            lblPayrollTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPayrollTitle.Location = new Point(6, 7);
            lblPayrollTitle.Name = "lblPayrollTitle";
            lblPayrollTitle.Size = new Size(235, 25);
            lblPayrollTitle.TabIndex = 0;
            lblPayrollTitle.Text = "PAYROLL MANAGEMENT";
            // 
            // lblPayrollSubtitle
            // 
            lblPayrollSubtitle.AutoSize = true;
            lblPayrollSubtitle.Font = new Font("Segoe UI", 8.5F);
            lblPayrollSubtitle.Location = new Point(7, 35);
            lblPayrollSubtitle.Name = "lblPayrollSubtitle";
            lblPayrollSubtitle.Size = new Size(195, 15);
            lblPayrollSubtitle.TabIndex = 1;
            lblPayrollSubtitle.Text = "Calculate and pay employee payroll";
            // 
            // pnlPayrollEntry
            // 
            pnlPayrollEntry.BorderStyle = BorderStyle.FixedSingle;
            pnlPayrollEntry.Controls.Add(lblEmployee);
            pnlPayrollEntry.Controls.Add(cmbEmployee);
            pnlPayrollEntry.Controls.Add(lblPayPeriod);
            pnlPayrollEntry.Controls.Add(cmbPayPeriod);
            pnlPayrollEntry.Controls.Add(lblBasicSalary);
            pnlPayrollEntry.Controls.Add(txtBasicSalary);
            pnlPayrollEntry.Controls.Add(lblDaysPresent);
            pnlPayrollEntry.Controls.Add(txtDaysPresent);
            pnlPayrollEntry.Controls.Add(lblDaysAbsent);
            pnlPayrollEntry.Controls.Add(txtDaysAbsent);
            pnlPayrollEntry.Controls.Add(lblOvertimeHours);
            pnlPayrollEntry.Controls.Add(txtOvertimeHours);
            pnlPayrollEntry.Controls.Add(lblSssContribution);
            pnlPayrollEntry.Controls.Add(txtSssContribution);
            pnlPayrollEntry.Controls.Add(lblPhilHealthContribution);
            pnlPayrollEntry.Controls.Add(txtPhilHealthContribution);
            pnlPayrollEntry.Controls.Add(lblPagIbigContribution);
            pnlPayrollEntry.Controls.Add(txtPagIbigContribution);
            pnlPayrollEntry.Controls.Add(lblWithholdingTax);
            pnlPayrollEntry.Controls.Add(txtWithholdingTax);
            pnlPayrollEntry.Controls.Add(lblDeductions);
            pnlPayrollEntry.Controls.Add(txtDeductions);
            pnlPayrollEntry.Controls.Add(lblGrossPay);
            pnlPayrollEntry.Controls.Add(txtGrossPay);
            pnlPayrollEntry.Controls.Add(lblNetPay);
            pnlPayrollEntry.Controls.Add(txtNetPay);
            pnlPayrollEntry.Controls.Add(btnUpdatePayroll);
            pnlPayrollEntry.Controls.Add(btnDeletePayroll);
            pnlPayrollEntry.Controls.Add(btnClearPayroll);
            pnlPayrollEntry.Location = new Point(0, 67);
            pnlPayrollEntry.Name = "pnlPayrollEntry";
            pnlPayrollEntry.Size = new Size(1102, 175);
            pnlPayrollEntry.TabIndex = 2;
            // 
            // lblEmployee
            // 
            lblEmployee.AutoSize = true;
            lblEmployee.Font = new Font("Segoe UI", 8.5F);
            lblEmployee.Location = new Point(20, 12);
            lblEmployee.Name = "lblEmployee";
            lblEmployee.Size = new Size(62, 15);
            lblEmployee.TabIndex = 0;
            lblEmployee.Text = "Employee:";
            // 
            // cmbEmployee
            // 
            cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEmployee.FormattingEnabled = true;
            cmbEmployee.Location = new Point(20, 32);
            cmbEmployee.Name = "cmbEmployee";
            cmbEmployee.Size = new Size(300, 23);
            cmbEmployee.TabIndex = 1;
            // 
            // lblPayPeriod
            // 
            lblPayPeriod.AutoSize = true;
            lblPayPeriod.Location = new Point(345, 12);
            lblPayPeriod.Name = "lblPayPeriod";
            lblPayPeriod.Size = new Size(66, 15);
            lblPayPeriod.TabIndex = 2;
            lblPayPeriod.Text = "Pay Period:";
            // 
            // cmbPayPeriod
            // 
            cmbPayPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayPeriod.FormattingEnabled = true;
            cmbPayPeriod.Items.AddRange(new object[] { "1st - 15th", "16th - End of Month", "Monthly" });
            cmbPayPeriod.Location = new Point(345, 32);
            cmbPayPeriod.Name = "cmbPayPeriod";
            cmbPayPeriod.Size = new Size(210, 23);
            cmbPayPeriod.TabIndex = 3;
            // 
            // lblBasicSalary
            // 
            lblBasicSalary.AutoSize = true;
            lblBasicSalary.Location = new Point(580, 12);
            lblBasicSalary.Name = "lblBasicSalary";
            lblBasicSalary.Size = new Size(71, 15);
            lblBasicSalary.TabIndex = 4;
            lblBasicSalary.Text = "Basic Salary:";
            // 
            // txtBasicSalary
            // 
            txtBasicSalary.Location = new Point(580, 32);
            txtBasicSalary.Name = "txtBasicSalary";
            txtBasicSalary.ReadOnly = true;
            txtBasicSalary.Size = new Size(170, 23);
            txtBasicSalary.TabIndex = 5;
            txtBasicSalary.TextAlign = HorizontalAlignment.Right;
            // 
            // lblDaysPresent
            // 
            lblDaysPresent.AutoSize = true;
            lblDaysPresent.Location = new Point(775, 12);
            lblDaysPresent.Name = "lblDaysPresent";
            lblDaysPresent.Size = new Size(77, 15);
            lblDaysPresent.TabIndex = 6;
            lblDaysPresent.Text = "Days Present:";
            // 
            // txtDaysPresent
            // 
            txtDaysPresent.Location = new Point(775, 32);
            txtDaysPresent.Name = "txtDaysPresent";
            txtDaysPresent.Size = new Size(95, 23);
            txtDaysPresent.TabIndex = 7;
            txtDaysPresent.TextAlign = HorizontalAlignment.Center;
            // 
            // lblDaysAbsent
            // 
            lblDaysAbsent.AutoSize = true;
            lblDaysAbsent.Location = new Point(895, 12);
            lblDaysAbsent.Name = "lblDaysAbsent";
            lblDaysAbsent.Size = new Size(75, 15);
            lblDaysAbsent.TabIndex = 8;
            lblDaysAbsent.Text = "Days Absent:";
            // 
            // txtDaysAbsent
            // 
            txtDaysAbsent.Location = new Point(895, 32);
            txtDaysAbsent.Name = "txtDaysAbsent";
            txtDaysAbsent.Size = new Size(95, 23);
            txtDaysAbsent.TabIndex = 9;
            txtDaysAbsent.TextAlign = HorizontalAlignment.Center;
            // 
            // lblOvertimeHours
            // 
            lblOvertimeHours.AutoSize = true;
            lblOvertimeHours.Location = new Point(20, 75);
            lblOvertimeHours.Name = "lblOvertimeHours";
            lblOvertimeHours.Size = new Size(94, 15);
            lblOvertimeHours.TabIndex = 10;
            lblOvertimeHours.Text = "OT Hours:";
            // 
            // txtOvertimeHours
            // 
            txtOvertimeHours.Location = new Point(20, 95);
            txtOvertimeHours.Name = "txtOvertimeHours";
            txtOvertimeHours.Size = new Size(120, 23);
            txtOvertimeHours.TabIndex = 11;
            txtOvertimeHours.TextAlign = HorizontalAlignment.Center;
            // 
            // lblSssContribution
            // 
            lblSssContribution.AutoSize = true;
            lblSssContribution.Location = new Point(165, 75);
            lblSssContribution.Name = "lblSssContribution";
            lblSssContribution.Size = new Size(91, 15);
            lblSssContribution.TabIndex = 12;
            lblSssContribution.Text = "SSS:";
            // 
            // txtSssContribution
            // 
            txtSssContribution.Location = new Point(165, 95);
            txtSssContribution.Name = "txtSssContribution";
            txtSssContribution.ReadOnly = true;
            txtSssContribution.Size = new Size(125, 23);
            txtSssContribution.TabIndex = 13;
            txtSssContribution.TabStop = false;
            txtSssContribution.TextAlign = HorizontalAlignment.Right;
            // 
            // lblPhilHealthContribution
            // 
            lblPhilHealthContribution.AutoSize = true;
            lblPhilHealthContribution.Location = new Point(305, 75);
            lblPhilHealthContribution.Name = "lblPhilHealthContribution";
            lblPhilHealthContribution.Size = new Size(128, 15);
            lblPhilHealthContribution.TabIndex = 14;
            lblPhilHealthContribution.Text = "PhilHealth:";
            // 
            // txtPhilHealthContribution
            // 
            txtPhilHealthContribution.Location = new Point(305, 95);
            txtPhilHealthContribution.Name = "txtPhilHealthContribution";
            txtPhilHealthContribution.ReadOnly = true;
            txtPhilHealthContribution.Size = new Size(135, 23);
            txtPhilHealthContribution.TabIndex = 15;
            txtPhilHealthContribution.TabStop = false;
            txtPhilHealthContribution.TextAlign = HorizontalAlignment.Right;
            // 
            // lblPagIbigContribution
            // 
            lblPagIbigContribution.AutoSize = true;
            lblPagIbigContribution.Location = new Point(455, 75);
            lblPagIbigContribution.Name = "lblPagIbigContribution";
            lblPagIbigContribution.Size = new Size(119, 15);
            lblPagIbigContribution.TabIndex = 16;
            lblPagIbigContribution.Text = "Pag-IBIG:";
            // 
            // txtPagIbigContribution
            // 
            txtPagIbigContribution.Location = new Point(455, 95);
            txtPagIbigContribution.Name = "txtPagIbigContribution";
            txtPagIbigContribution.ReadOnly = true;
            txtPagIbigContribution.Size = new Size(135, 23);
            txtPagIbigContribution.TabIndex = 17;
            txtPagIbigContribution.TabStop = false;
            txtPagIbigContribution.TextAlign = HorizontalAlignment.Right;
            // 
            // lblWithholdingTax
            // 
            lblWithholdingTax.AutoSize = true;
            lblWithholdingTax.Location = new Point(605, 75);
            lblWithholdingTax.Name = "lblWithholdingTax";
            lblWithholdingTax.Size = new Size(96, 15);
            lblWithholdingTax.TabIndex = 18;
            lblWithholdingTax.Text = "Withholding Tax:";
            // 
            // txtWithholdingTax
            // 
            txtWithholdingTax.Location = new Point(605, 95);
            txtWithholdingTax.Name = "txtWithholdingTax";
            txtWithholdingTax.ReadOnly = true;
            txtWithholdingTax.Size = new Size(145, 23);
            txtWithholdingTax.TabIndex = 19;
            txtWithholdingTax.TabStop = false;
            txtWithholdingTax.TextAlign = HorizontalAlignment.Right;
            // 
            // lblDeductions
            // 
            lblDeductions.AutoSize = true;
            lblDeductions.Location = new Point(20, 124);
            lblDeductions.Name = "lblDeductions";
            lblDeductions.Size = new Size(99, 15);
            lblDeductions.TabIndex = 20;
            lblDeductions.Text = "Total Deductions:";
            // 
            // txtDeductions
            // 
            txtDeductions.Location = new Point(20, 144);
            txtDeductions.Name = "txtDeductions";
            txtDeductions.ReadOnly = true;
            txtDeductions.Size = new Size(125, 23);
            txtDeductions.TabIndex = 21;
            txtDeductions.TabStop = false;
            txtDeductions.TextAlign = HorizontalAlignment.Right;
            // 
            // lblGrossPay
            // 
            lblGrossPay.AutoSize = true;
            lblGrossPay.Location = new Point(165, 124);
            lblGrossPay.Name = "lblGrossPay";
            lblGrossPay.Size = new Size(61, 15);
            lblGrossPay.TabIndex = 22;
            lblGrossPay.Text = "Gross Pay:";
            // 
            // txtGrossPay
            // 
            txtGrossPay.Location = new Point(165, 144);
            txtGrossPay.Name = "txtGrossPay";
            txtGrossPay.ReadOnly = true;
            txtGrossPay.Size = new Size(165, 23);
            txtGrossPay.TabIndex = 23;
            txtGrossPay.TabStop = false;
            txtGrossPay.TextAlign = HorizontalAlignment.Right;
            // 
            // lblNetPay
            // 
            lblNetPay.AutoSize = true;
            lblNetPay.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNetPay.Location = new Point(350, 124);
            lblNetPay.Name = "lblNetPay";
            lblNetPay.Size = new Size(53, 15);
            lblNetPay.TabIndex = 24;
            lblNetPay.Text = "Net Pay:";
            // 
            // txtNetPay
            // 
            txtNetPay.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtNetPay.Location = new Point(350, 144);
            txtNetPay.Name = "txtNetPay";
            txtNetPay.ReadOnly = true;
            txtNetPay.Size = new Size(180, 23);
            txtNetPay.TabIndex = 25;
            txtNetPay.TabStop = false;
            txtNetPay.TextAlign = HorizontalAlignment.Right;
            // 
            // btnUpdatePayroll
            // 
            btnUpdatePayroll.Location = new Point(876, 75);
            btnUpdatePayroll.Name = "btnUpdatePayroll";
            btnUpdatePayroll.Size = new Size(100, 32);
            btnUpdatePayroll.TabIndex = 27;
            btnUpdatePayroll.Text = "UPDATE";
            btnUpdatePayroll.UseVisualStyleBackColor = true;
            // 
            // btnDeletePayroll
            // 
            btnDeletePayroll.Enabled = false;
            btnDeletePayroll.Location = new Point(982, 75);
            btnDeletePayroll.Name = "btnDeletePayroll";
            btnDeletePayroll.Size = new Size(100, 32);
            btnDeletePayroll.TabIndex = 28;
            btnDeletePayroll.Text = "DELETE";
            btnDeletePayroll.UseVisualStyleBackColor = true;
            // 
            // btnClearPayroll
            // 
            btnClearPayroll.Location = new Point(770, 124);
            btnClearPayroll.Name = "btnClearPayroll";
            btnClearPayroll.Size = new Size(100, 32);
            btnClearPayroll.TabIndex = 29;
            btnClearPayroll.Text = "CLEAR";
            btnClearPayroll.UseVisualStyleBackColor = true;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Location = new Point(0, 252);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(1102, 48);
            pnlSearch.TabIndex = 3;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(18, 14);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(100, 15);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Search Employee:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(141, 6);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(300, 23);
            txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(466, 6);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(90, 28);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "SEARCH";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // dgvPayroll
            // 
            dgvPayroll.AllowUserToAddRows = false;
            dgvPayroll.AllowUserToDeleteRows = false;
            dgvPayroll.AllowUserToResizeRows = false;
            dgvPayroll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayroll.BackgroundColor = SystemColors.Window;
            dgvPayroll.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvPayroll.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPayroll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPayroll.Columns.AddRange(new DataGridViewColumn[] { colPayrollID, colEmployeeID, colEmployeeName, colPayPeriod, colBasicSalary, colDaysPresent, colDaysAbsent, colOvertimeHours, colGrossPay, colDeductions, colNetPay, colPaymentStatus, colReceiptNumber });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = SystemColors.Window;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle7.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvPayroll.DefaultCellStyle = dataGridViewCellStyle7;
            dgvPayroll.Location = new Point(0, 305);
            dgvPayroll.MultiSelect = false;
            dgvPayroll.Name = "dgvPayroll";
            dgvPayroll.ReadOnly = true;
            dgvPayroll.RowHeadersVisible = false;
            dgvPayroll.RowTemplate.Height = 28;
            dgvPayroll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayroll.Size = new Size(1102, 355);
            dgvPayroll.TabIndex = 4;
            // 
            // colPayrollID
            // 
            colPayrollID.DataPropertyName = "PayrollID";
            colPayrollID.HeaderText = "Payroll ID";
            colPayrollID.Name = "colPayrollID";
            colPayrollID.ReadOnly = true;
            // 
            // colEmployeeID
            // 
            colEmployeeID.DataPropertyName = "EmployeeID";
            colEmployeeID.HeaderText = "Employee ID";
            colEmployeeID.Name = "colEmployeeID";
            colEmployeeID.ReadOnly = true;
            // 
            // colEmployeeName
            // 
            colEmployeeName.DataPropertyName = "EmployeeName";
            colEmployeeName.HeaderText = "Employee Name";
            colEmployeeName.Name = "colEmployeeName";
            colEmployeeName.ReadOnly = true;
            // 
            // colPayPeriod
            // 
            colPayPeriod.DataPropertyName = "PayPeriod";
            colPayPeriod.HeaderText = "Pay Period";
            colPayPeriod.Name = "colPayPeriod";
            colPayPeriod.ReadOnly = true;
            // 
            // colBasicSalary
            // 
            colBasicSalary.DataPropertyName = "BasicSalary";
            dataGridViewCellStyle2.Format = "₱#,##0.00";
            colBasicSalary.DefaultCellStyle = dataGridViewCellStyle2;
            colBasicSalary.HeaderText = "Basic Salary";
            colBasicSalary.Name = "colBasicSalary";
            colBasicSalary.ReadOnly = true;
            // 
            // colDaysPresent
            // 
            colDaysPresent.DataPropertyName = "DaysPresent";
            colDaysPresent.HeaderText = "Days Present";
            colDaysPresent.Name = "colDaysPresent";
            colDaysPresent.ReadOnly = true;
            // 
            // colDaysAbsent
            // 
            colDaysAbsent.DataPropertyName = "DaysAbsent";
            colDaysAbsent.HeaderText = "Days Absent";
            colDaysAbsent.Name = "colDaysAbsent";
            colDaysAbsent.ReadOnly = true;
            // 
            // colOvertimeHours
            // 
            colOvertimeHours.DataPropertyName = "OvertimeHours";
            dataGridViewCellStyle3.Format = "0.00";
            colOvertimeHours.DefaultCellStyle = dataGridViewCellStyle3;
            colOvertimeHours.HeaderText = "Overtime Hours";
            colOvertimeHours.Name = "colOvertimeHours";
            colOvertimeHours.ReadOnly = true;
            // 
            // colGrossPay
            // 
            colGrossPay.DataPropertyName = "GrossPay";
            dataGridViewCellStyle4.Format = "₱#,##0.00";
            colGrossPay.DefaultCellStyle = dataGridViewCellStyle4;
            colGrossPay.HeaderText = "Gross Pay";
            colGrossPay.Name = "colGrossPay";
            colGrossPay.ReadOnly = true;
            // 
            // colDeductions
            // 
            colDeductions.DataPropertyName = "Deductions";
            dataGridViewCellStyle5.Format = "₱#,##0.00";
            colDeductions.DefaultCellStyle = dataGridViewCellStyle5;
            colDeductions.HeaderText = "Deductions";
            colDeductions.Name = "colDeductions";
            colDeductions.ReadOnly = true;
            // 
            // colNetPay
            // 
            colNetPay.DataPropertyName = "NetPay";
            dataGridViewCellStyle6.Format = "₱#,##0.00";
            colNetPay.DefaultCellStyle = dataGridViewCellStyle6;
            colNetPay.HeaderText = "Net Pay";
            colNetPay.Name = "colNetPay";
            colNetPay.ReadOnly = true;
            // 
            // colPaymentStatus
            // 
            colPaymentStatus.DataPropertyName = "PaymentStatus";
            colPaymentStatus.HeaderText = "Payment Status";
            colPaymentStatus.Name = "colPaymentStatus";
            colPaymentStatus.ReadOnly = true;
            // 
            // colReceiptNumber
            // 
            colReceiptNumber.DataPropertyName = "ReceiptNumber";
            colReceiptNumber.HeaderText = "Receipt No.";
            colReceiptNumber.Name = "colReceiptNumber";
            colReceiptNumber.ReadOnly = true;
            // 
            // PayrollControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(dgvPayroll);
            Controls.Add(pnlSearch);
            Controls.Add(pnlPayrollEntry);
            Controls.Add(lblPayrollSubtitle);
            Controls.Add(lblPayrollTitle);
            Name = "PayrollControl";
            Size = new Size(1102, 680);
            pnlPayrollEntry.ResumeLayout(false);
            pnlPayrollEntry.PerformLayout();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPayroll).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}