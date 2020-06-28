using System;
using System.Collections.Generic;

namespace Kwyjibo.Impl
{
    public class ContextTree : Context
    {
        private readonly IDictionary<string, Context> _contexts
            = new Dictionary<string, Context>(StringComparer.InvariantCultureIgnoreCase);

        private Context EnsureContext(string contextName)
        {
            contextName = contextName ?? string.Empty;

            Context context;
            if (_contexts.TryGetValue(contextName, out context)) {
                return context;
            }

            if (String.IsNullOrEmpty(contextName)) {
                context = this;
            }
            else {
                var ix = contextName.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase);
                if (ix < 0) {
                    context = new Context(this, contextName, contextName);
                }
                else {
                    string parentName = contextName.Substring(0, ix);
                    string name = contextName.Substring(ix + 1);
                    Context parent = EnsureContext(parentName);
                    context = new Context(parent, contextName, name);
                }
            }

            _contexts[contextName] = context;
            return context;
        }

        public ContextTree(KwyjiboOptions options)
            : base(null, "", "")
        {
            foreach (var definition in options.Definitions) {
                var context = EnsureContext(definition.Context);
                context.Status = definition.Status;
                context.ConfigureHandler(definition);
            }
        }

        public IContext GetContext(string name)
        {
            return _contexts.TryGetValue(name, out var context)
                ? context
                : null;
        }

        public IContext GetContext<TContext>()
        {
            return GetContext(typeof(TContext).FullName);
        }
    }
}
