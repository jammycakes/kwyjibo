using System.Threading.Tasks;

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

        internal void HandleInternal(string handlerName, object[] data)
        {
            Context?.Handle(handlerName, Session, data);
        }

        internal Task HandleInternalAsync(string handlerName, object[] data)
        {
            return Context?.HandleAsync(handlerName, Session, data);
        }

        public void Handle(params object[] data)
        {
            HandleInternal(string.Empty, data);
        }

        public Task HandleAsync(params object[] data)
        {
            return HandleInternalAsync(string.Empty, data);
        }
    }

    public class Kwyjibo<TContext> : Kwyjibo, IKwyjibo<TContext>
    {
        public Kwyjibo(ISession session) : base(session, typeof(TContext).FullName)
        {
        }
    }
}
