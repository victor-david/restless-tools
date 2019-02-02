using System;
using System.Windows.Input;

namespace Restless.Tools.Controls
{
    /// <summary>
    /// Implements ICommand to provide a command that always is executable.
    /// </summary>
    internal class ExecuteCommand : ICommand
    {
        #region Private 
        private readonly Action<object> execute;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteCommand"/> class.
        /// </summary>
        /// <param name="execute">The method that executes the command.</param>
        public ExecuteCommand(Action<object> execute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
        }
        #endregion

        /************************************************************************/

        #region Public members
        /// <summary>
        /// Occurs when the conditions that affect whether a command may excute change.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Satisfies the ICommand interface. Always returns trie.
        /// </summary>
        /// <param name="parameter">The parm</param>
        /// <returns>true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">The command parm.</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
        #endregion
    }







}
