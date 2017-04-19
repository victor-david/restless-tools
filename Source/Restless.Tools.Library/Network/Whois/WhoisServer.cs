using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restless.Tools.Utility;

namespace Restless.Tools.Network.Whois
{
    /// <summary>
    /// Represents a server for performing a WhoIs lookup.
    /// </summary>
    public class WhoisServer
    { 
        #region Private fields
        private static WhoisServer defaultServer;
        #endregion

        /************************************************************************/

        #region Public Fields
        /// <summary>
        /// The default port for a Whois lookup.
        /// </summary>
        public const int DefaultPort = 43;

        /// <summary>
        /// Gets the default (automatic) server. 
        /// When this server is used, the actual server is determined based upon the query
        /// </summary>
        public static WhoisServer DefaultServer
        {
            get
            {
                if (defaultServer == null)
                {
                    defaultServer = new WhoisServer("Automatic");
                    defaultServer.IsDefault = true;
                }
                return defaultServer;
            }
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the server name.
        /// </summary>
        public string ServerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the port for this server.
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value that indicates if this is the default (automatic) server, 
        /// one that determines the server based on the query.
        /// </summary>
        public bool IsDefault
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Creates a new instance of the WhoisServer class.
        /// </summary>
        /// <param name="serverName">The name of the server.</param>
        /// <param name="port">The port.</param>
        public WhoisServer(string serverName, int port)
        {
            Validations.ValidateNull(serverName, "serverName");
            serverName = serverName.Trim();
            Validations.ValidateNullEmpty(serverName, "serverName");
            ServerName = serverName;
            Port = port;
            IsDefault = false;
        }

        /// <summary>
        /// Creates a new instance of the WhoisServer class using the default port.
        /// </summary>
        /// <param name="serverName">The name of the server.</param>
        public WhoisServer(string serverName)
            : this(serverName, DefaultPort)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>true if this object is equal (same server and port) to the other object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            WhoisServer other = obj as WhoisServer;
            if (other != null)
            {
                return other.ServerName == ServerName && other.Port == Port;
            }
            return false;
        }

        /// <summary>
        /// Gets a hash code for this object.
        /// </summary>
        /// <returns>A hash code</returns>
        public override int GetHashCode()
        {
            return ServerName.GetHashCode() + Port;
        }

        /// <summary>
        /// Gets a friendly string for this object.
        /// </summary>
        /// <returns>The friendly string.</returns>
        public override string ToString()
        {
            if (Port == DefaultPort)
            {
                return ServerName;
            }
            return String.Format("{0}:{1}", ServerName, Port); 
        }
        #endregion
    }
}
