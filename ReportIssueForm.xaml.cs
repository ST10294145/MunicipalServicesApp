using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;  // ✅ Needed for OpenFileDialog

namespace MunicipalServicesApp
{
    public partial class ReportIssueForm : Window
    {
        private string selectedFilePath = "";  // File path for attachments

        public ReportIssueForm()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Validate required input
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter both a Title and Description.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get selected category and province
            string selectedCategory = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other";
            string selectedProvince = (cmbProvince.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Unknown";

            // Create a new Issue object
            Issue newIssue = new Issue
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                Reporter = txtName.Text,
                Email = txtEmail.Text,
                Province = selectedProvince,
                Category = selectedCategory,
                FilePath = selectedFilePath,  // ✅ stores attached file path
                Feedback = "",
                Status = "Received",
                DateReported = DateTime.Now,
                SLADeadline = CalculateSLA(selectedCategory)
            };

            // Add issue to global IssueList
            ((App)Application.Current).IssueList.AddIssue(newIssue);

            MessageBox.Show("Issue submitted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // Return to MainWindow
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private DateTime CalculateSLA(string category)
        {
            switch (category)
            {
                case "Water Leak":
                    return DateTime.Now.AddDays(3);
                case "Pothole":
                    return DateTime.Now.AddDays(5);
                case "Electricity Outage":
                    return DateTime.Now.AddDays(2);
                default:
                    return DateTime.Now.AddDays(7); // Default SLA
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Return to main window without saving
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        //File attachment logic
        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file to attach",
                Filter = "All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName; // Save full path
                txtFilePath.Text = System.IO.Path.GetFileName(selectedFilePath); // Show file name only in UI
            }
        }
    }
}
