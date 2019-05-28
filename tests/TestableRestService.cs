using Epinova.Infrastructure;
using EPiServer.Logging;

namespace Epinova.InfrastructureTests
{
    internal class TestableRestService : RestServiceBase
    {
        public TestableRestService(ILogger log) : base(log)
        {
        }

        public override string ServiceName => nameof(TestableRestService);
    }
}