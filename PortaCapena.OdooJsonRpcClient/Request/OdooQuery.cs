using System.Collections.Generic;
using System.Linq;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooQuery
    {
        public OdooFilter Filters { get; set; }
        public HashSet<string> ReturnFields { get; set; }

        // skip
        public int? Offset { get; set; }

        // take
        public int? Limit { get; set; }

        public string Order { get; set; }


        public OdooQuery()
        {
            ReturnFields = new HashSet<string>();
            Filters = new OdooFilter();
        }


        public object[] GetRequestFilters()
        {
            return Filters.Count > 0 ? Filters.ToArray() : null;
        }

        public string[] GetRequestFields()
        {
            return ReturnFields.Any() ? ReturnFields.ToArray() : null;
        }
    }
}