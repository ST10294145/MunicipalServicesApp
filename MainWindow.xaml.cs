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
            AddServiceRequest requestWindow = new AddServiceRequest("user@example.com");
            requestWindow.Show();
        }

        // View My Requests Button - Opens ViewUserRequest
        private void btnServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            ViewUserRequest viewUserRequest = new ViewUserRequest();
            viewUserRequest.Show();
        }

        // Report Manager Button - Opens RequestManager window
        private void btnRequestManager_Click(object sender, RoutedEventArgs e)
        {
           
            ReportManagement reportWindow = new ReportManagement();
            reportWindow.Show();
            this.Close();
        }

        // Local Events Button - Opens LocalEvents
        private void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            LocalEvents eventsWindow = new LocalEvents(isAdmin);
            eventsWindow.ShowDialog();
        }


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


        // Advanced Analysis Button - Opens AdvancedRequestAnalysis (Admin Only)
        private void btnAdvancedAnalysis_Click(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Admin access required. Please login as admin first.",
                    "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AdvancedRequestAnalysis analysisWindow = new AdvancedRequestAnalysis();
            analysisWindow.Show();
        }
    }
}