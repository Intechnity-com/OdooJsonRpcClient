using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class TestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            apiUrl: "https://db-name.dev.odoo.com",
            dbName: "db-name",
            userName: "admin",
            password: "admin"
        );
    }
}