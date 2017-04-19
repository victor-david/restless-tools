using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Restless.Tools.Resources;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Represents a top level exception handler.
    /// </summary>
    public sealed class TopLevelExceptionHandler
    {
        #region Private fields
        private static TopLevelExceptionHandler instance;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes the TopLevelExceptionHandler object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// By calling this static method, any exception that is not handled
        /// in another portion of code is handled by this object. If the unhandled
        /// exception occurs in the application thread, a dialog box is displayed
        /// with the exception information and a choice to continue running the application
        /// or to stop it. 
        /// </para>
        /// <para>
        /// If the unhandled exception occurs in the current domain, a dialog box is displayed
        /// with the exception information, and the application is then forced to terminate.
        /// </para>
        /// </remarks>
        public static void Initialize()
        {
            if (instance == null)
            {
                instance = new TopLevelExceptionHandler();
            }
        }

        /// <summary>
        /// Creates a new instance of the TopLevelExceptionHandler class.
        /// </summary>
        private TopLevelExceptionHandler()
        {
            //AppDomain.CurrentDomain.FirstChanceException += CurrentDomainFirstChanceException;
            Application.Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            //TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException; //Example 4
            //System.Windows.Forms.Application.ThreadException += WinFormApplication_ThreadException; //Example 5
        }
        #endregion

        /************************************************************************/

        #region Private methods (event handlers)
        /// <summary>
        /// Handles a first chance exception. If wired, this method is called before the CLR searches for possible exception handlers.
        /// For instance, this method will be called before the catch() of a try / catch.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments.</param>
        private void CurrentDomainFirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            MessageBox.Show(String.Format("1. CurrentDomain_FirstChanceException {0}", e.Exception.Message));
            //ProcessError(e.Exception);   - This could be used here to log ALL errors, even those caught by a Try/Catch block
        }

        /// <summary>
        /// Handles an otherwise unhandled exception.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args.</param>
        private void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(String.Format(Strings.UnhandledExceptionMessageFormat, e.Exception.Message), Strings.UnhandledExceptionCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args.</param>
        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            string msg = "An unknown error occurred.";
            if (ex != null)
            {
                msg = ex.Message;
            }

            MessageBox.Show(msg, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (e.IsTerminating)
            {
                // nothing for now.
            }
        }

        // Example 4
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show("4. TaskScheduler_UnobservedTaskException");
            //log.ProcessError(e.Exception);
            e.SetObserved();
        }

        // Example 5 (not implemented)
        private void WinFormApplication_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("5. WinFormApplication_ThreadException");
            //log.ProcessError(e.Exception);
        }



        #endregion

    }

}
