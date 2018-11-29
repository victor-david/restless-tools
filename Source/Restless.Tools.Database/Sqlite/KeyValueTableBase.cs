using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Provides base methods for managing an application table that uses 
    /// a simple key/value mechanism to store configuration. This class must be inherited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class handles the underlying details of fetching values from the table and creating
    /// them as needed. A derived class uses the GetItem and SetItem methods from its property getters
    /// and setters. The first time a value is requested, it is created with its specified default value
    /// if it doesn't already exist and saved into the table.
    /// </para>
    /// <para>
    /// When a value is set, this class first checks to make sure that the incoming value
    /// is different from the value that currently exists. If they are the same, the data
    /// row for that value remains unchanged. If the incoming value is different, then the
    /// data row is updated and the <see cref="OnPropertyChanged(string)"/> virtual method
    /// is called. A derived class cam override this method to receive notification,
    /// for instance, to implement INotifyPropertyChanged.
    /// </para>
    /// <para>
    /// The <see cref="TableBase"/> object that is passed to the constructor must adhere
    /// to certain specifications. It must contain two string (Sqlite TEXT) columns, one
    /// named <see cref="ColumnNameId"/> ('id', primary key) and the other 
    /// named <see cref="ColumnNameValue"/> ('value', nullable). There should be no other
    /// columns defined.
    /// </para>
    /// </remarks>
    public abstract class KeyValueTableBase
    {
        #region Public fields
        /// <summary>
        /// Provides the column name of the id column, 'id'.
        /// </summary>
        public const string ColumnNameId = "id";

        /// <summary>
        /// Provides the column name of the value column, 'value'.
        /// </summary>
        public const string ColumnNameValue = "value";
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the table associated with this instance.
        /// </summary>
        public TableBase Table
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueTableBase"/> class
        /// </summary>
        protected KeyValueTableBase(TableBase configTable)
        {
            Table = configTable ?? throw new ArgumentNullException(nameof(configTable));
            ThrowIfTableRequirementsNotMet();
        }
        #endregion

        /************************************************************************/

        #region Protected methods (GetItem)
        /// <summary>
        /// Gets a string value from <see cref="Table"/>.
        /// </summary>
        /// <param name="defaultValue">The default value to add to the table if an entry for <paramref name="id"/> doesn't yet exist.</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        /// <returns>The value.</returns>
        protected string GetItem(string defaultValue, [CallerMemberName] string id = null)
        {
            return GetValueFromRow(id, defaultValue);
        }

        /// <summary>
        /// Gets an int value from <see cref="Table"/>.
        /// </summary>
        /// <param name="defaultValue">The default value to add to the table if an entry for <paramref name="id"/> doesn't yet exist.</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        /// <returns>The value.</returns>
        protected int GetItem(int defaultValue, [CallerMemberName] string id = null)
        {
            if (int.TryParse(GetValueFromRow(id, defaultValue), out int val))
            {
                return val;
            }
            return 0;
        }

        /// <summary>
        /// Gets a long value from <see cref="Table"/>.
        /// </summary>
        /// <param name="defaultValue">The default value to add to the table if an entry for <paramref name="id"/> doesn't yet exist.</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        /// <returns>The value.</returns>
        protected long GetItem(long defaultValue, [CallerMemberName] string id = null)
        {
            if (long.TryParse(GetValueFromRow(id, defaultValue), out long val))
            {
                return val;
            }
            return 0;
        }

        /// <summary>
        /// Gets a double value from <see cref="Table"/>.
        /// </summary>
        /// <param name="defaultValue">The default value to add to the table if an entry for <paramref name="id"/> doesn't yet exist.</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        /// <returns>The value.</returns>
        protected double GetItem(double defaultValue, [CallerMemberName] string id = null)
        {
            if (double.TryParse(GetValueFromRow(id, defaultValue), out double val))
            {
                return val;
            }
            return 0;
        }

        /// <summary>
        /// Gets a boolean value from <see cref="Table"/>.
        /// </summary>
        /// <param name="defaultValue">The default value to add to the table if an entry for <paramref name="id"/> doesn't yet exist.</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        /// <returns>The value.</returns>
        protected bool GetItem(bool defaultValue, [CallerMemberName] string id = null)
        {
            string val = GetValueFromRow(id, defaultValue);
            return val.ToLower() == "true";
        }
        #endregion

        /************************************************************************/

        #region Protected methods (SetItem)
        /// <summary>
        /// Sets the specified string value in <see cref="Table"/>
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        protected void SetItem(string value, [CallerMemberName] string id = null)
        {
            SetRowValueIf(id, value);
        }

        /// <summary>
        /// Sets the specified int value in <see cref="Table"/>
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        protected void SetItem(int value, [CallerMemberName] string id = null)
        {
            SetRowValueIf(id, value.ToString());
        }

        /// <summary>
        /// Sets the specified long value in <see cref="Table"/>
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        protected void SetItem(long value, [CallerMemberName] string id = null)
        {
            SetRowValueIf(id, value.ToString());
        }

        /// <summary>
        /// Sets the specified bool value in <see cref="Table"/>
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="id">The id. Pass null (the default) to use CallerMemberName</param>
        protected void SetItem(bool value, [CallerMemberName] string id = null)
        {
            SetRowValueIf(id, value.ToString());
        }
        #endregion

        /************************************************************************/

        #region Protected methods (other)
        /// <summary>
        /// Called when a property value changes.
        /// Override this method in a derived class if you need
        /// notification, for instance to implement INotifyPropertyChanged.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="propertyId">The name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyId)
        {
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void ThrowIfTableRequirementsNotMet()
        {
            int colCount = Table.Columns.Count;
            var col1 = Table.Columns[ColumnNameId];
            var col2 = Table.Columns[ColumnNameValue];

            if (col1 == null || col2 == null || col1.DataType != typeof(string) || col2.DataType != typeof(string) || colCount < 2 || colCount > 3)
            {
                StringBuilder b = new StringBuilder();
                b.AppendLine("Specified table does not meet the requirements.");
                b.AppendLine($"The table must have two columns named '{ColumnNameId}' and '{ColumnNameValue}'");
                b.AppendLine("The two columns must each be of type string.");
                throw new ArgumentException(b.ToString());
            }
        }

        private string GetValueFromRow(string id, object defaultValue)
        {
            DataRow row = GetRow(id, defaultValue);
            return row[ColumnNameValue].ToString();
        }

        private DataRow GetRow(string id, object defaultValue = null)
        {
            return GetConfigurationRow(id, defaultValue);
        }

        private void SetRowValueIf(string id, string value)
        {
            DataRow row = GetRow(id);
            string currentValue = row[ColumnNameValue].ToString();
            if (currentValue != value)
            {
                row[ColumnNameValue] = value;
                OnPropertyChanged(id);
            }
        }

        /// <summary>
        /// Gets the configuration row with the specified id. Adds a row first, if it doesn't already exist.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <param name="value">The initial value</param>
        /// <returns>The data row</returns>
        /// <remarks>
        /// If the configuration value specified by <paramref name="id"/> does not already exist, this method first creates it.
        /// </remarks>
        private DataRow GetConfigurationRow(string id, object value)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            
            DataRow[] rows = Table.Select($"{ColumnNameId}='{id}'");
            if (rows.Length == 1) return rows[0];

            DataRow row = Table.NewRow();
            row[ColumnNameId] = id;
            if (value == null)
                row[ColumnNameValue] = DBNull.Value;
            else
                row[ColumnNameValue] = value;
            Table.Rows.Add(row);
            Table.Save();
            return row;
        }
        #endregion
    }
}
