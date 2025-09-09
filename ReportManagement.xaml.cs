using System.Collections.Generic;
using System.Windows;

namespace MunicipalServicesApp
{
    public partial class ReportManagement : Window
    {
        public ReportManagement()
        {
            InitializeComponent();
            LoadIssues();
        }

        private void LoadIssues()
        {
            // Get all issues from the global IssueList
            var issueList = ((App)Application.Current).IssueList;
            List<Issue> allIssues = issueList.GetAllIssues();

            // Bind to DataGrid
            dgIssues.ItemsSource = allIssues;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
