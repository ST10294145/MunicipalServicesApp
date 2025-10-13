using System;
using System.Collections.Generic;
using System.Linq;
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

            // Hook up events safely (controls are initialized here)
            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbCategory.SelectionChanged += CmbCategory_SelectionChanged;
            dpDate.SelectedDateChanged += DpDate_SelectedDateChanged;
        }

        private void LoadEvents()
        {
            // Sample dummy events data
            events.Add(new EventItem("Community Cleanup", "Sanitation", "2025-10-05", "Join the community cleanup event."));
            events.Add(new EventItem("Road Safety Meeting", "Roads", "2025-10-12", "Discussion on road safety improvements."));
            events.Add(new EventItem("Water Supply Update", "Utilities", "2025-10-20", "Announcement about water supply upgrades."));
            events.Add(new EventItem("Park Restoration", "Sanitation", "2025-10-25", "Help clean and restore the central park."));
            events.Add(new EventItem("Streetlight Maintenance", "Utilities", "2025-11-01", "Information about power maintenance."));

            // Bind to DataGrid
            dgEvents.ItemsSource = events;

            // Populate category filter
            cmbCategory.Items.Add("All Categories");
            cmbCategory.Items.Add("Sanitation");
            cmbCategory.Items.Add("Roads");
            cmbCategory.Items.Add("Utilities");
            cmbCategory.SelectedIndex = 0;
        }

        // Live search as you type
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowSuggestions(txtSearch.Text);
            FilterEvents();
        }

        // Filter when category changes
        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents();
        }

        // Filter when date changes
        private void DpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents();
        }

        // Main filtering logic
        private void FilterEvents()
        {
            if (events == null || events.Count == 0)
                return;

            string searchText = txtSearch.Text.ToLower();
            if (searchText == "search events...") searchText = "";

            string category = cmbCategory.SelectedItem?.ToString();
            DateTime? selectedDate = dpDate.SelectedDate;

            var filtered = events.Where(ev =>
                (string.IsNullOrEmpty(searchText) || ev.Title.ToLower().Contains(searchText)) &&
                (category == "All Categories" || string.IsNullOrEmpty(category) || ev.Category == category) &&
                (!selectedDate.HasValue || ev.Date == selectedDate.Value.ToString("yyyy-MM-dd"))
            ).ToList();

            dgEvents.ItemsSource = filtered;
            GenerateRecommendations(searchText);
        }

        // Show smart search suggestions
        private void ShowSuggestions(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text == "Search events...")
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            var matches = events
                .Where(ev => ev.Title.ToLower().Contains(text.ToLower()))
                .Select(ev => ev.Title)
                .Distinct()
                .Take(5)
                .ToList();

            lstSuggestions.ItemsSource = matches;
            lstSuggestions.Visibility = matches.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        // Click suggestion to auto-fill search box
        private void lstSuggestions_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                txtSearch.Text = lstSuggestions.SelectedItem.ToString();
                lstSuggestions.Visibility = Visibility.Collapsed;
                FilterEvents();
            }
        }

        // Recommendations logic
        private void GenerateRecommendations(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                lstRecommendations.ItemsSource = null;
                return;
            }

            var recs = events
                .Where(ev => ev.Description.ToLower().Contains(searchText.ToLower()))
                .Select(ev => ev.Title)
                .Distinct()
                .Take(3)
                .ToList();

            lstRecommendations.ItemsSource = recs;
        }

        // Placeholder text behavior
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
            FilterEvents();
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
