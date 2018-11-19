using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts a boolean value and returns a <see cref="Visibility"/> value.
    /// </summary>
    public class BooleanToInverseVisibilityConverter : MarkupExtension, IValueConverter
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToInverseVisibilityConverter"/> class.
        /// </summary>
        public BooleanToInverseVisibilityConverter()
        {
            // prevents the designer that's referencing this converter directly from going stupid every time you type a character
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> value.
        /// If <paramref name="value"/> is true, returns collapsed.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns>Either <see cref="Visibility.Visible"/> or <see cref="Visibility.Collapsed"/></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Visibility) && value is bool boolValue)
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
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
