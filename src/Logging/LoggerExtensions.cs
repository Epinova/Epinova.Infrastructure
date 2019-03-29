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


        public static void Debug(this ILogger instance, object message)
        {
            instance.Debug(message, GetMessage);
        }


        public static void Debug(this ILogger instance, object message, Exception exception)
        {
            instance.Debug(message, exception, GetMessage);
        }


        public static void Error(this ILogger instance, object message)
        {
            instance.Error(message, GetMessage);
        }


        public static void Error(this ILogger instance, object message, Exception exception)
        {
            instance.Error(message, exception, GetMessage);
        }


        public static void Information(this ILogger instance, object message)
        {
            instance.Information(message, GetMessage);
        }


        public static void Information(this ILogger instance, object message, Exception exception)
        {
            instance.Information(message, exception, GetMessage);
        }


        public static void Trace(this ILogger instance, object message)
        {
            instance.Trace(message, GetMessage);
        }


        public static void Trace(this ILogger instance, object message, Exception exception)
        {
            instance.Trace(message, exception, GetMessage);
        }


        public static void Warning(this ILogger instance, object message)
        {
            instance.Warning(message, GetMessage);
        }


        public static void Warning(this ILogger instance, object message, Exception exception)
        {
            instance.Warning(message, exception, GetMessage);
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