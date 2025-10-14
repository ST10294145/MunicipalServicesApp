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
        private Dictionary<string, List<EventItem>> categoryDictionary = new Dictionary<string, List<EventItem>>();

        public LocalEvents()
        {
            InitializeComponent();
            LoadEvents();
            LoadCategories();
            dgEvents.ItemsSource = events;
        }

        // Load sample events
        private void LoadEvents()
        {
            events = new List<EventItem>
            {
                new EventItem { Title = "Community Clean-up Drive", Category = "Environment", Date = new DateTime(2025, 10, 20), Description = "Join the clean-up effort in your local park." },
                new EventItem { Title = "Town Hall Meeting", Category = "Community", Date = new DateTime(2025, 10, 25), Description = "Discuss new developments in your area." },
                new EventItem { Title = "Youth Sports Tournament", Category = "Sports", Date = new DateTime(2025, 11, 02), Description = "Annual soccer and netball tournament for youth." },
                new EventItem { Title = "Cultural Heritage Day", Category = "Culture", Date = new DateTime(2025, 11, 10), Description = "Celebrate diversity and culture with music and dance." },
                new EventItem { Title = "Tree Planting Initiative", Category = "Environment", Date = new DateTime(2025, 11, 15), Description = "Help plant trees to combat climate change." }
            };

            // Fill category dictionary
            foreach (var ev in events)
            {
                if (!categoryDictionary.ContainsKey(ev.Category))
                    categoryDictionary[ev.Category] = new List<EventItem>();

                categoryDictionary[ev.Category].Add(ev);
            }
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All");
            foreach (var cat in categoryDictionary.Keys)
                cmbCategory.Items.Add(cat);
            cmbCategory.SelectedIndex = 0;
        }

        // Search button logic
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterEvents();
        }

        // Reset all filters
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Search events...";
            txtSearch.Foreground = Brushes.Gray;
            cmbCategory.SelectedIndex = 0;
            dpDate.SelectedDate = null;
            lstSuggestions.Visibility = Visibility.Collapsed;
            lstSuggestions.ItemsSource = null;
            dgEvents.ItemsSource = events;
            lstRecommendations.ItemsSource = null;
        }

        private void FilterEvents()
        {
            if (events == null) return;

            string searchText = txtSearch.Text.ToLower();
            string selectedCategory = cmbCategory.SelectedItem?.ToString();
            DateTime? selectedDate = dpDate.SelectedDate;

            var filtered = events.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchText) && searchText != "search events...")
                filtered = filtered.Where(ev => ev.Title.ToLower().Contains(searchText) ||
                                                ev.Description.ToLower().Contains(searchText));

            if (selectedCategory != null && selectedCategory != "All")
                filtered = filtered.Where(ev => ev.Category == selectedCategory);

            if (selectedDate.HasValue)
                filtered = filtered.Where(ev => ev.Date.Date == selectedDate.Value.Date);

            dgEvents.ItemsSource = filtered.ToList();

            // Save search term
            if (!string.IsNullOrWhiteSpace(searchText) && searchText != "search events...")
            {
                searchHistory.Add(searchText);
                ShowRecommendations(searchText);
            }
        }

        private void ShowRecommendations(string keyword)
        {
            var recommendations = new List<EventItem>();

            foreach (var term in searchHistory)
            {
                foreach (var ev in events)
                {
                    if (ev.Title.ToLower().Contains(term) || ev.Description.ToLower().Contains(term))
                        if (!recommendations.Contains(ev))
                            recommendations.Add(ev);
                }
            }

            lstRecommendations.ItemsSource = recommendations.Take(5).ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (events == null || txtSearch.Text == "Search events...") return;
            ShowSuggestions(txtSearch.Text);
        }

        private void ShowSuggestions(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            var suggestions = events
                .Where(ev => ev.Title.ToLower().Contains(text.ToLower()))
                .Select(ev => ev.Title)
                .Distinct()
                .Take(5)
                .ToList();

            if (suggestions.Any())
            {
                lstSuggestions.ItemsSource = suggestions;
                lstSuggestions.Visibility = Visibility.Visible;
            }
            else
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
            }
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }

    // Event Data Model
    public class EventItem
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
