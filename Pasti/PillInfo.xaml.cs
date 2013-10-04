using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

// Own files
using Pasti.Resources;
using System.Windows.Media;

namespace Pasti
{
    public partial class PillInfo : PhoneApplicationPage
    {
        // Save the selected pill index
        private string selectedIndex = "";
        // Brain mechanism
        private Brain brain;

        public PillInfo()
        {
            InitializeComponent();

            // Translated AppBar
            BuildLocalizedApplicationBar();

            // Initialize the Brain
            brain = new Brain();

            // Set initial values
            // Today
            lblToday.Text = AppResources.TodayIs + " " + DateTime.Today.ToString("ddd, d MMMM");
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
            ApplicationBarIconButton btnDelete = new ApplicationBarIconButton(new Uri("/Assets/appbar.delete.rest.png", UriKind.Relative));
            btnDelete.Text = AppResources.Delete;
            ApplicationBar.Buttons.Add(btnDelete);
            // Attached event
            btnDelete.Click += new EventHandler(btnDelete_Click);

            // Cancel button
            ApplicationBarIconButton btnCancel = new ApplicationBarIconButton(new Uri("/Assets/appbar.back.rest.png", UriKind.Relative));
            btnCancel.Text = AppResources.Back;
            ApplicationBar.Buttons.Add(btnCancel);
            // Attached event
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        // Event for the Save button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete the item in the ViewModel.
            App.ViewModel.DeletePill(App.ViewModel.AllPills[int.Parse(selectedIndex)]);

            // Auto go back
            btnCancel_Click(null, null);
        }

        // Event for the Cancel button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Return to the main page.
            if (NavigationService.CanGoBack)
            {
                // Save the changes to the DB to persist or only the view will reflect the changes
                App.ViewModel.SaveChangesToDB();

                NavigationService.GoBack();
            }
        }

        // Actions when calling the page:
        // Get the selected item and set the DataContext so all data can be showed
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);
                DataContext = App.ViewModel.AllPills[index];
                // Make a initial refresh of the interface
                refreshInterface();
            }
        }

        // =====================================================================
        // Change the first day (from stepper buttons)
        private void changeFirstDay(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            DateTime firstDay = ((DateTime)dtStartDate.Value).Date.Add(((DateTime)dtStartDate.Value).TimeOfDay);

            if (btn.Name == "btnSubFirst")
            {
                dtStartDate.Value = firstDay.AddDays(-1);
            }
            else if (btn.Name == "btnAddFirst")
            {
                dtStartDate.Value = firstDay.AddDays(1);
            }
	        
            refreshInterface();
        }

        // Change the interval days (from stepper buttons)
        private void changeInterval(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int intervalDays = int.Parse(lblDays.Text);

            if (btn.Name == "btnSubDay")
            {
                intervalDays -= 1;
                if (intervalDays == 0)
                    intervalDays = 1;

            }
            else if (btn.Name == "btnAddDay")
            {
                intervalDays += 1;
                if (intervalDays > 31)
                    intervalDays = 31;
            }
            lblDays.Text = Convert.ToString(intervalDays);

            refreshInterface();
        }

        // Change the first day (from the picker directly)
        private void dtStartDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime != null)
                refreshInterface();
        }

        // ====================================================
        // Change the bgcolor and color of a label, depending if is active or not
        private void setDayStatus(Border border, TextBlock label, bool active) {
            Color blue = Color.FromArgb(255, 4, 124, 255); // "#FF047CFF"
            label.Foreground = active ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            border.Background = active ? new SolidColorBrush(blue) : new SolidColorBrush(Colors.LightGray);
        }

        // Refreshes all the interactive interface. Called at loading and when some parameter changes
        private void refreshInterface()
        {
            if (dtStartDate.Value == null || lblDays.Text == "")
                return;

            DateTime firstDay = ((DateTime)dtStartDate.Value).Date.Add(((DateTime)dtStartDate.Value).TimeOfDay);

	        // Configure the whole week (this will configure the brain vars)
	        brain.calculateWeek(firstDay, int.Parse(lblDays.Text));

	        // Is today a take day?
	        lblIsDay.Text = brain.take ? AppResources.IsDay : AppResources.IsNotDay;
	        setDayStatus(brdIsDay, lblIsDay, brain.take);

            setDayStatus(brdMon, lblMon, brain.week[1]);
            setDayStatus(brdTue, lblTue, brain.week[2]);
            setDayStatus(brdWed, lblWed, brain.week[3]);
            setDayStatus(brdThu, lblThu, brain.week[4]);
            setDayStatus(brdFri, lblFri, brain.week[5]);
            setDayStatus(brdSat, lblSat, brain.week[6]);
            setDayStatus(brdSun, lblSun, brain.week[7]);
        }

    }
}