using System;
using System.Collections.Generic;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Represents a list of integer 64 values.
    /// </summary>
    public class Integer64List : List<long>
    {
        #region Public Fields
        /// <summary>
        /// The default delimeter.
        /// </summary>
        public const char DefaultDelimter = ',';
        #endregion

        /************************************************************************/

        #region Public Enum
        /// <summary>
        /// Describes the behavior when using the ToString method
        /// </summary>
        public enum ToStringDisposition
        {
            /// <summary>
            /// The string is returned with the value -1 appended whether any values exist in the list or not.
            /// </summary>
            Default,
            /// <summary>
            /// The string is returned with the final delimeter removed, ex: 65,399,211
            /// </summary>
            Remove,
            /// <summary>
            /// The string is returned with the value -1 appended to the final delimeter, ex: 65,399,211,-1
            /// </summary>
            AddNegativeOne,
            /// <summary>
            /// The string is returned with the value -1 appended whether any values exist in the list or not.
            /// </summary>
            AddNegativeOneEvenIfEmpty,
        }
        #endregion

        /************************************************************************/

        #region Public Methods
        /// <summary>
        /// Adds values to the list from the specified string value.
        /// </summary>
        /// <param name="value">The string</param>
        /// <param name="delimeter">The delimeter</param>
        public void AddValuesFromDelimitedString(string value, char delimeter = DefaultDelimter)
        {
            long id;
            string[] temp = value.Split(delimeter);
            foreach (string cid in temp)
            {
                if (long.TryParse(cid, out id))
                {
                    Add(id);
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the values in the list.
        /// </summary>
        /// <param name="delimeter">The delimeter to use.</param>
        /// <param name="final">The final delimeter behavior.</param>
        /// <returns>The string.</returns>
        public string ToString(char delimeter = DefaultDelimter, ToStringDisposition final = ToStringDisposition.Default)
        {
            string idStr = string.Empty;
            foreach (long id in this)
            {
                idStr += string.Format("{0}{1}", id, delimeter);
            }

            if (idStr.Length == 0)
            {
                switch (final)
                {
                    case ToStringDisposition.Default:
                    case ToStringDisposition.AddNegativeOneEvenIfEmpty:
                        return "-1";
                    default:
                        return string.Empty;
                }
            }

            // string is not zero-length
            switch (final)
            {
                case ToStringDisposition.Remove:
                    return idStr.Remove(idStr.Length - 1);
                case ToStringDisposition.Default:
                case ToStringDisposition.AddNegativeOne:
                case ToStringDisposition.AddNegativeOneEvenIfEmpty:
                    return idStr + "-1";
            }
            return idStr;
        }

        /// <summary>
        /// Returns a string representation of the values in the list using the default delimeter.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return ToString(DefaultDelimter);
        }
        #endregion
    }
}
