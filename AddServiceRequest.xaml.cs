using System;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class AddServiceRequest : Window
    {
        public ServiceRequest? NewRequest { get; private set; }
        public bool RequestSubmitted { get; private set; }

        public AddServiceRequest()
        {
            InitializeComponent();
            RequestSubmitted = false;
            NewRequest = null;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a request title.", "Required Field",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTitle.Focus();
                return;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Required Field",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbCategory.Focus();
                return;
            }

            if (cmbPriority.SelectedItem == null)
            {
                MessageBox.Show("Please select a priority level.", "Required Field",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbPriority.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a description.", "Required Field",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDescription.Focus();
                return;
            }

            // Generate unique ID based on timestamp
            int newId = GenerateUniqueId();

            // Get form values
            string title = txtTitle.Text.Trim();
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other";
            string priority = (cmbPriority.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Low";
            string description = txtDescription.Text.Trim();
            string location = string.IsNullOrWhiteSpace(txtLocation.Text) ? "Not specified" : txtLocation.Text.Trim();
            string reporter = string.IsNullOrWhiteSpace(txtReporter.Text) ? "Anonymous" : txtReporter.Text.Trim();
            string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? "Not provided" : txtEmail.Text.Trim();

            // Create new service request
            NewRequest = new ServiceRequest(
                id: newId,
                title: title,
                category: category,
                status: "Pending", // All new requests start as Pending
                priority: priority,
                dateReported: DateTime.Now,
                description: description
            );

            // Add optional fields
            NewRequest.Reporter = reporter;
            NewRequest.Email = email;
            NewRequest.StreetAddress = location;

            RequestSubmitted = true;

            // Show confirmation
            MessageBox.Show(
                $"Service Request #{newId} has been submitted successfully!\n\n" +
                $"Title: {title}\n" +
                $"Priority: {priority}\n" +
                $"Status: Pending\n\n" +
                "You can track your request status in the 'Service Request Status' section.",
                "Request Submitted",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            // Try to set DialogResult if opened as modal, otherwise just close
            try
            {
                this.DialogResult = true;
            }
            catch
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to cancel? All entered information will be lost.",
                "Cancel Request",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                RequestSubmitted = false;

                // Try to set DialogResult if opened as modal
                try
                {
                    this.DialogResult = false;
                }
                catch
                {
                    this.Close();
                }
            }
        }

        private int GenerateUniqueId()
        {
            // Generate ID based on current timestamp
            // Format: Current tick count modulo to keep it reasonable
            // In a real app, you'd use database auto-increment or GUID
            return (int)((DateTime.Now.Ticks / TimeSpan.TicksPerSecond) % 100000) + 1000;
        }
    }
}