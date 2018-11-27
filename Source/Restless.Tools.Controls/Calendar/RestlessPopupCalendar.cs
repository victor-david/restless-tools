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
        private object deferedUtcValue;
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
                nameof(IsUtcMode), typeof(bool), typeof(RestlessPopupCalendar), new PropertyMetadata(RestlessCalendar.IsUtcModeProperty.DefaultMetadata.DefaultValue, OnIsUtcModePropertyChanged)
            );

        private static void OnIsUtcModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessPopupCalendar control)
            {
                if (control.calendar != null)
                {
                    control.calendar.IsUtcMode = (bool)e.NewValue;
                }
            }
        }

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
                new FrameworkPropertyMetadata(default(DateTime?), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateUtcPropertyChanged)
            );

        private static void OnSelectedDateUtcPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessPopupCalendar control)
            {
                if (control.calendar != null)
                {
                    // Debug.WriteLine($"STATIC UTC CHANGED {e.NewValue}");
                    control.UpdateCalendarControls((DateTime?)e.NewValue);
                }
                else
                {
                    // Save value until template is ready.
                    control.deferedUtcValue = e.NewValue;
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
                calendar.SelectedDatesChanged -= CalendarSelectedDatesChanged;
                calendar.SelectedDateUtcChanged -= CalendarSelectedDateUtcChanged;
            }

            toggleButton = GetTemplateChild(PartButton) as ToggleButton;
            popup = GetTemplateChild(PartPopup) as Popup;
            calendar = GetTemplateChild(PartCalendar) as RestlessCalendar;

            if (popup != null) popup.Opened += PopupOpened;
            if (calendar != null)
            {
                calendar.KeyUp += CalendarKeyUp;
                calendar.SelectedDatesChanged += CalendarSelectedDatesChanged;
                calendar.SelectedDateUtcChanged += CalendarSelectedDateUtcChanged;
                if (deferedUtcValue != null)
                {
                    UpdateCalendarControls((DateTime?)deferedUtcValue);
                    deferedUtcValue = null;
                }
                else
                {
                    SetToggleButtonContent(calendar.DisplayDate);
                }
            }
        }
       
        /// <summary>
        /// Gets a string representation of this instance.
        /// </summary>
        /// <returns>A string that displays the type, SelectedDate, SelectedDateUtc, and DisplayDate</returns>
        public override string ToString()
        {
            return $"{GetType()} SelectedDateUtc: {SelectedDateUtc}";
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

        private void CalendarSelectedDateUtcChanged(object sender, CalendarDateChangedEventArgs e)
        {
            SelectedDateUtc = e.ChangedDate;
        }

        private void CalendarSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private void UpdateCalendarControls(DateTime? value)
        {
            calendar.SelectedDateUtc = value;
            SetToggleButtonContent(calendar.SelectedDate);
        }

        private void SetToggleButtonContent(DateTime? date)
        {
            if (toggleButton != null)
            {
                if (date.HasValue)
                {
                    toggleButton.Content = date.Value.ToString(Default.Format.PopupCalendarDate);
                }
                else
                {
                    toggleButton.Content = "(select a date)";
                }
            }
        }
        #endregion
    }
}
