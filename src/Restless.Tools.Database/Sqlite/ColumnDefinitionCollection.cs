using System.Collections;
using System.Collections.Generic;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents a collection of <see cref="ColumnDefinition"/> objects.
    /// </summary>
    public class ColumnDefinitionCollection : IEnumerable<ColumnDefinition>
    {
        #region Private
        private readonly List<ColumnDefinition> storage;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinitionCollection"/> class.
        /// </summary>
        public ColumnDefinitionCollection()
        {
            storage = new List<ColumnDefinition>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a column definition to the collection.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="type">The column type.</param>
        /// <param name="isPrimaryKey">true if this column is the primary key.</param>
        /// <param name="isNullable">true if this column is nullable.</param>
        /// <param name="defaultValue">The default value for the column.</param>
        /// <param name="index">The index type to create based on this column.</param>
        public void Add(string colName, ColumnType type, bool isPrimaryKey, bool isNullable, object defaultValue, IndexType index)
        {
            storage.Add(new ColumnDefinition(colName, type, isPrimaryKey, isNullable, defaultValue, index));
        }

        /// <summary>
        /// Adds a column definition to the collection.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="type">The column type.</param>
        /// <param name="isPrimaryKey">true if this column is the primary key.</param>
        /// <param name="isNullable">true if this column is nullable.</param>
        /// <param name="defaultValue">The default value for the column.</param>
        public void Add(string colName, ColumnType type, bool isPrimaryKey, bool isNullable, object defaultValue)
        {
            Add(colName, type, isPrimaryKey, isNullable, defaultValue, index: IndexType.None);
        }

        /// <summary>
        /// Adds a column definition to the collection.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="type">The column type.</param>
        /// <param name="isPrimaryKey">true if this column is the primary key.</param>
        /// <param name="isNullable">true if this column is nullable.</param>
        public void Add(string colName, ColumnType type, bool isPrimaryKey, bool isNullable)
        {
            Add(colName, type, isPrimaryKey, isNullable, defaultValue:null, index: IndexType.None);
        }

        /// <summary>
        /// Adds a column definition to the collection.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="type">The column type.</param>
        /// <param name="isPrimaryKey">true if this column is the primary key.</param>
        public void Add(string colName, ColumnType type, bool isPrimaryKey)
        {
            Add(colName, type, isPrimaryKey, isNullable:false, defaultValue: null, index: IndexType.None);
        }

        /// <summary>
        /// Adds a column definition to the collection.
        /// </summary>
        /// <param name="colName">The column name.</param>
        /// <param name="type">The column type.</param>
        public void Add(string colName, ColumnType type)
        {
            Add(colName, type, isPrimaryKey:false, isNullable: false, defaultValue: null, index: IndexType.None);
        }
        #endregion

        /************************************************************************/

        #region IEnumerable implementation
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<ColumnDefinition> GetEnumerator()
        {
            return ((IEnumerable<ColumnDefinition>)storage).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ColumnDefinition>)storage).GetEnumerator();
        }
        #endregion
    }
}
