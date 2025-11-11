using System.Windows;

namespace MunicipalServicesApp
{
    public partial class UpdateStatusWindow : Window
    {
        public string? NewStatus { get; private set; } = null;
        private string _currentStatus;

        public UpdateStatusWindow(string currentStatus)
        {
            InitializeComponent();
            _currentStatus = currentStatus;
            CurrentStatusText.Text = currentStatus;

            // Set current status as selected
            foreach (var item in StatusComboBox.Items)
            {
                if (item is System.Windows.Controls.ComboBoxItem comboItem &&
                    comboItem.Content.ToString() == currentStatus)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewStatus = ((System.Windows.Controls.ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}