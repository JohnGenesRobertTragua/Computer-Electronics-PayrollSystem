using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    partial class PayrollReceiptForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblReceiptNumber;
        private RichTextBox rtbReceipt;
        private Button btnPrint;
        private Button btnClose;

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
            lblReceiptNumber = new Label();
            rtbReceipt = new RichTextBox();
            btnPrint = new Button();
            btnClose = new Button();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(18, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(183, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "PAYROLL RECEIPT";

            // lblReceiptNumber
            lblReceiptNumber.AutoSize = true;
            lblReceiptNumber.Location = new Point(20, 44);
            lblReceiptNumber.Name = "lblReceiptNumber";
            lblReceiptNumber.Size = new Size(0, 15);
            lblReceiptNumber.TabIndex = 1;

            // rtbReceipt
            rtbReceipt.BackColor = Color.White;
            rtbReceipt.Font = new Font("Consolas", 9F);
            rtbReceipt.Location = new Point(20, 70);
            rtbReceipt.Name = "rtbReceipt";
            rtbReceipt.ReadOnly = true;
            rtbReceipt.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbReceipt.Size = new Size(560, 560);
            rtbReceipt.TabIndex = 2;
            rtbReceipt.Text = "";
            rtbReceipt.WordWrap = false;

            // btnPrint
            btnPrint.Location = new Point(352, 645);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(110, 35);
            btnPrint.TabIndex = 3;
            btnPrint.Text = "PRINT / PREVIEW";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;

            // btnClose
            btnClose.Location = new Point(470, 645);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 35);
            btnClose.TabIndex = 4;
            btnClose.Text = "CLOSE";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 700);
            Controls.Add(btnClose);
            Controls.Add(btnPrint);
            Controls.Add(rtbReceipt);
            Controls.Add(lblReceiptNumber);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(620, 700);
            Name = "PayrollReceiptForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Payroll Receipt";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
