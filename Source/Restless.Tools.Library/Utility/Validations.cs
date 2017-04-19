using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restless.Tools.Resources;
using System.Data;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides static methods to perform standard validation tasks.
    /// </summary>
    public class Validations
    {
        /// <summary>
        /// Throws an ArgumentNullException if the specified object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="name">The name of the argument to present in the exception.</param>
        /// <param name="message">The message to use in the exception, or null to use the default message.</param>
        public static void ValidateNull(object obj, string name, string message = null)
        {
            if (obj == null)
            {
                name = GetObjectName(name);
                message = (!String.IsNullOrEmpty(message)) ? message : String.Format(Strings.ArgumentNull_ValidateNull, name);
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if the specified string is null or empty.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="name">The name of the argument to present in the exception.</param>
        /// <param name="message">The message to use in the exception, or null to use the default message.</param>
        public static void ValidateNullEmpty(string s, string name, string message = null)
        {
            if (String.IsNullOrEmpty(s))
            {
                name = GetObjectName(name);
                message = (!String.IsNullOrEmpty(message)) ? message : String.Format(Strings.ArgumentNull_ValidateNullOrEmpty, name);
                ValidateNull(null, name, message);
            }
        }

        /// <summary>
        /// Throws an InvalidOperationException if the specified condition is true.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">The message to use for the exception.</param>
        public static void ValidateInvalidOperation(bool condition, string message)
        {
            if (condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws an ArgumentException if the specified condition is true.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">The message to use for the exception.</param>
        public static void ValidateArgument(bool condition, string message)
        {
            if (condition)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the integer is not within bounds
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">Minimum inclusive value.</param>
        /// <param name="max">Maximum inclusive value</param>
        /// <param name="name">The name of the argument to present in the exception.</param>
        public static void ValidateInt(int value, int min, int max, string name)
        {
            if (value < min || value > max)
            {
                throw new ArgumentOutOfRangeException(String.Format(Strings.ArgumentOutOfRange_ValidateInteger, GetObjectName(name), min, max), innerException:null);
            }
        }

        /// <summary>
        /// Throws an InvalidOperationException if <paramref name="row"/> is null or does not belong to the table specified by <paramref name="tableName"/>,
        /// </summary>
        /// <param name="row">The data row to check.</param>
        /// <param name="tableName">The table name that the row must belong to.</param>
        public static void ValidateDataRow(DataRow row, string tableName)
        {
            ValidateNullEmpty(tableName, "ValidateDataRow.TableName");
            ValidateInvalidOperation(row == null || row.Table.TableName != tableName, String.Format(Strings.InvalidOperation_DataRowMustBeTable, tableName));
        }

        /// <summary>
        /// Throws an ArgumentNullException if the array is null, or an ArgumentException
        /// if the array does not have the specified minimum number of entries.
        /// </summary>
        /// <param name="array">The array to check.</param>
        /// <param name="minSize">The minimum size of the array</param>
        /// <param name="name">The name of the argument to present in the exception.</param>
        public static void ValidateArray(object[] array, int minSize, string name)
        {
            name = GetObjectName(name);
            ValidateNull(array, name);
            if (array.Length < minSize)
            {
                throw new ArgumentException(String.Format(Strings.Argument_ValidateArray, name, minSize));
            }
        }

        private static string GetObjectName(string passedName)
        {
            return (!String.IsNullOrEmpty(passedName)) ? passedName : Strings.UnspecifiedName;
        }
    }
}
