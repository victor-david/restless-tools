using System;
using System.Collections.ObjectModel;

namespace Restless.Tools.Mvvm.Collections
{
    /// <summary>
    /// Represents a list of <see cref="SelectableItem"/> objects.
    /// </summary>
    public class SelectableItemList<T> : ObservableCollection<T> where T : SelectableItem
    {
        #region Private
        private bool isSelectChangedActive;
        #endregion

        /************************************************************************/

        #region Public events
        /// <summary>
        /// Occurs when an item of the collection has changed its <see cref="SelectableItem.IsSelected"/> property.
        /// You must use the <see cref="AddObservable(T)"/> method for each item added to the collection 
        /// to enable this event.
        /// </summary>
        public event EventHandler IsSelectedChanged;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableItemList{T}"/> class.
        /// </summary>
        public SelectableItemList()
        {
            isSelectChangedActive = true;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds an item to the collection and hooks up its <see cref="SelectableItem.IsSelectedChanged"/> event.
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddObservable(T item)
        {
            Add(item);
            if (item != null)
            {
                item.Index = Count - 1;
                item.IsSelectedChanged += ItemIsSelectedChanged;
            }
        }

        /// <summary>
        /// Selects all items in the list.
        /// </summary>
        public void SelectAll()
        {
            foreach (T item in this)
            {
                item.IsSelected = true;
            }
        }

        /// <summary>
        /// Deselects all items in the list.
        /// </summary>
        public void DeselectAll()
        {
            foreach (T item in this)
            {
                item.IsSelected = false;
            }
        }

        /// <summary>
        /// Suspends firing of the <see cref="IsSelectedChanged"/> event.
        /// </summary>
        public void SuspendIsSelectChanged()
        {
            isSelectChangedActive = false;
        }

        /// <summary>
        /// Resumes firing of the <see cref="IsSelectedChanged"/> event.
        /// </summary>
        public void ResumeIsSelectChanged()
        {
            isSelectChangedActive = true;
        }
        #endregion

        /************************************************************************/

        #region Private events
        private void ItemIsSelectedChanged(object sender, EventArgs e)
        {
            if (isSelectChangedActive)
            {
                IsSelectedChanged?.Invoke(sender, e);
            }
        }
        #endregion
    }
}
