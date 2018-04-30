﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Restless.Tools.Utility;
using Restless.Tools.Resources;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents an object that encapsulate a single row. This class must be inherited.
    /// </summary>
    /// <typeparam name="T">The table type to which the row belongs</typeparam>
    public abstract class RowObjectBase<T>  where T : TableBase 
    {
        #region Public properties
        /// <summary>
        /// Gets the data row that is the underlying basis for this object.
        /// </summary>
        public DataRow Row
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the table that is the underlying basis for this object.
        /// </summary>
        public T Table
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value that indicates if this row object is selected.
        /// </summary>
        /// <remarks>
        /// This property is not used by the base class. It is provided as a convienance
        /// property for use in data binding situations.
        /// </remarks>
        public bool IsSelected
        {
            get;
            set;
        }
        #endregion

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
            Table = (T)row.Table;
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
            if (Row[colName] != DBNull.Value)
            {
                return (Int64)Row[colName];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets a Decimal value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Decimal value.</returns>
        protected Decimal GetDecimal(string colName)
        {
            if (Row[colName] != DBNull.Value)
            {
                return (Decimal)Row[colName];
            }
            else
            {
                return 0;
            }
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
            if (Row[colName] != DBNull.Value)
            {
                return (DateTime)Row[colName];
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets a nullable DateTime value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The DateTime value, or null.</returns>
        protected DateTime? GetNullableDateTime(string colName)
        {
            if (Row[colName] is DateTime)
            {
                return (DateTime)Row[colName];
            }
            return null;
        }
        
        /// <summary>
        /// Gets a Boolean value from the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <returns>The Boolean value.</returns>
        protected bool GetBoolean(string colName)
        {
            if (Row[colName] != DBNull.Value)
            {
                return (bool)Row[colName];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets an Int64 value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, Int64 value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets an nullable Int64 value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, Int64? value)
        {
            if (!Row[colName].Equals(value))
            {
                if (value == null)
                {
                    Row[colName] = DBNull.Value;
                }
                else
                {
                    Row[colName] = value;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a Decimal value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, Decimal value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a nullable Decimal value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, Decimal? value)
        {
            if (!Row[colName].Equals(value))
            {
                if (value == null)
                {
                    Row[colName] = DBNull.Value;
                }
                else
                {
                    Row[colName] = value;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a string value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, string value)
        {
            if (!Row[colName].Equals(value))
            {
                if (String.IsNullOrEmpty(value))
                {
                    Row[colName] = DBNull.Value;
                }
                else
                {
                    Row[colName] = value;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a DateTime value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, DateTime value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a nullable DateTime value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, DateTime? value)
        {
            if (!Row[colName].Equals(value))
            {
                if (value == null)
                {
                    Row[colName] = DBNull.Value;
                }
                else
                { 
                    Row[colName] = value;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a Boolean value on the specified column.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the column value was set; false if the column value was already the same as <paramref name="value"/>.</returns>
        protected bool SetValue(string colName, bool value)
        {
            if (!Row[colName].Equals(value))
            {
                Row[colName] = value;
                return true;
            }
            return false;
        }
        #endregion
    }
}
