using System.Collections.Generic;

namespace Kwyjibo.Impl
{
    public class Context : IContext
    {
        private IDictionary<string, IHandler> _handlers
            = new Dictionary<string, IHandler>();

        public string FullName { get; }

        public string Name { get; }

        public IContext Parent { get; }

        public Status Status { get; internal set; }
            = Status.Inherit;

        public bool Enabled =>
            Status == Status.Enabled ||
            Status == Status.Inherit && (Parent?.Enabled ?? false);

        public IHandler GetHandler(string name)
        {
            return _handlers.TryGetValue(name, out var handler)
                ? handler
                : null;
        }

        public void Handle(string handlerName, IList<IInputSource> sources)
        {
            throw new System.NotImplementedException();
        }

        public Context(IContext parent, string fullName, string name)
        {
            Parent = parent;
            FullName = fullName;
            Name = name;
        }
    }
}
