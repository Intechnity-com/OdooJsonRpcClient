using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooClientContext
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        public OdooClientContext()
        {
        }

        public OdooClientContext(string language, string timezone)
        {
            this.Language = language;
            this.Timezone = timezone;
        }
    }
}