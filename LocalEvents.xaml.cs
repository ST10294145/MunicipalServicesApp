using System.Collections.Generic;
using System.Windows;

namespace MunicipalServicesApp
{
    public partial class LocalEvents : Window
    {
        private List<EventItem> events = new List<EventItem>();

        public LocalEvents()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            events.Add(new EventItem("Community Cleanup", "Sanitation", "2025-10-05", "Join the community cleanup event."));
            events.Add(new EventItem("Road Safety Meeting", "Roads", "2025-10-12", "Discussion on road safety improvements."));
            events.Add(new EventItem("Water Supply Update", "Utilities", "2025-10-20", "Announcement about water supply upgrades."));

            dgEvents.ItemsSource = events;
            cmbCategory.Items.Add("Sanitation");
            cmbCategory.Items.Add("Roads");
            cmbCategory.Items.Add("Utilities");
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            string category = cmbCategory.SelectedItem?.ToString();
            var filtered = events.FindAll(ev =>
                (string.IsNullOrEmpty(searchText) || ev.Title.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(category) || ev.Category == category));

            dgEvents.ItemsSource = filtered;
        }
    }

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
