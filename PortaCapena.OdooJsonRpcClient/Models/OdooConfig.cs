namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooConfig
    {
        public string ApiUrl { get; }
        public string ApiUrlJson => this.ApiUrl + "/jsonrpc";
        public string DbName { get; }
        public string UserName { get; }
        public string Password { get; }


        public OdooConfig(string apiUrl, string dbName, string userName, string password)
        {
            this.ApiUrl = apiUrl;
            this.DbName = dbName;
            this.UserName = userName;
            this.Password = password;
        }
    }
}