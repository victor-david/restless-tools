using Restless.Tools.Threading;
using Restless.Tools.Utility;
using System;
using System.IO;
using System.Net;
using System.Text;

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

        /************************************************************************/

        #region Public enumeration (RequestType)
        /// <summary>
        /// Enumerates the supported request types.
        /// </summary>
        public enum RequestType
        {
            /// <summary>
            /// A GET request.
            /// </summary>
            Get,
            /// <summary>
            /// A POST request
            /// </summary>
            Post
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the request data for this request
        /// </summary>
        public RequestData Data
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestManager"/> class.
        /// </summary>
        /// <param name="data">The request data</param>
        public RequestManager(RequestData data)
        {
            Data = data ?? throw new ArgumentNullException("RequestManager.Data");
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Sends a post request and returns the response.
        /// </summary>
        /// <returns>The response from the remote server.</returns>
        public HttpWebResponse SendPostRequest()
        {
            HttpWebRequest request = GenerateRequest("POST");
            return GetResponse(request);
        }

        /// <summary>
        /// Sends a get request and returns the response.
        /// </summary>
        /// <returns>The response from the remote server.</returns>
        public HttpWebResponse SendGetRequest()
        {
            HttpWebRequest request = GenerateRequest("GET");
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
            catch (Exception)
            {
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

        /// <summary>
        /// Makes an asychonous network request.
        /// </summary>
        /// <param name="taskId">The task id for the async request</param>
        /// <param name="requestType">The request type</param>
        /// <param name="receivedDataCallback">
        /// A callback method to use when the response has been received.
        /// The method parameter receives the response string.
        /// </param>
        /// <param name="exceptionCallback">
        /// A callback method to use if an exception occurs. Called on the original (normally UI) thread.
        /// Can be null if not needed.
        /// </param>
        /// <param name="completeCallback">
        /// A callback method to use when complete. Called on the original (normally UI) thread.
        /// Can be null if not needed.
        /// </param>
        /// <param name="completeCallbackParm">A parameter to pass to <paramref name="completeCallback"/>.</param>
        public void MakeAsyncRequest
            (
                int taskId,
                RequestType requestType,
                Action<string> receivedDataCallback,
                Action<Exception, HttpWebResponse> exceptionCallback,
                Action<object> completeCallback,
                object completeCallbackParm = null
            )
        {
            Validations.ValidateNull(receivedDataCallback, " MakeAsyncRequest.ReceivedDataCallback");

            TaskManager.Instance.ExecuteTask(taskId, (token) =>
            {
                HttpWebResponse response = GetResponse(requestType);
                string responseStr = GetResponseContent(response);
                receivedDataCallback(responseStr);
            }, null, null, false)

            .HandleExceptions((ex) =>
            {
                if (exceptionCallback != null)
                {
                    string msg = ex.Message;
                    if (msg.StartsWith("The remote server returned an error:"))
                    {
                        msg = msg.Substring(36).Trim();
                    }

                    WebException wex = ex as WebException;
                    HttpWebResponse resp = null;
                    if (wex != null)
                    {
                        resp = wex.Response as HttpWebResponse;
                    }

                    exceptionCallback(new Exception(msg, ex.InnerException), resp);
                }
            })

            .ContinueWith((t) =>
            {
                if (completeCallback != null)
                {
                    TaskManager.Instance.DispatchTask(() =>
                    {
                        completeCallback(completeCallbackParm);
                    });
                }
            });
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private HttpWebRequest GenerateRequest(string method)
        {
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Data.Uri);
            // Set the Method property of the request to POST.
            request.Method = method;
            // Set cookie container to maintain cookies
            request.CookieContainer = cookies;
            request.AllowAutoRedirect = Data.AllowAutoRedirect;
            // If login is empty use defaul credentials
            if (string.IsNullOrEmpty(Data.UserId))
            {
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                request.Credentials = new NetworkCredential(Data.UserId, Data.Password);
            }

            if (method == "POST" && !String.IsNullOrEmpty(Data.Content))
            {
                // Convert POST data to a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(Data.Content);
                // Set the ContentType property of the WebRequest.
                request.ContentType = Data.ContentType; // "application/x-www-form-urlencoded";
                // Set the Accept header
                request.Accept = Data.ContentType;
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

        private HttpWebResponse GetResponse(RequestType type)
        {
            switch (type)
            {
                case RequestType.Post:
                    return SendPostRequest();
                default:
                    return SendGetRequest();
            }
        }

        private string GetCookieValue(Uri siteUri, string name)
        {
            Cookie cookie = cookies.GetCookies(siteUri)[name];
            return (cookie == null) ? null : cookie.Value;
        }
        #endregion

    }
}
