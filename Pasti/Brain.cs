using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pasti
{
    /*
     * This module exposes all the logic to calculate:
     * - From a passed day and interval, the current week with the specific days to take the pill
     * - From a passed total days (from a date) and a total interval, if is a day to take the pill or not
     * - Also has vars to know the state for the current week and day 
     */
    public class Brain
    {
        // Holds the seven week days (1 to 7, spanish format) with bool value
        private bool[] _week;

        public bool[] week
        {
            get { return _week; }
        }

        // Represent, for the current day, if is a day to take the pill
        private bool _take;

        public bool take
        {
            get { return _take; }
        }

        // Constructor
        public Brain()
        {
            _week = new bool[8];
            _take = false;
        }

        /*
         * This function returns the modulus between the passed total days (from a date) 
         * and the interval the user wants to check. If the result is 0, then the interval
         * is complete (so, in this case, it represents that the user must take the pill)
         */
        public static bool todayTake(int days, int interval)
        {
	        return (Math.Abs(days) % interval == 0 ? true : false);
        }

        /*
         * This function will use the configured pill data and will calculate the full week, marking
         * the days that the user must take the pill. The process will be:
         * 1. Calculate the spent days since the first take
         * 2. Get the current weekday
         * 3. Calculate for the current day if is a take day
         * 4. Configure the current week
         */
        public void calculateWeek(DateTime firstDay, int interval)
        {
            DateTime today = DateTime.Today;
	        // Get the days between today and the starting day
            int numDays = Math.Abs((today - firstDay).Days);
            
	        // Get the current weekday (Sun will be 7, not 0, spanish format)
	        int currentWeekday = (int)today.DayOfWeek;
            if (currentWeekday == 0)
                currentWeekday = 7;

            // Calculate for the current day if is a take day
	        this._take = todayTake(numDays, interval);
            
	        // Configure the current week
	        configureCurrentWeek(numDays, currentWeekday, interval);
        }

        /*
	     * This function will configure our week array for each weekday. The week have 7 days
	     * and will be in spanish format (starting week on Monday, not on Sunday). The process will be:
	     * 1. From the current spent days, find out the spent days for monday on this week.
	     * 2. Save for monday if is a take day. 
	     * 3. From it, just calculate each next week day
	     */
	    private void configureCurrentWeek(int numDays, int currentWeekday, int interval) {
		    // Get the days between today and the monday.
		    // For example, if today is Wed (value 3), and have passed 10 days,
            // in Mon only 8 days have passed, so 10 - (3 - 1) = 8
		    int numDaysOnMonday = numDays - (currentWeekday - 1);
            
		    // From here, configure each weekday on our week (remember, 1=Monday, 7=Sunday)
		    for (int i=1; i <= 7; i++) {
    		    this._week[i] = todayTake(numDaysOnMonday + (i - 1), interval);
	        }
	    }
    }
}
