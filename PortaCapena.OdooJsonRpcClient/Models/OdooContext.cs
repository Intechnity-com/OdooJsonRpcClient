using System.Collections.Generic;
using System.Linq;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="OdooContext"]/*'/>
    public class OdooContext : Dictionary<string, object>
    {
        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="Language"]/*'/>
        public string Language
        {
            get => TryGetValue("lang", out var result) ? result as string : default;
            set => SetValue("lang", value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="Timezone"]/*'/>
        public string Timezone
        {
            get => TryGetValue("tz", out var result) ? result as string : default;
            set => SetValue("tz", value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="Tag"]/*'/>
        public string Tag
        {
            get => TryGetValue("tag", out var result) ? result as string : default;
            set => SetValue("tag", value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ActiveModel"]/*'/>
        public string ActiveModel
        {
            get => TryGetValue("active_model", out var result) ? result as string : default;
            set => SetValue("active_model", value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="DefaultIsCompany"]/*'/>
        public bool? DefaultIsCompany
        {
            get => TryGetValue("default_is_company", out var result) ? result as bool? : default;
            set => SetValue("default_is_company", value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ForceCompany"]/*'/>
        public long? ForceCompany
        {
            get => TryGetValue("force_company", out var result) ? ToNullableLong(result) : default;
            set => SetValue("force_company", value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="AllowedCompanyId"]/*'/>
        public long? AllowedCompanyId
        {
            get => TryGetValue("allowed_company_ids", out var result) ? ToNullableLong((result as System.Collections.IEnumerable)?.Cast<object>().FirstOrDefault()) : default;
            set => SetValue("allowed_company_ids", value.HasValue ? new long[] { value.Value } : default);
        }


        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ctor"]/*'/>
        public OdooContext() { }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ctor.Language"]/*'/>
        public OdooContext(string language)
        {
            Language = language;
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ctor.LanguageTimezone"]/*'/>
        public OdooContext(string language, string timezone) : this(language)
        {
            Timezone = timezone;
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ctor.Dictionary"]/*'/>
        public OdooContext(Dictionary<string, object> dict)
        {
            if (dict == null) return;

            foreach (var keyValuePair in dict)
                SetValue(keyValuePair.Key, keyValuePair.Value);
        }

        /// <include file='../Docs/OdooContext.xml' path='docs/member[@name="ctor.Merge"]/*'/>
        public OdooContext(params OdooContext[] dicts)
        {
            if (dicts == null) return;

            foreach (var dict in dicts.Where(x => x != null))
                foreach (var keyValuePair in dict)
                    SetValue(keyValuePair.Key, keyValuePair.Value);
        }

        private static long? ToNullableLong(object value)
        {
            switch (value)
            {
                case null:
                    return null;
                case long l:
                    return l;
                case int i:
                    return i;
                default:
                    try { return System.Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture); }
                    catch (System.Exception) { return null; }
            }
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
