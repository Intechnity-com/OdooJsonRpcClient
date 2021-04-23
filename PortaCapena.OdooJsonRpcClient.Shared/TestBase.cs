using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class TestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            apiUrl: "https://dbName.dev.odoo.com",
            dbName: "dbName",
            userName: "admin",
            password: "admin"
        );
    }
}