namespace ComputerElectronicsPayrollSystem
{
    partial class AttendanceControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Required method for disposing managed resources.
        /// </summary>
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
            pnlEntry = new Panel();
            btnClear = new Button();
            btnTimeOut = new Button();
            btnDeleteAttendance = new Button();
            btnTimeIn = new Button();
            cmbStatus = new ComboBox();
            btnEditAttendance = new Button();
            lblStatus = new Label();

            // IMPORTANT:
            // These are now the custom read-only DateTimePicker controls.
            dtpTimeOut = new ReadOnlyDateTimePicker();

            btnSaveAttendance = new Button();
            lblTimeOut = new Label();

            // IMPORTANT:
            // These are now the custom read-only DateTimePicker controls.
            dtpTimeIn = new ReadOnlyDateTimePicker();

            lblTimeIn = new Label();
            dtpAttendanceDate = new DateTimePicker();
            lblDate = new Label();
            cmbEmployee = new ComboBox();
            lblEmployee = new Label();

            pnlSearch = new Panel();
            btnSearch = new Button();
            txtSearch = new TextBox();
            lblSearch = new Label();

            dgvAttendance = new DataGridView();

            colAttendanceID = new DataGridViewTextBoxColumn();
            colEmployeeID = new DataGridViewTextBoxColumn();
            colEmployeeName = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colTimeIn = new DataGridViewTextBoxColumn();
            colTimeOut = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();

            lblAttendanceSubtitle = new Label();
            lblAttendanceTitle = new Label();

