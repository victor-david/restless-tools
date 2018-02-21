using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents a table that does not exist in the database on disk.
    /// By default, the <see cref="TableBase.IsReadOnly"/> property is set to <b>true</b>.
    /// </summary>
    public abstract class MemoryTable : TableBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTable"/> class.
        /// </summary>
        /// <param name="controller">The database controller object.</param>
        /// <param name="schemaName">The name of the schema this table belongs to.</param>
        /// <param name="tableName">The name of the table.</param>
        protected MemoryTable (DatabaseControllerBase controller, string schemaName, string tableName)
            : base(controller, schemaName, tableName)
        {
            IsReadOnly = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTable"/> class.
        /// </summary>
        /// <param name="controller">The database controller object.</param>
        /// <param name="tableName">The name of the table.</param>
        protected MemoryTable(DatabaseControllerBase controller, string tableName) : this(controller, DatabaseControllerBase.MainSchemaName,tableName)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Satisfies the implementation of <see cref="TableBase"/> but does nothing.
        /// </summary>
        public override void Load()
        {
        }

        /// <summary>
        /// Satisfies the implementation of <see cref="TableBase"/> but returns a SQL statement that does not affect the database.
        /// </summary>
        /// <returns>A SQL statement that does not affect the database.</returns>
        protected override string GetDdl()
        {
            return "SELECT abs(0)";
        }

        /// <summary>
        /// Satisfies the implementation of <see cref="TableBase"/> but does nothing.
        /// </summary>
        protected override void SetColumnProperties()
        {
        }
        #endregion
    }
}
