using System;
using System.Threading.Tasks;

namespace Kwyjibo.Fluent
{
    public interface IActionClause<TData>
    {
        /// <summary>
        ///  Throws an exception of the specified type when the kwyjibo
        ///  has been triggered.
        /// </summary>
        /// <typeparam name="TException">
        ///  The type of exception to throw.
        /// </typeparam>
        void Throw<TException>() where TException : Exception, new();

        /// <summary>
        ///  Creates and throws an exception when the kwyjibo has been triggered.
        /// </summary>
        /// <param name="exceptionFactory">
        ///  A factory function that creates the exception to be thrown.
        /// </param>
        void Throw(Func<Exception> exceptionFactory);

        /// <summary>
        ///  Waits for a specified time on the current thread when the kwyjibo
        ///  has been triggered.
        /// </summary>
        /// <param name="delay">
        ///  The length of time to delay.
        /// </param>
        void Wait(TimeSpan delay);

        /// <summary>
        ///  Waits for a specified time asynchronously when the kwyjibo has been
        ///  triggered.
        /// </summary>
        /// <param name="delay">
        ///  The length of time to delay.
        /// </param>
        /// <returns>
        ///  A <see cref="Task"/> that waits for the specified time.
        /// </returns>
        Task WaitAsync(TimeSpan delay);

        /// <summary>
        ///  Performs an action when the kwyjibo has been triggered.
        /// </summary>
        /// <param name="action">
        ///  The action to perform.
        /// </param>
        void Do(Action action);

        /// <summary>
        ///  Performs an action asynchronously when the kwyjibo has been triggered.
        /// </summary>
        /// <param name="task">
        ///  A <see cref="Task"/> containing the action to be performed.
        /// </param>
        /// <returns>
        ///  A <see cref="Task"/> that will perform the specified task if and
        ///  only if the kwyjibo has been triggered.
        /// </returns>
        Task DoAsync(Func<Task> task);

        /// <summary>
        ///  Gets a value indicating whether the kwyjibo has been triggered by
        ///  the current conditions.
        /// </summary>
        /// <returns>
        ///  true if the kwyjibo has been triggered, otherwise false.
        /// </returns>
        bool IsTriggered();
    }
}
