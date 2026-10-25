namespace ComputerElectronicsPayrollSystem
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            btnLogin = new Button();
            btnExit = new Button();
            chkShowPassword = new CheckBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(32, 34);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(386, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "COMPUTER & ELECTRONICS SHOP";
            lblTitle.Click += lblTitle_Click;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.Location = new Point(103, 63);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(230, 20);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "PAYROLL MANAGEMENT SYSTEM";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(84, 144);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(60, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username";
            lblUsername.Click += label1_Click;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(84, 182);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password";
            lblPassword.Click += lblPassword_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(150, 174);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(210, 23);
            txtPassword.TabIndex = 5;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(150, 136);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(210, 23);
            txtUsername.TabIndex = 4;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(238, 263);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(81, 32);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "LOGIN";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(119, 263);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(82, 32);
            btnExit.TabIndex = 7;
            btnExit.Text = "EXIT";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Location = new Point(150, 203);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(108, 19);
            chkShowPassword.TabIndex = 8;
            chkShowPassword.Text = "Show Password";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 417);
            Controls.Add(chkShowPassword);
            Controls.Add(btnExit);
            Controls.Add(btnLogin);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Computer and  Electronics Shop Payroll System - Login";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Button btnLogin;
        private Button btnExit;
        private CheckBox chkShowPassword;
    }
}
