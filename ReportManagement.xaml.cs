using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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

        // ✅ Handles double-click on a DataGrid row to open attachment
        private void dgIssues_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgIssues.SelectedItem is Issue selectedIssue && !string.IsNullOrEmpty(selectedIssue.FilePath))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = selectedIssue.FilePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open attachment: " + ex.Message,
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
