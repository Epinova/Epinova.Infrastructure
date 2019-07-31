using System;
using System.Linq;
using EPiServer.Logging;
using Xunit;
using LoggerExtensions = Epinova.Infrastructure.Logging.LoggerExtensions;

namespace Epinova.InfrastructureTests.Logging
{
    partial class LoggerExtensionsTests
    {
        [Fact]
        public void Information_LogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Information(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Information_LogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Information, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Information(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Information_LogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Information(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Information_LogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Information, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Information(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Information_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Information, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Information(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"INFORMATION: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }
    }
}