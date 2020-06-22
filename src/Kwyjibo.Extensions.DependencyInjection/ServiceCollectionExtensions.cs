using System;
using Kwyjibo.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Kwyjibo
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
            services.AddScoped(provider =>
                provider.GetService<KwyjiboBuilder>()
                    .CreateSession(provider.GetServices<IInputSource>()));
            services.AddScoped(typeof(IKwyjibo<>), typeof(Kwyjibo<>));
            services.AddScoped<IInputSource, ServiceProviderInputSource>();

            return services;
        }
    }
}
