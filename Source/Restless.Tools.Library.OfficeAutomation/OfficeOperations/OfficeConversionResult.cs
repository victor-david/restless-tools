using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Restless.Tools.OfficeAutomation
{
    /// <summary>
    /// A return object that describes the result of a conversion operation.
    /// </summary>
    public class OfficeConversionResult : OfficeFileResult
    {
        #region Public Properties
        /// <summary>
        /// Gets the file info object for the converted file.
        /// </summary>
        public FileInfo ConvertedInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a message that describes the status of the conversion.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeConversionResult"/> class.
        /// </summary>
        /// <param name="origFileName">The original file name</param>
        /// <param name="newFileName">The new (converted) file name</param>
        /// <param name="exception">The exception that occured, or null if none.</param>
        internal OfficeConversionResult(string origFileName, string newFileName, Exception exception)
            : base(origFileName, exception)
        {
            if (!String.IsNullOrEmpty(newFileName))
            {
                ConvertedInfo = new FileInfo(newFileName);
            }
            if (Success)
            {
                Message = "Converted";
            }
            else
            {
                Message = Exception.Message;
            }
        }
        #endregion
    }
}
