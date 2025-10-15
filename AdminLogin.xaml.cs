using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MunicipalServicesApp
{
    public partial class AdminLogin : Window
    {
        // Replace with your actual admin credentials
        private const string AdminUsername = "admin";
        private const string AdminPassword = "password123";

        public AdminLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (username == AdminUsername && password == AdminPassword)
            {
                MessageBox.Show("Login successful!", "Admin Login", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true; // Important: signals MainWindow that login succeeded
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Placeholder effect for Username TextBox
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text == "Username")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "Username";
                tb.Foreground = Brushes.Gray;
            }
        }
    }
}
