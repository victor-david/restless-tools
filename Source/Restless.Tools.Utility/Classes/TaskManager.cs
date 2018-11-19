using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restless.Tools.Utility
{
    /// <summary>
    /// A singleton class for managing background tasks.
    /// </summary>
    public sealed class TaskManager
    {
        #region Private
        private Dictionary<int, TaskProperty> taskMap;
        #endregion

        /************************************************************************/

        #region Public Properties
        /// <summary>
        /// Gets the main scheduler, the one that was set when the singleton object was initialized.
        /// </summary>
        public TaskScheduler MainScheduler
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor and Access (Singleton)
        private TaskManager()
        {
            MainScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            taskMap = new Dictionary<int, TaskProperty>();
        }

        /// <summary>
        /// Gets the singleton instance of this object.
        /// You must first access this property from the UI thread to properly
        /// initialize the synchronization context for later use from other threads.
        /// </summary>
        public static TaskManager Instance { get; } = new TaskManager();

        /// <summary>
        /// Static constructor. Tells C# compiler not to mark type as beforefieldinit.
        /// </summary>
        static TaskManager()
        {
            // not sure if this is still needed in .NET 4.x
        }
        #endregion

        /************************************************************************/

        #region Public Methods
        /// <summary>
        /// Exceutes a task on a background thread.
        /// </summary>
        /// <param name="taskId">The application specific task id of the task to execute.</param>
        /// <param name="mainTask">
        /// The action for the task. This task receives the CancellationToken. 
        /// If <paramref name="canCancel"/> is false, this will be CancellationToken.None.
        /// </param>
        /// <param name="preWait">
        /// The pre-wait action, called only if a wait is needed. 
        /// Runs in the synchronization context that was established when the <see cref="Instance"/> property was first accessed (usually from the UI thread). 
        /// This should be a lightweight task.
        /// </param>
        /// <param name="postWait">
        /// The post wait action, called only if a wait is needed.
        /// Runs in the synchronization context that was established when the <see cref="Instance"/> property was first accessed (usually from the UI thread). 
        /// This should be a lightweight task.
        /// </param>
        /// <param name="canCancel">A boolean value that indicates if this task can be cancelled.</param>
        /// <param name="taskIdsToWaitFor">A list of task ids to wait for.</param>
        /// <returns>The newly created task.</returns>
        public Task ExecuteTask(int taskId, Action<CancellationToken> mainTask, Action preWait, Action postWait, bool canCancel, params int[] taskIdsToWaitFor)
        {
            Validations.ValidateNull(mainTask, "Main Task");

            AddTaskIdIfNeeded(taskId);

            int count = ++taskMap[taskId].Count;

            CancellationToken token = CancellationToken.None;
            if (canCancel)
            {
                taskMap[taskId].TokenSource = new CancellationTokenSource();
                token = taskMap[taskId].TokenSource.Token;
            }

            Debug.WriteLine("Enter ExecuteTask {0} - Count {1}", taskId, count);

            List<Task> tasksToWaitFor = new List<Task>();
            foreach (int id in taskIdsToWaitFor)
            {
                AddTaskIdIfNeeded(id);

                if (taskMap[id].Task != null && !taskMap[id].Task.IsCompleted)
                {
                    tasksToWaitFor.Add(taskMap[id].Task);
                }
            }

            taskMap[taskId].Task = Task.Factory.StartNew(() =>
            {
                if (tasksToWaitFor.Count > 0)
                {
                    if (preWait != null) Task.Factory.StartNew(preWait, CancellationToken.None, TaskCreationOptions.None, MainScheduler).Wait();
                    Debug.WriteLine("Task {0} waiting - Count {1}", taskId, count);
                    Task.WaitAll(tasksToWaitFor.ToArray<Task>());
                    Debug.WriteLine("Task {0} done waiting - Count {1}", taskId, count);
                    if (postWait != null) Task.Factory.StartNew(postWait, CancellationToken.None, TaskCreationOptions.None, MainScheduler).Wait();
                }

                Debug.WriteLine("Run main task {0} - Count {1}", taskId, count);
                mainTask(token);
                Debug.WriteLine("Done run main task {0} - Count {1}", taskId, count);
            }, token, TaskCreationOptions.None, TaskScheduler.Default);

            return taskMap[taskId].Task;
        }

        /// <summary>
        /// Attempts to cancel the task.
        /// </summary>
        /// <param name="taskId">The task id</param>
        public void CancelTask(int taskId)
        {
            AddTaskIdIfNeeded(taskId);
            var tp = taskMap[taskId];
            if (tp.Task != null && !tp.Task.IsCompleted && tp.TokenSource != null)
            {
                Debug.WriteLine("Task Manager - Cancel Task");
                tp.TokenSource.Cancel();
            }
        }

        /// <summary>
        /// Waits for all tasks to complete.
        /// </summary>
        /// <param name="preWait">
        /// The pre-wait action, called only if a wait is needed. 
        /// Runs in the synchronization context that was established when the <see cref="Instance"/> property was first accessed (usually from the UI thread). 
        /// This should be a lightweight task.
        /// </param>
        /// <param name="postWait">
        /// The post wait action, called only if a wait is needed.
        /// Runs in the synchronization context that was established when the <see cref="Instance"/> property was first accessed (usually from the UI thread). 
        /// This should be a lightweight task.
        /// </param>
        /// <param name="taskIdsToExclude">The excluded tasks.</param>
        /// <returns>true if a wait begins, false if nothng needs waiting for.</returns>
        public bool WaitForAllRegisteredTasks(Action preWait, Action postWait, params int[] taskIdsToExclude)
        {
            List<Task> tasksToWaitFor = new List<Task>();
            foreach (KeyValuePair<int, TaskProperty> pair in taskMap)
            {
                if (pair.Value.Task != null && !pair.Value.Task.IsCompleted)
                {
                    if (!taskIdsToExclude.Contains(pair.Key))
                    {
                        tasksToWaitFor.Add(pair.Value.Task);
                    }
                }
            }

            if (tasksToWaitFor.Count > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    if (preWait != null) Task.Factory.StartNew(preWait, CancellationToken.None, TaskCreationOptions.None, MainScheduler).Wait();
                    Debug.WriteLine("WaitForAllRegisteredTasks waiting");
                    Task.WaitAll(tasksToWaitFor.ToArray<Task>());
                    Debug.WriteLine("WaitForAllRegisteredTasks done waiting");
                    if (postWait != null) Task.Factory.StartNew(postWait, CancellationToken.None, TaskCreationOptions.None, MainScheduler).Wait();
                });
            }

            return tasksToWaitFor.Count > 0;
        }

        /// <summary>
        /// Gets a boolean value indicating if any of the specified tasks are still in progress.
        /// </summary>
        /// <param name="taskIds">The task ids.</param>
        /// <returns>true if any of the specified tasks is still running; otherwise, false.</returns>
        public bool BusyWithAny(params int[] taskIds)
        {
            foreach (int id in taskIds)
            {
                if (taskMap.ContainsKey(id) && taskMap[id].Task != null && !taskMap[id].Task.IsCompleted)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Runs the specified task on the orginal caller's thread, normally the UI thread.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="e">The action argument.</param>
        /// <returns>The newly created task.</returns>
        public Task DispatchTask<T>(Action<T> action, T e)
        {
            return Task.Factory.StartNew(() =>
            {
                if (action != null && e != null) action(e);
            }, CancellationToken.None, TaskCreationOptions.None, MainScheduler);
        }

        /// <summary>
        /// Runs the specified task on the orginal caller's thread, normally the UI thread.
        /// </summary>
        /// <typeparam name="T1">The first type.</typeparam>
        /// <typeparam name="T2">The second type.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="e1">The first action argument.</param>
        /// <param name="e2">The second action argument.</param>
        /// <returns>The newly created task.</returns>
        public Task DispatchTask<T1, T2>(Action<T1, T2> action, T1 e1, T2 e2)
        {
            return Task.Factory.StartNew(() =>
            {
                if (action != null && e1 != null && e2 != null) action(e1, e2);
            }, CancellationToken.None, TaskCreationOptions.None, MainScheduler);
        }

        /// <summary>
        /// Runs the specified task on the orginal caller's thread, normally the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The newly created task.</returns>
        public Task DispatchTask(Action action)
        {
            return Task.Factory.StartNew(() =>
            {
                action?.Invoke();
            }, CancellationToken.None, TaskCreationOptions.None, MainScheduler);
        }

        /// <summary>
        /// Processes exceptions for the specified task.
        /// </summary>
        /// <param name="task">The task</param>
        /// <param name="exceptionAction">The exception action. This runs on the original caller's thread, usually the UI thread.</param>
        public void ProcessExceptions(Task task, Action<Exception> exceptionAction)
        {
            Validations.ValidateNull(task, "Task");
            var aggException = task.Exception.Flatten();
            if (exceptionAction != null)
            {
                foreach (var exception in aggException.InnerExceptions)
                {
                    DispatchTask(exceptionAction, exception);
                }
            }
        }
        #endregion

        /************************************************************************/
        
        #region Private Methods
        private void AddTaskIdIfNeeded(int taskId)
        {
            if (!taskMap.ContainsKey(taskId))
            {
                taskMap.Add(taskId, new TaskProperty());
            }
        }
        #endregion

        /************************************************************************/

        #region Private Helper Class
        private class TaskProperty
        {
            public Task Task;
            public int Count;
            public CancellationTokenSource TokenSource;

            public TaskProperty()
            {
                Task = null;
                Count = 0;
                TokenSource = null;
            }
        }
        #endregion

    }
}
