namespace Kwyjibo
{
    public interface IKwyjibo : IAssertion
    {
        ISession Session { get; }

        IContext Context { get; }

        IAssertion For(string handlerName);
    }

    public interface IKwyjibo<TContext> : IKwyjibo
    {
    }
}
