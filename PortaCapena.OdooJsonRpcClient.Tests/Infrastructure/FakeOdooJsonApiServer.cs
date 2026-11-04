using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models;

namespace PortaCapena.OdooJsonRpcClient.Tests.Infrastructure
{
    /// <summary>
    /// In-memory JSON API endpoint of Odoo 19+: records requests and answers them with configured handlers.
    /// </summary>
    public sealed class FakeOdooJsonApiServer : HttpMessageHandler
    {
        public const string ApiKey = "test-api-key";
        public const int UserId = 7;

        private readonly ConcurrentQueue<RecordedJsonApiRequest> _requests = new ConcurrentQueue<RecordedJsonApiRequest>();

        /// <summary>
        /// Answers model method calls (<c>/json/2/model/method</c>). Default: empty list.
        /// </summary>
        public Func<RecordedJsonApiRequest, HttpResponseMessage> OnModelCall { get; set; } = r => Value(new object[0]);

        public TimeSpan Delay { get; set; } = TimeSpan.Zero;

        public IReadOnlyList<RecordedJsonApiRequest> Requests => _requests.ToList();
        public IReadOnlyList<RecordedJsonApiRequest> ModelCalls => Requests.Where(x => x.Model != null && x.Method != "context_get").ToList();

        public OdooConfig CreateConfig(TimeSpan timeout = default)
        {
            // unique url per test - user ids are cached per url/db/user
            return new OdooConfig($"http://odoo-{Guid.NewGuid():N}.test", "db", "admin", ApiKey, timeout, protocol: OdooApiProtocol.JsonApi);
        }

        public OdooClient CreateClient(TimeSpan timeout = default)
        {
            return new OdooClient(CreateConfig(timeout), new HttpClient(this));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = request.Content == null ? null : await request.Content.ReadAsStringAsync();
            var recorded = new RecordedJsonApiRequest(request, body);
            _requests.Enqueue(recorded);

            if (Delay > TimeSpan.Zero)
                await Task.Delay(Delay, cancellationToken);

            var path = request.RequestUri.AbsolutePath;
            if (path == "/web/version")
                return Value(new { version_info = new object[] { 19, 0, 0, "final", 0, "" }, version = "19.0-20261102" });
            if (path == "/web/database/list")
                return Value(new { jsonrpc = "2.0", id = (int)recorded.Body["id"], result = new[] { "db", "other" } });
            if (path == "/json/2/res.users/context_get")
                return Value(new { lang = "en_US", tz = "Europe/Brussels", uid = UserId });
            return OnModelCall(recorded);
        }

        public static HttpResponseMessage Value(object value)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
            };
        }

        public static HttpResponseMessage Error(HttpStatusCode status, string name, string message)
        {
            return new HttpResponseMessage(status)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { name, message, arguments = new object[] { message, (int)status }, context = new { }, debug = "Traceback..." }), Encoding.UTF8, "application/json")
            };
        }
    }

    public sealed class RecordedJsonApiRequest
    {
        public RecordedJsonApiRequest(HttpRequestMessage message, string body)
        {
            Message = message;
            Body = string.IsNullOrEmpty(body) ? null : JObject.Parse(body);

            var segments = message.RequestUri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 4 && segments[0] == "json" && segments[1] == "2")
            {
                Model = segments[2];
                Method = segments[3];
            }
        }

        public HttpRequestMessage Message { get; }
        public JObject Body { get; }

        /// <summary>Model from <c>/json/2/model/method</c>, or null for other endpoints.</summary>
        public string Model { get; }

        /// <summary>Method from <c>/json/2/model/method</c>, or null for other endpoints.</summary>
        public string Method { get; }
    }
}
