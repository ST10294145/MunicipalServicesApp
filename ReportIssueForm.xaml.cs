using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace MunicipalServicesApp
{
    public partial class ReportIssueForm : Window
    {
        private string selectedFilePath = "";  // ✅ Fix: Declare file path

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
                Reporter = txtName.Text,  // ✅ fixed mismatch (was txtReporter)
                Email = txtEmail.Text,
                Province = cmbProvince?.SelectedItem?.ToString(),  // ✅ province
                Category = selectedCategory,
                FilePath = selectedFilePath,  // ✅ now exists
                Feedback = "",
                Status = "Received",
                DateReported = DateTime.Now,
                SLADeadline = CalculateSLA(selectedCategory)
            };

            // Add issue to the global linked list
            ((App)Application.Current).IssueList.AddIssue(newIssue);

            MessageBox.Show("Issue submitted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // Go back to main window
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private DateTime CalculateSLA(string category)
        {
            switch (category)
            {
                case "Water":
                    return DateTime.Now.AddDays(3);
                case "Roads":
                    return DateTime.Now.AddDays(5);
                case "Electricity":
                    return DateTime.Now.AddDays(2);
                default:
                    return DateTime.Now.AddDays(7);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

      
    }
}
