using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class AdminServiceRequests : Window
    {
        private ServiceRequest? selectedRequest;
        private List<ServiceRequest> displayedRequests;

        public AdminServiceRequests()
        {
            InitializeComponent();
            displayedRequests = new List<ServiceRequest>();
            Loaded += AdminServiceRequests_Loaded;
        }

        private void AdminServiceRequests_Loaded(object sender, RoutedEventArgs e)
        {
            // Subscribe to new request events
            ServiceRequestManager.Instance.RequestAdded += OnRequestAdded;

            // Load all categories into filter
            LoadCategoryFilter();

            // Load initial data
            RefreshRequestList();

            txtStatusBar.Text = $"Loaded {ServiceRequestManager.Instance.AllRequests.Count} total requests";
        }

        private void OnRequestAdded(object? sender, ServiceRequest newRequest)
        {
            // When a new request is added, refresh the display
            Dispatcher.Invoke(() =>
            {
                RefreshRequestList();
                txtStatusBar.Text = $"New request #{newRequest.IssueID} added - Total: {ServiceRequestManager.Instance.AllRequests.Count}";
            });
        }

        private void LoadCategoryFilter()
        {
            // Get unique categories from all requests
            var categories = ServiceRequestManager.Instance.AllRequests
                .Select(r => r.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Clear and populate category filter
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add(new ComboBoxItem { Content = "All Categories" });

            foreach (var category in categories)
            {
                cmbCategoryFilter.Items.Add(new ComboBoxItem { Content = category });
            }

            cmbCategoryFilter.SelectedIndex = 0;
        }

        private void RefreshRequestList()
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // Start with all requests from ServiceRequestManager
            var filtered = ServiceRequestManager.Instance.AllRequests.AsEnumerable();

            // Apply Status Filter
            if (cmbStatusFilter?.SelectedItem != null)
            {
                var selectedStatus = (cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All Statuses")
                {
                    filtered = filtered.Where(r => r.Status == selectedStatus);
                }
            }

            // Apply Priority Filter
            if (cmbPriorityFilter?.SelectedItem != null)
            {
                var selectedPriority = (cmbPriorityFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(selectedPriority) && selectedPriority != "All Priorities")
                {
                    filtered = filtered.Where(r => r.Priority == selectedPriority);
                }
            }

            // Apply Category Filter
            if (cmbCategoryFilter?.SelectedItem != null)
            {
                var selectedCategory = (cmbCategoryFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "All Categories")
                {
                    filtered = filtered.Where(r => r.Category == selectedCategory);
                }
            }

            displayedRequests = filtered.OrderByDescending(r => r.DateReported).ToList();
            dgRequests.ItemsSource = null;
            dgRequests.ItemsSource = displayedRequests;

            txtStatusBar.Text = $"Displaying {displayedRequests.Count} of {ServiceRequestManager.Instance.AllRequests.Count} requests";
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbPriorityFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnRefreshList_Click(object sender, RoutedEventArgs e)
        {
            LoadCategoryFilter();
            RefreshRequestList();
            txtStatusBar.Text = $"List refreshed - {ServiceRequestManager.Instance.AllRequests.Count} total requests";
        }

        private void dgRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRequests.SelectedItem is ServiceRequest request)
            {
                selectedRequest = request;
                DisplayRequestDetails(request);

                // Enable status update controls
                cmbNewStatus.IsEnabled = true;
                btnUpdateStatus.IsEnabled = true;

                // Set current status in dropdown
                SetCurrentStatus(request.Status);
            }
            else
            {
                selectedRequest = null;
                txtSelectedDetails.Text = "Select a request from the list above to view details and update status.";
                cmbNewStatus.IsEnabled = false;
                btnUpdateStatus.IsEnabled = false;
                txtUpdateMessage.Text = "";
            }
        }

        private void DisplayRequestDetails(ServiceRequest request)
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine($"REQUEST ID: {request.IssueID}");
            details.AppendLine($"Title: {request.Title}");
            details.AppendLine();
            details.AppendLine($"Category: {request.Category}");
            details.AppendLine($"Priority: {request.Priority}");
            details.AppendLine($"Current Status: {request.Status}");
            details.AppendLine();
            details.AppendLine($"Description:");
            details.AppendLine($"{request.Description}");
            details.AppendLine();
            details.AppendLine($"Reported By: {request.Reporter}");
            details.AppendLine($"Email: {request.Email}");
            details.AppendLine($"Location: {request.StreetAddress}");
            details.AppendLine();
            details.AppendLine($"Date Reported: {request.DateReported:yyyy-MM-dd HH:mm}");
            details.AppendLine($"Days Open: {request.DaysOpen} days");
            details.AppendLine($"SLA Deadline: {request.SLADeadline:yyyy-MM-dd HH:mm}");

            // Check if overdue
            if (DateTime.Now > request.SLADeadline &&
                request.Status != "Resolved" &&
                request.Status != "Closed")
            {
                details.AppendLine();
                details.AppendLine("⚠️ WARNING: This request is OVERDUE!");
            }

            txtSelectedDetails.Text = details.ToString();
        }

        private void SetCurrentStatus(string currentStatus)
        {
            foreach (ComboBoxItem item in cmbNewStatus.Items)
            {
                if (item.Content.ToString() == currentStatus)
                {
                    cmbNewStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRequest == null)
            {
                MessageBox.Show("Please select a request to update.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbNewStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a new status.", "No Status Selected",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newStatus = (cmbNewStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string oldStatus = selectedRequest.Status;

            if (newStatus == oldStatus)
            {
                MessageBox.Show($"The request is already in '{newStatus}' status.", "No Change",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Confirm the update
            var result = MessageBox.Show(
                $"Update Request #{selectedRequest.IssueID} status?\n\n" +
                $"From: {oldStatus}\n" +
                $"To: {newStatus}\n\n" +
                $"Title: {selectedRequest.Title}",
                "Confirm Status Update",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                // Update the status
                selectedRequest.Status = newStatus;

                // Update in ServiceRequestManager
                ServiceRequestManager.Instance.UpdateRequest(selectedRequest);

                // Refresh the display
                RefreshRequestList();
                DisplayRequestDetails(selectedRequest);

                // Show success message
                txtUpdateMessage.Text = $"✓ Status updated to '{newStatus}' successfully!";
                txtUpdateMessage.Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(39, 174, 96)); // Green

                txtStatusBar.Text = $"Request #{selectedRequest.IssueID} updated to {newStatus}";

                // Clear message after 3 seconds
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += (s, args) =>
                {
                    txtUpdateMessage.Text = "";
                    timer.Stop();
                };
                timer.Start();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Unsubscribe from events
            ServiceRequestManager.Instance.RequestAdded -= OnRequestAdded;
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            // Cleanup: Unsubscribe from events
            ServiceRequestManager.Instance.RequestAdded -= OnRequestAdded;
            base.OnClosed(e);
        }
    }
}