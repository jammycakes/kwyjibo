namespace Kwyjibo.Fluent
{
    public interface IConditionOrSessionClause<TData> : IConditionClause<TData>
    {
        /// <summary>
        ///  Retrieve data to inspect from the current session in addition to
        ///  the data that has been passed in directly.
        /// </summary>
        /// <returns></returns>
        IActionClause<TData> OrSession();
    }
}
