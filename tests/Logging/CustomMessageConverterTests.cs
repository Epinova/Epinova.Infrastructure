using System.IO;
using System.Text;
using Epinova.Infrastructure.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Epinova.InfrastructureTests.Logging
{
    public class CustomMessageConverterTests
    {
        [Fact]
        public void CanConvert_ICustomLogMessageType_ReturnsFalse()
        {
            var converter = new CustomMessageConverter();

            var obj = new object();

            bool result = converter.CanConvert(obj.GetType());
            Assert.False(result);
        }

        [Fact]
        public void CanConvert_ICustomLogMessageType_ReturnsTrue()
        {
            var converter = new CustomMessageConverter();

            ICustomLogMessage obj = new Mock<ICustomLogMessage>().Object;

            bool result = converter.CanConvert(obj.GetType());
            Assert.True(result);
        }

        [Fact]
        public void WriteJson_ICustomLogMessageType_WriteCustomLogMessage()
        {
            var converter = new CustomMessageConverter();

            var value = new SomeModel();

            var stringBuilder = new StringBuilder();
            JsonWriter writer = new JsonTextWriter(new StringWriter(stringBuilder));
            var serializer = new JsonSerializer();
            converter.WriteJson(writer, value, serializer);

            Assert.Equal("\"loggable string\"", stringBuilder.ToString());
        }

        #region Nested type: SomeModel

        private class SomeModel : ICustomLogMessage
        {
            public string ToLoggableString()
            {
                return "loggable string";
            }
        }

        #endregion
    }
}