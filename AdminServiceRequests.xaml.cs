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
        private List<ServiceRequest> filteredRequests;

        public AdminServiceRequests()
        {
            InitializeComponent();
            filteredRequests = new List<ServiceRequest>();

            Loaded += AdminServiceRequests_Loaded;
        }

        private void AdminServiceRequests_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceRequestManager.Instance.RequestAdded += OnRequestAdded;

            UpdateCategoryFilter();
            RefreshRequestsList();
        }

        protected override void OnClosed(EventArgs e)
        {
            ServiceRequestManager.Instance.RequestAdded -= OnRequestAdded;
            base.OnClosed(e);
        }

        private void OnRequestAdded(object? sender, ServiceRequest request)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateCategoryFilter();
                RefreshRequestsList();
                txtStatusBar.Text = $"New request #{request.IssueID} added - List refreshed";
            });
        }

        private void RefreshRequestsList()
        {
            var allRequests = ServiceRequestManager.Instance.AllRequests.ToList();

            var filtered = allRequests.AsEnumerable();

            var selectedStatus = (cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All Statuses")
                filtered = filtered.Where(r => r.Status == selectedStatus);

            var selectedPriority = (cmbPriorityFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(selectedPriority) && selectedPriority != "All Priorities")
                filtered = filtered.Where(r => r.Priority == selectedPriority);

            var selectedCategory = (cmbCategoryFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "All Categories")
                filtered = filtered.Where(r => r.Category == selectedCategory);

            filteredRequests = filtered.OrderByDescending(r => r.GetPriorityValue())
                                       .ThenBy(r => r.DateReported)
                                       .ToList();

            dgRequests.ItemsSource = null;
            dgRequests.ItemsSource = filteredRequests;

            txtStatusBar.Text = $"Displaying {filteredRequests.Count} of {allRequests.Count} requests";
        }

        private void UpdateCategoryFilter()
        {
            var allRequests = ServiceRequestManager.Instance.AllRequests;
            var categories = allRequests.Select(r => r.Category).Distinct().OrderBy(c => c).ToList();

            var currentSelection = (cmbCategoryFilter.SelectedItem as ComboBoxItem)?.Content.ToString();

            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add(new ComboBoxItem { Content = "All Categories" });

            foreach (var category in categories)
            {
                cmbCategoryFilter.Items.Add(new ComboBoxItem { Content = category });
            }

            if (!string.IsNullOrEmpty(currentSelection))
            {
                foreach (ComboBoxItem item in cmbCategoryFilter.Items)
                {
                    if (item.Content.ToString() == currentSelection)
                    {
                        cmbCategoryFilter.SelectedItem = item;
                        return;
                    }
                }
            }

            cmbCategoryFilter.SelectedIndex = 0;
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) RefreshRequestsList();
        }

        private void cmbPriorityFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) RefreshRequestsList();
        }

        private void cmbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) RefreshRequestsList();
        }

        private void btnRefreshList_Click(object sender, RoutedEventArgs e)
        {
            UpdateCategoryFilter();
            RefreshRequestsList();
            txtStatusBar.Text = "✓ List refreshed";
        }

        private void dgRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRequests.SelectedItem is ServiceRequest request)
            {
                selectedRequest = request;
                DisplayRequestDetails(request);
                EnableStatusUpdate(request);
            }
            else
            {
                selectedRequest = null;
                txtSelectedDetails.Text = "Select a request from the list above to view details and update status.";
                DisableStatusUpdate();
            }
        }

        private void DisplayRequestDetails(ServiceRequest request)
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine($"Request ID: #{request.IssueID}");
            details.AppendLine($"Title: {request.Title}");
            details.AppendLine($"Category: {request.Category}");
            details.AppendLine();
            details.AppendLine($"Description: {request.Description}");
            details.AppendLine();
            details.AppendLine($"Current Status: {request.Status}");
            details.AppendLine($"Priority: {request.Priority}");
            details.AppendLine();
            details.AppendLine($"Reporter: {request.Reporter}");
            details.AppendLine($"Email: {request.Email}");
            details.AppendLine($"Location: {request.StreetAddress}");
            details.AppendLine();
            details.AppendLine($"Date Reported: {request.DateReported:yyyy-MM-dd HH:mm}");
            details.AppendLine($"Days Open: {request.DaysOpen}");
            details.AppendLine($"SLA Deadline: {request.SLADeadline:yyyy-MM-dd HH:mm}");

            txtSelectedDetails.Text = details.ToString();
        }

        private void EnableStatusUpdate(ServiceRequest request)
        {
            cmbNewStatus.IsEnabled = true;
            btnUpdateStatus.IsEnabled = true;

            foreach (ComboBoxItem item in cmbNewStatus.Items)
            {
                if (item.Content.ToString() == request.Status)
                {
                    cmbNewStatus.SelectedItem = item;
                    break;
                }
            }

            txtUpdateMessage.Text = "";
        }

        private void DisableStatusUpdate()
        {
            cmbNewStatus.IsEnabled = false;
            btnUpdateStatus.IsEnabled = false;
            cmbNewStatus.SelectedIndex = -1;
            txtUpdateMessage.Text = "";
        }

        // 🔹 UPDATED: Safe btnUpdateStatus_Click
        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = dgRequests.SelectedItem as ServiceRequest;
            if (selectedRequest == null)
            {
                MessageBox.Show("Please select a request before updating.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbNewStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a new status.", "Missing Status",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newStatus = (cmbNewStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            if (newStatus == selectedRequest.Status)
            {
                MessageBox.Show("The selected status is the same as the current status.", "No Change",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Change status of Request #{selectedRequest.IssueID}?\n\n" +
                $"From: {selectedRequest.Status}\n" +
                $"To: {newStatus}\n\n" +
                $"This will update the request for all users.",
                "Confirm Status Change",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    selectedRequest.Status = newStatus;
                    ServiceRequestManager.Instance.UpdateRequest(selectedRequest);

                    txtUpdateMessage.Text = $"✓ Status updated to '{newStatus}' successfully!";
                    txtUpdateMessage.Foreground = System.Windows.Media.Brushes.Green;

                    txtStatusBar.Text = $"✓ Request #{selectedRequest.IssueID} status changed: {selectedRequest.Status} → {newStatus}";

                    RefreshRequestsList();

                    var updatedRequest = filteredRequests.FirstOrDefault(r => r.IssueID == selectedRequest.IssueID);
                    if (updatedRequest != null)
                    {
                        dgRequests.SelectedItem = updatedRequest;
                        dgRequests.ScrollIntoView(updatedRequest);
                    }

                    MessageBox.Show(
                        $"Request #{selectedRequest.IssueID} status has been updated to '{newStatus}'.\n\n" +
                        "This change is now visible in the Service Request Status window.",
                        "Status Updated",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    txtUpdateMessage.Text = $"✗ Error: {ex.Message}";
                    txtUpdateMessage.Foreground = System.Windows.Media.Brushes.Red;

                    MessageBox.Show(
                        $"An error occurred while updating the status:\n\n{ex.Message}",
                        "Update Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
