using System;
using System.Collections.Generic;

namespace Kwyjibo
{
    public interface IHandler
    {
        IContext Context { get; }

        string Name { get; }

        Type InputType { get; }

        Predicate<object> Predicate { get; }

        Func<Exception> ExceptionBuilder { get; }

        void Handle(IList<IInputSource> sources);
    }
}
