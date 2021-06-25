using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using ChargeBee.Api;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Internal
{
    public class Resource : JsonSupport
    {
        public Resource()
        {
        }

        internal Resource(string json)
        {
            if (!string.IsNullOrEmpty(json))
                MJobj = JToken.Parse(json);
        }

        internal Resource(JToken jobj)
        {
            MJobj = jobj;
        }

        public T GetValue<T>(string key, bool required = true)
        {
            if (required)
                ThrowIfKeyMissed(key);

            if (MJobj[key] == null) return default;

            return MJobj[key].ToObject<T>();
        }

        public DateTime? GetDateTime(string key, bool required = true)
        {
            var ts = GetValue<long?>(key, required);
            if (ts == null) return null;
            return ApiUtil.ConvertFromTimestamp((long) ts);
        }

        public JToken GetJToken(string key, bool required = true)
        {
            if (required)
                ThrowIfKeyMissed(key);

            if (MJobj[key] == null)
                return null;

            return JToken.Parse(MJobj[key].ToString());
        }

        public JArray GetJArray(string key, bool required = true)
        {
            if (required)
                ThrowIfKeyMissed(key);

            if (MJobj[key] == null)
                return null;

            return JArray.Parse(MJobj[key].ToString());
        }

        public T GetEnum<T>(string key, bool required = true)
        {
            var value = GetValue<string>(key, required);
            if (string.IsNullOrEmpty(value)) return default;

            var eType = typeof(T);

            // Handle nullable enum
            if (eType.IsConstructedGenericType)
                eType = eType.GenericTypeArguments[0];

            foreach (var fi in eType.GetTypeInfo().DeclaredFields)
            {
                var attrs =
                    (EnumMemberAttribute[]) fi.GetCustomAttributes(typeof(EnumMemberAttribute), false);

                if (attrs.Length == 0)
                    continue;

                if (value == attrs[0].Value)
                    return (T) fi.GetValue(null);
            }

            return default;
        }

        protected void ThrowIfKeyMissed(string key)
        {
            if (MJobj[key] == null)
                throw new ArgumentException($"The property {key} is not present!");
        }

        protected static string CheckNull(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID can't be null or emtpy!");

            return id;
        }

        protected List<T> GetResourceList<T>(string property) where T : Resource, new()
        {
            if (MJobj == null)
                return null;

            var jobj = MJobj[property];
            if (jobj == null)
                return null;

            var list = new List<T>();
            foreach (var item in jobj.Children())
            {
                var t = new T();
                t.JObj = item;
                list.Add(t);
            }

            return list;
        }


        protected List<T> GetList<T>(string property)
        {
            if (MJobj == null)
                return null;

            var jobj = MJobj[property];
            if (jobj == null)
                return null;

            var list = new List<T>();
            foreach (var item in jobj.Children()) list.Add(item.ToObject<T>());

            return list;
        }

        protected T GetSubResource<T>(string property) where T : Resource, new()
        {
            if (MJobj == null)
                return null;

            var jobj = MJobj[property];
            if (jobj == null)
                return null;
            var t = new T();
            t.JObj = jobj;
            return t;
        }

        protected static void ApiVersionCheck(JToken jObj)
        {
            if (jObj["api_version"] == null) return;
            var apiVersion = jObj["api_version"].ToString().ToUpper();
            if (!apiVersion.Equals(ApiConfig.ApiVersion, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("API version [" + apiVersion + "] in response does not match "
                                            + "with client library API version [" + ApiConfig.ApiVersion.ToUpper() +
                                            "]");
        }

        public JToken GetJToken()
        {
            return MJobj;
        }
    }
}