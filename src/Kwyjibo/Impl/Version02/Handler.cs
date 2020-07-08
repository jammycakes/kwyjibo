using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwyjibo.Impl.Version02
{
    public class Handler : IHandler
    {
        public IContext Context { get; }

        public string Name { get; }

        public Type InputType { get; private set; }

        public Predicate<object> Predicate { get; private set; }

        public Func<Exception> ExceptionBuilder { get; private set; }

        public TimeSpan Delay { get; private set; }

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
            Delay = definition.Delay ?? Delay;
        }

        private bool IsActivated(IEnumerable<IInputSource> sources)
        {
            if (InputType == null || Predicate == null) {
                return false;
            }

            if (ExceptionBuilder == null && Delay == TimeSpan.Zero) {
                return false;
            }

            foreach (var source in sources)
            foreach (var item in source.GetData(InputType)) {
                if (Predicate(item)) {
                    return true;
                }
            }

            return false;
        }

        public void Handle(IEnumerable<IInputSource> sources)
        {
            if (IsActivated(sources)) {
                if (Delay > TimeSpan.Zero) {
                    Thread.Sleep(Delay);
                }

                if (ExceptionBuilder != null) {
                    throw ExceptionBuilder();
                }
            }
        }

        public async Task HandleAsync(IEnumerable<IInputSource> sources)
        {
            if (IsActivated(sources)) {
                if (Delay > TimeSpan.Zero) {
                    await Task.Delay(Delay).ConfigureAwait(false);
                }

                if (ExceptionBuilder != null) {
                    throw ExceptionBuilder();
                }
            }
        }
    }
}
