using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public class TestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            apiUrl: "https://darling-uat-2324412.dev.odoo.com",
            dbName: "darling-uat-2324412",
            userName: "admin",
            password: "admin"
        );
    }
}