using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooVersion
    {
        [JsonProperty("server_version")]
        public string ServerVersion { get; set; }

        [JsonProperty("server_version_info")]
        public object[] ServerVersionInfo { get; set; }

        [JsonProperty("server_serie")]
        public string ServerSerie { get; set; }

        [JsonProperty("protocol_version")]
        public int ProtocolVersion { get; set; }
    }
}