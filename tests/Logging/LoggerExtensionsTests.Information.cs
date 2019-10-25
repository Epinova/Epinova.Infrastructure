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
        public void Information_LazyLogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Information_LazyLogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Information_LazyLogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Information_LazyLogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            bool isInvoked = false;
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
        public void Information_LazyLogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Information, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Information(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"INFORMATION: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }

        [Fact]
        public void Information_LogAnonymousObjectAndExceptionOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();
            var exception = new Exception("OMG!");

            LoggerExtensions.Information(logMock.Object, new { message = "Hello", number }, exception);
            logMock.VerifyLog<object, Exception>(Level.Information, exception, Times.Once());
        }

        [Fact]
        public void Information_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Information, _output);
            int number = Factory.GetInteger();

            LoggerExtensions.Information(logger, new { message = "Hello", number });
            Assert.Equal($"INFORMATION: {{\"message\":\"Hello\",\"number\":{number}}}", logger.Messages.First());
        }

        [Fact]
        public void Information_LogAnonymousObjectOnEnabledLevel_VerifyLog()
        {
            var logMock = new Mock<ILogger>();
            int number = Factory.GetInteger();

            LoggerExtensions.Information(logMock.Object, new { message = "Hello", number });
            logMock.VerifyLog<object>(Level.Information, Times.Once());
        }
    }
}
