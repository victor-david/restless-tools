using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides a control that is connected to a <see cref="RestlessCalendar"/> via a popup.
    /// </summary>
    [TemplatePart(Name = PartButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartCalendar, Type = typeof(RestlessCalendar))]
    public class RestlessPopupCalendar : ContentControl
    {
        #region Private
        private const string PartButton = "PART_Button";
        private const string PartPopup = "PART_Popup";
        private const string PartCalendar = "PART_Calendar";
        private ToggleButton toggleButton;
        private Popup popup;
        private RestlessCalendar calendar;

        //private static DateToFormattedDateConverter dateConverter = new DateToFormattedDateConverter();
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RestlessPopupCalendar"/> class.
        /// </summary>
        public RestlessPopupCalendar()
        {
            Background = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// Static constructor for <see cref="RestlessPopupCalendar"/>
        /// </summary>
        static RestlessPopupCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RestlessPopupCalendar), new FrameworkPropertyMetadata(typeof(RestlessPopupCalendar)));
        }
        #endregion

        /************************************************************************/

        #region Public properties
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
                nameof(IsUtcMode), typeof(bool), typeof(RestlessPopupCalendar), new PropertyMetadata(RestlessCalendar.IsUtcModeProperty.DefaultMetadata.DefaultValue)
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
                nameof(SelectedDateUtc), typeof(DateTime?), typeof(RestlessPopupCalendar),
                new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        /// <summary>
        /// Gets or sets the selected date
        /// </summary>
        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedDate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register
            (
                nameof(SelectedDate), typeof(DateTime?), typeof(RestlessPopupCalendar), 
                new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChange)
            );

        private static void OnSelectedDateChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessPopupCalendar control)
            {
                if (control.toggleButton != null)
                {
                    control.toggleButton.Content = e.NewValue;
                }
                if (control.popup != null)
                {
                    control.popup.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the placement mode for the popup
        /// </summary>
        public PlacementMode Placement
        {
            get => (PlacementMode)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Placement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register
            (
                nameof(Placement), typeof(PlacementMode), typeof(RestlessPopupCalendar), new PropertyMetadata(PlacementMode.Bottom)
            );

        /// <summary>
        /// Gets or sets the vertical offset for the popup.
        /// </summary>
        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register
            (
                nameof(VerticalOffset), typeof(double), typeof(RestlessPopupCalendar), new PropertyMetadata(0.0)
            );

        /// <summary>
        /// Gets or sets the horizontal offset for the popup.
        /// </summary>
        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register
            (
                nameof(HorizontalOffset), typeof(double), typeof(RestlessPopupCalendar), new PropertyMetadata(10.0)
            );
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Called when the control template is applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (popup != null) popup.Opened -= PopupOpened;
            if (calendar != null)
            {
                calendar.KeyUp -= CalendarKeyUp;
                calendar.DisplayDateChanged -= CalendarDisplayDateChanged;
            }

            toggleButton = GetTemplateChild(PartButton) as ToggleButton;
            popup = GetTemplateChild(PartPopup) as Popup;
            calendar = GetTemplateChild(PartCalendar) as RestlessCalendar;

            if (popup != null) popup.Opened += PopupOpened;
            if (calendar != null)
            {
                calendar.KeyUp += CalendarKeyUp;
                calendar.DisplayDateChanged += CalendarDisplayDateChanged;
                // Bit of a hack to get our event handler to fire the first time around.
                DateTime orig = calendar.DisplayDate;
                calendar.DisplayDate = DateTime.MinValue.AddYears(21);
                calendar.DisplayDate = orig;
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void PopupOpened(object sender, EventArgs e)
        {
            calendar?.Focus();
        }

        private void CalendarKeyUp(object sender, KeyEventArgs e)
        {
            if (popup != null && e.Key == Key.Escape)
            {
                popup.IsOpen = false;
            }
        }

        private void CalendarDisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            if (toggleButton != null)
            {
                if (e.AddedDate.HasValue)
                {
                    toggleButton.Content = e.AddedDate.Value.ToString(Default.Format.PopupCalendarDate);
                }
                else
                {
                    toggleButton.Content = "(no date)";
                }

            }
        }

        #endregion
    }
}
