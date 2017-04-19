using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Packaging;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

namespace Restless.Tools.OpenXml
{
    /// <summary>
    /// Represents a set of core document properties.
    /// </summary>
    /// <remarks>
    /// This class implements <see cref="PackageProperties"/> and is used for binding.
    /// </remarks>
    public class CoreProperties : PackageProperties
    {
        #region Public properties
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public override string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content status.
        /// </summary>
        public override string ContentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public override string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created date / time.
        /// </summary>
        public override DateTime? Created
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the creator
        /// </summary>
        public override string Creator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public override string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public override string Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public override string Keywords
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public override string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets last modified by.
        /// </summary>
        public override string LastModifiedBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last printed date / time.
        /// </summary>
        public override DateTime? LastPrinted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the modified date / time.
        /// </summary>
        public override DateTime? Modified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the revision
        /// </summary>
        public override string Revision
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public override string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public override string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public override string Version
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesAdapter"/> class.
        /// </summary>
        /// <param name="props">The core properties.</param>
        internal CoreProperties(PackageProperties props)
        {
            Category = props.Category;
            ContentStatus= props.ContentStatus;
            ContentType = props.ContentType;
            Created = props.Created;
            Creator = props.Creator;
            Description= props.Description;
            Identifier = props.Identifier;
            Keywords = props.Keywords;
            Language = props.Language;
            LastModifiedBy = props.LastModifiedBy;
            LastPrinted = props.LastPrinted;
            Modified = props.Modified;
            Revision = props.Revision;
            Subject = props.Subject;
            Title = props.Title;
            Version = props.Version;
        }
        #endregion
    }
}
