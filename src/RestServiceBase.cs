using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EPiServer.Logging;
using Newtonsoft.Json;

namespace Epinova.Infrastructure
{
    public abstract class RestServiceBase
    {
        private readonly ILogger _log;

        protected RestServiceBase(ILogger log)
        {
            _log = log;
        }

        public abstract string ServiceName { get; }

        public async Task<HttpResponseMessage> Call(Func<Task<HttpResponseMessage>> work, bool isVerbose = false)
        {
            HttpResponseMessage response;
            try
            {
                response = await work();
            }
            catch (Exception ex)
            {
                _log.Error($"{ServiceName} call failed horribly. Method: {work.Method?.Name}.", ex);
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _log.Warning($"Expected HTTP status code OK from {ServiceName} when fetching data. Actual: {response.StatusCode}. Method: {work.Method?.Name}.");
                return isVerbose ? response : null;
            }

            if (response.Content == null)
            {
                _log.Warning($"Could not fetch data from {ServiceName}. Service returned NULL. Method: {work.Method?.Name}.");
                return isVerbose ? response : null;
            }

            return response;
        }

        public async Task<T> ParseJson<T>(HttpResponseMessage response) where T : IServiceResponseMessage, new()
        {
            try
            {
                return await ParseJsonContent<T>(response.Content);
            }
            catch (Exception ex)
            {
                _log.Error("Deserializing json failed.", ex);
                return CreateErrorResult<T>("Deserializing json failed");
            }
        }

        public async Task<T[]> ParseJsonArray<T>(HttpResponseMessage response) where T : IServiceResponseMessage, new()
        {
            try
            {
                return await ParseJsonContent<T[]>(response.Content);
            }
            catch (Exception ex)
            {
                _log.Error("Deserializing json array failed.", ex);
                return new[] { CreateErrorResult<T>("Deserializing json array failed") };
            }
        }

        public async Task<T> ParseXml<T>(HttpResponseMessage response) where T : IServiceResponseMessage, new()
        {
            string xml = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                xml = await response.Content.ReadAsStringAsync();
                _log.Debug("Raw XML: {0}", xml);

                using (var reader = new StringReader(xml))
                {
                    return (T) serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Deserializing xml failed.", ex);
                return CreateErrorResult<T>($"Deserializing xml failed: {xml}");
            }
        }

        protected static string BuildQueryString(IDictionary<string, string> nvc)
        {
            return String.Join("&", nvc.Select(pair => $"{pair.Key}={pair.Value}"));
        }

        private static T CreateErrorResult<T>(string message) where T : IServiceResponseMessage, new()
        {
            return new T { ErrorMessage = message };
        }

        private async Task<T> ParseJsonContent<T>(HttpContent content)
        {
            if (_log.IsDebugEnabled())
            {
                string json = await content.ReadAsStringAsync();
                _log.Debug("Raw JSON: {0}", json);
                return JsonConvert.DeserializeObject<T>(json);
            }

            using (Stream s = await content.ReadAsStreamAsync())
            using (var sr = new StreamReader(s))
            using (JsonReader r = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(r);
            }
        }
    }
}