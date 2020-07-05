using System;
using System.Collections.Generic;

namespace Restless.Tools.Mvvm
{
    /// <summary>
    /// Represents a dictionary of commands.
    /// </summary>
    /// <remarks>
    /// A CommandDictionary collection is used by the various view models and associated controllers
    /// to create commands without the need to declare a separate property for each one.
    /// </remarks>
    public class CommandDictionary
    {
        #region Private
        private Dictionary<string, RelayCommand> storage;
        #endregion

        /************************************************************************/
        
        #region Public properties
        /// <summary>
        /// Acceses the dictionary value according to the string key
        /// </summary>
        /// <param name="key">The string key</param>
        /// <returns>The RelayCommand object, or null if not present</returns>
        public RelayCommand this [string key]
        {
            get 
            {
                if (storage.ContainsKey(key))
                {
                    return storage[key];
                }
                return null;
            }
        }

        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDictionary"/> class.
        /// </summary>
        public CommandDictionary()
        {
            storage = new Dictionary<string, RelayCommand>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a command to the dictionary.
        /// </summary>
        /// <param name="key">The name of the command in the dictionary</param>
        /// <param name="command">The RelayCommand object</param>
        public void Add(string key, RelayCommand command)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format("The command with key {0} already exists.", key));
            }

            storage.Add(key, command);
        }

        /// <summary>
        /// Adds a command to the dictionary.
        /// </summary>
        /// <param name="key">The name of the command in the dictionary</param>
        /// <param name="runCommand">The action to run the command</param>
        /// <param name="canRunCommand">The predicate to determine if the command can run, or null if it can always run</param>
        /// <param name="supported">A value that determines if the command is supported.</param>
        /// <param name="parameter">An optional parameter that if set will always be passed to the command method.</param>
        public void Add(string key, Action<object> runCommand, Predicate<object> canRunCommand, CommandSupported supported, object parameter)
        {
            Add(key, RelayCommand.Create(runCommand, canRunCommand, supported, parameter));
        }

        /// <summary>
        /// Adds a command to the dictionary.
        /// </summary>
        /// <param name="key">The name of the command in the dictionary</param>
        /// <param name="runCommand">The action to run the command</param>
        /// <param name="canRunCommand">The predicate to determine if the command can run, or null if it can always run</param>
        /// <param name="parameter">An optional parameter that if set will always be passed to the command method.</param>
        public void Add(string key, Action<object> runCommand, Predicate<object> canRunCommand, object parameter = null)
        {
            Add(key, RelayCommand.Create(runCommand, canRunCommand, CommandSupported.Yes, parameter));
        }

        /// <summary>
        /// Adds a command without a predicate.
        /// </summary>
        /// <param name="key">The name of the command in the dictionary</param>
        /// <param name="runCommand">The action to run the command</param>
        public void Add(string key, Action<object> runCommand)
        {
            Add(key, RelayCommand.Create(runCommand));
        }

        /// <summary>
        /// Gets a boolean value that indicates if the collection contains the sepcified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>true if the key already exists; otherwise, false.</returns>
        public bool ContainsKey(string key)
        {
            return storage.ContainsKey(key);
        }

        /// <summary>
        /// Clears all items from the command dictionary.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
        }
        #endregion
    }
}