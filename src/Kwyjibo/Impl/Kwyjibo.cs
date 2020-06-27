namespace Kwyjibo.Impl
{
    public class Kwyjibo : IKwyjibo
    {
        public ISession Session { get; }

        public IContext Context { get; }

        public Kwyjibo(ISession session, string context)
        {
            Session = session;
            Context = session.GetContext(context);
        }

        public void Assert(string handler = "")
        {
            Context?.Handle(handler, Session);
        }
    }

    public class Kwyjibo<TContext> : Kwyjibo, IKwyjibo<TContext>
    {
        public Kwyjibo(ISession session) : base(session, typeof(TContext).FullName)
        {
        }
    }
}
