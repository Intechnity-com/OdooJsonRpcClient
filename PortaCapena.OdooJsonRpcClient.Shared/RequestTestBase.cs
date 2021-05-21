using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class RequestTestBase
    {
        protected static readonly OdooConfig TestConfig = new OdooConfig(
            apiUrl: "https://db-name.dev.odoo.com",
            dbName: "db-name",
            userName: "admin",
            password: "admin"
        );
    }
}