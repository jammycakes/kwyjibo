using System.Collections.Generic;
using Kwyjibo.Fluent;
using Kwyjibo.Impl;

namespace Kwyjibo
{
    public class KwyjiboOptions
    {
        public IList<Definition> Definitions { get; } = new List<Definition>();

        public IForContextClause ForContext(string context)
        {
            var definition = new Definition(context);
            Definitions.Add(definition);
            return definition;
        }

        public IForContextClause ForContext<TContext>()
            => ForContext(typeof(TContext).FullName);
    }
}
