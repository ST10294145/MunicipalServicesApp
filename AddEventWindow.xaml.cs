using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MunicipalServicesApp
{
    public partial class AddEventWindow : Window
    {
        private List<LocalEvents.EventItem> events;

        public AddEventWindow(List<LocalEvents.EventItem> events)
        {
            InitializeComponent();
            this.events = events;
        }

        // Save button click
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Event Title" ||
                string.IsNullOrWhiteSpace(txtCategory.Text) || txtCategory.Text == "Category" ||
                dpDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text == "Description")
            {
                MessageBox.Show("Please fill in all fields.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add new event
            events.Add(new LocalEvents.EventItem(
                txtTitle.Text,
                txtCategory.Text,
                dpDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                txtDescription.Text
            ));

            this.Close();
        }

        // Cancel button click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Placeholder behavior for TextBoxes
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && (tb.Text == "Event Title" || tb.Text == "Category" || tb.Text == "Description"))
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                if (tb.Name == "txtTitle")
                    tb.Text = "Event Title";
                else if (tb.Name == "txtCategory")
                    tb.Text = "Category";
                else if (tb.Name == "txtDescription")
                    tb.Text = "Description";

                tb.Foreground = Brushes.Gray;
            }
        }
    }
}
