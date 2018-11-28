using Restless.Tools.Utility.Resources;
using System;
using System.Data;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides static methods to perform standard validation tasks.
    /// </summary>
    public class Validations
    {

        /// <summary>
        /// Gets a boolean value that indicates if the platform is at least Windows 7.
        /// </summary>
        public static bool RunningOnWin7
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Throws an exception if <see cref="RunningOnWin7"/> is false.
        /// </summary>
        public static void ThrowIfNotWin7()
        {
            if (!RunningOnWin7)
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Throws if the operating system is not at least Windows 7.
        /// </summary>
        public static void ThrowIfNotWindows7()
        {
            MS.WindowsAPICodePack.Internal.CoreHelpers.ThrowIfNotWin7();
        }

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
                message = (!string.IsNullOrEmpty(message)) ? message : string.Format(Strings.ArgumentNullValidateNull, name);
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
            if (string.IsNullOrEmpty(s))
            {
                name = GetObjectName(name);
                message = (!string.IsNullOrEmpty(message)) ? message : string.Format(Strings.ArgumentNullValidateNullOrEmpty, name);
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
                throw new ArgumentOutOfRangeException(string.Format(Strings.ArgumentOutOfRangeValidateInteger, GetObjectName(name), min, max), innerException:null);
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
            ValidateInvalidOperation(row == null || row.Table.TableName != tableName, string.Format(Strings.InvalidOperationDataRowMustBeTable, tableName));
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
                throw new ArgumentException(string.Format(Strings.ArgumentValidateArray, name, minSize));
            }
        }

        private static string GetObjectName(string passedName)
        {
            return (!string.IsNullOrEmpty(passedName)) ? passedName : Strings.UnspecifiedName;
        }
    }
}
