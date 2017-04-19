using System.Windows;

namespace Restless.Tools.Controls.DragDrop
{
    /// <summary>
    /// Business end of the drag source
    /// </summary>
    public interface IDragSource
    {
        /// <summary>
        /// Gets the supported drop effects.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <returns>The effects.</returns>
        DragDropEffects GetDragEffects(object dataContext);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <returns>The data.</returns>
        object GetData(object dataContext);
    }
}