using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient
{
    /// <summary>
    /// Sends requests with the JSON API of Odoo 19+ (used when <see cref="OdooConfig.Protocol"/> is <see cref="OdooApiProtocol.JsonApi"/>)
    /// and converts answers and errors to the same <see cref="OdooResult{TResult}"/> as the JSON-RPC transport.
    /// </summary>
    internal static class OdooJsonApiRequestSender
    {
        internal static readonly JsonSerializerSettings RequestSettings = new JsonSerializerSettings
        {
            Converters = { new IsoDateTimeConverter { DateTimeFormat = OdooConsts.DateTimeFormat } }
        };

        /// <summary>
        /// Translates the request to JSON API and sends it. Errors are returned as failed results, not thrown.
        /// </summary>
        internal static async Task<OdooResult<T>> SendAsync<T>(HttpClient httpClient, OdooRequestModel request, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var call = OdooJsonApiRequestMapper.Map(request);
            if (call.Error != null)
                return OdooResult<T>.FailedResult(call.Error);

            try
            {
                using (var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    timeoutCts.CancelAfter(timeout);
                    try
                    {
                        using (var message = CreateHttpRequest(call, request.Config))
                        using (var response = await httpClient.SendAsync(message, timeoutCts.Token).ConfigureAwait(false))
                        {
                            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            if (!response.IsSuccessStatusCode)
                                return ToFailedResult<T>(response, body, call.Url);

                            if (call.JsonRpcEnvelope)
                                return JsonConvert.DeserializeObject<OdooResult<T>>(body) ?? OdooResult<T>.FailedResult($"Odoo returned an empty response for '{call.Url}'.");

                            var value = ToAnswer(call.Answer, JToken.Parse(body));
                            return OdooResult<T>.SucceedResult(value.Type == JTokenType.Null ? default : value.ToObject<T>());
                        }
                    }
                    catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                    {
                        return OdooResult<T>.FailedResult($"Request to '{call.Url}' timed out after {timeout}.");
                    }
                }
            }
            catch (Exception e)
            {
                return OdooResult<T>.FailedResult(e.ToString());
            }
        }

        /// <summary>
        /// HTTP request with the API key as bearer token and the database in <c>X-Odoo-Database</c>.
        /// </summary>
        internal static HttpRequestMessage CreateHttpRequest(OdooJsonApiRequest call, OdooConfig config)
        {
            var message = new HttpRequestMessage(call.HttpMethod, call.Url);
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            message.Headers.UserAgent.ParseAdd("PortaCapena.OdooJsonRpcClient");
            message.Headers.ExpectContinue = false;

            if (!string.IsNullOrEmpty(config.Password))
                message.Headers.Authorization = new AuthenticationHeaderValue("bearer", config.Password);
            if (!string.IsNullOrEmpty(config.DbName))
                message.Headers.Add("X-Odoo-Database", config.DbName);

            if (call.Body != null)
                message.Content = new StringContent(JsonConvert.SerializeObject(call.Body, RequestSettings), Encoding.UTF8, "application/json");

            return message;
        }

        /// <summary>
        /// Converts JSON API answer to the value of the corresponding JSON-RPC call.
        /// </summary>
        internal static JToken ToAnswer(OdooJsonApiAnswer answer, JToken value)
        {
            switch (answer)
            {
                case OdooJsonApiAnswer.UserId:
                    return value["uid"] ?? JValue.CreateNull();

                case OdooJsonApiAnswer.Version:
                    var info = value["version_info"] as JArray;
                    return new JObject
                    {
                        ["server_version"] = value["version"],
                        ["server_version_info"] = info,
                        ["server_serie"] = info != null && info.Count > 1 ? $"{info[0]}.{info[1]}" : null,
                        ["protocol_version"] = 1
                    };

                case OdooJsonApiAnswer.FirstId:
                    return value is JArray ids && ids.Count > 0 ? ids[0] : value;

                default:
                    return value;
            }
        }

        /// <summary>
        /// JSON API errors come as HTTP 4xx / 5xx with <c>{name, message, arguments, context, debug}</c> - the same data as JSON-RPC <c>error.data</c>.
        /// </summary>
        internal static OdooResult<T> ToFailedResult<T>(HttpResponseMessage response, string body, string url)
        {
            var status = (int)response.StatusCode;
            OdooException exception = null;
            try
            {
                exception = JsonConvert.DeserializeObject<OdooException>(body);
            }
            catch (JsonException)
            {
                // not a JSON error (e.g. HTML error page of a proxy)
            }

            var message = exception?.Message ?? $"Odoo responded with HTTP {status} ({response.ReasonPhrase}) for '{url}'.";
            return new OdooResult<T> { Error = new OdooError(message, status, status.ToString()) { Data = exception } };
        }
    }
}
