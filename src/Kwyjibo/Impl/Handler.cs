using System;
using System.Collections.Generic;

namespace Kwyjibo.Impl
{
    public class Handler : IHandler
    {
        public IContext Context { get; }

        public string Name { get; }

        public Type InputType { get; private set; }

        public Predicate<object> Predicate { get; private set; }

        public Func<Exception> ExceptionBuilder { get; private set; }

        public Handler(IContext context, string name)
        {
            Context = context;
            Name = name;
        }

        internal void Configure(Definition definition)
        {
            InputType = definition.InputType ?? InputType;
            Predicate = definition.Predicate ?? Predicate;
            ExceptionBuilder = definition.ExceptionBuilder ?? ExceptionBuilder;
        }

        public void Handle(IEnumerable<IInputSource> sources)
        {
            if (InputType == null || Predicate == null || ExceptionBuilder == null) {
                return;
            }

            foreach (var source in sources)
            foreach (var item in source.GetData(InputType)) {
                if (Predicate(item)) {
                    throw ExceptionBuilder();
                }
            }
        }
    }
}
