using System;
using System.Collections.Generic;

namespace Kwyjibo.Impl
{
    public class Handler : IHandler
    {
        public IContext Context { get; }

        public string Name { get; }

        public Type InputType { get; }

        public Predicate<object> Predicate { get; }

        public Func<Exception> ExceptionBuilder { get; }

        public Handler(IContext context, Definition definition)
        {
            Context = context;
            Name = definition.Name;
            InputType = definition.InputType;
            Predicate = definition.Predicate;
            ExceptionBuilder = definition.ExceptionBuilder;
        }

        public void Handle(IList<IInputSource> sources)
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
