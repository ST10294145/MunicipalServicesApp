using System.Windows;

namespace MunicipalServicesApp
{
    public partial class FeedbackWindow : Window
    {
        private Issue _issue;

        public FeedbackWindow(Issue issue)
        {
            InitializeComponent();
            _issue = issue;
            txtFeedback.Text = issue.Feedback; // Load old feedback if exists
        }

        private void SaveFeedback_Click(object sender, RoutedEventArgs e)
        {
            _issue.Feedback = txtFeedback.Text; // Save new feedback
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
