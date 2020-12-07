using System;

namespace PortaCapena.OdooJsonRpcClient.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OdooTableNameAttribute : System.Attribute
    {
        public string Name;
        public double Version;

        public OdooTableNameAttribute(string name)
        {
            this.Name = name;
            Version = 1.0;
        }
    }
}