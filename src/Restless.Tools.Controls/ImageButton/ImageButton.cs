using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Extends <see cref="Button"/> to provide a button that hosts an image with its standard content.
    /// </summary>
    public class ImageButton : Button
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageButton"/> class.
        /// </summary>
        public ImageButton()
        {
        }

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        #endregion

        /************************************************************************/

        #region Image
        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register
            (
                nameof(ImageSource), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null, OnImageSourceChanged)
            );

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageButton button)
            {
                button.ImageVisibility = (e.NewValue != null) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the visibility for the image.
        /// </summary>
        public Visibility ImageVisibility
        {
            get => (Visibility)GetValue(ImageVisibilityProperty);
            private set => SetValue(ImageVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ImageVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ImageVisibility), typeof(Visibility), typeof(ImageButton), new PropertyMetadata(Visibility.Collapsed)
            );

        /// <summary>
        /// Identifies the <see cref="ImageVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageVisibilityProperty = ImageVisibilityPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the image width and height.
        /// </summary>
        public double ImageSize
        {
            get => (double)GetValue(ImageSizeProperty);
            set => SetValue(ImageSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register
            (
                nameof(ImageSize), typeof(double), typeof(ImageButton), new PropertyMetadata(24d)
            );
        
        /// <summary>
        /// Gets or sets the padding for the image.
        /// </summary>
        public Thickness ImagePadding
        {
            get => (Thickness)GetValue(ImagePaddingProperty);
            set =>SetValue(ImagePaddingProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImagePadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImagePaddingProperty = DependencyProperty.Register
            (
                nameof(ImagePadding), typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness())
            );
        #endregion

        /************************************************************************/

        #region Content
        /// <summary>
        /// Gets or sets the padding for the content.
        /// </summary>
        public Thickness ContentPadding
        {
            get => (Thickness)GetValue(ContentPaddingProperty);
            set => SetValue(ContentPaddingProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ContentPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register
            (
                nameof(ContentPadding), typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness())
            );

        /// <summary>
        /// Gets the visibility for the content.
        /// </summary>
        public Visibility ContentVisibility
        {
            get => (Visibility)GetValue(ContentVisibilityProperty);
            private set => SetValue(ContentVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ContentVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ContentVisibility), typeof(Visibility), typeof(ImageButton), new PropertyMetadata(Visibility.Collapsed)
            );

        /// <summary>
        /// Identifies the <see cref="ContentVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentVisibilityProperty = ContentVisibilityPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the background rollover brush.
        /// </summary>
        public Brush RolloverBackgroundBrush
        {
            get => (Brush)GetValue(RolloverBackgroundBrushProperty);
            set => SetValue(RolloverBackgroundBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RolloverBackgroundBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RolloverBackgroundBrushProperty = DependencyProperty.Register
            (
                nameof(RolloverBackgroundBrush), typeof(Brush), typeof(ImageButton), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the border rollover brush .
        /// </summary>
        public Brush RolloverBorderBrush
        {
            get => (Brush)GetValue(RolloverBorderBrushProperty);
            set =>SetValue(RolloverBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RolloverBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RolloverBorderBrushProperty = DependencyProperty.Register
            (
                nameof(RolloverBorderBrush), typeof(Brush), typeof(ImageButton), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the pressed brush
        /// </summary>
        public Brush PressedBrush
        {
            get => (Brush)GetValue(PressedBrushProperty);
            set => SetValue(PressedBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PressedBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register
            (
                nameof(PressedBrush), typeof(Brush), typeof(ImageButton), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region PressedOffset
        /// <summary>
        /// Gets or set the amount of offset to use when the button is pressed.
        /// </summary>
        public double PressedOffset
        {
            get => (double)GetValue(PressedOffsetProperty);
            set => SetValue(PressedOffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PressedOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PressedOffsetProperty = DependencyProperty.Register
            (
                nameof(PressedOffset), typeof(double), typeof(ImageButton), new PropertyMetadata(0d, OnPressedOffsetChanged, OnCoercePressedOffset)
            );

        private static void OnPressedOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageButton button)
            {
                button.PressedMargin = new Thickness((double)e.NewValue, 0, 0, 0);
            }
        }

        private static object OnCoercePressedOffset(DependencyObject d, object baseValue)
        {
            /* value is clamped between -2 and 2 */
            if (baseValue is double value)
            {
                return Math.Max(Math.Min(value, 2), -2);
            }
            return baseValue;
        }

        /// <summary>
        /// Gets the margin to use when the button is pressed.
        /// </summary>
        public Thickness PressedMargin
        {
            get => (Thickness)GetValue(PressedMarginProperty);
            private set => SetValue(PressedMarginPropertyKey, value);
        }

        private static readonly DependencyPropertyKey PressedMarginPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(PressedMargin), typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness())
            );

        /// <summary>
        /// Identifies the <see cref="PressedMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PressedMarginProperty = PressedMarginPropertyKey.DependencyProperty;

        #endregion

        /************************************************************************/

        #region CornerRadius
        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            (
                nameof(CornerRadius), typeof(CornerRadius), typeof(ImageButton), new PropertyMetadata(new CornerRadius())
            );
        #endregion

        /************************************************************************/

        #region Orientation
        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register
            (
                nameof(Orientation), typeof(Orientation), typeof(ImageButton), new PropertyMetadata(Orientation.Horizontal)
            );
        #endregion

        /************************************************************************/

        #region Methods
        /// <summary>
        /// Called when the content property changes to control content visibility.
        /// </summary>
        /// <param name="oldContent">Old content</param>
        /// <param name="newContent">New content</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            ContentVisibility = (newContent != null) ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
    }
}