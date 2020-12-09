using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Result
{
    public class OdooResult<TResult>
    {
        public int Id { get; set; }
        public string Jsonrpc { get; set; }
        public OdooError Error { get; set; }

        [JsonProperty("result")]
        public TResult Value { get; set; }


        public string Message => Error?.Message;
        public bool Succeed => Error == null;
        public bool Failed => !Succeed;

        public static OdooResult<TResult> SucceedResult(TResult value) => new OdooResult<TResult>(value);
        public static OdooResult<TResult> FailedResult(string message, int code = 0, string httpStatus = "") => new OdooResult<TResult>(message, code, httpStatus);
        public static OdooResult<TResult> FailedResult<T>(OdooResult<T> result) => new OdooResult<TResult>(result.Id, result.Jsonrpc, result.Error);
  
        public OdooResult() { }

        private OdooResult(TResult value)
        {
            Value = value;
        }

        private OdooResult(int id, string jsonrpc, OdooError error)
        {
            Id = id;
            Jsonrpc = jsonrpc;
            Error = error;
        }
        private OdooResult(string message, int code = 0, string httpStatus = "")
        {
            Error = new OdooError(message, code, httpStatus);
        }
    }
}