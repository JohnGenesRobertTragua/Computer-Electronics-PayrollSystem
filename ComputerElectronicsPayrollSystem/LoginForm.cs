namespace ComputerElectronicsPayrollSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            Shown += (_, _) => FuturisticUiTheme.Apply(this);
            KeyboardNavigation.Enable(this);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblPassword_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "admin" && txtPassword.Text == "admin123")
            {
                DashboardForm dashboard = new DashboardForm();

                dashboard.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Invalid username or password.",
                    "Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
