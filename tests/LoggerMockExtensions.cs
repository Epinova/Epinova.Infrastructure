using System;
using EPiServer.Logging;
using Moq;

namespace Epinova.InfrastructureTests
{
    internal static class LoggerMockExtensions
    {
        /// <summary>
        /// Performs verification on the quite complex <see cref="ILogger.Log{TState, TException}(Level, TState, TException, Func{TState, TException, string}, Type)"/>.
        /// <para>Do not pass in <see cref="It.IsAny{TValue}"/> as parameters here. That will be treated as NULLs</para>
        /// </summary>
        public static void VerifyLog<TException>(
            this Mock<ILogger> instance,
            Level level,
            string message,
            TException exception,
            Times times)
            where TException : Exception
        {
            instance.Verify(m => m.Log(level, message, exception, It.IsAny<Func<string, Exception, string>>(), It.IsAny<Type>()), times);
        }


        /// <summary>
        /// Performs verification on the quite complex <see cref="ILogger.Log{TState, TException}(Level, TState, TException, Func{TState, TException, string}, Type)"/>.
        /// <para>Do not pass in <see cref="It.IsAny{TValue}"/> as parameters here. That will be treated as NULLs</para>
        /// </summary>
        public static void VerifyLog<TState, TException>(this Mock<ILogger> instance, Level level, TException exception, Times times)
            where TException : Exception
        {
            instance.Verify(m => m.Log(level, It.IsAny<TState>(), exception, It.IsAny<Func<TState, Exception, string>>(), It.IsAny<Type>()), times);
        }


        /// <summary>
        /// Performs verification on the quite complex <see cref="ILogger.Log{TState, TException}(Level, TState, TException, Func{TState, TException, string}, Type)"/>.
        /// <para>Do not pass in <see cref="It.IsAny{TValue}"/> as parameters here. That will be treated as NULLs</para>
        /// </summary>
        public static void VerifyLog(this Mock<ILogger> instance, Level level, string message, Times times)
        {
            instance.Verify(m => m.Log(level, message, It.IsAny<Exception>(), It.IsAny<Func<string, Exception, string>>(), It.IsAny<Type>()), times);
        }


        /// <summary>
        /// Performs verification on the quite complex <see cref="ILogger.Log{TState, TException}(Level, TState, TException, Func{TState, TException, string}, Type)"/>.
        /// <para>Do not pass in <see cref="It.IsAny{TValue}"/> as parameters here. That will be treated as NULLs</para>
        /// </summary>
        public static void VerifyLog<TState>(this Mock<ILogger> instance, Level level, Times times)
        {
            instance.Verify(m => m.Log(level, It.IsAny<TState>(), It.IsAny<Exception>(), It.IsAny<Func<TState, Exception, string>>(), It.IsAny<Type>()), times);
        }
    }
}
