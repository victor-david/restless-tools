using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Restless.Tools.Threading
{
    /// <summary>
    /// Provides extension methods for tasks.
    /// </summary>
    public static class TaskManagerExtensions
    {
        /// <summary>
        /// Creates a continuation that executes according to the specified System.Threading.Tasks.TaskContinuationOptions.
        /// If the anecedent task threw an exception, this re-throws it.
        /// </summary>
        /// <param name="task">The anecedent task.</param>
        /// <param name="continuationAction">The continuation action.</param>
        /// <param name="continuationOptions">The continuation options.</param>
        /// <returns>The continuation task.</returns>
        public static Task ContinueWithException(this Task task, Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
        {
            return task.ContinueWith((t) =>
            {
                continuationAction(t);
                if (t.IsFaulted) throw t.Exception;
            }, continuationOptions);
        }

        /// <summary>
        /// Creates a continuation that executes using System.Threading.Tasks.TaskContinuationOptions.None.
        /// If the anecedent task threw an exception, this re-throws it.
        /// </summary>
        /// <param name="task">The anecedent task.</param>
        /// <param name="continuationAction">The continuation action.</param>
        /// <returns>The continuation task.</returns>
        public static Task ContinueWithException(this Task task, Action<Task> continuationAction)
        {
            return ContinueWithException(task, continuationAction, TaskContinuationOptions.None);
        }

        /// <summary>
        /// Provides a continuation that executes only on faulted.
        /// Callbacks are dispatched on the original caller's thread, normally the UI thread.
        /// </summary>
        /// <typeparam name="T">The type used to pass to <paramref name="exceptionAction"/></typeparam>
        /// <param name="task">The task.</param>
        /// <param name="exceptionAction">
        /// The action to run for each exception.
        /// This action is dispatched on the original caller's thread, normally the UI thread.
        /// </param>
        /// <param name="obj">The object of type T to pass to exceptionAction.</param>
        /// <returns>The continuation task.</returns>
        public static Task HandleExceptions<T>(this Task task, Action<T, Exception> exceptionAction, T obj)
        {
            return task.ContinueWith((t) =>
            {
                var aggException = t.Exception.Flatten();
                if (exceptionAction != null)
                {
                    foreach (var exception in aggException.InnerExceptions)
                    {
                        TaskManager.Instance.DispatchTask<T, Exception>(exceptionAction, obj, exception);
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Provides a continuation that executes only on faulted. 
        /// Callbacks are dispatched on the original caller's thread, normally the UI thread.
        /// </summary>
        /// <param name="task">The task</param>
        /// <param name="exceptionAction">
        /// The action to run for each exception.
        /// This action is dispatched on the original caller's thread, normally the UI thread.
        /// </param>
        /// <returns>The continuation task.</returns>
        public static Task HandleExceptions(this Task task, Action<Exception> exceptionAction)
        {
            return task.ContinueWith((t) =>
            {
                var aggException = t.Exception.Flatten();
                if (exceptionAction != null)
                {
                    foreach (var exception in aggException.InnerExceptions)
                    {
                        TaskManager.Instance.DispatchTask<Exception>(exceptionAction, exception);
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Provides a continuation that executes only on faulted. All exceptions are swallowed.
        /// </summary>
        /// <param name="task">The task</param>
        /// <returns>The continuation task.</returns>
        public static Task SwallowExceptions(this Task task)
        {
            return task.HandleExceptions(null);
        }
    }
}
