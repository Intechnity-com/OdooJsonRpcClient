using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;

namespace PortaCapena.OdooJsonRpcClient.Configurations
{
    /// <summary>
    /// Add custom delegating handler to the HttpClient pipeline
    /// The order of the http message handler pipeline is important.
    /// The execution order is FIFO (First in First out)
    ///
    /// Configure the http message handlers before creating an instance of OdooClient
    /// </summary>
    public sealed class OdooClientHttp
    {
        private static ConcurrentQueue<DelegatingHandler> HttpMessageHandlers { get; } = new ConcurrentQueue<DelegatingHandler>();
        private static readonly object Lock = new object();
        private static OdooClientHttp _instance;

        private OdooClientHttp()
        {
        }
        
        private static OdooClientHttp Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                lock (Lock)
                {
                    return _instance ?? (_instance = new OdooClientHttp());
                }
            }
        }

        #region Internals

        internal static HttpMessageHandler ChainClientHandler(HttpClientHandler clientHandler)
        {
            if (!HttpMessageHandlers.Any())
            {
                return clientHandler;
            }
            var lastMessageHandler = HttpMessageHandlers.Last();
            lastMessageHandler.InnerHandler = clientHandler;
            return HttpMessageHandlers.First();
        }

        #endregion

        #region Public

        public static void Configure(Action<OdooClientHttp> configure)
        {
            configure(Instance);
        }

        public void AddHttpMessageHandler(DelegatingHandler delegatingHandler)
        {
            var lastHandler = HttpMessageHandlers.LastOrDefault();
            if (lastHandler != null)
            {
                lastHandler.InnerHandler = delegatingHandler;
            }
            HttpMessageHandlers.Enqueue(delegatingHandler);
        }
        
        public static void ClearHttpMessageHandlers()
        {
            while (!HttpMessageHandlers.IsEmpty)
            {
                HttpMessageHandlers.TryDequeue(out _);
            }
        }

        #endregion
    }
}