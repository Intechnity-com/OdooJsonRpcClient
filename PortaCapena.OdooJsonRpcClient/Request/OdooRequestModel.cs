using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Utils;

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

        public static OdooRequestModel Search(OdooConfig config, int uid, string tableName, OdooQuery query = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.Search, query?.GetRequestFilters(), query?.GetRequestFields(), query?.Offset, query?.Limit, query?.Order);
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel SearchRead(OdooConfig config, int uid, string tableName, OdooQuery query = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.SearchRead, query?.GetRequestFilters(), query?.GetRequestFields(), query?.Offset, query?.Limit, query?.Order);
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel SearchCount(OdooConfig config, int uid, string tableName, OdooQuery query = null)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.SearchCount, query?.GetRequestFilters());
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel Create(OdooConfig config, int uid, string tableName, object model)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.Create, new [] { model }, config.Context);
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel Update(OdooConfig config, int uid, string tableName, long[] ids, object model)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.Update,  ids, model, config.Context);
            return new OdooRequestModel(param);
        }
        public static OdooRequestModel Delete(OdooConfig config, int uid, string tableName, long[] ids)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.Delete, ids , config.Context);
            return new OdooRequestModel(param);
        }

        public static OdooRequestModel Metadata(OdooConfig config, int uid, string tableName, long[] ids)
        {
            var param = new OdooRequestParams(config.ApiUrlJson, "object", "execute", config.DbName, uid, config.Password, tableName, OdooOperation.GetMetadata, new object[] { ids }, config.Context);
            return new OdooRequestModel(param);
        }
    }
}

// https://erppeek.readthedocs.io/en/latest/api.html