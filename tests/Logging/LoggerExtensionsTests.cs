using Epinova.Infrastructure.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Epinova.InfrastructureTests.Logging
{
    public partial class LoggerExtensionsTests
    {
        private readonly ITestOutputHelper _output;

        public LoggerExtensionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetMessage_ObjectHasPropertyXhtmlString_IgnoeresIt()
        {
            string s1 = Factory.GetString();
            var candidate = new { StringProperty = s1, XhtmlStringProperty = new PropertyXhtmlString(Factory.GetString()) };
            string result = LoggerExtensions.GetMessage(candidate);
            string expectedResult = $"{{\"StringProperty\":\"{s1}\"}}";
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetMessage_ObjectHasPropertyXhtmlString_ThrowsNothing()
        {
            string s1 = Factory.GetString();
            var candidate = new { StringProperty = s1, XhtmlStringProperty = new PropertyXhtmlString(Factory.GetString()) };
            LoggerExtensions.GetMessage(candidate);
        }

        [Fact]
        public void GetMessage_ObjectHasXhtmlString_IgnoeresIt()
        {
            string s1 = Factory.GetString();
            var candidate = new { StringProperty = s1, XhtmlStringProperty = new XhtmlString(Factory.GetString()) };
            string result = LoggerExtensions.GetMessage(candidate);
            string expectedResult = $"{{\"StringProperty\":\"{s1}\"}}";
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetMessage_ObjectHasXhtmlString_ThrowsNothing()
        {
            string s1 = Factory.GetString();
            var candidate = new { StringProperty = s1, XhtmlStringProperty = new XhtmlString(Factory.GetString()) };
            LoggerExtensions.GetMessage(candidate);
        }

        [Fact]
        public void GetMessage_ObjectIsString_ReturnsWithoutSerializingIt()
        {
            string candidate = Factory.GetString();
            string result = LoggerExtensions.GetMessage(candidate);
            Assert.Equal(candidate, result);
        }
    }
}