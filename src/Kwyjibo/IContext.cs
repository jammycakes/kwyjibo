using System.Threading.Tasks;

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

        Task HandleAsync(string handlerName, ISession session, object[] data);
    }
}
