using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Restless.Tools.Utility;
using Restless.Tools.Resources;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents an object that encapsulate a single row. This class must be inherited.
    /// </summary>
    /// <typeparam name="T">The table type to which the row belongs</typeparam>
    public abstract class RowObjectBase<T> where T : TableBase
    {
        /// <summary>
        /// Gets the data row that is the underlying basis for this object.
        /// </summary>
        public DataRow Row
        {
            get;
            private set;
        }

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RowObjectBase{T}"/> class.
        /// </summary>
        /// <param name="row">The data row</param>
        protected RowObjectBase(DataRow row)
        {
            Validations.ValidateNull(row, "RowObjectBase.Row");
            if (row.Table.GetType() != typeof(T))
            {
                throw new InvalidOperationException(Strings.InvalidOperation_DataRowTableMismatch);
            }
            Row = row;
        }
        #endregion

        /************************************************************************/
        
        #region Protected methods
        /// <summary>
        /// Gets an Int64 value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Int64 value.</returns>
        protected Int64 GetInt64(string colName)
        {
            return (Int64)Row[colName];
        }

        /// <summary>
        /// Gets a string value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The string value.</returns>
        protected string GetString(string colName)
        {
            return Row[colName].ToString();
        }


        /// <summary>
        /// Gets a DateTime value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The DateTime value.</returns>
        protected DateTime GetDateTime(string colName)
        {
            return (DateTime)Row[colName];
        }

        /// <summary>
        /// Sets an Int64 value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        protected void SetValue(string colName, Int64 value)
        {
            Row[colName] = value;
        }

        /// <summary>
        /// Sets a string value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        protected void SetValue(string colName, string value)
        {
            Row[colName] = value;
        }

        /// <summary>
        /// Sets a DateTime value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        protected void SetValue(string colName, DateTime value)
        {
            Row[colName] = value;
        }
        #endregion
    }
}
