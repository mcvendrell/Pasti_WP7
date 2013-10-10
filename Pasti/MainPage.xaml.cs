using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

// Directive for the ViewModel.
using LocalDatabaseSample.Model;

// Own controls
using Microsoft.Phone.Shell;
using System.Windows.Data;
using System.Globalization;
using Pasti.Resources;

namespace Pasti
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Holds the starting app day. If when going back to this page it has changed, refresh the list
        private DateTime loadDate;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Translated AppBar
            BuildLocalizedApplicationBar();

            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;

            // Starting day
            loadDate = DateTime.Today;
        }


        // Build a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Create the buttons and set the text value to the localized string from AppResources.
            // Add button
            ApplicationBarIconButton btnAddPill = new ApplicationBarIconButton(new Uri("/Assets/appbar.add.rest.png", UriKind.Relative));
            btnAddPill.Text = AppResources.Add;
            ApplicationBar.Buttons.Add(btnAddPill);
            // Attached event
            btnAddPill.Click += new EventHandler(btnAddPill_Click);
        }
        
        // Event for the Add button
        private void btnAddPill_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddPillPage.xaml", UriKind.Relative));
        }

        // Event for choosing a pill in the list
        private void lstPills_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If no index, cancel action
            if (lstPills.SelectedIndex == -1)
                return;

            // Go to the pill data page
            NavigationService.Navigate(new Uri("/PillInfo.xaml?selectedItem=" + lstPills.SelectedIndex, UriKind.Relative));

            // Restablish index
            lstPills.SelectedIndex = -1;
        }

        // Save the current day. If when going back to here it has changed, refresh the list
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            loadDate = DateTime.Today;
        }

        // Read the current day and compare with saved. If when going back to here it has changed, refresh the list
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Read the current day and compare
            if (loadDate != DateTime.Today)
            {
                // The day has changed. Loop the list to refresh every item
                foreach (PillItem item in lstPills.Items)
                {
                    item.CalculateIsToday();
                }
            }
        }

    }

    // Converter for the YES-NO column on the list: border background color
    public class IsTodayBackGroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var calculatedValue = (string)value;
            var color = calculatedValue == AppResources.NO ? Colors.LightGray : Color.FromArgb(255, 4, 124, 255); // "#FF047CFF"

            return new SolidColorBrush { Color = color };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for the YES-NO column on the list: text foreground color
    public class IsTodayForeGroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var calculatedValue = (string)value;
            var color = calculatedValue == AppResources.NO ? Colors.Black : Colors.White;

            return new SolidColorBrush { Color = color };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}