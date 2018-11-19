using System;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents the base class for table statistics. This class provides basic statistics;
    /// a dervired class can provide more specialized stats.
    /// </summary>
    public class TableStatisticBase
    {
        #region Public properties
        /// <summary>
        /// Gets the table that the statistics belong to.
        /// </summary>
        public TableBase Table
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        public int RowCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the column count.
        /// </summary>
        public int ColumnCount
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TableStatisticBase"/> class.
        /// This constructor calls the <see cref="Refresh"/> method.
        /// </summary>
        /// <param name="table">The table.</param>
        public TableStatisticBase(TableBase table)
        {
            Table = table ?? throw new ArgumentNullException();
            RowCount = table.Rows.Count;
            ColumnCount = table.Columns.Count;
            Refresh();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Refreshes the table statistics. Override in a derived class to populate / refresh specialized statisics
        /// </summary>
        protected virtual void Refresh()
        {
            RowCount = Table.Rows.Count;
            ColumnCount = Table.Columns.Count;
        }
        #endregion

    }
}
