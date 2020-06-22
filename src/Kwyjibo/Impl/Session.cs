using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Kwyjibo.Impl
{
    public class Session : ISession
    {
        private readonly IList<IInputSource> _sources;
        private readonly IList<Definition> _definitions;

        internal Session(IList<IInputSource> sources, IList<Definition> definitions)
        {
            _sources = sources;
            _definitions = definitions;
        }

        public void Assert(string context, string condition)
        {
            var applicableDefinitions =
                from definition in _definitions
                where definition.Context == context && definition.Name == condition
                select definition;

            var exceptions =
                from definition in applicableDefinitions
                from source in _sources
                from item in source.GetData(definition.InputType)
                where definition.Predicate(item)
                select definition.ExceptionBuilder();

            var exception = exceptions.FirstOrDefault();
            if (exception != null) {
                throw exception;
            }
        }
    }
}