            pnlEntry.SuspendLayout();
            pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).BeginInit();
            SuspendLayout();

            // 
            // pnlEntry
            // 
            pnlEntry.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            pnlEntry.BorderStyle =
                BorderStyle.FixedSingle;

            pnlEntry.Controls.Add(btnClear);
            pnlEntry.Controls.Add(btnTimeOut);
            pnlEntry.Controls.Add(btnDeleteAttendance);
            pnlEntry.Controls.Add(btnTimeIn);
            pnlEntry.Controls.Add(cmbStatus);
            pnlEntry.Controls.Add(btnEditAttendance);
            pnlEntry.Controls.Add(lblStatus);
            pnlEntry.Controls.Add(dtpTimeOut);
            pnlEntry.Controls.Add(btnSaveAttendance);
            pnlEntry.Controls.Add(lblTimeOut);
            pnlEntry.Controls.Add(dtpTimeIn);
            pnlEntry.Controls.Add(lblTimeIn);
            pnlEntry.Controls.Add(dtpAttendanceDate);
            pnlEntry.Controls.Add(lblDate);
            pnlEntry.Controls.Add(cmbEmployee);
            pnlEntry.Controls.Add(lblEmployee);

            pnlEntry.Location =
                new Point(20, 90);

            pnlEntry.Name =
                "pnlEntry";

            pnlEntry.Size =
                new Size(920, 145);

            pnlEntry.TabIndex = 0;

            // 
            // btnClear
            // 
            btnClear.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnClear.BackColor =
                SystemColors.Control;

            btnClear.FlatStyle =
                FlatStyle.Flat;

            btnClear.Location =
                new Point(812, 90);

            btnClear.Name =
                "btnClear";

            btnClear.Size =
                new Size(88, 30);

            btnClear.TabIndex = 14;

            btnClear.Text =
                "CLEAR";

            btnClear.UseVisualStyleBackColor =
                true;

            // 
            // btnTimeOut
            // 
            btnTimeOut.BackColor =
                SystemColors.Control;

            btnTimeOut.FlatStyle =
                FlatStyle.Flat;

            btnTimeOut.Location =
                new Point(529, 83);

            btnTimeOut.Name =
                "btnTimeOut";

            btnTimeOut.Size =
                new Size(88, 30);

            btnTimeOut.TabIndex = 10;

            btnTimeOut.Text =
                "TIME OUT";

            btnTimeOut.UseVisualStyleBackColor =
                true;

            btnTimeOut.Click +=
                btnTimeOut_Click;

            // 
            // btnDeleteAttendance
            // 
            btnDeleteAttendance.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnDeleteAttendance.BackColor =
                SystemColors.Control;

            btnDeleteAttendance.FlatStyle =
                FlatStyle.Flat;

            btnDeleteAttendance.Location =
                new Point(694, 88);

            btnDeleteAttendance.Name =
                "btnDeleteAttendance";

            btnDeleteAttendance.Size =
                new Size(88, 30);

            btnDeleteAttendance.TabIndex = 13;

            btnDeleteAttendance.Text =
                "DELETE";

            btnDeleteAttendance.UseVisualStyleBackColor =
                true;

            // 
            // btnTimeIn
            // 
            btnTimeIn.BackColor =
                SystemColors.Control;

            btnTimeIn.FlatStyle =
                FlatStyle.Flat;

            btnTimeIn.Location =
                new Point(435, 83);

            btnTimeIn.Name =
                "btnTimeIn";

            btnTimeIn.Size =
                new Size(88, 30);

            btnTimeIn.TabIndex = 9;

            btnTimeIn.Text =
                "TIME IN";

            btnTimeIn.UseVisualStyleBackColor =
                true;

            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbStatus.FormattingEnabled =
                true;

            cmbStatus.Items.AddRange(
                new object[]
                {
                    "Present",
                    "Absent",
                    "Late",
                    "Half Day",
                    "On Leave"
                });

            cmbStatus.Location =
                new Point(435, 29);

            cmbStatus.Name =
                "cmbStatus";

            cmbStatus.Size =
                new Size(160, 23);

            cmbStatus.TabIndex = 8;

            // 
            // btnEditAttendance
            // 
            btnEditAttendance.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnEditAttendance.BackColor =
                SystemColors.Control;

            btnEditAttendance.FlatStyle =
                FlatStyle.Flat;

            btnEditAttendance.Location =
                new Point(812, 29);

            btnEditAttendance.Name =
                "btnEditAttendance";

            btnEditAttendance.Size =
                new Size(88, 30);

            btnEditAttendance.TabIndex = 12;

            btnEditAttendance.Text =
                "UPDATE";

            btnEditAttendance.UseVisualStyleBackColor =
                true;

            btnEditAttendance.Click +=
                btnEditAttendance_Click_1;

            // 
            // lblStatus
            // 
            lblStatus.AutoSize =
                true;

            lblStatus.Location =
                new Point(435, 10);

            lblStatus.Name =
                "lblStatus";

            lblStatus.Size =
                new Size(42, 15);

            lblStatus.TabIndex = 7;

            lblStatus.Text =
                "Status:";

            // 
            // dtpTimeOut
            // 
            dtpTimeOut.CustomFormat =
                "hh:mm tt";

            dtpTimeOut.Format =
                DateTimePickerFormat.Custom;

            dtpTimeOut.Location =
                new Point(280, 90);

            dtpTimeOut.Name =
                "dtpTimeOut";

            dtpTimeOut.ShowCheckBox =
                true;

            dtpTimeOut.ShowUpDown =
                true;

            dtpTimeOut.Size =
                new Size(120, 23);

            dtpTimeOut.TabIndex = 6;

            // 
            // btnSaveAttendance
            // 
            btnSaveAttendance.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnSaveAttendance.BackColor =
                SystemColors.Control;

            btnSaveAttendance.FlatStyle =
                FlatStyle.Flat;

            btnSaveAttendance.Location =
                new Point(694, 29);

            btnSaveAttendance.Name =
                "btnSaveAttendance";

            btnSaveAttendance.Size =
                new Size(88, 30);

            btnSaveAttendance.TabIndex = 11;

            btnSaveAttendance.Text =
                "SAVE";

            btnSaveAttendance.UseVisualStyleBackColor =
                true;

            // 
            // lblTimeOut
            // 
            lblTimeOut.AutoSize =
                true;

            lblTimeOut.Location =
                new Point(280, 72);

            lblTimeOut.Name =
                "lblTimeOut";

            lblTimeOut.Size =
                new Size(60, 15);

            lblTimeOut.TabIndex = 5;

            lblTimeOut.Text =
                "Time Out:";

            // 
            // dtpTimeIn
            // 
            dtpTimeIn.CustomFormat =
                "hh:mm tt";

            dtpTimeIn.Format =
                DateTimePickerFormat.Custom;

            dtpTimeIn.Location =
                new Point(140, 90);

            dtpTimeIn.Name =
                "dtpTimeIn";

            dtpTimeIn.ShowCheckBox =
                true;

            dtpTimeIn.ShowUpDown =
                true;

            dtpTimeIn.Size =
                new Size(120, 23);

            dtpTimeIn.TabIndex = 4;

            // 
            // lblTimeIn
            // 
            lblTimeIn.AutoSize =
                true;

            lblTimeIn.Location =
                new Point(140, 72);

            lblTimeIn.Name =
                "lblTimeIn";

            lblTimeIn.Size =
                new Size(50, 15);

            lblTimeIn.TabIndex = 3;

            lblTimeIn.Text =
                "Time In:";

            // 
            // dtpAttendanceDate
            // 
            dtpAttendanceDate.Format =
                DateTimePickerFormat.Short;

            dtpAttendanceDate.Location =
                new Point(18, 90);

            dtpAttendanceDate.Name =
                "dtpAttendanceDate";

            dtpAttendanceDate.Size =
                new Size(105, 23);

            dtpAttendanceDate.TabIndex = 2;

            // 
            // lblDate
            // 
            lblDate.AutoSize =
                true;

            lblDate.Location =
                new Point(18, 72);

            lblDate.Name =
                "lblDate";

            lblDate.Size =
                new Size(34, 15);

            lblDate.TabIndex = 1;

            lblDate.Text =
                "Date:";

            // 
            // cmbEmployee
            // 
            cmbEmployee.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbEmployee.FormattingEnabled =
                true;

            cmbEmployee.Location =
                new Point(18, 29);

            cmbEmployee.Name =
                "cmbEmployee";

            cmbEmployee.Size =
                new Size(382, 23);

            cmbEmployee.TabIndex = 0;

            // 
            // lblEmployee
            // 
            lblEmployee.AutoSize =
                true;

            lblEmployee.Location =
                new Point(18, 10);

            lblEmployee.Name =
                "lblEmployee";

            lblEmployee.Size =
                new Size(62, 15);

            lblEmployee.TabIndex = 0;

            lblEmployee.Text =
                "Employee:";

            // 
            // pnlSearch
            // 
            pnlSearch.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(lblSearch);

            pnlSearch.Location =
                new Point(20, 250);

            pnlSearch.Name =
                "pnlSearch";

            pnlSearch.Size =
                new Size(920, 50);

            pnlSearch.TabIndex = 1;

            // 
            // btnSearch
            // 
            btnSearch.BackColor =
                SystemColors.Control;

            btnSearch.FlatStyle =
                FlatStyle.Flat;

            btnSearch.Location =
                new Point(427, 10);

            btnSearch.Name =
                "btnSearch";

            btnSearch.Size =
                new Size(85, 28);

            btnSearch.TabIndex = 2;

            btnSearch.Text =
                "SEARCH";

            btnSearch.UseVisualStyleBackColor =
                true;

            // 
            // txtSearch
            // 
            txtSearch.Location =
                new Point(120, 12);

            txtSearch.Name =
                "txtSearch";

            txtSearch.Size =
                new Size(300, 23);

            txtSearch.TabIndex = 1;

            // 
            // lblSearch
            // 
            lblSearch.AutoSize =
                true;

            lblSearch.Location =
                new Point(15, 16);

            lblSearch.Name =
                "lblSearch";

            lblSearch.Size =
                new Size(100, 15);

            lblSearch.TabIndex = 0;

            lblSearch.Text =
                "Search Employee:";

            // 
            // dgvAttendance
            // 
            dgvAttendance.AllowUserToAddRows =
                false;

            dgvAttendance.AllowUserToDeleteRows =
                false;

            dgvAttendance.AllowUserToResizeRows =
                false;

            dgvAttendance.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgvAttendance.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvAttendance.BackgroundColor =
                SystemColors.Window;

            dgvAttendance.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvAttendance.Columns.AddRange(
                new DataGridViewColumn[]
                {
                    colAttendanceID,
                    colEmployeeID,
                    colEmployeeName,
                    colDate,
                    colTimeIn,
                    colTimeOut,
                    colStatus
                });

            dgvAttendance.Location =
                new Point(20, 315);

            dgvAttendance.MultiSelect =
                false;

            dgvAttendance.Name =
                "dgvAttendance";

            dgvAttendance.ReadOnly =
                true;

            dgvAttendance.RowHeadersVisible =
                false;

            dgvAttendance.RowTemplate.Height =
                30;

            dgvAttendance.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvAttendance.Size =
                new Size(920, 320);

            dgvAttendance.TabIndex = 2;

            // 
            // colAttendanceID
            // 
            colAttendanceID.DataPropertyName =
                "AttendanceID";

            colAttendanceID.HeaderText =
                "Attendance ID";

            colAttendanceID.Name =
                "colAttendanceID";

            colAttendanceID.ReadOnly =
                true;

            // 
            // colEmployeeID
            // 
            colEmployeeID.DataPropertyName =
                "EmployeeID";

            colEmployeeID.HeaderText =
                "Employee ID";

            colEmployeeID.Name =
                "colEmployeeID";

            colEmployeeID.ReadOnly =
                true;

            // 
            // colEmployeeName
            // 
            colEmployeeName.DataPropertyName =
                "EmployeeName";

            colEmployeeName.HeaderText =
                "Employee Name";

            colEmployeeName.Name =
                "colEmployeeName";

            colEmployeeName.ReadOnly =
                true;

            // 
            // colDate
            // 
            colDate.DataPropertyName =
                "Date";

            colDate.HeaderText =
                "Date";

            colDate.Name =
                "colDate";

            colDate.ReadOnly =
                true;

            // 
            // colTimeIn
            // 
            colTimeIn.DataPropertyName =
                "TimeIn";

            colTimeIn.HeaderText =
                "Time In";

            colTimeIn.Name =
                "colTimeIn";

            colTimeIn.ReadOnly =
                true;

            // 
            // colTimeOut
            // 
            colTimeOut.DataPropertyName =
                "TimeOut";

            colTimeOut.HeaderText =
                "Time Out";

            colTimeOut.Name =
                "colTimeOut";

            colTimeOut.ReadOnly =
                true;

            // 
            // colStatus
            // 
            colStatus.DataPropertyName =
                "Status";

            colStatus.HeaderText =
                "Status";

            colStatus.Name =
                "colStatus";

            colStatus.ReadOnly =
                true;

            // 
            // lblAttendanceSubtitle
            // 
            lblAttendanceSubtitle.AutoSize =
                true;

            lblAttendanceSubtitle.Font =
                new Font("Segoe UI", 9F);

            lblAttendanceSubtitle.Location =
                new Point(20, 53);

            lblAttendanceSubtitle.Name =
                "lblAttendanceSubtitle";

            lblAttendanceSubtitle.Size =
                new Size(230, 15);

            lblAttendanceSubtitle.TabIndex = 4;

            lblAttendanceSubtitle.Text =
                "Record and manage employee attendance";

            // 
            // lblAttendanceTitle
            // 
            lblAttendanceTitle.AutoSize =
                true;

            lblAttendanceTitle.Font =
                new Font("Segoe UI", 18F, FontStyle.Bold);

            lblAttendanceTitle.Location =
                new Point(20, 17);

            lblAttendanceTitle.Name =
                "lblAttendanceTitle";

            lblAttendanceTitle.Size =
                new Size(353, 32);

            lblAttendanceTitle.TabIndex = 3;

            lblAttendanceTitle.Text =
                "ATTENDANCE MANAGEMENT";

            // 
            // AttendanceControl
            // 
            AutoScaleDimensions =
                new SizeF(7F, 15F);

            AutoScaleMode =
                AutoScaleMode.Font;

            BackColor =
                SystemColors.Control;

            Controls.Add(dgvAttendance);
            Controls.Add(pnlSearch);
            Controls.Add(pnlEntry);
            Controls.Add(lblAttendanceSubtitle);
            Controls.Add(lblAttendanceTitle);

            Name =
                "AttendanceControl";

            Size =
                new Size(964, 661);

            pnlEntry.ResumeLayout(false);
            pnlEntry.PerformLayout();

            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)dgvAttendance).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlEntry;

        private Label lblEmployee;
        private ComboBox cmbEmployee;

        private Label lblDate;
        private DateTimePicker dtpAttendanceDate;

        private Label lblTimeIn;
        private ReadOnlyDateTimePicker dtpTimeIn;

        private Label lblTimeOut;
        private ReadOnlyDateTimePicker dtpTimeOut;

        private Label lblStatus;
        private ComboBox cmbStatus;

        private Button btnTimeIn;
        private Button btnTimeOut;

        private Button btnSaveAttendance;
        private Button btnEditAttendance;
        private Button btnDeleteAttendance;
        private Button btnClear;

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnSearch;

        private DataGridView dgvAttendance;

        private DataGridViewTextBoxColumn colAttendanceID;
        private DataGridViewTextBoxColumn colEmployeeID;
        private DataGridViewTextBoxColumn colEmployeeName;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colTimeIn;
        private DataGridViewTextBoxColumn colTimeOut;
        private DataGridViewTextBoxColumn colStatus;

        private Label lblAttendanceTitle;
        private Label lblAttendanceSubtitle;
    }
}