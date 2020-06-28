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
            options.ForContext<KwyjiboFixture>()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>(mock.Object);
            Assert.Throws<SecurityException>(() => kwyjibo.Assert());
        }

        private IKwyjibo CreateKwyjibo<TDefinitionContext, TKwyjiboContext>(string name)
        {
            var options = new KwyjiboOptions();
            options.ForContext<TDefinitionContext>()
                .Named(name)
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            return builder.Build<TKwyjiboContext>(mock.Object);
        }

        [Test]
        public void NamedKwyjiboShouldThrow()
        {
            var kwyjibo = CreateKwyjibo<KwyjiboFixture, KwyjiboFixture>("foobar");
            Assert.Throws<SecurityException>(() => kwyjibo.For("foobar").Assert());
        }

        [Test]
        public void KwyjiboThatDoesNotMatchOnNameShouldNotThrow()
        {
            var kwyjibo = CreateKwyjibo<KwyjiboFixture, KwyjiboFixture>("foobar");
            Assert.DoesNotThrow(() => kwyjibo.Assert());
        }

        [Test]
        public void KwyjiboThatDoesNotMatchOnContextShouldNotThrow()
        {
            var kwyjibo = CreateKwyjibo<KwyjiboFixture, object>("foobar");
            Assert.DoesNotThrow(() => kwyjibo.For("foobar").Assert());
        }

        [Test]
        public void KwyjiboThatDoesNotSatisfyPredicateShouldNotThrow()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .When<IIdentity>(s => false)
                .Throw<SecurityException>();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>(mock.Object);
            Assert.DoesNotThrow(() => kwyjibo.Assert());
        }

        [Test]
        public void KwyjiboShouldThrowWithAdditionalData()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            Assert.Throws<SecurityException>(() => kwyjibo.Assert(mock.Object));
        }

        [Test]
        public void DisabledKwyjiboShouldNotThrow()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .Disable()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            Assert.DoesNotThrow(() => kwyjibo.Assert(mock.Object));
        }

        [Test]
        public void ReEnabledKwyjiboShouldThrow()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .Enable()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            options.ForContext("Kwyjibo")
                .Disable();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            Assert.Throws<SecurityException>(() => kwyjibo.Assert(mock.Object));
        }

        [Test]
        public void InheritedDisabledKwyjiboShouldNotThrow()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
            options.ForContext("Kwyjibo")
                .Disable();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            Assert.DoesNotThrow(() => kwyjibo.Assert(mock.Object));
        }
    }
}
