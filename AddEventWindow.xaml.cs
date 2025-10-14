using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MunicipalServicesApp
{
    public partial class AddEventWindow : Window
    {
        private List<EventItem> allEvents; // reference to the list in LocalEvents

        public AddEventWindow(List<EventItem> events)
        {
            InitializeComponent();
            allEvents = events;

            // Initialize placeholder text
            txtTitle.Text = "Event Title";
            txtTitle.Foreground = Brushes.Gray;

            txtCategory.Text = "Category";
            txtCategory.Foreground = Brushes.Gray;

            txtDescription.Text = "Description";
            txtDescription.Foreground = Brushes.Gray;

            // Attach GotFocus / LostFocus events
            txtTitle.GotFocus += TextBox_GotFocus;
            txtTitle.LostFocus += TextBox_LostFocus;
            txtCategory.GotFocus += TextBox_GotFocus;
            txtCategory.LostFocus += TextBox_LostFocus;
            txtDescription.GotFocus += TextBox_GotFocus;
            txtDescription.LostFocus += TextBox_LostFocus;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Event Title")
            {
                MessageBox.Show("Enter a valid Title.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCategory.Text) || txtCategory.Text == "Category")
            {
                MessageBox.Show("Enter a valid Category.");
                return;
            }
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Select a valid Date.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text == "Description")
            {
                MessageBox.Show("Enter a valid Description.");
                return;
            }

            // Add event to list
            EventItem newEvent = new EventItem(
                txtTitle.Text,
                txtCategory.Text,
                dpDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                txtDescription.Text
            );

            allEvents.Add(newEvent);
            MessageBox.Show("Event added successfully!");

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Foreground == Brushes.Gray)
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Foreground = Brushes.Gray;
                if (tb.Name == "txtTitle") tb.Text = "Event Title";
                if (tb.Name == "txtCategory") tb.Text = "Category";
                if (tb.Name == "txtDescription") tb.Text = "Description";
            }
        }
    }

    // Simple EventItem class
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
