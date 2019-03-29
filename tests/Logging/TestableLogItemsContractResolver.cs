using System;
using System.Collections.Generic;
using Epinova.Infrastructure.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Epinova.InfrastructureTests.Logging
{
    internal class TestableLogItemsContractResolver : LogItemsContractResolver
    {
        public IList<JsonProperty> TestableCreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return CreateProperties(type, memberSerialization);
        }
    }
}