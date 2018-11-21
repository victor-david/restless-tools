using System;
using System.Windows;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Drop target with a strongly typed payload
    /// </summary>
    /// <typeparam name="T">The payload type.</typeparam>
    public class DropTarget<T> : IDropTarget
    {
        private readonly Func<T, DragDropEffects> getEffects;
        private readonly Action<T> drop;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropTarget{T}"/> class.
        /// </summary>
        /// <param name="getEffects">The method to be used to get allowed drop effects.</param>
        /// <param name="drop">The method invoked when a payload is dropped on the target.</param>
        public DropTarget(Func<T, DragDropEffects> getEffects, Action<T> drop)
        {
            this.getEffects = getEffects ?? throw new ArgumentNullException(nameof(getEffects));
            this.drop = drop ?? throw new ArgumentNullException(nameof(drop));
        }

        /// <summary>
        /// Gets the effects.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns>The effects</returns>
        public DragDropEffects GetDropEffects(IDataObject dataObject)
        {
            if (!dataObject.GetDataPresent(typeof(T)))
            {
                return DragDropEffects.None;
            }
            return getEffects((T) dataObject.GetData(typeof(T)));
        }

        /// <summary>
        /// Drops the specified data object
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public void Drop(IDataObject dataObject)
        {
            drop((T) dataObject.GetData(typeof (T)));
        }
    }
}
