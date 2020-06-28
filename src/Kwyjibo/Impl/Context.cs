using System;
using System.Collections.Generic;
using System.Linq;

namespace Kwyjibo.Impl
{
    public class Context : IContext
    {
        private IDictionary<string, Handler> _handlers
            = new Dictionary<string, Handler>(StringComparer.InvariantCultureIgnoreCase);

        public string FullName { get; }

        public string Name { get; }

        public IContext Parent { get; }

        public Status Status { get; internal set; }
            = Status.Inherit;

        public bool Enabled =>
            Status == Status.Enabled ||
            Status == Status.Inherit && (Parent?.Enabled ?? true);

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

        public void Handle(string handlerName, ISession session, object[] data)
        {
            if (!Enabled) {
                return;
            }

            var handler = GetHandler(handlerName);
            if (handler == null) {
                return;
            }

            if (data != null && data.Any()) {
                handler.Handle(new[] {new InputSource(data)});
            }

            handler.Handle(session.GetSources());
        }

        public Context(IContext parent, string fullName, string name)
        {
            Parent = parent;
            FullName = fullName;
            Name = name;
        }
    }
}
