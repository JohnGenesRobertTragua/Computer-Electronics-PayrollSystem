namespace ComputerElectronicsPayrollSystem
{
    partial class EmployeeProfileForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblEmployeeID;
        private System.Windows.Forms.Label lblEmployeeIDValue;
        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.Label lblEmployeeNameValue;
        private System.Windows.Forms.Button btnInformation;
        private System.Windows.Forms.Button btnAttendance;
        private System.Windows.Forms.Button btnPayroll;
        private System.Windows.Forms.Button btnReceipts;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            lblEmployeeID = new System.Windows.Forms.Label();
            lblEmployeeIDValue = new System.Windows.Forms.Label();
            lblEmployeeName = new System.Windows.Forms.Label();
            lblEmployeeNameValue = new System.Windows.Forms.Label();
            btnInformation = new System.Windows.Forms.Button();
            btnAttendance = new System.Windows.Forms.Button();
            btnPayroll = new System.Windows.Forms.Button();
            btnReceipts = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            SuspendLayout();

            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(24, 20);
            lblTitle.Text = "EMPLOYEE PROFILE";

            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new System.Drawing.Point(26, 52);
            lblSubtitle.Text = "Select what you want to view for this employee";

            lblEmployeeID.AutoSize = true;
            lblEmployeeID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmployeeID.Location = new System.Drawing.Point(26, 88);
            lblEmployeeID.Text = "Employee ID:";

            lblEmployeeIDValue.AutoSize = true;
            lblEmployeeIDValue.Location = new System.Drawing.Point(120, 88);

            lblEmployeeName.AutoSize = true;
            lblEmployeeName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmployeeName.Location = new System.Drawing.Point(26, 116);
            lblEmployeeName.Text = "Employee:";

            lblEmployeeNameValue.AutoSize = true;
            lblEmployeeNameValue.Location = new System.Drawing.Point(120, 116);

            btnInformation.Location = new System.Drawing.Point(28, 160);
            btnInformation.Size = new System.Drawing.Size(260, 42);
            btnInformation.Text = "EMPLOYEE INFORMATION";
            btnInformation.UseVisualStyleBackColor = true;
            btnInformation.Click += btnInformation_Click;

            btnAttendance.Location = new System.Drawing.Point(306, 160);
            btnAttendance.Size = new System.Drawing.Size(190, 42);
            btnAttendance.Text = "ATTENDANCE";
            btnAttendance.UseVisualStyleBackColor = true;
            btnAttendance.Click += btnAttendance_Click;

            btnPayroll.Location = new System.Drawing.Point(28, 216);
            btnPayroll.Size = new System.Drawing.Size(260, 42);
            btnPayroll.Text = "PAYROLL RECORDS";
            btnPayroll.UseVisualStyleBackColor = true;
            btnPayroll.Click += btnPayroll_Click;

            btnReceipts.Location = new System.Drawing.Point(306, 216);
            btnReceipts.Size = new System.Drawing.Size(190, 42);
            btnReceipts.Text = "RECEIPTS";
            btnReceipts.UseVisualStyleBackColor = true;
            btnReceipts.Click += btnReceipts_Click;

            btnClose.Location = new System.Drawing.Point(386, 286);
            btnClose.Size = new System.Drawing.Size(110, 35);
            btnClose.Text = "CLOSE";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;

            ClientSize = new System.Drawing.Size(524, 340);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(lblEmployeeID);
            Controls.Add(lblEmployeeIDValue);
            Controls.Add(lblEmployeeName);
            Controls.Add(lblEmployeeNameValue);
            Controls.Add(btnInformation);
            Controls.Add(btnAttendance);
            Controls.Add(btnPayroll);
            Controls.Add(btnReceipts);
            Controls.Add(btnClose);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EmployeeProfileForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Employee Profile";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
