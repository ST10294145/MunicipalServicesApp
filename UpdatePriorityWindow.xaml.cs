using System.Windows;

namespace MunicipalServicesApp
{
    public partial class UpdatePriorityWindow : Window
    {
        public string? NewPriority { get; private set; } = null;
        private string _currentPriority;

        public UpdatePriorityWindow(string currentPriority)
        {
            InitializeComponent();
            _currentPriority = currentPriority;
            CurrentPriorityText.Text = currentPriority;

            // Set current priority as selected
            foreach (var item in PriorityComboBox.Items)
            {
                if (item is System.Windows.Controls.ComboBoxItem comboItem &&
                    comboItem.Content.ToString() == currentPriority)
                {
                    PriorityComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (PriorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a priority.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewPriority = ((System.Windows.Controls.ComboBoxItem)PriorityComboBox.SelectedItem).Content.ToString();
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