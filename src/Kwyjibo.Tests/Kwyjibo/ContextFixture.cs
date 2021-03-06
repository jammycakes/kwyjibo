using System;
using System.Security.Principal;
using FluentAssertions;
using Kwyjibo.Impl;
using Kwyjibo.Tests.Kwyjibo.Version02;
using Moq;
using NUnit.Framework;

namespace Kwyjibo.Tests.Kwyjibo
{
    [TestFixture]
    public class ContextFixture
    {
        [Test]
        public void EmptyConfigurationShouldCreateDisabledRootContextOnly()
        {
            var options = new KwyjiboOptions();
            var tree = new ContextTree(options);
            tree.Enabled.Should().BeTrue();
            tree.GetContext("wibble").Should().BeNull();
        }

        [Test]
        public void InitialisedConfigurationShouldCreateContext()
        {
            var options = new KwyjiboOptions();
            options.ForContext<ContextFixture>()
                .Disable()
                .When<IIdentity>(id => id.Name.Contains("kwyjibo"))
                .Throw<InvalidOperationException>();

            var tree = new ContextTree(options);
            tree.Enabled.Should().BeTrue();
            var context = tree.GetContext<ContextFixture>();
            context.Enabled.Should().BeFalse();
            context.Name.Should().Be(this.GetType().Name);
            context.FullName.Should().Be(this.GetType().FullName);
        }

        [Test]
        public void InitialisedConfigurationShouldCreateIntermediateContexts()
        {
            var options = new KwyjiboOptions();
            options.ForContext<ContextFixture>()
                .Enable()
                .When<IIdentity>(id => id.Name.Contains("kwyjibo"))
                .Throw<InvalidOperationException>();

            var tree = new ContextTree(options);
            var context = tree.GetContext<ContextFixture>();
            context.FullName.Should().Be(this.GetType().FullName);
            context.Parent.FullName.Should().Be(this.GetType().Namespace);
            context.Parent.Status.Should().Be(Status.Inherit);
            context.Parent.Enabled.Should().BeTrue();
            context.Parent.Parent.Parent.Parent.Should().Be(tree);
            context.Parent.Parent.Parent.Parent.Parent.Should().BeNull();
        }

        [Test]
        public void UnconfiguredContextShouldBeNull()
        {
            var options = new KwyjiboOptions();
            options.ForContext<ContextFixture>()
                .Enable()
                .When<IIdentity>(id => id.Name.Contains("kwyjibo"))
                .Throw<InvalidOperationException>();

            var tree = new ContextTree(options);
            var context = tree.GetContext<KwyjiboFixture>();
            context.Should().BeNull();
        }

        [Test]
        public void ContextShouldHaveHandler()
        {
            var options = new KwyjiboOptions();
            options.ForContext<ContextFixture>()
                .Enable()
                .Named("foobar")
                .When<IIdentity>(id => id.Name.Contains("kwyjibo"))
                .Throw<InvalidOperationException>();

            var tree = new ContextTree(options);
            var context = tree.GetContext<ContextFixture>();
            var handler = context.GetHandler("foobar");
            handler.Should().NotBeNull();
            handler.InputType.Should().Be(typeof(IIdentity));

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.SetupGet(id => id.Name).Returns("kwyjibo");
            handler.Predicate(mockIdentity.Object).Should().BeTrue();
            handler.ExceptionBuilder().Should().BeOfType<InvalidOperationException>();
        }

        [Test]
        public void ContextShouldNotHaveUndefinedHandler()
        {
            var options = new KwyjiboOptions();
            options.ForContext<ContextFixture>()
                .Enable()
                .Named("alien")
                .When<IIdentity>(id => id.Name.Contains("kwyjibo"))
                .Throw<InvalidOperationException>();

            var tree = new ContextTree(options);
            var context = tree.GetContext<ContextFixture>();
            var handler = context.GetHandler("foobar");
            handler.Should().BeNull();
        }
    }
}
