using System.Linq;
using System.Windows;

namespace MunicipalServicesApp
{
    public partial class AdminServiceRequests : Window
    {
        public AdminServiceRequests()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            dgRequests.ItemsSource = RequestManager.Instance.GetAllRequests();
            dgRequests.Items.Refresh();
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is ServiceRequest selected)
            {
                var statusWindow = new UpdateStatusWindow(selected.Status);
                if (statusWindow.ShowDialog() == true && statusWindow.NewStatus != null)
                {
                    RequestManager.Instance.UpdateRequest(selected, statusWindow.NewStatus, selected.Priority);
                    LoadRequests();
                }
            }
            else
            {
                MessageBox.Show("Please select a request first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdatePriority_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is ServiceRequest selected)
            {
                var priorityWindow = new UpdatePriorityWindow(selected.Priority);
                if (priorityWindow.ShowDialog() == true && priorityWindow.NewPriority != null)
                {
                    RequestManager.Instance.UpdateRequest(selected, selected.Status, priorityWindow.NewPriority);
                    LoadRequests();
                }
            }
            else
            {
                MessageBox.Show("Please select a request first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }
    }
}