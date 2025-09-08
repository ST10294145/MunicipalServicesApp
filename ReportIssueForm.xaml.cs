using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    /// <summary>
    /// Interaction logic for ReportIssueForm.xaml
    /// </summary>
    public partial class ReportIssueForm : Window
    {
        public ReportIssueForm()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill in both the Title and Description fields.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create new issue
            Issue newIssue = new Issue
            {
                Title = title,
                Description = description,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            // Add to global IssueList
            ((App)Application.Current).IssueList.AddIssue(newIssue);

            MessageBox.Show("Issue submitted successfully!",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Close and return to MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Simply go back to MainWindow without saving
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

       
    }
}
