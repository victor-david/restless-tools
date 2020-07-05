using System;
using System.Windows.Data;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides a converter that accepts a <see cref="DateTime"/> object and returns its local date/time value.
    /// </summary>
    internal class DateUtcToLocalConverter : IValueConverter
    {
        #region Public methods
        /// <summary>
        /// Converts a <see cref="DateTime"/> object its local date/time value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> object.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns><paramref name="value"/> converted to its local date/time.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.ToLocalTime();
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
        #endregion
    }
}
