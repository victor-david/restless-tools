using System.Collections.Generic;
using System.Text;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Provides a list of primary keys.
    /// </summary>
    public class PrimaryKeyCollection : List<string>
    {
        #region Constructor
        internal PrimaryKeyCollection()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A SQL string to be used when creating primary keys.</returns>
        public override string ToString()
        {
            if (Count > 0)
            {
                StringBuilder b = new StringBuilder();
                b.Append(", PRIMARY KEY(");
                foreach (string key in this)
                {
                    b.Append($"\"{key}\", ");
                }
                b.Remove(b.Length - 2, 2);
                b.Append(")");
                return b.ToString();
            }
            return null;
        }
        #endregion
    }
}
