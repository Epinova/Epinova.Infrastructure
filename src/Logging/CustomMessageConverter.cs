using System;
using Newtonsoft.Json;

namespace Epinova.Infrastructure.Logging
{
    internal class CustomMessageConverter : JsonConverter
    {
        public override bool CanRead => false;


        public override bool CanConvert(Type objectType)
        {
            return typeof(ICustomLogMessage).IsAssignableFrom(objectType);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var customLogger = value as ICustomLogMessage;

            writer.WriteValue(customLogger == null ? value : customLogger.ToLoggableString());
        }
    }
}
