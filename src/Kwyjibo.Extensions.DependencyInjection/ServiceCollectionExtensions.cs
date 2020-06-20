using System;
using System.Linq;
using Kwyjibo.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Kwyjibo.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKwyjibo(this IServiceCollection services,
            Action<KwyjiboDIOptions> configure)
        {
            var options = new KwyjiboDIOptions();
            configure(options);
            var kwyjiboBuilder = new KwyjiboBuilder(options);

            services.AddSingleton(kwyjiboBuilder);
            services.AddScoped(services =>
                services.GetService<KwyjiboBuilder>()
                    .CreateSession(services.GetService<IInputSource>()));
            services.AddScoped(typeof(IKwyjibo<>), typeof(Kwyjibo<>));
            var serviceTypes = options.GetInputs();
            services.AddScoped<IInputSource, ServiceProviderInputSource>();

            return services;
        }
    }
}
