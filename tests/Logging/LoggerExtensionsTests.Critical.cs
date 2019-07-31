using System;
using System.Linq;
using EPiServer.Logging;
using Moq;
using Xunit;
using LoggerExtensions = Epinova.Infrastructure.Logging.LoggerExtensions;

namespace Epinova.InfrastructureTests.Logging
{
    partial class LoggerExtensionsTests
    {
        [Fact]
        public void Critical_LazyLogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger((Level) 1337, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Critical(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Critical_LazyLogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Critical, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Critical(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Critical_LazyLogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger((Level) 1337, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Critical(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Critical_LazyLogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Critical, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Critical(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Critical_LazyLogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Critical, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Critical(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"CRITICAL: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }

        [Fact]
        public void Critical_LogAnonymousObjectAndExceptionOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();
            var exception = new Exception("OMG!");

            LoggerExtensions.Critical(logMock.Object, new { message = "Hello", number }, exception);
            logMock.VerifyLog<object, Exception>(Level.Critical, exception, Times.Once());
        }

        [Fact]
        public void Critical_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Critical, _output);
            int number = Factory.GetInteger();

            LoggerExtensions.Critical(logger, new { message = "Hello", number });
            Assert.Equal($"CRITICAL: {{\"message\":\"Hello\",\"number\":{number}}}", logger.Messages.First());
        }

        [Fact]
        public void Critical_LogAnonymousObjectOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();

            LoggerExtensions.Critical(logMock.Object, new { message = "Hello", number });
            logMock.VerifyLog<object>(Level.Critical, Times.Once());
        }
    }
}