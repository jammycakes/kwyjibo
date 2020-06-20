namespace Kwyjibo.Impl
{
    public class Session : ISession
    {
        private readonly IInputSource[] _sources;

        internal Session(IInputSource[] sources)
        {
            _sources = sources;
        }

        public void Assert(string context, string condition)
        {
        }
    }
}
