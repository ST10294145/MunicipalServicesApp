using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MunicipalServicesApp
{
    public partial class LocalEvents : Window
    {
        private List<EventItem> events = new List<EventItem>();

        public LocalEvents()
        {
            InitializeComponent();
            LoadEvents();

            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbCategory.SelectionChanged += CmbCategory_SelectionChanged;
        }

        private void LoadEvents()
        {
            // Sample events
            events.Add(new EventItem("Community Cleanup", "Sanitation", "2025-10-05", "Join the community cleanup event."));
            events.Add(new EventItem("Road Safety Meeting", "Roads", "2025-10-12", "Discussion on road safety improvements."));
            events.Add(new EventItem("Water Supply Update", "Utilities", "2025-10-20", "Announcement about water supply upgrades."));

            // Bind to DataGrid
            dgEvents.ItemsSource = events;

            // Populate category filter
            cmbCategory.Items.Add("Sanitation");
            cmbCategory.Items.Add("Roads");
            cmbCategory.Items.Add("Utilities");

            cmbCategory.SelectionChanged += CmbCategory_SelectionChanged;
        }

        // Live search as you type
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEvents();
        }

        // Filter when category changes
        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents();
        }

        // Handles filtering logic
        private void FilterEvents()
        {
            if (txtSearch == null || cmbCategory == null || dgEvents == null)
                return;

            string searchText = txtSearch.Text.ToLower();

            if (searchText == "search events...") searchText = "";

            string category = cmbCategory.SelectedItem?.ToString();

            var filtered = events.FindAll(ev =>
                (string.IsNullOrEmpty(searchText) || ev.Title.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(category) || ev.Category == category));

            dgEvents.ItemsSource = filtered;
        }

        // Placeholder logic
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search events...")
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterEvents(); // Calls the same filter method as live search
        }
    }

    // Event class
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
}
