﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts two boolean values and returns a <see cref="Visibility"/> value.
    /// </summary>
    /// <remarks>
    /// See:
    /// https://code.msdn.microsoft.com/windowsapps/How-to-add-a-hint-text-to-ed66a3c6
    /// </remarks>
    public class BooleanToVisibilityMultiConverter : MarkupExtension, IMultiValueConverter
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToVisibilityMultiConverter"/> class.
        /// </summary>
        public BooleanToVisibilityMultiConverter()
        {
            // prevents the designer that's referencing this converter directly from going stupid every time you type a character
        }
        #endregion
        
        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Converts two boolean values to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="values">The boolean values</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">
        /// A value from the <see cref="BooleanToVisibilityMultiConverterOptions"/> enumeration that describes how to treat the two boolean values.
        /// If not passed, the default is <see cref="BooleanToVisibilityMultiConverterOptions.OneTrueOrTwoTrue"/>.
        /// </param>
        /// <param name="culture">Not used.</param>
        /// <returns>A <see cref="Visibility"/> value.</returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Visibility) && values.Length > 1 && values[0] is bool b1 && values[1] is bool b2)
            {
                BooleanToVisibilityMultiConverterOptions op = BooleanToVisibilityMultiConverterOptions.OneTrueOrTwoTrue;
                if (parameter is BooleanToVisibilityMultiConverterOptions)
                {
                    op = (BooleanToVisibilityMultiConverterOptions)parameter;
                }
                switch (op)
                {
                    case BooleanToVisibilityMultiConverterOptions.OneFalseOrTwoTrue:
                        if (!b1 || b2) return Visibility.Collapsed;
                        break;

                    case BooleanToVisibilityMultiConverterOptions.OneTrueAndTwoTrue:
                        if (b1 && b2) return Visibility.Collapsed;
                        break;

                    case BooleanToVisibilityMultiConverterOptions.OneTrueOrTwoTrue:
                        if (b1 || b2) return Visibility.Collapsed;
                        break;
                }
            }
            return Visibility.Visible;
        }

        /// <summary>
        /// This method is not used. It throws a <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="value">n/a</param>
        /// <param name="targetTypes">n/a</param>
        /// <param name="parameter">n/a</param>
        /// <param name="culture">n/a</param>
        /// <returns>n/a</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
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
