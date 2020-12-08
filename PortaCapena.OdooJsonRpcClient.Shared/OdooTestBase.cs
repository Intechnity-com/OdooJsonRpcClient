using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Shared
{
    public abstract class OdooTestBase
    {
        protected static readonly OdooConfig Config = new OdooConfig(
            //apiUrl: "https://lipa-fenix-staging1-1757500.dev.odoo.com",
            //dbName: "lipa-fenix-staging1-1757500",
            //userName: "admin",
            //password: "admin"


        apiUrl: "https://thegreencommunity-test-staging2-1793197.dev.odoo.com",
        dbName: "thegreencommunity-test-staging2-1793197",
        userName: "admin",
        password: "99400891PC"
        );
    }
}