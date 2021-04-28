using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooClientContext
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        // https://stackoverflow.com/questions/46586281/alter-odoo-xmlrpc-context-to-use-a-specific-language
        // https://gist.github.com/ilyasProgrammer/cf6647356c9a3722f597f72b7685a4c3
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