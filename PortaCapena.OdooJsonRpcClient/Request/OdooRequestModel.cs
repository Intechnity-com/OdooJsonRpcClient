using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="OdooRequestModel"]/*'/>
    public class OdooRequestModel
    {
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Id"]/*'/>
        [JsonProperty("id")]
        public int Id { get; }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Jsonrpc"]/*'/>
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Method"]/*'/>
        [JsonProperty("method")]
        public string Method { get; }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Params"]/*'/>
        [JsonProperty("params")]
        public OdooRequestParams Params { get; }


        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="ctor"]/*'/>
        public OdooRequestModel(OdooRequestParams requestParams) : this("2.0", "call", requestParams)
        {
        }
        private static int _lastId = new Random().Next(0, 1000000);

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="ctor.Method"]/*'/>
        public OdooRequestModel(string jsonrpcVersion, string method, OdooRequestParams requestParams) : this(NextId(), jsonrpcVersion, method, requestParams)
        {
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="ctor.Id"]/*'/>
        public OdooRequestModel(int id, string jsonrpcVersion, string method, OdooRequestParams requestParams)
        {
            this.Id = id;
            this.Jsonrpc = jsonrpcVersion;
            this.Method = method;
            this.Params = requestParams;
        }

        private static int NextId()
        {
            return System.Threading.Interlocked.Increment(ref _lastId) & int.MaxValue;
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Version"]/*'/>
        public static OdooRequestModel Version(OdooConfig config)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "common", "version");
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Login"]/*'/>
        public static OdooRequestModel Login(OdooConfig config)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "common", "login", config.DbName, config.UserName, config.Password);
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="ModelFields"]/*'/>
        public static OdooRequestModel ModelFields(OdooConfig config, int uid, string tableName)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.FieldsGet);
            return new OdooRequestModel(param);
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Search"]/*'/>
        public static OdooRequestModel Search(OdooConfig config, int uid, string tableName, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Search, GetRequestFilters(query), MapQuery(context, query));
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="SearchRead"]/*'/>
        public static OdooRequestModel SearchRead(OdooConfig config, int uid, string tableName, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.SearchRead, GetRequestFilters(query), MapQuery(context, query));
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Read"]/*'/>
        public static OdooRequestModel Read(OdooConfig config, int uid, string tableName, long[] ids, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Read, new object[] { ids }, MapQuery(context, query));
            return new OdooRequestModel(param);
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="SearchCount"]/*'/>
        public static OdooRequestModel SearchCount(OdooConfig config, int uid, string tableName, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.SearchCount, GetRequestFilters(query), MapQuery(context, query));
            return new OdooRequestModel(param);
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Create"]/*'/>
        public static OdooRequestModel Create(OdooConfig config, int uid, string tableName, object model, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Create, new[] { model }, MapQuery(context));
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Update"]/*'/>
        public static OdooRequestModel Update(OdooConfig config, int uid, string tableName, long[] ids, object model, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Update, new []{ids, model}, MapQuery(context));
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Delete"]/*'/>
        public static OdooRequestModel Delete(OdooConfig config, int uid, string tableName, long[] ids, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Delete, new object[] { ids }, MapQuery(context));
            return new OdooRequestModel(param);
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Metadata"]/*'/>
        public static OdooRequestModel Metadata(OdooConfig config, int uid, string tableName, long[] ids, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.GetMetadata, new object[] { ids }, MapQuery(context));
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Action"]/*'/>
        public static OdooRequestModel Action(OdooConfig config, int uid, string tableName, string action, object model, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, action, new[] { model }, MapQuery(context));
            return new OdooRequestModel(param);
        }
        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="ActionCollection"]/*'/>
        public static OdooRequestModel ActionCollection(OdooConfig config, int uid, string tableName, string action, object model, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, action, model, MapQuery(context));
            return new OdooRequestModel(param);
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="MapQuery"]/*'/>
        protected static Dictionary<string, object> MapQuery(OdooContext context, OdooQuery query = null)
        {
            if (query == null && (context == null || !context.Any())) return null;

            var result = new Dictionary<string, object>();

            if (context != null && context.Any())
                result["context"] = context;

            if (query == null) return result;

            if (query.Limit.HasValue)
                result["limit"] = query.Limit;

            if (!string.IsNullOrWhiteSpace(query.Order))
                result["order"] = query.Order;

            if (query.Offset.HasValue)
                result["offset"] = query.Offset;

            if (query.ReturnFields.Any())
                result["fields"] = query.ReturnFields.ToArray();

            if (!result.Any()) return null;

            return result;
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="GetRequestFilters"]/*'/>
        protected static object[] GetRequestFilters(OdooQuery query = null)
        {
            return query != null && query.Filters.Count > 0 ? new object[] { query.Filters.ToArray() } : new object[] { new object[] { } };
        }

        /// <include file='../Docs/OdooRequestModel.xml' path='docs/member[@name="Call"]/*'/>
        public static OdooRequestModel Call(OdooConfig config, int uid, string tableName, string method, object[] args, Dictionary<string, object> kwargs = null, OdooContext context = null)
        {
            var allKwargs = kwargs == null ? new Dictionary<string, object>() : new Dictionary<string, object>(kwargs);
            if (context != null && context.Any())
                allKwargs["context"] = context;

            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, method, args ?? new object[0], allKwargs.Any() ? allKwargs : null);
            return new OdooRequestModel(param);
        }
    }
}

