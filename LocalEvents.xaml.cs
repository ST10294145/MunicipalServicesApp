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
        private HashSet<string> searchHistory = new HashSet<string>();
        private Stack<string> recentSearches = new Stack<string>();

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
            events.Add(new EventItem("Park Renovation", "Community", "2025-11-02", "New park benches and garden upgrades."));

            dgEvents.ItemsSource = events;

            // Populate category filter
            cmbCategory.Items.Add("All");
            cmbCategory.Items.Add("Sanitation");
            cmbCategory.Items.Add("Roads");
            cmbCategory.Items.Add("Utilities");
            cmbCategory.Items.Add("Community");
            cmbCategory.SelectedIndex = 0;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            if (searchText == "search events...") return;

            ShowSuggestions(searchText);
            FilterEvents();
        }

        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents();
        }

        private void FilterEvents()
        {
            if (txtSearch == null || cmbCategory == null || dgEvents == null)
                return;

            string searchText = txtSearch.Text.ToLower();
            string category = cmbCategory.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(searchText) || searchText == "search events...")
                searchText = "";

            var filtered = events.Where(ev =>
                (string.IsNullOrEmpty(searchText) || ev.Title.ToLower().Contains(searchText)) &&
                (category == "All" || string.IsNullOrEmpty(category) || ev.Category == category)
            ).ToList();

            dgEvents.ItemsSource = filtered;

            // Record search
            if (!string.IsNullOrEmpty(searchText))
            {
                searchHistory.Add(searchText);
                recentSearches.Push(searchText);
            }
        }

        // ✅ Fixed ShowSuggestions method (prevents NullReferenceException)
        private void ShowSuggestions(string text)
        {
            if (lstSuggestions == null)
                return;

            if (string.IsNullOrWhiteSpace(text) || text == "search events...")
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            if (events == null)
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            var matches = events
                .Where(ev => !string.IsNullOrEmpty(ev.Title) && ev.Title.ToLower().Contains(text.ToLower()))
                .Select(ev => ev.Title)
                .Distinct()
                .Take(5)
                .ToList();

            lstSuggestions.ItemsSource = matches;
            lstSuggestions.Visibility = matches.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void lstSuggestions_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                txtSearch.Text = lstSuggestions.SelectedItem.ToString();
                lstSuggestions.Visibility = Visibility.Collapsed;
                FilterEvents();
            }
        }

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

        private void ShowRecentSearches()
        {
            var recent = recentSearches.Take(3);
            MessageBox.Show("Recent Searches:\n" + string.Join("\n", recent));
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
