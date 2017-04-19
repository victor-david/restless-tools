using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Restless.Tools.Utility;
using System.IO;
using System.Data.SQLite;
using System.Collections.Specialized;
using System.Data;
using Restless.Tools.Database.Generic;
using Restless.Tools.Resources;
using System.Diagnostics;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents the base class for a data table
    /// </summary>
    [System.ComponentModel.DesignerCategory("foo")]
    public abstract class TableBase : DataTable
    {
        #region Private
        private SQLiteDataAdapter adapter;
        // SQLite automatically has a _rowid_ column for each table which uniquely identifies the row
        private const string RowId = "_rowid_";
        private const string RowIdAlias = "SYSROWID";
        //private const string ColumnActionPropertyKey = "{6554ed42-bd9a-403b-856e-2545ae03d2d5}";
        #endregion

        /************************************************************************/
        
        #region Public fields and properties
        /// <summary>
        /// Gets a value that indicates whether or not this table is read only. When True, no update operations are allowed
        /// </summary>
        public bool IsReadOnly
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a value that indicates if delete operations are allowed on this table.
        /// </summary>
        public bool IsDeleteRestricted
        {
            get;
            protected set;
        }

        /// <summary>
        /// When implemented in a derived class, gets the column name of the primary key, 
        /// or null if there is no single column primary key
        /// </summary>
        public abstract string PrimaryKeyName
        {
            get;
        }
        #endregion

        /************************************************************************/
        
        #region Protected properties
        /// <summary>
        /// Gets the database controller assigned to this table
        /// </summary>
        protected DatabaseControllerBase Controller
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableBase"/> class.
        /// </summary>
        /// <param name="controller">The database controller object.</param>
        /// <param name="tableName">The name of the table.</param>
        protected TableBase(DatabaseControllerBase controller, string tableName)
        {
            Validations.ValidateNull(controller, "TableBase.Controller");
            Validations.ValidateNullEmpty(tableName, "TableBase.TableName");
            Controller = controller;
            TableName = tableName;
            IsReadOnly = false;
            IsDeleteRestricted = false;
            adapter = new SQLiteDataAdapter();
            adapter.SelectCommand = new SQLiteCommand(controller.Connection);
            adapter.InsertCommand = new SQLiteCommand(controller.Connection);
            adapter.UpdateCommand = new SQLiteCommand(controller.Connection);
            adapter.DeleteCommand = new SQLiteCommand(controller.Connection);
        }
        #endregion

        /************************************************************************/
        
        #region Public methods
        /// <summary>
        /// Gets a boolean value that indicates if this table exists within the database.
        /// </summary>
        /// <returns>true if the table exists within the database; otherwise, false.</returns>
        public bool Exists()
        {
            string sql = String.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", TableName);
            var obj = Controller.Execution.Scalar(sql);
            return (obj != null);
        }

        /// <summary>
        /// Gets a boolean value that indicates if this table has any rows within the database.
        /// </summary>
        /// <returns>true if the tabke has any rows within the database; otherwise, false.</returns>
        public bool HasRows()
        {
            string sql = String.Format("SELECT COUNT(*) AS C FROM {0}", TableName);
            Int64 count = (Int64)Controller.Execution.Scalar(sql);
            return count > 0;
        }
        
        /// <summary>
        /// When implemented in a derived class, loads the data.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Saves the data that has changed.
        /// </summary>
        public void Save()
        {
            if (IsReadOnly) return;

            DirtyRows dirty = new DirtyRows(this);
            if (!dirty.IsDirty) return;
            Insert(dirty);
            Update(dirty);
            Delete(dirty);
            AcceptChanges();
        }

        /// <summary>
        /// Adds a new row to the table with default values, and calls the Save() method.
        /// </summary>
        public void AddDefaultRow()
        {
            Validations.ValidateInvalidOperation(IsReadOnly, Strings.InvalidOperation_TableIsReadOnly);
            DataRow row = NewRow();
            PopulateDefaultRow(row);
            Rows.Add(row);
            Save();
        }

        /// <summary>
        /// Gets a value that indicates if this table has unsaved changes.
        /// </summary>
        /// <returns>true if any row needs inserting, updating, or deleting; otherwise, false.</returns>
        public bool IsDirty()
        {
            if (!IsReadOnly)
            {
                DirtyRows dirty = new DirtyRows(this);
                return dirty.IsDirty;
            }
            return false;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// When implemented in a derived class, gets the DDL needed to create this table.
        /// </summary>
        /// <returns>A string that has the DDL needed to create the table</returns>
        protected abstract string GetDdl();

        /// <summary>
        /// Override in a derived class to get the SQL needed to populate this table
        /// with default data, such as lookup data, configuration values, etc.
        /// This method is only called when the table has no rows.
        /// </summary>
        /// <returns>A sql string needed to populate the table with any required one-time, default data</returns>
        protected virtual string GetPopulateSql()
        {
            return null;
        }

        /// <summary>
        /// Loads the data
        /// </summary>
        /// <param name="where">A SQL where clause (without the WHERE keyword), or null for all records.</param>
        /// <param name="orderBy">A SQL order clause (without the ORDER BY keyword), or null for no specific order.</param>
        /// <param name="fields">A list if database field names to load</param>
        protected void Load(string where, string orderBy, params string[] fields)
        {
            StringBuilder sql = new StringBuilder(String.Format("SELECT {0} AS {1},", RowId, RowIdAlias), 512);
            //StringBuilder sql = new StringBuilder("SELECT ", 512);

            if (fields.Length == 0) sql.Append("*,");
            foreach (string field in fields)
            {
                sql.Append(String.Format("{0},", field));
            }
            // get rid of the last comma
            sql.Remove(sql.Length - 1, 1);
            sql.Append(String.Format(" FROM {0}", TableName));
            if (!String.IsNullOrEmpty(where))
            {
                sql.Append(String.Format(" WHERE {0}", where));
            }
            if (!String.IsNullOrEmpty(orderBy))
            {
                sql.Append(String.Format(" ORDER BY {0}", orderBy));
            }

            Columns.Clear();
            Clear();
            adapter.SelectCommand.CommandText = sql.ToString();
            adapter.Fill(this);
            SetColumnProperties();
        }

        /// <summary>
        /// Gets the DDL used to create this table by querying the master metadata table
        /// </summary>
        /// <returns>The DDL used to create this table.</returns>
        protected string GetDdlByQuery()
        {
            string sql = String.Format("SELECT sql FROM sqlite_master WHERE name='{0}'", TableName);
            return Controller.Execution.Scalar(sql).ToString();
        }

        /// <summary>
        /// Creates a relation to a child table
        /// </summary>
        /// <typeparam name="T">The child type</typeparam>
        /// <param name="name">The name of the relation</param>
        /// <param name="parentColumnName">The parent column name, i.e. name of column in this table</param>
        /// <param name="childColumnName">The child column name, i.e. name of column in child table T</param>
        protected void CreateParentChildRelation<T>(string name, string parentColumnName, string childColumnName) where T: TableBase
        {
            Validations.ValidateNullEmpty(name, "CreateParentChildRelation.Name");
            Validations.ValidateNullEmpty(parentColumnName, "CreateParentChildRelation.ParentColumnName");
            Validations.ValidateNullEmpty(childColumnName, "CreateParentChildRelation.ChildColumnName");
            var child = Controller.GetTable<T>();
            DataRelation r = new DataRelation(name, Columns[parentColumnName], child.Columns[childColumnName]);
            ChildRelations.Add(r);
        }

        /// <summary>
        /// Creates an expression column to get data via a previously defined relation.
        /// </summary>
        /// <typeparam name="T">The type of the data column</typeparam>
        /// <param name="colName">The name of the new column to add to this table, the child table</param>
        /// <param name="relationName">The relation name, created by parent</param>
        /// <param name="parentColName">The name of the parent column that will populate this column</param>
        protected void CreateChildToParentColumn<T>(string colName, string relationName, string parentColName)
        {
            Validations.ValidateNullEmpty(colName, "CreateChildToParentColumn.ColName");
            Validations.ValidateNullEmpty(relationName, "CreateChildToParentColumn.RelationColName");
            Validations.ValidateNullEmpty(parentColName, "CreateChildToParentColumn.ParentColName");

            // This is related to DatabaseControllerBase.TableRegistrationComplete() in that a table
            // that failed to create its columns will get a chance to try again. However, we only 
            // want the table to create a column if it was unsuccessful the first time. It may have
            // successfully created columns A and B, but failed on column C
           
            if (!Columns.Contains(colName))
            {
                DataColumn col = new DataColumn(colName);
                col.DataType = typeof(T);
                col.Expression = String.Format("Parent({0}).{1}", relationName, parentColName);
                Columns.Add(col);
            }
        }

        /// <summary>
        /// Creates an expression column to get data via a previously defined relation. Uses default type of string.
        /// </summary>
        /// <param name="colName">The name of the new column to add to this table, the child table</param>
        /// <param name="relationName">The relation name, created by parent</param>
        /// <param name="parentColName">The name of the parent column that will populate this column</param>
        protected void CreateChildToParentColumn(string colName, string relationName, string parentColName)
        {
            CreateChildToParentColumn<string>(colName, relationName, parentColName);
        }


        /// <summary>
        /// Creates an expression column using a caller defined expression.
        /// </summary>
        /// <typeparam name="T">The type of the data column</typeparam>
        /// <param name="colName">The name of the new column to add to this table</param>
        /// <param name="expression">The expression as created by the caller</param>
        protected void CreateExpressionColumn<T>(string colName, string expression)
        {
            Validations.ValidateNullEmpty(colName, "CreateExpressionColumn.ColName");
            Validations.ValidateNullEmpty(expression, "CreateExpressionColumn.Expression");

            // This is related to DatabaseControllerBase.TableRegistrationComplete() in that a table
            // that failed to create its columns will get a chance to try again. However, we only 
            // want the table to create a column if it was unsuccessful the first time. It may have
            // successfully created columns A and B, but failed on column C
            
            if (!Columns.Contains(colName))
            {
                DataColumn col = new DataColumn(colName);
                col.DataType = typeof(T);
                col.Expression = expression;
                Columns.Add(col);
            }
        }

        /// <summary>
        /// Creates a column that gets its value from a callback method.
        /// </summary>
        /// <typeparam name="T">The type for the created column.</typeparam>
        /// <param name="colName">The name of the column.</param>
        /// <param name="dependentTable">The dependent table</param>
        /// <param name="updateAction">The action to perform when requesting a value for this column</param>
        /// <param name="dependentColumns">The names of the columns in the dependent table that when changed trigger the update.</param>
        protected void CreateActionExpressionColumn<T>(string colName, TableBase dependentTable, Action<ActionDataColumn, DataRowChangeEventArgs> updateAction, params string[] dependentColumns)
        {
            Validations.ValidateNullEmpty(colName, "CreateExpressionColumn.ColName");
            Validations.ValidateNull(updateAction, "CreateExpressionColumn.Action");

            if (!Columns.Contains(colName))
            {
                var col = new ActionDataColumn(colName, typeof(T), updateAction);
                col.SetDependentTable(dependentTable, dependentColumns);
                Columns.Add(col);
            }
        }

        /// <summary>
        /// Sets custom properties on a column.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="keys">Value from the <see cref="DataColumnPropertyKey"/> enumeration.</param>
        /// <remarks>
        /// This method enables a derived class to specify how a column should be treated
        /// during insert and update operations. You can specify that a column is not used
        /// during insert/update operations and/or that a column should receive a copy of 
        /// the freshly inserted row id during an insert operation. The latter should be used
        /// on columns that have been declared INTEGER PRIMARY KEY, as this is an alias 
        /// for the _rowid_ column that SQLite creates.
        /// </remarks>
        protected void SetColumnProperty(DataColumn col, params DataColumnPropertyKey[] keys)
        {
            Validations.ValidateNull(col, "SetColumnProperty.Col");
            foreach (DataColumnPropertyKey key in keys)
            {
                if (!col.ExtendedProperties.ContainsKey(key))
                {
                    col.ExtendedProperties.Add(key, 1);
                }
            }
        }

        /// <summary>
        /// Adds a column to the table within the database if it doesn't exist.
        /// </summary>
        /// <param name="colName">The column name</param>
        /// <param name="colDefinition">The column defintion, ex: TEXT NOT NULL</param>
        /// <remarks>
        /// This method is used to alter the database table by adding another column.
        /// It may be used for example during a version update when another column should be added.
        /// </remarks>
        protected void AddColumn(string colName, string colDefinition)
        {
            Validations.ValidateNullEmpty(colName, "ColName");
            Validations.ValidateNullEmpty(colDefinition, "ColDefinition");
            if (ColumnExists(colName))
            {
                return;
            }
            string sql = String.Format("ALTER TABLE {0} ADD COLUMN `{1}` {2}", TableName, colName, colDefinition);
            Controller.Execution.NonQuery(sql);
        }

        /// <summary>
        /// Enables a derived class to establish column settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Column defintions on the table are not available until the data adapter has had an opportunity to fill the table.
        /// During the adapter's fill operation, it creates the column definitions.
        /// </para>
        /// <para>
        /// This method is called after the table is filled and enables a derived class to call the <see cref="SetColumnProperty"/> method
        /// to set extended properties on the column.
        /// </para>
        /// </remarks>
        protected abstract void SetColumnProperties();

        /// <summary>
        /// Enables a derived class to populate a new row with default (starter) values.
        /// </summary>
        /// <param name="row">The freshly created DataRow to poulate</param>
        /// <remarks>
        /// Override this method in a derived class to provide default (starter) values for the row.
        /// If the child class does not support this operation, do not override. The base implementation
        /// throws a NotImplementedException.
        /// </remarks>
        protected virtual void PopulateDefaultRow(DataRow row)
        {
            throw new NotImplementedException(Strings.NotImplemented);
        }

        /// <summary>
        /// Override in a derived class to perform post registration operations such as setting table relations. The controller
        /// calls this method after all tables have been registered.
        /// </summary>
        protected internal virtual void SetDataRelations()
        {
        }

        /// <summary>
        /// Override in a derived class to create calculated columns based on established relations. The controller
        /// calls this method after all tables have been registered and all tables have had an opportunity
        /// to set data relations via the <see cref="SetDataRelations"/> method.
        /// </summary>
        protected internal virtual void UseDataRelations()
        {
        }

        /// <summary>
        /// Override in a derived class to perform special operations after load and relation operations.
        /// The controller calls this method after all tables have had an opportunity to set data relations
        /// via the  <see cref="SetDataRelations"/> method, and use data relations via the <see cref="UseDataRelations"/> method.
        /// </summary>
        protected internal virtual void OnInitializationComplete()
        {
        }

        /// <summary>
        /// Override in a derived class to perform any cleanup operations prior to shutting down the connection.
        /// The controller calls this method for each table from its Shutdown() method.
        /// </summary>
        /// <param name="saveOnShutdown">The value that was passed to the controller's Shutdown method. 
        /// If false, the table needs to call its Save() method if it makes changes to data.
        /// </param>
        protected internal virtual void OnShuttingDown(bool saveOnShutdown)
        {
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        /// <summary>
        /// Creates the table in the database from the DDL text.
        /// </summary>
        internal void CreateFromDdl()
        {
            string sql = GetDdl();
            Validations.ValidateInvalidOperation(String.IsNullOrWhiteSpace(sql), Strings.InvalidOperation_EmptyDdl);
            Controller.Execution.NonQuery(sql);
        }

        /// <summary>
        /// Populate the table with default data
        /// </summary>
        /// <remarks>
        /// This method is called by DatabaseControllerBase at startup if the table has no rows and its behavior flags include
        /// DatabaseControllerBehavior.AutoPopulate
        /// </remarks>
        internal void Populate()
        {
            string sql = GetPopulateSql();
            if (!String.IsNullOrEmpty(sql))
            {
                Controller.Execution.NonQuery(sql);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void Insert(DirtyRows dirty)
        {
            if (!dirty.IsInsertDirty) return;
            StringBuilder colList = new StringBuilder(512);
            StringBuilder sql = new StringBuilder(512);

            foreach (DataColumn col in Columns)
            {
                if (IsColumnElibible(col, DataColumnPropertyKey.ExcludeFromInsert))
                {
                    colList.Append(String.Format("{0},", col.ColumnName));
                }
            }
            // get rid of the last comma in the column list
            colList.Remove(colList.Length - 1, 1);       
            
            foreach (DataRow row in dirty.Insert)
            {
                adapter.InsertCommand.Parameters.Clear();
                sql.Clear();
                sql.Append(String.Format("INSERT INTO {0} ({1}) VALUES(", TableName, colList));

                foreach (DataColumn col in Columns)
                {
                    if (IsColumnElibible(col, DataColumnPropertyKey.ExcludeFromInsert))
                    {
                        sql.Append(String.Format(":{0},", col.ColumnName));
                        adapter.InsertCommand.Parameters.Add(col.ColumnName, TypeToDbType(col.DataType)).Value = row[col];
                    }
                }

                if (PrimaryKey.Length == 0)
                {
                }
                // get rid of the last comma in the values list
                sql.Remove(sql.Length - 1, 1);
                sql.Append(")");
                adapter.InsertCommand.CommandText = sql.ToString();
                adapter.InsertCommand.ExecuteNonQuery();

                InsertLastInsertId(row);

            }
        }

        private void Update(DirtyRows dirty)
        {
            if (!dirty.IsUpdateDirty) return;
            StringBuilder sql = new StringBuilder(512);
            // adapter.UpdateCommand.Parameters.Clear();

            foreach (DataRow row in dirty.Update)
            {
                adapter.UpdateCommand.Parameters.Clear();
                sql.Clear();
                sql.Append(String.Format("UPDATE {0} SET ", TableName));
                foreach (DataColumn col in Columns)
                {
                    if (IsColumnElibible(col, DataColumnPropertyKey.ExcludeFromUpdate))
                    {
                        sql.Append(String.Format("{0}=:{0},", col.ColumnName));
                        adapter.UpdateCommand.Parameters.Add(col.ColumnName, TypeToDbType(col.DataType)).Value = row[col];
                    }
                }
                // get rid of the last comma
                sql.Remove(sql.Length - 1, 1);
                sql.Append(String.Format(" WHERE {0}={1}", RowId, row[RowIdAlias]));
                adapter.UpdateCommand.CommandText = sql.ToString();
                adapter.UpdateCommand.ExecuteNonQuery();
            }
        }

        private void Delete(DirtyRows dirty)
        {
            if (IsDeleteRestricted || !dirty.IsDeleteDirty) return;
            StringBuilder sql = new StringBuilder(512);
            foreach (DataRow row in dirty.Delete)
            {
                adapter.DeleteCommand.Parameters.Clear();
                sql.Clear();
                sql.Append(String.Format("DELETE FROM {0} ", TableName));
                sql.Append(String.Format("WHERE {0}={1}", RowId, row[RowIdAlias, DataRowVersion.Original]));
                adapter.DeleteCommand.CommandText = sql.ToString();
                adapter.DeleteCommand.ExecuteNonQuery();
            }
        }


        private DbType TypeToDbType(Type type)
        {
            if (type == typeof(String)) return DbType.String;
            if (type == typeof(Int32)) return DbType.Int32;
            if (type == typeof(Int64)) return DbType.Int64;
            if (type == typeof(DateTime)) return DbType.DateTime;
            if (type == typeof(Boolean)) return DbType.Boolean;
            return DbType.String;
        }

        private bool IsColumnElibible(DataColumn col, DataColumnPropertyKey exclusionKey)
        {
            return
            (
                !col.ReadOnly &&
                String.IsNullOrEmpty(col.Expression) &&
                col.ColumnName != RowIdAlias &&
                !col.ExtendedProperties.ContainsKey(exclusionKey)
            );
        }

        private void InsertLastInsertId(DataRow row)
        {
            adapter.InsertCommand.CommandText = "SELECT last_insert_rowid()";
            object id = adapter.InsertCommand.ExecuteScalar();
            row[RowIdAlias] = id; 
            foreach (DataColumn col in Columns)
            {
                if (col.ExtendedProperties.ContainsKey(DataColumnPropertyKey.ReceiveInsertedId))
                {
                    row[col] = id;
                }
            }
        }

        /// <summary>
        /// Gets a boolean value that indicates if the specified column exists in this table.
        /// </summary>
        /// <param name="colName">The column name</param>
        /// <returns>true if exists; otherwise, false.</returns>
        private bool ColumnExists(string colName)
        {
            string sql = String.Format("PRAGMA table_info({0})", TableName);
            using (var reader = Controller.Execution.Query(sql))
            {
                int nameIndex = reader.GetOrdinal("name");
                while (reader.Read())
                {
                    if (reader.GetString(nameIndex).Equals(colName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
