﻿using System;
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
        #pragma warning disable 1591
        public CommandDictionary()
        {
            storage = new Dictionary<string, RelayCommand>();
        }
        #pragma warning restore 1591
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
            if (String.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (ContainsKey(key))
            {
                throw new InvalidOperationException(String.Format("The command with key {0} already exists.", key));
            }

            storage.Add(key, command);
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

            RelayCommand cmd = RelayCommand.Create(runCommand, canRunCommand);
            cmd.Parameter = parameter;
            Add(key, cmd);
        }

        /// <summary>
        /// Adds a command without a predicate.
        /// </summary>
        /// <param name="key">The name of the command in the dictionary</param>
        /// <param name="runCommand">The action to run the command</param>
        public void Add(string key, Action<object> runCommand)
        {
            Add(key, runCommand, null);
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
        #endregion
    }
}
