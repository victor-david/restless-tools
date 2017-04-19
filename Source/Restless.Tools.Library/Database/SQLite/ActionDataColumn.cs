using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Restless.Tools.Resources;
using Restless.Tools.Utility;

namespace Restless.Tools.Database.SQLite
{
    /// <summary>
    /// Represents a data column that gets updated via a callback action.
    /// This is similar to <see cref="DataColumn"/> with an expression, but enables
    /// more complex update scenarios.
    /// </summary>
    [System.ComponentModel.DesignerCategory("foo")]
    public class ActionDataColumn : DataColumn
    {
        #region Private
        private bool fireOnRowChanged;
        private DataTable dependentTable;
        private List<string> dependentColumns;
        private Action<ActionDataColumn, DataRowChangeEventArgs> updateAction;
        #endregion

        /************************************************************************/

        #region Public properties
        #endregion

        /************************************************************************/

        #region Constructor (internal)
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDataColumn"/> class.
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="dataType">The column's data type.</param>
        /// <param name="updateAction">The action to called when an update is needed.</param>
        internal ActionDataColumn(string columnName, Type dataType, Action<ActionDataColumn, DataRowChangeEventArgs> updateAction)
            : base(columnName, dataType)
        {
            Validations.ValidateNull(updateAction, "ActionDataColumn.UpdateAction");
            this.updateAction = updateAction;
            ExtendedProperties.Add(DataColumnPropertyKey.ExcludeFromInsert, 1);
            ExtendedProperties.Add(DataColumnPropertyKey.ExcludeFromUpdate, 1);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal void SetDependentTable(DataTable dependentTable, params string[] dependentColumns)
        {
            if (this.dependentTable == null)
            {
                Validations.ValidateNull(dependentTable, "AddDependentTable.Table");
                foreach (string colName in dependentColumns)
                {
                    Validations.ValidateInvalidOperation(!dependentTable.Columns.Contains(colName), Strings.InvalidOperation_ColumnDoesNotBelongToTable);
                }

                this.dependentTable = dependentTable;
                this.dependentColumns = dependentColumns.ToList<string>();

                this.dependentTable.ColumnChanged += DependentTableColumnChanged;
                this.dependentTable.RowChanged += DependentTableRowChanged;
            }

        }
        #endregion

        /************************************************************************/

        #region Private methods
        /// <summary>
        /// Occurs when the column changes. This occurs before RowChanged.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void DependentTableColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            fireOnRowChanged = fireOnRowChanged || dependentColumns.Contains(e.Column.ColumnName);
        }

        /// <summary>
        /// Occurs when the row changes. This occurs after ColumnChanged.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void DependentTableRowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (fireOnRowChanged)
            {
                // to prevent re-entrancy, must set fireOnRowChanged = false before calling the update action
                fireOnRowChanged = false;
                updateAction(this, e);
            }
        }
        #endregion
    }
}
