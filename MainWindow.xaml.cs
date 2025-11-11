using System.Windows;

namespace MunicipalServicesApp
{
    public partial class MainWindow : Window
    {
        private bool isAdmin = false; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdminLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin loginWindow = new AdminLogin();
            bool? result = loginWindow.ShowDialog(); 
            if (result == true)
            {
                // Admin successfully logged in
                isAdmin = true;
            }
        }

        private void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            LocalEvents eventsWindow = new LocalEvents(isAdmin);
            eventsWindow.ShowDialog();
        }

        // All buttons
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportIssueForm reportWindow = new ReportIssueForm();
            reportWindow.Show();
            this.Close();
        }

        private void btnReportManagement_Click(object sender, RoutedEventArgs e)
        {
            ReportManagement reportWindow = new ReportManagement();
            reportWindow.Show();
            this.Close();
        }

        private void btnServiceRequest_Click(object sender, RoutedEventArgs e)
        {
            AddServiceRequest requestWindow = new AddServiceRequest();
            requestWindow.Show();
           
        }

        private void btnServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            ServiceStatus statusWindow = new ServiceStatus(); 
            statusWindow.Show();
        }

        private void btnManageRequests_Click(object sender, RoutedEventArgs e)
        {
            AdminServiceRequests adminServiceRequests = new AdminServiceRequests();
            adminServiceRequests.Show();
        }
    }
}
