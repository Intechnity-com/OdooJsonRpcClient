using System.Collections.Generic;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooContext : Dictionary<string, object>
    {
        public string Language
        {
            get => TryGetValue("lang", out var result) ? result as string : default;
            set => this["lang"] = value;
        }

        public string Timezone
        {
            get => TryGetValue("tz", out var result) ? result as string : default;
            set => this["tz"] = value;
        }

        public string Tag
        {
            get => TryGetValue("tag", out var result) ? result as string : default;
            set => this["tag"] = value;
        }

        public string ActiveModel
        {
            get => TryGetValue("active_model", out var result) ? result as string : default;
            set => this["active_model"] = value;
        }

        public bool? DefaultIsCompany
        {
            get => TryGetValue("default_is_company", out var result) ? result as bool? : default;
            set => this["default_is_company"] = value;
        }

        public long? ForceCompany
        {
            get => TryGetValue("force_company", out var result) ? result as long? : default;
            set => this["force_company"] = value;
        }

        //public string ActiveIds
        //{
        //    get => TryGetValue("active_ids", out var result) ? result as string : default;
        //    set => this["active_ids"] = value;
        //}

        public static OdooContext Create() => new OdooContext();

        public OdooContext() { }

        internal OdooContext(Dictionary<string, object> dict)
        {
            foreach (var keyValuePair in dict)
                this[keyValuePair.Key] = keyValuePair.Value;
        }

        internal OdooContext(params OdooContext[] dicts)
        {
            foreach (var dict in dicts)
                foreach (var keyValuePair in dict)
                    this[keyValuePair.Key] = keyValuePair.Value;
        }
    }
}




// https://stackoverflow.com/questions/46586281/alter-odoo-xmlrpc-context-to-use-a-specific-language
// https://gist.github.com/ilyasProgrammer/cf6647356c9a3722f597f72b7685a4c3