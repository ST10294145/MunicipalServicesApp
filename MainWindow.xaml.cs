using System.Windows;

namespace MunicipalServicesApp
{
    public partial class MainWindow : Window
    {
        private bool isAdmin = false; // Track if current user is admin

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdminLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin loginWindow = new AdminLogin();
            bool? result = loginWindow.ShowDialog(); // Modal login
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

        // Other buttons...
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

        private void btnServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            ServiceStatus statusWindow = new ServiceStatus();
            statusWindow.Show();
            this.Close();
        }
    }
}
