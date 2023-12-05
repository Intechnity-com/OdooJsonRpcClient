using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooRequestModel
    {
        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; }

        [JsonProperty("method")]
        public string Method { get; }

        [JsonProperty("params")]
        public OdooRequestParams Params { get; }


        public OdooRequestModel(OdooRequestParams requestParams) : this("2.0", "call", requestParams)
        {
        }
        public OdooRequestModel(string jsonrpcVersion, string method, OdooRequestParams requestParams) : this(new Random().Next(0, 1000000000), jsonrpcVersion, method, requestParams)
        {
        }
        public OdooRequestModel(int id, string jsonrpcVersion, string method, OdooRequestParams requestParams)
        {
            this.Id = id;
            this.Jsonrpc = jsonrpcVersion;
            this.Method = method;
            this.Params = requestParams;
        }

        public static OdooRequestModel Version(OdooConfig config)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "common", "version");
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel Login(OdooConfig config)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "common", "login", config.DbName, config.UserName, config.Password);
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel ModelFields(OdooConfig config, int uid, string tableName)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.FieldsGet);
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel Search(OdooConfig config, int uid, string tableName, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Search, GetRequestFilters(query), MapQuery(context, query));
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel SearchRead(OdooConfig config, int uid, string tableName, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.SearchRead, GetRequestFilters(query), MapQuery(context, query));
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel Read(OdooConfig config, int uid, string tableName, long[] ids, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Read, ids, MapQuery(context, query));
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel SearchCount(OdooConfig config, int uid, string tableName, OdooQuery query = null, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.SearchCount, GetRequestFilters(query), MapQuery(context, query));
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel Create(OdooConfig config, int uid, string tableName, object model, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Create, new[] { model }, MapQuery(context));
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel Update(OdooConfig config, int uid, string tableName, long[] ids, object model, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Update, new []{ids, model}, MapQuery(context));
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel Delete(OdooConfig config, int uid, string tableName, long[] ids, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.Delete, ids, MapQuery(context));
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel ChangeProductQuantity(OdooConfig config, int uid, long[] ids, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, "stock.change.product.qty", OdooOperation.ChangeProductQty, ids, MapQuery(context));
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel Metadata(OdooConfig config, int uid, string tableName, long[] ids, OdooContext context = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute_kw", config.DbName, uid, config.Password, tableName, OdooOperation.GetMetadata, new object[] { ids }, MapQuery(context));
            return new OdooRequestModel(param);
        }

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

        protected static object[] GetRequestFilters(OdooQuery query = null)
        {
            return query != null && query.Filters.Count > 0 ? new object[] { query.Filters.ToArray() } : new object[] { new object[] { } };
        }
    }
}

