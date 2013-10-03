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
using Pasti.Resources;

namespace Pasti
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Translated AppBar
            BuildLocalizedApplicationBar();

            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
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

    }
}