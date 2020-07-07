using Restless.Tools.Database.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Extends <see cref="TableBase"/> to provide a base class for application tables. This class must be inherited.
    /// </summary>
    public abstract class ExtendedTableBase : TableBase
    {
        #region Public fields
        /// <summary>
        /// Gets the default name of the primary key column.
        /// </summary>
        public const string DefaultPrimaryKeyName = "id";
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedTableBase"/> class using the specified schema name.
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="tableName">The table name</param>
        protected ExtendedTableBase(DatabaseControllerBase controller, string schemaName, string tableName) : base(controller, schemaName, tableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the name of the primary key column.
        /// </summary>
        public override string PrimaryKeyName => DefaultPrimaryKeyName;
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Sets extended properties on certain columns. See the base implemntation <see cref="TableBase.SetColumnProperties"/> for more information.
        /// </summary>
        protected override void SetColumnProperties()
        {
            SetColumnProperty(Columns[DefaultPrimaryKeyName], DataColumnPropertyKey.ExcludeFromInsert, DataColumnPropertyKey.ExcludeFromUpdate, DataColumnPropertyKey.ReceiveInsertedId);
        }

        /// <summary>
        /// Gets column definitions.
        /// </summary>
        /// <returns>A collection of <see cref="ColumnDefinition"/> objects.</returns>
        protected abstract ColumnDefinitionCollection GetColumnDefinitions();

        /// <summary>
        /// Gets the primary key definition
        /// </summary>
        /// <returns>A collection of strings</returns>
        /// <remarks>
        /// You only need to override this method if you're creating composite primary keys.
        /// If only a single field is the primary key, that can be defined in <see cref="ColumnDefinition"/>.
        /// </remarks>
        protected virtual PrimaryKeyCollection GetPrimaryKeyDefinition()
        {
            return null;
        }

        /// <summary>
        /// Gets the DDL needed to create this table.
        /// </summary>
        /// <returns>A SQL string that describes how to create this table.</returns>
        /// <remarks>
        /// This method uses <see cref="MakeDdl"/> to obtain the DDL. Override if you need other logic.
        /// </remarks>
        protected override string GetDdl()
        {
            return MakeDdl();
        }

        /// <summary>
        /// Makes the DDL string used to create the table.
        /// </summary>
        /// <returns>The ddl string.</returns>
        protected string MakeDdl()
        {
            var colDefs = GetColumnDefinitions() ?? throw new ArgumentNullException();
            string cols = string.Join(",", colDefs);
            var pk = GetPrimaryKeyDefinition();

            StringBuilder builder = new StringBuilder(512);
            // ddl
            builder.AppendLine($"CREATE TABLE {{NS}}.{{NAME}} ({cols}{pk});");

            // indices
            foreach (var def in colDefs)
            {
                if (def.Index != IndexType.None)
                {
                    string unique = def.Index == IndexType.Unique ? "UNIQUE" : string.Empty;
                    builder.AppendLine($"CREATE {unique} INDEX {{NS}}.{{NAME}}__{def.Name} ON {{NAME}} ({def.Name});");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Gets the sql statements needed to populate this table with its initial values.
        /// </summary>
        /// <returns>A string, or null if none needed</returns>
        /// <remarks>
        /// This method uses <see cref="GetPopulateColumnList"/> and <see cref="EnumeratePopulateValues"/>
        /// to obtain the statements needed to populate the table. Override if you need other logic.
        /// </remarks>
        protected override string GetPopulateSql()
        {
            var columns = GetPopulateColumnList();
            if (columns != null && columns.Count > 0)
            {
                string columnStr = string.Join(",", columns);
                StringBuilder builder = new StringBuilder(512);
                foreach (var values in EnumeratePopulateValues())
                {
                    builder.Append($"INSERT INTO {{NS}}.{{NAME}} ({columnStr}) VALUES(");
                    foreach (var value in values)
                    {
                        builder.Append($"{ValueToString(value)},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.AppendLine(");");
                }
                return builder.ToString();
            }
            return null;
        }

        private string ValueToString(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            if (value is DateTime dt)
            {
                return SingleQuoted(dt.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (value is bool b)
            {
                return b ? "1" : "0";
            }
            return SingleQuoted(value.ToString());
        }

        private string SingleQuoted(string value)
        {
            return $"'{value}'";
        }

        /// <summary>
        /// Gets a list of column names to use in subsequent initial insert operations.
        /// These are used only when the table is empty, i.e. upon first creation.
        /// </summary>
        /// <returns>A list of column names</returns>
        /// <remarks>
        /// You must override this method and <see cref="EnumeratePopulateValues"/> if you need to pre-populate the table.
        /// The base method returns null.
        /// </remarks>
        protected virtual List<string> GetPopulateColumnList()
        {
            return null;
        }

        /// <summary>
        /// Provides an enumerable that returns values for each row to be populated.
        /// </summary>
        /// <returns>An IEnumerable</returns>
        /// <remarks>
        /// You must override this method and <see cref="GetPopulateColumnList"/> if you need to pre-populate the table.
        /// Each return object array must correspond to the column list returned by <see cref="GetPopulateColumnList"/>.
        /// The base method returns zero objects.
        /// </remarks>
        protected virtual IEnumerable<object[]> EnumeratePopulateValues()
        {
            yield break;
        }

        /// <summary>
        /// Gets a unique row from an array of rows.
        /// </summary>
        /// <param name="rows">The rows. Throws if length greater than one.</param>
        /// <returns>The DataRow, or null if length of <paramref name="rows"/> is zero.</returns>
        protected DataRow GetUniqueRow(DataRow[] rows)
        {
            if (rows == null) throw new ArgumentNullException(nameof(rows));
            if (rows.Length > 1) throw new InvalidOperationException("Row count greater than one");
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// Makes a new row, calls <paramref name="populate"/>,
        /// adds the new row to the rows collection, and saves the table.
        /// </summary>
        /// <param name="populate">The populare action.</param>
        /// <returns>The newly added DataRow.</returns>
        protected DataRow MakeNewRow(Action<DataRow> populate)
        {
            if (populate == null) throw new ArgumentNullException(nameof(populate));
            DataRow row = NewRow();
            populate(row);
            Rows.Add(row);
            Save();
            return row;
        }
        #endregion
    }
}
