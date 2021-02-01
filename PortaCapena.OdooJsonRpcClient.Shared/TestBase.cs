using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class TestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            apiUrl: "https://lipa-fenix-staging2-1884271.dev.odoo.com",
            dbName: "lipa-fenix-staging2-1884271",
            userName: "admin",
            password: "admin"
        );
    }
}