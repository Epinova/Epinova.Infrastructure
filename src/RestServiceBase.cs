using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Safely call API method with the provided work. Exceptions are catched and logged. Response message
        /// is returned if the HTTP status code is greater than (including) 200 and 299
        /// </summary>
        /// <param name="work">The work load to perform</param>
        /// <param name="isVerbose">Set to true if you need response messages with response codes outside 200-299 returned. In case you need to read response body of a 404 ore something.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallAsync(Func<Task<HttpResponseMessage>> work, bool isVerbose = false)
        {
            HttpResponseMessage response;
            try
            {
                response = await work();
            }
            catch (Exception ex)
            {
                _log.Error($"{GetType().Name} call failed horribly. Method: {work.Method?.Name}.", ex);
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                _log.Warning($"Expected HTTP status code OK from {GetType().Name} when fetching data. Actual: {response.StatusCode}. Method: {work.Method?.Name}.");
                return isVerbose ? response : null;
            }

            if (response.Content == null)
            {
                _log.Warning($"Could not fetch data from {GetType().Name}. Service returned NULL. Method: {work.Method?.Name}.");
                return isVerbose ? response : null;
            }

            return response;
        }

        /// <summary>
        /// Deserializes the JSON structure contained by the specified <see cref="HttpResponseMessage.Content"/>
        /// into an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="response">The response message Containing the JSON to deserialize.</param>
        /// <returns>The instance of <typeparamref name="T[]" /> being deserialized.</returns>
        public async Task<T[]> ParseJsonArrayAsync<T>(HttpResponseMessage response) where T : IServiceResponseMessage, new()
        {
            try
            {
                return await ParseJsonContentAsync<T[]>(response.Content);
            }
            catch (Exception ex)
            {
                _log.Error("Deserializing json array failed.", ex);
                return new[] { CreateErrorResult<T>("Deserializing json array failed") };
            }
        }

        /// <summary>
        /// Deserializes the JSON structure contained by the specified <see cref="HttpResponseMessage.Content"/>
        /// into an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="response">The response message Containing the JSON to deserialize.</param>
        /// <returns>The instance of <typeparamref name="T" /> being deserialized.</returns>
        public async Task<T> ParseJsonAsync<T>(HttpResponseMessage response) where T : IServiceResponseMessage, new()
        {
            try
            {
                return await ParseJsonContentAsync<T>(response.Content);
            }
            catch (Exception ex)
            {
                _log.Error("Deserializing json failed.", ex);
                return CreateErrorResult<T>("Deserializing json failed");
            }
        }

        /// <summary>
        /// Deserializes the XML structure contained by the specified <see cref="HttpResponseMessage.Content"/>
        /// into an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="response">The response message Containing the XML to deserialize.</param>
        /// <returns>The instance of <typeparamref name="T" /> being deserialized.</returns>
        public async Task<T> ParseXmlAsync<T>(HttpResponseMessage response) where T : IServiceResponseMessage, new()
        {
            string xml = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                xml = await response.Content.ReadAsStringAsync();
                _log.Debug($"Raw XML: {xml}");

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

        /// <summary>
        /// Convert a dictionary to a query string parameter list
        /// </summary>
        protected static string BuildQueryString(IDictionary<string, string> nvc)
        {
            return String.Join("&", nvc.Select(pair => $"{pair.Key}={pair.Value}"));
        }

        private static T CreateErrorResult<T>(string message) where T : IServiceResponseMessage, new()
        {
            return new T { ErrorMessage = message };
        }

        private async Task<T> ParseJsonContentAsync<T>(HttpContent content)
        {
            if (_log.IsDebugEnabled())
            {
                string json = await content.ReadAsStringAsync();
                _log.Debug($"Raw JSON: {json}");
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