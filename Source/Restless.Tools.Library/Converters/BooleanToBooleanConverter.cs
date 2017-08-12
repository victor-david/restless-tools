﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts a boolean value and returns a boolean value.
    /// This converter can be used to return true when the bound value is false, and vice-versa.
    /// </summary>
    public class BooleanToBooleanConverter : MarkupExtension, IValueConverter
    {
        #region Constructor
        #pragma warning disable 1591
        public BooleanToBooleanConverter()
        {
            // prevents the designer that's referencing this converter directly from going stupid every time you type a character
        }
        #pragma warning restore 1591
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts a boolean value and to a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">If used, you can pass boolean true to reverse the evaluataion.</param>
        /// <param name="culture">Not used</param>
        /// <returns>Either <see cref="Visibility.Visible"/> or <see cref="Visibility.Collapsed"/></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Boolean)) return value;

            if (value is bool)
            {
                bool reverse = false;
                if (parameter is bool) reverse = (bool)parameter;
                return (reverse) ? !(bool)value : (bool)value;
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
