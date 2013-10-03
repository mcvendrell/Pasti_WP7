using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

// Directive for the data model.
using LocalDatabaseSample.Model;


namespace LocalDatabaseSample.ViewModel
{
    public class PillsVM : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private PillDataContext pillsDB;

        // Class constructor, create the data context object.
        public PillsVM(string pillsDBConnectionString)
        {
            pillsDB = new PillDataContext(pillsDBConnectionString);
        }

        // All Pills.
        private ObservableCollection<PillItem> _allPills;
        public ObservableCollection<PillItem> AllPills
        {
            get { return _allPills; }
            set
            {
                _allPills = value;
                NotifyPropertyChanged("AllPills");
            }
        }

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            pillsDB.SubmitChanges();
        }

        // Query database and load the collections and list used by the pages.
        public void LoadCollectionsFromDatabase()
        {
            // Specify the query for all Pills in the database.
            var pillsInDB = from PillItem pill in pillsDB.Items
                                select pill;

            // Query the database and load all Pills.
            AllPills = new ObservableCollection<PillItem>(pillsInDB);
        }

        // Add a Pill to the database and collections.
        public void AddPill(PillItem newPill)
        {
            // Add a Pill to the data context.
            pillsDB.Items.InsertOnSubmit(newPill);

            // Save changes to the database.
            pillsDB.SubmitChanges();

            // Add a Pill to the "all" observable collection.
            AllPills.Add(newPill);
        }

        // Remove a Pill from the database and collections.
        public void DeletePill(PillItem pillsForDelete) {
            // Remove the Pill from the "all" observable collection.
            AllPills.Remove(pillsForDelete);

            // Remove the Pill from the data context.
            pillsDB.Items.DeleteOnSubmit(pillsForDelete);

            // Save changes to the database.
            pillsDB.SubmitChanges();
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
