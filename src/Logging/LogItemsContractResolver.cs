using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Epinova.Infrastructure.Logging
{
    internal class LogItemsContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<JsonProperty> fromBase = base.CreateProperties(type, memberSerialization).ToList();
            List<JsonProperty> result = fromBase
                .FindAll(
                    p =>
                    {
                        if (!IsSafeType(p.PropertyType))
                            return false;

                        if (p.DeclaringType != null && !IsSafeType(p.DeclaringType))
                            return false;
                        return true;
                    });
            return result;
        }

        private static bool IsSafeType(Type candidate)
        {
            if (candidate == null
                || candidate.Name == "PropertyXhtmlString"
                || candidate.Name == "XhtmlString"
                || candidate.Name == "PropertyXForm"
                || candidate.Name == "XForm")
                return false;
            return true;
        }
    }
}
