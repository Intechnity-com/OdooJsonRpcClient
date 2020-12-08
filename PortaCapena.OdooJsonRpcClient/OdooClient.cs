using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient
{
    public sealed class OdooClient
    {
        private readonly OdooConfig _config;

        [ThreadStatic] private static int? _userUid;

        public OdooClient(OdooConfig config)
        {
            _config = config;
        }

        #region Get

        public async Task<OdooResult<T[]>> GetAsync<T>(OdooQuery query = null) where T : IOdooModel, new()
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => GetAsync<T>(userUid, query));
        }

        public async Task<OdooResult<T[]>> GetAsync<T>(int userUid, OdooQuery query = null) where T : IOdooModel, new()
        {
            return await GetAsync<T>(_config, userUid, query);
        }

        public static async Task<OdooResult<T[]>> GetAsync<T>(OdooConfig odooConfig, int userUid, OdooQuery query = null) where T : IOdooModel, new()
        {
            var tableName = OdooExtensions.GetOdooTableName<T>();
            var request = OdooRequestModel.SearchRead(odooConfig, userUid, tableName, query);
            return await CallAndDeserializeAsync<T[]>(request);
        }

        #endregion

        #region Create

        public async Task<OdooResult<long>> CreateAsync(IOdooCreateModel model)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => CreateAsync(_config, userUid, model));
        }

        public static async Task<OdooResult<long>> CreateAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model)
        {
            var request = OdooRequestModel.Create(odooConfig, userUid, model.OdooTableName(), model);
            var result = await CallAndDeserializeAsync<long[]>(request);
            return result.Succeed ? result.ToResult(result.Value.FirstOrDefault()) : OdooResult<long>.FailedResult(result);
        }

        public async Task<OdooResult<long>> CreateAsync(OdooCreateDictionary model, string tableName = null)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => CreateAsync(_config, userUid, model, tableName));
        }

        public static async Task<OdooResult<long>> CreateAsync(OdooConfig odooConfig, int userUid, OdooCreateDictionary model, string tableName = null)
        {
            var request = OdooRequestModel.Create(odooConfig, userUid, GetTableName(model, tableName), model);
            var result = await CallAndDeserializeAsync<long[]>(request);
            return result.Succeed ? result.ToResult(result.Value.FirstOrDefault()) : OdooResult<long>.FailedResult(result);
        }

        #endregion

        #region  Update

        public async Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => UpdateAsync(_config, userUid, model, id));
        }

        public static async Task<OdooResult<bool>> UpdateAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, long id)
        {
            var tableName = model.OdooTableName();
            var request = OdooRequestModel.Update(odooConfig, userUid, tableName, new[] { id }, model);
            return await CallAndDeserializeAsync<bool>(request);
        }

        public async Task<OdooResult<bool>> UpdateAsync(OdooCreateDictionary model, long[] ids, string tableName = null)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => UpdateAsync(_config, userUid, model, ids, tableName));
        }

        public static async Task<OdooResult<bool>> UpdateAsync(OdooConfig odooConfig, int userUid, OdooCreateDictionary model, long[] ids, string tableName = null)
        {
            var request = OdooRequestModel.Update(odooConfig, userUid, GetTableName(model, tableName), ids, model);
            return await CallAndDeserializeAsync<bool>(request);
        }

        #endregion

        #region  Delete

        public async Task<OdooResult<bool>> DeleteAsync(IOdooModel model)
        {
            return await DeleteAsync(model.OdooTableName(), model.Id);
        }

        public async Task<OdooResult<bool>> DeleteAsync(string tableName, long id)
        {
            return await DeleteRangeAsync(tableName, new[] { id });
        }

        public static async Task<OdooResult<bool>> DeleteAsync(OdooConfig odooConfig, int userUid, string tableName, long id)
        {
            return await DeleteRangeAsync(odooConfig, userUid, tableName, new[] { id });
        }

        public async Task<OdooResult<bool>> DeleteRangeAsync(IOdooModel[] models)
        {
            var tableName = models.First().OdooTableName();
            if (models.Any(x => !string.Equals(x.OdooTableName(), tableName)))
                throw new ArgumentException("Models are not from the same odoo table");

            return await DeleteRangeAsync(tableName, models.Select(x => x.Id).ToArray());
        }

        public async Task<OdooResult<bool>> DeleteRangeAsync(string tableName, long[] ids)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => DeleteRangeAsync(_config, userUid, tableName, ids));
        }

        public static async Task<OdooResult<bool>> DeleteRangeAsync(OdooConfig odooConfig, int userUid, string tableName, long[] ids)
        {
            var request = OdooRequestModel.Delete(odooConfig, userUid, tableName, ids);
            return await CallAndDeserializeAsync<bool>(request);
        }

        #endregion

        #region Login

        public async Task<OdooResult<int>> GetCurrentUserUidOrLoginAsync()
        {
            if (_userUid.HasValue)
                return await Task.FromResult(OdooResult<int>.SucceedResult(_userUid.Value));

            return await LoginAsync();
        }

        public async Task<OdooResult<int>> LoginAsync()
        {
            var result = await LoginAsync(_config);

            if (result.Succeed)
                _userUid = result.Value;

            return result;
        }

        public static async Task<OdooResult<int>> LoginAsync(OdooConfig odooConfig)
        {
            var request = OdooRequestModel.Login(odooConfig);
            //TODO: if faild returns false
            return await CallAndDeserializeAsync<int>(request);
        }

        #endregion

        #region Build C# Model

        public async Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(string tableName)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => GetModelAsync(_config, userUid, tableName));
        }

        public static async Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(OdooConfig odooConfig, int userUid, string tableName)
        {
            var request = OdooRequestModel.ModelFields(odooConfig, userUid, tableName);
            return await CallAndDeserializeAsync<Dictionary<string, OdooPropertyInfo>>(request);
        }

        #endregion

        #region Version

        public async Task<OdooResult<OdooVersion>> GetVersionAsync()
        {
            return await GetVersionAsync(_config);
        }

        public static async Task<OdooResult<OdooVersion>> GetVersionAsync(OdooConfig odooConfig)
        {
            var request = OdooRequestModel.Version(odooConfig);
            return await CallAndDeserializeAsync<OdooVersion>(request);
        }

        #endregion

        private async Task<OdooResult<TResult>> ExecuteWitrAccesDenideRetryAsync<TResult>(Func<int, Task<OdooResult<TResult>>> func)
        {
            var userUid = await GetCurrentUserUidOrLoginAsync();
            if (userUid.Failed)
                return OdooResult<TResult>.FailedResult(userUid);

            var result = await func.Invoke(userUid.Value);

            if (!result.Failed || !string.Equals(result.Error?.Data?.Name, OdooExceptionName.AccessDenied))
                return result;

            var loginUid = await LoginAsync();
            if (loginUid.Failed)
                return OdooResult<TResult>.FailedResult(loginUid);

            return await func.Invoke(loginUid.Value);
        }

        public static async Task<OdooResult<T>> CallAndDeserializeAsync<T>(OdooRequestModel request)
        {
            try
            {
                var response = await CallAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OdooResult<T>>(responseString);
                return result;
            }
            catch (Exception e)
            {
                return OdooResult<T>.FailedResult(e.ToString());
            }
        }

        public static async Task<HttpResponseMessage> CallAsync(OdooRequestModel requestModel)
        {
            string json = JsonConvert.SerializeObject(requestModel, new IsoDateTimeConverter { DateTimeFormat = OdooConsts.DateTimeFormat });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(requestModel.Params.Url, data);
                return result;
            }
        }

        private static string GetTableName(OdooCreateDictionary model, string tableName)
        {
            if (tableName != null)
                return tableName;
            else if (model.TableName != null)
                return model.TableName;
            else
                throw new ArgumentException(
                    $"TableName not set, please set it in {nameof(OdooCreateDictionary)} or add in {nameof(UpdateAsync)} method parameter");
        }
    }
}
