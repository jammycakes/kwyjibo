namespace Kwyjibo
{
    public interface IKwyjibo
    {
        void Assert();

        void Assert(string condition);
    }

    public interface IKwyjibo<TContext> : IKwyjibo
    {
    }
}
