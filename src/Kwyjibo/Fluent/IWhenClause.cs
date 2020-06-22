using System;

namespace Kwyjibo.Fluent
{
    public interface IWhenClause
    {
        void Throw<TException>() where TException : Exception, new();

        void Throw(Func<Exception> exceptionBuilder);
    }
}
