using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Restless.Tools.OfficeAutomation.Resources;

namespace Restless.Tools.OfficeAutomation
{
    /// <summary>
    /// Represents the result of an Office automation operation.
    /// </summary>
    public class OfficeFileResult
    {
        #region Public Properties
        /// <summary>
        /// Gets the file info object associated with this result
        /// </summary>
        public FileInfo Info
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the exception for this result, or null if none.
        /// </summary>
        public Exception Exception
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a boolean value indicating if the operation was successfull.
        /// If this property is true, <see cref="Exception"/> is always null.
        /// </summary>
        public bool Success
        {
            get { return Exception == null; }
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeFileResult"/> class.
        /// </summary>
        /// <param name="fileName">The file name associated with this result.</param>
        /// <param name="exception">The exception for this result, or null if none.</param>
        internal OfficeFileResult(string fileName, Exception exception)
        {
            Info = new FileInfo(fileName);
            Exception = exception;
        }
        #endregion

        /************************************************************************/
        
        #region Public methods
        /// <summary>
        /// If this object is in a faulted state (<see cref="Exception"/> is not null), or
        /// the file associated with <see cref="Info"/> does not exist, throws an exception.
        /// Otherwise, this method does nothing.
        /// </summary>
        public void ThrowIfException()
        {
            if (!Info.Exists && Exception == null)
            {
                Exception = new FileNotFoundException(Strings.InvalidOperation_FileDoesNotExist);
            }

            if (Exception != null)
            {
                throw Exception;
            }
        }
        #endregion
    }
}
