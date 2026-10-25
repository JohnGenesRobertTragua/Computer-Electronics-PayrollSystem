using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    partial class ReportsControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblReportsTitle;
        private Label lblReportsSubtitle;

        private Panel pnlFilters;
        private Label lblReportType;
        private ComboBox cmbReportType;
        private Label lblFrom;
        private DateTimePicker dtpFrom;
        private Label lblTo;
        private DateTimePicker dtpTo;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnGenerateReport;
        private Button btnExportCsv;
        private Button btnSearch;
        private Button btnClearSearch;

        private Label lblSummary;
        private DataGridView dgvReport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblReportsTitle = new Label();
            lblReportsSubtitle = new Label();

            pnlFilters = new Panel();
            lblReportType = new Label();
            cmbReportType = new ComboBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnGenerateReport = new Button();
            btnExportCsv = new Button();
            btnSearch = new Button();
            btnClearSearch = new Button();

            lblSummary = new Label();
            dgvReport = new DataGridView();

            pnlHeader.SuspendLayout();
            pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
            SuspendLayout();

            // Header
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.BackColor = Color.FromArgb(245, 247, 251);
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(964, 78);
            pnlHeader.Controls.Add(lblReportsTitle);
            pnlHeader.Controls.Add(lblReportsSubtitle);

            lblReportsTitle.AutoSize = true;
            lblReportsTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblReportsTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblReportsTitle.Location = new Point(18, 7);
            lblReportsTitle.Name = "lblReportsTitle";
            lblReportsTitle.Text = "REPORTS";

            lblReportsSubtitle.AutoSize = true;
            lblReportsSubtitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblReportsSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblReportsSubtitle.Location = new Point(21, 49);
            lblReportsSubtitle.Name = "lblReportsSubtitle";
            lblReportsSubtitle.Text = "Generate payroll, attendance, employee, and payment receipt reports";

            // Filter panel - first row for report type/date/actions, second row for search.
            pnlFilters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFilters.BackColor = Color.White;
            pnlFilters.BorderStyle = BorderStyle.FixedSingle;
            pnlFilters.Location = new Point(22, 88);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Size = new Size(920, 112);
            pnlFilters.Controls.Add(lblReportType);
            pnlFilters.Controls.Add(cmbReportType);
            pnlFilters.Controls.Add(lblFrom);
            pnlFilters.Controls.Add(dtpFrom);
            pnlFilters.Controls.Add(lblTo);
            pnlFilters.Controls.Add(dtpTo);
            pnlFilters.Controls.Add(btnGenerateReport);
            pnlFilters.Controls.Add(btnExportCsv);
            pnlFilters.Controls.Add(lblSearch);
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(btnSearch);
            pnlFilters.Controls.Add(btnClearSearch);

            lblReportType.AutoSize = true;
            lblReportType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblReportType.ForeColor = Color.FromArgb(51, 65, 85);
            lblReportType.Location = new Point(15, 19);
            lblReportType.Name = "lblReportType";
            lblReportType.Text = "Report Type";

            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportType.Location = new Point(101, 15);
            cmbReportType.Name = "cmbReportType";
            cmbReportType.Size = new Size(190, 25);
            cmbReportType.TabIndex = 0;

            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFrom.ForeColor = Color.FromArgb(51, 65, 85);
            lblFrom.Location = new Point(307, 19);
            lblFrom.Name = "lblFrom";
            lblFrom.Text = "From";

            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(348, 15);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(122, 23);
            dtpFrom.TabIndex = 1;

            lblTo.AutoSize = true;
            lblTo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTo.ForeColor = Color.FromArgb(51, 65, 85);
            lblTo.Location = new Point(486, 19);
            lblTo.Name = "lblTo";
            lblTo.Text = "To";

            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(512, 15);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(122, 23);
            dtpTo.TabIndex = 2;

            btnGenerateReport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateReport.FlatStyle = FlatStyle.Flat;
            btnGenerateReport.FlatAppearance.BorderSize = 0;
            btnGenerateReport.BackColor = Color.FromArgb(37, 99, 235);
            btnGenerateReport.ForeColor = Color.White;
            btnGenerateReport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGenerateReport.Location = new Point(647, 12);
            btnGenerateReport.Name = "btnGenerateReport";
            btnGenerateReport.Size = new Size(132, 32);
            btnGenerateReport.TabIndex = 4;
            btnGenerateReport.Text = "GENERATE REPORT";
            btnGenerateReport.UseVisualStyleBackColor = false;
            btnGenerateReport.Cursor = Cursors.Hand;

            btnExportCsv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportCsv.FlatStyle = FlatStyle.Flat;
            btnExportCsv.FlatAppearance.BorderSize = 0;
            btnExportCsv.BackColor = Color.FromArgb(13, 148, 136);
            btnExportCsv.ForeColor = Color.White;
            btnExportCsv.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportCsv.Location = new Point(789, 12);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new Size(122, 32);
            btnExportCsv.TabIndex = 5;
            btnExportCsv.Text = "EXPORT CSV";
            btnExportCsv.UseVisualStyleBackColor = false;
            btnExportCsv.Cursor = Cursors.Hand;

            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSearch.ForeColor = Color.FromArgb(51, 65, 85);
            lblSearch.Location = new Point(15, 72);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search Records";

            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Location = new Point(118, 68);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(792, 23);
            txtSearch.TabIndex = 3;

            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 1;
            btnSearch.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 231, 248);
            btnSearch.BackColor = Color.FromArgb(226, 234, 246);
            btnSearch.ForeColor = Color.FromArgb(30, 58, 95);
            btnSearch.Font = new Font("Segoe UI Symbol", 11F, FontStyle.Regular);
            btnSearch.Location = new Point(0, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(36, 27);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "⌕";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Cursor = Cursors.Hand;

            btnClearSearch.FlatStyle = FlatStyle.Flat;
            btnClearSearch.FlatAppearance.BorderSize = 1;
            btnClearSearch.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnClearSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(219, 231, 248);
            btnClearSearch.BackColor = Color.FromArgb(226, 234, 246);
            btnClearSearch.ForeColor = Color.FromArgb(30, 58, 95);
            btnClearSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClearSearch.Location = new Point(0, 0);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new Size(58, 27);
            btnClearSearch.TabIndex = 7;
            btnClearSearch.Text = "CLEAR";
            btnClearSearch.UseVisualStyleBackColor = false;
            btnClearSearch.Cursor = Cursors.Hand;
            btnClearSearch.AccessibleName = "Clear search";
            btnClearSearch.TabStop = false;

            // Report summary
            lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSummary.AutoEllipsis = true;
            lblSummary.BackColor = Color.Transparent;
            lblSummary.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(51, 65, 85);
            lblSummary.Location = new Point(22, 207);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(920, 26);
            lblSummary.Text = "0 record(s)";
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;

            // Report results grid
            dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.FixedSingle;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReport.ColumnHeadersHeight = 42;
            dgvReport.Location = new Point(22, 239);
            dgvReport.Name = "dgvReport";
            dgvReport.Size = new Size(920, 365);
            dgvReport.TabIndex = 7;

            // ReportsControl
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 251);
            Controls.Add(dgvReport);
            Controls.Add(lblSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlHeader);
            Name = "ReportsControl";
            Size = new Size(964, 620);

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
            ResumeLayout(false);
        }
    }
}
