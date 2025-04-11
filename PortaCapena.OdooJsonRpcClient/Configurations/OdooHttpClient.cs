using System;
using System.Collections.Generic;
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
    public sealed class OdooHttpClient
    {
        private static OdooHttpClient _instance;
        private static readonly object Lock = new object();
        private readonly object _handlersLock = new object();
        private readonly List<DelegatingHandler> _httpMessageHandlers = new List<DelegatingHandler>();

        private OdooHttpClient()
        {
        }

        private static OdooHttpClient Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (Lock)
                {
                    return _instance ?? (_instance = new OdooHttpClient());
                }
            }
        }

        #region Internals

        internal static HttpMessageHandler ChainClientHandler(HttpClientHandler clientHandler)
        {
            var instance = Instance;
            lock (instance._handlersLock)
            {
                return BuildChain(clientHandler, instance);
            }
        }

        private static HttpMessageHandler BuildChain(
            HttpClientHandler clientHandler,
            OdooHttpClient instance
        )
        {
            if (instance._httpMessageHandlers.Count == 0)
            {
                return clientHandler;
            }
            HttpMessageHandler current = clientHandler;
            for (var i = instance._httpMessageHandlers.Count - 1; i >= 0; i--)
            {
                var handler = instance._httpMessageHandlers[i];
                handler.InnerHandler = current;
                current = handler;
            }

            return current;
        }

        #endregion

        #region Public

        public static void Configure(Action<OdooHttpClient> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            configure(Instance);
        }

        public void AddHttpMessageHandler(DelegatingHandler delegatingHandler)
        {
            if (delegatingHandler == null)
            {
                throw new ArgumentNullException(nameof(delegatingHandler));
            }

            lock (_handlersLock)
            {
                _httpMessageHandlers.Add(delegatingHandler);
            }
        }

        public static void ClearHttpMessageHandlers()
        {
            var instance = Instance;
            lock (instance._handlersLock)
            {
                instance._httpMessageHandlers.Clear();
            }
        }

        #endregion
    }

}