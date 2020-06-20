using System.Security;
using System.Security.Principal;
using Moq;
using NUnit.Framework;

namespace Kwyjibo.Tests.Kwyjibo
{
    [TestFixture]
    public class KwyjiboFixture
    {
        [Test]
        public void EmptyKwyjiboShouldDoNothing()
        {
            var options = new KwyjiboOptions();
            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            kwyjibo.Assert();
        }

        [Test]
        public void ActiveKwyjiboShouldThrow()
        {
            var options = new KwyjiboOptions();
            options.Add<KwyjiboFixture>()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>(mock.Object);
            Assert.Throws<SecurityException>(() => kwyjibo.Assert());
        }
    }
}
