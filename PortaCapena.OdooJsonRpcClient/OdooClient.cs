using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    /// <include file='Docs/OdooClient.xml' path='docs/member[@name="OdooClient"]/*'/>
    public sealed class OdooClient
    {
        /// <summary>
        /// Timeout used when <see cref="OdooConfig.Timeout"/> is not set (same as the HttpClient default).
        /// </summary>
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

        private static readonly JsonConverter[] RequestConverters = { new IsoDateTimeConverter { DateTimeFormat = OdooConsts.DateTimeFormat } };

        /// <summary>
        /// Logged-in user ids, cached per server / database / user name.
        /// </summary>
        private static readonly ConcurrentDictionary<string, int> UserUidCache = new ConcurrentDictionary<string, int>();

        private static readonly HttpClient DefaultHttpClient;

        private readonly HttpClient _httpClient;

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="Config"]/*'/>
        public OdooConfig Config { get; }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ValidateServerCertificate"]/*'/>
        public static bool ValidateServerCertificate { get; set; } = true;

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="BasicAuthenticationUsernamePassword"]/*'/>
        public static string BasicAuthenticationUsernamePassword { get; set; }

        static OdooClient()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            DefaultHttpClient = CreateDefaultHttpClient();
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ctor"]/*'/>
        public OdooClient(OdooConfig config) : this(config, null)
        {
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ctor.HttpClient"]/*'/>
        public OdooClient(OdooConfig config, HttpClient httpClient)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = httpClient ?? DefaultHttpClient;
        }

        #region Get

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetAsync.Model"]/*'/>
        public async Task<OdooResult<T[]>> GetAsync<T>(OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var tableName = OdooExtensions.GetOdooTableName<T>();
            return await ExecuteAsync<T[]>(uid => OdooRequestModel.SearchRead(Config, uid, tableName, query, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetAsync.Model.UserUid"]/*'/>
        public async Task<OdooResult<T[]>> GetAsync<T>(int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var request = OdooRequestModel.SearchRead(Config, userUid, OdooExtensions.GetOdooTableName<T>(), query, SelectContext(context));
            return await SendAsync<T[]>(request, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetAsync.Model.Static"]/*'/>
        public static async Task<OdooResult<T[]>> GetAsync<T>(OdooConfig odooConfig, int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var request = OdooRequestModel.SearchRead(odooConfig, userUid, OdooExtensions.GetOdooTableName<T>(), query, context);
            return await SendAsync<T[]>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetAsync.Table"]/*'/>
        public async Task<OdooResult<OdooDictionaryModel[]>> GetAsync(string tableName, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync<OdooDictionaryModel[]>(uid => OdooRequestModel.SearchRead(Config, uid, tableName, query, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetAsync.Table.UserUid"]/*'/>
        public async Task<OdooResult<OdooDictionaryModel[]>> GetAsync(string tableName, int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.SearchRead(Config, userUid, tableName, query, SelectContext(context));
            return await SendAsync<OdooDictionaryModel[]>(request, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetAsync.Table.Static"]/*'/>
        public static async Task<OdooResult<OdooDictionaryModel[]>> GetAsync(string tableName, OdooConfig odooConfig, int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.SearchRead(odooConfig, userUid, tableName, query, context);
            return await SendAsync<OdooDictionaryModel[]>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Count

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetCountAsync"]/*'/>
        public async Task<OdooResult<long>> GetCountAsync<T>(OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var tableName = OdooExtensions.GetOdooTableName<T>();
            return await ExecuteAsync<long>(uid => OdooRequestModel.SearchCount(Config, uid, tableName, query, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetCountAsync.UserUid"]/*'/>
        public async Task<OdooResult<long>> GetCountAsync<T>(int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var request = OdooRequestModel.SearchCount(Config, userUid, OdooExtensions.GetOdooTableName<T>(), query, SelectContext(context));
            return await SendAsync<long>(request, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetCountAsync.Static"]/*'/>
        public static async Task<OdooResult<long>> GetCountAsync<T>(OdooConfig odooConfig, int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var request = OdooRequestModel.SearchCount(odooConfig, userUid, OdooExtensions.GetOdooTableName<T>(), query, context);
            return await SendAsync<long>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Create

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Model"]/*'/>
        public async Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var tableName = model.OdooTableName();
            return await ExecuteAsync<long>(uid => OdooRequestModel.Create(Config, uid, tableName, model, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Model.Static"]/*'/>
        public static async Task<OdooResult<long>> CreateAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Create(odooConfig, userUid, model.OdooTableName(), model, context);
            return await SendAsync<long>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Dictionary"]/*'/>
        public async Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var tableName = GetTableName(model);
            return await ExecuteAsync<long>(uid => OdooRequestModel.Create(Config, uid, tableName, model, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Dictionary.Static"]/*'/>
        public static async Task<OdooResult<long>> CreateAsync(OdooConfig odooConfig, int userUid, OdooDictionaryModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Create(odooConfig, userUid, GetTableName(model), model, context);
            return await SendAsync<long>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Models"]/*'/>
        public async Task<OdooResult<long[]>> CreateAsync(IEnumerable<IOdooCreateModel> models, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var list = RequireNotEmpty(models, nameof(models));
            var tableName = GetSingleTableName(list.Select(x => x.OdooTableName()));
            return await ExecuteAsync<long[]>(uid => OdooRequestModel.Create(Config, uid, tableName, list, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Models.Static"]/*'/>
        public static async Task<OdooResult<long[]>> CreateAsync(OdooConfig odooConfig, int userUid, IEnumerable<IOdooCreateModel> models, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var list = RequireNotEmpty(models, nameof(models));
            var request = OdooRequestModel.Create(odooConfig, userUid, GetSingleTableName(list.Select(x => x.OdooTableName())), list, context);
            return await SendAsync<long[]>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Dictionaries"]/*'/>
        public async Task<OdooResult<long[]>> CreateAsync(IEnumerable<OdooDictionaryModel> models, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var list = RequireNotEmpty(models, nameof(models));
            var tableName = GetSingleTableName(list.Select(x => GetTableName(x)));
            return await ExecuteAsync<long[]>(uid => OdooRequestModel.Create(Config, uid, tableName, list, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CreateAsync.Dictionaries.Static"]/*'/>
        public static async Task<OdooResult<long[]>> CreateAsync(OdooConfig odooConfig, int userUid, IEnumerable<OdooDictionaryModel> models, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var list = RequireNotEmpty(models, nameof(models));
            var request = OdooRequestModel.Create(odooConfig, userUid, GetSingleTableName(list.Select(x => GetTableName(x))), list, context);
            return await SendAsync<long[]>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Action

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ActionAsync"]/*'/>
        public async Task<OdooResult<object>> ActionAsync(string tableName, string action, object param, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync<object>(uid => OdooRequestModel.Action(Config, uid, tableName, action, param, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ActionAsync.Static"]/*'/>
        public static async Task<OdooResult<object>> ActionAsync(OdooConfig odooConfig, int userUid, string tableName, string action, object model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Action(odooConfig, userUid, tableName, action, model, context);
            return await SendAsync<object>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ActionCollectionAsync"]/*'/>
        public async Task<OdooResult<object>> ActionCollectionAsync(string tableName, string action, object param, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync<object>(uid => OdooRequestModel.ActionCollection(Config, uid, tableName, action, param, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="ActionCollectionAsync.Static"]/*'/>
        public static async Task<OdooResult<object>> ActionCollectionAsync(OdooConfig odooConfig, int userUid, string tableName, string action, object model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.ActionCollection(odooConfig, userUid, tableName, action, model, context);
            return await SendAsync<object>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Update

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateAsync.Model"]/*'/>
        public Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return UpdateRangeAsync(model, new[] { id }, context, cancellationToken);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateAsync.Model.Static"]/*'/>
        public static Task<OdooResult<bool>> UpdateAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return UpdateRangeAsync(odooConfig, userUid, model, new[] { id }, context, cancellationToken);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateAsync.Dictionary"]/*'/>
        public Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return UpdateRangeAsync(model, new[] { id }, context, cancellationToken);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateAsync.Dictionary.Static"]/*'/>
        public static Task<OdooResult<bool>> UpdateAsync(OdooConfig odooConfig, int userUid, OdooDictionaryModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return UpdateRangeAsync(odooConfig, userUid, model, new[] { id }, context, cancellationToken);
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateRangeAsync.Model"]/*'/>
        public async Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var tableName = model.OdooTableName();
            return await ExecuteAsync<bool>(uid => OdooRequestModel.Update(Config, uid, tableName, ids, model, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateRangeAsync.Model.Static"]/*'/>
        public static async Task<OdooResult<bool>> UpdateRangeAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Update(odooConfig, userUid, model.OdooTableName(), ids, model, context);
            return await SendAsync<bool>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateRangeAsync.Dictionary"]/*'/>
        public async Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var tableName = GetTableName(model);
            return await ExecuteAsync<bool>(uid => OdooRequestModel.Update(Config, uid, tableName, ids, model, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="UpdateRangeAsync.Dictionary.Static"]/*'/>
        public static async Task<OdooResult<bool>> UpdateRangeAsync(OdooConfig odooConfig, int userUid, OdooDictionaryModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Update(odooConfig, userUid, GetTableName(model), ids, model, context);
            return await SendAsync<bool>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Delete

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="DeleteAsync.Model"]/*'/>
        public async Task<OdooResult<bool>> DeleteAsync(IOdooModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return await DeleteAsync(model.OdooTableName(), model.Id, context, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="DeleteAsync.Id"]/*'/>
        public Task<OdooResult<bool>> DeleteAsync(string tableName, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return DeleteRangeAsync(tableName, new[] { id }, context, cancellationToken);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="DeleteAsync.Static"]/*'/>
        public static Task<OdooResult<bool>> DeleteAsync(OdooConfig odooConfig, int userUid, string tableName, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return DeleteRangeAsync(odooConfig, userUid, tableName, new[] { id }, context, cancellationToken);
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="DeleteRangeAsync.Models"]/*'/>
        public async Task<OdooResult<bool>> DeleteRangeAsync(IOdooModel[] models, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var list = RequireNotEmpty(models, nameof(models));
            var tableName = GetSingleTableName(list.Select(x => x.OdooTableName()));
            return await DeleteRangeAsync(tableName, list.Select(x => x.Id).ToArray(), context, cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="DeleteRangeAsync.Ids"]/*'/>
        public async Task<OdooResult<bool>> DeleteRangeAsync(string tableName, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync<bool>(uid => OdooRequestModel.Delete(Config, uid, tableName, ids, SelectContext(context)), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="DeleteRangeAsync.Static"]/*'/>
        public static async Task<OdooResult<bool>> DeleteRangeAsync(OdooConfig odooConfig, int userUid, string tableName, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Delete(odooConfig, userUid, tableName, ids, context);
            return await SendAsync<bool>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Login

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetCurrentUserUidOrLoginAsync"]/*'/>
        public async Task<OdooResult<int>> GetCurrentUserUidOrLoginAsync(CancellationToken cancellationToken = default)
        {
            if (UserUidCache.TryGetValue(GetUserCacheKey(Config), out var userUid))
                return OdooResult<int>.SucceedResult(userUid);

            return await LoginAsync(cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="LoginAsync"]/*'/>
        public async Task<OdooResult<int>> LoginAsync(CancellationToken cancellationToken = default)
        {
            var result = await LoginAsync(_httpClient, Config, cancellationToken).ConfigureAwait(false);

            var cacheKey = GetUserCacheKey(Config);
            if (result.Succeed)
                UserUidCache[cacheKey] = result.Value;
            else
                UserUidCache.TryRemove(cacheKey, out _);

            return result;
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="LoginAsync.Static"]/*'/>
        public static Task<OdooResult<int>> LoginAsync(OdooConfig odooConfig, CancellationToken cancellationToken = default)
        {
            return LoginAsync(DefaultHttpClient, odooConfig, cancellationToken);
        }

        private static async Task<OdooResult<int>> LoginAsync(HttpClient httpClient, OdooConfig odooConfig, CancellationToken cancellationToken)
        {
            var request = OdooRequestModel.Login(odooConfig);
            var result = await SendAsync<JToken>(httpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
            if (result.Failed)
                return OdooResult<int>.FailedResult(result);

            // Odoo returns `false` (not an error) for invalid credentials
            if (result.Value?.Type != JTokenType.Integer)
                return OdooResult<int>.FailedResult($"Login failed for user '{odooConfig.UserName}' to database '{odooConfig.DbName}': invalid database, user name or password.");

            return result.ToResult(result.Value.Value<int>());
        }

        #endregion

        #region Build C# Model

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetModelAsync"]/*'/>
        public Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(string tableName)
        {
            return GetModelAsync(tableName, CancellationToken.None);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetModelAsync.CancellationToken"]/*'/>
        public async Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(string tableName, CancellationToken cancellationToken)
        {
            return await ExecuteAsync<Dictionary<string, OdooPropertyInfo>>(uid => OdooRequestModel.ModelFields(Config, uid, tableName), cancellationToken).ConfigureAwait(false);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetModelAsync.Static"]/*'/>
        public static async Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(OdooConfig odooConfig, int userUid, string tableName, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.ModelFields(odooConfig, userUid, tableName);
            return await SendAsync<Dictionary<string, OdooPropertyInfo>>(DefaultHttpClient, request, odooConfig.Timeout, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Version

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetVersionAsync"]/*'/>
        public Task<OdooResult<OdooVersion>> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            return SendAsync<OdooVersion>(OdooRequestModel.Version(Config), cancellationToken);
        }
        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="GetVersionAsync.Static"]/*'/>
        public static Task<OdooResult<OdooVersion>> GetVersionAsync(OdooConfig odooConfig, CancellationToken cancellationToken = default)
        {
            return SendAsync<OdooVersion>(DefaultHttpClient, OdooRequestModel.Version(odooConfig), odooConfig.Timeout, cancellationToken);
        }

        #endregion

        #region Transport

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CallAndDeserializeAsync"]/*'/>
        public static Task<OdooResult<T>> CallAndDeserializeAsync<T>(OdooRequestModel request, CancellationToken cancellationToken = default)
        {
            return SendAsync<T>(DefaultHttpClient, request, default, cancellationToken);
        }

        /// <include file='Docs/OdooClient.xml' path='docs/member[@name="CallAsync"]/*'/>
        public static Task<HttpResponseMessage> CallAsync(OdooRequestModel requestModel, CancellationToken cancellationToken = default)
        {
            return PostAsync(DefaultHttpClient, requestModel, cancellationToken);
        }

        /// <summary>
        /// Gets (or logs in to get) the user id, executes the request and, when Odoo answers with AccessDenied
        /// (e.g. session/password changed), logs in again and retries once.
        /// </summary>
        private async Task<OdooResult<TResult>> ExecuteAsync<TResult>(Func<int, OdooRequestModel> requestFactory, CancellationToken cancellationToken)
        {
            var userUid = await GetCurrentUserUidOrLoginAsync(cancellationToken).ConfigureAwait(false);
            if (userUid.Failed)
                return OdooResult<TResult>.FailedResult(userUid);

            var result = await SendAsync<TResult>(requestFactory(userUid.Value), cancellationToken).ConfigureAwait(false);

            if (result.Succeed || !string.Equals(result.Error?.Data?.Name, OdooExceptionName.AccessDenied))
                return result;

            var loginUid = await LoginAsync(cancellationToken).ConfigureAwait(false);
            if (loginUid.Failed)
                return OdooResult<TResult>.FailedResult(loginUid);

            return await SendAsync<TResult>(requestFactory(loginUid.Value), cancellationToken).ConfigureAwait(false);
        }

        private Task<OdooResult<T>> SendAsync<T>(OdooRequestModel request, CancellationToken cancellationToken)
        {
            return SendAsync<T>(_httpClient, request, Config.Timeout, cancellationToken);
        }

        private static async Task<OdooResult<T>> SendAsync<T>(HttpClient httpClient, OdooRequestModel request, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (timeout <= TimeSpan.Zero)
                timeout = DefaultTimeout;

            try
            {
                using (var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    timeoutCts.CancelAfter(timeout);
                    try
                    {
                        using (var response = await PostAsync(httpClient, request, timeoutCts.Token).ConfigureAwait(false))
                        {
                            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            if (!response.IsSuccessStatusCode && !LooksLikeJson(responseString))
                                return OdooResult<T>.FailedResult(
                                    $"Odoo responded with HTTP {(int)response.StatusCode} ({response.ReasonPhrase}) for '{request.Params.Url}'.",
                                    (int)response.StatusCode, ((int)response.StatusCode).ToString());

                            var result = JsonConvert.DeserializeObject<OdooResult<T>>(responseString);
                            return result ?? OdooResult<T>.FailedResult($"Odoo returned an empty response for '{request.Params.Url}'.");
                        }
                    }
                    catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                    {
                        return OdooResult<T>.FailedResult($"Request to '{request.Params.Url}' timed out after {timeout}.");
                    }
                }
            }
            catch (Exception e)
            {
                return OdooResult<T>.FailedResult(e.ToString());
            }
        }

        private static async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, OdooRequestModel requestModel, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(requestModel, RequestConverters);

            using (var message = new HttpRequestMessage(HttpMethod.Post, requestModel.Params.Url))
            {
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                message.Headers.ExpectContinue = false;

                var basicAuthentication = BasicAuthenticationUsernamePassword;
                if (!string.IsNullOrEmpty(basicAuthentication))
                    message.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(basicAuthentication)));

                return await httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
            }
        }

        private static HttpClient CreateDefaultHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = ServerCertificateValidation
            };

            // timeouts are handled per request (OdooConfig.Timeout), so the shared client must not limit them
            return new HttpClient(handler) { Timeout = System.Threading.Timeout.InfiniteTimeSpan };
        }

        private static bool ServerCertificateValidation(HttpRequestMessage httpRequestMessage, X509Certificate2 x509Certificate2, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            return !ValidateServerCertificate || sslPolicyErrors == SslPolicyErrors.None;
        }

        #endregion

        #region Helpers

        private static bool LooksLikeJson(string value)
        {
            var trimmed = value?.TrimStart();
            return !string.IsNullOrEmpty(trimmed) && (trimmed[0] == '{' || trimmed[0] == '[');
        }

        private static string GetUserCacheKey(OdooConfig config)
        {
            return string.Join("\n", config.ApiUrl, config.DbName, config.UserName);
        }

        private static List<TModel> RequireNotEmpty<TModel>(IEnumerable<TModel> models, string paramName)
        {
            if (models == null) throw new ArgumentNullException(paramName);
            var list = models.ToList();
            if (list.Count == 0) throw new ArgumentException("At least one model is required.", paramName);
            return list;
        }

        private static string GetSingleTableName(IEnumerable<string> tableNames)
        {
            var distinct = tableNames.Distinct().ToList();
            if (distinct.Count != 1)
                throw new ArgumentException($"Models are not from the same odoo table: {string.Join(", ", distinct)}");
            return distinct[0];
        }

        private static string GetTableName(OdooDictionaryModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.TableName != null)
                return model.TableName;

            throw new ArgumentException(
                $"TableName not set in {nameof(OdooDictionaryModel)}, use ctor or {nameof(OdooDictionaryModel.Create)} method with param");
        }

        private OdooContext SelectContext(OdooContext paramContext)
        {
            return paramContext ?? Config.Context;
        }

        #endregion
    }
}
