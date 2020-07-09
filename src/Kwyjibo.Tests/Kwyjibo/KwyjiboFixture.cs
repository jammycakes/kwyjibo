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
        private KwyjiboBuilder CreateBuilder()
            => new KwyjiboBuilder(new KwyjiboOptions());

        private IIdentity CreateIdentity(string name)
        {
            var mock = new Mock<IIdentity>();
            mock.SetupGet(m => m.Name).Returns(name);
            return mock.Object;
        }


        [Test]
        public void KwyjiboWithNoDataShouldNotThrow()
        {
            var builder = CreateBuilder();
            var kwyjibo = builder.Build<KwyjiboFixture>();
            kwyjibo.When<IIdentity>().Matches(id => id.Name.Contains("kwyjibo"))
                .Throw<SecurityException>();
        }

        [Test]
        public void KwyjiboWithMatchingDataShouldThrow()
        {
            var builder = CreateBuilder();
            var kwyjibo = builder.Build<KwyjiboFixture>();

            Assert.Throws<SecurityException>(() => {
                kwyjibo.When(CreateIdentity("kwyjibo"))
                    .Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboWithMatchingDataInSessionShouldThrow()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);

            Assert.Throws<SecurityException>(() => {
                kwyjibo.When<IIdentity>().Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboWithNonMatchingDataButNonSpecifiedMatchingSessionShouldNotThrow()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("bart");
            var identityForSession = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identityForSession);

            Assert.DoesNotThrow(() => {
                kwyjibo.When<IIdentity>(identity)
                    .Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboWithNonMatchingDataButSpecifiedMatchingSessionShouldNotThrow()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("bart");
            var identityForSession = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identityForSession);

            Assert.Throws<SecurityException>(() => {
                kwyjibo.When<IIdentity>(identity)
                    .OrSession()
                    .Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw<SecurityException>();
            });
        }

        [Test]
        public void KwyjiboShouldThrowFromFactoryMethod()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            Assert.Throws<SecurityException>(() => {
                kwyjibo.When<IIdentity>()
                    .Matches(id => id.Name.Contains("kwyjibo"))
                    .Throw(() => new SecurityException("kwyjibo"));
            }).Message.Should().Be("kwyjibo");
        }

        [Test]
        public void KwyjiboShouldDelay()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            kwyjibo.When<IIdentity>()
                .Matches(id => id.Name.Contains("kwyjibo"))
                .Wait(TimeSpan.FromSeconds(1));
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(900);
        }

        [Test]
        public async Task AsyncKwyjiboShouldDelay()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await kwyjibo.When<IIdentity>()
                .Matches(id => id.Name.Contains("kwyjibo"))
                .WaitAsync(TimeSpan.FromSeconds(1));
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(900);
        }

        [Test]
        public void MatchingKwyjiboShouldReportStatus()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("0");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            for (var i = 0; i <= 2; i++) {
                kwyjibo.When<IIdentity>()
                    .Matches(id => id.Name.Contains(i.ToString()))
                    .IsTriggered()
                    .Should().Be(i == 0);
            }
        }

        [Test]
        public void MatchingKwyjiboShouldPerformAction()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("0");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            for (var i = 0; i <= 2; i++) {
                bool done = false;
                kwyjibo.When<IIdentity>()
                    .Matches(id => id.Name.Contains(i.ToString()))
                    .Do(() => done = true);
                done.Should().Be(i == 0);
            }
        }

        [Test]
        public async Task MatchingAsyncKwyjiboShouldPerformAction()
        {
            var builder = CreateBuilder();
            var identity = CreateIdentity("0");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            for (var i = 0; i <= 2; i++) {
                bool done = false;
                await kwyjibo.When<IIdentity>()
                    .Matches(id => id.Name.Contains(i.ToString()))
                    .DoAsync(async () => done = true);
                done.Should().Be(i == 0);
            }
        }

        [Test]
        public void DisabledKwyjiboShouldNotTrigger()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>().Disable();
            var builder = new KwyjiboBuilder(options);
            var identity = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            kwyjibo.When<IIdentity>()
                .Matches(id => id.Name.Contains("kwyjibo"))
                .IsTriggered()
                .Should().Be(false);
        }

        [Test]
        public void ReEnabledKwyjiboShouldTrigger()
        {
            var options = new KwyjiboOptions();
            options.ForContext<KwyjiboFixture>().Enable();
            options.ForContext("Kwyjibo").Disable();
            var builder = new KwyjiboBuilder(options);
            var identity = CreateIdentity("kwyjibo");
            var kwyjibo = builder.Build<KwyjiboFixture>(identity);
            kwyjibo.When<IIdentity>()
                .Matches(id => id.Name.Contains("kwyjibo"))
                .IsTriggered()
                .Should().Be(true);
        }
    }
}
