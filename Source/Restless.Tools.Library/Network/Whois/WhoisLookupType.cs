using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restless.Tools.Network.Whois
{
    /// <summary>
    /// Represents the type of the whois lookup.
    /// </summary>
    public enum WhoisLookupType
    {
        /// <summary>
        /// The lookup is by Ip address.
        /// </summary>
        IpAddress,
        /// <summary>
        /// The lookup is by domain name.
        /// </summary>
        Domain,
        /// <summary>
        /// The type of lookup is unknown.
        /// </summary>
        Unknown,
    }
}
