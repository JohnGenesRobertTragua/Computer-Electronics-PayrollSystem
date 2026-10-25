using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            btnLogout = new Button();
            btnSettings = new Button();
            btnReports = new Button();
            btnPayroll = new Button();
            btnAttendance = new Button();
            btnEmployees = new Button();
            btnDashboard = new Button();
            lblCompany = new Label();
            pnlMain = new Panel();
            tblKpiCards = new TableLayoutPanel();
            pnlEmployeesCard = new Panel();
            lblEmployeeCount = new Label();
            lblEmployeesTitle = new Label();
            pnlActiveCard = new Panel();
            lblActiveCount = new Label();
            lblActiveTitle = new Label();
            pnlPayrollCard = new Panel();
            lblPayrollAmount = new Label();
            lblPayrollTitle = new Label();
            pnlPendingCard = new Panel();
            lblPendingCount = new Label();
            lblPendingTitle = new Label();
            lblRecentPayroll = new Label();
            dgvRecentPayroll = new DataGridView();
            colEmployee = new DataGridViewTextBoxColumn();
            colPayPeriod = new DataGridViewTextBoxColumn();
            colGrossPay = new DataGridViewTextBoxColumn();
            colDeductions = new DataGridViewTextBoxColumn();
            colNetPay = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            lblWelcome = new Label();
            lblDashboardTitle = new Label();
            pnlSidebar.SuspendLayout();
            pnlMain.SuspendLayout();
            tblKpiCards.SuspendLayout();
            pnlEmployeesCard.SuspendLayout();
            pnlActiveCard.SuspendLayout();
            pnlPayrollCard.SuspendLayout();
            pnlPendingCard.SuspendLayout();
            ((ISupportInitialize)dgvRecentPayroll).BeginInit();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.Controls.Add(btnLogout);
            pnlSidebar.Controls.Add(btnSettings);
            pnlSidebar.Controls.Add(btnReports);
            pnlSidebar.Controls.Add(btnPayroll);
            pnlSidebar.Controls.Add(btnAttendance);
            pnlSidebar.Controls.Add(btnEmployees);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(lblCompany);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(220, 661);
            pnlSidebar.TabIndex = 0;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(0, 331);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(220, 23);
            btnLogout.TabIndex = 9;
            btnLogout.Text = "LOGOUT";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(0, 302);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(220, 23);
            btnSettings.TabIndex = 8;
            btnSettings.Text = "SETTINGS";
            btnSettings.UseVisualStyleBackColor = true;
            // 
            // btnReports
            // 
            btnReports.Location = new Point(0, 273);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(220, 23);
            btnReports.TabIndex = 7;
            btnReports.Text = "REPORTS";
            btnReports.UseVisualStyleBackColor = true;
            btnReports.Click += btnReports_Click;
            // 
            // btnPayroll
            // 
            btnPayroll.Location = new Point(0, 215);
            btnPayroll.Name = "btnPayroll";
            btnPayroll.Size = new Size(220, 23);
            btnPayroll.TabIndex = 5;
            btnPayroll.Text = "PAYROLL";
            btnPayroll.UseVisualStyleBackColor = true;
            // 
            // btnAttendance
            // 
            btnAttendance.Location = new Point(0, 186);
            btnAttendance.Name = "btnAttendance";
            btnAttendance.Size = new Size(220, 23);
            btnAttendance.TabIndex = 4;
            btnAttendance.Text = "ATTENDANCE";
            btnAttendance.UseVisualStyleBackColor = true;
            // 
            // btnEmployees
            // 
            btnEmployees.Location = new Point(0, 157);
            btnEmployees.Name = "btnEmployees";
            btnEmployees.Size = new Size(220, 23);
            btnEmployees.TabIndex = 3;
            btnEmployees.Text = "EMPLOYEES";
            btnEmployees.UseVisualStyleBackColor = true;
            // 
            // btnDashboard
            // 
            btnDashboard.Location = new Point(0, 128);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(220, 23);
            btnDashboard.TabIndex = 2;
            btnDashboard.Text = "DASHBOARD";
            btnDashboard.UseVisualStyleBackColor = true;
            // 
            // lblCompany
            // 
            lblCompany.AutoSize = true;
            lblCompany.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCompany.Location = new Point(41, 17);
            lblCompany.Name = "lblCompany";
            lblCompany.Size = new Size(135, 75);
            lblCompany.TabIndex = 1;
            lblCompany.Text = "COMPUTER &\nELECTRONICS\nSHOP";
            lblCompany.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlMain
            // 
            pnlMain.AutoSize = true;
            pnlMain.Controls.Add(tblKpiCards);
            pnlMain.Controls.Add(lblRecentPayroll);
            pnlMain.Controls.Add(dgvRecentPayroll);
            pnlMain.Controls.Add(lblWelcome);
            pnlMain.Controls.Add(lblDashboardTitle);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(220, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(964, 661);
            pnlMain.TabIndex = 1;
            pnlMain.Paint += pnlMain_Paint;
            // 
            // tblKpiCards
            // 
            tblKpiCards.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tblKpiCards.ColumnCount = 4;
            tblKpiCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpiCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpiCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpiCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKpiCards.Controls.Add(pnlEmployeesCard, 0, 0);
            tblKpiCards.Controls.Add(pnlActiveCard, 1, 0);
            tblKpiCards.Controls.Add(pnlPayrollCard, 2, 0);
            tblKpiCards.Controls.Add(pnlPendingCard, 3, 0);
            tblKpiCards.Location = new Point(20, 75);
            tblKpiCards.Name = "tblKpiCards";
            tblKpiCards.RowCount = 1;
            tblKpiCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblKpiCards.Size = new Size(920, 120);
            tblKpiCards.TabIndex = 5;
            // 
            // pnlEmployeesCard
            // 
            pnlEmployeesCard.Controls.Add(lblEmployeeCount);
            pnlEmployeesCard.Controls.Add(lblEmployeesTitle);
            pnlEmployeesCard.Dock = DockStyle.Fill;
            pnlEmployeesCard.Location = new Point(3, 3);
            pnlEmployeesCard.Name = "pnlEmployeesCard";
            pnlEmployeesCard.Size = new Size(224, 114);
            pnlEmployeesCard.TabIndex = 3;
            // 
            // lblEmployeeCount
            // 
            lblEmployeeCount.AutoSize = true;
            lblEmployeeCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmployeeCount.Location = new Point(91, 40);
            lblEmployeeCount.Name = "lblEmployeeCount";
            lblEmployeeCount.Size = new Size(38, 45);
            lblEmployeeCount.TabIndex = 4;
            lblEmployeeCount.Text = "0";
            // 
            // lblEmployeesTitle
            // 
            lblEmployeesTitle.AutoSize = true;
            lblEmployeesTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmployeesTitle.Location = new Point(48, 12);
            lblEmployeesTitle.Name = "lblEmployeesTitle";
            lblEmployeesTitle.Size = new Size(125, 17);
            lblEmployeesTitle.TabIndex = 3;
            lblEmployeesTitle.Text = "TOTAL EMPLOYEES";
            // 
            // pnlActiveCard
            // 
            pnlActiveCard.Controls.Add(lblActiveCount);
            pnlActiveCard.Controls.Add(lblActiveTitle);
            pnlActiveCard.Dock = DockStyle.Fill;
            pnlActiveCard.Location = new Point(233, 3);
            pnlActiveCard.Name = "pnlActiveCard";
            pnlActiveCard.Size = new Size(224, 114);
            pnlActiveCard.TabIndex = 4;
            // 
            // lblActiveCount
            // 
            lblActiveCount.AutoSize = true;
            lblActiveCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblActiveCount.Location = new Point(93, 40);
            lblActiveCount.Name = "lblActiveCount";
            lblActiveCount.Size = new Size(38, 45);
            lblActiveCount.TabIndex = 4;
            lblActiveCount.Text = "0";
            // 
            // lblActiveTitle
            // 
            lblActiveTitle.AutoSize = true;
            lblActiveTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblActiveTitle.Location = new Point(47, 12);
            lblActiveTitle.Name = "lblActiveTitle";
            lblActiveTitle.Size = new Size(130, 17);
            lblActiveTitle.TabIndex = 3;
            lblActiveTitle.Text = "ACTIVE EMPLOYEES";
            // 
            // pnlPayrollCard
            // 
            pnlPayrollCard.Controls.Add(lblPayrollAmount);
            pnlPayrollCard.Controls.Add(lblPayrollTitle);
            pnlPayrollCard.Dock = DockStyle.Fill;
            pnlPayrollCard.Location = new Point(463, 3);
            pnlPayrollCard.Name = "pnlPayrollCard";
            pnlPayrollCard.Size = new Size(224, 114);
            pnlPayrollCard.TabIndex = 5;
            // 
            // lblPayrollAmount
            // 
            lblPayrollAmount.AutoSize = true;
            lblPayrollAmount.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPayrollAmount.Location = new Point(58, 40);
            lblPayrollAmount.Name = "lblPayrollAmount";
            lblPayrollAmount.Size = new Size(103, 45);
            lblPayrollAmount.TabIndex = 4;
            lblPayrollAmount.Text = "₱0.00";
            lblPayrollAmount.Click += lblPayrollAmount_Click;
            // 
            // lblPayrollTitle
            // 
            lblPayrollTitle.AutoSize = true;
            lblPayrollTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPayrollTitle.Location = new Point(46, 12);
            lblPayrollTitle.Name = "lblPayrollTitle";
            lblPayrollTitle.Size = new Size(131, 17);
            lblPayrollTitle.TabIndex = 3;
            lblPayrollTitle.Text = "MONTHLY PAYROLL";
            // 
            // pnlPendingCard
            // 
            pnlPendingCard.Controls.Add(lblPendingCount);
            pnlPendingCard.Controls.Add(lblPendingTitle);
            pnlPendingCard.Dock = DockStyle.Fill;
            pnlPendingCard.Location = new Point(693, 3);
            pnlPendingCard.Name = "pnlPendingCard";
            pnlPendingCard.Size = new Size(224, 114);
            pnlPendingCard.TabIndex = 6;
            // 
            // lblPendingCount
            // 
            lblPendingCount.AutoSize = true;
            lblPendingCount.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPendingCount.Location = new Point(95, 40);
            lblPendingCount.Name = "lblPendingCount";
            lblPendingCount.Size = new Size(38, 45);
            lblPendingCount.TabIndex = 4;
            lblPendingCount.Text = "0";
            // 
            // lblPendingTitle
            // 
            lblPendingTitle.AutoSize = true;
            lblPendingTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPendingTitle.Location = new Point(51, 12);
            lblPendingTitle.Name = "lblPendingTitle";
            lblPendingTitle.Size = new Size(125, 17);
            lblPendingTitle.TabIndex = 3;
            lblPendingTitle.Text = "PENDING PAYROLL";
            // 
            // lblRecentPayroll
            // 
            lblRecentPayroll.AutoSize = true;
            lblRecentPayroll.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecentPayroll.Location = new Point(18, 233);
            lblRecentPayroll.Name = "lblRecentPayroll";
            lblRecentPayroll.Size = new Size(141, 21);
            lblRecentPayroll.TabIndex = 7;
            lblRecentPayroll.Text = "RECENT PAYROLL";
            // 
            // dgvRecentPayroll
            // 
            dgvRecentPayroll.AllowUserToAddRows = false;
            dgvRecentPayroll.AllowUserToDeleteRows = false;
            dgvRecentPayroll.AllowUserToResizeRows = false;
            dgvRecentPayroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRecentPayroll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentPayroll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecentPayroll.Columns.AddRange(new DataGridViewColumn[] { colEmployee, colPayPeriod, colGrossPay, colDeductions, colNetPay, colStatus });
            dgvRecentPayroll.Location = new Point(23, 262);
            dgvRecentPayroll.MultiSelect = false;
            dgvRecentPayroll.Name = "dgvRecentPayroll";
            dgvRecentPayroll.ReadOnly = true;
            dgvRecentPayroll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentPayroll.Size = new Size(917, 387);
            dgvRecentPayroll.TabIndex = 6;
            dgvRecentPayroll.CellContentClick += dgvRecentPayroll_CellContentClick;
            // 
            // colEmployee
            // 
            colEmployee.HeaderText = "Employee";
            colEmployee.Name = "colEmployee";
            colEmployee.ReadOnly = true;
            // 
            // colPayPeriod
            // 
            colPayPeriod.HeaderText = "Pay Period";
            colPayPeriod.Name = "colPayPeriod";
            colPayPeriod.ReadOnly = true;
            // 
            // colGrossPay
            // 
            colGrossPay.HeaderText = "Gross Pay";
            colGrossPay.Name = "colGrossPay";
            colGrossPay.ReadOnly = true;
            // 
            // colDeductions
            // 
            colDeductions.HeaderText = "Deductions";
            colDeductions.Name = "colDeductions";
            colDeductions.ReadOnly = true;
            // 
            // colNetPay
            // 
            colNetPay.HeaderText = "Net Pay";
            colNetPay.Name = "colNetPay";
            colNetPay.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWelcome.Location = new Point(6, 46);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(268, 17);
            lblWelcome.TabIndex = 1;
            lblWelcome.Text = "Welcome to the Payroll Management System";
            // 
            // lblDashboardTitle
            // 
            lblDashboardTitle.AutoSize = true;
            lblDashboardTitle.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDashboardTitle.Location = new Point(6, 9);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(193, 37);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "DASH BOARD";
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 661);
            Controls.Add(pnlMain);
            Controls.Add(pnlSidebar);
            KeyPreview = true;
            MinimumSize = new Size(1000, 600);
            Name = "DashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Computer & Electronics Shop Payroll System";
            WindowState = FormWindowState.Maximized;
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            tblKpiCards.ResumeLayout(false);
            pnlEmployeesCard.ResumeLayout(false);
            pnlEmployeesCard.PerformLayout();
            pnlActiveCard.ResumeLayout(false);
            pnlActiveCard.PerformLayout();
            pnlPayrollCard.ResumeLayout(false);
            pnlPayrollCard.PerformLayout();
            pnlPendingCard.ResumeLayout(false);
            pnlPendingCard.PerformLayout();
            ((ISupportInitialize)dgvRecentPayroll).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlSidebar;
        private Label lblCompany;
        private Button btnDashboard;
        private Button btnSettings;
        private Button btnReports;
        private Button btnPayroll;
        private Button btnAttendance;
        private Button btnEmployees;
        private Button btnLogout;

        private Panel pnlMain;
        private Label lblDashboardTitle;
        private Label lblWelcome;

        private DataGridView dgvRecentPayroll;
        private Label lblRecentPayroll;

        private DataGridViewTextBoxColumn colEmployee;
        private DataGridViewTextBoxColumn colPayPeriod;
        private DataGridViewTextBoxColumn colGrossPay;
        private DataGridViewTextBoxColumn colDeductions;
        private DataGridViewTextBoxColumn colNetPay;
        private DataGridViewTextBoxColumn colStatus;
        private TableLayoutPanel tblKpiCards;
        private Panel pnlEmployeesCard;
        private Label lblEmployeeCount;
        private Label lblEmployeesTitle;
        private Panel pnlActiveCard;
        private Label lblActiveCount;
        private Label lblActiveTitle;
        private Panel pnlPendingCard;
        private Label lblPendingCount;
        private Label lblPendingTitle;
        private Panel pnlPayrollCard;
        private Label lblPayrollAmount;
        private Label lblPayrollTitle;
    }
}