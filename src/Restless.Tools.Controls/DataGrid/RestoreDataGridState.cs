using System;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides an enumeration that may be applied to the <see cref="RestlessDataGrid.RestoreStateBehavior"/> property.
    /// </summary>
    [Flags]
    public enum RestoreDataGridState
    {
        /// <summary>
        /// None. No restore behavior is defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// Select the first item in the data grid and scroll it into view.
        /// This option only applies when there is no item already selected.
        /// </summary>
        SelectFirst = 1,
        /// <summary>
        /// Select the last item in the data grid and scroll it into view.
        /// This option only applies when there is no item already selected.
        /// </summary>
        SelectLast = 2,
        /// <summary>
        /// Restore the last selected item.
        /// This option only applies when there is an item selected at the time of restore.
        /// When applicable, this option option also scrolls the last selected item into view.
        /// </summary>
        RestoreLastSelection = 4,
        /// <summary>
        /// Combination of <see cref="SelectFirst"/> and <see cref="RestoreLastSelection"/>.
        /// </summary>
        SelectFirstAndRestore = 5,
        /// <summary>
        /// Combination of <see cref="SelectLast"/> and <see cref="RestoreLastSelection"/>.
        /// </summary>
        SelectLastAndRestore = 6,
    }
}
