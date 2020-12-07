using System.Collections.Generic;
using System.Linq;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooQuery
    {
        public OdooFilter Filters { get; set; }
        public HashSet<string> ReturnFields { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public string Order { get; set; }


        public OdooQuery()
        {
            ReturnFields = new HashSet<string>();
            Filters = new OdooFilter();
        }

        public object[] GetFilters()
        {
            if (Filters.Count > 0)
                return Filters.ToArray();
            return null;
        }

        public string[] GetFields()
        {
            if (ReturnFields.Any())
                return ReturnFields.ToArray();
            return null;
        }
    }
}