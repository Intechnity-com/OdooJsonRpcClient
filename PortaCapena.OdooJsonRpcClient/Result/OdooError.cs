using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Result
{
    public class OdooError
    {
        public int Code { get; set; }
        public string Message { get; set; }

        [JsonProperty("http_status")]
        public string HttpStatus { get; set; }
        public OdooException Data { get; set; }


        public OdooError(string message, int code = 0, string httpStatus = "")
        {
            Message = message;
            Code = code;
            HttpStatus = httpStatus;
        }

        public override string ToString()
        {
            return $"{Message}, \n {Data?.Message}"; ;
        }
    }
}

