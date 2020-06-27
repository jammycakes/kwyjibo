using System.Collections.Generic;

namespace Kwyjibo.Impl
{
    public class Context : IContext
    {
        private IDictionary<string, Handler> _handlers
            = new Dictionary<string, Handler>();

        public string FullName { get; }

        public string Name { get; }

        public IContext Parent { get; }

        public Status Status { get; internal set; }
            = Status.Inherit;

        public bool Enabled =>
            Status == Status.Enabled ||
            Status == Status.Inherit && (Parent?.Enabled ?? false);

        internal void ConfigureHandler(Definition definition)
        {
            var name = definition.Name;
            if (!_handlers.TryGetValue(name, out var handler)) {
                handler = new Handler(this, name);
                _handlers.Add(name, handler);
            }

            handler.Configure(definition);
        }

        public IHandler GetHandler(string name)
        {
            return _handlers.TryGetValue(name, out var handler)
                ? handler
                : null;
        }

        public void Handle(string handlerName, ISession session)
        {
            GetHandler(handlerName)?.Handle(session.GetSources());
        }

        public Context(IContext parent, string fullName, string name)
        {
            Parent = parent;
            FullName = fullName;
            Name = name;
        }
    }
}
