using Restless.Tools.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Tools.Network
{
    /// <summary>
    /// Provides underlying services for making an Http / Https request.
    /// </summary>
    public class RequestManager
    {
        #region Private
        private CookieContainer cookies = new CookieContainer();

        #endregion

        /// <summary>
        /// Sends a post request and returns the response.
        /// </summary>
        /// <param name="data">The request data.</param>
        /// <returns>The response from the remote server.</returns>
        public HttpWebResponse SendPostRequest(RequestData data)
        {
            HttpWebRequest request = GenerateRequest(data, "POST");
            return GetResponse(request);
        }

        /// <summary>
        /// Sends a get request and returns the response.
        /// </summary>
        /// <param name="data">The request data.</param>
        /// <returns>The response from the remote server.</returns>
        public HttpWebResponse SendGetRequest(RequestData data)
        {
            HttpWebRequest request = GenerateRequest(data, "GET");
            return GetResponse(request);
        }

        /// <summary>
        /// Gets the response as a string.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <returns>The contents of the response.</returns>
        public string GetResponseContent(HttpWebResponse response)
        {
            Validations.ValidateNull(response, "GetResponseContent.Response");
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;

            try
            {
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Cleanup the streams and the response.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                response.Close();
            }
            return responseFromServer;
        }

        /************************************************************************/

        #region Private methods

        private HttpWebRequest GenerateRequest(RequestData data, string method)
        {
            Validations.ValidateNull(data, "GenerateRequest.Data");

            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(data.Uri);
            // Set the Method property of the request to POST.
            request.Method = method;
            // Set cookie container to maintain cookies
            request.CookieContainer = cookies;
            request.AllowAutoRedirect = data.AllowAutoRedirect;
            // If login is empty use defaul credentials
            if (string.IsNullOrEmpty(data.UserId))
            {
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                request.Credentials = new NetworkCredential(data.UserId, data.Password);
            }

            if (method == "POST" && !String.IsNullOrEmpty(data.Content))
            {
                // Convert POST data to a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(data.Content);
                // Set the ContentType property of the WebRequest.
                request.ContentType = data.ContentType; // "application/x-www-form-urlencoded";
                // Set the Accept header
                request.Accept = data.ContentType;
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
            }
            return request;
        }

        private HttpWebResponse GetResponse(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                cookies.Add(response.Cookies);
                //// Print the properties of each cookie.
                //Console.WriteLine("\nCookies: ");
                //foreach (Cookie cook in cookies.GetCookies(request.RequestUri))
                //{
                //    Console.WriteLine("Domain: {0}, String: {1}", cook.Domain, cook.ToString());
                //}
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        private string GetCookieValue(Uri siteUri, string name)
        {
            Cookie cookie = cookies.GetCookies(siteUri)[name];
            return (cookie == null) ? null : cookie.Value;
        }
        #endregion

    }
}
