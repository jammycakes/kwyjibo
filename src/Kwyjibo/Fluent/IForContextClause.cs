using System;

namespace Kwyjibo.Fluent
{
    public interface IForContextClause
    {
        IForContextClause Named(string name);

        IForContextClause SetStatus(Status status);

        IForContextClause Throw<TException>() where TException : Exception, new();

        IForContextClause Throw(Func<Exception> exceptionBuilder);

        IForContextClause When<TService>(Predicate<TService> predicate);

    }
}
