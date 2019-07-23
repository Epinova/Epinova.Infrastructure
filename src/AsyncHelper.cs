using System;
using System.Threading;
using System.Threading.Tasks;

namespace Epinova.Infrastructure
{
    /// <summary>
    /// Helper class to run async methods within a sync process. Use only when you _really_ cannot
    /// use the await keyword or use a proper synchronous method.
    /// </summary>
    public static class AsyncHelper
    {
        private static readonly TaskFactory TaskFactory = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        /// <summary>
        /// Synchronously executes an async task method which has a <see cref="TResult" /> return type
        /// </summary>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="task">Method to execute</param>
        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
            => TaskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Synchronously executes an async task method which has a void return value
        /// </summary>
        /// <param name="task">Method to execute</param>
        public static void RunSync(Func<Task> task)
            => TaskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}