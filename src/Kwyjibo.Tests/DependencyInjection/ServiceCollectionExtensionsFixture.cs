using System;
using System.Security.Principal;
using Kwyjibo.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Kwyjibo.Tests.DependencyInjection
{
    [TestFixture]
    public class ServiceCollectionExtensionsFixture
    {
        private IServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void Arrange()
        {
            var services = new ServiceCollection();
            services
                .AddScoped<KwyjiboedService>()
                .AddKwyjibo(cfg => {
                    cfg.Add<KwyjiboedService>("test")
                        .When<IIdentity>(identity => identity.Name.Contains("kwyjibo"))
                        .Throw<InvalidOperationException>();
                    cfg.AddInput<IIdentity>();
                });
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
