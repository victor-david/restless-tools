using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;

namespace Restless.Tools.OpenXml
{
    /// <summary>
    /// Represents a set of extended document properties.
    /// </summary>
    public class ExtendedProperties
    {
        #region Public properties
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        public string Company
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedProperties"/> class.
        /// </summary>
        /// <param name="part">The extended file properties part.</param>
        internal ExtendedProperties(ExtendedFilePropertiesPart part)
        {
            if (part != null && part.Properties != null)
            {
                if (part.Properties.Company != null)
                {
                    Company = part.Properties.Company.Text;
                }
            }
        }
        #endregion

    }
}
