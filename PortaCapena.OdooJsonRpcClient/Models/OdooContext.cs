using System.Collections.Generic;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooContext : Dictionary<string, object>
    {
        public string Language
        {
            get => TryGetValue("lang", out var result) ? result as string : default;
            set => SetValue("lang", value);
        }

        public string Timezone
        {
            get => TryGetValue("tz", out var result) ? result as string : default;
            set => SetValue("tz", value);
        }

        public string Tag
        {
            get => TryGetValue("tag", out var result) ? result as string : default;
            set => SetValue("tag", value);
        }

        public string ActiveModel
        {
            get => TryGetValue("active_model", out var result) ? result as string : default;
            set => SetValue("active_model", value);
        }

        public bool? DefaultIsCompany
        {
            get => TryGetValue("default_is_company", out var result) ? result as bool? : default;
            set => SetValue("default_is_company", value);
        }

        public long? ForceCompany
        {
            get => TryGetValue("force_company", out var result) ? result as long? : default;
            set => SetValue("force_company", value);
        }

        public long[] AllowedCompanyIds
        {
            get => TryGetValue("allowed_company_ids", out var result) ? result as long[] : default;
            set => SetValue("allowed_company_ids", value);
        }


        public OdooContext() { }

        public OdooContext(string language)
        {
            Language = language;
        }

        public OdooContext(string language, string timezone) : this(language)
        {
            Timezone = timezone;
        }

        public OdooContext(Dictionary<string, object> dict)
        {
            if (dict == null) Clear();

            foreach (var keyValuePair in dict)
                SetValue(keyValuePair.Key, keyValuePair.Value);
        }

        public OdooContext(params OdooContext[] dicts)
        {
            if (dicts == null) Clear();
           
            foreach (var dict in dicts)
                foreach (var keyValuePair in dict)
                    SetValue(keyValuePair.Key, keyValuePair.Value);
        }


        private void SetValue(string key, object value)
        {
            if (value == null && ContainsKey(key))
                Remove(key);
            else
                this[key] = value;
        }
    }
}
