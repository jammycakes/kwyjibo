using System.Collections.Generic;
using System.Linq;
using Kwyjibo.Impl;

namespace Kwyjibo
{
    public class KwyjiboBuilder
    {
        private readonly ContextTree _contextTree;

        public KwyjiboBuilder(KwyjiboOptions options)
        {
            _contextTree = new ContextTree(options);
        }

        public ISession CreateSession(IEnumerable<IInputSource> inputSources)
            => new Session(inputSources.ToList(), _contextTree);

        public ISession CreateSession(params object[] data)
            => new Session(new IInputSource[] {new InputSource(data)}, _contextTree);

        public IKwyjibo Build(string context, IEnumerable<IInputSource> inputSources)
            => new Impl.Kwyjibo(CreateSession(inputSources), context);

        public IKwyjibo Build(string context, params object[] data)
            => new Impl.Kwyjibo(CreateSession(data), context);

        public IKwyjibo<TContext> Build<TContext>(IEnumerable<IInputSource> inputSources)
            => new Kwyjibo<TContext>(CreateSession(inputSources));

        public IKwyjibo<TContext> Build<TContext>(params object[] data)
            => new Kwyjibo<TContext>(CreateSession(data));
    }
}
