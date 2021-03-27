using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;

namespace ErabliereApi.IdentityServer
{
    public class ClaimConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Claim);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string type = jo.GetValue("type", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            string value = jo.GetValue("value", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            string valueType = jo.GetValue("valueType", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            string issuer = jo.GetValue("issuer", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            string originalIssuer = jo.GetValue("originalIssuer", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            return new Claim(type, value, valueType, issuer, originalIssuer);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
