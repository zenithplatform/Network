using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Serialization
{
    public static class MessageSerializer
    {
        static readonly JsonSerializerSettings _settings = null;

        static MessageSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public static string Serialize<T>(T message) where T : class
        {
            return JsonConvert.SerializeObject(message, _settings);
        }

        public static T Deserialize<T>(string json) where T : class
        {
            var serializer = new JsonSerializer();
            return Deserialize<T>(ref serializer, json);
        }

        public static T Deserialize<T>(ref JsonSerializer serializer, string json) where T : class
        {
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            JObject jObject = JObject.Parse(json);
            return jObject.ToObject<T>(serializer);
        }
    }
}
