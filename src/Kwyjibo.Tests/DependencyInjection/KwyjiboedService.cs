namespace Kwyjibo.Tests.DependencyInjection
{
    public class KwyjiboedService
    {
        private readonly IKwyjibo<KwyjiboedService> _kwyjibo;

        public KwyjiboedService(IKwyjibo<KwyjiboedService> kwyjibo)
        {
            _kwyjibo = kwyjibo;
        }

        public void Execute()
        {
            _kwyjibo.For(nameof(Execute)).Handle();
        }
    }
}
