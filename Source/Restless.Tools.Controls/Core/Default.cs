using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides static default values
    /// </summary>
    public static class Default
    {
        /// <summary>
        /// Provides static default formatting values.
        /// </summary>
        public static class Format
        {
            private static string date = "MMM dd, yyyy";

            /// <summary>
            /// Gets or sets the date format to use for both
            /// the <see cref="DataGridColumnExtensions.MakeDate(DataGridBoundColumn, string, int, bool)"/> extension
            /// and the <see cref="RestlessPopupCalendar"/> control.
            /// </summary>
            public static string Date
            {
                get => date;
                set
                {
                    date = value;
                    DataGridDate = value;
                    PopupCalendarDate = value;
                }
            }

            /// <summary>
            /// Gets or sets the default date format to use with the <see cref="DataGridColumnExtensions.MakeDate(DataGridBoundColumn, string, int, bool)"/> extension.
            /// </summary>
            public static string DataGridDate = "MMM dd, yyyy";

            /// <summary>
            /// Gets or sets the format used for the <see cref="RestlessPopupCalendar"/> control.
            /// </summary>
            public static string PopupCalendarDate = "MMM dd, yyyy";
        }

        /// <summary>
        /// Provides static default names for styles
        /// </summary>
        public static class Style
        {
            /// <summary>
            /// Defines the name of a style that may be applied to DataGridColumnHeader in order to center it.
            /// </summary>
            public static string DataGridHeader = "DataGridHeaderDefault";

            /// <summary>
            /// Defines the name of a style that may be applied to DataGridColumnHeader in order to center it.
            /// </summary>
            public static string DataGridHeaderCenter = "DataGridHeaderCenter";

            /// <summary>
            /// Defines the name of a style that may be applied to TextBlock in order to center it.
            /// </summary>
            public static string TextBlockCenter = "TextBlockCenter";

            /// <summary>
            /// Defines the name of a style that may be applied to DataGridColumnHeader in order to right align it.
            /// </summary>
            public static string DataGridHeaderRight = "DataGridHeaderRight";

            /// <summary>
            /// Defines the name of a style that may be applied to TextBlock in order to right align it.
            /// </summary>
            public static string TextBlockRight = "TextBlockRight";
        }
    }
}
