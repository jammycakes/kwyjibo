using Kwyjibo.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Kwyjibo.Tests.Configuration
{
    [TestFixture]
    public class KwyjiboConfigurationExtensionsFixture
    {
        [Test]
        public void EmptyConfigurationShouldDisableAllContexts()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Configuration/empty.json")
                .Build();

            var options = new KwyjiboOptions();
            options.Configure(configuration);
        }
    }
}
