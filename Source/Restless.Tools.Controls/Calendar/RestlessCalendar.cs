using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides a calendar control with extended features.
    /// </summary>
    /// <remarks>
    /// This class extends System.Windows.Controls.Calendar to provide the ability to use 
    /// UTC dates as the backing while presenting the calendar controls using the local date.
    /// To activate, set <see cref="IsUtcMode"/> to true, and bind your date property to <see cref="SelectedDateUtc"/>.
    /// This class only permits <see cref=" CalendarSelectionMode.SingleDate"/>. Any attempt to set <see cref="Calendar.SelectionMode"/>
    /// to another value will be ignored.
    /// </remarks>
    public class RestlessCalendar : Calendar
    {
        #region Private
        private bool inSelectedDateChanged;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RestlessCalendar"/> class.
        /// </summary>
        public RestlessCalendar()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            DisplayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        /// <summary>
        /// Static constructor for <see cref="RestlessCalendar"/>
        /// </summary>
        static RestlessCalendar()
        {
            SelectionModeProperty.OverrideMetadata(typeof(RestlessCalendar), new FrameworkPropertyMetadata(CalendarSelectionMode.SingleDate, null, CoerceSelectionModeProperty));
        }

        private static object CoerceSelectionModeProperty(DependencyObject d, object baseValue)
        {
            return CalendarSelectionMode.SingleDate;
        }
        #endregion

        /************************************************************************/

        #region SelectedDateUtcChanged Routed Event
        /// <summary>
        /// Occurs when the <see cref="SelectedDateUtcChanged"/> property changes.
        /// </summary>
        public event CalendarDateChangedEventHandler SelectedDateUtcChanged
        {
            add => AddHandler(SelectedDateUtcChangedEvent, value);
            remove => RemoveHandler(SelectedDateUtcChangedEvent, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedDateUtcChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent SelectedDateUtcChangedEvent = EventManager.RegisterRoutedEvent
            (
                nameof(SelectedDateUtcChanged), RoutingStrategy.Bubble, typeof(CalendarDateChangedEventHandler), typeof(RestlessCalendar)
            );
        #endregion

        /************************************************************************/

        #region IsUtcMode
        /// <summary>
        /// Gets or sets a boolean value that determines if the calendar operates in UTC mode.
        /// </summary>
        public bool IsUtcMode
        {
            get => (bool)GetValue(IsUtcModeProperty);
            set => SetValue(IsUtcModeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsUtcMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsUtcModeProperty = DependencyProperty.Register
            (
                nameof(IsUtcMode), typeof(bool), typeof(RestlessCalendar), new PropertyMetadata(true)
            );

        #endregion

        /************************************************************************/

        #region SelectedDateUtc
        /// <summary>
        /// Gets or sets the selected date in UTC.
        /// </summary>
        public DateTime? SelectedDateUtc
        {
            get => (DateTime?)GetValue(SelectedDateUtcProperty);
            set => SetValue(SelectedDateUtcProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedDateUtc"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedDateUtcProperty = DependencyProperty.Register
            (
                nameof(SelectedDateUtc), typeof(DateTime?), typeof(RestlessCalendar),
                new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateUtcChanged)
            );

        private static void OnSelectedDateUtcChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessCalendar control)
            {
                if (e.NewValue is DateTime dt)
                {
                    DateTime dtLocal = control.ConvertIf(dt, toLocal: true);
                    control.SelectedDate = dtLocal;
                    if (!control.inSelectedDateChanged)
                    {
                        control.DisplayDate = dtLocal;
                    }
                }
                else
                {
                    control.SelectedDate = null;
                    if (!control.inSelectedDateChanged)
                    {
                        control.DisplayDate = DateTime.Now;
                    }
                }
                
                control.RaiseEvent(new CalendarDateChangedEventArgs(SelectedDateUtcChangedEvent, control.SelectedDateUtc));
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this instance.
        /// </summary>
        /// <returns>A string that displays the type, SelectedDate, SelectedDateUtc, and DisplayDate</returns>
        public override string ToString()
        {
            return $"{GetType()} SelectedDate: {SelectedDate} SelectedDateUtc: {SelectedDateUtc} DisplayDate: {DisplayDate}";
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Builds the visual tree for <see cref="RestlessCalendar"/> when a new template is applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Called when the PreviewMouseUp event is called. Prevents the calendar from "sticking".
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        /// <summary>
        /// Called when the selected date or dates change.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnSelectedDatesChanged(SelectionChangedEventArgs e)
        {
            inSelectedDateChanged = true;

            if (e.AddedItems.Count == 0)
            {
                SelectedDateUtc = null;
            }
            else
            {
                DateTime added = (DateTime)e.AddedItems[0];
                SelectedDateUtc = ConvertIf(added, toLocal: false);
            }

            inSelectedDateChanged = false;
            base.OnSelectedDatesChanged(e);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Converts the specified date if <see cref="IsUtcMode"/> is true.
        /// </summary>
        /// <param name="dt">The date to convert</param>
        /// <param name="toLocal">true to convert to local, false to convert to UTC</param>
        /// <returns>The converted date if <see cref="IsUtcMode"/> is true; otherwise, <paramref name="dt"/> unaltered</returns>
        private DateTime ConvertIf(DateTime dt, bool toLocal)
        {
            if (IsUtcMode)
            {
                if (toLocal)
                    return dt.ToLocalTime();
                else
                    return dt.ToUniversalTime();
            }
            return dt;
        }
        #endregion

    }
}

