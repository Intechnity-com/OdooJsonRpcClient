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


        public OdooContext() { }

        public OdooContext(Dictionary<string, object> dict)
        {
            foreach (var keyValuePair in dict)
                this[keyValuePair.Key] = keyValuePair.Value;
        }

        public OdooContext(params OdooContext[] dicts)
        {
            foreach (var dict in dicts)
                foreach (var keyValuePair in dict)
                    this[keyValuePair.Key] = keyValuePair.Value;
        }
    }
}
