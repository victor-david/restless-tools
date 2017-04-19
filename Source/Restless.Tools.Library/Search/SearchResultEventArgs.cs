using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Restless.Tools.Utility;

namespace Restless.Tools.Search
{
    /// <summary>
    /// Represents the event args for a search result even
    /// </summary>
    public class SearchResultEventArgs : CancelEventArgs
    {
        #region Public properties
        /// <summary>
        /// Gets the result associated with the event.
        /// </summary>
        public WindowsSearchResult Result
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Creates a new instance of this class
        /// </summary>
        /// <param name="result">The serahc result</param>
        internal SearchResultEventArgs(WindowsSearchResult result)
        {
            Validations.ValidateNull(result, "SearchResultEventArgs.Result");
            Result = result;
        }
        #endregion
    }
}
