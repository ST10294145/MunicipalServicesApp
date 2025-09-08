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
            // Validate input
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter both a Title and Description.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get selected category text
            string selectedCategory = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other";

            // Create a new issue
            Issue newIssue = new Issue
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                Reporter = txtName.Text,
                Email = txtEmail.Text,
                Category = selectedCategory,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            // Add issue to the global linked list
            ((App)Application.Current).IssueList.AddIssue(newIssue);

            MessageBox.Show("Issue submitted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // Go back to the main window
            MainWindow main = new MainWindow();
            main.Show();
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
