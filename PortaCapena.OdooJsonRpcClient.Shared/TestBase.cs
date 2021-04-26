using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class TestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            apiUrl: "https://lipa-fenix-staging5-2434654.dev.odoo.com",
            dbName: "lipa-fenix-staging5-2434654",
            userName: "admin",
            password: "admin"
        );
    }
}