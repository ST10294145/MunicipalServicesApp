using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MunicipalServicesApp
{
    public partial class LocalEvents : Window
    {
        private List<EventItem> allEvents = new List<EventItem>();
        private bool isAdmin = false;

        public LocalEvents(bool adminMode = false)
        {
            InitializeComponent();
            isAdmin = adminMode;

            if (isAdmin)
            {
                btnAddEvent.Visibility = Visibility.Visible;
            }

            // Sample events
            allEvents.Add(new EventItem("Community Cleanup", "Environment", "2025-10-20", "Join us for a community cleanup."));
            allEvents.Add(new EventItem("Health Awareness Talk", "Health", "2025-10-22", "Health awareness event."));
            allEvents.Add(new EventItem("Local Music Festival", "Entertainment", "2025-10-25", "Live music festival in town."));

            dgEvents.ItemsSource = allEvents;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            AddEventWindow addWindow = new AddEventWindow(allEvents);
            addWindow.ShowDialog();
            dgEvents.Items.Refresh();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            string selectedCategory = cmbCategory.SelectedItem?.ToString();
            DateTime? selectedDate = dpDate.SelectedDate;

            var filtered = allEvents.Where(ev =>
                (string.IsNullOrWhiteSpace(searchText) || ev.Title.ToLower().Contains(searchText)) &&
                (selectedCategory == null || ev.Category == selectedCategory) &&
                (!selectedDate.HasValue || ev.Date == selectedDate.Value.ToString("yyyy-MM-dd"))
            ).ToList();

            dgEvents.ItemsSource = filtered;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            cmbCategory.SelectedIndex = -1;
            dpDate.SelectedDate = null;
            dgEvents.ItemsSource = allEvents;
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Foreground == Brushes.Gray)
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search events...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Optional: live suggestions can be implemented here
        }

        private void lstSuggestions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Optional: suggestion click logic
        }
    }
}
