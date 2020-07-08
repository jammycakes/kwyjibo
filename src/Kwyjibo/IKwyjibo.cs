using Kwyjibo.Fluent;

namespace Kwyjibo
{
    public interface IKwyjibo : IAssertion
    {
        /// <summary>
        ///  Specifies an action to be performed on the basis of some data of
        ///  the specified type in the current session.
        /// </summary>
        /// <typeparam name="TData">
        ///  The data type of the object to retrieve from the session.
        /// </typeparam>
        /// <returns>
        /// </returns>
        IConditionClause<TData> When<TData>();

        /// <summary>
        ///  Specifies an action to be performed on the basis of an object
        ///  passed directly to the kwyjibo.
        /// </summary>
        /// <param name="item">
        ///  The object to be inspected.
        /// </param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        IConditionOrSessionClause<TData> When<TData>(TData item);

        /* ====== Version 0.2 API ====== */

        ISession Session { get; }

        IContext Context { get; }

        IAssertion For(string handlerName);
    }

    public interface IKwyjibo<TContext> : IKwyjibo
    {
    }
}
