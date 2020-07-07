using System;
using System.Diagnostics;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
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
            kwyjibo.Handle();
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
            Assert.Throws<SecurityException>(() => kwyjibo.Handle());
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
            Assert.Throws<SecurityException>(() => kwyjibo.For("foobar").Handle());
        }

        [Test]
        public void KwyjiboThatDoesNotMatchOnNameShouldNotThrow()
        {
            var kwyjibo = CreateKwyjibo<KwyjiboFixture, KwyjiboFixture>("foobar");
            Assert.DoesNotThrow(() => kwyjibo.Handle());
        }

        [Test]
        public void KwyjiboThatDoesNotMatchOnContextShouldNotThrow()
        {
            var kwyjibo = CreateKwyjibo<KwyjiboFixture, object>("foobar");
            Assert.DoesNotThrow(() => kwyjibo.For("foobar").Handle());
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
            Assert.DoesNotThrow(() => kwyjibo.Handle());
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
            Assert.Throws<SecurityException>(() => kwyjibo.Handle(mock.Object));
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
            Assert.DoesNotThrow(() => kwyjibo.Handle(mock.Object));
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
            Assert.Throws<SecurityException>(() => kwyjibo.Handle(mock.Object));
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
            Assert.DoesNotThrow(() => kwyjibo.Handle(mock.Object));
        }

        [Test]
        public async Task DelayKwyjiboShouldDelay()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Wait(TimeSpan.FromSeconds(1.5));
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await kwyjibo.HandleAsync(mock.Object);
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(1000);
        }


        [Test]
        public async Task DisabledDelayKwyjiboShouldNotDelay()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>()
                .When<IIdentity>(s => s.Name.Contains("kwyjibo"))
                .Wait(TimeSpan.FromSeconds(1.5))
                .Disable();
            var mock = new Mock<IIdentity>();
            mock.SetupGet(i => i.Name).Returns("kwyjibo");

            var builder = new KwyjiboBuilder(options);
            var kwyjibo = builder.Build<KwyjiboFixture>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await kwyjibo.HandleAsync(mock.Object);
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        }
    }
}
