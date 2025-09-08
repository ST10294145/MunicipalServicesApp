using System.Linq;
using System.Windows;

namespace MunicipalServicesApp
{
    public partial class ServiceStatus : Window
    {
        public ServiceStatus()
        {
            InitializeComponent();
            LoadIssues();
        }

        private void LoadIssues()
        {
            // Pull issues from the global linked list
            var issues = ((App)Application.Current).IssueList.GetAllIssues().ToList();

            dgIssues.ItemsSource = issues;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
