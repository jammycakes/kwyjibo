using System;

namespace Kwyjibo
{
    public class KwyjiboBuilder
    {
        private readonly KwyjiboOptions _options;

        public KwyjiboBuilder(KwyjiboOptions options)
        {
            _options = options;
        }

        public ISession CreateSession(IInputSource inputSource)
        {
            throw new NotImplementedException();
        }

        public ISession CreateSession(params object[] data)
        {
            throw new NotImplementedException();
        }

        public IKwyjibo Build(string context, IInputSource inputSource)
        {
            return new Impl.Kwyjibo(CreateSession(inputSource), context);
        }

        public IKwyjibo Build(string context, params object[] data)
        {
            return new Impl.Kwyjibo(CreateSession(data), context);
        }

        public IKwyjibo<TContext> Build<TContext>(IInputSource inputSource)
        {
            return new Impl.Kwyjibo<TContext>(CreateSession(inputSource));
        }

        public IKwyjibo<TContext> Build<TContext>(params object[] data)
        {
            return new Impl.Kwyjibo<TContext>(CreateSession(data));
        }
    }
}
