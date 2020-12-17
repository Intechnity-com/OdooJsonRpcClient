using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class TestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            apiUrl: "https://odoo-api-url.com",
            dbName: "odoo-db-name",
            userName: "admin",
            password: "admin"
        );
    }
}