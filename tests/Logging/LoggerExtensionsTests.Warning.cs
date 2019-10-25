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
        public void Warning_LazyLogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            bool isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Warning(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Warning_LazyLogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            bool isInvoked = false;
            var logger = new TestableLogger(Level.Warning, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Warning(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Warning_LazyLogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            bool isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Warning(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Warning_LazyLogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            bool isInvoked = false;
            var logger = new TestableLogger(Level.Warning, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Warning(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Warning_LazyLogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Warning, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Warning(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"WARNING: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }

        [Fact]
        public void Warning_LogAnonymousObjectAndExceptionOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();
            var exception = new Exception("OMG!");

            LoggerExtensions.Warning(logMock.Object, new { message = "Hello", number }, exception);
            logMock.VerifyLog<object, Exception>(Level.Warning, exception, Times.Once());
        }

        [Fact]
        public void Warning_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Warning, _output);
            int number = Factory.GetInteger();

            LoggerExtensions.Warning(logger, new { message = "Hello", number });
            Assert.Equal($"WARNING: {{\"message\":\"Hello\",\"number\":{number}}}", logger.Messages.First());
        }

        [Fact]
        public void Warning_LogAnonymousObjectOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();

            LoggerExtensions.Warning(logMock.Object, new { message = "Hello", number });
            logMock.VerifyLog<object>(Level.Warning, Times.Once());
        }
    }
}
