using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <summary>
    /// Translates JSON-RPC requests (<c>common</c> / <c>db</c> / <c>object.execute_kw</c>) built by the client
    /// to calls of the JSON API of Odoo 19+, where arguments are passed by name.
    /// </summary>
    internal static class OdooJsonApiRequestMapper
    {
        /// <summary>
        /// Names of positional arguments of model methods called without record ids (from Odoo 19 method signatures).
        /// </summary>
        internal static readonly Dictionary<string, string[]> ModelMethodArguments = new Dictionary<string, string[]>
        {
            [OdooOperation.SearchRead] = new[] { "domain", "fields", "offset", "limit", "order" },
            [OdooOperation.Search] = new[] { "domain", "offset", "limit", "order" },
            [OdooOperation.SearchCount] = new[] { "domain", "limit" },
            [OdooOperation.Create] = new[] { "vals_list" },
            [OdooOperation.FieldsGet] = new[] { "allfields", "attributes" },
            [OdooOperation.NameSearch] = new[] { "name", "domain", "operator", "limit" },
            [OdooOperation.NameCreate] = new[] { "name" },
            [OdooOperation.DefaultGet] = new[] { "fields" },
            [OdooOperation.ReadGroup] = new[] { "domain", "fields", "groupby", "offset", "limit", "orderby", "lazy" },
            [OdooOperation.CheckAccessRights] = new[] { "operation", "raise_exception" },
        };

        /// <summary>
        /// Names of positional arguments that follow record ids of record methods. Other record methods
        /// (unlink, get_metadata, actions...) take only ids.
        /// </summary>
        internal static readonly Dictionary<string, string[]> RecordMethodArguments = new Dictionary<string, string[]>
        {
            [OdooOperation.Read] = new[] { "fields", "load" },
            [OdooOperation.Update] = new[] { "vals" },
            [OdooOperation.Copy] = new[] { "default" },
        };

        /// <summary>
        /// Translates a request created by <see cref="OdooRequestModel"/> factory methods.
        /// When the request cannot be expressed in JSON API, <see cref="OdooJsonApiRequest.Error"/> is set.
        /// </summary>
        internal static OdooJsonApiRequest Map(OdooRequestModel request)
        {
            var config = request.Config;
            var service = request.Params.Service;
            var method = request.Params.Method;

            if (service == "common" && method == "version")
                return new OdooJsonApiRequest { HttpMethod = System.Net.Http.HttpMethod.Get, Url = config.ApiUrl + "/web/version", Answer = OdooJsonApiAnswer.Version };

            // JSON API has no login - the API key identifies the user
            if (service == "common" && method == "login")
                return new OdooJsonApiRequest { Url = config.ApiUrl + "/json/2/res.users/context_get", Body = new Dictionary<string, object>(), Answer = OdooJsonApiAnswer.UserId };

            if (service == "db" && method == "list")
                return new OdooJsonApiRequest
                {
                    Url = config.ApiUrl + "/web/database/list",
                    Body = new Dictionary<string, object> { ["jsonrpc"] = "2.0", ["id"] = request.Id, ["method"] = "call", ["params"] = new Dictionary<string, object>() },
                    JsonRpcEnvelope = true
                };

            if (service == "object" && (method == "execute_kw" || method == "execute"))
                return MapModelMethod(request.Params, config.ApiUrl);

            return new OdooJsonApiRequest { Error = $"JSON API does not support '{service}.{method}'." };
        }

        /// <summary>
        /// Translates <c>execute_kw(db, uid, password, model, method, args, kwargs)</c> / <c>execute(db, uid, password, model, method, *args)</c>
        /// to <c>POST /json/2/model/method</c> with named arguments.
        /// </summary>
        internal static OdooJsonApiRequest MapModelMethod(OdooRequestParams requestParams, string apiUrl)
        {
            var args = requestParams.Args;
            var model = (string)args[3];
            var method = (string)args[4];
            var positional = requestParams.Method == "execute_kw"
                ? (args.Length > 5 ? ToList(args[5]) : new List<object>())
                : args.Skip(5).ToList();
            var kwargs = requestParams.Method == "execute_kw" && args.Length > 6 ? args[6] as IDictionary<string, object> : null;

            var body = kwargs == null ? new Dictionary<string, object>() : new Dictionary<string, object>(kwargs);
            var call = new OdooJsonApiRequest { Url = $"{apiUrl}/json/2/{model}/{method}", Body = body };

            string[] names;
            if (ModelMethodArguments.TryGetValue(method, out names))
            {
                if (positional.Count > names.Length)
                    return new OdooJsonApiRequest { Error = PositionalArgumentsError(model, method) };
            }
            else
            {
                // record method: first positional argument are record ids
                if (positional.Count > 0)
                {
                    body["ids"] = ToIds(positional[0]);
                    positional.RemoveAt(0);
                }

                if (!RecordMethodArguments.TryGetValue(method, out names))
                    names = new string[0];
                if (positional.Count > names.Length)
                    return new OdooJsonApiRequest { Error = PositionalArgumentsError(model, method) };
            }

            for (var i = 0; i < positional.Count; i++)
                if (!body.ContainsKey(names[i]))
                    body[names[i]] = positional[i];

            // create of one record: JSON API takes only a list and returns a list of ids
            if (method == OdooOperation.Create && body.TryGetValue("vals_list", out var values) && !IsList(values))
            {
                body["vals_list"] = new[] { values };
                call.Answer = OdooJsonApiAnswer.FirstId;
            }

            return call;
        }

        /// <summary>
        /// Message for calls that need positional arguments, which JSON API does not accept.
        /// </summary>
        internal static string PositionalArgumentsError(string model, string method)
        {
            return $"JSON API does not support positional arguments of '{model}.{method}'. " +
                   $"Pass them by name with {nameof(OdooClient)}.{nameof(OdooClient.CallMethodAsync)} (kwargs), the first positional argument may be only the record ids.";
        }

        /// <summary>
        /// List of values (not a single record values dictionary or text).
        /// </summary>
        internal static bool IsList(object value)
        {
            return value is IEnumerable && !(value is string) && !(value is IDictionary);
        }

        /// <summary>
        /// Positional arguments as a list - an argument that is not a list is a single argument.
        /// </summary>
        internal static List<object> ToList(object value)
        {
            return IsList(value) ? ((IEnumerable)value).Cast<object>().ToList() : new List<object> { value };
        }

        /// <summary>
        /// Record ids as a list - a single id becomes a one element list.
        /// </summary>
        internal static object ToIds(object value)
        {
            return IsList(value) ? value : new[] { value };
        }
    }
}
