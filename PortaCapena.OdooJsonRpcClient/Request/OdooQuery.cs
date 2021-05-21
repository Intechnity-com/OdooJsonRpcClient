using System.Collections.Generic;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooQuery
    {
        /// <summary>
        /// Get records with condition
        /// </summary>
        public OdooFilter Filters { get; set; }

        /// <summary>
        /// Get records with ony selected names
        /// </summary>
        public HashSet<string> ReturnFields { get; set; }


        /// <summary>
        /// Skip records
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Take records
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Order by field
        /// "{odooPropertyName} ASC" or "{odooPropertyName} DESC";
        /// </summary>
        public string Order { get; set; }


        public OdooQuery() : this(null) { }
        public OdooQuery(OdooContext context)
        {
            ReturnFields = new HashSet<string>();
            Filters = new OdooFilter();
        }
    }
}