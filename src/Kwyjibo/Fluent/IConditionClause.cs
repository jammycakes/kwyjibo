using System;

namespace Kwyjibo.Fluent
{
    public interface IConditionClause<TData>
    {
        /// <summary>
        ///  Inspects the objects provided either directly or from the session
        ///  to trigger the kwyjibo.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IActionClause<TData> Matches(Predicate<TData> predicate);
    }
}
