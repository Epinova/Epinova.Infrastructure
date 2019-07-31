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
        public void Debug_LogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Debug_LogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Debug_LogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Debug_LogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Debug_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"DEBUG: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }
    }
}