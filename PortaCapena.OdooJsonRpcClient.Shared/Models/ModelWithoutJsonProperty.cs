using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [JsonConverter(typeof(OdooModelConverter))]
    public class ModelWithoutJsonProperty : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public string NotMapped { get; set; }
    }
}
