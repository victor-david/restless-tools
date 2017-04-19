using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using Restless.Tools.Utility;
using Restless.Tools.Resources;

namespace Restless.Tools.Network.Whois
{
    /// <summary>
    /// Provides an object to perform Whois lookups.
    /// </summary>
    public sealed class WhoisLookup
    {
        #region Private fields
        private const string IPLookupServer = "whois.arin.net";
        private const string TKLookupServer = "whois.dot.tk";
        private const string AccountRequired = "'.pro','.name', and '.tv' domains require an account for a whois";
        private const int ConnectTimeout = 5000;
        private List<string> followPhrase;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets a boolean value that is used when validating lookup input.
        /// When this property is true, only a doman name or an ip address is acceptable,
        /// otherwise any input that isn't an empty string is okay. The default is false.
        /// </summary>
        public bool Strict
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Creates a new instance of the WhoisLookup class.
        /// </summary>
        public WhoisLookup()
        {
            followPhrase = new List<string>();
            followPhrase.Add("whois server:");
            followPhrase.Add("referralserver:");
            followPhrase.Add("referral server:");
            followPhrase.Add("referral rwhois:");
            Strict = false;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Raised when the lookup has been completed.
        /// </summary>
        public event EventHandler<WhoisEventArgs> LookupComplete;

        /// <summary>
        /// Performs a whois lookup on the specified domain or IP.
        /// </summary>
        /// <param name="lookupEntry">The domain or IP address to lookup.</param>
        /// <param name="server">The server to send the request to, or null to use the default.</param>
        public void Lookup(string lookupEntry, WhoisServer server)
        {
            Validations.ValidateNullEmpty(lookupEntry, "Lookup.LookupEntry");
            BeginLookup(lookupEntry, server);
        }

        /// <summary>
        /// Performs a whois lookup on the specified domain or IP, using the default server.
        /// </summary>
        /// <param name="lookupEntry">The domain or IP address to lookup.</param>
        public void Lookup(string lookupEntry)
        {
            Lookup(lookupEntry, null);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void BeginLookup(string lookupEntry, WhoisServer server)
        {
            Validations.ValidateNullEmpty(lookupEntry, "Lookup.LookupEntry");

            var lookupParms = new WhoisLookupParms(lookupEntry);

            if (Strict && lookupParms.LookupType == WhoisLookupType.Unknown)
            {
                throw new InvalidOperationException(Strings.InvalidOperation_WhoisLookupTypeIsUnknown);
            }

            if (server == null || server.IsDefault)
            {
                server = GetServer(lookupParms);
            }

            OnLookupComplete(GetResult(server, lookupParms), server, lookupParms);
        }

        private WhoisLookupResult GetResult(WhoisServer server, WhoisLookupParms lookupParms)
        {
            try
            {
                WhoisLookupResult result = new WhoisLookupResult();

                TcpClient tcpClient = Connect(server.ServerName, server.Port, ConnectTimeout);
                NetworkStream networkStream = tcpClient.GetStream();

                // Send the domain name to the whois server
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(lookupParms.Query + "\r\n");
                networkStream.Write(buffer, 0, buffer.Length);

                // Read back the results
                buffer = new byte[8192];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    result.Text.Append( ASCIIEncoding.ASCII.GetString(buffer,0,bytesRead));
                    bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                    
                }
                networkStream.Close();
                tcpClient.Close();

                result.Text.Replace("\n", Environment.NewLine);

                GetReferalServers(result);

                return result;
            }

            catch (Exception ex)
            {
                string msg = String.Format("Cannot contact {0}", server.ServerName);
                throw new Exception(msg, ex);
            }
        }


        private class State
        {
            public TcpClient Client { get; set; }
            public bool Success { get; set; }
        }

        // http://stackoverflow.com/questions/17118632/how-to-set-the-timeout-for-a-tcpclient

        private TcpClient Connect(string hostName, int port, int timeout)
        {
            var client = new TcpClient();

            //when the connection completes before the timeout it will cause a race
            //we want EndConnect to always treat the connection as successful if it wins
            var state = new State { Client = client, Success = true };

            IAsyncResult ar = client.BeginConnect(hostName, port, EndConnect, state);
            state.Success = ar.AsyncWaitHandle.WaitOne(timeout, false);

            if (!state.Success || !client.Connected)
            {
                throw new Exception("Failed to connect.");
            }

            return client;
        }

        private void EndConnect(IAsyncResult ar)
        {
            var state = (State)ar.AsyncState;
            TcpClient client = state.Client;

            try
            {
                client.EndConnect(ar);
            }
            catch { }

            if (client.Connected && state.Success)
            {
                return;
            }

            client.Close();
        }
        

        /// <summary>
        /// Raises the LookupCompleted event.
        /// </summary>
        /// <param name="result">The result of the lookup.</param>
        /// <param name="server">The server than completed the lookup request.</param>
        /// <param name="lookup">The lookup parms that were used.</param>
        private void OnLookupComplete(WhoisLookupResult result, WhoisServer server, WhoisLookupParms lookup)
        {
            if (LookupComplete != null)
            {
                WhoisEventArgs e = new WhoisEventArgs(result,server, lookup);
                LookupComplete(this, e);
            }
        }

        private WhoisServer GetServer(WhoisLookupParms lookup)
        {
            if (lookup.LookupType == WhoisLookupType.IpAddress || lookup.LookupType == WhoisLookupType.Unknown)
            {
                return new WhoisServer(IPLookupServer);
            }

            if (lookup.TopLevelDomain == "tk")
            {
                return new WhoisServer(TKLookupServer);
            }

            return new WhoisServer(String.Format("{0}.whois-servers.net", lookup.TopLevelDomain));
        }

        /// <summary>
        /// Parses the results text and adds servers discovered to the result.
        /// </summary>
        /// <param name="result"></param>
        private void GetReferalServers(WhoisLookupResult result)
        {
            string[] lines = result.Text.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string lowLine = line.ToLower();
                foreach (string phrase in followPhrase)
                {
                    if (lowLine.Contains(phrase))
                    {
                        string[] parts = lowLine.Split(':');
                        for (int k = 0; k < parts.Length; k++)
                        {
                            parts[k] = parts[k].Replace("/", "");
                            string[] subParts = parts[k].Split('.');
                            if (subParts.Length > 2 &&
                                (subParts[0].Trim() == "whois" ||
                                 subParts[0].Trim() == "rwhois") )
                            {
                                int port;
                                if (Int32.TryParse(parts[parts.Length - 1], out port))
                                {
                                    result.AddReferralServer(new WhoisServer(parts[k].Trim(), port));
                                }
                                result.AddReferralServer(new WhoisServer(parts[k].Trim()));
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}

