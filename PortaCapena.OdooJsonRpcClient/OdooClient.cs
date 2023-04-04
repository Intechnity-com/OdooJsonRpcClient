using System;
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
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    public sealed class OdooClient
    {
        private static HttpClient _client;
        public OdooConfig Config { get; }

        [ThreadStatic] private static int? _userUid;

        /// <summary>
        /// Can be set to false, if server certificate shall not be validated.
        /// Useful for test systems without valid SSL certificate
        /// </summary>
        public static bool ValidateServerCertificate { get; set; } = true;

        private static string basicAuthenticationUsernamePassword;
        /// <summary>
        /// Username and Password for Basic Authentication (htaccess).
        /// Syntax: username:password
        /// </summary>
        public static string BasicAuthenticationUsernamePassword
        {
            get => basicAuthenticationUsernamePassword;
            set
            {
                basicAuthenticationUsernamePassword = value;
                InitializeHttpClient();
            }
        }

        static OdooClient()
        {
            InitializeHttpClient();
        }

        private static void InitializeHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };

            handler.ServerCertificateCustomValidationCallback = ServerCertificateValidation;

            _client = new HttpClient(handler);

            if (!string.IsNullOrEmpty(BasicAuthenticationUsernamePassword))
            {
                var byteArray = Encoding.ASCII.GetBytes(BasicAuthenticationUsernamePassword);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static bool ServerCertificateValidation(HttpRequestMessage httpRequestMessage, X509Certificate2 x509Certificate2,
            X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            if (!ValidateServerCertificate)
            {
                return true;
            }

            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            return false;
        }

        public OdooClient(OdooConfig config)
        {
            Config = config;
            if (config.Timeout != default(TimeSpan))
                _client.Timeout = config.Timeout;
        }

        #region Get

        public async Task<OdooResult<T[]>> GetAsync<T>(OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => GetAsync<T>(userUid, query, SelectContext(context, Config.Context), cancellationToken));
        }
        public async Task<OdooResult<T[]>> GetAsync<T>(int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            return await GetAsync<T>(Config, userUid, query, SelectContext(context, Config.Context), cancellationToken);
        }
        public static async Task<OdooResult<T[]>> GetAsync<T>(OdooConfig odooConfig, int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var tableName = OdooExtensions.GetOdooTableName<T>();
            var request = OdooRequestModel.SearchRead(odooConfig, userUid, tableName, query, context);
            return await CallAndDeserializeAsync<T[]>(request, cancellationToken);
        }

        #endregion

        #region Count 

        public async Task<OdooResult<long>> GetCountAsync<T>(OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => GetCountAsync<T>(userUid, query, SelectContext(context, Config.Context), cancellationToken));
        }
        public async Task<OdooResult<long>> GetCountAsync<T>(int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            return await GetCountAsync<T>(Config, userUid, query, SelectContext(context, Config.Context), cancellationToken);
        }
        public static async Task<OdooResult<long>> GetCountAsync<T>(OdooConfig odooConfig, int userUid, OdooQuery query = null, OdooContext context = null, CancellationToken cancellationToken = default) where T : IOdooModel, new()
        {
            var tableName = OdooExtensions.GetOdooTableName<T>();
            var request = OdooRequestModel.SearchCount(odooConfig, userUid, tableName, query, context);
            return await CallAndDeserializeAsync<long>(request, cancellationToken);
        }

        #endregion

        #region Create

        public async Task<OdooResult<long>> CreateAsync(IOdooCreateModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => CreateAsync(Config, userUid, model, SelectContext(context, Config.Context), cancellationToken));
        }
        public static async Task<OdooResult<long>> CreateAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Create(odooConfig, userUid, model.OdooTableName(), model, context);
            var result = await CallAndDeserializeAsync<long>(request, cancellationToken);
            return result.Succeed ? result.ToResult(result.Value) : OdooResult<long>.FailedResult(result);
        }
        public async Task<OdooResult<long>> CreateAsync(OdooDictionaryModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => CreateAsync(Config, userUid, model, SelectContext(context, Config.Context), cancellationToken));
        }
        public static async Task<OdooResult<long>> CreateAsync(OdooConfig odooConfig, int userUid, OdooDictionaryModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Create(odooConfig, userUid, GetTableName(model), model, context);
            var result = await CallAndDeserializeAsync<long>(request, cancellationToken);
            return result.Succeed ? result.ToResult(result.Value) : OdooResult<long>.FailedResult(result);
        }

        #endregion

        #region  Update

        public async Task<OdooResult<bool>> UpdateAsync(IOdooCreateModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await UpdateRangeAsync(model, new[] { id }, SelectContext(context, Config.Context), cancellationToken);
        }
        public static async Task<OdooResult<bool>> UpdateAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await UpdateRangeAsync(odooConfig, userUid, model, new[] { id }, context, cancellationToken);
        }
        public async Task<OdooResult<bool>> UpdateAsync(OdooDictionaryModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await UpdateRangeAsync(model, new[] { id }, SelectContext(context, Config.Context), cancellationToken);
        }
        public static async Task<OdooResult<bool>> UpdateAsync(OdooConfig odooConfig, int userUid, OdooDictionaryModel model, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await UpdateRangeAsync(odooConfig, userUid, model, new[] { id }, context, cancellationToken);
        }

        public async Task<OdooResult<bool>> UpdateRangeAsync(IOdooCreateModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => UpdateRangeAsync(Config, userUid, model, ids, SelectContext(context, Config.Context), cancellationToken));
        }
        public static async Task<OdooResult<bool>> UpdateRangeAsync(OdooConfig odooConfig, int userUid, IOdooCreateModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var tableName = model.OdooTableName();
            var request = OdooRequestModel.Update(odooConfig, userUid, tableName, ids, model, context);
            return await CallAndDeserializeAsync<bool>(request, cancellationToken);
        }
        public async Task<OdooResult<bool>> UpdateRangeAsync(OdooDictionaryModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => UpdateRangeAsync(Config, userUid, model, ids, SelectContext(context, Config.Context), cancellationToken));
        }
        public static async Task<OdooResult<bool>> UpdateRangeAsync(OdooConfig odooConfig, int userUid, OdooDictionaryModel model, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Update(odooConfig, userUid, GetTableName(model), ids, model, context);
            return await CallAndDeserializeAsync<bool>(request, cancellationToken);
        }

        #endregion

        #region  Delete

        public async Task<OdooResult<bool>> DeleteAsync(IOdooModel model, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(model.OdooTableName(), model.Id, SelectContext(context, Config.Context), cancellationToken);
        }
        public async Task<OdooResult<bool>> DeleteAsync(string tableName, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await DeleteRangeAsync(tableName, new[] { id }, SelectContext(context, Config.Context), cancellationToken);
        }
        public static async Task<OdooResult<bool>> DeleteAsync(OdooConfig odooConfig, int userUid, string tableName, long id, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await DeleteRangeAsync(odooConfig, userUid, tableName, new[] { id }, context, cancellationToken);
        }

        public async Task<OdooResult<bool>> DeleteRangeAsync(IOdooModel[] models, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var tableName = models.First().OdooTableName();
            if (models.Any(x => !string.Equals(x.OdooTableName(), tableName)))
                throw new ArgumentException("Models are not from the same odoo table");

            return await DeleteRangeAsync(tableName, models.Select(x => x.Id).ToArray(), SelectContext(context, Config.Context), cancellationToken);
        }
        public async Task<OdooResult<bool>> DeleteRangeAsync(string tableName, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => DeleteRangeAsync(Config, userUid, tableName, ids, SelectContext(context, Config.Context), cancellationToken));
        }
        public static async Task<OdooResult<bool>> DeleteRangeAsync(OdooConfig odooConfig, int userUid, string tableName, long[] ids, OdooContext context = null, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Delete(odooConfig, userUid, tableName, ids, context);
            return await CallAndDeserializeAsync<bool>(request, cancellationToken);
        }

        #endregion

        #region Login

        public async Task<OdooResult<int>> GetCurrentUserUidOrLoginAsync(CancellationToken cancellationToken = default)
        {
            if (_userUid.HasValue)
                return await Task.FromResult(OdooResult<int>.SucceedResult(_userUid.Value));

            return await LoginAsync(cancellationToken);
        }
        public async Task<OdooResult<int>> LoginAsync(CancellationToken cancellationToken = default)
        {
            var result = await LoginAsync(Config, cancellationToken);

            if (result.Succeed)
                _userUid = result.Value;

            return result;
        }
        public static async Task<OdooResult<int>> LoginAsync(OdooConfig odooConfig, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Login(odooConfig);
            return await CallAndDeserializeAsync<int>(request, cancellationToken);
        }

        #endregion

        #region Build C# Model

        public async Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(string tableName)
        {
            return await ExecuteWitrAccesDenideRetryAsync(userUid => GetModelAsync(Config, userUid, tableName));
        }
        public static async Task<OdooResult<Dictionary<string, OdooPropertyInfo>>> GetModelAsync(OdooConfig odooConfig, int userUid, string tableName, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.ModelFields(odooConfig, userUid, tableName);
            return await CallAndDeserializeAsync<Dictionary<string, OdooPropertyInfo>>(request, cancellationToken);
        }

        #endregion

        #region Version

        public async Task<OdooResult<OdooVersion>> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            return await GetVersionAsync(Config, cancellationToken);
        }
        public static async Task<OdooResult<OdooVersion>> GetVersionAsync(OdooConfig odooConfig, CancellationToken cancellationToken = default)
        {
            var request = OdooRequestModel.Version(odooConfig);
            return await CallAndDeserializeAsync<OdooVersion>(request, cancellationToken);
        }

        #endregion

        private async Task<OdooResult<TResult>> ExecuteWitrAccesDenideRetryAsync<TResult>(Func<int, Task<OdooResult<TResult>>> func, CancellationToken cancellationToken = default)
        {
            var userUid = await GetCurrentUserUidOrLoginAsync(cancellationToken);
            if (userUid.Failed)
                return OdooResult<TResult>.FailedResult(userUid);

            var result = await func.Invoke(userUid.Value);

            if (!result.Failed || !string.Equals(result.Error?.Data?.Name, OdooExceptionName.AccessDenied))
                return result;

            var loginUid = await LoginAsync(cancellationToken);
            if (loginUid.Failed)
                return OdooResult<TResult>.FailedResult(loginUid);

            return await func.Invoke(loginUid.Value);
        }
        public static async Task<OdooResult<T>> CallAndDeserializeAsync<T>(OdooRequestModel request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await CallAsync(request, cancellationToken);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OdooResult<T>>(responseString);
                return result;
            }
            catch (Exception e)
            {
                return OdooResult<T>.FailedResult(e.ToString());
            }
        }
        public static async Task<HttpResponseMessage> CallAsync(OdooRequestModel requestModel, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(requestModel, new IsoDateTimeConverter { DateTimeFormat = OdooConsts.DateTimeFormat });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(requestModel.Params.Url, data, cancellationToken);
            return result;
        }


        private static string GetTableName(OdooDictionaryModel model, string tableName = null)
        {
            if (tableName != null)
                return tableName;
            else if (model.TableName != null)
                return model.TableName;
            else
                throw new ArgumentException(
                    $"TableName not set in {nameof(OdooDictionaryModel)}, use ctor or {nameof(OdooDictionaryModel.Create)} method with param");
        }

        private OdooContext SelectContext(OdooContext paramContext, OdooContext mainContext)
        {
            return paramContext ?? mainContext;
        }
    }
}
