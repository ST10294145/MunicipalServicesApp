using System.Windows;

namespace MunicipalServicesApp
{
    public partial class AdminLogin : Window
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password; // PasswordBox requires .Password

            // Seeded credentials
            string seedUser = "admin";
            string seedPass = "admin123";

            if (username == seedUser && password == seedPass)
            {
                MessageBox.Show("Login successful!", "Admin Login", MessageBoxButton.OK, MessageBoxImage.Information);

                // Mark admin as logged in
                App.IsAdminLoggedIn = true;

                // Open the admin management window (for feedback or reports)
                ReportManagement reportWindow = new ReportManagement();
                reportWindow.Show();

                this.Close(); // close login window
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
