using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

// Own files
using Pasti;
using Pasti.Resources;

namespace LocalDatabaseSample.Model
{

    public class PillDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public PillDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the Pills.
        public Table<PillItem> Items;
    }

    [Table]
    public class PillItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property, and database column.
        private int _pillId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int PillId
        {
            get { return _pillId; }
            set
            {
                if (_pillId != value)
                {
                    NotifyPropertyChanging("PillId");
                    _pillId = value;
                    NotifyPropertyChanged("PillId");
                }
            }
        }

        // Define item name: private field, public property, and database column.
        private string _pillName;

        [Column]
        public string PillName
        {
            get { return _pillName; }
            set
            {
                if (_pillName != value)
                {
                    NotifyPropertyChanging("PillName");
                    _pillName = value;
                    NotifyPropertyChanged("PillName");
                }
            }
        }

        // Define item days: private field, public property, and database column.
        private int _pillDays;

        [Column]
        public int PillDays
        {
            get { return _pillDays; }
            set
            {
                if (_pillDays != value)
                {
                    NotifyPropertyChanging("PillDays");
                    _pillDays = value;
                    NotifyPropertyChanged("PillDays");

                    // Also calculate the IsToday value here, to avoid recalculate it on every read
                    // (in case we do this process in its own property)
                    CalculateIsToday();
                }
            }
        }

        // Define item starting day: private field, public property, and database column.
        private DateTime _pillStart;

        [Column]
        public DateTime PillStart
        {
            get { return _pillStart; }
            set
            {
                if (_pillStart != value)
                {
                    NotifyPropertyChanging("PillStart");
                    _pillStart = value;
                    NotifyPropertyChanged("PillStart");

                    // Also calculate the IsToday value here, to avoid recalculate it on every read
                    // (in case we do this process in its own property)
                    CalculateIsToday();
                }
            }
        }

        // Define a custom field based on some database values
        // Get is calculated in the other fields, while set will force it to refresh by Notifying
        private string _isToday;
        public string IsToday {
            get { return _isToday; }
            set { NotifyPropertyChanged("IsToday"); }
        }

        // Function to calculate the IsToday value
        private void CalculateIsToday()
        {
            DateTime today = DateTime.Today;
            // Get the days between today and the starting day
            int numDays = Math.Abs((today - _pillStart).Days);

            _isToday = Brain.todayTake(numDays, _pillDays) ? AppResources.YES : AppResources.NO;
            NotifyPropertyChanged("IsToday");
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

}
