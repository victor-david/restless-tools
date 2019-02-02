using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides a simple control that displays key / value pairs.
    /// </summary>
    public class KeyValue : ContentControl
    {
        #region Private
        private const double DefaultHeaderWidth = 120.0;
        #endregion
        
        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValue"/> class.
        /// </summary>
        public KeyValue()
        {
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        static KeyValue()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyValue), new FrameworkPropertyMetadata(typeof(KeyValue)));
        }
        #endregion

        /************************************************************************/

        #region ValueChanged routed event
        /// <summary>
        /// Occurs when the <see cref="Value"/> property changes.
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        /// <summary>
        /// Identifies the <see cref="ValueChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent
            (
                nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(KeyValue)
            );

        #endregion

        /************************************************************************/

        #region Header
        /// <summary>
        /// Gets or sets the control header.
        /// </summary>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that displays the control header.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register
            (
                nameof(Header), typeof(string), typeof(KeyValue), new UIPropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region HeaderWidth
        /// <summary>
        /// Gets or sets the width of <see cref="Header"/>.
        /// </summary>
        public double HeaderWidth
        {
            get => (double)GetValue(HeaderWidthProperty);
            set => SetValue(HeaderWidthProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that controls the width of <see cref="Header"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderWidthProperty = DependencyProperty.Register
            (
                nameof(HeaderWidth), typeof(double), typeof(KeyValue), new UIPropertyMetadata(DefaultHeaderWidth, HeaderWidthChanged, CoerceHeaderWidth)
            );

        private static void HeaderWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyValue control)
            {
                control.HeaderGridWidthInternal = new GridLength((double)e.NewValue);
            }
        }

        private static object CoerceHeaderWidth(DependencyObject d, object baseValue)
        {
            if (baseValue is double w)
            {
                return Math.Max(w, 80);
            }
            return baseValue;
        }

        /// <summary>
        /// Gets or sets (from this asembly) the GridLength that controls the header width.
        /// </summary>
        internal GridLength HeaderGridWidthInternal
        {
            get => (GridLength)GetValue(HeaderGridWidthInternalProperty);
            set => SetValue(HeaderGridWidthInternalProperty, value);
        }
        
        internal static readonly DependencyProperty HeaderGridWidthInternalProperty = DependencyProperty.Register
            (
                nameof(HeaderGridWidthInternal), typeof(GridLength), typeof(KeyValue), new PropertyMetadata(new GridLength(DefaultHeaderWidth))
            );

        /// <summary>
        /// Gets or sets (from this assembly) the Thickness that controls the header margin.
        /// </summary>
        internal Thickness HeaderMarginInternal
        {
            get => (Thickness)GetValue(HeaderMarginInternalProperty);
            set => SetValue(HeaderMarginInternalProperty, value);
        }

        internal static readonly DependencyProperty HeaderMarginInternalProperty = DependencyProperty.Register
            (
                nameof(HeaderMarginInternal), typeof(Thickness), typeof(KeyValue), new PropertyMetadata(new Thickness(0))
            );
        #endregion

        /************************************************************************/

        #region HeaderForegound
        /// <summary>
        /// Gets or sets the foregound of <see cref="Header"/>.
        /// </summary>
        public Brush HeaderForeground
        {
            get => (Brush)GetValue(HeaderForegroundProperty);
            set => SetValue(HeaderForegroundProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the foreground of <see cref="Header"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register
            (
                nameof(HeaderForeground), typeof(Brush), typeof(KeyValue), new UIPropertyMetadata(new SolidColorBrush(Colors.Black))
            );
        #endregion

        /************************************************************************/

        #region HeaderFontSize

        /// <summary>
        /// Gets or sets the font size of <see cref="Header"/>.
        /// </summary>
        public double HeaderFontSize
        {
            get => (double)GetValue(HeaderFontSizeProperty);
            set => SetValue(HeaderFontSizeProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the font size of <see cref="Header"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register
            (
                nameof(HeaderFontSize), typeof(double), typeof(KeyValue), new UIPropertyMetadata(11.0)
            );
        #endregion

        /************************************************************************/

        #region HeaderVerticalAlignment
        /// <summary>
        /// Gets or sets the vertical alignment for the header
        /// </summary>
        public VerticalAlignment HeaderVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(HeaderVerticalAlignmentProperty);
            set => SetValue(HeaderVerticalAlignmentProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the vertical alignment of the header.
        /// </summary>
        public static readonly DependencyProperty HeaderVerticalAlignmentProperty = DependencyProperty.Register
            (
                nameof(HeaderVerticalAlignment), typeof(VerticalAlignment), typeof(KeyValue), new PropertyMetadata(VerticalAlignment.Center)
            );
        #endregion

        /************************************************************************/

        #region Value
        /// <summary>
        /// Gets or sets the display value.
        /// </summary>
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that displays the control value.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
            (
                nameof(Value), typeof(object), typeof(KeyValue), new UIPropertyMetadata(null, OnValueChanged)
            );

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyValue control)
            {
                control.OnValueChanged(new RoutedEventArgs(ValueChangedEvent));
            }
        }
        #endregion

        /************************************************************************/

        #region ValueForeground
        /// <summary>
        /// Gets or sets the foregound of <see cref="Value"/>.
        /// </summary>
        public Brush ValueForeground
        {
            get => (Brush)GetValue(ValueForegroundProperty);
            set => SetValue(ValueForegroundProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the foreground of <see cref="Value"/>.
        /// </summary>
        public static readonly DependencyProperty ValueForegroundProperty = DependencyProperty.Register
            (
                nameof(ValueForeground), typeof(Brush), typeof(KeyValue), new UIPropertyMetadata(new SolidColorBrush(Colors.Black))
            );
        #endregion

        /************************************************************************/

        #region ValueFontSize
        /// <summary>
        /// Gets or sets the font size of <see cref="Value"/>.
        /// </summary>
        public double ValueFontSize
        {
            get => (double)GetValue(ValueFontSizeProperty);
            set => SetValue(ValueFontSizeProperty, value);
        }

        /// <summary>
        /// Defines a dependency property for the value font size.
        /// </summary>
        public static readonly DependencyProperty ValueFontSizeProperty = DependencyProperty.Register
            (
                nameof(ValueFontSize), typeof(double), typeof(KeyValue), new PropertyMetadata(11.0)
            );

        #endregion

        /************************************************************************/

        #region ValueVerticalAlignment
        /// <summary>
        /// Gets or sets the vertical alignment for the value
        /// </summary>
        public VerticalAlignment ValueVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(ValueVerticalAlignmentProperty);
            set => SetValue(ValueVerticalAlignmentProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the vertical alignment of the value.
        /// </summary>
        public static readonly DependencyProperty ValueVerticalAlignmentProperty = DependencyProperty.Register
            (
                nameof(ValueVerticalAlignment), typeof(VerticalAlignment), typeof(KeyValue), new PropertyMetadata(VerticalAlignment.Center)
            );
        #endregion

        /************************************************************************/

        #region DisplayLevel
        /// <summary>
        /// Gets or sets the display level of this control. This property affects
        /// the margin and header width to provide an indent
        /// </summary>
        public int DisplayLevel
        {
            get => (int)GetValue(DisplayLevelProperty);
            set => SetValue(DisplayLevelProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the display level.
        /// </summary>
        public static readonly DependencyProperty DisplayLevelProperty = DependencyProperty.Register
            (
                nameof(DisplayLevel), typeof(int), typeof(KeyValue), new UIPropertyMetadata(0, DisplayLevelChanged, CoerceDisplayLevel)
            );

        private static void DisplayLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            if (d is KeyValue kv)
            {
                int levelIndent = 12;
                int displayLevel = (int)e.NewValue;
                double left = levelIndent * displayLevel;
                kv.HeaderMarginInternal = new Thickness(left, 0, 0, 0);
            }
        }

        private static object CoerceDisplayLevel(DependencyObject d, object baseValue)
        {
            if (baseValue is int dl)
            {
                return Math.Max(0, Math.Min(dl, 4));
            }
            return baseValue;
        }

        #endregion

        /************************************************************************/

        #region Protected methods
            /// <summary>
            /// Raises the <see cref="ValueChangedEvent"/>.
            /// </summary>
            /// <param name="e"></param>
        protected virtual void OnValueChanged(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion
    }
}
