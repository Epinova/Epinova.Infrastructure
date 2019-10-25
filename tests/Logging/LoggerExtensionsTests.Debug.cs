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
        public void Debug_LazyLogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Debug_LazyLogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Debug_LazyLogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Debug_LazyLogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Debug_LazyLogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"DEBUG: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }

        [Fact]
        public void Debug_LogAnonymousObjectAndExceptionOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();
            var exception = new Exception("OMG!");

            LoggerExtensions.Debug(logMock.Object, new { message = "Hello", number }, exception);
            logMock.VerifyLog<object, Exception>(Level.Debug, exception, Times.Once());
        }

        [Fact]
        public void Debug_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Debug, _output);
            int number = Factory.GetInteger();

            LoggerExtensions.Debug(logger, new { message = "Hello", number });
            Assert.Equal($"DEBUG: {{\"message\":\"Hello\",\"number\":{number}}}", logger.Messages.First());
        }

        [Fact]
        public void Debug_LogAnonymousObjectOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();

            LoggerExtensions.Debug(logMock.Object, new { message = "Hello", number });
            logMock.VerifyLog<object>(Level.Debug, Times.Once());
        }
    }
}
