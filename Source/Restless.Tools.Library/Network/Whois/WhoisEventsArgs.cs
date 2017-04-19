using System;
using System.Text;

namespace Restless.Tools.Network.Whois
{
    /// <summary>
    /// Event arguments for Whois events
    /// </summary>
    public class WhoisEventArgs : EventArgs
    {
        #region Private fields
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the result of the whois operation.
        /// </summary>
        public WhoisLookupResult Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the server that was the target of the request.
        /// </summary>
        public WhoisServer WhoisServer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lookup parameters.
        /// </summary>
        public WhoisLookupParms LookupParameters
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Creates a new instance of the WhoisEventsArgs class.
        /// </summary>
        /// <param name="result">The lookup result object</param>
        /// <param name="whoisServer">The server that satisfied the request</param>
        /// <param name="lookupParameters">The lookup parameters.</param>
        internal WhoisEventArgs(WhoisLookupResult result, WhoisServer whoisServer, WhoisLookupParms lookupParameters)
        {
            Result = result;
            WhoisServer = whoisServer;
            LookupParameters = lookupParameters;
        }
        #endregion

    }
}
