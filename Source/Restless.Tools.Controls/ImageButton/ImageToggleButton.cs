using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Extends <see cref="ToggleButton"/>
    /// </summary>
    public class ImageToggleButton : ToggleButton
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageToggleButton"/> class.
        /// </summary>
        public ImageToggleButton()
        {
        }

        static ImageToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageToggleButton), new FrameworkPropertyMetadata(typeof(ImageToggleButton)));
        }
        #endregion

        /************************************************************************/

        #region Image (On / Checked)
        /// <summary>
        /// Gets or sets the image source to use when the button is checked.
        /// </summary>
        public ImageSource ImageSourceOn
        {
            get => (ImageSource)GetValue(ImageSourceOnProperty);
            set => SetValue(ImageSourceOnProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageSourceOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceOnProperty = DependencyProperty.Register
            (
                nameof(ImageSourceOn), typeof(ImageSource), typeof(ImageToggleButton), new PropertyMetadata(null, OnImageSourceOnChanged)
            );

        private static void OnImageSourceOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageToggleButton)?.SynchronizeImageVisibility();
        }

        /// <summary>
        /// Gets the visibility for the on / checked image.
        /// </summary>
        public Visibility ImageOnVisibility
        {
            get => (Visibility)GetValue(ImageOnVisibilityProperty);
            private set => SetValue(ImageOnVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ImageOnVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ImageOnVisibility), typeof(Visibility), typeof(ImageToggleButton), new PropertyMetadata(Visibility.Collapsed)
            );

        /// <summary>
        /// Identifies the <see cref="ImageOnVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageOnVisibilityProperty = ImageOnVisibilityPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Image (Off / Unchecked)
        /// <summary>
        /// Gets or sets the image source to use when the button is unchecked.
        /// </summary>
        public ImageSource ImageSourceOff
        {
            get => (ImageSource)GetValue(ImageSourceOffProperty);
            set => SetValue(ImageSourceOffProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageSourceOff"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceOffProperty = DependencyProperty.Register
            (
                nameof(ImageSourceOff), typeof(ImageSource), typeof(ImageToggleButton), new PropertyMetadata(null, OnImageSourceOffChanged)
            );

        private static void OnImageSourceOffChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageToggleButton)?.SynchronizeImageVisibility();
        }

        /// <summary>
        /// Gets the visibility for the off / unchecked image.
        /// </summary>
        public Visibility ImageOffVisibility
        {
            get => (Visibility)GetValue(ImageOffVisibilityProperty);
            private set => SetValue(ImageOffVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ImageOffVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ImageOffVisibility), typeof(Visibility), typeof(ImageToggleButton), new PropertyMetadata(Visibility.Collapsed)
            );

        /// <summary>
        /// Identifies the <see cref="ImageOffVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageOffVisibilityProperty = ImageOffVisibilityPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Images (both checked and unchecked)
        /// <summary>
        /// Gets or sets the width and height for both images.
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
                nameof(ImageSize), typeof(double), typeof(ImageToggleButton), new PropertyMetadata(24d)
            );

        /// <summary>
        /// Gets or sets the padding for both images.
        /// </summary>
        public Thickness ImagePadding
        {
            get => (Thickness)GetValue(ImagePaddingProperty);
            set => SetValue(ImagePaddingProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImagePadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImagePaddingProperty = DependencyProperty.Register
            (
                nameof(ImagePadding), typeof(Thickness), typeof(ImageToggleButton), new PropertyMetadata(new Thickness())
            );

        /// <summary>
        /// Gets or sets the horizontal alignment for the images.
        /// </summary>
        public HorizontalAlignment HorizontalImageAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalImageAlignmentProperty);
            set => SetValue(HorizontalImageAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalImageAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalImageAlignmentProperty = DependencyProperty.Register
            (
                nameof(HorizontalImageAlignment), typeof(HorizontalAlignment), typeof(ImageToggleButton), new PropertyMetadata(HorizontalAlignment.Center)
            );

        /// <summary>
        /// Gets or sets the vertical alignment for the images.
        /// </summary>
        public VerticalAlignment VerticalImageAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalImageAlignmentProperty);
            set => SetValue(VerticalImageAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="VerticalImageAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalImageAlignmentProperty = DependencyProperty.Register
            (
                nameof(VerticalImageAlignment), typeof(VerticalAlignment), typeof(ImageToggleButton), new PropertyMetadata(VerticalAlignment.Center)
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
                nameof(ContentPadding), typeof(Thickness), typeof(ImageToggleButton), new PropertyMetadata(new Thickness())
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
                nameof(ContentVisibility), typeof(Visibility), typeof(ImageToggleButton), new PropertyMetadata(Visibility.Collapsed)
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
                nameof(RolloverBackgroundBrush), typeof(Brush), typeof(ImageToggleButton), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the border rollover brush .
        /// </summary>
        public Brush RolloverBorderBrush
        {
            get => (Brush)GetValue(RolloverBorderBrushProperty);
            set => SetValue(RolloverBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RolloverBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RolloverBorderBrushProperty = DependencyProperty.Register
            (
                nameof(RolloverBorderBrush), typeof(Brush), typeof(ImageToggleButton), new PropertyMetadata(null)
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
                nameof(PressedBrush), typeof(Brush), typeof(ImageToggleButton), new PropertyMetadata(null)
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
                nameof(PressedOffset), typeof(double), typeof(ImageToggleButton), new PropertyMetadata(0d, OnPressedOffsetChanged, OnCoercePressedOffset)
            );

        private static void OnPressedOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageToggleButton button)
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
                nameof(PressedMargin), typeof(Thickness), typeof(ImageToggleButton), new PropertyMetadata(new Thickness())
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
                nameof(CornerRadius), typeof(CornerRadius), typeof(ImageToggleButton), new PropertyMetadata(new CornerRadius())
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
                nameof(Orientation), typeof(Orientation), typeof(ImageToggleButton), new PropertyMetadata(Orientation.Horizontal)
            );
        #endregion

        /************************************************************************/

        #region Methods
        /// <summary>
        /// Called when button state becomes checked.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            SynchronizeImageVisibility();
         }

        /// <summary>
        /// Called when button state becomes unchecked.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            SynchronizeImageVisibility();
        }

        /// <summary>
        /// Called when button state becomes indeterminate.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnIndeterminate(RoutedEventArgs e)
        {
            base.OnIndeterminate(e);
            SynchronizeImageVisibility();
        }

        /// <summary>
        /// Called when the content property changes to control content visibility.
        /// </summary>
        /// <param name="oldContent">Old content</param>
        /// <param name="newContent">New content</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            ContentVisibility = (newContent == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SynchronizeImageVisibility()
        {
            switch (IsChecked)
            {
                case true:
                    ImageOnVisibility = (ImageSourceOn == null) ? Visibility.Collapsed : Visibility.Visible;
                    ImageOffVisibility = Visibility.Collapsed;
                    break;
                case false:
                    ImageOffVisibility = (ImageSourceOff == null) ? Visibility.Collapsed : Visibility.Visible;
                    ImageOnVisibility = Visibility.Collapsed;
                    break;
                case null:
                    ImageOnVisibility = Visibility.Collapsed;
                    ImageOffVisibility = Visibility.Collapsed;
                    break;
            }
        }
        #endregion
    }
}