using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MunicipalServicesApp
{
    public partial class LocalEvents : Window
    {
        private bool isAdmin;

        public class EventItem
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public string Date { get; set; }
            public string Description { get; set; }

            public EventItem(string title, string category, string date, string description)
            {
                Title = title;
                Category = category;
                Date = date;
                Description = description;
            }
        }

        private List<EventItem> allEvents = new List<EventItem>();

        public LocalEvents(bool admin = false)
        {
            InitializeComponent();
            isAdmin = admin;

            // Add dummy events for testing
            allEvents.Add(new EventItem("Community Cleanup", "Environment", "2025-10-20", "Join us to clean up the park!"));
            allEvents.Add(new EventItem("Health Workshop", "Health", "2025-10-22", "Learn about nutrition and fitness."));

            dgEvents.ItemsSource = allEvents;

            PopulateCategoryFilter();
        }

        private void PopulateCategoryFilter()
        {
            // Get all unique categories
            var categories = allEvents.Select(ev => ev.Category).Distinct().ToList();
            categories.Insert(0, "All Categories"); // optional default item
            cmbCategory.ItemsSource = categories;
            cmbCategory.SelectedIndex = 0;
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Access Denied. Only admins can add events.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddEventWindow addWindow = new AddEventWindow(allEvents); // pass current events
            addWindow.ShowDialog();

            // Refresh DataGrid and category filter after adding a new event
            dgEvents.ItemsSource = null;
            dgEvents.ItemsSource = allEvents;
            PopulateCategoryFilter();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search events...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search events...";
                txtSearch.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgEvents == null || allEvents == null) return;

            string search = txtSearch.Text.ToLower();
            IEnumerable<EventItem> filtered = allEvents;

            if (!string.IsNullOrWhiteSpace(search) && search != "search events...")
            {
                filtered = filtered.Where(ev => ev.Title.ToLower().Contains(search) || ev.Category.ToLower().Contains(search));
            }

            // Filter by category if selected and not "All Categories"
            if (cmbCategory.SelectedItem != null && cmbCategory.SelectedItem.ToString() != "All Categories")
            {
                string selectedCategory = cmbCategory.SelectedItem.ToString();
                filtered = filtered.Where(ev => ev.Category == selectedCategory);
            }

            dgEvents.ItemsSource = filtered.ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch_TextChanged(null, null);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Search events...";
            txtSearch.Foreground = System.Windows.Media.Brushes.Gray;
            cmbCategory.SelectedIndex = 0;
            dpDate.SelectedDate = null;
            dgEvents.ItemsSource = allEvents;
        }
    }
}
