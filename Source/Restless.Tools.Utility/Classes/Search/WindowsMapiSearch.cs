using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Restless.Tools.Utility;
using PropSystem = Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SysProps = Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;

namespace Restless.Tools.Utility.Search
{
    /// <summary>
    /// Extends <see cref="WindowsSearchBase"/> to provide MAPI search capability.
    /// </summary>
    public class WindowsMapiSearch : WindowsSearchBase
    {
        #region Private
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets a list of MAPI types to be included in the search.
        /// </summary>
        public List<string> IncludedTypes
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        #pragma warning disable 1591
        public WindowsMapiSearch()
        {
            IncludedTypes = new List<string>();
        }
        #pragma warning restore 1591
        #endregion

        /************************************************************************/
        
        #region Public methods
        /// <summary>
        /// Gets the search results for the specified expression
        /// </summary>
        /// <param name="expression">The expression</param>
        /// <returns>A WindowsSearchResultCollection object</returns>
        public override WindowsSearchResultCollection GetSearchResults(string expression)
        {
            StringBuilder sql = new StringBuilder(512);
            sql.AppendFormat("SELECT {0}", GetSelectFieldList());
            sql.Append(" FROM SystemIndex");
            sql.Append(" WHERE (");
            if (Scopes.Count == 0)
            {
                sql.Append("scope='mapi:'");
            }
            else
            {
                for (int k = 0; k < Scopes.Count; k++)
                {
                    sql.AppendFormat("scope='mapi:{0}'", Scopes[k]);
                    sql.Append(k < Scopes.Count - 1 ? " OR " : string.Empty);
                }
            }
            sql.Append(")");

            if (!string.IsNullOrEmpty(expression))
            {
                sql.AppendFormat(" AND CONTAINS(\"ALL\",'\"{0}*\"')", expression);
            }

            if (IncludedTypes.Count != 0)
            {
                string propertyName = GetPropName(SysProps.System.ItemType);
                sql.Append(" AND(");
                for (int k = 0; k < IncludedTypes.Count; k++)
                {
                    sql.AppendFormat("{0}='{1}'", propertyName, IncludedTypes[k]);
                    sql.Append(k < IncludedTypes.Count - 1 ? " OR " : string.Empty);
                }
                sql.Append(")");

            }

            AppendOrderBy(sql);
            return GetCollection(sql.ToString());
        }

        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
