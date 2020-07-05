using System;
using System.Windows.Controls;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Represents the pair of TabItem objects involved in a drag drop operation.
    /// </summary>
    public class TabItemDragDrop
    {
        /// <summary>
        /// Gets the source tab item.
        /// </summary>
        public TabItem Source
        {
            get;
        }

        /// <summary>
        /// Gets the target tab item.
        /// </summary>
        public TabItem Target
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItemDragDrop"/> class.
        /// </summary>
        /// <param name="source">The source tab item object.</param>
        /// <param name="target">The target tab item object.</param>
        public TabItemDragDrop(TabItem source, TabItem target)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }
    }
}
