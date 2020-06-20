using System;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerable<object> GetData(Type inputType)
        {
            return _serviceProvider.GetServices(inputType);
        }
    }
}
