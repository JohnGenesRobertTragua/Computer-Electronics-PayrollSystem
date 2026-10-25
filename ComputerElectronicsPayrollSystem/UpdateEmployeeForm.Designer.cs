namespace ComputerElectronicsPayrollSystem
{
    partial class UpdateEmployeeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Required method to clean up any resources being used.
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
        /// Required designer support.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblEmployeeID = new Label();
            txtEmployeeID = new TextBox();
            lblFirstName = new Label();
            txtFirstName = new TextBox();
            lblLastName = new Label();
            txtLastName = new TextBox();
            lblPosition = new Label();
            txtPosition = new TextBox();
            lblDepartment = new Label();
            txtDepartment = new TextBox();
            lblSalary = new Label();
            txtSalary = new TextBox();
            lblSSSNo = new Label();
            txtSSSNo = new TextBox();
            lblPhilHealthPIN = new Label();
            txtPhilHealthPIN = new TextBox();
            lblPagIBIGMID = new Label();
            txtPagIBIGMID = new TextBox();
            lblTIN = new Label();
            txtTIN = new TextBox();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(25, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(214, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Update Employee";
            // 
            // lblEmployeeID
            // 
            lblEmployeeID.AutoSize = true;
            lblEmployeeID.Location = new Point(48, 79);
            lblEmployeeID.Name = "lblEmployeeID";
            lblEmployeeID.Size = new Size(75, 15);
            lblEmployeeID.TabIndex = 1;
            lblEmployeeID.Text = "Employee Id:";
            // 
            // txtEmployeeID
            // 
            txtEmployeeID.Location = new Point(178, 71);
            txtEmployeeID.MaxLength = 20;
            txtEmployeeID.Name = "txtEmployeeID";
            txtEmployeeID.ReadOnly = true;
            txtEmployeeID.Size = new Size(81, 23);
            txtEmployeeID.TabIndex = 0;
            // 
            // lblFirstName
            // 
            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(48, 116);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Size = new Size(67, 15);
            lblFirstName.TabIndex = 2;
            lblFirstName.Text = "First Name:";
            // 
            // txtFirstName
            // 
            txtFirstName.Location = new Point(178, 108);
            txtFirstName.MaxLength = 50;
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(234, 23);
            txtFirstName.TabIndex = 1;
            // 
            // lblLastName
            // 
            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(48, 154);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(66, 15);
            lblLastName.TabIndex = 3;
            lblLastName.Text = "Last Name:";
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(178, 146);
            txtLastName.MaxLength = 50;
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(234, 23);
            txtLastName.TabIndex = 2;
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(48, 192);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(53, 15);
            lblPosition.TabIndex = 4;
            lblPosition.Text = "Position:";
            // 
            // txtPosition
            // 
            txtPosition.Location = new Point(178, 184);
            txtPosition.MaxLength = 50;
            txtPosition.Name = "txtPosition";
            txtPosition.Size = new Size(234, 23);
            txtPosition.TabIndex = 3;
            // 
            // lblDepartment
            // 
            lblDepartment.AutoSize = true;
            lblDepartment.Location = new Point(48, 229);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Size = new Size(73, 15);
            lblDepartment.TabIndex = 5;
            lblDepartment.Text = "Department:";
            // 
            // txtDepartment
            // 
            txtDepartment.Location = new Point(178, 221);
            txtDepartment.MaxLength = 50;
            txtDepartment.Name = "txtDepartment";
            txtDepartment.Size = new Size(234, 23);
            txtDepartment.TabIndex = 4;
            // 
            // lblSalary
            // 
            lblSalary.AutoSize = true;
            lblSalary.Location = new Point(48, 266);
            lblSalary.Name = "lblSalary";
            lblSalary.Size = new Size(97, 15);
            lblSalary.TabIndex = 6;
            lblSalary.Text = "Salary / Monthly:";
            // 
            // txtSalary
            // 
            txtSalary.Location = new Point(178, 258);
            txtSalary.MaxLength = 15;
            txtSalary.Name = "txtSalary";
            txtSalary.Size = new Size(234, 23);
            txtSalary.TabIndex = 5;
            // 
            // lblSSSNo
            // 
            lblSSSNo.AutoSize = true;
            lblSSSNo.Location = new Point(48, 302);
            lblSSSNo.Name = "lblSSSNo";
            lblSSSNo.Size = new Size(50, 15);
            lblSSSNo.TabIndex = 7;
            lblSSSNo.Text = "SSS No.:";
            // 
            // txtSSSNo
            // 
            txtSSSNo.Location = new Point(178, 294);
            txtSSSNo.MaxLength = 12;
            txtSSSNo.Name = "txtSSSNo";
            txtSSSNo.Size = new Size(234, 23);
            txtSSSNo.TabIndex = 6;
            // 
            // lblPhilHealthPIN
            // 
            lblPhilHealthPIN.AutoSize = true;
            lblPhilHealthPIN.Location = new Point(48, 339);
            lblPhilHealthPIN.Name = "lblPhilHealthPIN";
            lblPhilHealthPIN.Size = new Size(87, 15);
            lblPhilHealthPIN.TabIndex = 8;
            lblPhilHealthPIN.Text = "PhilHealth PIN:";
            // 
            // txtPhilHealthPIN
            // 
            txtPhilHealthPIN.Location = new Point(178, 331);
            txtPhilHealthPIN.MaxLength = 14;
            txtPhilHealthPIN.Name = "txtPhilHealthPIN";
            txtPhilHealthPIN.Size = new Size(234, 23);
            txtPhilHealthPIN.TabIndex = 7;
            // 
            // lblPagIBIGMID
            // 
            lblPagIBIGMID.AutoSize = true;
            lblPagIBIGMID.Location = new Point(48, 376);
            lblPagIBIGMID.Name = "lblPagIBIGMID";
            lblPagIBIGMID.Size = new Size(81, 15);
            lblPagIBIGMID.TabIndex = 9;
            lblPagIBIGMID.Text = "Pag-IBIG MID:";
            // 
            // txtPagIBIGMID
            // 
            txtPagIBIGMID.Location = new Point(178, 368);
            txtPagIBIGMID.MaxLength = 14;
            txtPagIBIGMID.Name = "txtPagIBIGMID";
            txtPagIBIGMID.Size = new Size(234, 23);
            txtPagIBIGMID.TabIndex = 8;
            // 
            // lblTIN
            // 
            lblTIN.AutoSize = true;
            lblTIN.Location = new Point(48, 413);
            lblTIN.Name = "lblTIN";
            lblTIN.Size = new Size(29, 15);
            lblTIN.TabIndex = 10;
            lblTIN.Text = "TIN:";
            // 
            // txtTIN
            // 
            txtTIN.Location = new Point(178, 405);
            txtTIN.MaxLength = 15;
            txtTIN.Name = "txtTIN";
            txtTIN.Size = new Size(234, 23);
            txtTIN.TabIndex = 9;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(48, 450);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 11;
            lblStatus.Text = "Status:";
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            cmbStatus.Location = new Point(178, 442);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(81, 23);
            cmbStatus.TabIndex = 10;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(131, 510);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 11;
            btnSave.Text = "UPDATE";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(266, 510);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "CANCEL";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // UpdateEmployeeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 580);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cmbStatus);
            Controls.Add(lblStatus);
            Controls.Add(txtTIN);
            Controls.Add(lblTIN);
            Controls.Add(txtPagIBIGMID);
            Controls.Add(lblPagIBIGMID);
            Controls.Add(txtPhilHealthPIN);
            Controls.Add(lblPhilHealthPIN);
            Controls.Add(txtSSSNo);
            Controls.Add(lblSSSNo);
            Controls.Add(txtSalary);
            Controls.Add(lblSalary);
            Controls.Add(txtDepartment);
            Controls.Add(lblDepartment);
            Controls.Add(txtPosition);
            Controls.Add(lblPosition);
            Controls.Add(txtLastName);
            Controls.Add(lblLastName);
            Controls.Add(txtFirstName);
            Controls.Add(lblFirstName);
            Controls.Add(txtEmployeeID);
            Controls.Add(lblEmployeeID);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UpdateEmployeeForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Update Employee";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;

        private Label lblEmployeeID;
        private TextBox txtEmployeeID;

        private Label lblFirstName;
        private TextBox txtFirstName;

        private Label lblLastName;
        private TextBox txtLastName;

        private Label lblPosition;
        private TextBox txtPosition;

        private Label lblDepartment;
        private TextBox txtDepartment;

        private Label lblSalary;
        private TextBox txtSalary;

        private Label lblSSSNo;
        private TextBox txtSSSNo;

        private Label lblPhilHealthPIN;
        private TextBox txtPhilHealthPIN;

        private Label lblPagIBIGMID;
        private TextBox txtPagIBIGMID;

        private Label lblTIN;
        private TextBox txtTIN;

        private Label lblStatus;
        private ComboBox cmbStatus;

        private Button btnSave;
        private Button btnCancel;
    }
}
