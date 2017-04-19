using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Restless.Tools.Utility;

namespace Restless.Tools.Database.Generic
{
    /// <summary>
    /// Contains the count of rows that have changed since the last call to Update or AcceptChanges.
    /// </summary>
    public class DirtyRows
    {
        /// <summary>
        /// An array of DataRow objects to be inserted
        /// </summary>
        public DataRow[] Insert
        {
            get;
            private set;
        }

        /// <summary>
        /// An array of DataRow objects that have been updated.
        /// </summary>
        public DataRow[] Update
        {
            get;
            private set;
        }
        
        /// <summary>
        /// An array of DataRow objects to be deleted.
        /// </summary>
        public DataRow[] Delete
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value that indicates if there are records that should be inserted.
        /// </summary>
        public bool IsInsertDirty
        {
            get { return Insert.Length > 0; }
        }

        /// <summary>
        /// Gets a value that indicates iof there are records that should be updated.
        /// </summary>
        public bool IsUpdateDirty
        {
            get { return Update.Length > 0; }
        }

        /// <summary>
        /// Gets a value that indicates if there are records that should be deleted
        /// </summary>
        public bool IsDeleteDirty
        {
            get { return Delete.Length > 0; }
        }

        /// <summary>
        /// Gets a value that indicates if any row needs updating for any reason (insert, update, or delete); otherwise, false.
        /// </summary>
        public bool IsDirty
        {
            get { return IsInsertDirty || IsUpdateDirty || IsDeleteDirty; }
        }

        /// <summary>
        /// Creates a new instance of this object
        /// </summary>
        /// <param name="table">The data table object</param>
        public DirtyRows(DataTable table)
        {
            Validations.ValidateNull(table, "DirtyRows.Table");
            Insert =  table.Select("1=1", table.Columns[0].ColumnName, DataViewRowState.Added);
            Update = table.Select("1=1", table.Columns[0].ColumnName, DataViewRowState.ModifiedOriginal);
            Delete = table.Select("1=1", table.Columns[0].ColumnName, DataViewRowState.Deleted);
        }
    }
}
