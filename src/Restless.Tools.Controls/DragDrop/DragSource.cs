using System;
using System.Windows;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Drag source implementation with strongly typed payload
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DragSource<T> : IDragSource
    {
        private readonly Func<T, DragDropEffects> getSupportedEffects;
        private readonly Func<T, object> getData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragSource{T}"/> class.
        /// </summary>
        /// <param name="getSupportedEffects">The get supported effects.</param>
        /// <param name="getData">The get data.</param>
        public DragSource(Func<T, DragDropEffects> getSupportedEffects, Func<T, object> getData)
        {
            this.getSupportedEffects = getSupportedEffects ?? throw new ArgumentNullException(nameof(getSupportedEffects));
            this.getData = getData ?? throw new ArgumentNullException(nameof(getData));
        }

        /// <summary>
        /// Gets the supported drop effects.
        /// </summary>
        /// <param name="dataContext">The data context of the element initiating the drag operation.</param>
        /// <returns>The effects.</returns>
        public DragDropEffects GetDragEffects(object dataContext)
        {
            return getSupportedEffects((T) dataContext);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="dataContext">The data context of the element initiating the drag operation.</param>
        /// <returns>The data.</returns>
        public object GetData(object dataContext)
        {
            return getData((T) dataContext);
        }
    }
}
