using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Epinova.Infrastructure;
using EPiServer.Logging;
using Moq;
using Xunit;

namespace Epinova.InfrastructureTests
{
    public class RestServiceBaseTests
    {
        private readonly Mock<ILogger> _logMock;
        private readonly RestServiceBase _service;

        public RestServiceBaseTests()
        {
            _logMock = new Mock<ILogger>();
            _service = new TestableRestService(_logMock.Object);
        }

        [Fact]
        public async Task Call_ReturnsBadRequestStatusCode_ReturnsNull()
        {
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);

            HttpResponseMessage result = await _service.CallAsync(() => sss);

            Assert.Null(result);
        }

        [Fact]
        public async Task Call_ReturnsBadRequestStatusCode_LogWarning()
        {
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);
            Func<Task<HttpResponseMessage>>  work = () => sss;
            await _service.CallAsync(work);

            _logMock.VerifyLog(Level.Warning, $"Expected HTTP status code OK from {_service.GetType().Name} when fetching data. Actual: {message.StatusCode}. Method: {work.Method?.Name}.", Times.Once());
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.Accepted)]
        public async Task Call_ReturnsOkStatusCodeWithContent_ReturnsMessage(HttpStatusCode statusCode)
        {
            var message = new HttpResponseMessage { StatusCode = statusCode, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);

            HttpResponseMessage result = await _service.CallAsync(() => sss);

            Assert.Equal(message, result);
        }

        [Fact]
        public async Task Call_Verbose_ReturnsBadRequestStatusCode_ReturnsMessageWithContent()
        {
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);

            HttpResponseMessage result = await _service.CallAsync(() => sss, true);

            Assert.Equal(message.Content, result.Content);
        }

        [Fact]
        public async Task ParseJson_DebugEnabled_LogRawJson()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"{{\"Foo\": \"{fooValue}\", \"Bar\": {barValue}}}";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            _logMock.Setup(m => m.IsEnabled(Level.Debug)).Returns(true);

            await _service.ParseJsonAsync<TestablePayload>(message);

            _logMock.VerifyLog(Level.Debug, $"Raw JSON: {payload}", Times.Once());
        }

        [Fact]
        public async Task ParseJson_InvalidJson_LogError()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"\"Foo\": \"{fooValue}\", \"Bar\": {barValue}";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            await _service.ParseJsonAsync<TestablePayload>(message);

            _logMock.VerifyLog(Level.Error, "Deserializing json failed.", Times.Once());
        }

        [Fact]
        public async Task ParseJson_InvalidJson_ReturnsInstanceWithErrorMessage()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"\"Foo\": \"{fooValue}\", \"Bar\": {barValue}";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            TestablePayload result = await _service.ParseJsonAsync<TestablePayload>(message);

            Assert.Equal("Deserializing json failed", result.ErrorMessage);
        }

        [Fact]
        public async Task ParseJson_ValidJson_ReturnsSerializedObject()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"{{\"Foo\": \"{fooValue}\", \"Bar\": {barValue}}}";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            TestablePayload result = await _service.ParseJsonAsync<TestablePayload>(message);

            Assert.Equal(fooValue + barValue, result.Foo + result.Bar);
        }

        [Fact]
        public async Task ParseJsonArray_ValidJson_ReturnsSerializedObject()
        {
            string fooValue1 = Factory.GetString();
            int barValue1 = Factory.GetInteger();
            string fooValue2 = Factory.GetString();
            int barValue2 = Factory.GetInteger();
            string payload = $"[{{\"Foo\": \"{fooValue1}\", \"Bar\": {barValue1}}}, {{\"Foo\": \"{fooValue2}\", \"Bar\": {barValue2}}}]";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            TestablePayload[] result = await _service.ParseJsonArrayAsync<TestablePayload>(message);

            Assert.Collection(
                result,
                first => Assert.Equal(fooValue1 + barValue1, first.Foo + first.Bar),
                second => Assert.Equal(fooValue2 + barValue2, second.Foo + second.Bar)
            );
        }

        [Fact]
        public async Task ParseXml_DebugEnabled_LogRawXml()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"<root><Foo>{fooValue}</Foo><Bar>{barValue}</Bar></root>";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            _logMock.Setup(m => m.IsEnabled(Level.Debug)).Returns(true);

            await _service.ParseXmlAsync<TestablePayload>(message);

            _logMock.VerifyLog(Level.Debug, $"Raw XML: {payload}", Times.Once());
        }

        [Fact]
        public async Task ParseXml_InvalidXml_LogError()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"<rootX><Foo>{fooValue}</Foo><Bar>{barValue}</Bar></rootY>";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            await _service.ParseXmlAsync<TestablePayload>(message);

            _logMock.VerifyLog(Level.Error, "Deserializing xml failed.", Times.Once());
        }

        [Fact]
        public async Task ParseXml_InvalidXml_ReturnsInstanceWithErrorMessage()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"<rootX><Foo>{fooValue}</Foo><Bar>{barValue}</Bar></rootY>";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            TestablePayload result = await _service.ParseXmlAsync<TestablePayload>(message);

            Assert.Equal($"Deserializing xml failed: {payload}", result.ErrorMessage);
        }

        [Fact]
        public async Task ParseXml_ValidXml_ReturnsSerializedObject()
        {
            string fooValue = Factory.GetString();
            int barValue = Factory.GetInteger();
            string payload = $"<root><Foo>{fooValue}</Foo><Bar>{barValue}</Bar></root>";
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(payload) };

            TestablePayload result = await _service.ParseXmlAsync<TestablePayload>(message);

            Assert.Equal(fooValue + barValue, result.Foo + result.Bar);
        }

        #region Nested type: TestablePayload

        /// <remarks>Made public for XML serializing purposes</remarks>
        [XmlRoot("root")]
        public class TestablePayload : IServiceResponseMessage
        {
            public int Bar { get; set; }
            public string ErrorMessage { get; set; }
            public string Foo { get; set; }
        }

        #endregion

        #region Nested type: TestableRestService

        private class TestableRestService : RestServiceBase
        {
            public TestableRestService(ILogger log) : base(log)
            {
            }
        }

        #endregion
    }
}