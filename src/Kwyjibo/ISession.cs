namespace Kwyjibo
{
    public interface ISession
    {
        void Assert(string context, string condition);
    }
}
