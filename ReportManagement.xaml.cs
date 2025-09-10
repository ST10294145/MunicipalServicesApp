using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MunicipalServicesApp
{
    public partial class ReportManagement : Window
    {
        public ReportManagement()
        {
            InitializeComponent();
            dgIssues.ItemsSource = ((App)Application.Current).IssueList.GetAllIssues();
        }

        private void dgIssues_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgIssues.SelectedItem is not Issue issue) return;
            if (dgIssues.CurrentColumn is not DataGridColumn column) return;

            string header = column.Header?.ToString();

            // ✅ Only react if they double-click Feedback or Attachment
            if (header == "Feedback")
            {
                FeedbackWindow feedbackWindow = new FeedbackWindow(issue);
                feedbackWindow.ShowDialog();

                // Refresh DataGrid to show updated feedback
                dgIssues.Items.Refresh();
            }
            else if (header == "Attachment" && !string.IsNullOrEmpty(issue.FilePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(issue.FilePath) { UseShellExecute = true });
                }
                catch
                {
                    MessageBox.Show("Unable to open the file.", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have successfully logged out.", "Logout",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            AdminLogin login = new AdminLogin();
            login.Show();
            this.Close();
        }
    }
}
