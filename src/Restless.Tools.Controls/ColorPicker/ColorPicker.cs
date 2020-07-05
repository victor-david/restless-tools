using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Represents a control for simple color selection.
    /// </summary>
    [TemplatePart(Name = PartAvailableColors, Type = typeof(ListBox))]
    [TemplatePart(Name = PartToggleButton, Type = typeof(ToggleButton))]
    public class RestlessColorPicker : ContentControl
    {
        #region Private
        private const string PartAvailableColors = "PART_AvailableColors";
        private const string PartToggleButton = "PART_ColorPickerToggleButton";
        private ListBox availableColors;
        private ToggleButton toggleButton;
        #endregion


        #region Public Properties
        /// <summary>
        /// Gets or sets the selected color used for this control.
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        /// <summary>
        /// Dependency property definition for the <see cref="SelectedColor"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register
            (
                nameof(SelectedColor), typeof(Color), typeof(RestlessColorPicker), 
                new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorPropertyChanged)
            );

        /// <summary>
        /// Gets or sets a command to run when <see cref="SelectedColor"/> changes.
        /// </summary>
        public ICommand SelectedColorChangedCommand
        {
            get => (ICommand)GetValue(SelectedColorChangedCommandProperty);
            set => SetValue(SelectedColorChangedCommandProperty, value);
        }

        /// <summary>
        /// Dependency property definition for the <see cref="SelectedColorChangedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorChangedCommandProperty = DependencyProperty.Register
            (
                nameof(SelectedColorChangedCommand), typeof(ICommand), typeof(RestlessColorPicker), new PropertyMetadata(null)
            );
       

        /// <summary>
        /// Gets or sets the collection of available colors.
        /// </summary>
        public ObservableCollection<ColorItem> AvailableColors
        {
            get => (ObservableCollection<ColorItem>)GetValue(AvailableColorsProperty);
            set => SetValue(AvailableColorsProperty, value);
        }

        /// <summary>
        /// Dependency property definition for the <see cref="AvailableColors"/> property.
        /// </summary>
        public static readonly DependencyProperty AvailableColorsProperty = DependencyProperty.Register
            (
                nameof(AvailableColors), typeof(ObservableCollection<ColorItem>), typeof(RestlessColorPicker), new UIPropertyMetadata(CreateAvailableColors())
            );

        /// <summary>
        /// Gets or sets the mode to use when sorting the <see cref="AvailableColors"/> collection.
        /// </summary>
        public ColorSortingMode ColorSortingMode
        {
            get => (ColorSortingMode)GetValue(ColorSortingModeProperty);
            set => SetValue(ColorSortingModeProperty, value);
        }

        /// <summary>
        /// Dependency property definition for the <see cref="ColorSortingMode"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorSortingModeProperty = DependencyProperty.Register
            (
                nameof(ColorSortingMode), typeof(ColorSortingMode), typeof(RestlessColorPicker), 
                new UIPropertyMetadata(ColorSortingMode.Alpha, OnColorSortingModeChanged)
            );

        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RestlessColorPicker"/> class.
        /// </summary>
        public RestlessColorPicker()
        {
        }

        static RestlessColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RestlessColorPicker), new FrameworkPropertyMetadata(typeof(RestlessColorPicker)));
        }
        #endregion

        /************************************************************************/

        #region Public emthods
        /// <summary>
        /// Invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Remove handlers if present.
            if (availableColors != null) availableColors.SelectionChanged -= ColorSelectionChanged;

            // Get the references from the template
            availableColors = GetTemplateChild(PartAvailableColors) as ListBox;
            toggleButton = GetTemplateChild(PartToggleButton) as ToggleButton;

            // Add handlers if present
            if (availableColors != null) availableColors.SelectionChanged += ColorSelectionChanged;
        }
        #endregion

        /************************************************************************/

        #region Private Methods

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessColorPicker control && control.SelectedColorChangedCommand != null)
            {
                if (control.SelectedColorChangedCommand.CanExecute(e.NewValue))
                {
                    control.SelectedColorChangedCommand.Execute(e.NewValue);
                }
            }
        }

        private static ObservableCollection<ColorItem> CreateStandardColors()
        {
            ObservableCollection<ColorItem> collection = new ObservableCollection<ColorItem>
            {
                new ColorItem(Colors.Transparent, "Transparent"),
                new ColorItem(Colors.White, "White"),
                new ColorItem(Colors.Gray, "Gray"),
                new ColorItem(Colors.Black, "Black"),
                new ColorItem(Colors.Red, "Red"),
                new ColorItem(Colors.Green, "Green"),
                new ColorItem(Colors.Blue, "Blue"),
                new ColorItem(Colors.Yellow, "Yellow"),
                new ColorItem(Colors.Orange, "Orange"),
                new ColorItem(Colors.Purple, "Purple")
            };
            return collection;
        }

        private static ObservableCollection<ColorItem> CreateAvailableColors()
        {
            ObservableCollection<ColorItem> collection = new ObservableCollection<ColorItem>();

            foreach (var item in ColorUtilities.KnownColors)
            {
                var colorItem = new ColorItem(item.Value, item.Key);
                if (!collection.Contains(colorItem))
                {
                    collection.Add(colorItem);
                }

            }
            return collection;
        }

        private static void OnColorSortingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestlessColorPicker control)
            {
                control.OnColorSortingModeChanged();
            }
        }

        private void OnColorSortingModeChanged()
        {
            ListCollectionView lcv = (ListCollectionView)(CollectionViewSource.GetDefaultView(AvailableColors));
            if (lcv != null)
            {
                lcv.CustomSort = (ColorSortingMode == ColorSortingMode.HSB)
                                  ? new ColorSorter()
                                  : null;
            }
        }

        private void CloseColorPicker()
        {
            if (toggleButton != null)
            {
                toggleButton.IsChecked = false;
            }
            ReleaseMouseCapture();
        }

        private void ColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            if (e.AddedItems.Count > 0)
            {
                var colorItem = (ColorItem)e.AddedItems[0];
                SelectedColor = colorItem.Color;
                CloseColorPicker();
                lb.SelectedIndex = -1;
            }
        }
        #endregion
    }
}
