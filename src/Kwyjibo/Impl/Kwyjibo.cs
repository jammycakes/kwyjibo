namespace Kwyjibo.Impl
{
    public class Kwyjibo : IKwyjibo
    {
        private readonly ISession _session;
        private readonly IContext _context;

        public Kwyjibo(ISession session, string context)
        {
            _session = session;
            _context = session.GetContext(context);
        }

        public void Assert(string condition)
        {
            _context?.Handle(condition, _session);
        }
    }

    public class Kwyjibo<TContext> : Kwyjibo, IKwyjibo<TContext>
    {
        public Kwyjibo(ISession session) : base(session, typeof(TContext).FullName)
        {
        }
    }
}
