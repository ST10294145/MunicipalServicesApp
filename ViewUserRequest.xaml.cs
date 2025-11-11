using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class ViewUserRequest : Window
    {
        public ViewUserRequest()
        {
            InitializeComponent();
            LoadRequests();

            // Handle selection changed to show details
            dgRequests.SelectionChanged += DgRequests_SelectionChanged;
        }

        private void LoadRequests()
        {
            // Get all requests from RequestManager
            var allRequests = RequestManager.Instance.GetAllRequests();

            // Display in DataGrid
            dgRequests.ItemsSource = allRequests;

            // Update statistics
            UpdateStatistics(allRequests);

            // Show message if no requests
            if (allRequests.Count == 0)
            {
                MessageBox.Show("No service requests found. Submit a new request to get started!",
                    "No Requests", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateStatistics(List<ServiceRequest> requests)
        {
            txtTotalCount.Text = requests.Count.ToString();
            txtPendingCount.Text = requests.Count(r => r.Status == "Pending").ToString();
            txtInProgressCount.Text = requests.Count(r => r.Status == "In Progress").ToString();
            txtCompletedCount.Text = requests.Count(r => r.Status == "Completed").ToString();
        }

        private void DgRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRequests.SelectedItem is ServiceRequest selected)
            {
                // Show details panel
                pnlDetails.Visibility = Visibility.Visible;

                // Populate details
                txtDescription.Text = selected.Description;
                txtReporter.Text = selected.Reporter;
                txtEmail.Text = selected.Email;
                txtStreetAddress.Text = selected.StreetAddress;
                txtSLADeadline.Text = selected.SLADeadline.ToString("MMMM dd, yyyy");
            }
            else
            {
                pnlDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RequestManager.Instance.LoadFromFile();
            LoadRequests();
            MessageBox.Show("Requests refreshed successfully!", "Refresh",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}