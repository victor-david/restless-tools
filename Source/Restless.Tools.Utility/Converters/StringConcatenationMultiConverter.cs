using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts multiple values and returns a concatenated string.
    /// </summary>
    public class StringConcatenationMultiConverter : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Converts multi values into a single concatenated string.
        /// </summary>
        /// <param name="values">The values</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The separator between values, or null to use the default of a single space</param>
        /// <param name="culture"></param>
        /// <returns>A string that consists of <paramref name="values"/> concatenated.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            object delim = parameter ?? " ";
            for (int k=0; k < values.Length; k++)
            {
                result += values[k].ToString();
                if (k < values.Length - 1)
                {
                    result += delim.ToString();
                }
            }
            return result.Trim();
        }

        /// <summary>
        /// This method is not used. It throws a <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="value">n/a</param>
        /// <param name="targetTypes">n/a</param>
        /// <param name="parameter">n/a</param>
        /// <param name="culture">n/a</param>
        /// <returns>n/a</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
    }
}
