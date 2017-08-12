using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts an integer value and returns a string if the integer value is negative.
    /// </summary>
    public class NegativeIntegerToString : MarkupExtension, IValueConverter
    {
        #region Constructor
        #pragma warning disable 1591
        public NegativeIntegerToString()
        {
            // prevents the designer that's referencing this converter directly from going stupid every time you type a character
        }
        #pragma warning restore 1591
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts an integer that has a negative value to the specified string.
        /// </summary>
        /// <param name="value">The integer value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">A string to use if the value is negative.</param>
        /// <param name="culture">Not used</param>
        /// <returns>The string specified by <paramref name="parameter"/> if <paramref name="value"/> is negative; otherwise, <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(String)) return value;

            if (value is int && (int)value < 0)
            {
                return parameter;
            }

            return value;
        }


        /// <summary>
        /// This method is not used. It throws a <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="value">n/a</param>
        /// <param name="targetType">n/a</param>
        /// <param name="parameter">n/a</param>
        /// <param name="culture">n/a</param>
        /// <returns>n/a</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
