using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restless.Tools.Utility;

namespace Restless.Tools.Network.Whois
{
    /// <summary>
    /// Represents the parameters for a Whois lookup operation
    /// </summary>
    public class WhoisLookupParms
    {
        #region Private fields
        private string[] parts;
        #endregion

        /************************************************************************/
        
        #region Public Properties
        /// <summary>
        /// Gets the entry that was used to create this lookup
        /// </summary>
        public string Entry
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lookup type: IPAddress, Domain, or Invalid.
        /// </summary>
        public WhoisLookupType LookupType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain
        /// </summary>
        public string Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the actual query that should be sent to the host.
        /// The value returned by this property depends on LookupType.
        /// </summary>
        public string Query
        {
            get
            {
                return (LookupType != WhoisLookupType.Unknown) ? Domain : Entry;
            }
        }

        /// <summary>
        /// Gets the top level domain
        /// </summary>
        public string TopLevelDomain
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="entry">The entry string to lookup</param>
        public WhoisLookupParms(string entry)
        {
            Validations.ValidateNullEmpty(entry, " WhoisLookupParms.Entry");

            Domain = TopLevelDomain = String.Empty;
            
            Entry = entry.Trim().ToLower();

            LookupType = GetLookupType(Entry);

            if (LookupType != WhoisLookupType.Unknown)
            {
                Domain = Entry;
                if (Domain.StartsWith("http://"))
                {
                    Domain = Domain.Substring(7);
                }
                
                if (Domain.StartsWith("https://"))
                {
                    Domain = Domain.Substring(8);
                }

                if (Domain.StartsWith("www."))
                {
                    Domain = Domain.Substring(4);
                }
                TopLevelDomain = parts[parts.Length - 1];
            }
        }
        #endregion
        
        /************************************************************************/

        #region Private Methods
        /// <summary>
        /// Parses the entry string and returns the type of lookup associated with the entry.
        /// </summary>
        /// <param name="entry">The entry string</param>
        /// <returns>A value form the WhoisLookupTYpe enumeration.</returns>
        private WhoisLookupType GetLookupType(string entry)
        {
            if (entry.Length == 0)
            {
                return WhoisLookupType.Unknown;
            }

            LookupItem item = GetLookupItem(entry);

            if (item.PartCount < 2)
            {
                return WhoisLookupType.Unknown;
            }

            if (item.NumberCount == item.PartCount && item.PartCount == 4)
            {
                return WhoisLookupType.IpAddress;
            }

            if (item.NumberCount > 0)
            {
                return WhoisLookupType.Unknown;
            }

            return WhoisLookupType.Domain;
        }

        private LookupItem GetLookupItem(string entry)
        {
            parts = entry.Split('.');

            LookupItem result = new LookupItem();

            result.PartCount = parts.Length;

            int intPart;

            for (int k = 0; k < parts.Length; k++)
            {
                if (Int32.TryParse(parts[k], out intPart))
                {
                    result.NumberCount++;
                }
            }
            return result;
        }

        private struct LookupItem
        {
            public int NumberCount;
            public int PartCount;
        }
        #endregion
    }
}
