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
        // === Core data structures ===
        private List<EventItem> events = new List<EventItem>();                 // Main event list
        private SortedDictionary<DateTime, List<EventItem>> sortedEvents = new SortedDictionary<DateTime, List<EventItem>>(); // Organised by date
        private Queue<EventItem> upcomingEvents = new Queue<EventItem>();       // Queue for upcoming events
        private HashSet<string> eventCategories = new HashSet<string>();        // Unique categories
        private HashSet<string> searchHistory = new HashSet<string>();          // Unique past searches
        private Stack<string> recentSearches = new Stack<string>();             // Most recent search terms

        public LocalEvents()
        {
            InitializeComponent();
            LoadEvents();

            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbCategory.SelectionChanged += CmbCategory_SelectionChanged;
        }

        // === Load all events ===
        private void LoadEvents()
        {
            // Add some sample events
            events.Add(new EventItem("Community Cleanup", "Sanitation", "2025-10-05", "Join the community cleanup event."));
            events.Add(new EventItem("Road Safety Meeting", "Roads", "2025-10-12", "Discussion on road safety improvements."));
            events.Add(new EventItem("Water Supply Update", "Utilities", "2025-10-20", "Announcement about water supply upgrades."));
            events.Add(new EventItem("Park Renovation", "Community", "2025-11-02", "New park benches and garden upgrades."));
            events.Add(new EventItem("Electricity Maintenance", "Utilities", "2025-10-25", "Scheduled power maintenance notice."));

            // Fill SortedDictionary by date
            foreach (var ev in events)
            {
                DateTime date = DateTime.Parse(ev.Date);
                if (!sortedEvents.ContainsKey(date))
                    sortedEvents[date] = new List<EventItem>();
                sortedEvents[date].Add(ev);
            }

            // Fill Queue (upcoming events sorted by date)
            foreach (var ev in events.OrderBy(e => DateTime.Parse(e.Date)))
                upcomingEvents.Enqueue(ev);

            // Fill unique category set
            foreach (var ev in events)
                eventCategories.Add(ev.Category);

            // Display events in the DataGrid
            dgEvents.ItemsSource = events;

            // Populate category dropdown
            cmbCategory.Items.Add("All");
            foreach (var cat in eventCategories)
                cmbCategory.Items.Add(cat);
            cmbCategory.SelectedIndex = 0;
        }

        // === Filter events when search or category changes ===
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

            // Record the search
            if (!string.IsNullOrEmpty(searchText))
            {
                searchHistory.Add(searchText);
                recentSearches.Push(searchText);
            }
        }

        // === Suggestions dropdown ===
        private void ShowSuggestions(string text)
        {
            if (lstSuggestions == null)
                return;

            if (string.IsNullOrWhiteSpace(text) || text == "search events...")
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

        private void lstSuggestions_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                txtSearch.Text = lstSuggestions.SelectedItem.ToString();
                lstSuggestions.Visibility = Visibility.Collapsed;
                FilterEvents();
            }
        }

        // === Search box focus ===
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

        // === Show 3 most recent searches ===
        private void btnRecentSearches_Click(object sender, RoutedEventArgs e)
        {
            if (recentSearches.Count == 0)
            {
                MessageBox.Show("No recent searches yet.");
                return;
            }

            var recent = recentSearches.Take(3);
            MessageBox.Show("Recent Searches:\n" + string.Join("\n", recent));
        }

        // === Show recommendations based on last search ===
        private void btnRecommendations_Click(object sender, RoutedEventArgs e)
        {
            ShowRecommendations();
        }

        private void ShowRecommendations()
        {
            if (recentSearches.Count == 0)
            {
                MessageBox.Show("Search for events to get recommendations!");
                return;
            }

            string lastSearch = recentSearches.Peek().ToLower();
            var recommendations = events
                .Where(ev => ev.Title.ToLower().Contains(lastSearch) || ev.Description.ToLower().Contains(lastSearch))
                .Take(3)
                .ToList();

            if (recommendations.Count > 0)
            {
                string recText = string.Join("\n", recommendations.Select(r => "• " + r.Title));
                MessageBox.Show("Recommended Events based on your last search:\n" + recText);
            }
            else
            {
                MessageBox.Show("No recommendations found for your last search.");
            }
        }

        // === Show upcoming events using Queue ===
        private void btnUpcoming_Click(object sender, RoutedEventArgs e)
        {
            if (upcomingEvents.Count == 0)
            {
                MessageBox.Show("No upcoming events.");
                return;
            }

            var nextEvents = upcomingEvents.Take(3).Select(ev => $"{ev.Title} on {ev.Date}");
            MessageBox.Show("Next Upcoming Events:\n" + string.Join("\n", nextEvents));
        }

        // === Back to main menu ===
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    // === EventItem class ===
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
