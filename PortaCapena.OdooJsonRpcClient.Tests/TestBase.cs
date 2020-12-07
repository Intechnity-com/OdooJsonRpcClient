using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Tests
{
    public abstract class TestBase
    {
        protected static readonly OdooConfig _config = new OdooConfig(

            // fenix staging

            apiUrl: "https://lipa-fenix-staging1-1757500.dev.odoo.com",
            dbName: "lipa-fenix-staging1-1757500",

            // fenix uat 

            //    apiUrl: "https://lipa-fenix-lastag-1654570.dev.odoo.com",
            //    dbName: "lipa-fenix-lastag-1654570",

            userName: "admin",
            password: "admin"
        );
    }
}