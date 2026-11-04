using System.Collections.Generic;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery"]/*'/>
    public class OdooQuery
    {
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery.Filters"]/*'/>
        public OdooFilter Filters { get; set; }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery.ReturnFields"]/*'/>
        public HashSet<string> ReturnFields { get; set; }


        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery.Offset"]/*'/>
        public int? Offset { get; set; }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery.Limit"]/*'/>
        public int? Limit { get; set; }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery.Order"]/*'/>
        public string Order { get; set; }


        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQuery.ctor"]/*'/>
        public OdooQuery()
        {
            ReturnFields = new HashSet<string>();
            Filters = new OdooFilter();
        }
    }
}
