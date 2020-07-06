using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kwyjibo
{
    public interface IHandler
    {
        IContext Context { get; }

        string Name { get; }

        Type InputType { get; }

        Predicate<object> Predicate { get; }

        Func<Exception> ExceptionBuilder { get; }

        void Handle(IEnumerable<IInputSource> sources);

        Task HandleAsync(IEnumerable<IInputSource> sources);
    }
}
