using System.Linq;
using Newtonsoft.Json;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="OdooRequestParams"]/*'/>
    public class OdooRequestParams
    {
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params.Service"]/*'/>
        [JsonProperty("service")]
        public string Service { get; }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params.Method"]/*'/>
        [JsonProperty("method")]
        public string Method { get; }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params.Args"]/*'/>
        [JsonProperty("args")]
        public object[] Args { get; }


        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params.Url"]/*'/>
        [JsonIgnore]
        public string Url { get; }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params.ctor"]/*'/>
        public OdooRequestParams(string url, string service, string method, params object[] paramethers)
        {
            this.Url = url;
            this.Service = service;
            this.Method = method;
            this.Args = PrepareParams(paramethers);
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params.PrepareParams"]/*'/>
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
