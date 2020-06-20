using System;
using System.Collections.Generic;
using Kwyjibo.Fluent;
using Kwyjibo.Impl;

namespace Kwyjibo
{
    public class KwyjiboOptions
    {
        public IList<Definition> Definitions { get; } = new List<Definition>();

        public IAddClause Add(string context, string name = "")
        {
            var definition = new Definition(context, name);
            Definitions.Add(definition);
            return definition;
        }

        public IAddClause Add<TContext>(string name = "")
            => Add(typeof(TContext).FullName, name);
    }
}
