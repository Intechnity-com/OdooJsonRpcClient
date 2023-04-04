﻿using System;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooConfig
    {
        public string ApiUrl { get; }
        public string ApiUrlJson => this.ApiUrl + "/jsonrpc";
        public string DbName { get; }
        public string UserName { get; }
        public string Password { get; }
        public TimeSpan Timeout { get; }

        public OdooContext Context { get; }


        public OdooConfig(string apiUrl, string dbName, string userName, string password, TimeSpan timeout = default(TimeSpan))
        {
            this.ApiUrl = apiUrl.TrimEnd(new[] { '/' });
            this.DbName = dbName;
            this.UserName = userName;
            this.Password = password;
            this.Context = new OdooContext();
            this.Timeout = timeout;
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, OdooContext context, TimeSpan timeout = default(TimeSpan)) : this(apiUrl, dbName, userName, password, timeout)
        {
            this.Context = new OdooContext(context);
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, TimeSpan timeout = default(TimeSpan)) : this(apiUrl, dbName, userName, password, timeout)
        {
            this.Context = new OdooContext(language);
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, string timezone, TimeSpan timeout = default(TimeSpan)) : this(apiUrl, dbName, userName, password, timeout)
        {
            this.Context = new OdooContext(language, timezone);
        }
    }
}