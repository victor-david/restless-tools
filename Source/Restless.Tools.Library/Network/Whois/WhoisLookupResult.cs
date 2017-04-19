using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restless.Tools.Utility;

namespace Restless.Tools.Network.Whois
{
    /// <summary>
    /// Represents a result for a who-is lookup.
    /// </summary>
    public class WhoisLookupResult
    {
        #region Private fields
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the text of this result.
        /// </summary>
        public StringBuilder Text
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of referral servers found in the Whois response.
        /// </summary>
        public List<WhoisServer> ReferralServers
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        internal WhoisLookupResult()
        {
            Text = new StringBuilder();
            ReferralServers = new List<WhoisServer>();
        }
        #endregion

        /************************************************************************/
        
        #region Internal methods
        internal void AddReferralServer(WhoisServer server)
        {
            Validations.ValidateNull(server, "AddReferralServer.Server");
            ReferralServers.Add(server);
        }
        #endregion

    }
}
