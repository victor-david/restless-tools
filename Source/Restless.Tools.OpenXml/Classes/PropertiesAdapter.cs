using DocumentFormat.OpenXml.Packaging;
using System;
using System.IO;

namespace Restless.Tools.OpenXml
{
    /// <summary>
    /// Represents a set of core and extended properties.
    /// </summary>
    public class PropertiesAdapter 
    {
        #region Public properties
        /// <summary>
        /// Gets the file name associated with this set of properties.
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the core properties object.
        /// </summary>
        public CoreProperties Core
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the extended properties object.
        /// </summary>
        public ExtendedProperties Extended
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesAdapter"/> class.
        /// </summary>
        /// <param name="fileName">The name of the file associated with these properties.</param>
        /// <param name="doc">The word processing document.</param>
        internal PropertiesAdapter(string fileName, WordprocessingDocument doc)
        {
            FileName = fileName;
            Core = new CoreProperties(doc.PackageProperties);
            Extended = new ExtendedProperties(doc.ExtendedFilePropertiesPart);
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Saves the properties back into the document.
        /// </summary>
        public void Save()
        {
            DateTime lastWrite = File.GetLastWriteTimeUtc(FileName);
            OpenXmlDocument.Writer.SetProperties(FileName, this);
            File.SetLastWriteTimeUtc(FileName, lastWrite.AddSeconds(OpenXmlDocument.SecondsToAdd));
        }
        #endregion
    }
}
