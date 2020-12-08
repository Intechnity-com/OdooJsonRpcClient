using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public abstract class OdooTestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
        apiUrl: "https://lipa-fenix-staging1-17575000.dev.odoo.com",
        dbName: "lipa-fenix-staging1-17575000",

        userName: "admin",
        password: "admin"
        );
    }
}