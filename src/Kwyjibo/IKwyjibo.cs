namespace Kwyjibo
{
    public interface IKwyjibo
    {
        ISession Session { get; }

        IContext Context { get; }

        void Assert(string handler = "");
    }

    public interface IKwyjibo<TContext> : IKwyjibo
    {
    }
}
