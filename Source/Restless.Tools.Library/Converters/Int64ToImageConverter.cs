﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Restless.Tools.Converters
{
    /// <summary>
    /// Provides a converter that accepts an integer value and returns an <see cref="ImageSource"/> object.
    /// </summary>
    public class Int64ToImageConverter : MarkupExtension, IValueConverter
    {
        #region Public methods
        /// <summary>
        /// Converts an integer value to an <see cref="ImageSource"/> object.
        /// </summary>
        /// <param name="value">The integer value</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">The preface of the image resouce name. The integer value is prepended to this value to obtain the resouce name.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>The <see cref="ImageSource"/> object associated with the value</returns>
        /// <remarks>
        /// <para>
        /// This converter enables you to associate a particular integer value with an image. Your must have the image resources
        /// defined in accordance with the integer values they represent. For instance:
        /// </para>
        /// <code>
        ///   &lt;BitmapImage x:Key="ImageFileType1" UriSource="..."/&gt;
        ///   &lt;BitmapImage x:Key="ImageFileType2" UriSource="..."/&gt;
        ///   &lt;BitmapImage x:Key="ImageFileType3" UriSource="..."/&gt;
        ///   // etc.
        /// </code>
        /// <para>
        /// When using this converter, pass the preface of the image resource name in <paramref name="parameter"/>. In the example above,
        /// we'd pass in "ImageFileType". This method then looks for a resource named according to the preface followed by the integer value,
        /// and if found, returns it. If there is no corresponding resource, this method returns null.
        /// </para>
        /// </remarks>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource result = null;

            if (value is Int64)
            {
                string resourcePreface = (parameter is string) ? parameter.ToString() : String.Empty;
                result = (ImageSource)Application.Current.TryFindResource(String.Format("{0}{1}", resourcePreface, value));
            }
            return result;
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
