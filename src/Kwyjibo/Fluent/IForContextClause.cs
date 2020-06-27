using System;

namespace Kwyjibo.Fluent
{
    public interface IForContextClause
    {
        IWhenClause When<TService>(Predicate<TService> predicate);

        IForContextClause SetStatus(Status status);
    }
}
