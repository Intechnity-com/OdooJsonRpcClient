using System.Collections.Generic;
using System.Net.Http;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <summary>
    /// HTTP call of the JSON API (Odoo 19+), translated from a JSON-RPC <see cref="OdooRequestModel"/> by <see cref="OdooJsonApiRequestMapper"/>.
    /// </summary>
    internal sealed class OdooJsonApiRequest
    {
        /// <summary>HTTP method - POST for model methods, GET for <c>/web/version</c>.</summary>
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;

        /// <summary>Full url, e.g. <c>https://my-company.odoo.com/json/2/res.partner/search_read</c>.</summary>
        public string Url { get; set; }

        /// <summary>JSON body: <c>ids</c>, <c>context</c> and named arguments; null for GET.</summary>
        public Dictionary<string, object> Body { get; set; }

        /// <summary>The endpoint is a web controller answering with a JSON-RPC envelope (e.g. <c>/web/database/list</c>).</summary>
        public bool JsonRpcEnvelope { get; set; }

        /// <summary>How the answer is converted to the value expected by the JSON-RPC based client methods.</summary>
        public OdooJsonApiAnswer Answer { get; set; } = OdooJsonApiAnswer.Value;

        /// <summary>Reason why the request cannot be sent with JSON API, or null.</summary>
        public string Error { get; set; }
    }

    /// <summary>
    /// Conversion of a JSON API answer to the value the JSON-RPC based client methods expect.
    /// </summary>
    internal enum OdooJsonApiAnswer
    {
        /// <summary>Answer is used as it is.</summary>
        Value,

        /// <summary><c>uid</c> from <c>res.users/context_get</c> - replaces <c>common.login</c>.</summary>
        UserId,

        /// <summary><c>/web/version</c> converted to the shape of <c>common.version</c>.</summary>
        Version,

        /// <summary>First id of the list - <c>create</c> of a single record returns a list in JSON API.</summary>
        FirstId
    }
}
