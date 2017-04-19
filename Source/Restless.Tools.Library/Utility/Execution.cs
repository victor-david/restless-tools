using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// Provides static methods for executing code.
    /// </summary>
    public static class Execution
    {
        /// <summary>
        /// Executes an action within a try/catch block.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="handler">A handler to process any exception or null to use the default handler (which shows a dialog box).</param>
        public static void TryCatch(Action action, Action<Exception> handler = null)
        {
            try
            {
                Validations.ValidateNull(action, "Action");
                action();
            }
            catch (Exception ex)
            {
                if (handler == null)
                {
                    MessageBox.Show(GetExceptionMessage(ex), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    handler(ex);
                }
            }
        }

        /// <summary>
        /// Executes an action within a try/catch block, swallowing any exception
        /// </summary>
        /// <param name="action">The action</param>
        public static void TryCatchSwallow(Action action)
        {
            TryCatch(action, (e) => { });
        }

        /// <summary>
        /// Exceutes an action within a using statement that's enclosed within a try/catch block.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="obj">The object that will be the target of the using statement.</param>
        /// <param name="action">The action.</param>
        public static void TryCatchUsing<T>(T obj, Action<T> action) where T : IDisposable
        {
            TryCatch(() =>
                {
                    Validations.ValidateNull(obj, "Object of TryCatchUsing");
                    using (obj)
                    {
                        action(obj);
                    }
                });
        }

        /// <summary>
        /// Recursively drills through inner execeptions, and returns the entite concatenrated message
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The string</returns>
        public static string GetExceptionMessage(Exception ex)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine(ex.Message);
            if (ex.InnerException != null)
            {
                b.AppendLine(GetExceptionMessage(ex.InnerException));
            }
            return b.ToString();
        }
    }
}
