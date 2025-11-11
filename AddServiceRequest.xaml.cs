using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class AddServiceRequest : Window
    {
        private string userEmail;

        public AddServiceRequest(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var request = new ServiceRequest
            {
                Title = txtTitle.Text,
                Category = txtCategory.Text,
                Priority = ((ComboBoxItem)cmbPriority.SelectedItem).Content.ToString(),
                Description = txtDescription.Text,
                Email = userEmail,
                Reporter = userEmail
            };

            RequestManager.Instance.AddRequest(request);
            MessageBox.Show("Service request submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}