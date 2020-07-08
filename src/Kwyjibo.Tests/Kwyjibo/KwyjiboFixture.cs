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
        public void KwyjiboWithNoDataShouldNotThrow()
        {
            var options = new KwyjiboOptions();
            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            kwyjibo.When<IIdentity>().Matches(id => id.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
        }

        [Test]
        public void KwyjiboWithMatchingDataShouldThrow()
        {
            var options = new KwyjiboOptions();
            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();

            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            Assert.Throws<SecurityException>(() => {
                kwyjibo.When(mock.Object).Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboWithMatchingDataInSessionShouldThrow()
        {
            var options = new KwyjiboOptions();
            var builder = new KwyjiboBuilder(options);

            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var kwyjibo = builder.Build<KwyjiboFixture>(mock.Object);

            Assert.Throws<SecurityException>(() => {
                kwyjibo.When<IIdentity>().Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboWithNonMatchingDataButNonSpecifiedMatchingSessionShouldNotThrow()
        {
            var options = new KwyjiboOptions();
            var builder = new KwyjiboBuilder(options);

            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("bart");

            var mockForSession = new Mock<IIdentity>();
            mockForSession.SetupGet(i => i.Name).Returns("kwyjibo");

            var kwyjibo = builder.Build<KwyjiboFixture>(mockForSession.Object);

            Assert.DoesNotThrow(() => {
                kwyjibo.When<IIdentity>(mock.Object).Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboWithNonMatchingDataButSpecifiedMatchingSessionShouldNotThrow()
        {
            var options = new KwyjiboOptions();
            var builder = new KwyjiboBuilder(options);

            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("bart");

            var mockForSession = new Mock<IIdentity>();
            mockForSession.SetupGet(i => i.Name).Returns("kwyjibo");

            var kwyjibo = builder.Build<KwyjiboFixture>(mockForSession.Object);

            Assert.Throws<SecurityException>(() => {
                kwyjibo.When(mock.Object)
                    .OrSession()
                    .Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }
    }
}
