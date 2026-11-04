using System;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="OdooConfig"]/*'/>
    public class OdooConfig
    {
        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="ApiUrl"]/*'/>
        public string ApiUrl { get; }
        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="ApiUrlJson"]/*'/>
        public string ApiUrlJson => this.ApiUrl + "/jsonrpc";
        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="DbName"]/*'/>
        public string DbName { get; }
        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="UserName"]/*'/>
        public string UserName { get; }
        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="Password"]/*'/>
        public string Password { get; }
        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="Timeout"]/*'/>
        public TimeSpan Timeout { get; }

        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="Context"]/*'/>
        public OdooContext Context { get; }


        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="ctor"]/*'/>
        public OdooConfig(string apiUrl, string dbName, string userName, string password, TimeSpan timeout = default(TimeSpan))
        {
            if (string.IsNullOrWhiteSpace(apiUrl)) throw new ArgumentException("Odoo url is required", nameof(apiUrl));
            this.ApiUrl = apiUrl.TrimEnd(new[] { '/' });
            this.DbName = dbName;
            this.UserName = userName;
            this.Password = password;
            this.Context = new OdooContext();
            this.Timeout = timeout;
        }

        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="ctor.Context"]/*'/>
        public OdooConfig(string apiUrl, string dbName, string userName, string password, OdooContext context, TimeSpan timeout = default(TimeSpan)) : this(apiUrl, dbName, userName, password, timeout)
        {
            this.Context = new OdooContext(context);
        }

        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="ctor.Language"]/*'/>
        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, TimeSpan timeout = default(TimeSpan)) : this(apiUrl, dbName, userName, password, timeout)
        {
            this.Context = new OdooContext(language);
        }

        /// <include file='../Docs/OdooConfig.xml' path='docs/member[@name="ctor.LanguageTimezone"]/*'/>
        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, string timezone, TimeSpan timeout = default(TimeSpan)) : this(apiUrl, dbName, userName, password, timeout)
        {
            this.Context = new OdooContext(language, timezone);
        }
    }
}
