using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Epinova.Infrastructure;
using EPiServer.Logging;
using Moq;
using Xunit;

namespace Epinova.InfrastructureTests
{
    public class RestServiceBaseTests
    {
        private readonly RestServiceBase _service;

        public RestServiceBaseTests()
        {
            _service = new TestableRestService(new Mock<ILogger>().Object);
        }

        [Fact]
        public async Task Call_ReturnsBadRequestStatusCode_ReturnsNull()
        {
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);

            HttpResponseMessage result = await _service.Call(() => sss);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.Accepted)]
        public async Task Call_ReturnsOkStatusCodeWithContent_ReturnsMessage(HttpStatusCode statusCode)
        {
            var message = new HttpResponseMessage { StatusCode = statusCode, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);

            HttpResponseMessage result = await _service.Call(() => sss);

            Assert.Equal(message, result);
        }

        [Fact]
        public async Task Call_Verbose_ReturnsBadRequestStatusCode_ReturnsMessageWithContent()
        {
            var message = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(Factory.GetString()) };
            Task<HttpResponseMessage> sss = Task.FromResult(message);

            HttpResponseMessage result = await _service.Call(() => sss, true);

            Assert.Equal(message.Content, result.Content);
        }

        #region Nested type: TestableRestService

        private class TestableRestService : RestServiceBase
        {
            public TestableRestService(ILogger log) : base(log)
            {
            }

            public override string ServiceName => nameof(TestableRestService);
        }

        #endregion
    }
}