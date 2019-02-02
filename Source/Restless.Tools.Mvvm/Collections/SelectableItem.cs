using System;
using System.Windows.Input;

namespace Restless.Tools.Mvvm.Collections
{
    /// <summary>
    /// Represents the base class a general purpose helper class that can be used when binding.
    /// </summary>
    public abstract class SelectableItem : ObservableObject
    {
        #region Private
        private string name;
        private bool isSelected;
        private int index;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates if this item is selected.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (SetProperty(ref isSelected, value))
                {
                    OnIsSelectedChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a command associated with this item.
        /// </summary>
        public ICommand Command
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a command option associated with this item.
        /// </summary>
        public object CommandParm
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the item index.
        /// This is used by <see cref="SelectableItemList{T}"/> when adding an item via
        /// the <see cref="SelectableItemList{T}.AddObservable(T)"/> method.
        /// </summary>
        public int Index
        {
            get => index;
            internal set => SetProperty(ref index, value);
        }
        #endregion

        /************************************************************************/

        #region Public events
        /// <summary>
        /// Occurs when the <see cref="IsSelected"/> property changes.
        /// </summary>
        public event EventHandler IsSelectedChanged;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableItem"/> class.
        /// </summary>
        protected SelectableItem()
        {
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Raises the <see cref="IsSelectedChanged"/> event.
        /// </summary>
        protected virtual void OnIsSelectedChanged()
        {
            IsSelectedChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }

}
