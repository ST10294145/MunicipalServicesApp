using System.Windows;

namespace MunicipalServicesApp
{
    public partial class MainWindow : Window
    {
        private bool isAdmin = false;

        public MainWindow()
        {
            InitializeComponent();
            // Load existing requests when app starts
            RequestManager.Instance.LoadFromFile();
        }

        // Admin Login Button
        private void btnAdminLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin loginWindow = new AdminLogin();
            bool? result = loginWindow.ShowDialog();
            if (result == true)
            {
                // Admin successfully logged in
                isAdmin = true;
                MessageBox.Show("Admin login successful!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // USER SERVICES

        // Report Issues Button - Opens ReportIssueForm
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportIssueForm reportWindow = new ReportIssueForm();
            reportWindow.Show();
            this.Close();
        }

        // Submit Service Request Button - Opens AddServiceRequest
        private void btnServiceRequest_Click(object sender, RoutedEventArgs e)
        {
            // You can prompt user for email or use a default
            // For now using a default email - you can enhance this later
            AddServiceRequest requestWindow = new AddServiceRequest("user@example.com");
            requestWindow.Show();
        }

        // View My Requests Button - Opens ViewUserRequest
        private void btnServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            ViewUserRequest viewUserRequest = new ViewUserRequest();
            viewUserRequest.Show();
        }

        // Local Events Button - Opens LocalEvents
        private void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            LocalEvents eventsWindow = new LocalEvents(isAdmin);
            eventsWindow.ShowDialog();
        }

        // ADMIN SERVICES

        // Manage Service Requests Button - Opens AdminServiceRequests (Admin Only)
        private void btnManageRequests_Click(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Admin access required. Please login as admin first.",
                    "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AdminServiceRequests adminServiceRequests = new AdminServiceRequests();
            adminServiceRequests.Show();
        }

        // View All Reports Button - Opens ReportManagement (Admin Only)
        private void btnReportManagement_Click(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Admin access required. Please login as admin first.",
                    "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ReportManagement reportWindow = new ReportManagement();
            reportWindow.Show();
            this.Close();
        }
    }
}