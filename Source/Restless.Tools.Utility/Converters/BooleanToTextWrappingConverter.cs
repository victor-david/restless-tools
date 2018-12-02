using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts a boolean value and returns a <see cref="TextWrapping"/> value.
    /// </summary>
    public class BooleanToTextWrappingConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="TextWrapping"/> value.
        /// </summary>
        /// <param name="value">The boolean value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>TextWrapping.Wrap if <paramref name="value"/> is true; otherwise, TextWrapping.NoWrap</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? TextWrapping.Wrap : TextWrapping.NoWrap;
            }
            return TextWrapping.NoWrap;
        }

        /// <summary>
        /// Converts a <see cref="TextWrapping"/> value to a boolean value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used</param>
        /// <returns>true if value is TextWrapping.Wrap; otherwise, false.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TextWrapping)
            {
                return (TextWrapping)value ==  TextWrapping.Wrap;
            }
            return false;
        }

        /// <summary>
        /// Gets the object that is set as the value of the target property for this markup extension. 
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>This object.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
