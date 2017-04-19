using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Utility helper class for obtaining assembly information.
    /// </summary>
    public sealed class AssemblyInfo
    {
        #region Private Vars
        private Assembly assembly;
        #endregion

        /************************************************************************/

        #region Public Properties
        /// <summary>
        /// Gets the title of the assembly.
        /// </summary>
        public string Title
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(assembly.CodeBase);
            }
        }

        /// <summary>
        /// Gets the full version of the assembly.
        /// </summary>
        public string Version
        {
            get
            {
                var version = assembly.GetName().Version;
                return version.ToString();
            }
        }

        /// <summary>
        /// Gets the major version of the calling assembly.
        /// </summary>
        public string VersionMajor
        {
            get
            {
                var version = assembly.GetName().Version;
                return String.Format("{0}.{1}", version.Major, version.Minor);
            }
        }

        /// <summary>
        /// Gets the framework version of the assembly.
        /// </summary>
        public string FrameworkVersion
        {
            get
            {
                return assembly.ImageRuntimeVersion;
            }
        }

        /// <summary>
        /// Gets the description of the assembly.
        /// </summary>
        public string Description
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Gets the location of the assembly.
        /// </summary>
        public string Location
        {
            get { return assembly.Location; }
        }

        /// <summary>
        /// Gets the product name of the assembly.
        /// </summary>
        public string Product
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Gets the copyright of the assembly.
        /// </summary>
        public string Copyright
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets the company of the assembly.
        /// </summary>
        public string Company
        {
            get
            {
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        /************************************************************************/
        
        #region Constructor
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="assemblyType">The type of assembly information that will be returned from the properties of the instance.</param>
        public AssemblyInfo(AssemblyInfoType assemblyType)
        {
            switch (assemblyType)
            {
                case AssemblyInfoType.Calling:
                    assembly = Assembly.GetCallingAssembly();
                    break;
                case AssemblyInfoType.Executing:
                    assembly = Assembly.GetExecutingAssembly();
                    break;
                default:
                    assembly = Assembly.GetEntryAssembly();
                    break;
            }
        }
        #endregion
    }
}
