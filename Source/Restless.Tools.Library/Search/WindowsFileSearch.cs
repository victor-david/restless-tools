using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Restless.Tools.Resources;
using Restless.Tools.Utility;
using PropSystem = Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SysProps = Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;

namespace Restless.Tools.Search
{
    /// <summary>
    /// Extends <see cref="WindowsSearchBase"/> to provide file search capability.
    /// </summary>
    public class WindowsFileSearch : WindowsSearchBase
    {
        #region Private
        //private const string ConnectionStr = "Provider=Search.CollatorDSO;Extended Properties=\"Application=Windows\"";
        //private const string Rank = "rank";
        //private List<PropSystem.PropertyKey> keys;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets a list of scopes to be excluded from the search. Only applies to file search
        /// </summary>
        public List<string> ExcludedScopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of types to be excluded from the search. Only applies to file search.
        /// Default exclusions are "Directory", ".dll", ".exe", ".zip", ".cmd"
        /// </summary>
        public List<string> ExcludedTypes
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        #pragma warning disable 1591
        public WindowsFileSearch()
        {
            ExcludedScopes = new List<string>();
            ExcludedTypes = new List<string>();
            ExcludedTypes.Add("Directory");
            ExcludedTypes.Add(".dll");
            ExcludedTypes.Add(".exe");
            ExcludedTypes.Add(".zip");
            ExcludedTypes.Add(".cmd");
            ExcludedTypes.Add(".pspimage");
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
            Validations.ValidateInvalidOperation(Scopes.Count == 0, Strings.InvalidOperation_NoSearchScopeSpecified);
            var result = new List<WindowsSearchResult>();

            StringBuilder sql = new StringBuilder(512);
            sql.AppendFormat("SELECT {0}", GetSelectFieldList());

            sql.Append(" FROM SystemIndex");
            sql.Append(" WHERE (");
            for (int k = 0; k < Scopes.Count; k++)
            {
                sql.AppendFormat(" scope='file:{0}'", Scopes[k]);
                sql.Append(k < Scopes.Count - 1 ? " OR " : String.Empty);

            }
            sql.Append(")");

            foreach (string scope in ExcludedScopes)
            {
                if (!String.IsNullOrEmpty(scope))
                {
                    sql.AppendFormat(" AND NOT scope='file:{0}'", scope);
                }
            }

            sql.AppendFormat(" AND CONTAINS(\"ALL\",'\"{0}*\"')", expression);
            //sql.AppendFormat(" AND FREETEXT('{0}*')", expression);

            foreach (string type in ExcludedTypes)
            {
                sql.AppendFormat(" AND {0} != '{1}'", GetPropName(SysProps.System.ItemType), type);
            }

            AppendOrderBy(sql);
            return GetCollection(sql.ToString());
        }


        #endregion

    }
}
