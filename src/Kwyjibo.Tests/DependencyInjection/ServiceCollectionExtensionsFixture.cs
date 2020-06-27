using System;
using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
            var identity = new Mock<IIdentity>();
            identity.SetupGet(i => i.Name).Returns("kwyjibo");

            var services = new ServiceCollection();
            services
                .AddScoped<KwyjiboedService>()
                .AddSingleton(identity.Object)
                .AddKwyjibo(cfg => {
                    cfg.ForContext<KwyjiboedService>()
                        .Named(nameof(KwyjiboedService.Execute))
                        .When<IIdentity>(identity => identity.Name.Contains("kwyjibo"))
                        .Throw<InvalidOperationException>();
                    cfg.AddInput<IIdentity>();
                });
            _serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public void KwyjiboCreatedByDependencyInjectionShouldThrow()
        {
            var service = _serviceProvider.GetService<KwyjiboedService>();
            Assert.Throws<InvalidOperationException>(() => service.Execute());
        }
    }
}
