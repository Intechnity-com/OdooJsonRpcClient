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
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models;

namespace PortaCapena.OdooJsonRpcClient.Tests.Infrastructure
{
    /// <summary>
    /// In-memory JSON-RPC endpoint: records requests and answers them with configured handlers.
    /// </summary>
    public sealed class FakeOdooServer : HttpMessageHandler
    {
        public const int UserId = 7;

        private readonly ConcurrentQueue<RecordedRequest> _requests = new ConcurrentQueue<RecordedRequest>();

        /// <summary>
        /// Answers `object.execute_kw` / `object.execute` calls. Default: empty list.
        /// </summary>
        public Func<RecordedRequest, HttpResponseMessage> OnObjectCall { get; set; } = r => Result(new object[0]);

        /// <summary>
        /// Answers `common.login`. Default: returns <see cref="UserId"/>.
        /// </summary>
        public Func<RecordedRequest, HttpResponseMessage> OnLogin { get; set; } = r => Result(UserId);

        public Func<RecordedRequest, HttpResponseMessage> OnVersion { get; set; } = r => Result(new { server_version = "15.0", server_serie = "15.0", protocol_version = 1, server_version_info = new object[] { 15, 0, 0, "final", 0, "" } });

        public TimeSpan Delay { get; set; } = TimeSpan.Zero;

        public IReadOnlyList<RecordedRequest> Requests => _requests.ToList();
        public IReadOnlyList<RecordedRequest> ObjectCalls => Requests.Where(x => x.Service == "object").ToList();
        public int LoginCount => Requests.Count(x => x.Service == "common" && x.Method == "login");

        public OdooConfig CreateConfig(TimeSpan timeout = default, OdooContext context = null)
        {
            // unique url per test - user ids are cached per url/db/user
            var url = $"http://odoo-{Guid.NewGuid():N}.test";
            return context == null
                ? new OdooConfig(url, "db", "admin", "secret", timeout)
                : new OdooConfig(url, "db", "admin", "secret", context, timeout);
        }

        public OdooClient CreateClient(TimeSpan timeout = default, OdooContext context = null)
        {
            return new OdooClient(CreateConfig(timeout, context), new HttpClient(this));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = await request.Content.ReadAsStringAsync();
            var recorded = new RecordedRequest(request, body);
            _requests.Enqueue(recorded);

            if (Delay > TimeSpan.Zero)
                await Task.Delay(Delay, cancellationToken);

            if (recorded.Service == "common" && recorded.Method == "login")
                return OnLogin(recorded);
            if (recorded.Service == "common" && recorded.Method == "version")
                return OnVersion(recorded);
            return OnObjectCall(recorded);
        }

        public static HttpResponseMessage Result(object result)
        {
            return Json(new { jsonrpc = "2.0", id = 1, result });
        }

        public static HttpResponseMessage Error(string exceptionName, string message = "error", int code = 200)
        {
            return Json(new
            {
                jsonrpc = "2.0",
                id = 1,
                error = new { code, message = "Odoo Server Error", data = new { name = exceptionName, message, debug = "Traceback..." } }
            });
        }

        public static HttpResponseMessage Json(object payload, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            };
        }

        public static HttpResponseMessage Raw(string content, HttpStatusCode statusCode, string mediaType = "text/html")
        {
            return new HttpResponseMessage(statusCode) { Content = new StringContent(content, Encoding.UTF8, mediaType) };
        }
    }

    public sealed class RecordedRequest
    {
        public RecordedRequest(HttpRequestMessage message, string body)
        {
            Message = message;
            Body = JObject.Parse(body);
        }

        public HttpRequestMessage Message { get; }
        public JObject Body { get; }

        public string Service => (string)Body["params"]?["service"];
        public string Method => (string)Body["params"]?["method"];
        public JArray Args => (JArray)Body["params"]?["args"];

        /// <summary>execute_kw: [db, uid, password, model, method, args, kwargs]</summary>
        public int Uid => Args[1].Value<int>();
        public string Model => Args[3].Value<string>();
        public string ModelMethod => Args[4].Value<string>();
        public JToken ModelArgs => Args.Count > 5 ? Args[5] : null;
        public JObject Kwargs => Args.Count > 6 ? (JObject)Args[6] : null;
    }
}
