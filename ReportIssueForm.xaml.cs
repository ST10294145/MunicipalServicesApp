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
            string name = txtName.Text;
            string email = txtEmail.Text;
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
            string description = txtDescription.Text;

            // Validation
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill in all fields before submitting.",
                                "Validation Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            // Show success message (you can replace this with database/file saving later)
            MessageBox.Show($"Issue reported successfully!\n\n" +
                            $"Name: {name}\n" +
                            $"Email: {email}\n" +
                            $"Category: {category}\n" +
                            $"Description: {description}",
                            "Submission Successful",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            // Redirect back to MainWindow
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
