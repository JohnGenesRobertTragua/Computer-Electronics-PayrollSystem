namespace ComputerElectronicsPayrollSystem
{
    partial class EmployeesControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblEmployeesTitle = new Label();
            lblEmployeesSubtitle = new Label();
            pnlEmployeeSearch = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            pnlEmployeeActions = new Panel();
            btnAddEmployee = new Button();
            btnUpdateEmployee = new Button();
            btnDeleteEmployee = new Button();
            btnClearEmployee = new Button();
            dgvEmployees = new DataGridView();
            colEmployeeID = new DataGridViewTextBoxColumn();
            colFirstName = new DataGridViewTextBoxColumn();
            colLastName = new DataGridViewTextBoxColumn();
            colPosition = new DataGridViewTextBoxColumn();
            colDepartment = new DataGridViewTextBoxColumn();
            colSalary = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            pnlEmployeeSearch.SuspendLayout();
            pnlEmployeeActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            SuspendLayout();
            // 
            // lblEmployeesTitle
            // 
            lblEmployeesTitle.AutoSize = true;
            lblEmployeesTitle.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmployeesTitle.Location = new Point(20, 20);
            lblEmployeesTitle.Name = "lblEmployeesTitle";
            lblEmployeesTitle.Size = new Size(352, 37);
            lblEmployeesTitle.TabIndex = 0;
            lblEmployeesTitle.Text = "EMPLOYEE MANAGEMENT";
            // 
            // lblEmployeesSubtitle
            // 
            lblEmployeesSubtitle.AutoSize = true;
            lblEmployeesSubtitle.Location = new Point(20, 55);
            lblEmployeesSubtitle.Name = "lblEmployeesSubtitle";
            lblEmployeesSubtitle.Size = new Size(171, 15);
            lblEmployeesSubtitle.TabIndex = 1;
            lblEmployeesSubtitle.Text = "Manage employee information";
            lblEmployeesSubtitle.Click += lblEmployeesSubtitle_Click;
            // 
            // pnlEmployeeSearch
            // 
            pnlEmployeeSearch.BorderStyle = BorderStyle.FixedSingle;
            pnlEmployeeSearch.Controls.Add(btnSearch);
            pnlEmployeeSearch.Controls.Add(txtSearch);
            pnlEmployeeSearch.Controls.Add(lblSearch);
            pnlEmployeeSearch.Location = new Point(24, 84);
            pnlEmployeeSearch.Name = "pnlEmployeeSearch";
            pnlEmployeeSearch.Size = new Size(920, 72);
            pnlEmployeeSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlEmployeeSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = false;
            lblSearch.TextAlign = ContentAlignment.MiddleLeft;
            lblSearch.Location = new Point(16, 7);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(150, 18);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Search Employee:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 30);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(650, 30);
            txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(800, 30);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(42, 30);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "\uE721";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.BackColor = Color.FromArgb(220, 225, 232);
            btnSearch.ForeColor = Color.FromArgb(45, 55, 72);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            // 
            // pnlEmployeeActions
            // 
            pnlEmployeeActions.Controls.Add(btnClearEmployee);
            pnlEmployeeActions.Controls.Add(btnDeleteEmployee);
            pnlEmployeeActions.Controls.Add(btnUpdateEmployee);
            pnlEmployeeActions.Controls.Add(btnAddEmployee);
            pnlEmployeeActions.Location = new Point(20, 150);
            pnlEmployeeActions.Name = "pnlEmployeeActions";
            pnlEmployeeActions.Size = new Size(920, 50);
            pnlEmployeeActions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlEmployeeActions.TabIndex = 3;
            // 
            // btnAddEmployee
            // 
            btnAddEmployee.Location = new Point(16, 7);
            btnAddEmployee.Name = "btnAddEmployee";
            btnAddEmployee.Size = new Size(120, 35);
            btnAddEmployee.TabIndex = 0;
            btnAddEmployee.Text = "ADD EMPLOYEE";
            btnAddEmployee.UseVisualStyleBackColor = true;
            // 
            // btnUpdateEmployee
            // 
            btnUpdateEmployee.Location = new Point(159, 7);
            btnUpdateEmployee.Name = "btnUpdateEmployee";
            btnUpdateEmployee.Size = new Size(100, 35);
            btnUpdateEmployee.TabIndex = 1;
            btnUpdateEmployee.Text = "UPDATE";
            btnUpdateEmployee.UseVisualStyleBackColor = true;
            // 
            // btnDeleteEmployee
            // 
            btnDeleteEmployee.Location = new Point(283, 7);
            btnDeleteEmployee.Name = "btnDeleteEmployee";
            btnDeleteEmployee.Size = new Size(100, 35);
            btnDeleteEmployee.TabIndex = 2;
            btnDeleteEmployee.Text = "DELETE";
            btnDeleteEmployee.UseVisualStyleBackColor = true;
            // 
            // btnClearEmployee
            // 
            btnClearEmployee.Location = new Point(403, 7);
            btnClearEmployee.Name = "btnClearEmployee";
            btnClearEmployee.Size = new Size(100, 35);
            btnClearEmployee.TabIndex = 3;
            btnClearEmployee.Text = "CLEAR";
            btnClearEmployee.UseVisualStyleBackColor = true;
            btnClearEmployee.Click += btnClearEmployee_Click;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployees.Columns.AddRange(new DataGridViewColumn[] { colEmployeeID, colFirstName, colLastName, colPosition, colDepartment, colSalary, colStatus });
            dgvEmployees.Location = new Point(24, 226);
            dgvEmployees.MultiSelect = false;
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(920, 400);
            dgvEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEmployees.TabIndex = 4;
            // 
            // colEmployeeID
            // 
            colEmployeeID.HeaderText = "Employee ID";
            colEmployeeID.Name = "colEmployeeID";
            colEmployeeID.ReadOnly = true;
            // 
            // colFirstName
            // 
            colFirstName.HeaderText = "First Name";
            colFirstName.Name = "colFirstName";
            colFirstName.ReadOnly = true;
            // 
            // colLastName
            // 
            colLastName.HeaderText = "Last Name";
            colLastName.Name = "colLastName";
            colLastName.ReadOnly = true;
            // 
            // colPosition
            // 
            colPosition.HeaderText = "Position";
            colPosition.Name = "colPosition";
            colPosition.ReadOnly = true;
            // 
            // colDepartment
            // 
            colDepartment.HeaderText = "Department";
            colDepartment.Name = "colDepartment";
            colDepartment.ReadOnly = true;
            // 
            // colSalary
            // 
            colSalary.HeaderText = "Salary / Monthly";
            colSalary.Name = "colSalary";
            colSalary.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // EmployeesControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(dgvEmployees);
            Controls.Add(pnlEmployeeActions);
            Controls.Add(pnlEmployeeSearch);
            Controls.Add(lblEmployeesSubtitle);
            Controls.Add(lblEmployeesTitle);
            Name = "EmployeesControl";
            MinimumSize = new Size(700, 500);
            Size = new Size(964, 661);
            pnlEmployeeSearch.ResumeLayout(false);
            pnlEmployeeSearch.PerformLayout();
            pnlEmployeeActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEmployeesTitle;
        private Label lblEmployeesSubtitle;
        private Panel pnlEmployeeSearch;
        private Label lblSearch;
        private Button btnSearch;
        private TextBox txtSearch;
        private Panel pnlEmployeeActions;
        private Button btnAddEmployee;
        private Button btnClearEmployee;
        private Button btnDeleteEmployee;
        private Button btnUpdateEmployee;
        private DataGridView dgvEmployees;
        private DataGridViewTextBoxColumn colEmployeeID;
        private DataGridViewTextBoxColumn colFirstName;
        private DataGridViewTextBoxColumn colLastName;
        private DataGridViewTextBoxColumn colPosition;
        private DataGridViewTextBoxColumn colDepartment;
        private DataGridViewTextBoxColumn colSalary;
        private DataGridViewTextBoxColumn colStatus;
    }
}
