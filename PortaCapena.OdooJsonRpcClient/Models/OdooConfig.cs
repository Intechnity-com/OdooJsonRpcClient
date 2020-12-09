namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooConfig
    {
        public string ApiUrl { get; }
        public string ApiUrlJson => this.ApiUrl + "/jsonrpc";
        public string DbName { get; }
        public string UserName { get; }
        public string Password { get; }

        public OdooClientContext Context { get; set; }

        public OdooConfig(string apiUrl, string dbName, string userName, string password)
        {
            this.ApiUrl = apiUrl;
            this.DbName = dbName;
            this.UserName = userName;
            this.Password = password;
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, string timezone) : this( apiUrl,  dbName,  userName,  password)
        {
            this.Context = new OdooClientContext(language, timezone);
        }

    }
}