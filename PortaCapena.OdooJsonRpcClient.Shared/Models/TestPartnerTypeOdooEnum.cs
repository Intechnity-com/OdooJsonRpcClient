using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestPartnerTypeOdooEnum
    {
        [EnumMember(Value = "contact")]
        Contact = 1,

        [EnumMember(Value = "invoice")]
        InvoiceAddress = 2,
    }
}
