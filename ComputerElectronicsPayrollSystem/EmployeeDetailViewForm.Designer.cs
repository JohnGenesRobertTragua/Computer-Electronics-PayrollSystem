namespace ComputerElectronicsPayrollSystem
{
    partial class EmployeeDetailViewForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmployeeID;
        private System.Windows.Forms.Label lblEmployeeIDValue;
        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.Label lblEmployeeNameValue;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Button btnViewReceipt;
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
            lblEmployeeID = new System.Windows.Forms.Label();
            lblEmployeeIDValue = new System.Windows.Forms.Label();
            lblEmployeeName = new System.Windows.Forms.Label();
            lblEmployeeNameValue = new System.Windows.Forms.Label();
            lblSummary = new System.Windows.Forms.Label();
            dgvDetails = new System.Windows.Forms.DataGridView();
            btnViewReceipt = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            SuspendLayout();

            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.Text = "EMPLOYEE DETAILS";

            lblEmployeeID.AutoSize = true;
            lblEmployeeID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmployeeID.Location = new System.Drawing.Point(22, 52);
            lblEmployeeID.Text = "Employee ID:";

            lblEmployeeIDValue.AutoSize = true;
            lblEmployeeIDValue.Location = new System.Drawing.Point(112, 52);

            lblEmployeeName.AutoSize = true;
            lblEmployeeName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEmployeeName.Location = new System.Drawing.Point(205, 52);
            lblEmployeeName.Text = "Employee:";

            lblEmployeeNameValue.AutoSize = true;
            lblEmployeeNameValue.Location = new System.Drawing.Point(274, 52);

            lblSummary.AutoSize = true;
            lblSummary.Location = new System.Drawing.Point(22, 80);
            lblSummary.Text = "";

            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvDetails.Location = new System.Drawing.Point(20, 106);
            dgvDetails.MultiSelect = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.Size = new System.Drawing.Size(1060, 475);

            btnViewReceipt.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnViewReceipt.Location = new System.Drawing.Point(20, 595);
            btnViewReceipt.Size = new System.Drawing.Size(140, 35);
            btnViewReceipt.Text = "VIEW RECEIPT";
            btnViewReceipt.UseVisualStyleBackColor = true;
            btnViewReceipt.Click += btnViewReceipt_Click;

            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnClose.Location = new System.Drawing.Point(960, 595);
            btnClose.Size = new System.Drawing.Size(120, 35);
            btnClose.Text = "CLOSE";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;

            ClientSize = new System.Drawing.Size(1100, 650);
            Controls.Add(lblTitle);
            Controls.Add(lblEmployeeID);
            Controls.Add(lblEmployeeIDValue);
            Controls.Add(lblEmployeeName);
            Controls.Add(lblEmployeeNameValue);
            Controls.Add(lblSummary);
            Controls.Add(dgvDetails);
            Controls.Add(btnViewReceipt);
            Controls.Add(btnClose);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EmployeeDetailViewForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Employee Details";
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
