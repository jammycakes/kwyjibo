using System.Collections.Generic;

namespace Kwyjibo
{
    public interface IContext
    {
        bool Enabled { get; }

        string FullName { get; }

        string Name { get; }

        IContext Parent { get; }

        Status Status { get; }

        IHandler GetHandler(string handlerName);

        void Handle(string handlerName, ISession session, object[] data);
    }
}
