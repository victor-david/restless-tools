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
        }
        #endregion

        /************************************************************************/

        #region Properties
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
            if (d is RestlessCalendar c)
            {
                if (e.NewValue is DateTime dt)
                {
                    DateTime dtLocal = c.ConvertIf(dt, toLocal: true);
                    c.SelectedDate = dtLocal;
                    if (!c.inSelectedDateChanged)
                    {
                        c.DisplayDate = dtLocal;
                    }
                }
                else
                {
                    c.SelectedDate = null;
                    if (!c.inSelectedDateChanged)
                    {
                        c.DisplayDate = DateTime.Now;
                    }
                }
            }
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
        /// Called when the SelectionMode property changes
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnSelectionModeChanged(EventArgs e)
        {
            base.OnSelectionModeChanged(e);
            SelectionMode = CalendarSelectionMode.SingleDate;
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
            base.OnSelectedDatesChanged(e);
            inSelectedDateChanged = true;

            if (SelectionMode == CalendarSelectionMode.SingleDate)
            {
                if (e.AddedItems.Count == 0)
                {
                    SelectedDateUtc = null;
                }
                else
                {
                    DateTime added = (DateTime)e.AddedItems[0];
                    SelectedDateUtc = ConvertIf(added, toLocal: false);
                }
            }
            inSelectedDateChanged = false;
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

