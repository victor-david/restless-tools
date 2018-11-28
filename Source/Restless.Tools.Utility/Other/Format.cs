using System;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides static methods to format strings.
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// Returns a string prefaced with http:// if the string doesn't already have it.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>A string with http://</returns>
        public static string MakeHttp(string s)
        {
            s = ((s.IndexOf("://") == -1) ? "http://" : string.Empty) + s;
            return s;
        }

        /// <summary>
        /// Returns a string with current date appended to preface
        /// and any invalid characters replaced with a dash character
        /// </summary>
        /// <param name="preface">The desired preface</param>
        /// <returns>The file name string.</returns>
        public static string MakeFileName(string preface)
        {
            DateTime now = DateTime.Now;

            string fileName =
                preface + "_" +
                now.Year.ToString(("D4")) + "-" +
                now.Month.ToString("D2") + "-" +
                now.Day.ToString("D2") + "-" +
                now.Hour.ToString("D2") + "-" +
                now.Minute.ToString("D2") + "-" +
                now.Second.ToString("D2") + "-" +
                now.Millisecond.ToString("D3");

            return ValidFileName(fileName);
        }

        /// <summary>
        /// Returns the specified string, with any invalid file name characters replaced with a dash.
        /// </summary>
        /// <param name="str">The string to process.</param>
        /// <returns>The specified string with any invalid file name characters replaced with a dash.</returns>
        public static string ValidFileName(string str)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '-');
            }
            return str;
        }

        /// <summary>
        /// Gets a formatted string according to value and singular / plural. Example: 1 day, 3 days
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="singular">The singular form of the word, example: day.</param>
        /// <param name="plural">The plural form of the word, example: days.</param>
        /// <returns>A string formatted according to value and singular / plur.al</returns>
        public static string Plural(int value, string singular, string plural)
        {
            return string.Format("{0} {1}", value, (value != 1) ? plural : singular);
        }

        /// <summary>
        /// Returns the specified string trimmed to the specified Gets a string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="maxLength">Max length</param>
        /// <returns>The string trimmed to max length</returns>
        public static string TrimString(string s, int maxLength)
        {
            Validations.ValidateNullEmpty(s, "TrimString.s");
            if (s.Length > maxLength)
            {
                s = s.Substring(0, maxLength);
            }
            return s;
        }
    }
}
