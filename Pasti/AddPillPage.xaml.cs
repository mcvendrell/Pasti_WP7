using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

// Directive for the data model.
using LocalDatabaseSample.Model;

// Own files
using Pasti.Resources;

namespace Pasti
{
    public partial class AddPillPage : PhoneApplicationPage
    {
        public AddPillPage()
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
            ApplicationBarIconButton btnOk = new ApplicationBarIconButton(new Uri("/Assets/appbar.check.rest.png", UriKind.Relative));
            btnOk.Text = AppResources.Save;
            ApplicationBar.Buttons.Add(btnOk);
            // Attached event
            btnOk.Click += new EventHandler(btnOk_Click);

            // Cancel button
            ApplicationBarIconButton btnCancel = new ApplicationBarIconButton(new Uri("/Assets/appbar.cancel.rest.png", UriKind.Relative));
            btnCancel.Text = AppResources.Cancel;
            ApplicationBar.Buttons.Add(btnCancel);
            // Attached event
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        // Event for the Save button
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Confirm there is some text in the text boxes.
            if (txtName.Text.Length == 0 || txtDays.Text.Length == 0)
            {
                MessageBox.Show(AppResources.FillAllFields);
            } 
            else
            {
                // Create a new pill item.
                PillItem newPill = new PillItem
                {
                    PillName = txtName.Text,
                    PillDays = Convert.ToInt32(txtDays.Text),
                    PillStart = Convert.ToString(dtStartDate.Value)
                };

                // Add the item to the ViewModel.
                App.ViewModel.AddPill(newPill);

                // Return to the main page.
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }

        // Event for the Cancel button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Return to the main page.
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // Ensure that the Days field is only number allowed (allow only digits)
        private void txtDays_KeyDown(object sender, KeyEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}