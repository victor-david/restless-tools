using System;

namespace Restless.Tools.Mvvm
{
    /// <summary>
    /// Represents the base class for all view models.This class must be inherited.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IDisposable
    {
        #region Private
        private string displayName;
        private bool isActivated;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the view model that owns this view model, or null if none.
        /// </summary>
        public ViewModelBase Owner
        {
            get;
        }

        /// <summary>
        /// Gets the display name for this instance.
        /// </summary>
        public string DisplayName
        {
            get => displayName;
            protected set => SetProperty(ref displayName, value);
        }

        /// <summary>
        /// Gets a boolean value that indicates if this VM is active.
        /// </summary>
        /// <remarks>
        /// The VM can be signaled via this property. When this property is set to true,
        /// the <see cref="OnActivated"/> method is called. When set to false, 
        /// the <see cref="OnDeactivated"/> method is called.
        /// </remarks>
        public bool IsActivated
        {
            get => isActivated;
            private set
            {
                if (SetProperty(ref isActivated, value))
                {
                    if (isActivated)
                        OnActivated();
                    else
                        OnDeactivated();
                }
            }
        }

        /// <summary>
        /// Gets a dictionary of commands. 
        /// </summary>
        public CommandDictionary Commands
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Protected properties
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="owner">The owner of this view model, or null if none.</param>
        protected ViewModelBase(ViewModelBase owner)
        {
            Owner = owner;
            Commands = new CommandDictionary();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Activates the view model.
        /// </summary>
        public void Activate()
        {
            IsActivated = true;
        }

        /// <summary>
        /// Deactivates the view model.
        /// </summary>
        public void Deactivate()
        {
            IsActivated = false;
        }

        /// <summary>
        /// Toggles the view model between activated and deactivated.
        /// </summary>
        public void ToggleActivation()
        {
            if (!IsActivated)
                Activate();
            else
                Deactivate();
        }

        /// <summary>
        /// Toggles the specifed view model between activated and deactivated.
        /// </summary>
        /// <param name="vm">The <see cref="ViewModelBase"/> to toggle activation for.</param>
        public void ToggleActivation(ViewModelBase vm)
        {
            if (vm != null)
            {
                vm.ToggleActivation();
                OnActivationToggled(vm);
            }
        }

        /// <summary>
        /// Causes the view model to be updated.
        /// A derived class can override <see cref="OnUpdate"/> to perform update actions.
        /// </summary>
        public void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// Signal the view model that it is closing.
        /// </summary>
        public void SignalClosing()
        {
            OnClosing();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when this view model becomes active.
        /// A derived class can override this method to perform initialization actions.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>
        /// Called when this view model becomes inactive.
        /// A derived class can override this method to perform cleanup actions.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnDeactivated()
        {
        }

        /// <summary>
        /// Called when an update to this view model is requested.
        /// A derived class can override this method to perform update actions.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// Called after <see cref="ToggleActivation(ViewModelBase)"/> has toggled the activation state of the specified <see cref="ViewModelBase"/>.
        /// A derived class can override this method to perform update actions.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="vm">The <see cref="ViewModelBase"/> that was toggled.</param>
        protected virtual void OnActivationToggled(ViewModelBase vm)
        {
        }

        /// <summary>
        /// Gets the <see cref="Owner"/> property as the specified type
        /// </summary>
        /// <typeparam name="T">The type that derives from <see cref="ViewModelBase"/>.</typeparam>
        /// <returns>The owner as the specified type, or null if owner not set or can't be cast.</returns>
        protected T GetOwner<T>() where T : ViewModelBase
        {
            return Owner as T;
        }

        /// <summary>
        /// Attempts to set the passed item owner to the specified type. Throws an exception if <see cref="Owner"/> is not <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type reference.</typeparam>
        /// <param name="item">The item to set.</param>
        /// <remarks>
        /// This is a convienence method that allows the caller to set a local var to the more specific type
        /// rather than needing to call <see cref="GetOwner{T}"/>. If <see cref="Owner"/> is not type <typeparamref name="T"/>, 
        /// this method throws an exception.
        /// </remarks>
        protected void SetLocalOwner<T>(ref T item) where T : ViewModelBase
        {
            item = GetOwner<T>() ?? throw new InvalidCastException();
        }

        /// <summary>
        /// Called when the view model is closing, that is when <see cref="SignalClosing"/> is called.
        /// Override in a derived class to perform cleanup operations such as removing event handlers, etc.
        /// Always call the base method.
        /// </summary>
        protected virtual void OnClosing()
        {
            Commands.Clear();
        }
        #endregion

        /************************************************************************/

        #region IDisposable Members
        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A derived class can override this method to handle disposal cleanup.
        /// </summary>
        /// <param name="disposing">true if called from the <see cref="Dispose()"/> method.</param>
        protected virtual void Dispose(bool disposing)
        {
        }


#if DEBUG && VBASE
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected.
        /// </summary>
        ~ViewModelBase()
        {
            Dispose(false);
            string msg = string.Format("{0} ({1}) ({2}) Finalized", GetType().Name, DisplayName, GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif
        #endregion

        /************************************************************************/

        #region Private Methods
        #endregion

    }
}