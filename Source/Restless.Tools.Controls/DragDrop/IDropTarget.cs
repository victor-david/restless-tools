using System.Windows;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Describes methods an object must implement to participate in drag and drop as a drop target.
    /// </summary>
    public interface IDropTarget
    {
        /// <summary>
        /// Gets the effects.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns>The effects</returns>
        DragDropEffects GetDropEffects(IDataObject dataObject);

        /// <summary>
        /// Drops the specified data object
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        void Drop(IDataObject dataObject);
    }
}