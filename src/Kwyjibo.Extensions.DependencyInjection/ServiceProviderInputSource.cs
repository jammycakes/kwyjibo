using System;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Kwyjibo.Extensions.DependencyInjection
{
    public class ServiceProviderInputSource : IInputSource
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderInputSource(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable GetData(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }
    }
}
