using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropSystem = Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SysProps = Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;

namespace Restless.Tools.Search
{
    /// <summary>
    /// Represents a single result from a Windows search operation
    /// </summary>
    public class WindowsSearchResult
    {
        #region Public properties
        /// <summary>
        /// Gets or sets an object of custom data.
        /// </summary>
        public object Extended
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the values for this search result.
        /// </summary>
        public SearchValueCollection Values
        {
            get;
            private set;
        }
        #endregion
        
        /************************************************************************/
        
        #region Constructor (internal)

        internal WindowsSearchResult()
        {
            Values = new SearchValueCollection();
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        #endregion

        /************************************************************************/
        
        #region Public methods

        /// <summary>
        /// Sets the System.ItemPathDisplay property
        /// </summary>
        /// <param name="value">The value to set</param>
        public void SetItemPathDisplay(string value)
        {
            Values.SetProperty(SysProps.System.ItemPathDisplay, value);
        }

        /// <summary>
        /// Gets a property descriptor for binding (ex: "Values[System.ItemName]") based
        /// upon the specified property key.
        /// </summary>
        /// <param name="key">The property key</param>
        /// <returns>A string that can be used in programatic binding, ex: "Values[System.ItemName]"</returns>
        public static string GetBindingReference(PropSystem.PropertyKey key)
        {
            var desc = SysProps.GetPropertyDescription(key);
            return String.Format("Values[{0}]", desc.CanonicalName);
        }
        #endregion
    }
}
