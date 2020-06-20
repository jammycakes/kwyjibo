using System;

namespace Kwyjibo.Fluent
{
    public interface IAddClause
    {
        IWhenClause When<TService>(Predicate<TService> predicate);
    }
}
