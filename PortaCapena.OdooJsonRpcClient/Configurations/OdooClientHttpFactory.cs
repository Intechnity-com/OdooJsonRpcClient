using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PortaCapena.OdooJsonRpcClient.Configurations
{
    /// <summary>
    /// Constructs OdooClient HttpClient
    /// </summary>
    internal static class OdooClientHttpFactory
    {
        /// <summary>
        /// Creates and configures a new HttpClient as needed when a new Odoo instance is created.
        /// </summary>
        internal static HttpClient CreateHttpClient()
        {
            return CreateHttpClient(CreateInnerHandler());
        }

        private static HttpClient CreateHttpClient(HttpMessageHandler handler) 
        {
            var client = new HttpClient(handler);
            
            if (!string.IsNullOrEmpty(OdooClient.BasicAuthenticationUsernamePassword))
            {
                var byteArray = Encoding.ASCII.GetBytes(OdooClient.BasicAuthenticationUsernamePassword);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        
        private static HttpMessageHandler CreateInnerHandler()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = ServerCertificateValidation
            };
            return OdooClientHttp.ChainClientHandler(handler);
        }

        private static bool ServerCertificateValidation(
            HttpRequestMessage httpRequestMessage,
            X509Certificate2 x509Certificate2,
            X509Chain x509Chain,
            SslPolicyErrors sslPolicyErrors
        )
        {
            if (!OdooClient.ValidateServerCertificate)
            {
                return true;
            }
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}