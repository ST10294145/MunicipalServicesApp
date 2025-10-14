using System;
using System.Collections.Generic;
using System.Windows;

namespace MunicipalServicesApp
{
    public partial class AddEventWindow : Window
    {
        private List<EventItem> eventList;

        // ✅ Constructor that accepts event list (fixes your CS1729)
        public AddEventWindow(List<EventItem> allEvents)
        {
            InitializeComponent();
            eventList = allEvents;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text) ||
                dpDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please fill in all fields before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newEvent = new EventItem
            {
                Title = txtTitle.Text.Trim(),
                Category = txtCategory.Text.Trim(),
                Date = dpDate.SelectedDate.Value,
                Description = txtDescription.Text.Trim()
            };

            eventList.Add(newEvent);
            MessageBox.Show("New event added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
