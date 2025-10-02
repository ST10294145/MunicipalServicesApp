using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MunicipalServicesApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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
            this.Close(); // Optional: Close MainWindow if you want
        }


        private void btnServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            ServiceStatus statusWindow = new ServiceStatus();
            statusWindow.Show();
            this.Close();
        }

        private void btnAdminLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin loginWindow = new AdminLogin();
            loginWindow.ShowDialog(); // Opens login as a modal window
        }

        private void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            LocalEvents eventsform = new LocalEvents();
            eventsform.ShowDialog();
        }
    }
}