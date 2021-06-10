using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class RequestTestBase
    {
        protected static readonly OdooConfig TestConfig = new OdooConfig(
            apiUrl: "http://localhost:8069", // "https://db-name.dev.odoo.com"
            dbName: "db-test-2",
            userName: "admin",
            password: "admin"
        );
    }
}