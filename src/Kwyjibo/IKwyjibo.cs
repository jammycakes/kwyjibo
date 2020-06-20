namespace Kwyjibo
{
    public interface IKwyjibo
    {
        void Assert(string condition = "");
    }

    public interface IKwyjibo<TContext> : IKwyjibo
    {
    }
}
