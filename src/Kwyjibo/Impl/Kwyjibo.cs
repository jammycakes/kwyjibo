namespace Kwyjibo.Impl
{
    public class Kwyjibo : IKwyjibo
    {
        public ISession Session { get; }

        public IContext Context { get; }

        public IAssertion For(string handlerName)
        {
            return new Assertion(this, handlerName);
        }

        public Kwyjibo(ISession session, string context)
        {
            Session = session;
            Context = session.GetContext(context);
        }

        internal void AssertInternal(string handlerName, object[] data)
        {
            Context?.Handle(handlerName, Session, data);
        }

        public void Assert(params object[] data)
        {
            AssertInternal(string.Empty, data);
        }
    }

    public class Kwyjibo<TContext> : Kwyjibo, IKwyjibo<TContext>
    {
        public Kwyjibo(ISession session) : base(session, typeof(TContext).FullName)
        {
        }
    }
}
