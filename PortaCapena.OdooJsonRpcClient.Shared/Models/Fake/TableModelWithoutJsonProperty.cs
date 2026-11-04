using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models.Fake
{
    [OdooTableName("test.other")]
    public class TableModelWithoutJsonProperty : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public string NotMapped { get; set; }
    }
}
