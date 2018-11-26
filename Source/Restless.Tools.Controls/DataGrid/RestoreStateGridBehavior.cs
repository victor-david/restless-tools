using System;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Provides an enumeration that may be applied to the <see cref="RestlessDataGrid.RestoreStateBehavior"/> property.
    /// </summary>
    [Flags]
    public enum RestoreGridStateBehavior
    {
        /// <summary>
        /// None. No restore behavior is defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// Select the first item in the data grid.
        /// </summary>
        SelectStart = 1,
        /// <summary>
        /// Select the last item in the data grid.
        /// </summary>
        SelectEnd = 2,
        /// <summary>
        /// Restore the last selected item.
        /// </summary>
        RestoreLastSelection = 4,
        /// <summary>
        /// Scroll into view
        /// </summary>
        ScrollIntoView = 8,
        /// <summary>
        /// Combination of <see cref="SelectStart"/> and <see cref="RestoreLastSelection"/>.
        /// </summary>
        SelectStartAndRestore = 5,
        /// <summary>
        /// Combination of <see cref="SelectEnd"/> and <see cref="RestoreLastSelection"/>.
        /// </summary>
        SelectEndAndRestore = 6,
        /// <summary>
        /// Combination of <see cref="SelectStart"/> and <see cref="RestoreLastSelection"/> and <see cref="ScrollIntoView"/>.
        /// </summary>
        SelectStartAndRestoreAndScroll = 13,
        /// <summary>
        /// Combination of <see cref="SelectEnd"/> and <see cref="RestoreLastSelection"/> and <see cref="ScrollIntoView"/>.
        /// </summary>
        SelectEndAndRestoreAndScroll = 14,
    }
}
