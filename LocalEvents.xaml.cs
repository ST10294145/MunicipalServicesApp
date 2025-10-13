using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class LocalEvents : Window
    {
        private List<Event> allEvents;  // stores all events
        private List<string> eventSuggestions; // stores event titles for autocomplete

        public LocalEvents()
        {
            InitializeComponent();

            // Initialize data
            allEvents = new List<Event>
            {
                new Event { Title = "Community Clean-up", Category = "Environment", Date = new DateTime(2025, 10, 15), Description = "Join us for a clean-up event at Central Park." },
                new Event { Title = "Food Drive", Category = "Charity", Date = new DateTime(2025, 10, 20), Description = "Donate food items for the local shelter." },
                new Event { Title = "Youth Sports Day", Category = "Recreation", Date = new DateTime(2025, 10, 25), Description = "A fun day of sports for local youth." },
                new Event { Title = "Town Hall Meeting", Category = "Civic", Date = new DateTime(2025, 11, 1), Description = "Discuss community updates with the mayor." }
            };

            eventSuggestions = allEvents.Select(ev => ev.Title).ToList();

            dgEvents.ItemsSource = allEvents;
            cmbCategory.ItemsSource = allEvents.Select(ev => ev.Category).Distinct().ToList();
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
            // Null-safe check to avoid crashes
            if (txtSearch == null || lstSuggestions == null || allEvents == null)
                return;

            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText) || searchText == "search events...")
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            var matches = eventSuggestions
                .Where(s => s.ToLower().Contains(searchText))
                .ToList();

            lstSuggestions.ItemsSource = matches;
            lstSuggestions.Visibility = matches.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void lstSuggestions_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                txtSearch.Text = lstSuggestions.SelectedItem.ToString();
                lstSuggestions.Visibility = Visibility.Collapsed;
                PerformSearch();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (allEvents == null)
                return;

            string searchText = txtSearch.Text.ToLower();
            string selectedCategory = cmbCategory.SelectedItem as string;
            DateTime? selectedDate = dpDate.SelectedDate;

            var filtered = allEvents.Where(ev =>
                (string.IsNullOrEmpty(searchText) || ev.Title.ToLower().Contains(searchText) || ev.Description.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(selectedCategory) || ev.Category == selectedCategory) &&
                (!selectedDate.HasValue || ev.Date.Date == selectedDate.Value.Date)
            ).ToList();

            dgEvents.ItemsSource = filtered;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }

    public class Event
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
// testing done need to do reset search