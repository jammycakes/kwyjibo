using System;
using Kwyjibo.Fluent;

namespace Kwyjibo.Impl
{
    public class Definition : IForContextClause, IWhenClause
    {
        public string Context { get; }

        public string Name { get; }

        public Type InputType { get; private set; }

        public Predicate<object> Predicate { get; private set; }

        public Func<Exception> ExceptionBuilder { get; private set; }

        public Definition(string context, string name)
        {
            Context = context;
            Name = name;
        }

        public IWhenClause When<TInput>(Predicate<TInput> predicate)
        {
            InputType = typeof(TInput);
            Predicate = svc => svc is TInput && predicate((TInput)svc);
            return this;
        }

        public void Throw<TException>() where TException : Exception, new()
        {
            ExceptionBuilder = () => new TException();
        }

        public void Throw(Func<Exception> exceptionBuilder)
        {
            ExceptionBuilder = exceptionBuilder;
        }
    }
}
