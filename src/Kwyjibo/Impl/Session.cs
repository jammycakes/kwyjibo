using System.Collections.Generic;
using System.Linq;

namespace Kwyjibo.Impl
{
    public class Session : ISession
    {
        private readonly IList<IInputSource> _sources;
        private readonly ContextTree _contextTree;

        internal Session(IList<IInputSource> sources, ContextTree contextTree)
        {
            _sources = sources;
            _contextTree = contextTree;
        }

        public IContext GetContext(string context)
        {
            return _contextTree.GetContext(context);
        }

        public IEnumerable<IInputSource> GetSources()
        {
            return _sources.AsEnumerable();
        }
    }
}
