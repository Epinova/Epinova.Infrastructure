using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Epinova.InfrastructureTests.Logging
{
    public class LogItemsContractResolverTests
    {
        [Fact]
        public void CreateProperties_DeclaringTypeIsXhtmlString_ReturnsEmptyList()
        {
            var resolver = new TestableLogItemsContractResolver();

            var candidate = new XhtmlString(Factory.GetString());

            IList<JsonProperty> result = resolver.TestableCreateProperties(candidate.GetType(), default(MemberSerialization));
            Assert.Empty(result);
        }


        [Fact]
        public void CreateProperties_TypeHasPropertyXhtmlStringProperty_BurnIt()
        {
            var resolver = new TestableLogItemsContractResolver();

            var candidate = new { StringProperty = Factory.GetString(), XhtmlStringProperty = new PropertyXhtmlString(Factory.GetString()) };

            IList<JsonProperty> result = resolver.TestableCreateProperties(candidate.GetType(), default(MemberSerialization));
            Assert.All(result.Select(p => p.PropertyType), Assert.IsNotType<PropertyXhtmlString>);
        }


        [Fact]
        public void CreateProperties_TypeHasXhtmlStringProperty_BurnIt()
        {
            var resolver = new TestableLogItemsContractResolver();

            var candidate = new { StringProperty = Factory.GetString(), XhtmlStringProperty = new XhtmlString(Factory.GetString()) };

            IList<JsonProperty> result = resolver.TestableCreateProperties(candidate.GetType(), default(MemberSerialization));
            Assert.All(result.Select(p => p.PropertyType), Assert.IsNotType<XhtmlString>);
        }
    }
}
