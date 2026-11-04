using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class RequestTestBase
    {
        protected static readonly OdooConfig TestConfig = new OdooConfig(
            apiUrl: "http://localhost:8069", // "https://db-name.dev.odoo.com"
            dbName: "db-test",
            userName: "admin",
            password: "admin"
        );

        /// <summary>
        /// Odoo 19 from docker/odoo19 with the JSON API; password is the fixed test API key from docker/odoo19/seed.py.
        /// </summary>
        protected static readonly OdooConfig JsonApiTestConfig = new OdooConfig(
            apiUrl: "http://localhost:8070",
            dbName: "db-test-19",
            userName: "admin",
            password: "a1b2c3d4e5f60718293a4b5c6d7e8f9001234567"
        )
        {
            Protocol = Consts.OdooApiProtocol.JsonApi
        };
    }
}