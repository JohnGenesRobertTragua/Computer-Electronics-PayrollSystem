using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    partial class PayEmployeeForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlDetails;

        private Label lblEmployeeName;
        private Label lblEmployeeNameValue;
        private Label lblEmployeeID;
        private Label lblEmployeeIDValue;
        private Label lblPayPeriod;
        private Label lblPayPeriodValue;
        private Label lblGrossPay;
        private Label lblGrossPayValue;
        private Label lblDeductions;
        private Label lblDeductionsValue;
        private Label lblNetPay;
        private Label lblNetPayValue;

        private Label lblPaymentMethod;
        private ComboBox cmbPaymentMethod;
        private Label lblPaymentDate;
        private DateTimePicker dtpPaymentDate;

        private Button btnConfirmPayment;
        private Button btnCancelPayment;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubtitle = new Label();
            pnlDetails = new Panel();

            lblEmployeeName = new Label();
            lblEmployeeNameValue = new Label();
            lblEmployeeID = new Label();
            lblEmployeeIDValue = new Label();
            lblPayPeriod = new Label();
            lblPayPeriodValue = new Label();
            lblGrossPay = new Label();
            lblGrossPayValue = new Label();
            lblDeductions = new Label();
            lblDeductionsValue = new Label();
            lblNetPay = new Label();
            lblNetPayValue = new Label();

            lblPaymentMethod = new Label();
            cmbPaymentMethod = new ComboBox();
            lblPaymentDate = new Label();
            dtpPaymentDate = new DateTimePicker();

            btnConfirmPayment = new Button();
            btnCancelPayment = new Button();

            pnlDetails.SuspendLayout();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(18, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(178, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "PAY EMPLOYEE";

            // lblSubtitle
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(20, 48);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(266, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Record payment and generate the payroll receipt";

            // pnlDetails
            pnlDetails.BorderStyle = BorderStyle.FixedSingle;
            pnlDetails.Location = new Point(20, 78);
            pnlDetails.Name = "pnlDetails";
            pnlDetails.Size = new Size(474, 250);
            pnlDetails.TabIndex = 2;
            pnlDetails.Controls.Add(lblEmployeeName);
            pnlDetails.Controls.Add(lblEmployeeNameValue);
            pnlDetails.Controls.Add(lblEmployeeID);
            pnlDetails.Controls.Add(lblEmployeeIDValue);
            pnlDetails.Controls.Add(lblPayPeriod);
            pnlDetails.Controls.Add(lblPayPeriodValue);
            pnlDetails.Controls.Add(lblGrossPay);
            pnlDetails.Controls.Add(lblGrossPayValue);
            pnlDetails.Controls.Add(lblDeductions);
            pnlDetails.Controls.Add(lblDeductionsValue);
            pnlDetails.Controls.Add(lblNetPay);
            pnlDetails.Controls.Add(lblNetPayValue);

            // Employee Name
            lblEmployeeName.AutoSize = true;
            lblEmployeeName.Location = new Point(18, 16);
            lblEmployeeName.Name = "lblEmployeeName";
            lblEmployeeName.Size = new Size(91, 15);
            lblEmployeeName.TabIndex = 0;
            lblEmployeeName.Text = "Employee Name:";

            lblEmployeeNameValue.AutoEllipsis = true;
            lblEmployeeNameValue.Location = new Point(130, 15);
            lblEmployeeNameValue.Name = "lblEmployeeNameValue";
            lblEmployeeNameValue.Size = new Size(320, 18);
            lblEmployeeNameValue.TabIndex = 1;
            lblEmployeeNameValue.Text = "-";

            // Employee ID
            lblEmployeeID.AutoSize = true;
            lblEmployeeID.Location = new Point(18, 48);
            lblEmployeeID.Name = "lblEmployeeID";
            lblEmployeeID.Size = new Size(69, 15);
            lblEmployeeID.TabIndex = 2;
            lblEmployeeID.Text = "Employee ID:";

            lblEmployeeIDValue.Location = new Point(130, 47);
            lblEmployeeIDValue.Name = "lblEmployeeIDValue";
            lblEmployeeIDValue.Size = new Size(320, 18);
            lblEmployeeIDValue.TabIndex = 3;
            lblEmployeeIDValue.Text = "-";

            // Pay Period
            lblPayPeriod.AutoSize = true;
            lblPayPeriod.Location = new Point(18, 80);
            lblPayPeriod.Name = "lblPayPeriod";
            lblPayPeriod.Size = new Size(63, 15);
            lblPayPeriod.TabIndex = 4;
            lblPayPeriod.Text = "Pay Period:";

            lblPayPeriodValue.AutoEllipsis = true;
            lblPayPeriodValue.Location = new Point(130, 79);
            lblPayPeriodValue.Name = "lblPayPeriodValue";
            lblPayPeriodValue.Size = new Size(320, 18);
            lblPayPeriodValue.TabIndex = 5;
            lblPayPeriodValue.Text = "-";

            // Gross Pay
            lblGrossPay.AutoSize = true;
            lblGrossPay.Location = new Point(18, 118);
            lblGrossPay.Name = "lblGrossPay";
            lblGrossPay.Size = new Size(61, 15);
            lblGrossPay.TabIndex = 6;
            lblGrossPay.Text = "Gross Pay:";

            lblGrossPayValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGrossPayValue.Location = new Point(130, 117);
            lblGrossPayValue.Name = "lblGrossPayValue";
            lblGrossPayValue.Size = new Size(320, 18);
            lblGrossPayValue.TabIndex = 7;
            lblGrossPayValue.Text = "₱0.00";
            lblGrossPayValue.TextAlign = ContentAlignment.MiddleRight;

            // Deductions
            lblDeductions.AutoSize = true;
            lblDeductions.Location = new Point(18, 150);
            lblDeductions.Name = "lblDeductions";
            lblDeductions.Size = new Size(70, 15);
            lblDeductions.TabIndex = 8;
            lblDeductions.Text = "Deductions:";

            lblDeductionsValue.Location = new Point(130, 149);
            lblDeductionsValue.Name = "lblDeductionsValue";
            lblDeductionsValue.Size = new Size(320, 18);
            lblDeductionsValue.TabIndex = 9;
            lblDeductionsValue.Text = "₱0.00";
            lblDeductionsValue.TextAlign = ContentAlignment.MiddleRight;

            // Net Pay
            lblNetPay.AutoSize = true;
            lblNetPay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNetPay.Location = new Point(18, 190);
            lblNetPay.Name = "lblNetPay";
            lblNetPay.Size = new Size(60, 19);
            lblNetPay.TabIndex = 10;
            lblNetPay.Text = "NET PAY:";

            lblNetPayValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblNetPayValue.Location = new Point(130, 188);
            lblNetPayValue.Name = "lblNetPayValue";
            lblNetPayValue.Size = new Size(320, 23);
            lblNetPayValue.TabIndex = 11;
            lblNetPayValue.Text = "₱0.00";
            lblNetPayValue.TextAlign = ContentAlignment.MiddleRight;

            // Payment Method
            lblPaymentMethod.AutoSize = true;
            lblPaymentMethod.Location = new Point(20, 350);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(97, 15);
            lblPaymentMethod.TabIndex = 3;
            lblPaymentMethod.Text = "Payment Method:";

            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.FormattingEnabled = true;
            cmbPaymentMethod.Items.AddRange(new object[]
            {
                "Cash",
                "Bank Transfer",
                "GCash",
                "Check"
            });
            cmbPaymentMethod.Location = new Point(20, 371);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.Size = new Size(220, 23);
            cmbPaymentMethod.TabIndex = 4;

            // Payment Date
            lblPaymentDate.AutoSize = true;
            lblPaymentDate.Location = new Point(260, 350);
            lblPaymentDate.Name = "lblPaymentDate";
            lblPaymentDate.Size = new Size(81, 15);
            lblPaymentDate.TabIndex = 5;
            lblPaymentDate.Text = "Payment Date:";

            dtpPaymentDate.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            dtpPaymentDate.Format = DateTimePickerFormat.Custom;
            dtpPaymentDate.Location = new Point(260, 371);
            dtpPaymentDate.Name = "dtpPaymentDate";
            dtpPaymentDate.Size = new Size(234, 23);
            dtpPaymentDate.TabIndex = 6;

            // Confirm
            btnConfirmPayment.Location = new Point(248, 418);
            btnConfirmPayment.Name = "btnConfirmPayment";
            btnConfirmPayment.Size = new Size(122, 35);
            btnConfirmPayment.TabIndex = 7;
            btnConfirmPayment.Text = "CONFIRM PAYMENT";
            btnConfirmPayment.UseVisualStyleBackColor = true;
            btnConfirmPayment.Click += btnConfirmPayment_Click;

            // Cancel
            btnCancelPayment.Location = new Point(372, 418);
            btnCancelPayment.Name = "btnCancelPayment";
            btnCancelPayment.Size = new Size(122, 35);
            btnCancelPayment.TabIndex = 8;
            btnCancelPayment.Text = "CANCEL";
            btnCancelPayment.UseVisualStyleBackColor = true;
            btnCancelPayment.Click += btnCancelPayment_Click;

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 475);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(pnlDetails);
            Controls.Add(lblPaymentMethod);
            Controls.Add(cmbPaymentMethod);
            Controls.Add(lblPaymentDate);
            Controls.Add(dtpPaymentDate);
            Controls.Add(btnConfirmPayment);
            Controls.Add(btnCancelPayment);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PayEmployeeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pay Employee";

            pnlDetails.ResumeLayout(false);
            pnlDetails.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
