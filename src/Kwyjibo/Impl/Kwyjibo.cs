namespace Kwyjibo.Impl
{
    public class Kwyjibo : IKwyjibo
    {
        private readonly ISession _session;
        private readonly string _context;

        public Kwyjibo(ISession session, string context)
        {
            _session = session;
            _context = context;
        }

        public void Assert()
        {
            _session.Assert(_context, string.Empty);
        }

        public void Assert(string condition)
        {
            _session.Assert(_context, condition);
        }
    }

    public class Kwyjibo<TContext> : Kwyjibo, IKwyjibo<TContext>
    {
        public Kwyjibo(ISession session) : base(session, typeof(TContext).FullName)
        {
        }
    }
}
