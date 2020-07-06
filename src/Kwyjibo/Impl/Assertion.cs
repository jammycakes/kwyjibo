using System.Threading.Tasks;

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

        public void Handle(params object[] data)
        {
            _kwyjibo.HandleInternal(_handlerName, data);
        }

        public Task HandleAsync(params object[] data)
        {
            return _kwyjibo.HandleInternalAsync(_handlerName, data);
        }
    }
}
