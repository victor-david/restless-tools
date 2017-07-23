using Restless.Tools.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Tools.Network
{
    /// <summary>
    /// Represents the base class for request data
    /// </summary>
    public class RequestData
    {
        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the uri for the request.
        /// </summary>
        public string Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user id for this request, or null if not needed.
        /// </summary>
        public string UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the password for this request, or null if not needed.
        /// </summary>
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the content for the request.
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the request content type.
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether auto redirect is enabled for the request. The default is true.
        /// </summary>
        public bool AllowAutoRedirect
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestData"/> class.
        /// </summary>
        /// <param name="uri">The uri for the request.</param>
        public RequestData(string uri) : this(uri, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestData"/> class.
        /// </summary>
        /// <param name="uri">The uri for the request.</param>
        /// <param name="userId">The user id for the request.</param>
        /// <param name="password">The password for the request.</param>
        public RequestData(string uri, string userId, string password)
        {
            Validations.ValidateNullEmpty(uri, "RequestData.Uri");
            Uri = uri;
            AllowAutoRedirect = true;
        }
        #endregion

        /************************************************************************/
    }
}
