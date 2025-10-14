using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class LocalEvents : Window
    {
        private List<EventItem> allEvents = new List<EventItem>();
        private List<EventItem> filteredEvents = new List<EventItem>();
        private string currentUserRole;

        public LocalEvents(string role = "User")
        {
            InitializeComponent();
            currentUserRole = role;

            // Show Add Event button if admin
            if (currentUserRole == "Admin")
                btnAddEvent.Visibility = Visibility.Visible;

            LoadSampleEvents();
            PopulateCategories();
            RefreshEventList(allEvents);
        }

        // --- Step 1: Load initial sample events ---
        private void LoadSampleEvents()
        {
            allEvents = new List<EventItem>
            {
                new EventItem { Title="Community Clean-up", Category="Community", Date=new DateTime(2025,10,20), Description="Join us for a neighbourhood clean-up event." },
                new EventItem { Title="Tree Planting Drive", Category="Environment", Date=new DateTime(2025,11,01), Description="Help us plant trees around town to improve air quality." },
                new EventItem { Title="Water Awareness Seminar", Category="Education", Date=new DateTime(2025,11,15), Description="Learn about sustainable water usage and preservation." },
                new EventItem { Title="Local Market Day", Category="Business", Date=new DateTime(2025,10,25), Description="Support small local businesses and enjoy food, crafts, and music." }
            };
        }

        // --- Step 2: Populate categories dynamically ---
        private void PopulateCategories()
        {
            var categories = allEvents.Select(e => e.Category).Distinct().ToList();
            cmbCategory.ItemsSource = categories;
            cmbCategory.SelectedIndex = -1;
        }

        // --- Step 3: Update event grid display ---
        private void RefreshEventList(List<EventItem> list)
        {
            dgEvents.ItemsSource = null;
            dgEvents.ItemsSource = list;
        }

        // --- Step 4: Search button click ---
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower().Trim();
            string selectedCategory = cmbCategory.SelectedItem as string;
            DateTime? selectedDate = dpDate.SelectedDate;

            filteredEvents = allEvents
                .Where(ev =>
                    (string.IsNullOrEmpty(searchTerm) || ev.Title.ToLower().Contains(searchTerm) || ev.Description.ToLower().Contains(searchTerm)) &&
                    (string.IsNullOrEmpty(selectedCategory) || ev.Category == selectedCategory) &&
                    (!selectedDate.HasValue || ev.Date.Date == selectedDate.Value.Date))
                .ToList();

            RefreshEventList(filteredEvents);
        }

        // --- Step 5: Reset button click ---
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Search events...";
            txtSearch.Foreground = System.Windows.Media.Brushes.Gray;
            cmbCategory.SelectedIndex = -1;
            dpDate.SelectedDate = null;
            lstSuggestions.Visibility = Visibility.Collapsed;
            RefreshEventList(allEvents);
        }

        // --- Step 6: Search textbox logic ---
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

        // --- Step 7: Dynamic suggestions ---
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text == "Search events..." || string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            string search = txtSearch.Text.ToLower();
            var suggestions = allEvents
                .Where(ev => ev.Title.ToLower().Contains(search))
                .Select(ev => ev.Title)
                .Distinct()
                .ToList();

            if (suggestions.Count > 0)
            {
                lstSuggestions.ItemsSource = suggestions;
                lstSuggestions.Visibility = Visibility.Visible;
            }
            else
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
            }
        }

        // --- Step 8: Handle clicking a suggestion ---
        private void lstSuggestions_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                txtSearch.Text = lstSuggestions.SelectedItem.ToString();
                lstSuggestions.Visibility = Visibility.Collapsed;
                btnSearch_Click(null, null);
            }
        }

        // --- Step 9: Admin adds new event ---
        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEventWindow(allEvents);
            addWindow.ShowDialog();
            RefreshEventList(allEvents);
            PopulateCategories();
        }

        // --- Step 10: Back button ---
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(currentUserRole);
            main.Show();
            this.Close();
        }
    }

    // --- Supporting Event class ---
    public class EventItem
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
