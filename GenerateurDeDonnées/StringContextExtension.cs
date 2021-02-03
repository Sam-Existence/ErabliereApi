using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;

namespace GenerateurDeDonnées
{
    public static class StringContextExtension
    {
        public static StringContent ToStringContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj, jsonSerializerSettings);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
