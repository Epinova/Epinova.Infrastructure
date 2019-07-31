using System;
using EPiServer.Logging;
using Newtonsoft.Json;

namespace Epinova.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        private static readonly ILogger CrisisLogger = LogManager.GetLogger();


        public static void Critical(this ILogger instance, object message)
        {
            instance.Critical(message, GetMessage);
        }


        public static void Critical(this ILogger instance, object message, Exception exception)
        {
            instance.Critical(message, exception, GetMessage);
        }

        /// <summary>
        /// Logs a message at the Critical level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Critical<TState>(
            this ILogger logger,
            TState state,
            Func<TState, object> messageFormatter)
        {
            logger.Log(Level.Critical, state, x => GetMessage(messageFormatter(x)));
        }

        /// <summary>
        /// Logs a message and exception at the Critical level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <typeparam name="TException">The type of the exception that is being logged.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="exception">The exception that should be logged.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Critical<TState, TException>(
            this ILogger logger,
            TState state,
            TException exception,
            Func<TState, TException, object> messageFormatter)
            where TException : Exception
        {
            logger.Log(Level.Critical, state, exception, (x, y) => GetMessage(messageFormatter(x, y)), typeof (LoggerExtensions));
        }


        public static void Debug(this ILogger instance, object message)
        {
            instance.Debug(message, GetMessage);
        }


        public static void Debug(this ILogger instance, object message, Exception exception)
        {
            instance.Debug(message, exception, GetMessage);
        }

        /// <summary>
        /// Logs a message at the Debug level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Debug<TState>(
            this ILogger logger,
            TState state,
            Func<TState, object> messageFormatter)
        {
            logger.Log(Level.Debug, state, x => GetMessage(messageFormatter(x)));
        }

        /// <summary>
        /// Logs a message and exception at the Debug level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <typeparam name="TException">The type of the exception that is being logged.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="exception">The exception that should be logged.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Debug<TState, TException>(
            this ILogger logger,
            TState state,
            TException exception,
            Func<TState, TException, object> messageFormatter)
            where TException : Exception
        {
            logger.Log(Level.Debug, state, exception, (x, y) => GetMessage(messageFormatter(x, y)), typeof (LoggerExtensions));
        }

        public static void Error(this ILogger instance, object message)
        {
            instance.Error(message, GetMessage);
        }


        public static void Error(this ILogger instance, object message, Exception exception)
        {
            instance.Error(message, exception, GetMessage);
        }
        /// <summary>
        /// Logs a message at the Error level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Error<TState>(
            this ILogger logger,
            TState state,
            Func<TState, object> messageFormatter)
        {
            logger.Log(Level.Error, state, x => GetMessage(messageFormatter(x)));
        }

        /// <summary>
        /// Logs a message and exception at the Error level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <typeparam name="TException">The type of the exception that is being logged.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="exception">The exception that should be logged.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Error<TState, TException>(
            this ILogger logger,
            TState state,
            TException exception,
            Func<TState, TException, object> messageFormatter)
            where TException : Exception
        {
            logger.Log(Level.Error, state, exception, (x, y) => GetMessage(messageFormatter(x, y)), typeof (LoggerExtensions));
        }

        public static void Information(this ILogger instance, object message)
        {
            instance.Information(message, GetMessage);
        }


        public static void Information(this ILogger instance, object message, Exception exception)
        {
            instance.Information(message, exception, GetMessage);
        }
        /// <summary>
        /// Logs a message at the Information level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Information<TState>(
            this ILogger logger,
            TState state,
            Func<TState, object> messageFormatter)
        {
            logger.Log(Level.Information, state, x => GetMessage(messageFormatter(x)));
        }

        /// <summary>
        /// Logs a message and exception at the Information level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <typeparam name="TException">The type of the exception that is being logged.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="exception">The exception that should be logged.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Information<TState, TException>(
            this ILogger logger,
            TState state,
            TException exception,
            Func<TState, TException, object> messageFormatter)
            where TException : Exception
        {
            logger.Log(Level.Information, state, exception, (x, y) => GetMessage(messageFormatter(x, y)), typeof (LoggerExtensions));
        }

        public static void Trace(this ILogger instance, object message)
        {
            instance.Trace(message, GetMessage);
        }


        public static void Trace(this ILogger instance, object message, Exception exception)
        {
            instance.Trace(message, exception, GetMessage);
        }

        /// <summary>
        /// Logs a message at the Trace level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Trace<TState>(
            this ILogger logger,
            TState state,
            Func<TState, object> messageFormatter)
        {
            logger.Log(Level.Trace, state, x => GetMessage(messageFormatter(x)));
        }

        /// <summary>
        /// Logs a message and exception at the Trace level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <typeparam name="TException">The type of the exception that is being logged.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="exception">The exception that should be logged.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Trace<TState, TException>(
            this ILogger logger,
            TState state,
            TException exception,
            Func<TState, TException, object> messageFormatter)
            where TException : Exception
        {
            logger.Log(Level.Trace, state, exception, (x, y) => GetMessage(messageFormatter(x, y)), typeof (LoggerExtensions));
        }


        public static void Warning(this ILogger instance, object message)
        {
            instance.Warning(message, GetMessage);
        }


        public static void Warning(this ILogger instance, object message, Exception exception)
        {
            instance.Warning(message, exception, GetMessage);
        }
        /// <summary>
        /// Logs a message at the Warning level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Warning<TState>(
            this ILogger logger,
            TState state,
            Func<TState, object> messageFormatter)
        {
            logger.Log(Level.Warning, state, x => GetMessage(messageFormatter(x)));
        }

        /// <summary>
        /// Logs a message and exception at the Warning level using the supplied formatter and state.
        /// </summary>
        /// <typeparam name="TState">The type of the state object that should be passed to the formatter method.</typeparam>
        /// <typeparam name="TException">The type of the exception that is being logged.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="state">A state object to pass to the message formatter.</param>
        /// <param name="exception">The exception that should be logged.</param>
        /// <param name="messageFormatter">A message formatter that will be called when formatting the log message.</param>
        public static void Warning<TState, TException>(
            this ILogger logger,
            TState state,
            TException exception,
            Func<TState, TException, object> messageFormatter)
            where TException : Exception
        {
            logger.Log(Level.Warning, state, exception, (x, y) => GetMessage(messageFormatter(x, y)), typeof (LoggerExtensions));
        }

        internal static string GetMessage(object message, Exception exception)
        {
            return GetMessage(message);
        }


        internal static string GetMessage(object message)
        {
            if (message == null)
                return null;

            if (message is string)
                return message.ToString();

            string result;
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new LogItemsContractResolver()
                };
                jsonSerializerSettings.Converters.Add(new CustomMessageConverter());

                result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                CrisisLogger.Critical("Error when serializing log message.{0,-6}Log message: \"{1}\".{0,-6}Exception: \"{2}\"", Environment.NewLine, message, ex.Message);
                result = message.ToString();
            }

            return result;
        }
    }
}