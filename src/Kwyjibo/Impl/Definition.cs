using System;
using Kwyjibo.Fluent;

namespace Kwyjibo.Impl
{
    public class Definition : IForContextClause
    {
        public string Context { get; }

        public string Name { get; private set; } = string.Empty;

        public Type InputType { get; private set; }

        public Predicate<object> Predicate { get; private set; }

        public Func<Exception> ExceptionBuilder { get; private set; }

        public Status Status { get; private set; } = Status.Inherit;

        public TimeSpan? Delay { get; private set; }

        public Definition(string context)
        {
            Context = context;
        }

        public IForContextClause When<TInput>(Predicate<TInput> predicate)
        {
            InputType = typeof(TInput);
            Predicate = svc => svc is TInput && predicate((TInput)svc);
            return this;
        }

        public IForContextClause Named(string name)
        {
            this.Name = name;
            return this;
        }

        public IForContextClause SetStatus(Status status)
        {
            Status = status;
            return this;
        }

        public IForContextClause Throw<TException>() where TException : Exception, new()
        {
            ExceptionBuilder = () => new TException();
            return this;
        }

        public IForContextClause Throw(Func<Exception> exceptionBuilder)
        {
            ExceptionBuilder = exceptionBuilder;
            return this;
        }

        public IForContextClause Wait(TimeSpan delay)
        {
            Delay = delay;
            return this;
        }
    }
}
