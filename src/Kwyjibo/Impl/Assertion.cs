namespace Kwyjibo.Impl
{
    public class Assertion : IAssertion
    {
        private readonly Kwyjibo _kwyjibo;
        private readonly string _handlerName;

        internal Assertion(Kwyjibo kwyjibo, string handlerName)
        {
            _kwyjibo = kwyjibo;
            _handlerName = handlerName;
        }

        public void Assert(params object[] data)
        {
            _kwyjibo.AssertInternal(_handlerName, data);
        }
    }
}
