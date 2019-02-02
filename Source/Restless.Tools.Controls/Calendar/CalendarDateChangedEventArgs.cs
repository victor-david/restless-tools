using System;
using System.Windows;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Represents the delegate that is used for <see cref="RestlessCalendar.SelectedDateUtcChangedEvent"/> routed event
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">The event arguments</param>
    public delegate void CalendarDateChangedEventHandler(object sender, CalendarDateChangedEventArgs e);

    /// <summary>
    /// Represents the routed event args set with a calendar changed event.
    /// </summary>
    public class CalendarDateChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets the changed date
        /// </summary>
        public DateTime? ChangedDate
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarDateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event</param>
        /// <param name="changedDate">The changed date</param>
        internal CalendarDateChangedEventArgs(RoutedEvent routedEvent, DateTime? changedDate) : base(routedEvent)
        {
            ChangedDate = changedDate;
        }
    }
}
