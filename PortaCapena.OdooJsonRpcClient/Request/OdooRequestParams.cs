using System.Linq;
using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooRequestParams
    {
        [JsonProperty("service")]
        public string Service { get; }

        [JsonProperty("method")]
        public string Method { get; }

        [JsonProperty("args")]
        public object[] Args { get; }


        [JsonIgnore]
        public string Url { get; }

        public OdooRequestParams(string url, string service, string method, params object[] paramethers)
        {
            this.Url = url;
            this.Service = service;
            this.Method = method;
            this.Args = PrepareParams(paramethers);
        }

        protected object[] PrepareParams(object[] paramethers)
        {
            var list = paramethers.ToList();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null) list.RemoveAt(i);
                else break;
            }
            return list.ToArray();
        }
    }
}