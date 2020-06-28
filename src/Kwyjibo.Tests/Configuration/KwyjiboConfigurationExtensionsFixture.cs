using System.Security;
using System.Security.Principal;
using FluentAssertions;
using Kwyjibo.Extensions.Configuration;
using Kwyjibo.Impl;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Kwyjibo.Tests.Configuration
{
    [TestFixture]
    public class KwyjiboConfigurationExtensionsFixture
    {
        [Test]
        public void EmptyConfigurationShouldEnableAllContexts()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Configuration/empty.json")
                .Build();

            var options = new KwyjiboOptions();
            options.Configure(configuration);
            var tree = new ContextTree(options);
            tree.Enabled.Should().BeTrue();
        }

        [Test]
        public void SpecifiedConfigurationShouldBeReadCorrectly()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Configuration/sample.json")
                .Build();

            var options = new KwyjiboOptions();
            options.Configure(configuration);
            options.ForContext<KwyjiboConfigurationExtensionsFixture>()
                .When<IIdentity>(id => id.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            var tree = new ContextTree(options);
            tree.Enabled.Should().BeFalse();
            var context = tree.GetContext<KwyjiboConfigurationExtensionsFixture>();
            context.Enabled.Should().BeFalse();
            context.Parent.Enabled.Should().BeFalse();
            context.Parent.Parent.Enabled.Should().BeTrue();
            context.Parent.Parent.Parent.Enabled.Should().BeTrue();
        }
    }
}
